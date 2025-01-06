using System.Linq;
using FezEngine.Tools;
using Microsoft.Xna.Framework;

namespace FezEngine.Structure
{
	public class InstancePhysicsState : ISimplePhysicsEntity, IPhysicsEntity
	{
		private readonly TrileInstance instance;

		public bool Vanished { get; set; }

		public bool ShouldRespawn { get; set; }

		public bool Respawned { get; set; }

		public bool UpdatingPhysics { get; set; }

		public bool Sticky { get; set; }

		public bool Puppet { get; set; }

		public bool Paused { get; set; }

		public bool PushedUp { get; set; }

		public TrileInstance PushedDownBy { get; set; }

		public bool IgnoreCollision { get; set; }

		public bool ForceNonStatic { get; set; }

		public bool Background { get; set; }

		public MultipleHits<CollisionResult> WallCollision { get; set; }

		public MultipleHits<TrileInstance> Ground { get; set; }

		public Vector3 Velocity { get; set; }

		public Vector3 GroundMovement { get; set; }

		public bool NoVelocityClamping { get; set; }

		public bool IgnoreClampToWater { get; set; }

		public PointCollision[] CornerCollision { get; private set; }

		public bool Grounded => Ground.First != null;

		public Vector3 Center { get; set; }

		public Vector3 Size => instance.TransformedSize;

		public bool Static => StaticGrounds && Grounded && FezMath.AlmostEqual(Velocity, Vector3.Zero) && !ForceNonStatic;

		public bool StaticGrounds => IsGroundStatic(Ground.NearLow) && IsGroundStatic(Ground.FarHigh);

		public bool Sliding => !FezMath.AlmostEqual(Velocity.XZ(), Vector2.Zero);

		public float Elasticity { get; private set; }

		public bool Floating { get; set; }

		public InstancePhysicsState(TrileInstance instance)
		{
			this.instance = instance;
			Center = instance.Center;
			CornerCollision = new PointCollision[4];
			Trile trile = instance.Trile;
			Elasticity = ((trile.ActorSettings.Type == ActorType.Vase || trile.Faces.Values.Any((CollisionType x) => x == CollisionType.AllSides)) ? 0f : 0.15f);
		}

		private static bool IsGroundStatic(TrileInstance ground)
		{
			return ground == null || ground.PhysicsState == null || (ground.PhysicsState.Velocity == Vector3.Zero && ground.PhysicsState.GroundMovement == Vector3.Zero);
		}

		public void UpdateInstance()
		{
			instance.Position = Center - (instance.Center - instance.Position);
		}
	}
}
