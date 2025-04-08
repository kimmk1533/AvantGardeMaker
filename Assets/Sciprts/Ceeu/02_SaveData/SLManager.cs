using System.Collections;
using System.Collections.Generic;
using System.IO;
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
	public static class SLManager
	{
		public static async void Serialize<T>(string filePath, string key, T value)
		{
			string json = JsonUtility.ToJson(value, true);

			File.WriteAllText(filePath, json);

			byte[] data = File.ReadAllBytes(filePath);

			// 유니티 서비스 초기화
			await UnityServices.InitializeAsync();

			// 유니티 서비스 로그인
			await AuthenticationService.Instance.SignInAnonymouslyAsync();

			// 유니티 서비스에 저장
			await CloudSaveService.Instance.Files.Player.SaveAsync(key, data);

			// 유니티 서비스 로그아웃
			AuthenticationService.Instance.SignOut();
		}
		public static T Deserialize<T>(string filePath)
		{
			if (File.Exists(filePath) == false)
			{
				Debug.LogError("파일이 존재하지 않습니다. 경로: " + filePath);
				return default;
			}

			string json = File.ReadAllText(filePath);

			return JsonUtility.FromJson<T>(json);
		}
	}
}