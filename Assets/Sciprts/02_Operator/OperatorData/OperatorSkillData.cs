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
		public float MaxSP;
		public float CurrentSP;
		public float StartSP;
		public E_SPGainType GainSPType;
		public E_SkillActivationType ActivationSkillType;
		public UtilClass.Timer ActiveSkillTime;
		public string SkillText;

		public Dictionary<E_OperatorSkillType,SkillAbilityInfo> SkillAbilityInfoList;

		public SkillInfo(SkillInfo skillData)
		{
			MaxSP = skillData.MaxSP;
			CurrentSP = skillData.CurrentSP;
			StartSP = skillData.StartSP;
			GainSPType = skillData.GainSPType;
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