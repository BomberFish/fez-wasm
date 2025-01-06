namespace Common
{
	public class Crc32
	{
		private static readonly uint[] table;

		static Crc32()
		{
			table = new uint[256];
			for (uint num = 0u; num < table.Length; num++)
			{
				uint num2 = num;
				for (int num3 = 8; num3 > 0; num3--)
				{
					num2 = (((num2 & 1) != 1) ? (num2 >> 1) : ((num2 >> 1) ^ 0xEDB88320u));
				}
				table[num] = num2;
			}
		}

		public static uint ComputeChecksum(byte[] bytes)
		{
			uint num = uint.MaxValue;
			for (int i = 0; i < bytes.Length; i++)
			{
				byte b = (byte)((num & 0xFFu) ^ bytes[i]);
				num = (num >> 8) ^ table[b];
			}
			return ~num;
		}
	}
}
