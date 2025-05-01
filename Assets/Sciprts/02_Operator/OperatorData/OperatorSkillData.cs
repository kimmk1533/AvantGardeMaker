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
		public float MaxSkillCost;
		public float CurrentSkillCost;
		public float StartSkillCost;
		public E_SkillCostGainType GainSkillCostType;
		public E_SkillActivationType ActivationSkillType;
		public UtilClass.Timer ActiveSkillTime;
		public string SkillText;

		public Dictionary<E_OperatorSkillType,SkillAbilityInfo> SkillAbilityInfoList;

		public SkillInfo(SkillInfo skillData)
		{
			MaxSkillCost = skillData.MaxSkillCost;
			CurrentSkillCost = skillData.CurrentSkillCost;
			StartSkillCost = skillData.StartSkillCost;
			GainSkillCostType = skillData.GainSkillCostType;
			ActivationSkillType = skillData.ActivationSkillType;
			ActiveSkillTime = skillData.ActiveSkillTime;
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