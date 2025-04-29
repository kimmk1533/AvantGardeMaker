using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	
	[System.Serializable]
	public class SkillInfo
	{
		public float SkillCostMax;
		public float SkillInitCost;
		public float StartSkillCost;
		public E_SkillCostGainType SkillCostGainType;
		public E_SkillActivationType SkillActivationType;
		public UtilClass.Timer SkillActiveTime;
		public string SkillText;

		public Dictionary<E_OperatorSkillType,SkillAbilityInfo> SkillAbilityInfoList;

		public SkillInfo(SkillInfo skillData)
		{
			SkillCostMax = skillData.SkillCostMax;
			SkillInitCost = skillData.SkillInitCost;
			StartSkillCost = skillData.StartSkillCost;
			SkillCostGainType = skillData.SkillCostGainType;
			SkillActivationType = skillData.SkillActivationType;
			SkillActiveTime = skillData.SkillActiveTime;
			SkillText = skillData.SkillText;
			SkillAbilityInfoList = skillData.SkillAbilityInfoList;
		}
	}
	[System.Serializable]
	public class SkillAbilityInfo
	{
		public E_OperatorSkillType SkillType;
		public float SkillValue;

		public float GetSkillTypeValue(E_OperatorSkillType skillType)
		{
			return SkillValue;
		}
	}
}