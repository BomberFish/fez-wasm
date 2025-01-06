using FezEngine.Structure;

namespace FezGame.Structure
{
	public static class BoxCollisionResultExtensions
	{
		public static CollisionResult First(this MultipleHits<CollisionResult> result)
		{
			return result.NearLow.Collided ? result.NearLow : (result.FarHigh.Collided ? result.FarHigh : default(CollisionResult));
		}

		public static bool AnyCollided(this MultipleHits<CollisionResult> result)
		{
			return result.NearLow.Collided || result.FarHigh.Collided;
		}

		public static bool AnyHit(this MultipleHits<CollisionResult> result)
		{
			return result.NearLow.Destination != null || result.FarHigh.Destination != null;
		}
	}
}
