namespace FezEngine.Structure
{
	public struct MultipleHits<T>
	{
		public T NearLow;

		public T FarHigh;

		public T First => object.Equals(NearLow, default(T)) ? FarHigh : NearLow;

		public override string ToString()
		{
			return $"{{Near/Low: {NearLow} Far/High: {FarHigh}}}";
		}
	}
}
