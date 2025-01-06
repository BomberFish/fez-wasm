using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Common;
using FezEngine.Components;
using FezEngine.Tools;
using FezGame;
using FezEngine;
using FezGame.Services;
using FezGame.Services.Scripting;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

[assembly: System.Runtime.Versioning.SupportedOSPlatform("browser")]

partial class Program
{
	private static void Main()
	{
		Console.WriteLine("Hello from WASM world!");
		Logger.Log("WASM", "Logger");

		// find ./Fez-Src/FezEngine/FezEngine.Readers -type f -name "*.cs" | awk -F'/' '{filename=$NF; gsub(".cs$", "", filename); print "ContentTypeReaderManager.AddCustomTypeReader(\"FezEngine.Readers." filename ", FezEngine\", typeof(FezEngine.Readers." filename "));"}' | sort
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.AmbienceTrackReader, FezEngine", typeof(FezEngine.Readers.AmbienceTrackReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.AnimatedTextureReader, FezEngine", typeof(FezEngine.Readers.AnimatedTextureReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.ArtObjectActorSettingsReader, FezEngine", typeof(FezEngine.Readers.ArtObjectActorSettingsReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.ArtObjectInstanceReader, FezEngine", typeof(FezEngine.Readers.ArtObjectInstanceReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.ArtObjectReader, FezEngine", typeof(FezEngine.Readers.ArtObjectReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.BackgroundPlaneReader, FezEngine", typeof(FezEngine.Readers.BackgroundPlaneReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.CameraNodeDataReader, FezEngine", typeof(FezEngine.Readers.CameraNodeDataReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.DotDialogueLineReader, FezEngine", typeof(FezEngine.Readers.DotDialogueLineReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.EntityReader, FezEngine", typeof(FezEngine.Readers.EntityReader));
		// ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.ExtensibleReader, FezEngine", typeof(FezEngine.Readers.ExtensibleReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.FezVertexPositionNormalTextureReader, FezEngine", typeof(FezEngine.Readers.FezVertexPositionNormalTextureReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.FrameReader, FezEngine", typeof(FezEngine.Readers.FrameReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.FutureTexture2DReader, FezEngine", typeof(FezEngine.Readers.FutureTexture2DReader));
		// ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.IndexedUserPrimitivesReader, FezEngine", typeof(FezEngine.Readers.IndexedUserPrimitivesReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.InstanceActorSettingsReader, FezEngine", typeof(FezEngine.Readers.InstanceActorSettingsReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.LevelReader, FezEngine", typeof(FezEngine.Readers.LevelReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.LoopReader, FezEngine", typeof(FezEngine.Readers.LoopReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.MapNodeConnectionReader, FezEngine", typeof(FezEngine.Readers.MapNodeConnectionReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.MapNodeReader, FezEngine", typeof(FezEngine.Readers.MapNodeReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.MapTreeReader, FezEngine", typeof(FezEngine.Readers.MapTreeReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.MovementPathReader, FezEngine", typeof(FezEngine.Readers.MovementPathReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.NpcActionContentReader, FezEngine", typeof(FezEngine.Readers.NpcActionContentReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.NpcInstanceReader, FezEngine", typeof(FezEngine.Readers.NpcInstanceReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.NpcMetadataReader, FezEngine", typeof(FezEngine.Readers.NpcMetadataReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.PathSegmentReader, FezEngine", typeof(FezEngine.Readers.PathSegmentReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.RectangularTrixelSurfacePartReader, FezEngine", typeof(FezEngine.Readers.RectangularTrixelSurfacePartReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.ScriptActionReader, FezEngine", typeof(FezEngine.Readers.ScriptActionReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.ScriptConditionReader, FezEngine", typeof(FezEngine.Readers.ScriptConditionReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.ScriptReader, FezEngine", typeof(FezEngine.Readers.ScriptReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.ScriptTriggerReader, FezEngine", typeof(FezEngine.Readers.ScriptTriggerReader));
		// ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.ShaderInstancedIndexedPrimitivesReader, FezEngine", typeof(FezEngine.Readers.ShaderInstancedIndexedPrimitivesReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.SkyLayerReader, FezEngine", typeof(FezEngine.Readers.SkyLayerReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.SkyReader, FezEngine", typeof(FezEngine.Readers.SkyReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.SpeechLineReader, FezEngine", typeof(FezEngine.Readers.SpeechLineReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.TrackedSongReader, FezEngine", typeof(FezEngine.Readers.TrackedSongReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.TrileEmplacementReader, FezEngine", typeof(FezEngine.Readers.TrileEmplacementReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.TrileFaceReader, FezEngine", typeof(FezEngine.Readers.TrileFaceReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.TrileGroupReader, FezEngine", typeof(FezEngine.Readers.TrileGroupReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.TrileInstanceReader, FezEngine", typeof(FezEngine.Readers.TrileInstanceReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.TrileReader, FezEngine", typeof(FezEngine.Readers.TrileReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.TrileSetReader, FezEngine", typeof(FezEngine.Readers.TrileSetReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.TrixelIdentifierReader, FezEngine", typeof(FezEngine.Readers.TrixelIdentifierReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.VertexPositionNormalTextureInstanceReader, FezEngine", typeof(FezEngine.Readers.VertexPositionNormalTextureInstanceReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.VolumeActorSettingsReader, FezEngine", typeof(FezEngine.Readers.VolumeActorSettingsReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.VolumeReader, FezEngine", typeof(FezEngine.Readers.VolumeReader));
		ContentTypeReaderManager.AddCustomTypeReader("FezEngine.Readers.WinConditionsReader, FezEngine", typeof(FezEngine.Readers.WinConditionsReader));

		// manually added
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.Loop, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.Loop>));
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.ArrayReader`1[[FezEngine.Structure.ShardNotes, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ArrayReader<FezEngine.Structure.ShardNotes>));
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.EnumReader`1[[FezEngine.Structure.ShardNotes, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.EnumReader<FezEngine.Structure.ShardNotes>));
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.EnumReader`1[[FezEngine.Structure.AssembleChords, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.EnumReader<FezEngine.Structure.AssembleChords>));
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.EnumReader`1[[FezEngine.FaceOrientation, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.EnumReader<FezEngine.FaceOrientation>));
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.EnumReader`1[[FezEngine.Structure.LiquidType, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.EnumReader<FezEngine.Structure.LiquidType>));
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.DictionaryReader`2[[System.Int32, mscorlib],[FezEngine.Structure.Volume, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.DictionaryReader<System.Int32, FezEngine.Structure.Volume>));
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.ArrayReader`1[[FezEngine.FaceOrientation, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ArrayReader<FezEngine.FaceOrientation>));
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.DictionaryReader`2[[System.Int32, mscorlib],[FezEngine.Structure.Scripting.Script, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.DictionaryReader<System.Int32, FezEngine.Structure.Scripting.Script>));
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.Scripting.ScriptTrigger, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.Scripting.ScriptTrigger>));
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.Scripting.ScriptAction, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.Scripting.ScriptAction>));
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.DictionaryReader`2[[FezEngine.Structure.TrileEmplacement, FezEngine],[FezEngine.Structure.TrileInstance, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.DictionaryReader<FezEngine.Structure.TrileEmplacement, FezEngine.Structure.TrileInstance>));
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.DictionaryReader`2[[System.Int32, mscorlib],[FezEngine.Structure.ArtObjectInstance, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.DictionaryReader<System.Int32, FezEngine.Structure.ArtObjectInstance>));
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.EnumReader`1[[FezEngine.Structure.ActorType, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.EnumReader<FezEngine.Structure.ActorType>));
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.EnumReader`1[[FezEngine.Viewpoint, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.EnumReader<FezEngine.Viewpoint>));
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.DictionaryReader`2[[System.Int32, mscorlib],[FezEngine.Structure.BackgroundPlane, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.DictionaryReader<System.Int32, FezEngine.Structure.BackgroundPlane>));
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.DictionaryReader`2[[System.Int32, mscorlib],[FezEngine.Structure.TrileGroup, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.DictionaryReader<System.Int32, FezEngine.Structure.TrileGroup>));
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.DictionaryReader`2[[System.Int32, mscorlib],[FezEngine.Structure.NpcInstance, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.DictionaryReader<System.Int32, FezEngine.Structure.NpcInstance>));
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.DictionaryReader`2[[System.Int32, mscorlib],[FezEngine.Structure.MovementPath, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.DictionaryReader<System.Int32, FezEngine.Structure.MovementPath>));
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.ListReader`1[[FezEngine.Structure.AmbienceTrack, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.ListReader<FezEngine.Structure.AmbienceTrack>));
		ContentTypeReaderManager.AddCustomTypeReader("Microsoft.Xna.Framework.Content.EnumReader`1[[FezEngine.LevelNodeType, FezEngine]]", typeof(Microsoft.Xna.Framework.Content.EnumReader<FezEngine.LevelNodeType>));
	}

	// [DllImport("Emscripten")]
	// public extern static int mount_opfs();

	private static Fez fez;
	public static bool firstLaunch = true;


	[JSExport]
	internal static Task PreInit()
	{
		return Task.Run(() =>
		{
			Console.WriteLine("PreInit");
			// Console.WriteLine("calling mount_opfs");
			// int ret = mount_opfs();
			// Console.WriteLine($"called mount_opfs: {ret}");
			// if (ret != 0)
			// {
			//     throw new Exception("Failed to mount OPFS");
			// }
		});
	}

	[JSExport]
	internal static void Init()
	{
		try
		{
			// Any init for the Game - usually before game.Run() in the decompilation
			Logger.Log("FEZ", "Initializing.");
			Environment.SetEnvironmentVariable("SDL_AUDIO_CHANNELS", "2");
			Environment.SetEnvironmentVariable("SDL_AUDIO_FREQUENCY", "44100");
			Logger.Clear();
			Logger.Log("FEZ", "Past Logger.Clear()");
			// FNALoggerEXT.LogInfo = delegate(string msg)
			// {
			// 	Logger.Log("FNA", LogSeverity.Information, msg);
			// };
			// FNALoggerEXT.LogWarn = delegate(string msg)
			// {
			// 	Logger.Log("FNA", LogSeverity.Warning, msg);
			// };
			// FNALoggerEXT.LogError = delegate(string msg)
			// {
			// 	Logger.Log("FNA", LogSeverity.Error, msg);
			// };
			Logger.Log("FEZ", "Initializing settings.");
			SettingsManager.InitializeSettings();
			Logger.Log("FEZ", "Settings initialized.");
			PersistentThreadPool.SingleThreaded = true; // SettingsManager.Settings.Singlethreaded;
			Logger.Log("FEZ", "Set SingleThreaded.");
			Fez.DisableSteamworksInit = true; // SettingsManager.Settings.DisableSteamworks;
			Logger.Log("FEZ", "Disabled Steamworks.");
			// Queue<string> queue = new Queue<string>();
			// foreach (string item in args)
			// {
			// 	queue.Enqueue(item);
			// }
			// while (queue.Count > 0)
			// {
			// 	switch (queue.Dequeue().ToLower(CultureInfo.InvariantCulture))
			// 	{
			// 	case "-c":
			// 	case "--clear-save-file":
			// 	{
			// 		string text3 = Path.Combine(Util.LocalSaveFolder, "SaveSlot");
			// 		if (File.Exists(text3 + "0"))
			// 		{
			// 			File.Delete(text3 + "0");
			// 		}
			// 		break;
			// 	}
			// 	case "-ng":
			// 	case "--no-gamepad":
			// 		SettingsManager.Settings.DisableController = true;
			// 		break;
			// 	case "-r":
			// 	case "--region":
			// 		SettingsManager.Settings.Language = (Language)Enum.Parse(typeof(Language), queue.Dequeue());
			// 		break;
			// 	case "--trace":
			// 		TraceFlags.TraceContentLoad = true;
			// 		break;
			// 	case "-pd":
			// 	case "--public-demo":
			// 	{
			// 		Fez.PublicDemo = true;
			// 		string text2 = Path.Combine(Util.LocalSaveFolder, "SaveSlot");
			// 		for (int j = -1; j < 3; j++)
			// 		{
			// 			if (File.Exists(text2 + j))
			// 			{
			// 				File.Delete(text2 + j);
			// 			}
			// 		}
			// 		break;
			// 	}
			// 	case "-nm":
			// 	case "--no-music":
			// 		Fez.NoMusic = true;
			// 		break;
			// 	case "-st":
			// 	case "--singlethreaded":
			// 		SettingsManager.Settings.Singlethreaded = true;
			// 		PersistentThreadPool.SingleThreaded = true;
			// 		break;
			// 	case "--msaa-option":
			// 		SettingsManager.Settings.MultiSampleOption = true;
			// 		break;
			// 	case "--attempt-highdpi":
			// 		SettingsManager.Settings.HighDPI = true;
			// 		break;
			// 	case "--gotta-gomez-fast":
			// 		Fez.SpeedRunMode = true;
			// 		PersistentThreadPool.SingleThreaded = true;
			// 		break;
			// 	case "--mgvs":
			// 	{
			// 		Fez.SkipIntro = true;
			// 		Fez.ForcedLevelName = "VILLAGEVILLE_3D";
			// 		DotService.DisableDotScripting = true;
			// 		GameLevelManager.ResetCubeBitsOnLevelChange = true;
			// 		string text = Path.Combine(Util.LocalSaveFolder, "SaveSlot");
			// 		if (File.Exists(text + "0"))
			// 		{
			// 			File.Delete(text + "0");
			// 		}
			// 		Fez.ForceGoldenCubes = 1;
			// 		TimeHost.FreezeTime = true;
			// 		PersistentThreadPool.SingleThreaded = true;
			// 		break;
			// 	}
			// 	case "--give-bugs-pls":
			// 		Fez.SpeedRunBugs = true;
			// 		break;
			// 	}
			// }
			if (SettingsManager.Settings.HighDPI)
			{
				Environment.SetEnvironmentVariable("FNA_GRAPHICS_ENABLE_HIGHDPI", "1");
			}
			Logger.Log("FEZ", "Initializing audio.");
			try
			{
				SoundEffect.MasterVolume = SoundEffect.MasterVolume;
				Logger.Log("FEZ", "Initialized audio.");
			}
			catch
			{
				// SDL.SDL_SetHint("SDL_AUDIODRIVER", "dummy");
				Logger.Log("FEZ", LogSeverity.Error, "Failed to initialize audio.");
			}
			Logger.Log("FEZ", "Setting thread priority.");
			// Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
			Logger.Log("FEZ", "Instantiating Game.");
			fez = new Fez();

			// SetMainLoop(MainLoop);

			// if (!fez.IsDisposed)
			// {
			// 	Logger.Log("FEZ", "Running Game.");
			// 	fez.Run();
			// }

			// Logger.Try(MainInternal);
			// if (fez != null)
			// {
			// 	fez.Dispose();
			// }
			// fez = null;
			// Logger.Log("FEZ", "Exiting.");
		}
		catch (Exception e)
		{
			Logger.Log("FEZ", LogSeverity.Error, "Error in Init()");
			Logger.LogError(e);
		}
	}

	[JSExport]
	internal static void Cleanup()
	{
		Logger.Log("FEZ", "Cleaning up.");
		// Any cleanup for the Game - usually after game.Run() in the decompilation
	}

	[JSExport]
	internal static bool MainLoop()
	{
		try
		{
			fez.RunOneFrame();
			return true;
		}
		catch (Exception e)
		{
			Logger.Log("FEZ", LogSeverity.Error, "Error in MainLoop()!");
			Logger.LogError(e);
			throw;
		}
		// return fez.RunApplication;
	}

	// [JSImport("setMainLoop", "main.js")]
	// internal static partial void SetMainLoop([JSMarshalAs<JSType.Function>] Action cb);

	// [JSImport("stopMainLoop", "main.js")]
	// internal static partial void StopMainLoop();

	// [JSImport("syncFs", "main.js")]
	// internal static partial void Sync([JSMarshalAs<JSType.Function>] Action cb);
}