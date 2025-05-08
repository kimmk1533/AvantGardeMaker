using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace.Enum;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	[System.Serializable]
	public abstract class OperatorSkillData : SerializedScriptableObject
	{
		[Title("OpratorSkillData")]
		public string Name;
		public float MaxSP;
		public float InitSP;
		public E_SPGainType SPGainType;
		public E_SkillActivationType SkillActivationType;
		public UtilClass.Timer ActiveSkillTimer = null;
		public string SkillInfoText = null;
		public OperatorSkillData()
		{
			Name = string.Empty;
			ActiveSkillTimer = new UtilClass.Timer();
			SkillInfoText = string.Empty;
		}

		public OperatorSkillData(OperatorSkillData skillInfo)
		{
			name = skillInfo.Name;
			MaxSP = skillInfo.MaxSP;
			InitSP = skillInfo.InitSP;
			SPGainType = skillInfo.SPGainType;
			SkillActivationType = skillInfo.SkillActivationType;
			ActiveSkillTimer = new UtilClass.Timer(skillInfo.ActiveSkillTimer);
			SkillInfoText = skillInfo.SkillInfoText;
		}
		public abstract IOperatorSkill CreateSkill();
	}
}