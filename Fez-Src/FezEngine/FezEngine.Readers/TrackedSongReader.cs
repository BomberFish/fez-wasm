using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;
using Common;
using System;

namespace FezEngine.Readers
{
	public class TrackedSongReader : ContentTypeReader<TrackedSong>
	{
		protected override TrackedSong Read(ContentReader input, TrackedSong existingInstance)
		{
			if (existingInstance == null)
			{
				existingInstance = new TrackedSong();
			}

			try 
			{
			Logger.Log("TrackedSongReader", "Reading Loops");
			existingInstance.Loops = input.ReadObject(existingInstance.Loops);
			Logger.Log("TrackedSongReader", "Reading Name");
			existingInstance.Name = input.ReadString();
			Logger.Log("TrackedSongReader", "Reading Tempo");
			existingInstance.Tempo = input.ReadInt32();
			Logger.Log("TrackedSongReader", "Reading TimeSignature");
			existingInstance.TimeSignature = input.ReadInt32();
			Logger.Log("TrackedSongReader", "Reading Notes");
			existingInstance.Notes = input.ReadObject<ShardNotes[]>();
			Logger.Log("TrackedSongReader", "Reading AssembleChords");
			existingInstance.AssembleChord = input.ReadObject<AssembleChords>();
			Logger.Log("TrackedSongReader", "Reading RandomOrdering");
			existingInstance.RandomOrdering = input.ReadBoolean();
			Logger.Log("TrackedSongReader", "Reading CustomOrdering");
			existingInstance.CustomOrdering = input.ReadObject<int[]>();
			}
			catch (Exception e)
			{
				Logger.Log("TrackedSongReader", LogSeverity.Error, "Error reading TrackedSong");
				Logger.LogError(e);
				throw;
			}
			
			return existingInstance;
		}
	}
}
