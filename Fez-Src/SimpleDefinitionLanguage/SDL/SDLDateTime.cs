using System;
using System.Text;

namespace SDL
{
	public class SDLDateTime
	{
		private DateTime dateTime;

		private string timeZone;

		public DateTime DateTime => dateTime;

		public bool HasTime => dateTime.Hour != 0 || dateTime.Minute != 0 || dateTime.Second != 0 || dateTime.Millisecond != 0;

		public string TimeZone => timeZone;

		public int Year => dateTime.Year;

		public int Month => dateTime.Month;

		public int Day => dateTime.Day;

		public int Hour => dateTime.Hour;

		public int Minute => dateTime.Minute;

		public int Second => dateTime.Second;

		public int Millisecond => dateTime.Millisecond;

		public SDLDateTime(DateTime dateTime, string timeZone)
		{
			this.dateTime = dateTime;
			if (timeZone == null)
			{
				timeZone = getCurrentTimeZone();
			}
			this.timeZone = timeZone;
		}

		public SDLDateTime(int year, int month, int day)
			: this(new DateTime(year, month, day), null)
		{
		}

		public SDLDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
			: this(new DateTime(year, month, day, hour, minute, second, millisecond), null)
		{
		}

		public SDLDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, string timeZone)
			: this(new DateTime(year, month, day, hour, minute, second, millisecond), timeZone)
		{
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(Year + "/");
			if (Month < 10)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(Month + "/");
			if (Day < 10)
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append(Day);
			if (HasTime)
			{
				stringBuilder.Append(" ");
				if (Hour < 10)
				{
					stringBuilder.Append("0");
				}
				stringBuilder.Append(Hour);
				stringBuilder.Append(":");
				if (Minute < 10)
				{
					stringBuilder.Append("0");
				}
				stringBuilder.Append(Minute);
				if (Second != 0 || Millisecond != 0)
				{
					stringBuilder.Append(":");
					if (Second < 10)
					{
						stringBuilder.Append("0");
					}
					stringBuilder.Append(Second);
					if (Millisecond != 0)
					{
						stringBuilder.Append(".");
						string text = Millisecond.ToString() ?? "";
						if (text.Length == 1)
						{
							text = "00" + text;
						}
						else if (text.Length == 2)
						{
							text = "0" + text;
						}
						stringBuilder.Append(text);
					}
				}
				stringBuilder.Append("-");
				stringBuilder.Append((TimeZone == null) ? getCurrentTimeZone() : TimeZone);
			}
			return stringBuilder.ToString();
		}

		internal static string getCurrentTimeZone()
		{
			TimeZone currentTimeZone = System.TimeZone.CurrentTimeZone;
			TimeSpan utcOffset = currentTimeZone.GetUtcOffset(DateTime.Now);
			StringBuilder stringBuilder = new StringBuilder("GMT");
			stringBuilder.Append((utcOffset.Hours < 0) ? "-" : "+");
			int num = Math.Abs(utcOffset.Hours);
			stringBuilder.Append((num < 10) ? ("0" + num) : (num.ToString() ?? ""));
			stringBuilder.Append(":");
			int num2 = Math.Abs(utcOffset.Minutes);
			stringBuilder.Append((num2 < 10) ? ("0" + num2) : (num2.ToString() ?? ""));
			return stringBuilder.ToString();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			return ToString().Equals(obj.ToString());
		}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}
	}
}
