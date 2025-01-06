using System;
using Common;
using Microsoft.Xna.Framework;

namespace FezEngine.Structure
{
	public class TrileFace : IEquatable<TrileFace>
	{
		public TrileEmplacement Id;

		public FaceOrientation Face { get; set; }

		public TrileFace()
		{
			Id = default(TrileEmplacement);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode() ^ Face.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return obj is TrileFace && Equals(obj as TrileFace);
		}

		public override string ToString()
		{
			return Util.ReflectToString(this);
		}

		public bool Equals(TrileFace other)
		{
			return (object)other != null && other.Id == Id && other.Face == Face;
		}

		public static bool operator ==(TrileFace lhs, TrileFace rhs)
		{
			return lhs?.Equals(rhs) ?? ((object)rhs == null);
		}

		public static bool operator !=(TrileFace lhs, TrileFace rhs)
		{
			return !(lhs == rhs);
		}

		public static TrileFace operator +(TrileFace lhs, Vector3 rhs)
		{
			return new TrileFace
			{
				Id = lhs.Id + rhs,
				Face = lhs.Face
			};
		}

		public static TrileFace operator -(TrileFace lhs, Vector3 rhs)
		{
			return new TrileFace
			{
				Id = lhs.Id - rhs,
				Face = lhs.Face
			};
		}
	}
}
