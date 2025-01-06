namespace FezEngine.Structure
{
	public static class NpcActionExtensions
	{
		public static bool AllowsRandomChange(this NpcAction action)
		{
			if (action == NpcAction.Idle || (uint)(action - 3) <= 1u)
			{
				return true;
			}
			return false;
		}

		public static bool Loops(this NpcAction action)
		{
			switch (action)
			{
			case NpcAction.Idle2:
			case NpcAction.Turn:
			case NpcAction.Burrow:
			case NpcAction.Hide:
			case NpcAction.ComeOut:
			case NpcAction.TakeOff:
			case NpcAction.Land:
				return false;
			default:
				return true;
			}
		}

		public static bool IsSpecialIdle(this NpcAction action)
		{
			if ((uint)(action - 2) <= 1u)
			{
				return true;
			}
			return false;
		}
	}
}
