using System;
using System.IO;
using Common;
using SDL2;
// using Steamworks;

namespace EasyStorage
{
	public class PCSaveDevice : ISaveDevice
	{
		private struct CloudEntry
		{
			public bool Corrupted;

			public bool Exists;

			public long? LastDeletedTimestamp;

			public long? LastModifiedTimestamp;

			public byte[] Data;
		}

		public const int MaxSize = 40960;

		private static bool CloudSavesSynced;

		public static bool DisableCloudSaves;

		private static readonly string LocalSaveFolder = GetLocalSaveFolder();

		public string RootDirectory { get; set; }

		private static string GetLocalSaveFolder()
		{
			// string text = SDL.SDL_GetPlatform();
			string text3;
			// if (text.Equals("Linux"))
			// {
				string text2 = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
				if (string.IsNullOrEmpty(text2))
				{
					text2 = Environment.GetEnvironmentVariable("HOME");
					if (string.IsNullOrEmpty(text2))
					{
						return ".";
					}
					text2 += "/.local/share";
				}
				text3 = Path.Combine(text2, "FEZ");
			// }
			// else if (!text.Equals("Mac OS X"))
			// {
			// 	text3 = ((!text.Equals("Windows")) ? SDL.SDL_GetPrefPath(null, "FEZ") : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FEZ"));
			// }
			// else
			// {
			// 	string environmentVariable = Environment.GetEnvironmentVariable("HOME");
			// 	if (string.IsNullOrEmpty(environmentVariable))
			// 	{
			// 		return ".";
			// 	}
			// 	environmentVariable += "/Library/Application Support";
			// 	text3 = Path.Combine(environmentVariable, "FEZ");
			// }
			if (!Directory.Exists(text3))
			{
				Directory.CreateDirectory(text3);
			}
			return text3;
		}

		public PCSaveDevice(string gameName)
		{
			RootDirectory = LocalSaveFolder;
			// if (DisableCloudSaves || CloudSavesSynced)
			// {
			// 	return;
			// }
			// for (int i = 0; i < 3; i++)
			// {
			// 	string text = "SaveSlot" + i;
			// 	CloudEntry cloudEntry = new CloudEntry
			// 	{
			// 		Exists = SteamRemoteStorage.FileExists(text)
			// 	};
			// 	try
			// 	{
			// 		if (cloudEntry.Exists)
			// 		{
			// 			int fileSize = SteamRemoteStorage.GetFileSize(text);
			// 			cloudEntry.Data = new byte[fileSize];
			// 			SteamRemoteStorage.FileRead(text, cloudEntry.Data, fileSize);
			// 			using MemoryStream input = new MemoryStream(cloudEntry.Data);
			// 			using BinaryReader binaryReader = new BinaryReader(input);
			// 			cloudEntry.LastModifiedTimestamp = binaryReader.ReadInt64();
			// 		}
			// 	}
			// 	catch (Exception ex)
			// 	{
			// 		Logger.Log("SaveDevice", LogSeverity.Warning, "Error getting cloud save #" + i + " : " + ex);
			// 		cloudEntry.Corrupted = true;
			// 	}
			// 	try
			// 	{
			// 		if (SteamRemoteStorage.FileExists(text + "_LastDelete"))
			// 		{
			// 			int fileSize2 = SteamRemoteStorage.GetFileSize(text);
			// 			byte[] array = new byte[fileSize2];
			// 			SteamRemoteStorage.FileRead(text, array, fileSize2);
			// 			using MemoryStream input2 = new MemoryStream(array);
			// 			using BinaryReader binaryReader2 = new BinaryReader(input2);
			// 			cloudEntry.LastDeletedTimestamp = binaryReader2.ReadInt64();
			// 		}
			// 	}
			// 	catch (Exception ex2)
			// 	{
			// 		Logger.Log("SaveDevice", LogSeverity.Warning, "Error getting last delete time for cloud save #" + i + " : " + ex2);
			// 	}
			// 	string path = Path.Combine(RootDirectory, text);
			// 	if (!File.Exists(path))
			// 	{
			// 		if (cloudEntry.Exists && !cloudEntry.Corrupted && (!cloudEntry.LastDeletedTimestamp.HasValue || cloudEntry.LastDeletedTimestamp.Value < cloudEntry.LastModifiedTimestamp.Value))
			// 		{
			// 			Logger.Log("SaveDevice", LogSeverity.Information, "Copying back cloud save #" + i + " to local because it did not exist locally");
			// 			try
			// 			{
			// 				using FileStream output = new FileStream(path, FileMode.Create);
			// 				using BinaryWriter binaryWriter = new BinaryWriter(output);
			// 				binaryWriter.Write(cloudEntry.Data);
			// 			}
			// 			catch (Exception ex3)
			// 			{
			// 				Logger.Log("SaveDevice", LogSeverity.Warning, "Error copying cloud entry data to local for cloud save #" + i + " : " + ex3);
			// 			}
			// 		}
			// 	}
			// 	else
			// 	{
			// 		long num = long.MinValue;
			// 		try
			// 		{
			// 			using FileStream input3 = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			// 			using BinaryReader binaryReader3 = new BinaryReader(input3);
			// 			num = binaryReader3.ReadInt64();
			// 		}
			// 		catch (Exception ex4)
			// 		{
			// 			Logger.Log("SaveDevice", LogSeverity.Warning, "Error while loading local file for timestamp compare : " + ex4);
			// 		}
			// 		if (cloudEntry.LastDeletedTimestamp.HasValue && cloudEntry.LastDeletedTimestamp.Value > num)
			// 		{
			// 			Logger.Log("SaveDevice", LogSeverity.Information, "Deleting local save #" + i + " because of pending cloud deletion");
			// 			File.Delete(path);
			// 			num = long.MinValue;
			// 		}
			// 		if (cloudEntry.Exists && !cloudEntry.Corrupted && (!cloudEntry.LastDeletedTimestamp.HasValue || cloudEntry.LastDeletedTimestamp.Value < cloudEntry.LastModifiedTimestamp.Value) && cloudEntry.LastModifiedTimestamp.Value > num)
			// 		{
			// 			Logger.Log("SaveDevice", LogSeverity.Information, "Copying back cloud save #" + i + " to local because the local copy is older");
			// 			try
			// 			{
			// 				using FileStream output2 = new FileStream(path, FileMode.Create);
			// 				using BinaryWriter binaryWriter2 = new BinaryWriter(output2);
			// 				binaryWriter2.Write(cloudEntry.Data);
			// 			}
			// 			catch (Exception ex5)
			// 			{
			// 				Logger.Log("SaveDevice", LogSeverity.Warning, "Error copying cloud entry data to local for cloud save #" + i + " : " + ex5);
			// 			}
			// 		}
			// 	}
			// 	CloudSavesSynced = true;
			// }
		}

		public virtual bool Save(string fileName, SaveAction saveAction)
		{
			if (!Directory.Exists(RootDirectory))
			{
				Directory.CreateDirectory(RootDirectory);
			}
			string text = Path.Combine(RootDirectory, fileName);
			if (File.Exists(text))
			{
				File.Copy(text, text + "_Backup", overwrite: true);
			}
			try
			{
				byte[] buffer = new byte[40960];
				using (MemoryStream memoryStream = new MemoryStream(buffer))
				{
					using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
					binaryWriter.Write(DateTime.Now.ToFileTime());
					saveAction(binaryWriter);
					if (memoryStream.Length < 40960)
					{
						long num = 40960 - memoryStream.Length;
						binaryWriter.Write(new byte[num]);
					}
					else if (memoryStream.Length > 40960)
					{
						throw new InvalidOperationException("Save file greater than the imposed limit!");
					}
				}
				using (FileStream output = new FileStream(text, FileMode.Create, FileAccess.Write, FileShare.Read))
				{
					using BinaryWriter binaryWriter2 = new BinaryWriter(output);
					binaryWriter2.Write(buffer);
				}
				return true;
			}
			catch (Exception ex)
			{
				Logger.Log("SaveDevice", LogSeverity.Warning, "Error while saving : " + ex);
			}
			return false;
		}

		public virtual bool Load(string fileName, LoadAction loadAction)
		{
			if (!Directory.Exists(RootDirectory))
			{
				Directory.CreateDirectory(RootDirectory);
			}
			bool result = false;
			string text = Path.Combine(RootDirectory, fileName);
			if (!File.Exists(text))
			{
				return false;
			}
			try
			{
				using FileStream input = new FileStream(text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				using BinaryReader binaryReader = new BinaryReader(input);
				binaryReader.ReadInt64();
				loadAction(binaryReader);
				result = true;
			}
			catch (Exception ex)
			{
				if (!fileName.EndsWith("_Backup"))
				{
					if (File.Exists(text + "_Backup"))
					{
						Logger.Log("SaveDevice", LogSeverity.Warning, text + " | Loading error, will try with backup : " + ex);
						return Load(fileName + "_Backup", loadAction);
					}
					Logger.Log("SaveDevice", LogSeverity.Warning, text + " | Loading error, no backup found : " + ex);
				}
				else
				{
					Logger.Log("SaveDevice", LogSeverity.Warning, text + " | Error loading backup : " + ex);
				}
			}
			return result;
		}

		public virtual bool Delete(string fileName)
		{
			if (!Directory.Exists(RootDirectory))
			{
				Directory.CreateDirectory(RootDirectory);
			}
			// if (!DisableCloudSaves)
			// {
			// 	if (SteamRemoteStorage.FileExists(fileName))
			// 	{
			// 		SteamRemoteStorage.FileDelete(fileName);
			// 	}
			// 	if (SteamRemoteStorage.FileExists(fileName + "_LastDelete"))
			// 	{
			// 		SteamRemoteStorage.FileDelete(fileName + "_LastDelete");
			// 	}
			// 	byte[] bytes = BitConverter.GetBytes(DateTime.Now.ToFileTime());
			// 	SteamRemoteStorage.FileWrite(fileName + "_LastDelete", bytes, bytes.Length);
			// }
			string path = Path.Combine(RootDirectory, fileName);
			if (File.Exists(path))
			{
				File.Delete(path);
				return !File.Exists(path);
			}
			return true;
		}

		public virtual bool FileExists(string fileName)
		{
			if (!Directory.Exists(RootDirectory))
			{
				Directory.CreateDirectory(RootDirectory);
			}
			return File.Exists(Path.Combine(RootDirectory, fileName));
		}
	}
}
