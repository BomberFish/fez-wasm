namespace FezEngine.Structure
{
	public static class LiquidTypeExtensions
	{
		public static bool IsWater(this LiquidType waterType)
		{
			return waterType == LiquidType.Blood || waterType == LiquidType.Water || waterType == LiquidType.Purple || waterType == LiquidType.Green;
		}
	}
}
