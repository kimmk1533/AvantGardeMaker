using System.Collections;
using System.Collections.Generic;
using System.IO;
using AvantGardeMaker.MikangMark.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AvantGardeMaker.MikangMark
{
	[CreateAssetMenu(fileName = "OperatorInfo", menuName = "Scriptable Object/OperatorInfo", order = int.MaxValue)]
	public class OperInfo : SerializedScriptableObject
	{
		public string m_FileSaveDirectory = "C:/Users/kimjh741963/Desktop/ARK3D/AvantGardeMaker/Assets/Sciprts/MikangMark/OperInfo_Yaml";

		public string m_EngOperName;//영어이름
		public string m_KorOperName;//한글이름
		public int m_Rate;//레어도

		public int m_MaxLevel;//최대레벨
		public int m_Level;//현재레벨
		public int m_MaxExp;//현재레벨 최대경험치
		public int m_Exp;//현재경험치

		public int m_Elite;//특성
		public int m_Potential;//재능
		public E_Jop m_Job;//직군

		public int m_MaxHp;//최대체력
		public int m_Atk;//공격력
		public int m_Def;//방어력
		public int m_Res;//마항
		public E_ResetSpeed m_ReSet;//재배치속도
		public int m_SetCost;//배치코스트
		public int m_BlockCount;//저지
		public E_AttackSpeed m_AtkSpeed;//공격속도
		public E_AttackRange m_AtkRange;//공격범위

		public int m_Provocation;//도발
		public int m_SkillLevel;//스킬레벨

		public E_TileAttackRange[,] m_AttackRange = new E_TileAttackRange[7, 7];//이차원배열로 오퍼위치(0), 오퍼의 공격범위(1), 오퍼의공격권외(2) 등을 정수형으로 저장
		
		public void SaveData(string directory, string fileName, OperInfo operData)
		{
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
			string m_FilePath = Path.Combine(directory, fileName);

			var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
			string yaml = serializer.Serialize(operData);
			File.WriteAllText(m_FilePath, yaml);
		}

		public OperInfo LoadData(string directory, string fileName)
		{
			/*
			if (!File.Exists(directory))
			{
				Debug.LogError("YAML 파일을 찾을 수 없습니다!");
				return new OperInfo();
			}
			*/
			string yaml = File.ReadAllText(directory);
			var deserializer = new DeserializerBuilder()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.Build();
			OperInfo character = deserializer.Deserialize<OperInfo>(yaml);

			return character;
		}
	}
}