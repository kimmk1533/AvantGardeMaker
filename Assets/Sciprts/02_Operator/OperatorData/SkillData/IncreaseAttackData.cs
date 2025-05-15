using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	[CreateAssetMenu(fileName = "ScriptableOperatorIncreaseAttackData", menuName = "Scriptable Object/ScriptableOperatorSkill/IncreaseAttackData", order = int.MinValue)]
	public class IncreaseAttackData : OperatorSkillCreate
	{
		[Title("IncreaseAttackData")]
		public float IncreaseAttackValue;
		public override OperatorSkill CreateSkill()
		{
			IncreaseAttack newIncreaseAttack = new IncreaseAttack();
			newIncreaseAttack.SetOperatorSkillData(this);
			newIncreaseAttack.SetIncreaseAttackValue(IncreaseAttackValue);
			return newIncreaseAttack;
		}
	}
}