using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	[System.Serializable]
	public class OperatorSkillData : SerializedScriptableObject
	{
		[Title("OpratorSkillData")]
		public string Name;
		public float MaxSP;
		public float InitSP;
		public E_SPGainType SPGainType;
		public E_SkillActivationType SkillActivationType;
		public UtilClass.Timer ActiveSkillTimer = null;
		public string SkillInfoText = null;
	}
}