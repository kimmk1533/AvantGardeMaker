using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	public class IncreaseAttack : OperatorSkill
	{
		#region 기본 템플릿
		#region 변수
		private float m_IncreaseAttackValue;
		float increasedAttack = 0f;
		#endregion

		#region 프로퍼티
		#endregion
		#endregion
		public void SetOperatorSkillData(OperatorSkillData skillData)
		{
			OperatorSkillInfo = skillData;
			IsSkillActive = false;
			IsSkillEnd = true;
			OperatorSkillInfo.ActiveSkillTimer = new UtilClass.Timer(skillData.ActiveSkillTimer);
			OnSkillButton = false;
		}

		public void SetIncreaseAttackValue(float value)
		{
			m_IncreaseAttackValue = value;
		}

		public override void Activate(OperatorData operatorData)
		{
			OperatorSkillInfo.ActiveSkillTimer.Update();
			if (OperatorSkillInfo.ActiveSkillTimer.TimeCheck(true) == true)
			{
				Debug.Log("공격력원상복귀");
				operatorData.VariableData.Atk -= increasedAttack;
				IsSkillActive = false;
			}
		}

		public override void OnClickSkillEvent(OperatorData operatorData)
		{
			if(OperatorSkillInfo.MaxSP <= currentSP)
			{
				Debug.Log("공격력증가");
				increasedAttack = operatorData.VariableData.Atk * m_IncreaseAttackValue;
				operatorData.VariableData.Atk += increasedAttack;
				IsSkillActive = true;
				currentSP = 0;
				OperatorSkillInfo.ActiveSkillTimer.Clear();
			}
			else
			{
				Debug.Log("Sp부족");
			}
		}
	}
}