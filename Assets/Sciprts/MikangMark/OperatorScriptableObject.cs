using System.Collections;
using System.Collections.Generic;
using System.IO;
using AvantGardeMaker.MikangMark.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.MikangMark
{
	[CreateAssetMenu(fileName = "OperatorInfo", menuName = "Scriptable Object/OperatorInfo", order = int.MinValue)]
	public class OperInfo : SerializedScriptableObject
	{
		//영어이름
		public string EngOperName;
		//한글이름
		public string KorOperName;
		//레어도
		public int Rate;
		//최대레벨
		public int MaxLevel;
		//현재레벨
		public int Level;
		//현재레벨 최대경험치
		public int MaxExp;
		//현재경험치
		public int Exp;
		//특성
		public int Elite;
		//재능
		public int Potential;
		//직군
		public E_Jop Job;
		//최대체력
		public int MaxHp;
		//공격력
		public int Atk;
		//방어력
		public int Def;
		//마항
		public int Res;
		//재배치속도
		public E_ResetSpeed ReSet;
		//배치코스트
		public int SetCost;
		//저지
		public int BlockCount;
		//공격속도
		public E_AttackSpeed AtkSpeed;
		//공격범위
		public E_AttackRange AtkRange;
		//도발
		public int Provocation;
		//스킬레벨
		public int SkillLevel;
		//공격범위좌표
		public Vector2[] AttackPos;
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