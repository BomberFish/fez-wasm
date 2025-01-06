using System.Collections.Generic;
using FezEngine.Services;
using FezEngine.Tools;
using Microsoft.Xna.Framework.Content;
using Common;
using System;

namespace FezGame.Tools
{
	public static class StaticText
	{
		private static readonly Dictionary<string, string> Fallback;

		private static readonly Dictionary<string, Dictionary<string, string>> AllResources;

		static StaticText()
		{
			Logger.Log("StaticText", "StaticText()");
			ContentManager global = ServiceHelper.Get<IContentManagerProvider>().Global;
			Logger.Log("StaticText", "Loading static text resources");
			try
			{
				Logger.Log("StaticText", "Loading static text resources for current culture");
				AllResources = global.Load<Dictionary<string, Dictionary<string, string>>>("Resources/StaticText");
			}
			catch(Exception ex)
			{
				Logger.Log("StaticText", LogSeverity.Error, "Failed to load static text resources for current culture");
				Logger.LogError(ex);
			}
			Logger.Log("StaticText", "Done loading static text resources");
			Fallback = AllResources[string.Empty];
		}

		public static bool TryGetString(string tag, out string text)
		{
			// Logger.Log("StaticText", "Getting static text for tag " + tag);
			string twoLetterISOLanguageName = Culture.TwoLetterISOLanguageName;
			if (!AllResources.TryGetValue(twoLetterISOLanguageName, out var value))
			{
				value = Fallback;
			}
			if ((tag == null || !value.TryGetValue(tag, out text)) && (tag == null || !Fallback.TryGetValue(tag, out text)))
			{
				text = "[MISSING TEXT]";
				return false;
			}
			return true;
		}

		public static string GetString(string tag)
		{
			// Logger.Log("StaticText", "Getting static text for tag " + tag);
			if (TryGetString(tag, out var text))
			{
				return text;
			}
			return "[MISSING TEXT]";
		}
	}
}
