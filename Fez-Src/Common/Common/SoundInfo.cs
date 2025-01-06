using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Common
{
	public static class SoundInfo
	{
		[DllImport("winmm.dll")]
		private static extern uint mciSendString(string command, StringBuilder returnValue, int returnLength, IntPtr winHandle);

		public static int GetSoundLength(string fileName)
		{
			StringBuilder stringBuilder = new StringBuilder(32);
			mciSendString($"open \"{fileName}\" type waveaudio alias wave", null, 0, IntPtr.Zero);
			mciSendString("status wave length", stringBuilder, stringBuilder.Capacity, IntPtr.Zero);
			mciSendString("close wave", null, 0, IntPtr.Zero);
			int.TryParse(stringBuilder.ToString(), out var result);
			return result;
		}
	}
}
