using System;
using Microsoft.Xna.Framework;

namespace FezEngine.Structure.Input
{
	public struct MouseDragState : IEquatable<MouseDragState>
	{
		private readonly Point start;

		private readonly Point movement;

		private readonly bool preDrag;

		public Point Start => start;

		public Point Movement => movement;

		internal bool PreDrag => preDrag;

		internal MouseDragState(Point start, Point current)
			: this(start, current, preDrag: false)
		{
		}

		internal MouseDragState(Point start, Point current, bool preDrag)
		{
			this.start = start;
			this.preDrag = preDrag;
			movement = new Point(current.X - start.X, current.Y - start.Y);
		}

		public bool Equals(MouseDragState other)
		{
			int result;
			if (other.start.Equals(start) && other.movement.Equals(movement))
			{
				bool flag = other.preDrag;
				result = (flag.Equals(preDrag) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != typeof(MouseDragState))
			{
				return false;
			}
			return Equals((MouseDragState)obj);
		}

		public override int GetHashCode()
		{
			Point point = start;
			int hashCode = point.GetHashCode();
			int num = hashCode * 397;
			point = movement;
			hashCode = num ^ point.GetHashCode();
			int num2 = hashCode * 397;
			bool flag = preDrag;
			return num2 ^ flag.GetHashCode();
		}

		public static bool operator ==(MouseDragState left, MouseDragState right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(MouseDragState left, MouseDragState right)
		{
			return !left.Equals(right);
		}
	}
}
