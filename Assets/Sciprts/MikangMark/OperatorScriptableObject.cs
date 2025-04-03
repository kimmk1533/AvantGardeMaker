using System.Collections;
using System.Collections.Generic;
using System.IO;
using AvantGardeMaker.MikangMark.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.MikangMark
{
	[CreateAssetMenu(fileName = "OperatorInfo", menuName = "Scriptable Object/OperatorInfo", order = int.MaxValue)]
	public class OperInfo : SerializedScriptableObject
	{
		//영어이름
		[SerializeField]
		public string EngOperName { get; set; }
		[SerializeField]
		//한글이름
		public string KorOperName { get; set; }
		[SerializeField]
		//레어도
		public int Rate { get; set; }
		[SerializeField]
		//최대레벨
		public int MaxLevel { get; set; }
		[SerializeField]
		//현재레벨
		public int Level { get; set; }
		[SerializeField]
		//현재레벨 최대경험치
		public int MaxExp { get; set; }
		[SerializeField]
		//현재경험치
		public int Exp { get; set; }
		[SerializeField]
		//특성
		public int Elite { get; set; }
		[SerializeField]
		//재능
		public int Potential { get; set; }
		[SerializeField]
		//직군
		public E_Jop Job { get; set; }
		[SerializeField]
		//최대체력
		public int MaxHp { get; set; }
		[SerializeField]
		//공격력
		public int Atk { get; set; }
		[SerializeField]
		//방어력
		public int Def { get; set; }
		[SerializeField]
		//마항
		public int Res { get; set; }
		[SerializeField]
		//재배치속도
		public E_ResetSpeed ReSet { get; set; }
		[SerializeField]
		//배치코스트
		public int SetCost { get; set; }
		[SerializeField]
		//저지
		public int BlockCount { get; set; }
		[SerializeField]
		//공격속도
		public E_AttackSpeed AtkSpeed { get; set; }
		[SerializeField]
		//공격범위
		public E_AttackRange AtkRange { get; set; }
		[SerializeField]
		//도발
		public int Provocation { get; set; }
		[SerializeField]
		//스킬레벨
		public int SkillLevel { get; set; }
		[SerializeField]
		//공격범위좌표
		public Vector2[] AttackPos { get; set; }
		public void SaveJsonData(string directory, string fileName, OperInfo operData)
		{
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			// 저장할 파일의 전체 경로
			string filePath = Path.Combine(directory, fileName);

			// JSON 직렬화
			string jsonContent = JsonUtility.ToJson(operData, true);

			// JSON 파일 저장
			File.WriteAllText(filePath, jsonContent);
		}

		public OperInfo LoadJsonData(string directory, string fileName)
		{
			string folderPath = Path.Combine(Application.persistentDataPath, directory);
			string filePath = Path.Combine(folderPath, fileName);
			if (File.Exists(filePath))
			{
				string jsonContent = File.ReadAllText(filePath);
			}
			else
			{
				Debug.Log("파일이 존재하지 않습니다.");
			}
			return new OperInfo();
			//return character;
		}
	}
}