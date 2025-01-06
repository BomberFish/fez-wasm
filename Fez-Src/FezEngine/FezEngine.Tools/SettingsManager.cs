using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Common;
using ContentSerialization;
using FezEngine.Effects;
using FezEngine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SDL2;

namespace FezEngine.Tools
{
	public static class SettingsManager
	{
		public const float SixteenByNine = 1.7777778f;

		private const string SettingsFilename = "Settings";

		public static Settings Settings;

		public static GraphicsDeviceManager DeviceManager;

		public static bool FirstOpen;

		public static List<DisplayMode>[] PerAdapterResolutions;

		public static DisplayMode[] PerAdapterNativeResolution;

		private static float viewScale;

		private static int letterboxW = 0;

		private static int letterboxH = 0;

		public static List<DisplayMode> Resolutions
		{
			get
			{
				int num = Settings.DisplayAdapterIndex;
				if (num >= GraphicsAdapter.Adapters.Count)
				{
					num = 0;
				}
				return PerAdapterResolutions[num];
			}
		}

		public static DisplayMode NativeResolution
		{
			get
			{
				int num = Settings.DisplayAdapterIndex;
				if (num >= GraphicsAdapter.Adapters.Count)
				{
					num = 0;
				}
				return PerAdapterNativeResolution[num];
			}
		}

		public static GraphicsAdapter DesiredAdapter
		{
			get
			{
				int num = Settings.DisplayAdapterIndex;
				if (num >= GraphicsAdapter.Adapters.Count)
				{
					num = 0;
				}
				return GraphicsAdapter.Adapters[num];
			}
		}

		public static bool SupportsHardwareInstancing { get; private set; }

		public static int MaxMultiSampleCount { get; private set; }

		public static void InitializeSettings()
		{
			Logger.Log("SettingsManager", "Initializing settings");
			string text = Path.Combine(Util.LocalConfigFolder, "Settings");
			Logger.Log("SettingsManager", "Settings file is " + text);
			FirstOpen = !File.Exists(text);
			Logger.Log("SettingsManager", "First open is " + FirstOpen);
			if (FirstOpen)
			{
				Logger.Log("SettingsManager", "Creating new settings");
				Settings = new Settings();
			}
			else
			{
				try
				{
					Logger.Log("SettingsManager", "Deserializing settings");
					Settings = SdlSerializer.Deserialize<Settings>(text);
				}
				catch (Exception e)
				{
					Logger.Log("SettingsManager", LogSeverity.Warning, "Failed to deserialize settings! Resetting to defaults.");
					Logger.LogError(e);
					Settings = new Settings();
				}
			}
			Logger.Log("SettingsManager", "Settings initialized");
			Culture.Language = Settings.Language;
			Logger.Log("SettingsManager", "Language set to " + Settings.Language);
			Save();
			Logger.Log("SettingsManager", "Settings saved");
		}

		public static void InitializeResolutions()
		{
			PerAdapterResolutions = GraphicsAdapter.Adapters.Select((GraphicsAdapter ga) => (from x in ga.SupportedDisplayModes.Distinct(DisplayModeEqualityComparer.Default)
				where x.Width >= 1280 || x.Height >= 720
				orderby x.Width, x.Height
				select x).ToList()).ToArray();
			PerAdapterNativeResolution = GraphicsAdapter.Adapters.Select((GraphicsAdapter x) => x.CurrentDisplayMode).ToArray();
			int i;
			for (i = 0; i < GraphicsAdapter.Adapters.Count; i++)
			{
				if (PerAdapterResolutions[i].Any((DisplayMode x) => x.Width >= 1280 && x.Height >= 720))
				{
					PerAdapterResolutions[i].RemoveAll((DisplayMode x) => x.Width < 1280 || (x.Height < 720 && x != PerAdapterNativeResolution[i]));
				}
			}
		}

		[DllImport("FNA3D", CallingConvention = CallingConvention.Cdecl)]
		public static extern byte FNA3D_SupportsHardwareInstancing(IntPtr device);

		[DllImport("FNA3D", CallingConvention = CallingConvention.Cdecl)]
		public static extern int FNA3D_GetMaxMultiSampleCount(IntPtr device, SurfaceFormat format, int multiSampleCount);

		public static void InitializeCapabilities()
		{
			FieldInfo field = typeof(GraphicsDevice).GetField("GLDevice", BindingFlags.Instance | BindingFlags.NonPublic);
			IntPtr device = (IntPtr)field.GetValue(DeviceManager.GraphicsDevice);
			SupportsHardwareInstancing = FNA3D_SupportsHardwareInstancing(device) == 1;
			MaxMultiSampleCount = FNA3D_GetMaxMultiSampleCount(device, SurfaceFormat.Color, 16);
			if (Settings.HardwareInstancing && !SupportsHardwareInstancing)
			{
				Settings.HardwareInstancing = false;
			}
			Logger.Log("Instancing", LogSeverity.Information, "Hardware instancing is " + (Settings.HardwareInstancing ? "enabled" : "disabled"));
			if (Settings.MultiSampleCount > MaxMultiSampleCount)
			{
				Settings.MultiSampleCount = MaxMultiSampleCount;
			}
			Settings.HighDPI = Environment.GetEnvironmentVariable("FNA_GRAPHICS_ENABLE_HIGHDPI") == "1";
			DeviceManager.PreparingDeviceSettings += delegate(object o, PreparingDeviceSettingsEventArgs e)
			{
				if (Settings.DisplayAdapterIndex >= GraphicsAdapter.Adapters.Count)
				{
					Settings.DisplayAdapterIndex = 0;
				}
				GraphicsAdapter adapter = GraphicsAdapter.Adapters[Settings.DisplayAdapterIndex];
				e.GraphicsDeviceInformation.Adapter = adapter;
			};
			DeviceManager.DeviceReset += delegate
			{
				DeviceManager.GraphicsDevice.SetupViewport();
				Settings.DisplayAdapterIndex = GraphicsAdapter.Adapters.IndexOf(DeviceManager.GraphicsDevice.Adapter);
			};
		}

		public static void Save()
		{
			SdlSerializer.Serialize(Path.Combine(Util.LocalConfigFolder, "Settings"), Settings);
		}

		public static void Apply()
		{
			Game game = ServiceHelper.Game;
			int num = Settings.Width;
			int num2 = Settings.Height;
			if (Settings.DisplayAdapterIndex >= GraphicsAdapter.Adapters.Count)
			{
				Settings.DisplayAdapterIndex = 0;
			}
			GraphicsAdapter graphicsAdapter = GraphicsAdapter.Adapters[Settings.DisplayAdapterIndex];
			if (Settings.UseCurrentMode)
			{
				DisplayMode currentDisplayMode = graphicsAdapter.CurrentDisplayMode;
				num2 = (Settings.Height = currentDisplayMode.Height);
				num = (Settings.Width = (int)Math.Round((float)num2 * currentDisplayMode.AspectRatio));
			}
			DeviceManager.IsFullScreen = Settings.ScreenMode == ScreenMode.Fullscreen;
			if (DeviceManager.IsFullScreen)
			{
				float num4 = (float)num / (float)num2 / graphicsAdapter.CurrentDisplayMode.AspectRatio;
				if (num4 > 1f)
				{
					int num5 = (int)Math.Round((float)num2 * num4);
					letterboxH = num5 - num2;
					num2 = num5;
					letterboxW = 0;
				}
				else
				{
					int num6 = (int)Math.Round((float)num / num4);
					letterboxW = num6 - num;
					num = num6;
					letterboxH = 0;
				}
			}
			else
			{
				letterboxW = 0;
				letterboxH = 0;
			}
			DeviceManager.PreferredBackBufferWidth = num;
			DeviceManager.PreferredBackBufferHeight = num2;
			DeviceManager.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
			game.IsMouseVisible = !DeviceManager.IsFullScreen;
			DeviceManager.SynchronizeWithVerticalRetrace = Settings.VSync;
			BaseEffect.UseHardwareInstancing = Settings.HardwareInstancing;
			DeviceManager.PreferMultiSampling = Settings.MultiSampleCount != 0;
			if (DeviceManager.PreferMultiSampling)
			{
				DeviceManager.GraphicsDevice.PresentationParameters.MultiSampleCount = Settings.MultiSampleCount;
			}
			DeviceManager.ApplyChanges();
			game.Window.IsBorderlessEXT = Settings.ScreenMode == ScreenMode.BorderlessWindowed;
			// if (game.Window.IsBorderlessEXT && !DeviceManager.IsFullScreen && DeviceManager.GraphicsDevice.DisplayMode == DeviceManager.GraphicsDevice.Adapter.CurrentDisplayMode)
			// {
			// 	int num7 = SDL.SDL_WINDOWPOS_CENTERED_DISPLAY(SDL.SDL_GetWindowDisplayIndex(game.Window.Handle));
			// 	SDL.SDL_SetWindowPosition(game.Window.Handle, num7, num7);
			// }
			if (Settings.ScaleMode == ScaleMode.Supersampled)
			{
				float num8 = (float)num / (float)num2;
				int num11;
				int num12;
				if (Math.Abs(num8 - 1.7777778f) > 0.1f && num8 > 1.7777778f)
				{
					int num9 = 720;
					int num10 = (int)Math.Ceiling((float)num2 / (float)num9);
					num11 = num9 * num10;
					num12 = (int)Math.Ceiling((float)num9 * num8 * (float)num10);
				}
				else
				{
					int num9 = 1280;
					int num10 = (int)Math.Ceiling((float)num / (float)num9);
					num12 = num9 * num10;
					num11 = (int)Math.Ceiling((float)num9 / num8 * (float)num10);
				}
				if (DeviceManager.IsFullScreen)
				{
					letterboxW = (int)Math.Round((float)letterboxW * ((float)num12 / (float)num));
					letterboxH = (int)Math.Round((float)letterboxH * ((float)num11 / (float)num2));
				}
				num = num12;
				num2 = num11;
				if (DeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth != num || DeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight != num2)
				{
					DeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth = num;
					DeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight = num2;
					DeviceManager.GraphicsDevice.Reset();
				}
			}
			Logger.Log("SettingsManager", "Using display adapter #" + Settings.DisplayAdapterIndex + " (" + graphicsAdapter.DeviceName + ")");
			Logger.Log("SettingsManager", "Screen set to " + DeviceManager.GraphicsDevice.Adapter.CurrentDisplayMode);
			Logger.Log("SettingsManager", "Screen mode is : " + ((Settings.ScreenMode == ScreenMode.Fullscreen) ? "Fullscreen" : ((Settings.ScreenMode == ScreenMode.BorderlessWindowed) ? "Borderless Window" : "Windowed")));
			if (Settings.HighDPI)
			{
				Logger.Log("SettingsManager", "Hi-DPI mode is enabled.");
			}
			Logger.Log("SettingsManager", $"Backbuffer is {DeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth}x{DeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight}");
			Logger.Log("SettingsManager", "VSync is " + (DeviceManager.SynchronizeWithVerticalRetrace ? "on" : "off"));
			Logger.Log("SettingsManager", "Multisample count is " + DeviceManager.GraphicsDevice.PresentationParameters.MultiSampleCount);
			game.IsMouseVisible = false;
		}

		public static void SetupViewport(this GraphicsDevice device)
		{
			RenderTargetBinding[] renderTargets = device.GetRenderTargets();
			if (renderTargets.Length != 0 && renderTargets[0].RenderTarget is Texture2D)
			{
				return;
			}
			int backBufferWidth = device.PresentationParameters.BackBufferWidth;
			int backBufferHeight = device.PresentationParameters.BackBufferHeight;
			device.Viewport = new Viewport
			{
				X = letterboxW / 2,
				Y = letterboxH / 2,
				Width = backBufferWidth - letterboxW,
				Height = backBufferHeight - letterboxH,
				MinDepth = 0f,
				MaxDepth = 1f
			};
			device.ScissorRectangle = new Rectangle(letterboxW / 2, letterboxH / 2, backBufferWidth - letterboxW, backBufferHeight - letterboxH);
			backBufferWidth = device.Viewport.Width;
			backBufferHeight = device.Viewport.Height;
			float num = (float)backBufferWidth / (float)backBufferHeight;
			switch (Settings.ScaleMode)
			{
			case ScaleMode.FullAspect:
			{
				float num2 = (float)backBufferWidth / 1.7777778f;
				float num3 = (float)backBufferHeight * 1.7777778f;
				if ((float)backBufferWidth >= num3)
				{
					viewScale = num3 / 1280f;
				}
				else
				{
					viewScale = num2 / 720f;
				}
				if (viewScale < 1f)
				{
					viewScale = 1f;
				}
				break;
			}
			case ScaleMode.PixelPerfect:
				if (num > 1.7777778f)
				{
					viewScale = Math.Max(backBufferHeight / 720, 1f);
				}
				else
				{
					viewScale = Math.Max(backBufferWidth / 1280, 1f);
				}
				break;
			case ScaleMode.Supersampled:
			{
				float val = (float)backBufferHeight / 720f;
				float val2 = (float)backBufferWidth / 1280f;
				viewScale = Math.Min(val, val2);
				break;
			}
			}
		}

		public static void UnsetupViewport(this GraphicsDevice device)
		{
			int backBufferWidth = device.PresentationParameters.BackBufferWidth;
			int backBufferHeight = device.PresentationParameters.BackBufferHeight;
			device.Viewport = new Viewport
			{
				X = 0,
				Y = 0,
				Width = backBufferWidth,
				Height = backBufferHeight,
				MinDepth = 0f,
				MaxDepth = 1f
			};
			viewScale = (float)device.Viewport.Width / 1280f;
		}

		public static Point PositionInViewport(this IMouseStateManager mouse)
		{
			Viewport viewport = ServiceHelper.Game.GraphicsDevice.Viewport;
			if (viewport.Width != ServiceHelper.Game.GraphicsDevice.PresentationParameters.BackBufferWidth && viewport.X == 0)
			{
				viewport.X = (ServiceHelper.Game.GraphicsDevice.PresentationParameters.BackBufferWidth - viewport.Width) / 2;
				viewport.Y = (ServiceHelper.Game.GraphicsDevice.PresentationParameters.BackBufferHeight - viewport.Height) / 2;
			}
			Point position = mouse.Position;
			return new Point((int)MathHelper.Clamp(position.X - viewport.X, 0f, viewport.Width), (int)MathHelper.Clamp(position.Y - viewport.Y, 0f, viewport.Height));
		}

		public static float GetViewScale(this GraphicsDevice _)
		{
			return viewScale;
		}
	}
}
