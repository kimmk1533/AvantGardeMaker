using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	public class IncreaseAttackAndAttackSpeed : OperatorSkill
	{
		#region 기본 템플릿
		#region 변수
		private float m_IncreaseAttackSpeedValue;
		private float m_IncreaseAtkValue;
		float increasedAttackSpeed = 0f;
		float increasedAtk = 0f;
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

		public void SetIncreaseAttackSpeedValue(float value)
		{
			m_IncreaseAttackSpeedValue = value;
		}
		public void SetIncreaseAttackValue(float value)
		{
			m_IncreaseAtkValue = value;
		}

		public override void Activate(OperatorData operatorData)
		{
			if (OperatorSkillInfo.ActiveSkillTimer.Update() == true)
			{
				Debug.Log("공속원상복귀");
				operatorData.VariableData.CurrentAttackSpeed -= increasedAttackSpeed;
				operatorData.VariableData.Atk -= increasedAtk;
				IsSkillActive = false;
			}
		}

		public override void OnClickSkillEvent(OperatorData operatorData)
		{
			if(OperatorSkillInfo.MaxSP <= currentSP)
			{
				Debug.Log("공속증가");
				increasedAttackSpeed = operatorData.VariableData.CurrentAttackSpeed * m_IncreaseAttackSpeedValue;
				increasedAtk = m_IncreaseAtkValue;

				operatorData.VariableData.CurrentAttackSpeed += increasedAttackSpeed;
				operatorData.VariableData.Atk += increasedAtk;
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