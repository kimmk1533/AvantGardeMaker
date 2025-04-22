using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	[System.Serializable]
	public class OperatorFixedData
	{
		// 최대체력
		public OperatorLevelData<float> MaxHp;
		// 공격력
		public OperatorLevelData<float> Atk;
		// 방어력
		public OperatorLevelData<float> Def;
		// 마항
		public OperatorLevelData<float> Res;
		// 배치코스트
		public OperatorLevelData<int> DeploymentCost;
		// 저지
		public OperatorLevelData<int> BlockCount;
		//최대레벨
		public OperatorLevelData<int> MaxLevel;

		//레어도
		public int Rate;
		//현재레벨
		public int Level;
		//특성
		public int Elite;
		//재능
		public int Potential;
		//직군
		public E_JopType Job;
		//공격속도(ex)느림 빠름)
		public E_AttackSpeed AtkSpeed;
		//공격범위
		public E_AttackRange AtkRange;
		//스킬 이름
		public string SkillName;
		//스킬레벨
		public int SkillLevel;
		//초상화 에셋경로
		public string PortraitPath;
		//전신 이미지 에셋경로
		public string FullShotPath;
	}
}