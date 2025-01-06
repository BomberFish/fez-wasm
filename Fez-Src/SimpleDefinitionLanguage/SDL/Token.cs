using System;

namespace SDL
{
	internal class Token
	{
		private const string NumericalChars = "01234567890-.";

		internal readonly Parser parser;

		internal readonly Type type;

		internal readonly string text;

		internal readonly int line;

		internal readonly int position;

		internal readonly int size;

		internal readonly object obj;

		internal readonly bool punctuation;

		internal readonly bool literal;

		internal Token(Parser parser, string text, int line, int position)
		{
			this.parser = parser;
			this.text = text;
			this.line = line;
			this.position = position;
			size = text.Length;
			obj = null;
			try
			{
				char c = text[0];
				if (c == '"' || c == '`')
				{
					type = Type.STRING;
					obj = Parser.ParseString(text);
				}
				else if (c == '\'')
				{
					type = Type.CHARACTER;
					obj = text[1];
				}
				else if (text == "null")
				{
					type = Type.NULL;
				}
				else if (text == "true" || text == "on")
				{
					type = Type.BOOLEAN;
					obj = true;
				}
				else if (text == "false" || text == "off")
				{
					type = Type.BOOLEAN;
					obj = false;
				}
				else
				{
					switch (c)
					{
					case '[':
						type = Type.BINARY;
						obj = Parser.ParseBinary(text);
						goto end_IL_0038;
					default:
						if (text.IndexOf('/') == -1 || text.IndexOf(':') != -1)
						{
							break;
						}
						type = Type.DATE;
						obj = Parser.ParseDateTime(text);
						goto end_IL_0038;
					case '/':
						break;
					}
					if (c != ':' && text.IndexOf(':') != -1)
					{
						type = Type.TIME;
						obj = ParseTimeSpanWithZone(text, parser, line, position);
					}
					else if ("01234567890-.".IndexOf(c) != -1)
					{
						type = Type.NUMBER;
						obj = Parser.ParseNumber(text);
					}
					else
					{
						switch (c)
						{
						case '{':
							type = Type.START_BLOCK;
							break;
						case '}':
							type = Type.END_BLOCK;
							break;
						case '=':
							type = Type.EQUALS;
							break;
						case ':':
							type = Type.COLON;
							break;
						default:
							type = Type.IDENTIFIER;
							break;
						}
					}
				}
				end_IL_0038:;
			}
			catch (FormatException ex)
			{
				throw new SDLParseException(ex.Message, line, position);
			}
			punctuation = type == Type.COLON || type == Type.EQUALS || type == Type.START_BLOCK || type == Type.END_BLOCK;
			literal = type != 0 && !punctuation;
		}

		internal object GetObjectForLiteral()
		{
			return obj;
		}

		public override string ToString()
		{
			string[] obj = new string[5]
			{
				type.ToString(),
				" ",
				text,
				" pos:",
				null
			};
			int num = position;
			obj[4] = num.ToString();
			return string.Concat(obj);
		}

		internal static TimeSpanWithZone ParseTimeSpanWithZone(string text, Parser parser, int line, int position)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			string text2 = null;
			string text3 = text;
			int num6 = text3.IndexOf('-', 1);
			if (num6 != -1)
			{
				text2 = text3.Substring(num6 + 1);
				text3 = text.Substring(0, num6);
			}
			string[] array = text3.Split(new char[1] { ':' });
			if (text2 != null)
			{
				if (array.Length < 2 || array.Length > 3)
				{
					parser.ParseException("date/time format exception.  Must use hh:mm(:ss)(.xxx)(-z)", line, position);
				}
			}
			else if (array.Length < 2 || array.Length > 4)
			{
				parser.ParseException("Time format exception.  For time spans use (d:)hh:mm:ss(.xxx) and for the time component of a date/time type use hh:mm(:ss)(.xxx)(-z)  If you use the day component of a time span make sure to prefix it with a lower case d", line, position);
			}
			try
			{
				if (array.Length == 4)
				{
					string text4 = array[0];
					if (!text4.EndsWith("d"))
					{
						parser.ParseException("The day component of a time span must end with a lower case d", line, position);
					}
					num = Convert.ToInt32(text4.Substring(0, text4.Length - 1));
					num2 = Convert.ToInt32(array[1]);
					num3 = Convert.ToInt32(array[2]);
					if (array.Length == 4)
					{
						string text5 = array[3];
						int num7 = text5.IndexOf('.');
						if (num7 == -1)
						{
							num4 = Convert.ToInt32(text5);
						}
						else
						{
							num4 = Convert.ToInt32(text5.Substring(0, num7));
							string text6 = text5.Substring(num7 + 1);
							if (text6.Length == 1)
							{
								text6 += "00";
							}
							else if (text6.Length == 2)
							{
								text6 += "0";
							}
							num5 = Convert.ToInt32(text6);
						}
					}
					if (num < 0)
					{
						num2 = Parser.ReverseIfPositive(num2);
						num3 = Parser.ReverseIfPositive(num3);
						num4 = Parser.ReverseIfPositive(num4);
						num5 = Parser.ReverseIfPositive(num5);
					}
				}
				else
				{
					num2 = Convert.ToInt32(array[0]);
					num3 = Convert.ToInt32(array[1]);
					if (array.Length == 3)
					{
						string text7 = array[2];
						int num8 = text7.IndexOf(".");
						if (num8 == -1)
						{
							num4 = Convert.ToInt32(text7);
						}
						else
						{
							num4 = Convert.ToInt32(text7.Substring(0, num8));
							string text8 = text7.Substring(num8 + 1);
							if (text8.Length == 1)
							{
								text8 += "00";
							}
							else if (text8.Length == 2)
							{
								text8 += "0";
							}
							num5 = Convert.ToInt32(text8);
						}
					}
					if (num2 < 0)
					{
						num3 = Parser.ReverseIfPositive(num3);
						num4 = Parser.ReverseIfPositive(num4);
						num5 = Parser.ReverseIfPositive(num5);
					}
				}
			}
			catch (FormatException ex)
			{
				parser.ParseException("Time format: " + ex.Message, line, position);
			}
			return new TimeSpanWithZone(num, num2, num3, num4, num5, text2);
		}
	}
}
