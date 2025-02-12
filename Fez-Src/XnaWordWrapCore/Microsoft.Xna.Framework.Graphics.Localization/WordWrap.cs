namespace Microsoft.Xna.Framework.Graphics.Localization
{
	public static class WordWrap
	{
		public delegate uint CB_GetWidth(SpriteFont spriteFont, char c);

		public delegate uint CB_Reserved();

		private static Prohibition prohibition;

		private static NoHangulWrap nohangul;

		private static CB_GetWidth GetWidth;

		private static CB_Reserved Reserved;

		private static BreakInfo[] BreakArray;

		public static Prohibition ProhibitionSetting
		{
			get
			{
				return prohibition;
			}
			set
			{
				prohibition = value;
			}
		}

		public static NoHangulWrap NoHangulWrapSetting
		{
			get
			{
				return nohangul;
			}
			set
			{
				nohangul = value;
			}
		}

		static WordWrap()
		{
			prohibition = Prohibition.On;
			nohangul = NoHangulWrap.Off;
			BreakArray = new BreakInfo[146]
			{
				new BreakInfo(94u, isNonBeginningCharacter: true, isNonEndingCharacter: true),
				new BreakInfo(33u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(36u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(37u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(39u, isNonBeginningCharacter: true, isNonEndingCharacter: true),
				new BreakInfo(40u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(41u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(44u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(46u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(47u, isNonBeginningCharacter: true, isNonEndingCharacter: true),
				new BreakInfo(58u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(59u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(63u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(91u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(92u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(93u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(123u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(125u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(162u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(163u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(165u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(167u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(168u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(169u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(174u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(176u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(183u, isNonBeginningCharacter: true, isNonEndingCharacter: true),
				new BreakInfo(711u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(713u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(8211u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(8212u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(8213u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(8214u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(8216u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(8217u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(8220u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(8221u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(8226u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(8229u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(8230u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(8231u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(8242u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(8243u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(8245u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(8451u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(8482u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(8758u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(9588u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(9839u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(12289u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12290u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12291u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12293u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12296u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(12297u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12298u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(12299u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12300u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(12301u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12302u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(12303u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12304u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(12305u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12306u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(12308u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(12309u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12310u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(12311u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12317u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(12318u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12319u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12353u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12355u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12357u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12359u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12361u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12387u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12419u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12421u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12423u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12430u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12441u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12442u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12443u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12444u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12445u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12446u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12449u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12451u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12453u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12455u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12457u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12483u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12515u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12517u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12519u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12526u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12533u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12534u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12539u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12540u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12541u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(12542u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65072u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65104u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65105u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65106u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65108u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65109u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65110u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65111u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65113u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(65114u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65115u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(65116u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65117u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(65118u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65281u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65282u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65284u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(65285u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65287u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65288u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(65289u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65292u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65294u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65306u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65307u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65311u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65312u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(65339u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(65341u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65344u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65371u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(65372u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65373u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65374u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65377u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65380u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65392u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65438u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65439u, isNonBeginningCharacter: true, isNonEndingCharacter: false),
				new BreakInfo(65504u, isNonBeginningCharacter: true, isNonEndingCharacter: true),
				new BreakInfo(65505u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(65509u, isNonBeginningCharacter: false, isNonEndingCharacter: true),
				new BreakInfo(65510u, isNonBeginningCharacter: false, isNonEndingCharacter: true)
			};
			SetCallback(MyGetCharWidth, null);
		}

		private static uint MyGetCharWidth(SpriteFont spriteFont, char c)
		{
			return (uint)spriteFont.MeasureString(new string(c, 1)).X;
		}

		public static string Split(string text, SpriteFont font, float maxTextSize)
		{
			string text2 = text;
			text = "";
			if (text2.Length == 0)
			{
				return text;
			}
			do
			{
				string EOL;
				int EOLOffset;
				string text3 = FindNextLine(font, text2, (uint)maxTextSize, out EOL, out EOLOffset);
				int num = ((EOL != null) ? (EOLOffset + 1) : 0);
				if (num != 0)
				{
					string text4 = text2.Substring(0, num);
					text = text + text4 + "\r\n";
				}
				else
				{
					text += "\r\n";
				}
				text2 = text3;
			}
			while (text2 != null);
			if (text.EndsWith("\r\n"))
			{
				text = text.Substring(0, text.Length - 2);
			}
			else if (text.EndsWith("\n"))
			{
				text = text.Substring(0, text.Length - 1);
			}
			return text;
		}

		public static bool IsNonBeginningChar(char c)
		{
			if (prohibition == Prohibition.Off)
			{
				return false;
			}
			int num = 0;
			int num2 = BreakArray.Length;
			int num3 = 0;
			while (num <= num2)
			{
				num3 = (num2 - num) / 2 + num;
				if (BreakArray[num3].Character == c)
				{
					return BreakArray[num3].IsNonBeginningCharacter;
				}
				if (c < BreakArray[num3].Character)
				{
					num2 = num3 - 1;
				}
				else
				{
					num = num3 + 1;
				}
			}
			return false;
		}

		public static bool IsNonEndingChar(char c)
		{
			if (prohibition == Prohibition.Off)
			{
				return false;
			}
			int num = 0;
			int num2 = BreakArray.Length;
			int num3 = 0;
			while (num <= num2)
			{
				num3 = (num2 - num) / 2 + num;
				if (BreakArray[num3].Character == c)
				{
					return BreakArray[num3].IsNonEndingCharacter;
				}
				if (c < BreakArray[num3].Character)
				{
					num2 = num3 - 1;
				}
				else
				{
					num = num3 + 1;
				}
			}
			return false;
		}

		public static void SetCallback(CB_GetWidth cbGetWidth, CB_Reserved pReserved)
		{
			if (cbGetWidth != null)
			{
				GetWidth = cbGetWidth;
			}
			if (pReserved != null)
			{
				Reserved = pReserved;
			}
		}

		public static bool IsEastAsianChar(char c)
		{
			if (nohangul == NoHangulWrap.On && (('ᄀ' <= c && c <= 'ᇿ') || ('\u3130' <= c && c <= '\u318f') || ('가' <= c && c <= '힣')))
			{
				return false;
			}
			return ('ᄀ' <= c && c <= 'ᇿ') || ('\u3000' <= c && c <= '\ud7af') || ('豈' <= c && c <= '\ufaff') || ('\uff00' <= c && c <= 'ￜ');
		}

		public static bool CanBreakLineAt(string pszStart, int index)
		{
			if (index == 0)
			{
				return false;
			}
			if (IsWhiteSpace(pszStart[index]) && IsNonBeginningChar(pszStart[index + 1]))
			{
				return false;
			}
			if (index > 1 && IsWhiteSpace(pszStart[index - 2]) && pszStart[index - 1] == '"' && !IsWhiteSpace(pszStart[index]))
			{
				return false;
			}
			if (!IsWhiteSpace(pszStart[index - 1]) && pszStart[index] == '"' && IsWhiteSpace(pszStart[index + 1]))
			{
				return false;
			}
			return (IsWhiteSpace(pszStart[index]) || IsEastAsianChar(pszStart[index]) || IsEastAsianChar(pszStart[index - 1]) || pszStart[index - 1] == '-') && !IsNonBeginningChar(pszStart[index]) && !IsNonEndingChar(pszStart[index - 1]);
		}

		public static string FindNonWhiteSpaceForward(string text)
		{
			int i;
			for (i = 0; i < text.Length && IsWhiteSpace(text[i]); i++)
			{
			}
			if (i < text.Length && IsLineFeed(text[i]))
			{
				i++;
			}
			return (i < text.Length) ? text.Substring(i) : null;
		}

		public static string FindNonWhiteSpaceBackward(string source, int index, out int offset)
		{
			while (index >= 0 && (IsWhiteSpace(source[index]) || IsLineFeed(source[index])))
			{
				index--;
			}
			offset = index;
			return (index < 0) ? null : source.Substring(index);
		}

		public static bool IsWhiteSpace(char c)
		{
			return c == '\t' || c == '\r' || c == ' ' || c == '\u3000';
		}

		public static bool IsLineFeed(char c)
		{
			return c == '\n';
		}

		public static string FindNextLine(SpriteFont spriteFont, string source, uint width, out string EOL, out int EOLOffset)
		{
			if (GetWidth == null || source == null)
			{
				EOL = null;
				EOLOffset = -1;
				return null;
			}
			int i = 0;
			uint num = 0u;
			for (; i < source.Length && !IsLineFeed(source[i]); i++)
			{
				num += GetWidth(spriteFont, source[i]);
				if (num > width)
				{
					break;
				}
			}
			if (i == 0)
			{
				EOL = FindNonWhiteSpaceBackward(source, i, out EOLOffset);
				return FindNonWhiteSpaceForward(source.Substring(i + 1));
			}
			if (num <= width)
			{
				EOL = FindNonWhiteSpaceBackward(source, i - 1, out EOLOffset);
				if (i - 1 >= 0 && IsLineFeed(source[i - 1]))
				{
					return source.Substring(i);
				}
				return (i >= 0 && i < source.Length) ? FindNonWhiteSpaceForward(source.Substring(i)) : null;
			}
			int num2 = i;
			while (i > 0)
			{
				if (IsWhiteSpace(source[i]))
				{
					EOL = FindNonWhiteSpaceBackward(source, i, out EOLOffset);
					if (EOL != null)
					{
						return FindNonWhiteSpaceForward(source.Substring(i + 1));
					}
					i = EOLOffset + 1;
				}
				if (CanBreakLineAt(source, i))
				{
					break;
				}
				i--;
			}
			if (i <= 0)
			{
				EOL = source.Substring(num2 - 1);
				EOLOffset = num2 - 1;
				return source.Substring(num2);
			}
			EOL = source.Substring(i - 1);
			EOLOffset = i - 1;
			return FindNonWhiteSpaceForward(source.Substring(i));
		}
	}
}
