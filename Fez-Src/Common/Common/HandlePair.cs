using System;

namespace Common
{
	internal struct HandlePair<T, U> : IEquatable<HandlePair<T, U>>
	{
		private readonly T first;

		private readonly U second;

		private readonly int hash;

		public HandlePair(T first, U second)
		{
			this.first = first;
			this.second = second;
			hash = 27232 + first.GetHashCode();
		}

		public override int GetHashCode()
		{
			return hash;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is HandlePair<T, U>))
			{
				return false;
			}
			return Equals((HandlePair<T, U>)obj);
		}

		public bool Equals(HandlePair<T, U> other)
		{
			T val = other.first;
			int result;
			if (val.Equals(first))
			{
				U val2 = other.second;
				result = (val2.Equals(second) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}
	}
}
