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
		public E_OperatorSkillType SkillType;
		public float SkillValue;

		public float GetSkillTypeValue(E_OperatorSkillType skillType)
		{
			return SkillValue;
		}
	}
}