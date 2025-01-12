using System;
using System.IO;
using System.Threading;
using Common;
using EasyStorage;
using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Structure;
using FezGame.Tools;
using Microsoft.Xna.Framework;
// using Steamworks;

namespace FezGame.Services
{
	public class GameStateManager : EngineStateManager, IGameStateManager, IEngineStateManager
	{
		public const string StorageContainerName = "FEZ";

		public const string SaveFileName = "SaveSlot";

		private Action OnLoadingComplete;

		private bool scheduleLoadEnd;

		private bool shouldSaveToCloud;

		private readonly SaveData tempSaveData = new SaveData();

		public Action OnStorageSelected { get; set; }

		public bool SkipLoadScreen { get; set; }

		public bool LoadingVisible { get; set; }

		public bool ForceLoadIcon { get; set; }

		public float SinceSaveRequest { get; set; }

		public bool MenuCubeIsZoomed { get; set; }

		public bool InEndCutscene { get; set; }

		public bool DisallowRotation { get; set; }

		public bool ForceTimePaused { get; set; }

		public bool ForcedSignOut { get; set; }

		public string LoggedOutPlayerTag { get; set; }

		public bool EndGame { get; set; }

		public bool IsAchievementSave { get; set; }

		public int SaveSlot { get; set; }

		public override bool TimePaused => base.Paused || InMenuCube || InMap || ForceTimePaused || InCutscene || base.InFpsMode;

		public override float WaterLevelOffset => SaveData.GlobalWaterLevelModifier.GetValueOrDefault();

		public new bool InMap
		{
			get
			{
				return inMap;
			}
			set
			{
				inMap = value;
			}
		}

		public new bool InMenuCube
		{
			get
			{
				return inMenuCube;
			}
			set
			{
				inMenuCube = value;
			}
		}

		public override bool Loading
		{
			get
			{
				return base.Loading;
			}
			set
			{
				if (!base.Loading && value)
				{
					SpeedRun.PauseForLoading();
				}
				else if (base.Loading && !value)
				{
					SpeedRun.ResumeAfterLoading();
				}
				base.Loading = value;
			}
		}

		public bool ScheduleLoadEnd
		{
			get
			{
				return scheduleLoadEnd;
			}
			set
			{
				if (base.Loading && value)
				{
					SpeedRun.ResumeAfterLoading();
				}
				scheduleLoadEnd = value;
			}
		}

		public ISaveDevice ActiveSaveDevice { get; set; }

		public TextScroll ActiveScroll { get; set; }

		public bool HasActivePlayer => InputManager.ActiveControllers != ControllerIndex.Any && InputManager.ActiveControllers != ControllerIndex.None;

		public PlayerIndex ActivePlayer => InputManager.ActiveControllers.GetPlayer();

		public SteamUser ActiveGamer => null; // Fez.NoSteamworks ? null : SteamUser.Default;

		public SaveData SaveData { get; set; }

		public bool Saving { get; set; }

		public bool ShowDebuggingBag { get; set; }

		public bool JetpackMode { get; set; }

		public bool DebugMode { get; set; }

		public bool SkipFadeOut { get; set; }

		public bool InCutscene { get; set; }

		public bool SkipLoadBackground { get; set; }

		public bool HideHUD { get; set; }

		public bool IsTrialMode => true;

		[ServiceDependency]
		public IThreadPool ThreadPool { private get; set; }

		[ServiceDependency]
		public IGameLevelManager LevelManager { private get; set; }

		[ServiceDependency]
		public IPlayerManager PlayerManager { private get; set; }

		[ServiceDependency]
		public IInputManager InputManager { private get; set; }

		[ServiceDependency]
		public ITimeManager TimeManager { private get; set; }

		[ServiceDependency]
		public IGameService TrialAndAchievements { private get; set; }

		[ServiceDependency]
		public IDefaultCameraManager CameraManager { private get; set; }

		[ServiceDependency]
		public ISoundManager SoundManager { private get; set; }

		public event Action ActiveGamerSignedOut = Util.NullAction;

		public event Action LiveConnectionChanged = Util.NullAction;

		public event Action DynamicUpgrade = Util.NullAction;

		public event Action HudElementChanged = Util.NullAction;

		public GameStateManager()
		{
			SinceSaveRequest = -1f;
			SaveSlot = -1;
		}

		public void ShowScroll(string actualString, float forSeconds, bool onTop)
		{
			if (ActiveScroll != null)
			{
				TextScroll textScroll = ActiveScroll;
				while (textScroll.NextScroll != null)
				{
					textScroll = textScroll.NextScroll;
				}
				if (!(textScroll.Key == actualString) || textScroll.Closing)
				{
					textScroll.Closing = true;
					textScroll.Timeout = null;
					TextScroll textScroll2 = new TextScroll(ServiceHelper.Game, actualString, onTop)
					{
						Key = actualString
					};
					if (forSeconds > 0f)
					{
						textScroll2.Timeout = forSeconds;
					}
					ActiveScroll.NextScroll = textScroll2;
				}
			}
			else
			{
				TextScroll textScroll3 = new TextScroll(ServiceHelper.Game, actualString, onTop)
				{
					Key = actualString
				};
				TextScroll component = (ActiveScroll = textScroll3);
				ServiceHelper.AddComponent(component);
				if (forSeconds > 0f)
				{
					ActiveScroll.Timeout = forSeconds;
				}
			}
		}

		public void OnHudElementChanged()
		{
			this.HudElementChanged();
		}

		public void AwardAchievement(SteamAchievement achievement)
		{
			IsAchievementSave = true;
			// if (!Fez.NoSteamworks)
			// {
			// 	SteamUserStats.SetAchievement(achievement.AchievementName);
			// 	SteamUserStats.StoreStats();
			// }
		}

		private void WriteLeaderboardEntry()
		{
			// if (SaveData.ScoreDirty && !Fez.NoSteamworks)
			// {
			// 	CallResult<LeaderboardFindResult_t> callResult = new CallResult<LeaderboardFindResult_t>(OnLeaderboardFound);
			// 	SteamAPICall_t hAPICall = SteamUserStats.FindLeaderboard("CompletionPercentage");
			// 	callResult.Set(hAPICall);
			// }
		}

		// private void OnLeaderboardFound(LeaderboardFindResult_t result, bool bIOFailure)
		// {
		// 	if (bIOFailure || result.m_bLeaderboardFound == 0)
		// 	{
		// 		Logger.Log("Leaderboard", LogSeverity.Error, "Could not find leaderboard!");
		// 		return;
		// 	}
		// 	int nScore = Math.Min(SaveData.CubeShards, 32) + Math.Min(SaveData.SecretCubes, 32) + Math.Min(SaveData.PiecesOfHeart, 3);
		// 	SteamUserStats.UploadLeaderboardScore(result.m_hSteamLeaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, nScore, null, 0);
		// 	SaveData.ScoreDirty = false;
		// }

		public void SignInAndChooseStorage(Action onFinish)
		{
			OnStorageSelected = onFinish;
			ActiveSaveDevice = new PCSaveDevice("FEZ");
			OnStorageSelected();
			OnStorageSelected = null;
		}

		public void LoadSaveFile(Action onFinish)
		{
			if (IsTrialMode || Fez.PublicDemo || !ActiveSaveDevice.FileExists("SaveSlot" + SaveSlot))
			{
				SaveData = new SaveData
				{
					Level = (IsTrialMode ? "trial/BIG_TOWER" : Fez.ForcedLevelName),
					IsNew = true
				};
			}
			else if (!ActiveSaveDevice.Load("SaveSlot" + SaveSlot, LoadSaveFile))
			{
				SaveData = new SaveData
				{
					Level = (IsTrialMode ? "trial/BIG_TOWER" : Fez.ForcedLevelName),
					IsNew = true
				};
			}
			if (Fez.ForceGoldenCubes != 0 || Fez.ForceAntiCubes != 0)
			{
				if (Fez.ForceGoldenCubes != 0)
				{
					SaveData.CubeShards = Fez.ForceGoldenCubes;
				}
				if (Fez.ForceAntiCubes != 0)
				{
					SaveData.SecretCubes = Fez.ForceAntiCubes;
				}
			}
			onFinish();
		}

		private void LoadSaveFile(BinaryReader reader)
		{
			SaveData = SaveFileOperations.Read(new CrcReader(reader));
		}

		public void LoadLevelAsync(Action onFinish)
		{
			if (IsTrialMode)
			{
				throw new InvalidOperationException("Save files should not be used in trial mode");
			}
			if (Saving)
			{
				throw new InvalidOperationException("Can't save and load at the same time...?");
			}
			OnLoadingComplete = onFinish;
			Loading = true;
			Worker<bool> worker = ThreadPool.Take<bool>(DoLoadLevel);
			worker.Finished += delegate
			{
				ThreadPool.Return(worker);
			};
			worker.Start(context: false);
		}

		public void LoadLevel()
		{
			DoLoadLevel(dummy: false);
		}

		private void DoLoadLevel(bool dummy)
		{
			TimeManager.CurrentTime = DateTime.Today.Add(SaveData.TimeOfDay);
			LevelManager.ChangeLevel(SaveData.Level ?? (IsTrialMode ? "trial/BIG_TOWER" : Fez.ForcedLevelName));
			TimeManager.OnTick();
			if (OnLoadingComplete != null)
			{
				OnLoadingComplete();
			}
			OnLoadingComplete = null;
			Loading = false;
		}

		public void ReturnToArcade()
		{
			ScreenFade obj = new ScreenFade(ServiceHelper.Game)
			{
				FromColor = ColorEx.TransparentBlack,
				ToColor = Color.Black,
				Duration = 0.75f,
				EaseOut = true,
				DrawOrder = 3000
			};
			ScreenFade screenFade = obj;
			ServiceHelper.AddComponent(obj);
			screenFade.Faded = (Action)Delegate.Combine(screenFade.Faded, new Action(ServiceHelper.Game.Exit));
		}

		public void SaveToCloud(bool force = false)
		{
			if (!IsTrialMode && !Fez.PublicDemo && ActiveSaveDevice != null && (force || shouldSaveToCloud))
			{
				Worker<bool> worker = ThreadPool.Take<bool>(DoSaveToCloud);
				worker.Finished += delegate
				{
					ThreadPool.Return(worker);
				};
				worker.Start(context: false);
			}
		}

		private void DoSaveToCloud(bool dummy)
		{
			Logger.Log("GameState", LogSeverity.Warning, "Saving to cloud is not supported in WASM");
			// if (Fez.NoSteamworks)
			// {
			// 	return;
			// }
			// string path = Path.Combine((ActiveSaveDevice as PCSaveDevice).RootDirectory, "SaveSlot" + SaveSlot);
			// try
			// {
			// 	byte[] array = File.ReadAllBytes(path);
			// 	SteamRemoteStorage.FileWrite("SaveSlot" + SaveSlot, array, array.Length);
			// 	shouldSaveToCloud = false;
			// }
			// catch (IOException)
			// {
			// 	Logger.Log("GameState", LogSeverity.Information, "Failed to save to Steam Cloud, will try again later");
			// 	shouldSaveToCloud = true;
			// }
		}

		public void Save()
		{
			if (!IsTrialMode && ActiveSaveDevice != null)
			{
				SinceSaveRequest = 0f;
				SaveData.CloneInto(tempSaveData);
			}
		}

		public void SaveImmediately(bool ngpBackup = false)
		{
			if (!IsTrialMode && !Fez.PublicDemo && ActiveSaveDevice != null)
			{
				shouldSaveToCloud = true;
				SinceSaveRequest = -1f;
				Saving = true;
				Worker<bool> worker = ThreadPool.Take<bool>(SaveInternal);
				worker.Finished += delegate
				{
					ThreadPool.Return(worker);
					Saving = false;
				};
				worker.Start(ngpBackup);
			}
		}

		private void SaveInternal(bool ngpBackup)
		{
			ActiveSaveDevice.Save("SaveSlot" + SaveSlot, DoSave);
			if (ngpBackup)
			{
				ActiveSaveDevice.Save("SaveSlot" + SaveSlot + "_EndGame", DoSave);
			}
			if (!PersistentThreadPool.IsOnMainThread)
			{
				Thread.Sleep(33);
			}
			WriteLeaderboardEntry();
		}

		private void DoSave(BinaryWriter writer)
		{
			try
			{
				if (SaveData.SinceLastSaved.HasValue)
				{
					tempSaveData.PlayTime += DateTime.Now.Ticks - SaveData.SinceLastSaved.Value;
					SaveData.PlayTime = tempSaveData.PlayTime;
				}
				SaveData.SinceLastSaved = DateTime.Now.Ticks;
				SaveFileOperations.Write(new CrcWriter(writer), tempSaveData);
			}
			catch (Exception ex)
			{
				Logger.Log("Saving", LogSeverity.Error, ex.ToString());
			}
		}

		public void Reset()
		{
			bool flag2 = (base.InFpsMode = false);
			bool flag4 = (InMenuCube = flag2);
			InMap = flag4;
			LevelManager.Reset();
			PlayerManager.Reset();
			SoundManager.Stop();
			if (ActiveScroll != null)
			{
				ActiveScroll.Close();
				ActiveScroll.NextScroll = null;
			}
		}

		public void Restart()
		{
			if (ForcedSignOut)
			{
				SinceSaveRequest = -1f;
				this.ActiveGamerSignedOut();
			}
			ForceTimePaused = true;
			ScreenFade screenFade = new ScreenFade(ServiceHelper.Game)
			{
				FromColor = ColorEx.TransparentBlack,
				ToColor = Color.Black,
				EaseOut = true,
				CaptureScreen = true,
				Duration = 0.5f,
				WaitUntil = () => !Loading,
				DrawOrder = 2050
			};
			screenFade.ScreenCaptured = (Action)Delegate.Combine(screenFade.ScreenCaptured, (Action)delegate
			{
				Loading = true;
				Worker<bool> worker = ThreadPool.Take<bool>(DoRestart);
				worker.Finished += delegate
				{
					ThreadPool.Return(worker);
					SkipLoadBackground = false;
					Loading = false;
				};
				worker.Start(context: false);
			});
			ServiceHelper.AddComponent(screenFade);
		}

		private void DoRestart(bool dummy)
		{
			SkipLoadBackground = true;
			if (MenuCube.Instance != null)
			{
				ServiceHelper.RemoveComponent(MenuCube.Instance);
			}
			if (WorldMap.Instance != null)
			{
				ServiceHelper.RemoveComponent(WorldMap.Instance);
			}
			if (EndGame)
			{
				if (SaveData.CubeShards + SaveData.SecretCubes < 64)
				{
					SaveData.Finished32 = true;
				}
				else
				{
					SaveData.Finished64 = true;
				}
				SaveData.CanNewGamePlus = true;
				if (SaveData.World.ContainsKey("GOMEZ_HOUSE"))
				{
					SaveData.World["GOMEZ_HOUSE"].FirstVisit = true;
				}
				SaveData.CloneInto(tempSaveData);
				SaveImmediately(ngpBackup: true);
			}
			Reset();
			if (InCutscene && Intro.Instance != null)
			{
				ServiceHelper.RemoveComponent(Intro.Instance);
			}
			if (MainMenu.Instance != null)
			{
				ServiceHelper.RemoveComponent(MainMenu.Instance);
			}
			if (PauseMenu.Instance != null)
			{
				ServiceHelper.RemoveComponent(PauseMenu.Instance);
			}
			ServiceHelper.AddComponent(new Intro(ServiceHelper.Game)
			{
				Restarted = true,
				FullLogos = (Fez.PublicDemo || EndGame)
			});
			EndGame = false;
		}

		public void ClearSaveFile()
		{
			SaveData.Clear();
			SaveData.IsNew = true;
		}

		public void StartNewGame(Action onFinish)
		{
			OnLoadingComplete = onFinish;
			ClearSaveFile();
			Loading = true;
			Worker<bool> worker = ThreadPool.Take<bool>(DoStartNewGame);
			worker.Finished += delegate
			{
				ThreadPool.Return(worker);
			};
			worker.Start(context: false);
		}

		private void DoStartNewGame(bool dummy)
		{
			SaveData = new SaveData();
			PlayerManager.Reset();
			LevelManager.Reset();
			SoundManager.SoundEffectVolume = SettingsManager.Settings.SoundVolume;
			SoundManager.MusicVolume = SettingsManager.Settings.MusicVolume;
			TimeManager.CurrentTime = DateTime.Today.Add(SaveData.TimeOfDay);
			LevelManager.ChangeLevel(SaveData.Level ?? (IsTrialMode ? "trial/BIG_TOWER" : Fez.ForcedLevelName));
			TimeManager.OnTick();
			OnLoadingComplete();
			Loading = false;
		}

		public void Pause()
		{
			Pause(toCredits: false);
		}

		public void Pause(bool toCredits)
		{
			if (base.Paused)
			{
				return;
			}
			paused = true;
			OnPauseStateChanged();
			PauseMenu pauseMenu = new PauseMenu(ServiceHelper.Game);
			if (toCredits)
			{
				pauseMenu.EndGameMenu = true;
				ServiceHelper.AddComponent(pauseMenu);
				pauseMenu.nextMenuLevel = pauseMenu.CreditsMenu;
				pauseMenu.nextMenuLevel.Reset();
				return;
			}
			ServiceHelper.AddComponent(new TileTransition(ServiceHelper.Game)
			{
				ScreenCaptured = delegate
				{
					ServiceHelper.AddComponent(pauseMenu);
				},
				WaitFor = () => pauseMenu.Ready
			});
		}

		public void UnPause()
		{
			if (base.Paused)
			{
				paused = false;
				OnPauseStateChanged();
			}
		}

		public void ToggleInventory()
		{
			InMenuCube = true;
			ServiceHelper.AddComponent(new MenuCube(ServiceHelper.Game));
		}

		public void ToggleMap()
		{
			ServiceHelper.AddComponent(new WorldMap(ServiceHelper.Game));
		}
	}
}
