using System;
using System.Reflection;

namespace ContentSerialization
{
	public class SdlSerializationException : Exception
	{
		public SdlSerializationException(string message, Type type, string memberName)
			: base(message + "\nType : " + type.Name + "\nMember : " + memberName)
		{
		}

		public SdlSerializationException(string message, Type type, MemberInfo member)
			: this(message, type, member.Name)
		{
		}

		public SdlSerializationException(string message, Type type, string memberName, Exception cause)
			: base(message + "\nType : " + type.Name + "\nMember : " + memberName, cause)
		{
		}

		public SdlSerializationException(string message, Type type, MemberInfo member, Exception cause)
			: this(message, type, member.Name)
		{
		}
	}
}
