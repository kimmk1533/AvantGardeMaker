using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	[System.Serializable]
	public struct OperatorFixedData
	{
		// 최대 체력
		public OperatorLevelData<float> MaxHp;
		// 공격력
		public OperatorLevelData<float> Atk;
		// 방어력
		public OperatorLevelData<float> Def;
		// 마법 저항력
		public OperatorLevelData<float> Res;
		// 배치 코스트
		public OperatorLevelData<int> DeploymentCost;
		// 저지
		public OperatorLevelData<int> BlockCount;
		// 최대 레벨
		public OperatorLevelData<int> MaxLevel;

		// 레어도
		public int Rate;
		// 현재 레벨
		public int Level;
		// 특성 단계
		public int EliteLevel;
		// 재능 단계
		public int PotentialLevel;
		// 직군
		public E_OperatorClass OperatorClass;
		// 공격 속도 (ex)느림 빠름)
		public E_AttackSpeed AtkSpeed;
		// 스킬 이름
		public string SkillName;
		// 스킬레벨
		public int SkillLevel;
		// 초상화 에셋 경로
		public string PortraitImagePath;
		// 전신 이미지 에셋 경로
		public string FullShotImagePath;
	}
}