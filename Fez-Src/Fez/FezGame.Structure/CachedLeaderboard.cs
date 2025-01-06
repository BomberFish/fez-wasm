using System;
using System.Collections.Generic;
using System.Linq;
using FezGame.Services;
// using Steamworks;

namespace FezGame.Structure
{
	public class CachedLeaderboard
	{
		private const int ReadPageSize = 100;

		private const string LeaderboardKey = "CompletionPercentage";

		private readonly int virtualPageSize;

		// private readonly List<LeaderboardEntry_t> cachedEntries = new List<LeaderboardEntry_t>();

		// private SteamLeaderboard_t leaderboard;

		private int startIndex;

		private Action callback;

		private Action onLeaderboardFound;

		public LeaderboardView View { get; private set; }

		public FezGame.Services.SteamUser ActiveGamer { get; set; }

		// public bool InError => leaderboard.m_SteamLeaderboard == 0L && !Reading;

		public bool Reading { get; private set; }

		public bool ChangingPage { get; private set; }

		public bool CanPageUp = false;
		// {
		// 	get
		// 	{
		// 		if (cachedEntries.Count == 0)
		// 		{
		// 			return false;
		// 		}
		// 		if (View == LeaderboardView.Friends)
		// 		{
		// 			return startIndex - virtualPageSize >= 0;
		// 		}
		// 		return startIndex > 0 || cachedEntries[0].m_nGlobalRank > 1;
		// 	}
		// }

		public bool CanPageDown = false;
		// {
		// 	get
		// 	{
		// 		if (cachedEntries.Count == 0)
		// 		{
		// 			return false;
		// 		}
		// 		if (View == LeaderboardView.Friends)
		// 		{
		// 			return startIndex + virtualPageSize < cachedEntries.Count;
		// 		}
		// 		return startIndex < cachedEntries.Count || cachedEntries[cachedEntries.Count - 1].m_nGlobalRank < TotalEntries;
		// 	}
		// }

		// public IEnumerable<LeaderboardEntry_t> Entries => cachedEntries.Skip(startIndex).Take(virtualPageSize);

		// public int TotalEntries => (leaderboard.m_SteamLeaderboard != 0L) ? SteamUserStats.GetLeaderboardEntryCount(leaderboard) : 0;

		public CachedLeaderboard(FezGame.Services.SteamUser activeGamer, int virtualPageSize)
		{
			// this.virtualPageSize = virtualPageSize;
			// ActiveGamer = activeGamer;
			// CallResult<LeaderboardFindResult_t> callResult = new CallResult<LeaderboardFindResult_t>(OnReceiveLeaderboard);
			// SteamAPICall_t hAPICall = SteamUserStats.FindLeaderboard("CompletionPercentage");
			// callResult.Set(hAPICall);
		}

		// private void OnReceiveLeaderboard(LeaderboardFindResult_t result, bool bIOFailure)
		// {
		// 	leaderboard = result.m_hSteamLeaderboard;
		// 	if (onLeaderboardFound != null)
		// 	{
		// 		onLeaderboardFound();
		// 	}
		// 	onLeaderboardFound = null;
		// }

		// private void CacheEntries(LeaderboardScoresDownloaded_t result, bool bIOFailure)
		// {
		// 	CacheEntries(result, 0);
		// }

		// private void CacheEntriesUp(LeaderboardScoresDownloaded_t result, bool bIOFailure)
		// {
		// 	CacheEntries(result, -1);
		// }

		// private void CacheEntriesDown(LeaderboardScoresDownloaded_t result, bool bIOFailure)
		// {
		// 	CacheEntries(result, 1);
		// }

		// private void CacheEntries(LeaderboardScoresDownloaded_t result, int pageSign)
		// {
		// 	if (result.m_hSteamLeaderboardEntries.m_SteamLeaderboardEntries != 0)
		// 	{
		// 		if (pageSign == 0)
		// 		{
		// 			cachedEntries.Clear();
		// 		}
		// 		if (leaderboard.m_SteamLeaderboard != 0)
		// 		{
		// 			if (pageSign == -1)
		// 			{
		// 				for (int num = result.m_cEntryCount - 1; num >= 0; num--)
		// 				{
		// 					SteamUserStats.GetDownloadedLeaderboardEntry(result.m_hSteamLeaderboardEntries, num, out var pLeaderboardEntry, null, 0);
		// 					cachedEntries.Insert(0, pLeaderboardEntry);
		// 				}
		// 				startIndex += result.m_cEntryCount;
		// 				startIndex -= virtualPageSize;
		// 			}
		// 			else
		// 			{
		// 				for (int i = 0; i < result.m_cEntryCount; i++)
		// 				{
		// 					SteamUserStats.GetDownloadedLeaderboardEntry(result.m_hSteamLeaderboardEntries, i, out var pLeaderboardEntry2, null, 0);
		// 					cachedEntries.Add(pLeaderboardEntry2);
		// 				}
		// 			}
		// 		}
		// 	}
		// 	Reading = false;
		// 	callback();
		// }

		public void ChangeView(LeaderboardView leaderboardView, Action onFinished)
		{
			// if (Reading)
			// {
			// 	onFinished();
			// 	return;
			// }
			// if (leaderboard.m_SteamLeaderboard == 0)
			// {
			// 	onLeaderboardFound = delegate
			// 	{
			// 		ChangeView(leaderboardView, onFinished);
			// 	};
			// 	return;
			// }
			// Reading = true;
			// View = leaderboardView;
			// callback = onFinished;
			// CallResult<LeaderboardScoresDownloaded_t> callResult = new CallResult<LeaderboardScoresDownloaded_t>(CacheEntries);
			// switch (View)
			// {
			// case LeaderboardView.Friends:
			// {
			// 	SteamAPICall_t hAPICall = SteamUserStats.DownloadLeaderboardEntries(leaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestFriends, 0, 100);
			// 	callResult.Set(hAPICall);
			// 	startIndex = 0;
			// 	break;
			// }
			// case LeaderboardView.MyScore:
			// {
			// 	SteamAPICall_t hAPICall = SteamUserStats.DownloadLeaderboardEntries(leaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, -50, 50);
			// 	callResult.Set(hAPICall);
			// 	startIndex = 50 - virtualPageSize / 2 + 1;
			// 	break;
			// }
			// case LeaderboardView.Overall:
			// {
			// 	SteamAPICall_t hAPICall = SteamUserStats.DownloadLeaderboardEntries(leaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, 100);
			// 	callResult.Set(hAPICall);
			// 	startIndex = 0;
			// 	break;
			// }
			// }
		}

		public void PageUp(Action onFinished)
		{
			// if (InError)
			// {
			// 	onFinished();
			// 	return;
			// }
			// if (startIndex >= virtualPageSize)
			// {
			// 	startIndex -= virtualPageSize;
			// 	onFinished();
			// 	return;
			// }
			// if (!CanPageUp)
			// {
			// 	onFinished();
			// 	return;
			// }
			// callback = onFinished;
			// CallResult<LeaderboardScoresDownloaded_t> callResult = new CallResult<LeaderboardScoresDownloaded_t>(CacheEntriesUp);
			// SteamAPICall_t hAPICall = SteamUserStats.DownloadLeaderboardEntries(leaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, cachedEntries[0].m_nGlobalRank - 100, cachedEntries[0].m_nGlobalRank - 1);
			// callResult.Set(hAPICall);
		}

		public void PageDown(Action onFinished)
		{
			// if (InError)
			// {
			// 	onFinished();
			// 	return;
			// }
			// if (startIndex + virtualPageSize * 2 <= cachedEntries.Count)
			// {
			// 	startIndex += virtualPageSize;
			// 	onFinished();
			// 	return;
			// }
			// if (!CanPageDown)
			// {
			// 	onFinished();
			// 	return;
			// }
			// if (View == LeaderboardView.Friends)
			// {
			// 	startIndex += virtualPageSize;
			// 	onFinished();
			// 	return;
			// }
			// startIndex += virtualPageSize;
			// callback = onFinished;
			// CallResult<LeaderboardScoresDownloaded_t> callResult = new CallResult<LeaderboardScoresDownloaded_t>(CacheEntriesDown);
			// SteamAPICall_t hAPICall = SteamUserStats.DownloadLeaderboardEntries(leaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, cachedEntries[cachedEntries.Count - 1].m_nGlobalRank + 1, cachedEntries[cachedEntries.Count - 1].m_nGlobalRank + 100);
			// callResult.Set(hAPICall);
		}
	}
}
