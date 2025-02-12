using System;
using FezEngine.Services;
using FezEngine.Tools;
using Microsoft.Xna.Framework;

namespace FezEngine.Components
{
	public class TimeHost : GameComponent
	{
		public static bool FreezeTime = false;

		[ServiceDependency]
		public IEngineStateManager EngineState { private get; set; }

		[ServiceDependency]
		public ITimeManager TimeManager { private get; set; }

		public TimeHost(Game game)
			: base(game)
		{
		}

		public override void Update(GameTime gameTime)
		{
			if (!EngineState.TimePaused && !EngineState.Loading && !FreezeTime)
			{
				TimeManager.CurrentTime += TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.TotalMilliseconds * (double)TimeManager.TimeFactor);
				TimeManager.OnTick();
			}
		}
	}
}
