using System;
using System.Globalization;
using System.IO;
using Common;
using EasyStorage;
using FezEngine.Components;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Services;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezGame.Structure
{
	internal class SaveManagementLevel : MenuLevel
	{
		private enum SMOperation
		{
			Change,
			CopySource,
			CopyDestination,
			Clear
		}

		private readonly MenuBase Menu;

		private readonly SaveSlotInfo[] Slots = new SaveSlotInfo[3];

		private SaveSlotInfo CopySourceSlot;

		private MenuLevel CopyDestLevel;

		private IGameStateManager GameState;

		private IFontManager FontManager;

		public SaveManagementLevel(MenuBase menu)
		{
			Menu = menu;
		}

		public override void Initialize()
		{
			base.Initialize();
			FontManager = ServiceHelper.Get<IFontManager>();
			GameState = ServiceHelper.Get<IGameStateManager>();
			ReloadSlots();
			base.Title = "SaveManagementTitle";
			SpriteFont sf = FontManager.Small;
			MenuLevel changeLevel = new MenuLevel
			{
				Title = "SaveChangeSlot",
				AButtonString = "ChangeWithGlyph"
			};
			changeLevel.OnPostDraw = delegate(SpriteBatch b, SpriteFont f, GlyphTextRenderer tr, float a)
			{
				IconPostDraw(changeLevel, b, sf, tr, a);
				DrawWarning(b, sf, tr, a, "SaveChangeWarning");
			};
			changeLevel.Parent = this;
			changeLevel.Initialize();
			MenuLevel copySrcLevel = new MenuLevel
			{
				Title = "SaveCopySourceTitle",
				AButtonString = "ChooseWithGlyph"
			};
			copySrcLevel.OnPostDraw = delegate(SpriteBatch b, SpriteFont f, GlyphTextRenderer tr, float a)
			{
				IconPostDraw(copySrcLevel, b, sf, tr, a);
			};
			copySrcLevel.Parent = this;
			copySrcLevel.Initialize();
			CopyDestLevel = new MenuLevel
			{
				Title = "SaveCopyDestTitle",
				AButtonString = "ChooseWithGlyph"
			};
			CopyDestLevel.OnPostDraw = delegate(SpriteBatch b, SpriteFont f, GlyphTextRenderer tr, float a)
			{
				IconPostDraw(CopyDestLevel, b, sf, tr, a);
				DrawWarning(b, sf, tr, a, "SaveCopyWarning");
			};
			CopyDestLevel.Parent = copySrcLevel;
			CopyDestLevel.Initialize();
			MenuLevel clearLevel = new MenuLevel
			{
				Title = "SaveClearTitle",
				AButtonString = "ChooseWithGlyph"
			};
			clearLevel.OnPostDraw = delegate(SpriteBatch b, SpriteFont f, GlyphTextRenderer tr, float a)
			{
				IconPostDraw(clearLevel, b, sf, tr, a);
				DrawWarning(b, sf, tr, a, "SaveClearWarning");
			};
			clearLevel.Parent = this;
			clearLevel.Initialize();
			AddItem("SaveChangeSlot", delegate
			{
				RefreshSlotsFor(changeLevel, SMOperation.Change, (SaveSlotInfo s) => s.Index != GameState.SaveSlot);
				Menu.ChangeMenuLevel(changeLevel);
			});
			AddItem("SaveCopyTitle", delegate
			{
				RefreshSlotsFor(copySrcLevel, SMOperation.CopySource, (SaveSlotInfo s) => !s.Empty);
				Menu.ChangeMenuLevel(copySrcLevel);
			});
			AddItem("SaveClearTitle", delegate
			{
				RefreshSlotsFor(clearLevel, SMOperation.Clear, (SaveSlotInfo s) => !s.Empty);
				Menu.ChangeMenuLevel(clearLevel);
			});
		}

		private void DrawWarning(SpriteBatch batch, SpriteFont font, GlyphTextRenderer tr, float alpha, string locString)
		{
			float scale = FontManager.SmallFactor * batch.GraphicsDevice.GetViewScale();
			float num = (float)batch.GraphicsDevice.Viewport.Height / 2f;
			tr.DrawCenteredString(batch, font, StaticText.GetString(locString), new Color(1f, 1f, 1f, alpha), new Vector2(0f, num * 1.6f), scale);
		}

		private void ReloadSlots()
		{
			IsDynamic = false;
			for (int i = 0; i < 3; i++)
			{
				SaveSlotInfo saveSlotInfo = (Slots[i] = new SaveSlotInfo
				{
					Index = i
				});
				PCSaveDevice pCSaveDevice = new PCSaveDevice("FEZ");
				string fileName = "SaveSlot" + i;
				if (!pCSaveDevice.FileExists(fileName))
				{
					saveSlotInfo.Empty = true;
					continue;
				}
				SaveData saveData = null;
				if (!pCSaveDevice.Load(fileName, delegate(BinaryReader stream)
				{
					saveData = SaveFileOperations.Read(new CrcReader(stream));
				}) || saveData == null)
				{
					saveSlotInfo.Empty = true;
					continue;
				}
				saveSlotInfo.Percentage = ((float)(saveData.CubeShards + saveData.SecretCubes + saveData.PiecesOfHeart) + (float)saveData.CollectedParts / 8f) / 32f;
				saveSlotInfo.PlayTime = new TimeSpan(saveData.PlayTime);
				string text = saveData.Level;
				if (text.Contains("GOMEZ_HOUSE"))
				{
					text = "GOMEZ_HOUSE";
				}
				if (text.Contains("VILLAGEVILLE") || text == "ELDERS" || text == "DRUM")
				{
					text = "VILLAGEVILLE_3D";
				}
				if (text == "PYRAMID" || text == "HEX_REBUILD")
				{
					text = "STARGATE";
				}
				try
				{
					saveSlotInfo.PreviewTexture = base.CMProvider.Global.Load<Texture2D>("Other Textures/map_screens/" + text);
				}
				catch
				{
					Logger.Log("Content", "Room " + text + " does not have a map image!");
				}
				saveSlotInfo.SaveData = saveData;
				IsDynamic = true;
			}
		}

		private void RefreshSlotsFor(MenuLevel level, SMOperation operation, Func<SaveSlotInfo, bool> condition)
		{
			level.Items.Clear();
			level.IsDynamic = IsDynamic;
			if (IsDynamic)
			{
				for (int i = 0; i < 3; i++)
				{
					level.AddItem(null).Selectable = false;
				}
			}
			SaveSlotInfo[] slots = Slots;
			foreach (SaveSlotInfo saveSlotInfo in slots)
			{
				SaveSlotInfo s = saveSlotInfo;
				MenuItem menuItem;
				if (saveSlotInfo.Empty)
				{
					(menuItem = level.AddItem(null, delegate
					{
						ChooseSaveSlot(s, operation);
					})).SuffixText = () => StaticText.GetString("NewSlot");
				}
				else
				{
					(menuItem = level.AddItem("SaveSlotPrefix", delegate
					{
						ChooseSaveSlot(s, operation);
					})).SuffixText = () => string.Format(CultureInfo.InvariantCulture, " {0} ({1:P1} - {2:dd\\.hh\\:mm})", new object[3]
					{
						s.Index + 1,
						s.Percentage,
						s.PlayTime
					});
				}
				menuItem.Disabled = !condition(saveSlotInfo);
				menuItem.Selectable = condition(saveSlotInfo);
			}
			for (int k = (IsDynamic ? 3 : 0); k < level.Items.Count; k++)
			{
				if (level.Items[k].Selectable)
				{
					level.SelectedIndex = k;
					break;
				}
			}
		}

		private void ChooseSaveSlot(SaveSlotInfo slot, SMOperation operation)
		{
			switch (operation)
			{
			case SMOperation.CopySource:
				CopySourceSlot = slot;
				RefreshSlotsFor(CopyDestLevel, SMOperation.CopyDestination, (SaveSlotInfo s) => CopySourceSlot != s);
				Menu.ChangeMenuLevel(CopyDestLevel);
				break;
			case SMOperation.CopyDestination:
			{
				PCSaveDevice pCSaveDevice = new PCSaveDevice("FEZ");
				pCSaveDevice.Save("SaveSlot" + slot.Index, delegate(BinaryWriter writer)
				{
					SaveFileOperations.Write(new CrcWriter(writer), CopySourceSlot.SaveData);
				});
				GameState.SaveToCloud(force: true);
				ReloadSlots();
				Menu.ChangeMenuLevel(this);
				break;
			}
			case SMOperation.Clear:
			{
				PCSaveDevice pCSaveDevice = new PCSaveDevice("FEZ");
				pCSaveDevice.Delete("SaveSlot" + slot.Index);
				if (GameState.SaveSlot == slot.Index)
				{
					GameState.LoadSaveFile(delegate
					{
						GameState.Save();
						GameState.SaveImmediately();
						GameState.SaveToCloud(force: true);
						GameState.Restart();
					});
				}
				else
				{
					ReloadSlots();
					Menu.ChangeMenuLevel(this);
				}
				break;
			}
			case SMOperation.Change:
				GameState.SaveSlot = slot.Index;
				GameState.LoadSaveFile(delegate
				{
					GameState.Save();
					GameState.SaveImmediately();
					GameState.SaveToCloud(force: true);
					GameState.Restart();
				});
				SpeedRun.Dispose();
				break;
			}
		}

		private void IconPostDraw(MenuLevel level, SpriteBatch batch, SpriteFont font, GlyphTextRenderer tr, float alpha)
		{
			if (level.SelectedIndex > 2 && !Slots[level.SelectedIndex - 3].Empty)
			{
				float viewScale = batch.GraphicsDevice.GetViewScale();
				int num = (int)(192f * viewScale);
				Texture2D previewTexture = Slots[level.SelectedIndex - 3].PreviewTexture;
				if (previewTexture != null)
				{
					batch.Draw(previewTexture, new Rectangle(batch.GraphicsDevice.Viewport.Width / 2, batch.GraphicsDevice.Viewport.Height / 2 - (int)(128f * viewScale), num, num), null, Color.White, 0f, new Vector2(previewTexture.Width / 2, previewTexture.Height / 2), SpriteEffects.None, 0f);
				}
			}
		}
	}
}
