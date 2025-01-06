using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using Common;
using ContentSerialization.Attributes;
using ContentSerialization.Properties;
using Microsoft.Xna.Framework;
using SDL;

namespace ContentSerialization
{
	public static class SdlSerializer
	{
		public const string AnonymousTag = "content";

		public static bool Compiling;

		public static bool IgnoreMissing;

		private const string TypeAttribute = "type";

		private static readonly SerializationAttribute DefaultAttribute = new SerializationAttribute();

		private static readonly System.Type[] EmptyTypes = new System.Type[0];

		public static void Serialize<T>(StreamWriter writer, T instance)
		{
			System.Type typeFromHandle = typeof(T);
			Tag tag = new Tag(LowerCamelCase(typeFromHandle.Name));
			tag["type"] = ReflectionHelper.GetShortAssemblyQualifiedName(typeFromHandle);
			SerializeInternal(instance, typeFromHandle, tag);
			tag.Write(writer, includeRoot: true);
		}

		public static void Serialize<T>(string filePath, T instance)
		{
			System.Type typeFromHandle = typeof(T);
			Tag tag = new Tag(LowerCamelCase(typeFromHandle.Name));
			tag["type"] = ReflectionHelper.GetShortAssemblyQualifiedName(typeFromHandle);
			SerializeInternal(instance, typeFromHandle, tag);
			tag.WriteFile(filePath, includeRoot: true);
		}

		private static void SerializeInternal(object instance, System.Type declaredType, Tag tag)
		{
			TypeSerializationAttribute typeSerializationAttribute = ReflectionHelper.GetFirstAttribute<TypeSerializationAttribute>(declaredType) ?? new TypeSerializationAttribute();
			MemberInfo[] serializableMembers = ReflectionHelper.GetSerializableMembers(declaredType);
			foreach (MemberInfo memberInfo in serializableMembers)
			{
				SerializationAttribute serializationAttribute = ReflectionHelper.GetFirstAttribute<SerializationAttribute>(memberInfo) ?? new SerializationAttribute();
				if (serializationAttribute.Ignore)
				{
					continue;
				}
				System.Type memberType = ReflectionHelper.GetMemberType(memberInfo);
				object value = ReflectionHelper.GetValue(memberInfo, instance);
				if ((IgnoreMissing || serializationAttribute.Optional) && !typeSerializationAttribute.FlattenToList && (value == null || (serializationAttribute.DefaultValueOptional && IsDefault(value))))
				{
					continue;
				}
				value = TryCoerce(value, out var simpleType);
				if ((typeSerializationAttribute.FlattenToList || serializationAttribute.UseAttribute) && !simpleType)
				{
					throw new SdlSerializationException(Resources.SimpleTypeRequired, declaredType, memberInfo);
				}
				if (typeSerializationAttribute.FlattenToList)
				{
					tag.AddValue(value);
					continue;
				}
				string text = LowerCamelCase(serializationAttribute.Name ?? memberInfo.Name);
				if (serializationAttribute.UseAttribute)
				{
					tag[text] = value;
				}
				else
				{
					SerializeChild(text, value, memberType, simpleType, serializationAttribute, tag);
				}
			}
		}

		private static bool IsDefault(object value)
		{
			System.Type type = value.GetType();
			if (type == typeof(string))
			{
				return (string)value == "";
			}
			if (type == typeof(char))
			{
				return (char)value == '\0';
			}
			if (type == typeof(decimal))
			{
				return (decimal)value == 0m;
			}
			if (type == typeof(double))
			{
				return (double)value == 0.0;
			}
			if (type == typeof(float))
			{
				return (float)value == 0f;
			}
			if (type == typeof(long))
			{
				return (long)value == 0;
			}
			if (type == typeof(ushort))
			{
				return (ushort)value == 0;
			}
			if (type == typeof(uint))
			{
				return (uint)value == 0;
			}
			if (type == typeof(int))
			{
				return (int)value == 0;
			}
			if (type == typeof(short))
			{
				return (short)value == 0;
			}
			if (type == typeof(sbyte))
			{
				return (sbyte)value == 0;
			}
			if (type == typeof(byte))
			{
				return (byte)value == 0;
			}
			if (type == typeof(bool))
			{
				return !(bool)value;
			}
			if (value is ICollection)
			{
				return (value as ICollection).Count == 0;
			}
			if (value is Array)
			{
				return (value as Array).Length == 0;
			}
			if (ReflectionHelper.IsGenericSet(type))
			{
				return (int)ReflectionHelper.GetValue(type.GetProperty("Count", BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding, null, typeof(int), EmptyTypes, null), value) == 0;
			}
			if (type.IsEnum)
			{
				return (int)value == 0;
			}
			return value.Equals(ReflectionHelper.Instantiate(type));
		}

		private static string LowerCamelCase(string identifier)
		{
			return identifier.Substring(0, 1).ToLower(CultureInfo.InvariantCulture) + identifier.Substring(1);
		}

		private static object TryCoerce(object value, out bool simpleType)
		{
			value = SDLUtil.TryCoerce(value, out simpleType);
			if (!simpleType)
			{
				System.Type type = value.GetType();
				if (value is Enum)
				{
					value = Util.GetName(type, value);
					simpleType = true;
				}
				else if (ReflectionHelper.IsNullable(type))
				{
					PropertyInfo property = type.GetProperty("Value");
					value = SDLUtil.TryCoerce(ReflectionHelper.GetValue(property, value), out simpleType);
				}
			}
			return value;
		}

		private static void SerializeChild(string name, object value, System.Type type, bool simpleType, SerializationAttribute attr, Tag parent)
		{
			Tag tag = new Tag(name);
			if ((!simpleType || type == typeof(object)) && value != null && !ReflectionHelper.IsNullable(type) && value.GetType() != type)
			{
				type = value.GetType();
				tag["type"] = ReflectionHelper.GetShortAssemblyQualifiedName(type);
			}
			if (simpleType)
			{
				tag.AddValue(value);
			}
			else if (type == typeof(Color))
			{
				Color color = (Color)value;
				AddValueList(tag, color.R, color.G, color.B);
				if (color.A != byte.MaxValue)
				{
					tag.AddValue(color.A);
				}
			}
			else if (type == typeof(Vector2))
			{
				Vector2 vector = (Vector2)value;
				AddValueList(tag, vector.X, vector.Y);
			}
			else if (type == typeof(Vector3))
			{
				Vector3 vector2 = (Vector3)value;
				AddValueList(tag, vector2.X, vector2.Y, vector2.Z);
			}
			else if (type == typeof(Vector4))
			{
				Vector4 vector3 = (Vector4)value;
				AddValueList(tag, vector3.X, vector3.Y, vector3.Z, vector3.W);
			}
			else if (type == typeof(Quaternion))
			{
				Quaternion quaternion = (Quaternion)value;
				AddValueList(tag, quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
			}
			else if (type == typeof(Matrix))
			{
				SerializeMatrix(tag, (Matrix)value);
			}
			else if (ReflectionHelper.IsGenericDictionary(type))
			{
				SerializeDictionary(tag, value as IDictionary, attr.CollectionItemName);
			}
			else if (ReflectionHelper.IsGenericCollection(type))
			{
				SerializeCollection(tag, value as IEnumerable, attr.CollectionItemName);
			}
			else
			{
				SerializeInternal(value, type, tag);
			}
			parent.AddChild(tag);
		}

		private static void SerializeMatrix(Tag tag, Matrix matrix)
		{
			Tag tag2 = new Tag("content");
			tag.AddChild(tag2);
			AddValueList(tag2, matrix.M11, matrix.M12, matrix.M13, matrix.M14);
			tag2 = new Tag("content");
			tag.AddChild(tag2);
			AddValueList(tag2, matrix.M21, matrix.M22, matrix.M23, matrix.M24);
			tag2 = new Tag("content");
			tag.AddChild(tag2);
			AddValueList(tag2, matrix.M31, matrix.M32, matrix.M33, matrix.M34);
			tag2 = new Tag("content");
			tag.AddChild(tag2);
			AddValueList(tag2, matrix.M41, matrix.M42, matrix.M43, matrix.M44);
		}

		private static void SerializeDictionary(Tag tag, IDictionary dictionary, string customItemName)
		{
			bool simpleType = false;
			foreach (object key2 in dictionary.Keys)
			{
				TryCoerce(key2, out simpleType);
				if (!simpleType)
				{
					break;
				}
				TryCoerce(dictionary[key2], out simpleType);
				if (!simpleType)
				{
					break;
				}
			}
			if (simpleType)
			{
				foreach (object key3 in dictionary.Keys)
				{
					bool simpleType2;
					string key = TryCoerce(key3, out simpleType2).ToString();
					object obj2 = (tag[key] = TryCoerce(dictionary[key3], out simpleType2));
				}
				return;
			}
			System.Type[] genericArguments = dictionary.GetType().GetGenericArguments();
			System.Type type = genericArguments[0];
			System.Type type2 = genericArguments[1];
			foreach (object key4 in dictionary.Keys)
			{
				object obj3 = dictionary[key4];
				bool simpleType3;
				object value = TryCoerce(key4, out simpleType3);
				string name = LowerCamelCase(customItemName ?? type2.Name);
				Tag tag2 = new Tag(name);
				tag.AddChild(tag2);
				if (simpleType3)
				{
					tag2["key"] = value;
					SerializeInternal(obj3, type2, tag2);
				}
				else
				{
					SerializeChild("key", key4, type, simpleType: false, DefaultAttribute, tag2);
					bool simpleType4;
					object value2 = TryCoerce(obj3, out simpleType4);
					SerializeChild("value", value2, type2, simpleType4, DefaultAttribute, tag2);
				}
			}
		}

		private static void SerializeCollection(Tag tag, IEnumerable collection, string customItemName)
		{
			List<object> list = new List<object>();
			System.Type type = collection.GetType();
			System.Type type2 = (type.IsArray ? type.GetElementType() : type.GetGenericArguments()[0]);
			bool simpleType = false;
			foreach (object item2 in collection)
			{
				object item = TryCoerce(item2, out simpleType);
				if (!simpleType)
				{
					break;
				}
				list.Add(item);
			}
			if (simpleType)
			{
				foreach (object item3 in list)
				{
					tag.AddValue(item3);
				}
				return;
			}
			foreach (object item4 in collection)
			{
				string name = LowerCamelCase(customItemName ?? type2.Name);
				bool simpleType2;
				object value = TryCoerce(item4, out simpleType2);
				SerializeChild(name, value, type2, simpleType2, DefaultAttribute, tag);
			}
		}

		private static void AddValueList(Tag tag, params object[] list)
		{
			AddValueList(tag, (IList)list);
		}

		private static void AddValueList(Tag tag, IList list)
		{
			foreach (object item in list)
			{
				tag.AddValue(item);
			}
		}

		public static T Deserialize<T>(StreamReader reader)
		{
			Tag tag = new Tag("content").Read(reader);
			tag = tag.Children[0];
			return (T)DeserializeInternal(null, tag, null);
		}

		public static T Deserialize<T>(string filePath)
		{
			Tag tag = new Tag("content").ReadFile(filePath);
			tag = tag.Children[0];
			return (T)DeserializeInternal(null, tag, null);
		}

		private static object DeserializeInternal(System.Type declaredType, Tag tag, object existingInstance)
		{
			System.Type type = declaredType;
			object obj = existingInstance;
			string text = tag["type"] as string;
			if (Compiling && text == "FezEngine.Structure.TrileSet, FezEngine")
			{
				text = "FezContentPipeline.Content.TrileSetContent, FezContentPipeline";
			}
			if (Compiling && text == "FezEngine.Structure.ArtObject, FezEngine")
			{
				text = "FezContentPipeline.Content.ArtObjectContent, FezContentPipeline";
			}
			if (Compiling && text != null)
			{
				text = text.Replace(", FezEngine", ", FezContentPipeline");
			}
			if (text != null)
			{
				type = System.Type.GetType(text);
			}
			if ((obj == null || declaredType != type) && !IsCoercible(type))
			{
				obj = ReflectionHelper.Instantiate(type);
			}
			TypeSerializationAttribute typeSerializationAttribute = ReflectionHelper.GetFirstAttribute<TypeSerializationAttribute>(type) ?? new TypeSerializationAttribute();
			int num = 0;
			MemberInfo[] serializableMembers = ReflectionHelper.GetSerializableMembers(type);
			foreach (MemberInfo memberInfo in serializableMembers)
			{
				SerializationAttribute serializationAttribute = ReflectionHelper.GetFirstAttribute<SerializationAttribute>(memberInfo) ?? new SerializationAttribute();
				if (serializationAttribute.Ignore)
				{
					continue;
				}
				bool valueFound = true;
				object value = null;
				if (typeSerializationAttribute.FlattenToList)
				{
					value = tag[num++];
				}
				else
				{
					System.Type memberType = ReflectionHelper.GetMemberType(memberInfo);
					string text2 = LowerCamelCase(serializationAttribute.Name ?? memberInfo.Name);
					if (serializationAttribute.UseAttribute)
					{
						if (tag.Attributes.ContainsKey(text2))
						{
							value = DeCoerce(tag[text2], memberType);
						}
						else
						{
							if (!IgnoreMissing && !serializationAttribute.Optional)
							{
								throw new SdlSerializationException(Resources.MissingNonOptionalTagOrAttribute, type, memberInfo);
							}
							valueFound = false;
						}
					}
					else
					{
						object value2 = ReflectionHelper.GetValue(memberInfo, obj);
						bool simpleType = IsCoercible(memberType);
						value = DeserializeChild(text2, value2, memberType, simpleType, serializationAttribute, tag, out valueFound);
					}
				}
				if (valueFound)
				{
					ReflectionHelper.SetValue(memberInfo, obj, value);
				}
			}
			if (obj is IDeserializationCallback)
			{
				(obj as IDeserializationCallback).OnDeserialization();
			}
			return obj;
		}

		private static bool IsCoercible(System.Type type)
		{
			return SDLUtil.IsCoercible(type) || type.IsEnum || ReflectionHelper.IsNullable(type);
		}

		private static object DeserializeChild(string name, object existingInstance, System.Type type, bool simpleType, SerializationAttribute attr, Tag tag, out bool valueFound)
		{
			Tag child = tag.GetChild(name);
			if (child == null)
			{
				if (IgnoreMissing || attr.Optional)
				{
					valueFound = false;
					return existingInstance;
				}
				throw new SdlSerializationException(Resources.MissingNonOptionalTagOrAttribute, type, name);
			}
			valueFound = true;
			return DeserializeChild(child, type, existingInstance, simpleType);
		}

		private static object DeserializeChild(Tag childTag, System.Type type, object existingInstance, bool simpleType)
		{
			string text = childTag["type"] as string;
			if (text != null)
			{
				if (Compiling)
				{
					text = text.Replace(", FezEngine", ", FezContentPipeline");
				}
				type = System.Type.GetType(text);
				simpleType = IsCoercible(type);
			}
			object obj = existingInstance;
			if (IsNull(childTag))
			{
				obj = null;
			}
			else if (simpleType)
			{
				obj = DeCoerce(childTag.Value, type);
			}
			else if (type.Name == "Color")
			{
				obj = ((childTag.Values.Count == 4) ? new Color((byte)(int)childTag.Values[0], (byte)(int)childTag.Values[1], (byte)(int)childTag.Values[2], (byte)(int)childTag.Values[3]) : new Color((byte)(int)childTag.Values[0], (byte)(int)childTag.Values[1], (byte)(int)childTag.Values[2]));
			}
			else if (type.Name == "Vector2")
			{
				obj = new Vector2((float)childTag.Values[0], (float)childTag.Values[1]);
			}
			else if (type.Name == "Vector3")
			{
				obj = new Vector3(Convert.ToSingle(childTag.Values[0]), Convert.ToSingle(childTag.Values[1]), Convert.ToSingle(childTag.Values[2]));
			}
			else if (type.Name == "Vector4")
			{
				obj = new Vector4((float)childTag.Values[0], (float)childTag.Values[1], (float)childTag.Values[2], (float)childTag.Values[3]);
			}
			else if (type.Name == "Quaternion")
			{
				obj = new Quaternion((float)childTag.Values[0], (float)childTag.Values[1], (float)childTag.Values[2], (float)childTag.Values[3]);
			}
			else if (type.Name == "Matrix")
			{
				obj = DeserializeMatrix(childTag);
			}
			else if (!ReflectionHelper.IsGenericDictionary(type))
			{
				obj = ((!ReflectionHelper.IsGenericCollection(type)) ? DeserializeInternal(type, childTag, obj) : DeserializeCollection(childTag, obj as IEnumerable, type));
			}
			else
			{
				DeserializeDictionary(childTag, obj as IDictionary, type);
			}
			return obj;
		}

		private static bool IsNull(Tag tag)
		{
			bool flag = false;
			foreach (string key in tag.Attributes.Keys)
			{
				if (key != "type")
				{
					flag = true;
					break;
				}
			}
			return tag.Values.Count == 1 && tag.Children.Count == 0 && tag.Value == null && !flag;
		}

		private static object DeCoerce(object coerced, System.Type type)
		{
			if (coerced == null)
			{
				return null;
			}
			if (type.IsInstanceOfType(coerced))
			{
				return coerced;
			}
			if (type.IsEnum && coerced is string)
			{
				return Enum.Parse(type, coerced as string, ignoreCase: false);
			}
			if (ReflectionHelper.IsNullable(type))
			{
				object obj = Activator.CreateInstance(type);
				PropertyInfo property = type.GetProperty("Value");
				ReflectionHelper.SetValue(property, obj, coerced);
				return obj;
			}
			throw new NotImplementedException();
		}

		private static Matrix DeserializeMatrix(Tag tag)
		{
			Matrix result = default(Matrix);
			Tag tag2 = tag.Children[0];
			result.M11 = (float)tag2.Values[0];
			result.M12 = (float)tag2.Values[1];
			result.M13 = (float)tag2.Values[2];
			result.M14 = (float)tag2.Values[3];
			tag2 = tag.Children[1];
			result.M21 = (float)tag2.Values[0];
			result.M22 = (float)tag2.Values[1];
			result.M23 = (float)tag2.Values[2];
			result.M24 = (float)tag2.Values[3];
			tag2 = tag.Children[2];
			result.M31 = (float)tag2.Values[0];
			result.M32 = (float)tag2.Values[1];
			result.M33 = (float)tag2.Values[2];
			result.M34 = (float)tag2.Values[3];
			tag2 = tag.Children[3];
			result.M41 = (float)tag2.Values[0];
			result.M42 = (float)tag2.Values[1];
			result.M43 = (float)tag2.Values[2];
			result.M44 = (float)tag2.Values[3];
			return result;
		}

		private static void DeserializeDictionary(Tag tag, IDictionary dictionary, System.Type declaredType)
		{
			bool flag = tag.Children.Count == 0;
			if (dictionary == null)
			{
				dictionary = ReflectionHelper.Instantiate(declaredType) as IDictionary;
			}
			System.Type[] genericArguments = dictionary.GetType().GetGenericArguments();
			System.Type type = genericArguments[0];
			System.Type type2 = genericArguments[1];
			if (flag)
			{
				foreach (string key4 in tag.Attributes.Keys)
				{
					object key = DeCoerce(key4, type);
					object value = DeCoerce(tag[key4], type2);
					SafeAddToDictionary(dictionary, key, value);
				}
				return;
			}
			foreach (Tag child2 in tag.Children)
			{
				object obj = child2["key"];
				Tag child = child2.GetChild("key");
				if (obj != null == (child != null))
				{
					throw new SdlSerializationException(Resources.IllegalCollectionStructure, dictionary.GetType(), tag.Name);
				}
				if (obj != null)
				{
					object key2 = DeCoerce(obj, type);
					object value2 = DeserializeInternal(type2, child2, null);
					SafeAddToDictionary(dictionary, key2, value2);
				}
				else
				{
					object key3 = DeserializeInternal(type, child, null);
					bool simpleType = IsCoercible(type2);
					bool valueFound;
					object value3 = DeserializeChild("value", null, type2, simpleType, DefaultAttribute, child2, out valueFound);
					SafeAddToDictionary(dictionary, key3, value3);
				}
			}
		}

		private static void SafeAddToDictionary(IDictionary dictionary, object key, object value)
		{
			if (dictionary.Contains(key))
			{
				dictionary.Remove(key);
			}
			dictionary.Add(key, value);
		}

		private static IEnumerable DeserializeCollection(Tag tag, IEnumerable collection, System.Type declaredType)
		{
			bool flag = tag.Values.Count > 0;
			bool flag2 = tag.Children.Count > 0;
			if (collection == null)
			{
				collection = ((!declaredType.IsArray) ? (ReflectionHelper.Instantiate(declaredType) as IEnumerable) : Array.CreateInstance(declaredType.GetElementType(), 0));
			}
			if (!flag && !flag2)
			{
				return collection;
			}
			System.Type type = collection.GetType();
			bool flag3 = collection is IList;
			bool isArray = type.IsArray;
			bool flag4 = ReflectionHelper.IsGenericSet(type);
			System.Type type2 = (isArray ? type.GetElementType() : type.GetGenericArguments()[0]);
			bool simpleType = IsCoercible(type2);
			DynamicMethodDelegate dynamicMethodDelegate = null;
			if (flag4)
			{
				dynamicMethodDelegate = ReflectionHelper.GetDelegate(type.GetMethod("Add"));
			}
			if (flag && flag2)
			{
				throw new SdlSerializationException(Resources.IllegalCollectionStructure, type, tag.Name);
			}
			if (isArray)
			{
				int length = (flag ? tag.Values.Count : tag.Children.Count);
				collection = Array.CreateInstance(type2, length);
			}
			int num = 0;
			if (flag)
			{
				foreach (object value in tag.Values)
				{
					object obj = DeCoerce(value, type2);
					if (isArray)
					{
						((IList)collection)[num++] = obj;
						continue;
					}
					if (flag4)
					{
						dynamicMethodDelegate(collection, obj);
						continue;
					}
					if (flag3)
					{
						((IList)collection).Add(obj);
						continue;
					}
					throw new NotImplementedException();
				}
			}
			else
			{
				foreach (Tag child in tag.Children)
				{
					object obj2 = (IsNull(child) ? null : DeserializeChild(child, type2, null, simpleType));
					if (isArray)
					{
						((IList)collection)[num++] = obj2;
						continue;
					}
					if (flag4)
					{
						dynamicMethodDelegate(collection, obj2);
						continue;
					}
					if (flag3)
					{
						((IList)collection).Add(obj2);
						continue;
					}
					throw new NotImplementedException();
				}
			}
			return collection;
		}
	}
}
