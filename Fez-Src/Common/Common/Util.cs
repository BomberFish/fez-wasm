using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;
using SDL2;

namespace Common
{
	public static class Util
	{
		public static readonly string LocalSaveFolder = GetLocalSaveFolder();

		public static readonly string LocalConfigFolder = GetLocalConfigFolder();

		private static string GetLocalSaveFolder()
		{
			// string text = SDL.SDL_GetPlatform();
			string text3;
			// if (text.Equals("Linux"))
			// {
				string text2 = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
				if (string.IsNullOrEmpty(text2))
				{
					text2 = Environment.GetEnvironmentVariable("HOME");
					if (string.IsNullOrEmpty(text2))
					{
						return ".";
					}
					text2 += "/.local/share";
				}
				text3 = Path.Combine(text2, "FEZ");
			// }
			// else if (!text.Equals("Mac OS X"))
			// {
			// 	text3 = ((!text.Equals("Windows")) ? SDL.SDL_GetPrefPath(null, "FEZ") : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FEZ"));
			// }
			// else
			// {
			// 	string environmentVariable = Environment.GetEnvironmentVariable("HOME");
			// 	if (string.IsNullOrEmpty(environmentVariable))
			// 	{
			// 		return ".";
			// 	}
			// 	environmentVariable += "/Library/Application Support";
			// 	text3 = Path.Combine(environmentVariable, "FEZ");
			// }
			if (!Directory.Exists(text3))
			{
				Directory.CreateDirectory(text3);
			}
			return text3;
		}

		private static string GetLocalConfigFolder()
		{
			// string text = SDL.SDL_GetPlatform();
			string text3;
			// if (text.Equals("Linux"))
			// {
				string text2 = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
				if (string.IsNullOrEmpty(text2))
				{
					text2 = Environment.GetEnvironmentVariable("HOME");
					if (string.IsNullOrEmpty(text2))
					{
						return ".";
					}
					text2 += "/.config";
				}
				text3 = Path.Combine(text2, "FEZ");
			// }
			// else if (!text.Equals("Mac OS X"))
			// {
			// 	text3 = ((!text.Equals("Windows")) ? SDL.SDL_GetPrefPath(null, "FEZ") : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FEZ"));
			// }
			// else
			// {
			// 	string environmentVariable = Environment.GetEnvironmentVariable("HOME");
			// 	if (string.IsNullOrEmpty(environmentVariable))
			// 	{
			// 		return ".";
			// 	}
			// 	environmentVariable += "/Library/Application Support";
			// 	text3 = Path.Combine(environmentVariable, "FEZ");
			// }
			if (!Directory.Exists(text3))
			{
				Directory.CreateDirectory(text3);
			}
			return text3;
		}

		private unsafe static void Hash(byte* d, int len, ref uint h)
		{
			for (int i = 0; i < len; i++)
			{
				h += d[i];
				h += h << 10;
				h ^= h >> 6;
			}
		}

		public unsafe static void Hash(ref uint h, string s)
		{
			fixed (char* ptr = s)
			{
				byte* d = (byte*)ptr;
				Hash(d, s.Length * 2, ref h);
			}
		}

		public unsafe static void Hash(ref uint h, int data)
		{
			byte* d = (byte*)(&data);
			Hash(d, 4, ref h);
		}

		public unsafe static void Hash(ref uint h, long data)
		{
			byte* d = (byte*)(&data);
			Hash(d, 8, ref h);
		}

		public unsafe static void Hash(ref uint h, bool data)
		{
			byte* d = (byte*)(&data);
			Hash(d, 1, ref h);
		}

		public unsafe static void Hash(ref uint h, float data)
		{
			byte* d = (byte*)(&data);
			Hash(d, 4, ref h);
		}

		public static int Avalanche(uint h)
		{
			h += h << 3;
			h ^= h >> 11;
			h += h << 15;
			return (int)h;
		}

		public static bool ArrayEquals<T>(T[] a, T[] b) where T : struct
		{
			if (a == null != (b == null))
			{
				return false;
			}
			if (a == null)
			{
				return true;
			}
			if (a.Length != b.Length)
			{
				return false;
			}
			for (int i = 0; i < a.Length; i++)
			{
				if (!b[i].Equals(a[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static bool ContainsIgnoreCase(this string source, string value)
		{
			int num = source.IndexOf(value, StringComparison.CurrentCultureIgnoreCase);
			return num != -1;
		}

		public static int CombineHashCodes(int first, int second, int third, int fourth)
		{
			uint h = 0u;
			Hash(ref h, first);
			Hash(ref h, second);
			Hash(ref h, third);
			Hash(ref h, fourth);
			return Avalanche(h);
		}

		public static int CombineHashCodes(int first, int second, int third)
		{
			uint h = 0u;
			Hash(ref h, first);
			Hash(ref h, second);
			Hash(ref h, third);
			return Avalanche(h);
		}

		public static int CombineHashCodes(int first, int second)
		{
			uint h = 0u;
			Hash(ref h, first);
			Hash(ref h, second);
			return Avalanche(h);
		}

		public static int CombineHashCodes(params object[] keys)
		{
			uint h = 0u;
			for (int i = 0; i < keys.Length; i++)
			{
				Hash(ref h, keys[i]?.GetHashCode() ?? 0);
			}
			return Avalanche(h);
		}

		public static string DeepToString<T>(IEnumerable<T> collection)
		{
			return DeepToString(collection, omitBrackets: false);
		}

		public static string DeepToString<T>(IEnumerable<T> collection, bool omitBrackets)
		{
			StringBuilder stringBuilder = new StringBuilder(omitBrackets ? string.Empty : "{");
			foreach (T item in collection)
			{
				stringBuilder.Append((item == null) ? string.Empty : item.ToString());
				stringBuilder.Append(", ");
			}
			if (stringBuilder.Length >= 2)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}
			if (!omitBrackets)
			{
				stringBuilder.Append("}");
			}
			return stringBuilder.ToString();
		}

		public static string ReflectToString(object obj)
		{
			StringBuilder stringBuilder = new StringBuilder("{");
			MemberInfo[] serializableMembers = ReflectionHelper.GetSerializableMembers(obj.GetType());
			for (int i = 0; i < serializableMembers.Length; i++)
			{
				MemberInfo memberInfo = serializableMembers[i];
				stringBuilder.AppendFormat("{0}:{1}", memberInfo.Name, ReflectionHelper.GetValue(memberInfo, obj));
				if (i != serializableMembers.Length - 1)
				{
					stringBuilder.Append(", ");
				}
			}
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		public static string CompactToString(this Matrix matrix)
		{
			return $"{matrix.M11:0.##} {matrix.M12:0.##} {matrix.M13:0.##} {matrix.M14:0.##} | {matrix.M21:0.##} {matrix.M22:0.##} {matrix.M23:0.##} {matrix.M24:0.##} | {matrix.M31:0.##} {matrix.M32:0.##} {matrix.M33:0.##} {matrix.M34:0.##} | {matrix.M41:0.##} {matrix.M42:0.##} {matrix.M43:0.##} {matrix.M44:0.##}";
		}

		public static T[] JoinArrays<T>(T[] first, T[] second)
		{
			T[] array = new T[first.Length + second.Length];
			Array.Copy(first, array, first.Length);
			Array.Copy(second, 0, array, first.Length, second.Length);
			return array;
		}

		public static T[] AppendToArray<T>(T[] array, T element)
		{
			T[] array2 = new T[array.Length + 1];
			Array.Copy(array, array2, array.Length);
			array2[array.Length] = element;
			return array2;
		}

		public static string StripExtensions(string path)
		{
			int startIndex = path.LastIndexOf('\\') + 1;
			return path.Substring(0, path.IndexOf('.', startIndex));
		}

		public static string GetFileNameWithoutAnyExtension(string path)
		{
			int num = path.LastIndexOf('\\') + 1;
			return path.Substring(num, path.IndexOf('.', num) - num);
		}

		public static string AllExtensions(this FileInfo file)
		{
			int startIndex = file.FullName.LastIndexOf('\\');
			if (file.FullName.IndexOf('.', startIndex) == -1)
			{
				return "";
			}
			return file.FullName.Substring(file.FullName.IndexOf('.', startIndex));
		}

		public static Array GetValues(Type t)
		{
			return Enum.GetValues(t);
		}

		public static IEnumerable<T> GetValues<T>()
		{
			return Enum.GetValues(typeof(T)).Cast<T>();
		}

		public static IEnumerable<string> GetNames<T>()
		{
			return Enum.GetNames(typeof(T));
		}

		public static string GetName<T>(object value)
		{
			return GetName(typeof(T), value);
		}

		public static string GetName(Type t, object value)
		{
			return Enum.GetName(t, value);
		}

		public static bool Implements(this Type type, Type interfaceType)
		{
			return type.GetInterfaces().Contains(interfaceType);
		}

		public static Color FromName(string name)
		{
			Color result = Color.White;
			switch (name.ToUpper(CultureInfo.InvariantCulture))
			{
			case "WHITE":
				result = Color.White;
				break;
			case "BLACK":
				result = Color.Black;
				break;
			case "RED":
				result = Color.Red;
				break;
			case "GREEN":
				result = Color.Green;
				break;
			case "BLUE":
				result = Color.Blue;
				break;
			case "CYAN":
				result = Color.Cyan;
				break;
			case "MAGENTA":
				result = Color.Magenta;
				break;
			case "YELLOW":
				result = Color.Yellow;
				break;
			}
			return result;
		}

		public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
		{
			int num = Math.Max(color.R, Math.Max(color.G, color.B));
			int num2 = Math.Min(color.R, Math.Min(color.G, color.B));
			hue = color.GetHue();
			saturation = ((num == 0) ? 0.0 : (1.0 - 1.0 * (double)num2 / (double)num));
			value = (double)num / 255.0;
		}

		public static float GetHue(this Color color)
		{
			if (color.R == color.G && color.G == color.B)
			{
				return 0f;
			}
			float num = (float)(int)color.R / 255f;
			float num2 = (float)(int)color.G / 255f;
			float num3 = (float)(int)color.B / 255f;
			float num4 = 0f;
			float num5 = num;
			float num6 = num;
			if (num2 > num5)
			{
				num5 = num2;
			}
			if (num3 > num5)
			{
				num5 = num3;
			}
			if (num2 < num6)
			{
				num6 = num2;
			}
			if (num3 < num6)
			{
				num6 = num3;
			}
			float num7 = num5 - num6;
			if (num == num5)
			{
				num4 = (num2 - num3) / num7;
			}
			else if (num2 == num5)
			{
				num4 = 2f + (num3 - num) / num7;
			}
			else if (num3 == num5)
			{
				num4 = 4f + (num - num2) / num7;
			}
			num4 *= 60f;
			if (num4 < 0f)
			{
				num4 += 360f;
			}
			return num4;
		}

		public static Color ColorFromHSV(double hue, double saturation, double value)
		{
			int num = (int)(hue / 60.0) % 6;
			double num2 = hue / 60.0 - (double)(int)(hue / 60.0);
			value *= 255.0;
			byte b = (byte)value;
			byte b2 = (byte)(value * (1.0 - saturation));
			byte b3 = (byte)(value * (1.0 - num2 * saturation));
			byte b4 = (byte)(value * (1.0 - (1.0 - num2) * saturation));
			return num switch
			{
				0 => new Color(b, b4, b2), 
				1 => new Color(b3, b, b2), 
				2 => new Color(b2, b, b4), 
				3 => new Color(b2, b3, b), 
				4 => new Color(b4, b2, b), 
				_ => new Color(b, b2, b3), 
			};
		}

		public static string StripPunctuation(this string s)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in s)
			{
				if (!char.IsPunctuation(c))
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		public static void NullAction()
		{
		}

		public static void NullAction<T>(T t)
		{
		}

		public static void NullAction<T, U>(T t, U u)
		{
		}

		public static void NullAction<T, U, V>(T t, U u, V v)
		{
		}

		public static void NullAction<T, U, V, W>(T t, U u, V v, W w)
		{
		}

		public static TResult NullFunc<TResult>()
		{
			return default(TResult);
		}

		public static TResult NullFunc<T, TResult>(T t)
		{
			return default(TResult);
		}

		public static TResult NullFunc<T, U, TResult>(T t, U u)
		{
			return default(TResult);
		}

		public static TResult NullFunc<T, U, V, TResult>(T t, U u, V v)
		{
			return default(TResult);
		}

		public static TResult NullFunc<T, U, V, W, TResult>(T t, U u, V v, W w)
		{
			return default(TResult);
		}
	}
}
