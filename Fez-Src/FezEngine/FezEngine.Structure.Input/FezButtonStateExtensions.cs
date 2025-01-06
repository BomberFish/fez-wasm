namespace FezEngine.Structure.Input
{
	public static class FezButtonStateExtensions
	{
		public static bool IsDown(this FezButtonState state)
		{
			return state == FezButtonState.Pressed || state == FezButtonState.Down;
		}

		public static FezButtonState NextState(this FezButtonState state, bool pressed)
		{
			return state switch
			{
				FezButtonState.Up => pressed ? FezButtonState.Pressed : FezButtonState.Up, 
				FezButtonState.Pressed => pressed ? FezButtonState.Down : FezButtonState.Released, 
				FezButtonState.Released => pressed ? FezButtonState.Pressed : FezButtonState.Up, 
				_ => pressed ? FezButtonState.Down : FezButtonState.Released, 
			};
		}
	}
}
