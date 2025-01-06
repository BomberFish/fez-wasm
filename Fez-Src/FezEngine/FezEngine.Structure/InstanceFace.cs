using System;
using Common;

namespace FezEngine.Structure
{
	public class InstanceFace : IEquatable<InstanceFace>
	{
		public TrileInstance Instance { get; set; }

		public FaceOrientation Face { get; set; }

		public InstanceFace()
		{
		}

		public InstanceFace(TrileInstance instance, FaceOrientation face)
		{
			Instance = instance;
			Face = face;
		}

		public override int GetHashCode()
		{
			return Instance.GetHashCode() ^ Face.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return obj is TrileFace && Equals(obj as TrileFace);
		}

		public override string ToString()
		{
			return Util.ReflectToString(this);
		}

		public bool Equals(InstanceFace other)
		{
			return (object)other != null && other.Instance == Instance && other.Face == Face;
		}

		public static bool operator ==(InstanceFace lhs, InstanceFace rhs)
		{
			return lhs?.Equals(rhs) ?? ((object)rhs == null);
		}

		public static bool operator !=(InstanceFace lhs, InstanceFace rhs)
		{
			return !(lhs == rhs);
		}
	}
}
