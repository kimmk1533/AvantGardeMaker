using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	[CreateAssetMenu(fileName = "ScriptableOperatorIncreaseAttackSpeedData", menuName = "Scriptable Object/ScriptableOperatorSkill/IncreaseAttackSpeedData", order = int.MinValue)]
	public class IncreaseAttackSpeedData : OperatorSkillData
	{
		[Title("IncreaseAttackSpeedData")]
		public float IncreaseAttackSpeedValue;
		public override IOperatorSkill CreateSkill()
		{
			IncreaseAttackSpeed newIncreaseAttackSpeed = new IncreaseAttackSpeed();
			newIncreaseAttackSpeed.SetOperatorSkillData(this);
			newIncreaseAttackSpeed.SetIncreaseStatusValue(IncreaseAttackSpeedValue);
			return newIncreaseAttackSpeed;
		}
	}
}