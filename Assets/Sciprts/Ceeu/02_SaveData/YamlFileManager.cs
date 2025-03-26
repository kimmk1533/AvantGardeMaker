using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using YamlDotNet.Serialization;

namespace AvantGardeMaker.Ceeu
{
	// 출처: https://velog.io/@opzerg/YamlDotNet
	public sealed class YamlFileManager : SerializedSingleton<YamlFileManager>
	{
		public void Serialize<T>(string filePath, T value)
		{
			var serializer = new SerializerBuilder()
				.WithIndentedSequences()
				.Build();

			using (var stream = File.Open(filePath, FileMode.Create, FileAccess.Write))
			{
				using (var writer = new StreamWriter(stream))
				{
					serializer.Serialize(writer, value);
				}
			}
		}
		public void Serialize<T>(string filePath, string fileName, T value)
		{
			Serialize(Path.Combine(filePath, fileName), value);
		}

		public T Deserialize<T>(string filePath)
		{
			if (File.Exists(filePath) == false)
			{
				Debug.LogError("파일이 존재하지 않습니다. 경로: " + filePath);
				return default;
			}

			var deserializer = new DeserializerBuilder()
				.Build();

			using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
			{
				using (var reader = new StreamReader(stream))
				{
					return deserializer.Deserialize<T>(reader);
				}
			}
		}
		public T Deserialize<T>(string filePath, string fileName)
		{
			return Deserialize<T>(Path.Combine(filePath, fileName));
		}
	}
}