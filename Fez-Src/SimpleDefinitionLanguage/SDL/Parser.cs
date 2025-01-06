using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace SDL
{
	internal class Parser
	{
		private readonly TextReader reader;

		private string line;

		private List<Token> toks;

		private StringBuilder sb;

		private bool startEscapedQuoteLine;

		internal int lineNumber = -1;

		internal int pos;

		internal int lineLength;

		internal int tokenStart;

		private const string AlphaNumericChars = "0123456789.-+:abcdefghijklmnopqrstuvwxyz";

		private readonly StringBuilder numberSb = new StringBuilder();

		internal Parser(TextReader reader)
		{
			this.reader = reader;
		}

		internal IList<Tag> Parse()
		{
			List<Tag> list = new List<Tag>();
			List<Token> lineTokens;
			while ((lineTokens = GetLineTokens()) != null)
			{
				int count = lineTokens.Count;
				if (lineTokens[count - 1].type == Type.START_BLOCK)
				{
					Tag tag = ConstructTag(lineTokens.GetRange(0, count - 1));
					AddChildren(tag);
					list.Add(tag);
				}
				else if (lineTokens[0].type == Type.END_BLOCK)
				{
					ParseException("No opening block ({) for close block (}).", lineTokens[0].line, lineTokens[0].position);
				}
				else
				{
					list.Add(ConstructTag(lineTokens));
				}
			}
			reader.Close();
			return list;
		}

		private void AddChildren(Tag parent)
		{
			List<Token> lineTokens;
			while ((lineTokens = GetLineTokens()) != null)
			{
				int count = lineTokens.Count;
				if (lineTokens[0].type == Type.END_BLOCK)
				{
					return;
				}
				if (lineTokens[count - 1].type == Type.START_BLOCK)
				{
					Tag tag = ConstructTag(lineTokens.GetRange(0, count - 1));
					AddChildren(tag);
					parent.AddChild(tag);
				}
				else
				{
					parent.AddChild(ConstructTag(lineTokens));
				}
			}
			ParseException("No close block (}).", lineNumber, -2);
		}

		private Tag ConstructTag(List<Token> toks)
		{
			if (toks.Count == 0)
			{
				ParseException("Internal Error: Empty token list", lineNumber, -2);
			}
			Token token = toks[0];
			if (token.literal)
			{
				toks.Insert(0, token = new Token(this, "content", -1, -1));
			}
			else if (token.type != 0)
			{
				ExpectingButGot("IDENTIFIER", token.type.ToString() + " (" + token.text + ")", token.line, token.position);
			}
			int count = toks.Count;
			Tag tag = null;
			if (count == 1)
			{
				tag = new Tag(token.text);
			}
			else
			{
				int tpos = 1;
				Token token2 = toks[1];
				if (token2.type == Type.COLON)
				{
					if (count == 2 || toks[2].type != 0)
					{
						ParseException("Colon (:) encountered in unexpected location.", token2.line, token2.position);
					}
					Token token3 = toks[2];
					tag = new Tag(token.text, token3.text);
					tpos = 3;
				}
				else
				{
					tag = new Tag(token.text);
				}
				int num = AddTagValues(tag, toks, tpos);
				if (num < count)
				{
					AddTagAttributes(tag, toks, num);
				}
			}
			return tag;
		}

		private int AddTagValues(Tag tag, IList<Token> toks, int tpos)
		{
			int count = toks.Count;
			int i;
			for (i = tpos; i < count; i++)
			{
				Token token = toks[i];
				if (token.literal)
				{
					if (token.type == Type.DATE && i + 1 < count && toks[i + 1].type == Type.TIME)
					{
						SDLDateTime sDLDateTime = (SDLDateTime)token.GetObjectForLiteral();
						TimeSpanWithZone timeSpanWithZone = (TimeSpanWithZone)toks[i + 1].GetObjectForLiteral();
						if (timeSpanWithZone.Days != 0)
						{
							tag.AddValue(sDLDateTime);
							tag.AddValue(new TimeSpan(timeSpanWithZone.Days, timeSpanWithZone.Hours, timeSpanWithZone.Minutes, timeSpanWithZone.Seconds, timeSpanWithZone.Milliseconds));
							if (timeSpanWithZone.TimeZone != null)
							{
								ParseException("TimeSpan cannot have a timezone", token.line, token.position);
							}
						}
						else
						{
							tag.AddValue(Combine(sDLDateTime, timeSpanWithZone));
						}
						i++;
						continue;
					}
					object objectForLiteral = token.GetObjectForLiteral();
					if (objectForLiteral is TimeSpanWithZone)
					{
						TimeSpanWithZone timeSpanWithZone2 = (TimeSpanWithZone)objectForLiteral;
						if (timeSpanWithZone2.TimeZone != null)
						{
							ExpectingButGot("TIME SPAN", "TIME (component of date/time)", token.line, token.position);
						}
						tag.AddValue(new TimeSpan(timeSpanWithZone2.Days, timeSpanWithZone2.Hours, timeSpanWithZone2.Minutes, timeSpanWithZone2.Seconds, timeSpanWithZone2.Milliseconds));
					}
					else
					{
						tag.AddValue(objectForLiteral);
					}
				}
				else
				{
					if (token.type == Type.IDENTIFIER)
					{
						break;
					}
					ExpectingButGot("LITERAL or IDENTIFIER", token.type, token.line, token.position);
				}
			}
			return i;
		}

		private void AddTagAttributes(Tag tag, IList<Token> toks, int tpos)
		{
			int num = tpos;
			int count = toks.Count;
			while (num < count)
			{
				Token token = toks[num];
				if (token.type != 0)
				{
					ExpectingButGot("IDENTIFIER", token.type, token.line, token.position);
				}
				string text = token.text;
				if (num == count - 1)
				{
					ExpectingButGot("\":\" or \"=\" \"LITERAL\"", "END OF LINE.", token.line, token.position);
				}
				token = toks[++num];
				if (token.type == Type.COLON)
				{
					if (num == count - 1)
					{
						ExpectingButGot("IDENTIFIER", "END OF LINE", token.line, token.position);
					}
					token = toks[++num];
					if (token.type != 0)
					{
						ExpectingButGot("IDENTIFIER", token.type, token.line, token.position);
					}
					string text2 = token.text;
					if (num == count - 1)
					{
						ExpectingButGot("\"=\"", "END OF LINE", token.line, token.position);
					}
					token = toks[++num];
					if (token.type != Type.EQUALS)
					{
						ExpectingButGot("\"=\"", token.type, token.line, token.position);
					}
					if (num == count - 1)
					{
						ExpectingButGot("LITERAL", "END OF LINE", token.line, token.position);
					}
					token = toks[++num];
					if (!token.literal)
					{
						ExpectingButGot("LITERAL", token.type, token.line, token.position);
					}
					if (token.type == Type.DATE && num + 1 < count && toks[num + 1].type == Type.TIME)
					{
						SDLDateTime dt = (SDLDateTime)token.GetObjectForLiteral();
						TimeSpanWithZone timeSpanWithZone = (TimeSpanWithZone)toks[num + 1].GetObjectForLiteral();
						if (timeSpanWithZone.Days != 0)
						{
							ExpectingButGot("TIME (component of date/time) in attribute value", "TIME SPAN", token.line, token.position);
						}
						tag[text, text2] = Combine(dt, timeSpanWithZone);
						num++;
					}
					else
					{
						object objectForLiteral = token.GetObjectForLiteral();
						if (objectForLiteral is TimeSpanWithZone)
						{
							TimeSpanWithZone timeSpanWithZone2 = (TimeSpanWithZone)objectForLiteral;
							if (timeSpanWithZone2.TimeZone != null)
							{
								ExpectingButGot("TIME SPAN", "TIME (component of date/time)", token.line, token.position);
							}
							TimeSpan timeSpan = new TimeSpan(timeSpanWithZone2.Days, timeSpanWithZone2.Hours, timeSpanWithZone2.Minutes, timeSpanWithZone2.Seconds, timeSpanWithZone2.Milliseconds);
							tag[text, text2] = timeSpan;
						}
						else
						{
							tag[text, text2] = objectForLiteral;
						}
					}
				}
				else if (token.type == Type.EQUALS)
				{
					if (num == count - 1)
					{
						ExpectingButGot("LITERAL", "END OF LINE", token.line, token.position);
					}
					token = toks[++num];
					if (!token.literal)
					{
						ExpectingButGot("LITERAL", token.type, token.line, token.position);
					}
					if (token.type == Type.DATE && num + 1 < count && toks[num + 1].type == Type.TIME)
					{
						SDLDateTime dt2 = (SDLDateTime)token.GetObjectForLiteral();
						TimeSpanWithZone timeSpanWithZone3 = (TimeSpanWithZone)toks[num + 1].GetObjectForLiteral();
						if (timeSpanWithZone3.Days != 0)
						{
							ExpectingButGot("TIME (component of date/time) in attribute value", "TIME SPAN", token.line, token.position);
						}
						tag[text] = Combine(dt2, timeSpanWithZone3);
						num++;
					}
					else
					{
						object objectForLiteral2 = token.GetObjectForLiteral();
						if (objectForLiteral2 is TimeSpanWithZone)
						{
							TimeSpanWithZone timeSpanWithZone4 = (TimeSpanWithZone)objectForLiteral2;
							if (timeSpanWithZone4.TimeZone != null)
							{
								ExpectingButGot("TIME SPAN", "TIME (component of date/time)", token.line, token.position);
							}
							TimeSpan timeSpan2 = new TimeSpan(timeSpanWithZone4.Days, timeSpanWithZone4.Hours, timeSpanWithZone4.Minutes, timeSpanWithZone4.Seconds, timeSpanWithZone4.Milliseconds);
							tag[text] = timeSpan2;
						}
						else
						{
							tag[text] = objectForLiteral2;
						}
					}
				}
				else
				{
					ExpectingButGot("\":\" or \"=\"", token.type, token.line, token.position);
				}
				num++;
			}
		}

		private List<Token> GetLineTokens()
		{
			line = ReadLine();
			if (line == null)
			{
				return null;
			}
			toks = new List<Token>();
			lineLength = line.Length;
			sb = null;
			for (tokenStart = 0; pos < lineLength; pos++)
			{
				char c = line[pos];
				if (sb != null)
				{
					toks.Add(new Token(this, sb.ToString(), lineNumber, tokenStart));
					sb = null;
				}
				switch (c)
				{
				case '"':
					HandleDoubleQuoteString();
					continue;
				case '\'':
					HandleCharacterLiteral();
					continue;
				default:
					if ("{}=:".IndexOf(c) != -1)
					{
						toks.Add(new Token(this, c.ToString() ?? "", lineNumber, pos));
						sb = null;
						continue;
					}
					if (c == '#')
					{
						break;
					}
					if (c == '/')
					{
						if (pos + 1 < lineLength && line[pos + 1] == '/')
						{
							break;
						}
						HandleSlashComment();
						continue;
					}
					if (c == '`')
					{
						HandleBackQuoteString();
						continue;
					}
					if (c == '[')
					{
						HandleBinaryLiteral();
						continue;
					}
					if (c == ' ' || c == '\t')
					{
						while (pos + 1 < lineLength && " \t".IndexOf(line[pos + 1]) != -1)
						{
							pos++;
						}
						continue;
					}
					if (c == '\\')
					{
						HandleLineContinuation();
						continue;
					}
					if ("0123456789-.".IndexOf(c) != -1)
					{
						if (c == '-' && pos + 1 < lineLength && line[pos + 1] == '-')
						{
							break;
						}
						HandleNumberDateOrTimeSpan();
						continue;
					}
					if (char.IsLetter(c) || c == '_')
					{
						HandleIdentifier();
					}
					else
					{
						ParseException("Unexpected character \"" + c + "\".)", lineNumber, pos);
					}
					continue;
				}
				break;
			}
			if (sb != null)
			{
				toks.Add(new Token(this, sb.ToString(), lineNumber, tokenStart));
			}
			while (toks != null && toks.Count == 0)
			{
				toks = GetLineTokens();
			}
			return toks;
		}

		private void AddEscapedCharInString(char c)
		{
			switch (c)
			{
			case '\\':
				sb.Append(c);
				break;
			case '"':
				sb.Append(c);
				break;
			case 'n':
				sb.Append('\n');
				break;
			case 'r':
				sb.Append('\r');
				break;
			case 't':
				sb.Append('\t');
				break;
			default:
				ParseException("Ellegal escape character in string literal: \"" + c + "\".", lineNumber, pos);
				break;
			}
		}

		private void HandleDoubleQuoteString()
		{
			bool flag = false;
			startEscapedQuoteLine = false;
			sb = new StringBuilder("\"");
			pos++;
			while (pos < lineLength)
			{
				char c = line[pos];
				if (" \t".IndexOf(c) == -1 || !startEscapedQuoteLine)
				{
					startEscapedQuoteLine = false;
					if (flag)
					{
						AddEscapedCharInString(c);
						flag = false;
					}
					else if (c == '\\')
					{
						if (pos == lineLength - 1 || (pos + 1 < lineLength && " \t".IndexOf(line[pos + 1]) != -1))
						{
							HandleEscapedDoubleQuotedString();
						}
						else
						{
							flag = true;
						}
					}
					else
					{
						sb.Append(c);
						if (c == '"')
						{
							toks.Add(new Token(this, sb.ToString(), lineNumber, tokenStart));
							sb = null;
							return;
						}
					}
				}
				pos++;
			}
			if (sb != null)
			{
				string text = sb.ToString();
				if (text.Length > 0 && text[0] == '"' && text[text.Length - 1] != '"')
				{
					ParseException("String literal \"" + text + "\" not terminated by end quote.", lineNumber, line.Length);
				}
				else if (text.Length == 1 && text[0] == '"')
				{
					ParseException("Orphan quote (unterminated string)", lineNumber, line.Length);
				}
			}
		}

		private void HandleEscapedDoubleQuotedString()
		{
			if (pos == lineLength - 1)
			{
				line = ReadLine();
				if (line == null)
				{
					ParseException("Escape at end of file.", lineNumber, pos);
				}
				lineLength = line.Length;
				pos = -1;
				startEscapedQuoteLine = true;
				return;
			}
			int i;
			for (i = pos + 1; i < lineLength && " \t".IndexOf(line[i]) != -1; i++)
			{
			}
			if (i == lineLength)
			{
				line = ReadLine();
				if (line == null)
				{
					ParseException("Escape at end of file.", lineNumber, pos);
				}
				lineLength = line.Length;
				pos = -1;
				startEscapedQuoteLine = true;
			}
			else
			{
				ParseException("Malformed string literal - escape followed by whitespace followed by non-whitespace.", lineNumber, pos);
			}
		}

		private void HandleCharacterLiteral()
		{
			if (pos == lineLength - 1)
			{
				ParseException("Got ' at end of line", lineNumber, pos);
			}
			pos++;
			char c = line[pos];
			if (c == '\\')
			{
				if (pos == lineLength - 1)
				{
					ParseException("Got '\\ at end of line", lineNumber, pos);
				}
				pos++;
				char c2 = line[pos];
				if (pos == lineLength - 1)
				{
					ParseException("Got '\\" + c2 + " at end of line", lineNumber, pos);
				}
				switch (c2)
				{
				case '\\':
					toks.Add(new Token(this, "'\\'", lineNumber, pos));
					break;
				case '\'':
					toks.Add(new Token(this, "'''", lineNumber, pos));
					break;
				case 'n':
					toks.Add(new Token(this, "'\n'", lineNumber, pos));
					break;
				case 'r':
					toks.Add(new Token(this, "'\r'", lineNumber, pos));
					break;
				case 't':
					toks.Add(new Token(this, "'\t'", lineNumber, pos));
					break;
				default:
					ParseException("Illegal escape character " + line[pos], lineNumber, pos);
					break;
				}
				pos++;
				if (line[pos] != '\'')
				{
					ExpectingButGot("single quote (')", "\"" + line[pos] + "\"", lineNumber, pos);
				}
			}
			else
			{
				toks.Add(new Token(this, "'" + c + "'", lineNumber, pos));
				if (pos == lineLength - 1)
				{
					ParseException("Got '" + c + " at end of line", lineNumber, pos);
				}
				pos++;
				if (line[pos] != '\'')
				{
					ExpectingButGot("quote (')", "\"" + line[pos] + "\"", lineNumber, pos);
				}
			}
		}

		private void HandleSlashComment()
		{
			if (pos == lineLength - 1)
			{
				ParseException("Got slash (/) at end of line.", lineNumber, pos);
			}
			if (line[pos + 1] == '*')
			{
				int num = line.IndexOf("*/", pos + 1);
				if (num != -1)
				{
					pos = num + 1;
					return;
				}
				do
				{
					line = ReadRawLine();
					if (line == null)
					{
						ParseException("/* comment not terminated.", lineNumber, -2);
					}
					num = line.IndexOf("*/");
				}
				while (num == -1);
				lineLength = line.Length;
				pos = num + 1;
			}
			else if (line[pos + 1] == '/')
			{
				ParseException("Got slash (/) in unexpected location.", lineNumber, pos);
			}
		}

		private void HandleBackQuoteString()
		{
			int num = line.IndexOf("`", pos + 1);
			if (num != -1)
			{
				toks.Add(new Token(this, line.Substring(pos, num + 1 - pos), lineNumber, pos));
				sb = null;
				pos = num;
				return;
			}
			sb = new StringBuilder(line.Substring(pos) + "\n");
			int position = pos;
			while (true)
			{
				line = ReadRawLine();
				if (line == null)
				{
					ParseException("` quote not terminated.", lineNumber, -2);
				}
				num = line.IndexOf('`');
				if (num != -1)
				{
					break;
				}
				sb.Append(line + "\n");
			}
			sb.Append(line.Substring(0, num + 1));
			line = line.Trim();
			lineLength = line.Length;
			pos = num;
			toks.Add(new Token(this, sb.ToString(), lineNumber, position));
			sb = null;
		}

		private void HandleBinaryLiteral()
		{
			int num = line.IndexOf(']', pos + 1);
			if (num != -1)
			{
				toks.Add(new Token(this, line.Substring(pos, num + 1 - pos), lineNumber, pos));
				sb = null;
				pos = num;
				return;
			}
			sb = new StringBuilder(line.Substring(pos) + "\n");
			int position = pos;
			while (true)
			{
				line = ReadRawLine();
				if (line == null)
				{
					ParseException("[base64] binary literal not terminated.", lineNumber, -2);
				}
				num = line.IndexOf(']');
				if (num != -1)
				{
					break;
				}
				sb.Append(line + "\n");
			}
			sb.Append(line.Substring(0, num + 1));
			line = line.Trim();
			lineLength = line.Length;
			pos = num;
			toks.Add(new Token(this, sb.ToString(), lineNumber, position));
			sb = null;
		}

		private void HandleLineContinuation()
		{
			if (line.Substring(pos + 1).Trim().Length != 0)
			{
				ParseException("Line continuation (\\) before end of line", lineNumber, pos);
				return;
			}
			line = ReadLine();
			if (line == null)
			{
				ParseException("Line continuation at end of file.", lineNumber, pos);
			}
			lineLength = line.Length;
			pos = -1;
		}

		private void HandleNumberDateOrTimeSpan()
		{
			for (tokenStart = pos; pos < lineLength; pos++)
			{
				char c = line[pos];
				if ("0123456789.-+:abcdefghijklmnopqrstuvwxyz".IndexOf(char.ToLower(c)) != -1)
				{
					numberSb.Append(c);
				}
				else
				{
					if (c != '/' || (pos + 1 < lineLength && line[pos + 1] == '*'))
					{
						pos--;
						break;
					}
					numberSb.Append(c);
				}
			}
			toks.Add(new Token(this, numberSb.ToString(), lineNumber, tokenStart));
			numberSb.Remove(0, numberSb.Length);
		}

		private void HandleIdentifier()
		{
			tokenStart = pos;
			sb = new StringBuilder();
			while (pos < lineLength)
			{
				char c = line[pos];
				if (char.IsLetterOrDigit(c) || c == '-' || c == '_' || c == '.')
				{
					sb.Append(c);
					pos++;
					continue;
				}
				pos--;
				break;
			}
			toks.Add(new Token(this, sb.ToString(), lineNumber, tokenStart));
			sb = null;
		}

		private string ReadLine()
		{
			string text = reader.ReadLine();
			pos = 0;
			if (text == null)
			{
				return null;
			}
			lineNumber++;
			string text2 = text.Trim();
			while (text2.StartsWith("#") || text2.Length == 0)
			{
				text = reader.ReadLine();
				if (text == null)
				{
					return null;
				}
				lineNumber++;
				text2 = text.Trim();
			}
			return text;
		}

		private string ReadRawLine()
		{
			string text = reader.ReadLine();
			pos = 0;
			if (text == null)
			{
				return null;
			}
			lineNumber++;
			return text;
		}

		private static SDLDateTime Combine(SDLDateTime dt, TimeSpanWithZone tswz)
		{
			return new SDLDateTime(dt.Year, dt.Month, dt.Day, tswz.Hours, tswz.Minutes, tswz.Seconds, tswz.Milliseconds, tswz.TimeZone);
		}

		internal static int ReverseIfPositive(int val)
		{
			if (val < 1)
			{
				return val;
			}
			return -val;
		}

		internal void ParseException(string description, int line, int position)
		{
			try
			{
				reader.Close();
			}
			catch
			{
			}
			throw new SDLParseException(description, line + 1, position + 1);
		}

		internal void ExpectingButGot(string expecting, object got, int line, int position)
		{
			ParseException("Was expecting " + expecting + " but got " + got, line, position);
		}

		internal static string ParseString(string literal)
		{
			if (literal[0] != literal[literal.Length - 1])
			{
				throw new FormatException("Malformed string <" + literal + ">.  Strings must start and end with \" or `");
			}
			return literal.Substring(1, literal.Length - 2);
		}

		internal static char ParseCharacter(string literal)
		{
			if (literal[0] != '\'' || literal[literal.Length - 1] != '\'')
			{
				throw new FormatException("Malformed character <" + literal + ">.  Character literals must start and end with single quotes.");
			}
			return literal[1];
		}

		internal static object ParseNumber(string literal)
		{
			int length = literal.Length;
			bool flag = false;
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				char c = literal[i];
				if ("-0123456789".IndexOf(c) == -1)
				{
					if (c != '.')
					{
						num = i;
						break;
					}
					if (flag)
					{
						new FormatException("Encountered second decimal point.");
					}
					else if (i == length - 1)
					{
						new FormatException("Encountered decimal point at the end of the number.");
					}
					else
					{
						flag = true;
					}
				}
				else
				{
					num = i + 1;
				}
			}
			string text = literal.Substring(0, num);
			string text2 = literal.Substring(num);
			if (text2.Length == 0)
			{
				if (flag)
				{
					return Convert.ToDouble(text, NumberFormatInfo.InvariantInfo);
				}
				return int.Parse(text, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
			}
			if (text2.ToUpper(CultureInfo.InvariantCulture).Equals("BD"))
			{
				return Convert.ToDecimal(text, NumberFormatInfo.InvariantInfo);
			}
			if (text2.ToUpper(CultureInfo.InvariantCulture).Equals("L"))
			{
				if (flag)
				{
					new FormatException("Long literal with decimal point");
				}
				return Convert.ToInt64(text, NumberFormatInfo.InvariantInfo);
			}
			if (text2.ToUpper(CultureInfo.InvariantCulture).Equals("F"))
			{
				return Convert.ToSingle(text, NumberFormatInfo.InvariantInfo);
			}
			if (text2.ToUpper(CultureInfo.InvariantCulture).Equals("D"))
			{
				return Convert.ToDouble(text, NumberFormatInfo.InvariantInfo);
			}
			throw new FormatException("Could not parse number <" + literal + ">");
		}

		internal static SDLDateTime ParseDateTime(string literal)
		{
			int num = literal.IndexOf(' ');
			if (num == -1)
			{
				return ParseDate(literal);
			}
			SDLDateTime sDLDateTime = ParseDate(literal.Substring(0, num));
			string text = literal.Substring(num + 1);
			int num2 = text.IndexOf('-');
			string timeZone = null;
			if (num2 != -1)
			{
				timeZone = text.Substring(num2 + 1);
				text = text.Substring(0, num2);
			}
			string[] array = text.Split(new char[1] { ':' });
			if (array.Length < 2 || array.Length > 3)
			{
				throw new FormatException("Malformed time component in date/time literal.  Must use hh:mm(:ss)(.xxx)");
			}
			int second = 0;
			int millisecond = 0;
			int hour;
			int minute;
			try
			{
				hour = Convert.ToInt32(array[0]);
				minute = Convert.ToInt32(array[1]);
				if (array.Length == 3)
				{
					string text2 = array[2];
					int num3 = text2.IndexOf('.');
					if (num3 == -1)
					{
						second = Convert.ToInt32(text2);
					}
					else
					{
						second = Convert.ToInt32(text2.Substring(0, num3));
						string text3 = text2.Substring(num3 + 1);
						if (text3.Length == 1)
						{
							text3 += "00";
						}
						else if (text3.Length == 2)
						{
							text3 += "0";
						}
						millisecond = Convert.ToInt32(text3);
					}
				}
			}
			catch (FormatException ex)
			{
				throw new FormatException("Number format exception in time portion of date/time literal \"" + ex.Message + "\"");
			}
			return new SDLDateTime(sDLDateTime.Year, sDLDateTime.Month, sDLDateTime.Day, hour, minute, second, millisecond, timeZone);
		}

		internal static SDLDateTime ParseDate(string literal)
		{
			string[] array = literal.Split(new char[1] { '/' });
			if (array.Length != 3)
			{
				throw new FormatException("Malformed Date <" + literal + ">");
			}
			try
			{
				return new SDLDateTime(Convert.ToInt32(array[0]), Convert.ToInt32(array[1]), Convert.ToInt32(array[2]));
			}
			catch (FormatException ex)
			{
				throw new FormatException("Number format exception in date literal \"" + ex.Message + "\"");
			}
		}

		internal static byte[] ParseBinary(string literal)
		{
			string text = literal.Substring(1, literal.Length - 2);
			StringBuilder stringBuilder = new StringBuilder();
			int length = text.Length;
			for (int i = 0; i < length; i++)
			{
				char value = text[i];
				if ("\n\r\t ".IndexOf(value) == -1)
				{
					stringBuilder.Append(value);
				}
			}
			return Convert.FromBase64String(stringBuilder.ToString());
		}

		internal static TimeSpan ParseTimeSpan(string literal)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			string[] array = literal.Split(new char[1] { ':' });
			if (array.Length < 3 || array.Length > 4)
			{
				throw new FormatException("Malformed time span <" + literal + ">.  Time spans must use the format (d:)hh:mm:ss(.xxx) Note: if the day component is included it must be suffixed with lower case \"d\"");
			}
			int num4;
			int num5;
			try
			{
				if (array.Length == 4)
				{
					string text = array[0];
					if (!text.EndsWith("d"))
					{
						new FormatException("The day component of a time span must end with a lower case d");
					}
					num = Convert.ToInt32(text.Substring(0, text.Length - 1));
					num4 = Convert.ToInt32(array[1]);
					num5 = Convert.ToInt32(array[2]);
					if (array.Length == 4)
					{
						string text2 = array[3];
						int num6 = text2.IndexOf('.');
						if (num6 == -1)
						{
							num2 = Convert.ToInt32(text2);
						}
						else
						{
							num2 = Convert.ToInt32(text2.Substring(0, num6));
							string text3 = text2.Substring(num6 + 1);
							if (text3.Length == 1)
							{
								text3 += "00";
							}
							else if (text3.Length == 2)
							{
								text3 += "0";
							}
							num3 = Convert.ToInt32(text3);
						}
					}
					if (num < 0)
					{
						num4 = ReverseIfPositive(num4);
						num5 = ReverseIfPositive(num5);
						num2 = ReverseIfPositive(num2);
						num3 = ReverseIfPositive(num3);
					}
				}
				else
				{
					num4 = Convert.ToInt32(array[0]);
					num5 = Convert.ToInt32(array[1]);
					string text4 = array[2];
					int num7 = text4.IndexOf(".");
					if (num7 == -1)
					{
						num2 = Convert.ToInt32(text4);
					}
					else
					{
						num2 = Convert.ToInt32(text4.Substring(0, num7));
						string text5 = text4.Substring(num7 + 1);
						if (text5.Length == 1)
						{
							text5 += "00";
						}
						else if (text5.Length == 2)
						{
							text5 += "0";
						}
						num3 = Convert.ToInt32(text5);
					}
					if (num4 < 0)
					{
						num5 = ReverseIfPositive(num5);
						num2 = ReverseIfPositive(num2);
						num3 = ReverseIfPositive(num3);
					}
				}
			}
			catch (FormatException ex)
			{
				throw new FormatException("Number format in time span exception: \"" + ex.Message + "\" for literal <" + literal + ">");
			}
			return new TimeSpan(num, num4, num5, num2, num3);
		}
	}
}
