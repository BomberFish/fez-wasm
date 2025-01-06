// using Steamworks;

namespace FezGame.Components
{
	public class SteamAchievement
	{
		public readonly string AchievementName;

		// TODO: Fix achievements in WASM
		public bool IsAchieved = true;
		// {
		// 	get
		// 	{
		// 		SteamUserStats.GetAchievement(AchievementName, out var pbAchieved);
		// 		return pbAchieved;
		// 	}
		// }

		public SteamAchievement(string key)
		{
			AchievementName = key;
		}
	}
}
