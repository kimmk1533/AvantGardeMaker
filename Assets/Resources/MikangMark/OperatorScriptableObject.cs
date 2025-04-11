using System.Collections;
using System.Collections.Generic;
using System.IO;
using AvantGardeMaker.MikangMark.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

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
		public float MaxExp;
		//현재경험치
		public float Exp;
		//특성
		public int Elite;
		//재능
		public int Potential;
		//직군
		public E_Jop Job;
		//최대체력
		public float MaxHp;
		//현재체력
		public float RealHp;
		//공격력
		public float Atk;
		//방어력
		public float Def;
		//마항
		public float Res;
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
		//오퍼 방향 디폴트 왼쪽
		public E_OperatorDirection E_OperDirection_H;
		public E_OperatorDirection E_OperDirection_V;

		public Sprite RightDownImg;
		public Sprite LeftDownImg;
		public Sprite RightUpImg;
		public Sprite LeftUpImg;

		public OperInfo Clone()
		{
			return new OperInfo
			{
				EngOperName = this.EngOperName,
				KorOperName = this.KorOperName,
				Rate = this.Rate,
				MaxLevel = this.MaxLevel,
				Level = this.Level,
				MaxExp = this.MaxExp,
				Exp = this.Exp,
				Elite = this.Elite,
				Potential = this.Potential,
				Job = this.Job,
				MaxHp = this.MaxHp,
				RealHp = this.RealHp,
				Atk = this.Atk,
				Def = this.Def,
				Res = this.Res,
				ReSet = this.ReSet,
				SetCost = this.SetCost,
				BlockCount = this.BlockCount,
				AtkSpeed = this.AtkSpeed,
				AtkRange = this.AtkRange,
				Provocation = this.Provocation,
				SkillLevel = this.SkillLevel,
				AttackPos = (Vector2[])this.AttackPos.Clone(), // 배열은 복사
				E_OperDirection_H = this.E_OperDirection_H,
				E_OperDirection_V = this.E_OperDirection_V,
				RightDownImg = this.RightDownImg,
				LeftDownImg = this.LeftDownImg,
				RightUpImg = this.RightUpImg,
				LeftUpImg = this.LeftUpImg
			};
		}
		
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