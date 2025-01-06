using System;
using Common;
using ContentSerialization.Attributes;

namespace FezEngine.Structure
{
	public class RectangularTrixelSurfacePart : IEquatable<RectangularTrixelSurfacePart>
	{
		public TrixelEmplacement Start { get; set; }

		[Serialization(Name = "tSize")]
		public int TangentSize { get; set; }

		[Serialization(Name = "bSize")]
		public int BitangentSize { get; set; }

		[Serialization(Ignore = true)]
		public FaceOrientation Orientation { get; set; }

		public override int GetHashCode()
		{
			return Util.CombineHashCodes(Start.GetHashCode(), TangentSize.GetHashCode(), BitangentSize.GetHashCode(), Orientation.GetHashCode());
		}

		public override bool Equals(object obj)
		{
			return obj != null && Equals(obj as RectangularTrixelSurfacePart);
		}

		public bool Equals(RectangularTrixelSurfacePart other)
		{
			return other != null && other.Orientation == Orientation && other.Start.Equals(Start) && other.TangentSize == TangentSize && other.BitangentSize == BitangentSize;
		}
	}
}
