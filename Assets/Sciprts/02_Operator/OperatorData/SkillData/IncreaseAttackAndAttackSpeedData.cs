using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	[CreateAssetMenu(fileName = "ScriptableOperatorIncreaseAttackAndAttackSpeedData", menuName = "Scriptable Object/ScriptableOperatorSkill/IncreaseAttackAndAttackSpeedData", order = int.MinValue)]
	public class IncreaseAttackAndAttackSpeedData : OperatorSkillData
	{
		[Title("IncreaseAttackAndAttackSpeedData")]
		public float IncreaseAttackSpeedValue;
		public float IncreaseAttackValue;
		public override IOperatorSkill CreateSkill()
		{
			IncreaseAttackAndAttackSpeed newIncreaseAttackAndAttackSpeed = new IncreaseAttackAndAttackSpeed();
			newIncreaseAttackAndAttackSpeed.SetOperatorSkillData(this);
			newIncreaseAttackAndAttackSpeed.SetIncreaseAttackSpeedValue(IncreaseAttackSpeedValue);
			newIncreaseAttackAndAttackSpeed.SetIncreaseAttackValue(IncreaseAttackValue);
			return newIncreaseAttackAndAttackSpeed;
		}
	}
}