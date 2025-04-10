using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.CloudSave.Models.Data.Player;
using Unity.Services.Core;
using UnityEngine;
using SaveOptions = Unity.Services.CloudSave.Models.Data.Player.SaveOptions;

namespace AvantGardeMaker.Ceeu
{
	public static class SaveLoadUtility
	{
		static SaveLoadUtility()
		{
			Initialize();
		}

		public static async void Initialize()
		{
			// 유니티 서비스 초기화
			await UnityServices.InitializeAsync();

			AuthenticationService.Instance.SignedIn += () =>
			{
				Debug.Log("Signed in");
			};
			AuthenticationService.Instance.SignedOut += () =>
			{
				Debug.Log("Signed out");
			};
		}

		public static async Awaitable SaveStageData(string mapTitle, StageData stageData)
		{
			try
			{
				SaveOptions saveOption = new SaveOptions(new PublicWriteAccessClassOptions());

				// 유니티 서비스 로그인
				await AuthenticationService.Instance.SignInAnonymouslyAsync();

				// Json 변환
				string json = JsonUtility.ToJson(stageData);

				// 압축
				byte[] compressedJson = await Compression.Compress(json);

				// 기존 제작한 맵 타이틀 리스트 가져오기
				List<string> mapTitleList = await GetMakingMapTitleList(AuthenticationService.Instance.PlayerId);

				if (mapTitleList.Contains(mapTitle) == false)
					mapTitleList.Add(mapTitle);

				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < mapTitleList.Count - 1; ++i)
				{
					sb.Append(mapTitleList[i]);
					sb.Append(", ");
				}
				sb.Append(mapTitleList[mapTitleList.Count - 1]);

				// 저장할 데이터 만들기
				Dictionary<string, object> data = new Dictionary<string, object>();

				data.Add("isUse", true);
				data.Add("makingMapTitleList", sb.ToString());
				data.Add(mapTitle, compressedJson);

				// 데이터 저장
				await CloudSaveService.Instance.Data.Player.SaveAsync(data, saveOption);
			}
			finally
			{
				// 유니티 서비스 로그아웃
				AuthenticationService.Instance.SignOut();
			}
		}
		public static async Awaitable<List<StageData>> LoadAllStageData()
		{
			List<StageData> stageDataList = new List<StageData>();

			try
			{
				// 유니티 서비스 로그인
				await AuthenticationService.Instance.SignInAnonymouslyAsync();

				List<string> playerIdList = await GetPlayerIdList();

				for (int i = 0; i < playerIdList.Count; ++i)
				{
					string playerId = playerIdList[i];

					LoadAllOptions loadAllOption = new LoadAllOptions(new PublicReadAccessClassOptions(playerId));

					// 데이터 불러오기
					var loadData = await CloudSaveService.Instance.Data.Player.LoadAllAsync(loadAllOption);

					foreach (var item in loadData)
					{
						// index 건너뛰기
						if (item.Key.Equals("isUse") == true ||
							item.Key.Equals("makingMapTitleList") == true)
							continue;

						byte[] compressedJson = item.Value.Value.GetAs<byte[]>();

						// 압축 해제
						string json = await Compression.Decompress(compressedJson);

						// Json 변환
						StageData data = JsonUtility.FromJson<StageData>(json);

						stageDataList.Add(data);
					}
				}
			}
			finally
			{
				// 유니티 서비스 로그아웃
				AuthenticationService.Instance.SignOut();
			}

			return stageDataList;
		}
		public static async Awaitable<List<StageData>> LoadAllStageData(string mapTitleFilter)
		{
			List<StageData> stageDataList = new List<StageData>();

			try
			{
				// 유니티 서비스 로그인
				await AuthenticationService.Instance.SignInAnonymouslyAsync();

				List<string> playerIdList = await GetPlayerIdList(mapTitleFilter);

				for (int i = 0; i < playerIdList.Count; ++i)
				{
					string playerId = playerIdList[i];

					LoadAllOptions loadAllOption = new LoadAllOptions(new PublicReadAccessClassOptions(playerId));

					// 데이터 불러오기
					var loadData = await CloudSaveService.Instance.Data.Player.LoadAllAsync(loadAllOption);

					foreach (var item in loadData)
					{
						// index 건너뛰기
						if (item.Key.Equals("isUse") == true || // isUse 인덱스 예외처리
							item.Key.Equals("makingMapTitleList") == true || // makingMapTitleList 인덱스 예외처리
							item.Key.Contains(mapTitleFilter) == false) // 검색 필터 예외처리
							continue;

						byte[] compressedJson = item.Value.Value.GetAs<byte[]>();

						// 압축 해제
						string json = await Compression.Decompress(compressedJson);

						// Json 변환
						StageData data = JsonUtility.FromJson<StageData>(json);

						stageDataList.Add(data);
					}
				}
			}
			finally
			{
				// 유니티 서비스 로그아웃
				AuthenticationService.Instance.SignOut();
			}

			return stageDataList;
		}

		private static async Awaitable<string> GetMakingMapTitleStr(string playerId)
		{
			LoadOptions loadOption = new LoadOptions(new PublicReadAccessClassOptions(playerId));

			// 기존에 만든 맵 이름 리스트 가져오기
			HashSet<string> keySet = new HashSet<string>() { "makingMapTitleList" };
			var loadData = await CloudSaveService.Instance.Data.Player.LoadAsync(keySet, loadOption);

			if (loadData.TryGetValue("makingMapTitleList", out Item mapTitleListItem) == false)
				return string.Empty;

			string mapTitleStr = mapTitleListItem.Value.GetAsString();

			return mapTitleStr;
		}
		private static async Awaitable<List<string>> GetMakingMapTitleList(string playerId)
		{
			string mapTitleStr = await GetMakingMapTitleStr(playerId);

			if (mapTitleStr == string.Empty)
				return new List<string>();

			List<string> mapTitleList = new List<string>(mapTitleStr.Split(", "));

			return mapTitleList;
		}

		private static async Awaitable<List<string>> GetPlayerIdList()
		{
			var query = new Query(
				new List<FieldFilter>()
				{
					new FieldFilter("isUse", true, FieldFilter.OpOptions.EQ, true)
				}
			);

			var results = await CloudSaveService.Instance.Data.Player.QueryAsync(query, new QueryOptions());

			List<string> playerIdList = new List<string>();

			for (int i = 0; i < results.Count; ++i)
			{
				playerIdList.Add(results[i].Id);
			}

			return playerIdList;
		}
		private static async Awaitable<List<string>> GetPlayerIdList(string mapTitleFilter)
		{
			var query = new Query(
				new List<FieldFilter>()
				{
					new FieldFilter("isUse", true, FieldFilter.OpOptions.EQ, true),
					new FieldFilter("makingMapTitleList", string.Empty, FieldFilter.OpOptions.NE, true),
				}
			);

			var results = await CloudSaveService.Instance.Data.Player.QueryAsync(query, new QueryOptions());

			List<string> playerIdList = new List<string>();

			for (int i = 0; i < results.Count; ++i)
			{
				string mapTitleStr = await GetMakingMapTitleStr(results[i].Id);

				if (mapTitleStr.Contains(mapTitleFilter))
					playerIdList.Add(results[i].Id);
			}

			return playerIdList;
		}

		public static class Compression
		{
			public static async Task<byte[]> Compress(string source)
			{
				var bytes = Encoding.UTF8.GetBytes(source);

				await using var input = new MemoryStream(bytes);
				await using var output = new MemoryStream();
				await using var brotliStream = new BrotliStream(output, System.IO.Compression.CompressionLevel.Optimal);

				await input.CopyToAsync(brotliStream);
				await brotliStream.FlushAsync();

				return output.ToArray();
			}

			public static async Task<string> Decompress(byte[] compressed)
			{
				await using var input = new MemoryStream(compressed);
				await using var brotliStream = new BrotliStream(input, CompressionMode.Decompress);

				await using var output = new MemoryStream();

				await brotliStream.CopyToAsync(output);
				await brotliStream.FlushAsync();

				return Encoding.UTF8.GetString(output.ToArray());
			}
		}
	}
}