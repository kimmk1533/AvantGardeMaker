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
		#endregion

		#region 프로퍼티
		public override string SkillName { get; protected set; }

		public override OperatorSkillData OperatorSkillInfo { get; set; }

		public override int SkillLevel { get; }

		public override int SkillValue { get; }

		public override bool IsSkillActive { get; set; }
		public override bool IsSkillEnd { get; set; }
		#endregion
		#endregion
		public void SetOperatorSkillData(OperatorSkillData skillData)
		{
			OperatorSkillInfo = skillData;
			IsSkillActive = false;
			IsSkillEnd = true;
			OperatorSkillInfo.ActiveSkillTimer = new UtilClass.Timer(skillData.ActiveSkillTimer);
		}

		public void SetIncreaseStatusValue(float value)
		{
			m_IncreaseAttackSpeedValue = value;
		}

		public override void Activate(OperatorData operatorData)
		{
			Debug.Log("Activate");
			if (IsSkillActive == true || OperatorSkillInfo.MaxSP <= currentSP)
			{
				Debug.Log("MaxSP: " + OperatorSkillInfo.MaxSP);
				Debug.Log("currentSP: " + currentSP);
				float increasedAttackSpeed = operatorData.VariableData.CurrentAttakSpeed * m_IncreaseAttackSpeedValue;
				if (IsSkillActive == false)
				{
					Debug.Log("FirstActivate");
					IsSkillActive = true;
					IsSkillEnd = false;
					Debug.Log("IncreaseBack: " + operatorData.VariableData.CurrentAttakSpeed);
					operatorData.VariableData.CurrentAttakSpeed += increasedAttackSpeed;
					Debug.Log("IncreaseNow: " + operatorData.VariableData.CurrentAttakSpeed);
					currentSP = 0;
				}
				else
				{
					OperatorSkillInfo.ActiveSkillTimer.Update();
					if (OperatorSkillInfo.ActiveSkillTimer.TimeCheck(false) == true)
					{
						operatorData.VariableData.CurrentAttakSpeed -= increasedAttackSpeed;
						Debug.Log("IncreaseEnd: " + operatorData.VariableData.CurrentAttakSpeed);
						IsSkillActive = false;
						IsSkillEnd = true;
					}
				}
			}
		}
	}
}