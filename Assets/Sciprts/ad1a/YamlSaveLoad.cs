using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using YamlDotNet.Serialization;

namespace AvantGardeMaker
{
	[Serializable]
	public class MapData
	{
		public string m_MapName;
		public int m_Width;
		public int m_Height;
		public List<UnitData> m_Enemies;
		public List<UnitData> m_Allies;
	}

	[Serializable]
	public class UnitData
	{
		public string m_Name;
		public int m_Hp;
		public int m_Atk;
		public float m_PosX;
		public float m_PosY;
	}
	public class YamlSaveLoad : SerializedMonoBehaviour
	{
		#region 변수
		private string m_FilePath;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수
		private void Start()
		{
			m_FilePath = Path.Combine(Application.persistentDataPath, "mapData.yaml");
		}
		#endregion

		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{

		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{

		}

		//데이터를 YAML로 저장
		public void SaveData(MapData mapData)
		{
			var serializer = new SerializerBuilder().Build();
			string yaml = serializer.Serialize(mapData);

			File.WriteAllText(m_FilePath, yaml);
			Debug.Log($"YAML 저장 완료: {m_FilePath}");
		}

		//YAML 파일을 불러와 객체로 변환
		public MapData LoadData()
		{
			if (!File.Exists(m_FilePath))
			{
				Debug.LogWarning("파일이 존재하지 않습니다.");
				return null;
			}

			string yaml = File.ReadAllText(m_FilePath);
			var deserializer = new DeserializerBuilder().Build();
			MapData mapData = deserializer.Deserialize<MapData>(yaml);

			return mapData;
		}
	}
}