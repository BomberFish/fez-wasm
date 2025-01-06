using System;

namespace ContentSerialization.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class SerializationAttribute : Attribute
	{
		public bool Ignore { get; set; }

		public bool UseAttribute { get; set; }

		public string Name { get; set; }

		public bool Optional { get; set; }

		public bool DefaultValueOptional { get; set; }

		public string CollectionItemName { get; set; }
	}
}
