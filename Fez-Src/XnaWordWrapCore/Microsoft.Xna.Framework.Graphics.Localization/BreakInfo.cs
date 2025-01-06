using System;

namespace Microsoft.Xna.Framework.Graphics.Localization
{
	public struct BreakInfo
	{
		private uint m_Character;

		private bool m_IsNonBeginningCharacter;

		private bool m_IsNonEndingCharacter;

		public uint Character => m_Character;

		public bool IsNonBeginningCharacter => m_IsNonBeginningCharacter;

		public bool IsNonEndingCharacter => m_IsNonEndingCharacter;

		public BreakInfo(uint character, bool isNonBeginningCharacter, bool isNonEndingCharacter)
		{
			if (character > 1114111)
			{
				throw new ArgumentException("Invalid code point.");
			}
			m_Character = character;
			m_IsNonBeginningCharacter = isNonBeginningCharacter;
			m_IsNonEndingCharacter = isNonEndingCharacter;
		}
	}
}
