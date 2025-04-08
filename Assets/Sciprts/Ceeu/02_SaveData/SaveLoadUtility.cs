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

		public static async Awaitable SaveData<T>(string dataName, T value)
		{
			try
			{
				SaveOptions saveOption = new SaveOptions(new PublicWriteAccessClassOptions());

				// 유니티 서비스 로그인
				await AuthenticationService.Instance.SignInAnonymouslyAsync();

				// Json 변환
				string json = JsonUtility.ToJson(value);

				// 압축
				byte[] compressedJson = await Compression.Compress(json);

				Dictionary<string, object> data = new Dictionary<string, object>();

				//string makingMapNameList = await GetMakingMapNameList();
				//if (makingMapNameList == null ||
				//	makingMapNameList == string.Empty)
				//	makingMapNameList = dataName;
				//else
				//	makingMapNameList += ", " + dataName;

				//data.Add("MakingMapNameList", makingMapNameList);
				data.Add("isUse", true);
				data.Add(dataName, compressedJson);

				// 데이터 저장
				await CloudSaveService.Instance.Data.Player.SaveAsync(data, saveOption);
			}
			finally
			{
				// 유니티 서비스 로그아웃
				AuthenticationService.Instance.SignOut();
			}
		}
		public static async Awaitable<T> LoadData<T>(string dataName)
		{
			T data = default(T);

			try
			{
				// 유니티 서비스 로그인
				await AuthenticationService.Instance.SignInAnonymouslyAsync();

				//string playerId = await GetPlayerIdList(dataName);
				string playerId = "";

				LoadAllOptions loadAllOption = new LoadAllOptions(new PublicReadAccessClassOptions(playerId));

				// 데이터 불러오기
				var loadData = await CloudSaveService.Instance.Data.Player.LoadAllAsync(loadAllOption);

				if (loadData.TryGetValue(dataName, out var value) == false)
					throw new System.Exception("Failed To Load Data");

				byte[] compressedJson = value.Value.GetAs<byte[]>();

				// 압축 해제
				string json = await Compression.Decompress(compressedJson);

				// Json 변환
				data = JsonUtility.FromJson<T>(json);
			}
			finally
			{
				// 유니티 서비스 로그아웃
				AuthenticationService.Instance.SignOut();
			}

			return data;
		}

		private static async Awaitable<string> GetMakingMapNameList()
		{
			LoadOptions loadOption = new LoadOptions(new PublicReadAccessClassOptions());

			// 본인이 기존에 만든 맵 이름 리스트 가져오기
			HashSet<string> keySet = new HashSet<string>() { "MakingMapNameList" };
			var loadData = await CloudSaveService.Instance.Data.Player.LoadAsync(keySet, loadOption);
			loadData.TryGetValue("MakingMapNameList", out Item mapNameListItem);

			string mapNameList = mapNameListItem.Value.GetAsString();

			return mapNameList;
		}
		private static async Awaitable<List<string>> GetPlayerIdList(string dataName)
		{
			var query = new Query(
				new List<FieldFilter>()
				{
					new FieldFilter("isUse", true, FieldFilter.OpOptions.EQ, true)
				},
				new HashSet<string> { dataName }
			);

			var results = await CloudSaveService.Instance.Data.Player.QueryAsync(query, new QueryOptions());

			List<string> playerIdList = new List<string>();

			Debug.Log("Number of players found: " + results.Count);
			for (int i = 0; i < results.Count; ++i)
			{
				playerIdList.Add(results[i].Id);

				Debug.Log(results[i].Id);
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