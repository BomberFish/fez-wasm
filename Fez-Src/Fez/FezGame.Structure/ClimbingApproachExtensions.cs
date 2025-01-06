using FezEngine;

namespace FezGame.Structure
{
	public static class ClimbingApproachExtensions
	{
		public static HorizontalDirection AsDirection(this ClimbingApproach approach)
		{
			return approach switch
			{
				ClimbingApproach.Left => HorizontalDirection.Left, 
				ClimbingApproach.Right => HorizontalDirection.Right, 
				_ => HorizontalDirection.Right, 
			};
		}
	}
}
