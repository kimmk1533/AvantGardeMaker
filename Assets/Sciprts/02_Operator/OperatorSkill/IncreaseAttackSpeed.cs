using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	public class IncreaseAttackSpeed : OperatorSkill
	{
		#region 기본 템플릿
		#region 변수
		private float m_IncreaseAttackSpeedValue;
		float increasedAttackSpeed = 0f;
		#endregion

		#region 프로퍼티
		public override string SkillName { get; protected set; }

		public override OperatorSkillData OperatorSkillInfo { get; set; }

		public override int SkillLevel { get; }

		public override int SkillValue { get; }

		public override bool IsSkillActive { get; set; }
		public override bool IsSkillEnd { get; set; }
		public override bool OnSkillButton { get; set; }

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

		public void SetIncreaseStatusValue(float value)
		{
			m_IncreaseAttackSpeedValue = value;
		}

		public override void Activate(OperatorData operatorData)
		{
			OperatorSkillInfo.ActiveSkillTimer.Update();
			if (OperatorSkillInfo.ActiveSkillTimer.TimeCheck(true) == true)
			{
				Debug.Log("ActivateOff");
				operatorData.VariableData.CurrentAttackSpeed -= increasedAttackSpeed;
				IsSkillActive = false;
			}
		}

		public override void OnClickSkillEvent(OperatorData operatorData, float value)
		{
			if(OperatorSkillInfo.MaxSP <= currentSP)
			{
				Debug.Log("ActivateOn");
				increasedAttackSpeed = operatorData.VariableData.CurrentAttackSpeed * value;
				operatorData.VariableData.CurrentAttackSpeed += increasedAttackSpeed;
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