using System;

namespace FezEngine.Structure.Geometry
{
	public class SharedVertex<T> : IEquatable<SharedVertex<T>> where T : struct, IEquatable<T>, IVertex
	{
		public int Index { get; set; }

		public int References { get; set; }

		public T Vertex { get; set; }

		public override int GetHashCode()
		{
			return Vertex.GetHashCode();
		}

		public override string ToString()
		{
			return $"{{Vertex:{Vertex} Index:{Index} References:{References}}}";
		}

		public override bool Equals(object obj)
		{
			return obj is SharedVertex<T> && Equals(obj as SharedVertex<T>);
		}

		public bool Equals(SharedVertex<T> other)
		{
			return other.Vertex.Equals(Vertex);
		}
	}
}
