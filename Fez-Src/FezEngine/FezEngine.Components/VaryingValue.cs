using System;

namespace FezEngine.Components
{
	public abstract class VaryingValue<T>
	{
		public T Base;

		public T Variation;

		public Func<T, T, T> Function;

		protected abstract Func<T, T, T> DefaultFunction { get; }

		public T Evaluate()
		{
			return (Function == null) ? DefaultFunction(Base, Variation) : Function(Base, Variation);
		}
	}
}
