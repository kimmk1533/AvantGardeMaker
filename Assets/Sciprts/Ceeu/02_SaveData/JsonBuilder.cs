using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.Ceeu
{
	public static class JsonBuilder
	{
		public static void Serialize<T>(string filePath, T value)
		{
			string json = JsonUtility.ToJson(value, true);

			File.WriteAllText(filePath, json);
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