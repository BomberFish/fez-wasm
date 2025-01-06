using System;
using FezEngine;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace FezGame.Components.Actions
{
	public class ClimbVine : PlayerAction
	{
		private const float ClimbingSpeed = 0.475f;

		private const float SideClimbingFactor = 0.75f;

		private ClimbingApproach currentApproach;

		private bool shouldSyncAnimationHalfway;

		private Vector3? lastGrabbedLocation;

		private SoundEffect climbSound;

		private int lastFrame;

		public ClimbVine(Game game)
			: base(game)
		{
			base.UpdateOrder = 1;
		}

		protected override void LoadContent()
		{
			base.LoadContent();
			climbSound = base.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/ClimbVine");
		}

		public override void Initialize()
		{
			base.CameraManager.ViewpointChanged += ChangeApproach;
			base.Initialize();
		}

		private void ChangeApproach()
		{
			lastGrabbedLocation = null;
			if (IsActionAllowed(base.PlayerManager.Action) && base.CameraManager.Viewpoint.IsOrthographic() && base.CameraManager.Viewpoint != base.CameraManager.LastViewpoint && !base.PlayerManager.IsOnRotato)
			{
				int distance = base.CameraManager.Viewpoint.GetDistance(base.CameraManager.LastViewpoint);
				int num = (int)(currentApproach + distance);
				if (num > 4)
				{
					num -= 4;
				}
				if (num < 1)
				{
					num += 4;
				}
				currentApproach = (ClimbingApproach)num;
				RefreshPlayerAction(force: true);
				shouldSyncAnimationHalfway = true;
			}
		}

		protected override void TestConditions()
		{
			switch (base.PlayerManager.Action)
			{
			case ActionType.Idle:
			case ActionType.LookingLeft:
			case ActionType.LookingRight:
			case ActionType.LookingUp:
			case ActionType.LookingDown:
			case ActionType.Walking:
			case ActionType.Running:
			case ActionType.Jumping:
			case ActionType.Lifting:
			case ActionType.Falling:
			case ActionType.Bouncing:
			case ActionType.Flying:
			case ActionType.Dropping:
			case ActionType.Sliding:
			case ActionType.Landing:
			case ActionType.Teetering:
			case ActionType.IdlePlay:
			case ActionType.IdleSleep:
			case ActionType.IdleLookAround:
			case ActionType.GrabCornerLedge:
			case ActionType.GrabLedgeBack:
			case ActionType.IdleYawn:
			{
				TrileInstance heldInstance = IsOnVine(out currentApproach);
				if (currentApproach == ClimbingApproach.None || ((!base.InputManager.Up.IsDown() || base.PlayerManager.Action.IsOnLedge()) && (base.PlayerManager.Grounded || (currentApproach != ClimbingApproach.Left && currentApproach != ClimbingApproach.Right) || Math.Sign(base.InputManager.Movement.X) != currentApproach.AsDirection().Sign()) && (!base.PlayerManager.Action.IsOnLedge() || !base.InputManager.Down.IsDown())))
				{
					break;
				}
				if (lastGrabbedLocation.HasValue)
				{
					Vector3 value = lastGrabbedLocation.Value;
					Vector3 position = base.PlayerManager.Position;
					Vector3 a = position - value;
					Vector3 b = base.CameraManager.Viewpoint.SideMask();
					if (Math.Abs(a.Dot(b)) <= 1f && (double)a.Dot(Vector3.UnitY) < 1.625 && (double)a.Dot(Vector3.UnitY) > -0.5)
					{
						break;
					}
				}
				base.PlayerManager.HeldInstance = heldInstance;
				switch (currentApproach)
				{
				case ClimbingApproach.Back:
					base.PlayerManager.NextAction = ActionType.BackClimbingVine;
					break;
				case ClimbingApproach.Right:
				case ClimbingApproach.Left:
					base.PlayerManager.NextAction = ActionType.SideClimbingVine;
					break;
				case ClimbingApproach.Front:
					base.PlayerManager.NextAction = ActionType.FrontClimbingVine;
					break;
				}
				if (base.PlayerManager.Action.IsOnLedge())
				{
					base.PlayerManager.Action = base.PlayerManager.NextAction;
					base.PlayerManager.NextAction = ActionType.None;
					break;
				}
				base.PlayerManager.Action = ((currentApproach == ClimbingApproach.Back) ? ActionType.JumpToClimb : ActionType.JumpToSideClimb);
				base.PlayerManager.Velocity = Vector3.Zero;
				if (currentApproach == ClimbingApproach.Left || currentApproach == ClimbingApproach.Right)
				{
					base.PlayerManager.LookingDirection = currentApproach.AsDirection();
				}
				break;
			}
			}
		}

		private TrileInstance IsOnVine(out ClimbingApproach approach)
		{
			Vector3 vector = base.CameraManager.Viewpoint.ForwardVector();
			Vector3 vector2 = base.CameraManager.Viewpoint.RightVector();
			float num = float.MaxValue;
			bool flag = false;
			TrileInstance trileInstance = null;
			bool flag2 = true;
			if (currentApproach == ClimbingApproach.None)
			{
				NearestTriles nearestTriles = base.LevelManager.NearestTrile(base.PlayerManager.Position - 0.002f * Vector3.UnitY);
				flag2 = nearestTriles.Surface != null && nearestTriles.Surface.Trile.ActorSettings.Type == ActorType.Vine;
			}
			PointCollision[] cornerCollision = base.PlayerManager.CornerCollision;
			for (int i = 0; i < cornerCollision.Length; i++)
			{
				PointCollision pointCollision = cornerCollision[i];
				if (pointCollision.Instances.Surface != null && TestVineCollision(pointCollision.Instances.Surface, onAxis: true))
				{
					TrileInstance surface = pointCollision.Instances.Surface;
					float num2 = surface.Position.Dot(vector);
					if (flag2 && num2 < num && TestVineCollision(pointCollision.Instances.Surface, onAxis: true))
					{
						num = num2;
						trileInstance = surface;
					}
				}
			}
			foreach (NearestTriles value in base.PlayerManager.AxisCollision.Values)
			{
				if (value.Surface != null && TestVineCollision(value.Surface, onAxis: false))
				{
					TrileInstance surface2 = value.Surface;
					float num3 = surface2.Position.Dot(vector);
					if (flag2 && num3 < num)
					{
						flag = true;
						num = num3;
						trileInstance = surface2;
					}
				}
			}
			if (trileInstance != null)
			{
				float phi = FezMath.WrapAngle(trileInstance.Trile.ActorSettings.Face.ToPhi() + trileInstance.Phi);
				Vector3 a = FezMath.OrientationFromPhi(phi).AsVector();
				float num4 = a.Dot(flag ? vector2 : vector);
				if (flag)
				{
					approach = ((!(num4 > 0f)) ? ClimbingApproach.Right : ClimbingApproach.Left);
				}
				else
				{
					approach = ((num4 > 0f) ? ClimbingApproach.Front : ClimbingApproach.Back);
				}
			}
			else
			{
				approach = ClimbingApproach.None;
			}
			return trileInstance;
		}

		private bool TestVineCollision(TrileInstance instance, bool onAxis)
		{
			TrileActorSettings actorSettings = instance.Trile.ActorSettings;
			float combinedPhi = FezMath.WrapAngle(actorSettings.Face.ToPhi() + instance.Phi);
			Axis axis = FezMath.AxisFromPhi(combinedPhi);
			return actorSettings.Type == ActorType.Vine && axis == base.CameraManager.Viewpoint.VisibleAxis() == onAxis;
		}

		protected override void Begin()
		{
			if (currentApproach == ClimbingApproach.None)
			{
				ClimbingApproach approach;
				TrileInstance heldInstance = IsOnVine(out approach);
				base.PlayerManager.HeldInstance = heldInstance;
				currentApproach = approach;
			}
			base.GomezService.OnClimbVine();
		}

		public override void Update(GameTime gameTime)
		{
			if (base.GameState.Loading || base.GameState.InMap || base.GameState.Paused || !base.CameraManager.Viewpoint.IsOrthographic() || base.GameState.InMenuCube)
			{
				return;
			}
			if (base.PlayerManager.Velocity.Y < -0.01f)
			{
				lastGrabbedLocation = null;
			}
			if (shouldSyncAnimationHalfway && (double)base.CameraManager.ViewTransitionStep >= 0.5)
			{
				if (base.PlayerManager.Action == ActionType.BackClimbingVine || base.PlayerManager.Action == ActionType.BackClimbingVineSideways)
				{
					base.PlayerManager.Background = false;
				}
				shouldSyncAnimationHalfway = false;
				SyncAnimation(isActive: true);
				RefreshPlayerDirection(force: true);
			}
			base.Update(gameTime);
		}

		protected override bool Act(TimeSpan elapsed)
		{
			ClimbingApproach approach;
			TrileInstance trileInstance = IsOnVine(out approach);
			base.PlayerManager.HeldInstance = trileInstance;
			if (trileInstance == null || currentApproach == ClimbingApproach.None)
			{
				base.PlayerManager.Action = ActionType.Idle;
				return false;
			}
			lastGrabbedLocation = base.PlayerManager.Position;
			if ((currentApproach == ClimbingApproach.Back || currentApproach == ClimbingApproach.Front) && (approach == ClimbingApproach.Right || approach == ClimbingApproach.Left))
			{
				currentApproach = approach;
			}
			if (base.PlayerManager.Action == ActionType.SideClimbingVine && Math.Abs(base.InputManager.Movement.X) > 0.5f)
			{
				Vector3 vector = Math.Sign(base.InputManager.Movement.X) * base.CameraManager.Viewpoint.RightVector();
				NearestTriles nearestTriles = base.LevelManager.NearestTrile(base.PlayerManager.Position + vector);
				if (nearestTriles.Surface != null && nearestTriles.Surface.Trile.ActorSettings.Type == ActorType.Vine)
				{
					base.PlayerManager.Position += vector * 0.1f;
					base.PlayerManager.ForceOverlapsDetermination();
					trileInstance = IsOnVine(out currentApproach);
					base.PlayerManager.HeldInstance = trileInstance;
				}
			}
			if (trileInstance == null || currentApproach == ClimbingApproach.None)
			{
				base.PlayerManager.Action = ActionType.Idle;
				return false;
			}
			RefreshPlayerAction(force: false);
			RefreshPlayerDirection(force: false);
			Vector3 vector2 = trileInstance.Position + FezMath.HalfVector;
			Vector3 vector3 = vector2;
			Vector3 vector4 = Vector3.Zero;
			switch (currentApproach)
			{
			case ClimbingApproach.Back:
			case ClimbingApproach.Front:
				vector4 = base.CameraManager.Viewpoint.DepthMask();
				break;
			case ClimbingApproach.Right:
			case ClimbingApproach.Left:
			{
				TrileInstance trileInstance2 = base.LevelManager.ActualInstanceAt(base.PlayerManager.Position);
				vector4 = ((trileInstance2 != null && trileInstance2.Trile.ActorSettings.Type == ActorType.Vine) ? base.CameraManager.Viewpoint.SideMask() : FezMath.XZMask);
				break;
			}
			}
			base.PlayerManager.Position = base.PlayerManager.Position * (Vector3.One - vector4) + vector3 * vector4;
			Vector2 vector5 = base.InputManager.Movement * 4.7f * 0.475f * (float)elapsed.TotalSeconds;
			Vector3 vector6 = Vector3.Zero;
			if (base.PlayerManager.Action != ActionType.SideClimbingVine)
			{
				vector6 = Vector3.Transform(Vector3.UnitX * vector5.X * 0.75f, base.CameraManager.Rotation);
			}
			Vector3 vector7 = vector5.Y * Vector3.UnitY;
			QueryOptions options = (base.PlayerManager.Background ? QueryOptions.Background : QueryOptions.None);
			FaceOrientation face = (base.PlayerManager.Background ? base.CameraManager.VisibleOrientation : base.CameraManager.VisibleOrientation.GetOpposite());
			NearestTriles nearestTriles2 = base.LevelManager.NearestTrile(base.PlayerManager.Center + Vector3.Down * 1.5f + base.PlayerManager.Size / 2f * vector6.Sign(), options);
			NearestTriles nearestTriles3 = base.LevelManager.NearestTrile(base.PlayerManager.Center + vector6 * 2f, options);
			if ((nearestTriles3.Surface == null || nearestTriles3.Surface.Trile.ActorSettings.Type != ActorType.Vine) && (nearestTriles2.Deep == null || nearestTriles2.Deep.GetRotatedFace(face) == CollisionType.None))
			{
				vector6 = Vector3.Zero;
			}
			nearestTriles2 = base.LevelManager.NearestTrile(base.PlayerManager.Center + base.PlayerManager.Size / 2f * Vector3.Down, options);
			nearestTriles3 = base.LevelManager.NearestTrile(base.PlayerManager.Center + vector7 * 2f, options);
			if ((nearestTriles3.Surface == null || nearestTriles3.Surface.Trile.ActorSettings.Type != ActorType.Vine) && (nearestTriles2.Deep == null || nearestTriles2.Deep.GetRotatedFace(face) == CollisionType.None))
			{
				vector7 = Vector3.Zero;
				if (base.InputManager.Up.IsDown())
				{
					Vector3 vector8 = base.CameraManager.Viewpoint.RightVector() * base.PlayerManager.LookingDirection.Sign();
					TrileInstance deep = base.LevelManager.NearestTrile(trileInstance.Center + vector8 * 0.5f + vector8 * trileInstance.TransformedSize / 2f).Deep;
					if (deep != null && !deep.Trile.Immaterial && deep.Enabled && deep.GetRotatedFace(face) != CollisionType.None)
					{
						TrileInstance trileInstance3 = base.LevelManager.ActualInstanceAt(deep.Position - vector8 + new Vector3(0.5f));
						TrileInstance deep2 = base.LevelManager.NearestTrile(deep.Position - vector8 + new Vector3(0.5f)).Deep;
						if ((deep2 == null || !deep2.Enabled || deep2.GetRotatedFace(base.CameraManager.VisibleOrientation) == CollisionType.None) && (trileInstance3 == null || !trileInstance3.Enabled || trileInstance3.Trile.Immaterial))
						{
							base.PlayerManager.HeldInstance = deep;
							base.PlayerManager.Action = ActionType.GrabCornerLedge;
							Vector3 vector9 = (-vector8 + Vector3.UnitY) * deep.TransformedSize / 2f;
							base.PlayerManager.Position = deep.Center + vector9;
							base.PlayerManager.ForceOverlapsDetermination();
							return false;
						}
					}
				}
			}
			float num = FezMath.Saturate(Math.Abs(base.PlayerManager.Animation.Timing.NormalizedStep * 2f % 1f - 0.5f)) * 1.4f + 0.25f;
			float num2 = FezMath.Saturate(Math.Abs((base.PlayerManager.Animation.Timing.NormalizedStep + 0.3f) % 1f)) + 0.2f;
			int frame = base.PlayerManager.Animation.Timing.Frame;
			if (lastFrame != frame)
			{
				bool flag = Math.Abs(base.InputManager.Movement.Y) < 0.5f;
				if ((flag && frame == 0) || (!flag && (frame == 1 || frame == 4)))
				{
					climbSound.EmitAt(base.PlayerManager.Position, RandomHelper.Between(-0.10000000149011612, 0.10000000149011612), RandomHelper.Between(0.8999999761581421, 1.0));
				}
				lastFrame = frame;
			}
			base.PlayerManager.Velocity = vector6 * num2 + vector7 * num;
			if (trileInstance.PhysicsState != null)
			{
				base.PlayerManager.Velocity += trileInstance.PhysicsState.Velocity;
			}
			float num3 = ((vector7 == Vector3.Zero) ? 0f : Math.Abs(base.InputManager.Movement.Y));
			if (base.PlayerManager.Action != ActionType.SideClimbingVine)
			{
				num3 = ((vector6 == Vector3.Zero) ? num3 : FezMath.Saturate(num3 + Math.Abs(base.InputManager.Movement.X)));
			}
			base.PlayerManager.Animation.Timing.Update(elapsed, num3);
			base.PlayerManager.GroundedVelocity = base.PlayerManager.Velocity;
			MultipleHits<CollisionResult> multipleHits = base.CollisionManager.CollideEdge(base.PlayerManager.Center, vector7, base.PlayerManager.Size / 2f, Direction2D.Vertical);
			if (vector7.Y < 0f && (multipleHits.NearLow.Collided || multipleHits.FarHigh.Collided) && multipleHits.First.Destination.GetRotatedFace(base.CameraManager.VisibleOrientation) != CollisionType.None)
			{
				lastGrabbedLocation = null;
				base.PlayerManager.HeldInstance = null;
				base.PlayerManager.Action = ActionType.Falling;
			}
			return false;
		}

		private void RefreshPlayerAction(bool force)
		{
			if (force || !(base.InputManager.Movement == Vector2.Zero))
			{
				switch (currentApproach)
				{
				case ClimbingApproach.Back:
					base.PlayerManager.Action = ((base.InputManager.Movement.Y != 0f) ? ActionType.BackClimbingVine : ActionType.BackClimbingVineSideways);
					break;
				case ClimbingApproach.Front:
					base.PlayerManager.Action = ((base.InputManager.Movement.Y != 0f) ? ActionType.FrontClimbingVine : ActionType.FrontClimbingVineSideways);
					break;
				case ClimbingApproach.Right:
				case ClimbingApproach.Left:
					base.PlayerManager.Action = ActionType.SideClimbingVine;
					break;
				}
			}
		}

		private void RefreshPlayerDirection(bool force)
		{
			if (force || !(base.InputManager.Movement == Vector2.Zero))
			{
				if (base.PlayerManager.Action == ActionType.SideClimbingVine)
				{
					base.PlayerManager.LookingDirection = currentApproach.AsDirection();
				}
				else
				{
					base.PlayerManager.LookingDirection = FezMath.DirectionFromMovement(base.InputManager.Movement.X);
				}
			}
		}

		protected override bool IsActionAllowed(ActionType type)
		{
			return base.PlayerManager.Action.IsClimbingVine();
		}
	}
}
