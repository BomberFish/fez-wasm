using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Common
{
	public static class Logger
	{
		private const string TimeFormat = "HH:mm:ss.fff";

		private const string DateTimeFormat = "yyyy-MM-dd";

		private static readonly HashSet<string> OnceLogged = new HashSet<string>();

		private static bool FirstLog = true;

		private static string LogFilePath;

		private static bool errorEncountered;

		public static bool ErrorEncountered
		{
			get
			{
				return errorEncountered;
			}
			private set
			{
				errorEncountered = value;
			}
		}

		public static void LogOnce(string component, LogSeverity severity, string message)
		{
			if (!OnceLogged.Contains(component + message))
			{
				OnceLogged.Add(component + message);
				Log(component, severity, message);
			}
		}

		public static void LogOnce(string component, string message)
		{
			LogOnce(component, LogSeverity.Information, message);
		}

		public static void Log(string component, string message)
		{
			Log(component, LogSeverity.Information, message);
		}

		public static void Log(string component, LogSeverity severity, string message)
		{
		// 	if (FirstLog)
		// 	{
		// 		try
		// 		{
		// 			string path = Path.Combine(Util.LocalSaveFolder, "Debug Log.txt");
		// 			if (File.Exists(path))
		// 			{
		// 				File.Delete(path);
		// 			}
		// 		}
		// 		catch
		// 		{
		// 		}
		// 		int num = 1;
		// 		LogFilePath = Path.Combine(Util.LocalSaveFolder, string.Format("[{0}] Debug Log.txt", DateTime.Now.ToString("yyyy-MM-dd")));
		// 		while (File.Exists(LogFilePath))
		// 		{
		// 			num++;
		// 			LogFilePath = Path.Combine(Util.LocalSaveFolder, string.Format("[{0}] Debug Log #{1}.txt", DateTime.Now.ToString("yyyy-MM-dd"), num));
		// 		}
		// 		FirstLog = false;
		// 		try
		// 		{
		// 			string[] files = Directory.GetFiles(Util.LocalSaveFolder, "*.txt");
		// 			foreach (string path2 in files)
		// 			{
		// 				if ((DateTime.UtcNow - File.GetLastWriteTimeUtc(path2)).TotalDays > 31.0)
		// 				{
		// 					File.Delete(path2);
		// 				}
		// 			}
		// 		}
		// 		catch (Exception ex)
		// 		{
		// 			Log("Logger", LogSeverity.Warning, "Log archival failed : " + ex.ToString());
		// 		}
		// 	}
		// 	try
		// 	{
		// 		using FileStream stream = File.Open(LogFilePath, FileMode.Append);
		// 		using StreamWriter streamWriter = new StreamWriter(stream);
		// 		streamWriter.WriteLine("({0}) [{1}] {2} : {3}", DateTime.Now.ToString("HH:mm:ss.fff"), component, severity.ToString().ToUpper(CultureInfo.InvariantCulture), message);
		// 	}
		// 	catch (Exception)
		// 	{
		// 	}
		// 	if (severity == LogSeverity.Error)
		// 	{
		// 		errorEncountered = true;
		// 	}
		if (severity == LogSeverity.Error) {
			Console.Error.WriteLine($"({DateTime.Now.ToString("HH:mm:ss.fff")}) [{component}] {severity.ToString().ToUpper(CultureInfo.InvariantCulture)}: {message}");
		} else {
			Console.WriteLine($"({DateTime.Now.ToString("HH:mm:ss.fff")}) [{component}] {severity.ToString().ToUpper(CultureInfo.InvariantCulture)}: {message}");
		}
			
		}

		public static void Try(Action action)
		{
			try
			{
				action();
			}
			catch (Exception e)
			{
				LogError(e);
			}
		}

		public static void Try<T>(Action<T> action, T arg)
		{
			try
			{
				action(arg);
			}
			catch (Exception e)
			{
				LogError(e);
			}
		}

		public static void Try<T, U>(Action<T, U> action, T arg1, U arg2)
		{
			try
			{
				action(arg1, arg2);
			}
			catch (Exception e)
			{
				LogError(e);
			}
		}

		public static void Try<T, U, V>(Action<T, U, V> action, T arg1, U arg2, V arg3)
		{
			try
			{
				action(arg1, arg2, arg3);
			}
			catch (Exception e)
			{
				LogError(e);
			}
		}

		public static void LogError(Exception e)
		{
			Log("Unhandled Exception", LogSeverity.Error, e.ToString());
		}

		public static void Clear()
		{
			errorEncountered = false;
		}
	}
}
