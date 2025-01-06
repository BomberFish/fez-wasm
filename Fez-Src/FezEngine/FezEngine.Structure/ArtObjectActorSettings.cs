using System;
using System.Collections.Generic;
using ContentSerialization.Attributes;
using FezEngine.Structure.Input;
using Microsoft.Xna.Framework;

namespace FezEngine.Structure
{
	public class ArtObjectActorSettings : IEquatable<ArtObjectActorSettings>
	{
		[Serialization(Optional = true, DefaultValueOptional = true)]
		public bool Inactive { get; set; }

		[Serialization(Optional = true, DefaultValueOptional = true)]
		public ActorType ContainedTrile { get; set; }

		[Serialization(Optional = true)]
		public int? AttachedGroup { get; set; }

		[Serialization(Optional = true, DefaultValueOptional = true)]
		public float SpinOffset { get; set; }

		[Serialization(Optional = true, DefaultValueOptional = true)]
		public float SpinEvery { get; set; }

		[Serialization(Optional = true, DefaultValueOptional = true)]
		public Viewpoint SpinView { get; set; }

		[Serialization(Optional = true, DefaultValueOptional = true)]
		public bool OffCenter { get; set; }

		[Serialization(Optional = true, DefaultValueOptional = true)]
		public Vector3 RotationCenter { get; set; }

		[Serialization(Optional = true)]
		public VibrationMotor[] VibrationPattern { get; set; }

		[Serialization(Optional = true)]
		public CodeInput[] CodePattern { get; set; }

		[Serialization(Optional = true)]
		public PathSegment Segment { get; set; }

		[Serialization(Optional = true)]
		public int? NextNode { get; set; }

		[Serialization(Optional = true)]
		public string DestinationLevel { get; set; }

		[Serialization(Optional = true)]
		public string TreasureMapName { get; set; }

		[Serialization(Optional = true, DefaultValueOptional = true)]
		public float TimeswitchWindBackSpeed { get; set; }

		[Serialization(Optional = true, DefaultValueOptional = true)]
		public HashSet<FaceOrientation> InvisibleSides { get; set; }

		[Serialization(Ignore = true)]
		public ArtObjectInstance NextNodeAo { get; set; }

		[Serialization(Ignore = true)]
		public ArtObjectInstance PrecedingNodeAo { get; set; }

		[Serialization(Ignore = true)]
		public bool ShouldMoveToEnd { get; set; }

		[Serialization(Ignore = true)]
		public float? ShouldMoveToHeight { get; set; }

		public ArtObjectActorSettings()
		{
			InvisibleSides = new HashSet<FaceOrientation>(FaceOrientationComparer.Default);
		}

		public bool Equals(ArtObjectActorSettings other)
		{
			return (object)other != null && other.ContainedTrile == ContainedTrile && other.Inactive == Inactive && other.AttachedGroup == AttachedGroup && other.SpinOffset == SpinOffset && other.SpinEvery == SpinEvery && other.SpinView == SpinView && other.OffCenter == OffCenter && other.RotationCenter == RotationCenter && other.VibrationPattern == VibrationPattern && other.CodePattern == CodePattern && other.Segment == Segment && other.NextNode == NextNode && other.DestinationLevel == DestinationLevel && other.TreasureMapName == TreasureMapName && other.TimeswitchWindBackSpeed == TimeswitchWindBackSpeed && other.InvisibleSides.Equals(InvisibleSides);
		}

		public override bool Equals(object obj)
		{
			return obj is ArtObjectActorSettings && Equals(obj as ArtObjectActorSettings);
		}

		public static bool operator ==(ArtObjectActorSettings lhs, ArtObjectActorSettings rhs)
		{
			return lhs?.Equals(rhs) ?? ((object)rhs == null);
		}

		public static bool operator !=(ArtObjectActorSettings lhs, ArtObjectActorSettings rhs)
		{
			return !(lhs == rhs);
		}
	}
}
