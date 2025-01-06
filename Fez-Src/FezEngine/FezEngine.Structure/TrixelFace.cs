using System;
using Common;

namespace FezEngine.Structure
{
	public class TrixelFace : IEquatable<TrixelFace>
	{
		public TrixelEmplacement Id;

		public FaceOrientation Face { get; set; }

		public TrixelFace()
			: this(default(TrixelEmplacement), FaceOrientation.Left)
		{
		}

		public TrixelFace(int x, int y, int z, FaceOrientation face)
			: this(new TrixelEmplacement(x, y, z), face)
		{
		}

		public TrixelFace(TrixelEmplacement id, FaceOrientation face)
		{
			Id = id;
			Face = face;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode() + Face.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return obj is TrixelFace && Equals(obj as TrixelFace);
		}

		public override string ToString()
		{
			return Util.ReflectToString(this);
		}

		public bool Equals(TrixelFace other)
		{
			return other.Id.Equals(Id) && other.Face == Face;
		}
	}
}
