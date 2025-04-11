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
	public class OperatorData : SerializedScriptableObject
	{
		//영어이름
		public string EngName;
		//한글이름
		public string KorName;
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
		public E_JopType Job;
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
		public E_RedeploySpeed RedeploySpeed;
		//배치코스트
		public int DeploymentCost;
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
		public E_OperatorDirection HorizontalDirection;
		public E_OperatorDirection VerticalDirection;

		public Sprite RightDownImg;
		public Sprite LeftDownImg;
		public Sprite RightUpImg;
		public Sprite LeftUpImg;
		public string PortraitPath;

		public OperatorData Clone()
		{
			OperatorData newInfo = CreateInstance<OperatorData>();

			newInfo.EngName = EngName;
			newInfo.KorName = KorName;
			newInfo.Rate = Rate;
			newInfo.MaxLevel = MaxLevel;
			newInfo.Level = Level;
			newInfo.MaxExp = MaxExp;
			newInfo.Exp = Exp;
			newInfo.Elite = Elite;
			newInfo.Potential = Potential;
			newInfo.Job = Job;
			newInfo.MaxHp = MaxHp;
			newInfo.RealHp = RealHp;
			newInfo.Atk = Atk;
			newInfo.Def = Def;
			newInfo.Res = Res;
			newInfo.RedeploySpeed = RedeploySpeed;
			newInfo.DeploymentCost = DeploymentCost;
			newInfo.BlockCount = BlockCount;
			newInfo.AtkSpeed = AtkSpeed;
			newInfo.AtkRange = AtkRange;
			newInfo.Provocation = Provocation;
			newInfo.SkillLevel = SkillLevel;
			newInfo.AttackPos = (Vector2[])AttackPos.Clone(); // 배열은 복사
			newInfo.HorizontalDirection = HorizontalDirection;
			newInfo.VerticalDirection = VerticalDirection;
			newInfo.RightDownImg = RightDownImg;
			newInfo.LeftDownImg = LeftDownImg;
			newInfo.RightUpImg = RightUpImg;
			newInfo.LeftUpImg = LeftUpImg;
			newInfo.PortraitPath = PortraitPath;

			return newInfo;
		}
	}
}