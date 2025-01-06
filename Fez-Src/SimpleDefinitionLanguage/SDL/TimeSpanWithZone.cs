namespace SDL
{
	internal class TimeSpanWithZone
	{
		private readonly string timeZone;

		private readonly int days;

		private readonly int hours;

		private readonly int minutes;

		private readonly int seconds;

		private readonly int milliseconds;

		internal int Days => days;

		internal int Hours => hours;

		internal int Minutes => minutes;

		internal int Seconds => seconds;

		internal int Milliseconds => milliseconds;

		internal string TimeZone => timeZone;

		internal TimeSpanWithZone(int days, int hours, int minutes, int seconds, int milliseconds, string timeZone)
		{
			this.days = days;
			this.hours = hours;
			this.minutes = minutes;
			this.seconds = seconds;
			this.milliseconds = milliseconds;
			this.timeZone = timeZone;
		}
	}
}
