using System;
using FezGame.Structure;
using Microsoft.Xna.Framework;

namespace FezGame.Components.Actions
{
	public class Victory : PlayerAction
	{
		private static readonly TimeSpan HappyTime = TimeSpan.FromSeconds(2.0);

		private TimeSpan sinceActive;

		public Victory(Game game)
			: base(game)
		{
		}

		protected override void Begin()
		{
			sinceActive = TimeSpan.Zero;
			base.PlayerManager.Velocity = new Vector3(0f, 0.05f, 0f);
		}

		protected override bool Act(TimeSpan elapsed)
		{
			if (base.PlayerManager.Action != ActionType.VictoryForever)
			{
				sinceActive += elapsed;
				base.PlayerManager.Velocity *= 0.95f;
				long ticks = sinceActive.Ticks;
				TimeSpan happyTime = HappyTime;
				if (ticks >= happyTime.Ticks)
				{
					base.PlayerManager.Action = ActionType.Idle;
				}
			}
			return true;
		}

		protected override bool IsActionAllowed(ActionType type)
		{
			return type == ActionType.Victory || type == ActionType.VictoryForever;
		}
	}
}
