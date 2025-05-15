using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	[CreateAssetMenu(fileName = "ScriptableOperatorRecoverCostData", menuName = "Scriptable Object/ScriptableOperatorSkill/RecoverCostSkillData", order = int.MinValue)]
	public class RecoverCostData : OperatorSkillCreate
	{
		[Title("RecoverCostData")]
		public int RecoverCostValue;

		public override OperatorSkill CreateSkill()
		{
			RecoverCost newRecoverCost = new RecoverCost();
			newRecoverCost.SetOperatorSkillData(this);
			newRecoverCost.SetRecoverCostValue(RecoverCostValue);
			return newRecoverCost;
		}

	}
}