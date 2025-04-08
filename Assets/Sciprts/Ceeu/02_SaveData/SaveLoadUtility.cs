using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;

namespace AvantGardeMaker.Ceeu
{
	/// <summary>
	/// Save & Load Manager
	/// </summary>
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

		public static async Task SaveData<T>(string dataName, T value)
		{
			try
			{
				// 유니티 서비스 로그인
				await AuthenticationService.Instance.SignInAnonymouslyAsync();

				// Json 변환
				string json = JsonUtility.ToJson(value);

				// 압축
				byte[] compressedJson = await Compression.Compress(json);

				Dictionary<string, object> data = new Dictionary<string, object>();
				data.Add(dataName, compressedJson);

				// 데이터 저장
				await CloudSaveService.Instance.Data.Player.SaveAsync(data);
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

				// 데이터 불러오기
				var loadData = await CloudSaveService.Instance.Data.Player.LoadAllAsync();

				if (loadData.TryGetValue(dataName, out var value) == false)
					throw new System.Exception("Failed To Load Data");

				// 압축 해제
				byte[] compressedJson = value.Value.GetAs<byte[]>();

				// Json 변환
				string json = await Compression.Decompress(compressedJson);

				data = JsonUtility.FromJson<T>(json);
			}
			finally
			{
				// 유니티 서비스 로그아웃
				AuthenticationService.Instance.SignOut();
			}

			return data;
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