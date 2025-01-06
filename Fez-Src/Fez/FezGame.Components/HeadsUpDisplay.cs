using System;
using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezGame.Components
{
	public class HeadsUpDisplay : DrawableGameComponent
	{
		private static readonly TimeSpan HudVisibleTime = TimeSpan.FromSeconds(3.0);

		private static readonly TimeSpan HudFadeInTime = TimeSpan.FromSeconds(0.25);

		private static readonly TimeSpan HudFadeOutTime = TimeSpan.FromSeconds(1.0);

		private SpriteBatch spriteBatch;

		private Texture2D keyIcon;

		private Texture2D cubeIcon;

		private Texture2D antiIcon;

		private Texture2D[] smallCubes;

		private TimeSpan sinceHudUpdate;

		private GlyphTextRenderer tr;

		private string cubeShardsText;

		private string antiText;

		private string keysText;

		private TimeSpan sinceSave;

		[ServiceDependency]
		public IGameCameraManager CameraManager { private get; set; }

		[ServiceDependency]
		public ILevelManager LevelManager { private get; set; }

		[ServiceDependency(Optional = true)]
		public IDebuggingBag DebuggingBag { private get; set; }

		[ServiceDependency]
		public IGameStateManager GameState { private get; set; }

		[ServiceDependency]
		public IFontManager Fonts { private get; set; }

		[ServiceDependency]
		public IContentManagerProvider CMProvider { get; set; }

		public HeadsUpDisplay(Game game)
			: base(game)
		{
		}

		public override void Initialize()
		{
			base.DrawOrder = 10000;
			GameState.HudElementChanged += RefreshHud;
			RefreshHud();
			base.Initialize();
		}

		private void RefreshHud()
		{
			long ticks = sinceHudUpdate.Ticks;
			TimeSpan hudFadeInTime = HudFadeInTime;
			long ticks2 = hudFadeInTime.Ticks;
			hudFadeInTime = HudVisibleTime;
			long num = ticks2 + hudFadeInTime.Ticks;
			hudFadeInTime = HudFadeOutTime;
			if (ticks > num + hudFadeInTime.Ticks)
			{
				sinceHudUpdate = TimeSpan.Zero;
			}
			else
			{
				long ticks3 = sinceHudUpdate.Ticks;
				hudFadeInTime = HudFadeInTime;
				long ticks4 = hudFadeInTime.Ticks;
				hudFadeInTime = HudVisibleTime;
				if (ticks3 > ticks4 + hudFadeInTime.Ticks * 4 / 5)
				{
					hudFadeInTime = HudFadeInTime;
					long ticks5 = hudFadeInTime.Ticks;
					hudFadeInTime = HudVisibleTime;
					sinceHudUpdate = TimeSpan.FromTicks(ticks5 + hudFadeInTime.Ticks * 4 / 5);
				}
			}
			cubeShardsText = GameState.SaveData.CubeShards.ToString();
			antiText = GameState.SaveData.SecretCubes.ToString();
			keysText = GameState.SaveData.Keys.ToString();
		}

		protected override void LoadContent()
		{
			tr = new GlyphTextRenderer(base.Game);
			DrawActionScheduler.Schedule(delegate
			{
				keyIcon = CMProvider.Global.Load<Texture2D>("Other Textures/hud/KEY_CUBE");
				cubeIcon = CMProvider.Global.Load<Texture2D>("Other Textures/hud/NORMAL_CUBE");
				antiIcon = CMProvider.Global.Load<Texture2D>("Other Textures/hud/ANTI_CUBE");
				smallCubes = new Texture2D[8];
				for (int i = 0; i < 8; i++)
				{
					smallCubes[i] = CMProvider.Global.Load<Texture2D>("Other Textures/smallcubes/sc_" + (i + 1));
				}
				spriteBatch = new SpriteBatch(base.GraphicsDevice);
			});
		}

		public override void Update(GameTime gameTime)
		{
			sinceHudUpdate += gameTime.ElapsedGameTime;
			if (GameState.InMenuCube)
			{
				RefreshHud();
			}
			if (GameState.Saving)
			{
				long ticks = sinceSave.Ticks;
				TimeSpan hudFadeInTime = HudFadeInTime;
				if (ticks < hudFadeInTime.Ticks)
				{
					sinceSave += gameTime.ElapsedGameTime;
				}
			}
			else if (sinceSave.Ticks > 0)
			{
				sinceSave -= gameTime.ElapsedGameTime;
			}
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			if (!GameState.Loading && !GameState.InCutscene && (!GameState.HideHUD || GameState.Paused))
			{
				base.GraphicsDevice.SetBlendingMode(BlendingMode.Alphablending);
				spriteBatch.BeginPoint();
				if (!Fez.LongScreenshot)
				{
					DrawHud();
					DrawDebugging();
				}
				spriteBatch.End();
			}
		}

		private void DrawHud()
		{
			Vector2 vector = new Vector2(50f, 58f);
			float num = sinceHudUpdate.Ticks;
			TimeSpan hudFadeInTime = HudFadeInTime;
			float num2 = FezMath.Saturate(num / (float)hudFadeInTime.Ticks);
			long ticks = sinceHudUpdate.Ticks;
			hudFadeInTime = HudVisibleTime;
			float num3 = ticks - hudFadeInTime.Ticks;
			hudFadeInTime = HudFadeOutTime;
			float num4 = FezMath.Saturate(num3 / (float)hudFadeInTime.Ticks);
			if (num4 != 1f)
			{
				float alpha = Easing.EaseOut(num2, EasingType.Quadratic) - Easing.EaseOut(num4, EasingType.Quadratic);
				Color color = new Color(1f, 1f, 1f, alpha);
				SpriteFont font = (Culture.IsCJK ? Fonts.Big : Fonts.Small);
				float num5 = (Culture.IsCJK ? (Fonts.BigFactor * 0.625f) : Fonts.SmallFactor);
				int num6 = FezMath.Round(base.GraphicsDevice.GetViewScale());
				num5 *= (float)num6;
				Vector2 vector2 = new Vector2(60f, 32f);
				Vector2 vector3;
				if (GameState.SaveData.CollectedParts > 0)
				{
					int num7 = Math.Min(GameState.SaveData.CollectedParts, 8);
					spriteBatch.Draw(smallCubes[num7 - 1], vector, null, color, 0f, Vector2.Zero, num6, SpriteEffects.None, 0f);
					string text = num7.ToString();
					vector3 = Fonts.Small.MeasureString(text) * num5;
					tr.DrawShadowedText(spriteBatch, font, text, vector + vector2 * num6 - vector3 * Vector2.UnitY / 2f + Fonts.TopSpacing * Fonts.SmallFactor * Vector2.UnitY, color, num5, Color.Black, 1f, 1f);
					vector.Y += 51 * num6;
				}
				spriteBatch.Draw(cubeIcon, vector, null, color, 0f, Vector2.Zero, num6, SpriteEffects.None, 0f);
				vector3 = Fonts.Small.MeasureString(cubeShardsText) * num5;
				tr.DrawShadowedText(spriteBatch, font, cubeShardsText, vector + vector2 * num6 - vector3 * Vector2.UnitY / 2f + Fonts.TopSpacing * Fonts.SmallFactor * Vector2.UnitY, color, num5, Color.Black, 1f, 1f);
				vector.Y += 51 * num6;
				if (GameState.SaveData.SecretCubes > 0)
				{
					spriteBatch.Draw(antiIcon, vector, null, color, 0f, Vector2.Zero, num6, SpriteEffects.None, 0f);
					vector3 = Fonts.Small.MeasureString(antiText) * num5;
					tr.DrawShadowedText(spriteBatch, font, antiText, vector + vector2 * num6 - vector3 * Vector2.UnitY / 2f + Fonts.TopSpacing * Fonts.SmallFactor * Vector2.UnitY, color, num5, Color.Black, 1f, 1f);
					vector.Y += 51 * num6;
				}
				spriteBatch.Draw(keyIcon, vector, null, color, 0f, Vector2.Zero, num6, SpriteEffects.None, 0f);
				vector3 = Fonts.Small.MeasureString(keysText) * num5;
				tr.DrawShadowedText(spriteBatch, font, keysText, vector + vector2 * num6 - vector3 * Vector2.UnitY / 2f + Fonts.TopSpacing * Fonts.SmallFactor * Vector2.UnitY, color, num5, Color.Black, 1f, 1f);
			}
		}

		private void DrawDebugging()
		{
		}
	}
}
