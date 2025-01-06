using System;

namespace SDL
{
	public class SDLParseException : FormatException
	{
		private int line;

		private int position;

		public int Line => line;

		public int Position => position;

		public SDLParseException(string description, int line, int position)
			: base(description + " Line " + ((line == -1) ? "unknown" : (line.ToString() ?? "")) + ", Position " + ((position == -1) ? "unknown" : (position.ToString() ?? "")))
		{
			this.line = line;
			this.position = position;
		}

		public override string ToString()
		{
			return Message;
		}
	}
}
