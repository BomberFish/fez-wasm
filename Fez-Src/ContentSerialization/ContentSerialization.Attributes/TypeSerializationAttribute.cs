using System;

namespace ContentSerialization.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
	public class TypeSerializationAttribute : Attribute
	{
		public bool FlattenToList { get; set; }
	}
}
