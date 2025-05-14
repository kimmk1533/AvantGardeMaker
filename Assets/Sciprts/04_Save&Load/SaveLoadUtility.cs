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
using DeleteOptions = Unity.Services.CloudSave.Models.Data.Player.DeleteOptions;

namespace AvantGardeMaker.CoreSpace.SaveLoad
{
	public static class SaveLoadUtility
	{
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

			// 유니티 서비스 로그인
			await AuthenticationService.Instance.SignInAnonymouslyAsync();
		}
		public static void Finallize()
		{
			// 유니티 서비스 로그아웃
			AuthenticationService.Instance.SignOut();
		}

		public static async Awaitable SaveData<T>(string key, T value)
		{
			SaveOptions saveOption = new SaveOptions(new PublicWriteAccessClassOptions());

			// 저장할 데이터 만들기
			Dictionary<string, object> data = new Dictionary<string, object>();

			data.Add(key, value);

			// 데이터 저장
			await CloudSaveService.Instance.Data.Player.SaveAsync(data, saveOption);
		}
		public static async Awaitable SaveJsonData<T>(string key, T value)
		{
			SaveOptions saveOption = new SaveOptions(new PublicWriteAccessClassOptions());

			// Json 변환
			string json = JsonUtility.ToJson(value);

			// 저장할 데이터 만들기
			Dictionary<string, object> data = new Dictionary<string, object>();

			data.Add(key, json);

			// 데이터 저장
			await CloudSaveService.Instance.Data.Player.SaveAsync(data, saveOption);
		}
		public static async Awaitable<T> LoadData<T>(string key)
		{
			LoadOptions loadOption = new LoadOptions(new PublicReadAccessClassOptions());

			HashSet<string> loadKeySet = new HashSet<string>()
			{
				key,
			};

			// 데이터 불러오기
			Dictionary<string, Item> loadData = await CloudSaveService.Instance.Data.Player.LoadAsync(loadKeySet, loadOption);

			if (loadData.TryGetValue(key, out Item item) == false)
				return default;

			return item.Value.GetAs<T>();
		}
		public static async Awaitable<T> LoadJsonData<T>(string key)
		{
			LoadOptions loadOption = new LoadOptions(new PublicReadAccessClassOptions());

			HashSet<string> loadKeySet = new HashSet<string>()
			{
				key,
			};

			// 데이터 불러오기
			Dictionary<string, Item> loadData = await CloudSaveService.Instance.Data.Player.LoadAsync(loadKeySet, loadOption);

			if (loadData.TryGetValue(key, out Item item) == false)
				return default;

			return JsonUtility.FromJson<T>(item.Value.GetAsString());
		}
		public static async Awaitable DeleteData(string key)
		{
			DeleteOptions deleteOption = new DeleteOptions(new PublicWriteAccessClassOptions());

			await CloudSaveService.Instance.Data.Player.DeleteAsync(key, deleteOption);
		}

		public static async Awaitable SaveStageData(string mapTitle, StageData stageData)
		{
			SaveOptions saveOption = new SaveOptions(new PublicWriteAccessClassOptions());

			float t1, t2;

			// Json 변환
			string json = JsonUtility.ToJson(stageData);

			// 디버깅용 파일 저장
			string debugJson = JsonUtility.ToJson(stageData, true);
			string debugPath = Path.Combine(Application.dataPath, "..", "Data");
			string debugFile = mapTitle + ".json";

			if (Directory.Exists(debugPath) == false)
				Directory.CreateDirectory(debugPath);

			File.WriteAllText(Path.Combine(debugPath, debugFile), debugJson);

			t1 = Time.realtimeSinceStartup;

			// 압축
			byte[] compressedJson = await Compression.Compress(json);

			t2 = Time.realtimeSinceStartup;

			Debug.Log("[압축]: " + (t2 - t1));

			t1 = Time.realtimeSinceStartup;

			// 기존 제작한 맵 타이틀 리스트 가져오기
			List<string> mapTitleList = await GetMakingMapTitleList(AuthenticationService.Instance.PlayerId);

			if (mapTitleList.Contains(mapTitle) == false)
				mapTitleList.Add(mapTitle);

			string mapTitlListData = string.Join(", ", mapTitleList);

			t2 = Time.realtimeSinceStartup;

			Debug.Log("[기존 제작한 맵 타이틀 리스트 가져오기]: " + (t2 - t1));

			// 저장할 데이터 제작
			Dictionary<string, object> data = new Dictionary<string, object>();

			data.Add("isUse", true);
			data.Add("makingMapTitleList", mapTitlListData);
			data.Add(mapTitle, compressedJson);

			// 데이터 저장
			await CloudSaveService.Instance.Data.Player.SaveAsync(data, saveOption);
		}
		public static async Awaitable DeleteStageData(string mapTitle)
		{
			DeleteOptions deleteOption = new DeleteOptions(new PublicWriteAccessClassOptions());

			await CloudSaveService.Instance.Data.Player.DeleteAsync(mapTitle, deleteOption);

			// 기존 제작한 맵 타이틀 리스트 가져오기
			List<string> mapTitleList = await GetMakingMapTitleList(AuthenticationService.Instance.PlayerId);

			mapTitleList.Remove(mapTitle);

			string mapTitlListData = string.Join(", ", mapTitleList);

			await SaveData<string>("makingMapTitleList", mapTitlListData);
		}

		public static async Awaitable<List<StageData>> LoadAllStageData()
		{
			List<StageData> stageDataList = new List<StageData>();

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
					if (IsIndexKey(item.Key)) // 인덱스 예외처리
						continue;

					byte[] compressedJson = item.Value.Value.GetAs<byte[]>();

					// 압축 해제
					string json = await Compression.ConvertToString(compressedJson);

					// Json 변환
					StageData data = JsonUtility.FromJson<StageData>(json);

					stageDataList.Add(data);
				}
			}

			return stageDataList;
		}
		public static async Awaitable<List<StageData>> LoadAllStageData(string mapTitleFilter)
		{
			List<StageData> stageDataList = new List<StageData>();

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
					if (IsIndexKey(item.Key) || // 인덱스 예외처리
						item.Key.Contains(mapTitleFilter) == false) // 검색 필터 예외처리
						continue;

					byte[] compressedJson = item.Value.Value.GetAs<byte[]>();

					// 압축 해제
					string json = await Compression.ConvertToString(compressedJson);

					// Json 변환
					StageData data = JsonUtility.FromJson<StageData>(json);

					stageDataList.Add(data);
				}
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

		public static string GetPlayerId()
		{
			return AuthenticationService.Instance.PlayerId;
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

		private static bool IsIndexKey(string key)
		{
			return key.Equals("isUse") || // isUse 인덱스 예외처리
				key.Equals("makingMapTitleList") || // makingMapTitleList 인덱스 예외처리
				key.Equals("nickName"); // nickName 인덱스 예외처리
		}

		public static class Compression
		{
			public static async Task<byte[]> Compress(string source)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(source);

				await using MemoryStream output = new MemoryStream();
				await using (BrotliStream brotliStream = new BrotliStream(output, System.IO.Compression.CompressionLevel.Optimal))
				{
					await brotliStream.WriteAsync(bytes, 0, bytes.Length);
				}

				return output.ToArray();

				//var bytes = Encoding.UTF8.GetBytes(source);

				//await using var input = new MemoryStream(bytes);
				//await using var output = new MemoryStream();
				//await using var brotliStream = new BrotliStream(output, System.IO.Compression.CompressionLevel.Optimal);

				//await input.CopyToAsync(brotliStream);
				//await brotliStream.FlushAsync();

				//return output.ToArray();
			}

			public static async Task<byte[]> Decompress(byte[] compressed)
			{
				await using var input = new MemoryStream(compressed);
				await using var brotliStream = new BrotliStream(input, CompressionMode.Decompress);

				await using var output = new MemoryStream();

				await brotliStream.CopyToAsync(output);
				await brotliStream.FlushAsync();

				return output.ToArray();
			}
			public static async Task<string> ConvertToString(byte[] compressed)
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