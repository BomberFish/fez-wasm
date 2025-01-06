using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Common
{
	public static class ReflectionHelper
	{
		private static readonly Type[] EmptyTypes;

		public const BindingFlags PublicInstanceMembers = BindingFlags.Instance | BindingFlags.Public;

		private static readonly Dictionary<HandlePair<Type, Type>, Attribute> typeAttributeCache;

		private static readonly Dictionary<HandlePair<PropertyInfo, Type>, Attribute> propertyAttributeCache;

		private static readonly Dictionary<HandlePair<FieldInfo, Type>, Attribute> fieldAttributeCache;

		private static readonly Dictionary<Type, MemberInfo[]> propertyCache;

		private static readonly Dictionary<Type, MemberInfo[]> serviceCache;

		private static readonly Dictionary<MethodInfo, DynamicMethodDelegate> methodCache;

		private static readonly Dictionary<Type, DynamicMethodDelegate> constructorCache;

		static ReflectionHelper()
		{
			EmptyTypes = new Type[0];
			typeAttributeCache = new Dictionary<HandlePair<Type, Type>, Attribute>();
			propertyAttributeCache = new Dictionary<HandlePair<PropertyInfo, Type>, Attribute>();
			fieldAttributeCache = new Dictionary<HandlePair<FieldInfo, Type>, Attribute>();
			propertyCache = new Dictionary<Type, MemberInfo[]>();
			serviceCache = new Dictionary<Type, MemberInfo[]>();
			methodCache = new Dictionary<MethodInfo, DynamicMethodDelegate>();
			constructorCache = new Dictionary<Type, DynamicMethodDelegate>();
		}

		public static T GetFirstAttribute<T>(Type type) where T : Attribute, new()
		{
			Type typeFromHandle = typeof(T);
			HandlePair<Type, Type> key = new HandlePair<Type, Type>(type, typeFromHandle);
			Attribute value;
			lock (typeAttributeCache)
			{
				if (!typeAttributeCache.TryGetValue(key, out value))
				{
					typeAttributeCache.Add(key, value = (T)type.GetCustomAttributes(typeof(T), inherit: false).FirstOrDefault());
				}
			}
			return value as T;
		}

		public static T GetFirstAttribute<T>(PropertyInfo propInfo) where T : Attribute, new()
		{
			Type typeFromHandle = typeof(T);
			HandlePair<PropertyInfo, Type> key = new HandlePair<PropertyInfo, Type>(propInfo, typeFromHandle);
			Attribute value;
			lock (propertyAttributeCache)
			{
				if (!propertyAttributeCache.TryGetValue(key, out value))
				{
					propertyAttributeCache.Add(key, value = (T)propInfo.GetCustomAttributes(typeof(T), inherit: false).FirstOrDefault());
				}
			}
			return value as T;
		}

		public static T GetFirstAttribute<T>(FieldInfo fieldInfo) where T : Attribute, new()
		{
			Type typeFromHandle = typeof(T);
			HandlePair<FieldInfo, Type> key = new HandlePair<FieldInfo, Type>(fieldInfo, typeFromHandle);
			Attribute value;
			lock (fieldAttributeCache)
			{
				if (!fieldAttributeCache.TryGetValue(key, out value))
				{
					fieldAttributeCache.Add(key, value = (T)fieldInfo.GetCustomAttributes(typeof(T), inherit: false).FirstOrDefault());
				}
			}
			return value as T;
		}

		public static T GetFirstAttribute<T>(MemberInfo memberInfo) where T : Attribute, new()
		{
			return (memberInfo is PropertyInfo) ? GetFirstAttribute<T>(memberInfo as PropertyInfo) : GetFirstAttribute<T>(memberInfo as FieldInfo);
		}

		public static MemberInfo[] GetSerializableMembers(Type type)
		{
			MemberInfo[] value;
			lock (propertyCache)
			{
				if (!propertyCache.TryGetValue(type, out value))
				{
					value = (from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)
						where p.GetGetMethod() != null && p.GetSetMethod() != null && p.GetGetMethod().GetParameters().Length == 0
						select p).Cast<MemberInfo>().Union(type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy).Cast<MemberInfo>()).ToArray();
					propertyCache.Add(type, value);
				}
			}
			return value;
		}

		public static MemberInfo[] GetSettableProperties(Type type)
		{
			MemberInfo[] value;
			lock (serviceCache)
			{
				if (!serviceCache.TryGetValue(type, out value))
				{
					MemberInfo[] array = (from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
						where p.GetSetMethod(nonPublic: true) != null
						select p).ToArray();
					value = array;
					serviceCache.Add(type, value);
				}
			}
			return value;
		}

		public static Type GetMemberType(MemberInfo member)
		{
			if (member is PropertyInfo)
			{
				return (member as PropertyInfo).PropertyType;
			}
			if (member is FieldInfo)
			{
				return (member as FieldInfo).FieldType;
			}
			throw new NotImplementedException();
		}

		public static bool IsGenericSet(Type type)
		{
			return type.GetInterfaces().Any((Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISet<>));
		}

		public static bool IsGenericList(Type type)
		{
			return type.GetInterfaces().Any((Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>));
		}

		public static bool IsGenericCollection(Type type)
		{
			return type.GetInterfaces().Any((Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>));
		}

		public static bool IsGenericDictionary(Type type)
		{
			return type.GetInterfaces().Any((Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<, >));
		}

		public static bool IsNullable(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		public static DynamicMethodDelegate CreateDelegate(MethodBase method)
		{
			ParameterInfo[] parameters = method.GetParameters();
			int num = parameters.Length;
			Type[] parameterTypes = new Type[2]
			{
				typeof(object),
				typeof(object[])
			};
			DynamicMethod dynamicMethod = new DynamicMethod("", typeof(object), parameterTypes, typeof(ReflectionHelper).Module, skipVisibility: true);
			ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
			Label label = iLGenerator.DefineLabel();
			iLGenerator.Emit(OpCodes.Ldarg_1);
			iLGenerator.Emit(OpCodes.Ldlen);
			iLGenerator.Emit(OpCodes.Ldc_I4, num);
			iLGenerator.Emit(OpCodes.Beq, label);
			iLGenerator.Emit(OpCodes.Newobj, typeof(TargetParameterCountException).GetConstructor(Type.EmptyTypes));
			iLGenerator.Emit(OpCodes.Throw);
			iLGenerator.MarkLabel(label);
			if (!method.IsStatic && !method.IsConstructor)
			{
				iLGenerator.Emit(OpCodes.Ldarg_0);
				if (method.DeclaringType.IsValueType)
				{
					iLGenerator.Emit(OpCodes.Unbox, method.DeclaringType);
				}
			}
			for (int i = 0; i < num; i++)
			{
				iLGenerator.Emit(OpCodes.Ldarg_1);
				iLGenerator.Emit(OpCodes.Ldc_I4, i);
				iLGenerator.Emit(OpCodes.Ldelem_Ref);
				Type parameterType = parameters[i].ParameterType;
				if (parameterType.IsValueType)
				{
					iLGenerator.Emit(OpCodes.Unbox_Any, parameterType);
				}
			}
			if (method.IsConstructor)
			{
				iLGenerator.Emit(OpCodes.Newobj, method as ConstructorInfo);
			}
			else if (method.IsFinal || !method.IsVirtual)
			{
				iLGenerator.Emit(OpCodes.Call, method as MethodInfo);
			}
			else
			{
				iLGenerator.Emit(OpCodes.Callvirt, method as MethodInfo);
			}
			Type type = (method.IsConstructor ? method.DeclaringType : (method as MethodInfo).ReturnType);
			if (type != typeof(void))
			{
				if (type.IsValueType)
				{
					iLGenerator.Emit(OpCodes.Box, type);
				}
			}
			else
			{
				iLGenerator.Emit(OpCodes.Ldnull);
			}
			iLGenerator.Emit(OpCodes.Ret);
			return (DynamicMethodDelegate)dynamicMethod.CreateDelegate(typeof(DynamicMethodDelegate));
		}

		public static object Instantiate(Type type)
		{
			if (type.IsValueType)
			{
				return Activator.CreateInstance(type);
			}
			if (type.IsArray)
			{
				return Array.CreateInstance(type.GetElementType(), 0);
			}
			DynamicMethodDelegate value;
			lock (constructorCache)
			{
				if (!constructorCache.TryGetValue(type, out value))
				{
					value = CreateDelegate(type.GetConstructor(EmptyTypes));
					constructorCache.Add(type, value);
				}
			}
			return value(null);
		}

		public static DynamicMethodDelegate GetDelegate(MethodInfo info)
		{
			DynamicMethodDelegate value;
			lock (methodCache)
			{
				if (!methodCache.TryGetValue(info, out value))
				{
					value = CreateDelegate(info);
					methodCache.Add(info, value);
				}
			}
			return value;
		}

		public static object InvokeMethod(MethodInfo info, object targetInstance, params object[] arguments)
		{
			return GetDelegate(info)(targetInstance, arguments);
		}

		public static object GetValue(PropertyInfo member, object instance)
		{
			return InvokeMethod(member.GetGetMethod(nonPublic: true), instance);
		}

		public static object GetValue(MemberInfo member, object instance)
		{
			if (member is PropertyInfo)
			{
				return GetValue(member as PropertyInfo, instance);
			}
			if (member is FieldInfo)
			{
				return (member as FieldInfo).GetValue(instance);
			}
			throw new NotImplementedException();
		}

		public static void SetValue(PropertyInfo member, object instance, object value)
		{
			InvokeMethod(member.GetSetMethod(nonPublic: true), instance, value);
		}

		public static void SetValue(MemberInfo member, object instance, object value)
		{
			if (member is PropertyInfo)
			{
				SetValue(member as PropertyInfo, instance, value);
				return;
			}
			if (member is FieldInfo)
			{
				(member as FieldInfo).SetValue(instance, value);
				return;
			}
			throw new NotImplementedException();
		}

		public static string GetShortAssemblyQualifiedName<T>()
		{
			return GetShortAssemblyQualifiedName(typeof(T));
		}

		public static string GetShortAssemblyQualifiedName(Type type)
		{
			return GetShortAssemblyQualifiedName(type.AssemblyQualifiedName);
		}

		public static string GetShortAssemblyQualifiedName(string assemblyQName)
		{
			while (assemblyQName.Contains(", Version"))
			{
				int num = assemblyQName.IndexOf(", Version");
				int num2 = assemblyQName.IndexOf("],", num);
				if (num2 == -1)
				{
					num2 = assemblyQName.Length;
				}
				if (assemblyQName[num2 - 1] == ']')
				{
					num2--;
				}
				assemblyQName = assemblyQName.Substring(0, num) + assemblyQName.Substring(num2);
			}
			return assemblyQName;
		}
	}
}
