using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SDL
{
	public class SDLUtil
	{
		public const string TIME_FORMAT = "HH:mm:ss.fff-z";

		public const string DATE_FORMAT = "yyyy/MM/dd";

		public const string DATE_TIME_FORMAT = "yyyy/MM/dd HH:mm:ss.fff-z";

		public static string ValidateIdentifier(string identifier)
		{
			if (identifier == null || identifier.Length == 0)
			{
				throw new ArgumentException("SDL identifiers cannot be null or empty.");
			}
			if (!char.IsLetter(identifier[0]) && identifier[0] != '_')
			{
				throw new ArgumentException("'" + identifier[0] + "' is not a legal first character for an SDL identifier. SDL Identifiers must start with a unicode letter or underscore (_).");
			}
			int length = identifier.Length;
			for (int i = 1; i < length; i++)
			{
				if (!char.IsLetterOrDigit(identifier[i]) && identifier[i] != '_' && identifier[i] != '-' && identifier[i] != '.')
				{
					throw new ArgumentException("'" + identifier[i] + "' is not a legal character for an SDL identifier. SDL Identifiers must start with a unicode letter or underscore (_) followed by zero or more unicode letters, digits, dashes (-) or underscores (_)");
				}
			}
			return identifier;
		}

		public static string Format(object obj)
		{
			return Format(obj, addQuotes: true);
		}

		public static string Format(object obj, bool addQuotes)
		{
			if (obj == null)
			{
				return "null";
			}
			if (obj is int num)
			{
				return num.ToString(CultureInfo.InvariantCulture);
			}
			if (obj is string)
			{
				if (addQuotes)
				{
					return "\"" + Escape((string)obj) + "\"";
				}
				return Escape((string)obj);
			}
			if (obj is char)
			{
				if (addQuotes)
				{
					return "'" + Escape((char)obj) + "'";
				}
				return Escape((char)obj);
			}
			if (obj is decimal num2)
			{
				return num2.ToString(NumberFormatInfo.InvariantInfo) + "BD";
			}
			if (obj is float num3)
			{
				return num3.ToString("0.######", NumberFormatInfo.InvariantInfo) + "F";
			}
			if (obj is long num4)
			{
				return num4.ToString(NumberFormatInfo.InvariantInfo) + "L";
			}
			if (obj is byte[])
			{
				return "[" + Convert.ToBase64String((byte[])obj) + "]";
			}
			if (obj is bool)
			{
				return obj.Equals(true) ? "true" : "false";
			}
			if (obj is TimeSpan timeSpan)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (timeSpan.Days != 0)
				{
					stringBuilder.Append(timeSpan.Days).Append("d:");
					string text = Math.Abs(timeSpan.Hours).ToString() ?? "";
					if (text.Length == 1)
					{
						text = "0" + text;
					}
					stringBuilder.Append(text);
				}
				else
				{
					if (timeSpan.Hours < 0)
					{
						stringBuilder.Append('-');
					}
					string text2 = Math.Abs(timeSpan.Hours).ToString() ?? "";
					if (text2.Length == 1)
					{
						text2 = "0" + text2;
					}
					stringBuilder.Append(text2);
				}
				stringBuilder.Append(":");
				string text3 = Math.Abs(timeSpan.Minutes).ToString() ?? "";
				if (text3.Length == 1)
				{
					text3 = "0" + text3;
				}
				stringBuilder.Append(text3);
				stringBuilder.Append(":");
				string text4 = Math.Abs(timeSpan.Seconds).ToString() ?? "";
				if (text4.Length == 1)
				{
					text4 = "0" + text4;
				}
				stringBuilder.Append(text4);
				if (timeSpan.Milliseconds != 0)
				{
					string text5 = Math.Abs(timeSpan.Milliseconds).ToString() ?? "";
					if (text5.Length == 1)
					{
						text5 = "00" + text5;
					}
					else if (text5.Length == 2)
					{
						text5 = "0" + text5;
					}
					stringBuilder.Append(".").Append(text5);
					string text6 = stringBuilder.ToString();
					int num5 = text6.Length - 1;
					while (num5 > -1 && text6[num5] == '0')
					{
						num5--;
					}
					return text6.Substring(0, num5 + 1);
				}
				return stringBuilder.ToString();
			}
			return obj.ToString();
		}

		private static string Escape(string s)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int length = s.Length;
			for (int i = 0; i < length; i++)
			{
				char c = s[i];
				switch (c)
				{
				case '\\':
					stringBuilder.Append("\\\\");
					break;
				case '"':
					stringBuilder.Append("\\\"");
					break;
				case '\t':
					stringBuilder.Append("\\t");
					break;
				case '\r':
					stringBuilder.Append("\\r");
					break;
				case '\n':
					stringBuilder.Append("\\n");
					break;
				default:
					stringBuilder.Append(c);
					break;
				}
			}
			return stringBuilder.ToString();
		}

		private static string Escape(char c)
		{
			return c switch
			{
				'\\' => "\\\\", 
				'\'' => "\\'", 
				'\t' => "\\t", 
				'\r' => "\\r", 
				'\n' => "\\n", 
				_ => c.ToString() ?? "", 
			};
		}

		public static object CoerceOrFail(object obj)
		{
			bool succeeded;
			object result = TryCoerce(obj, out succeeded);
			if (!succeeded)
			{
				throw new ArgumentException(obj.GetType()?.ToString() + " is not coercible to an SDL type.");
			}
			return result;
		}

		public static object TryCoerce(object obj, out bool succeeded)
		{
			succeeded = true;
			if (obj == null)
			{
				return null;
			}
			if (obj is string || obj is char || obj is int || obj is long || obj is float || obj is double || obj is decimal || obj is bool || obj is TimeSpan || obj is SDLDateTime || obj is byte[])
			{
				return obj;
			}
			if (obj is DateTime)
			{
				return new SDLDateTime((DateTime)obj, null);
			}
			if (obj is sbyte || obj is byte || obj is short || obj is ushort)
			{
				return Convert.ToInt32(obj);
			}
			if (obj is uint)
			{
				return Convert.ToInt64(obj);
			}
			succeeded = false;
			return obj;
		}

		public static bool IsCoercible(System.Type type)
		{
			return type != null && type == typeof(string) || type == typeof(char) || type == typeof(int) || type == typeof(long) || type == typeof(float) || type == typeof(double) || type == typeof(decimal) || type == typeof(bool) || type == typeof(TimeSpan) || type == typeof(SDLDateTime) || type == typeof(byte[]) || type == typeof(DateTime) || type == typeof(sbyte) || type == typeof(byte) || type == typeof(short) || type == typeof(ushort) || type == typeof(uint);
		}

		public static object Value(string literal)
		{
			if (literal == null)
			{
				throw new ArgumentNullException("literal argument to SDL.Value(string) cannot be null");
			}
			if (literal.StartsWith("\"") || literal.StartsWith("`"))
			{
				return Parser.ParseString(literal);
			}
			if (literal.StartsWith("'"))
			{
				return Parser.ParseCharacter(literal);
			}
			if (literal.Equals("null"))
			{
				return null;
			}
			if (literal.Equals("true") || literal.Equals("on"))
			{
				return true;
			}
			if (literal.Equals("false") || literal.Equals("off"))
			{
				return false;
			}
			if (literal.StartsWith("["))
			{
				return Parser.ParseBinary(literal);
			}
			if (literal[0] != '/' && literal.IndexOf('/') != -1)
			{
				return Parser.ParseDateTime(literal);
			}
			if (literal[0] != ':' && literal.IndexOf(':') != -1)
			{
				return Parser.ParseTimeSpan(literal);
			}
			if ("01234567890-.".IndexOf(literal[0]) != -1)
			{
				return Parser.ParseNumber(literal);
			}
			throw new FormatException("String " + literal + " does not represent an SDL type.");
		}

		public static IList<object> List(string valueList)
		{
			if (valueList == null)
			{
				throw new ArgumentNullException("valueList cannot be null");
			}
			return new Tag("root").ReadString(valueList).GetChild("content").Values;
		}

		public static IDictionary<string, object> Map(string attributeString)
		{
			if (attributeString == null)
			{
				throw new ArgumentNullException("attributeString cannot be null");
			}
			return new Tag("root").ReadString("atts " + attributeString).GetChild("atts").Attributes;
		}
	}
}
