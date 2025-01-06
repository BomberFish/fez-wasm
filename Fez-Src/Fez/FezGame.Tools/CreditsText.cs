using System.Collections.Generic;
using FezEngine.Services;
using FezEngine.Tools;
using Microsoft.Xna.Framework.Content;

namespace FezGame.Tools
{
	public static class CreditsText
	{
		private static readonly Dictionary<string, string> Fallback;

		static CreditsText()
		{
			ContentManager global = ServiceHelper.Get<IContentManagerProvider>().Global;
			Dictionary<string, Dictionary<string, string>> dictionary = global.Load<Dictionary<string, Dictionary<string, string>>>("Resources/CreditsText");
			Fallback = dictionary[string.Empty];
		}

		public static string GetString(string tag)
		{
			if (tag == null || !Fallback.TryGetValue(tag, out var value))
			{
				return "[MISSING TEXT]";
			}
			return value;
		}
	}
}
