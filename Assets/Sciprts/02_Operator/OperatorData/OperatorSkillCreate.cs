using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace.Enum;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	[System.Serializable]
	public abstract class OperatorSkillCreate: OperatorSkillData
	{
		public abstract OperatorSkill CreateSkill();
	}
}