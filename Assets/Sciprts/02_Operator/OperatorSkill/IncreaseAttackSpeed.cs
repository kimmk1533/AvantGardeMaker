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
		#endregion

		#region 이벤트

		#region 이벤트 함수
		#endregion
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{

		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{

		}
		#endregion
		#endregion
		public void SetOperatorSkillData(OperatorSkillData skillData)
		{
			OperatorSkillInfo = skillData;
			IsSkillActive = false;
			OperatorSkillInfo.ActiveSkillTimer = new UtilClass.Timer(skillData.ActiveSkillTimer);
		}

		public void SetIncreaseStatusValue(float value)
		{
			m_IncreaseAttackSpeedValue = value;
		}

		public override void Activate(OperatorData operatorData)
		{
			if (OperatorSkillInfo.MaxSP > currentSP)
				return;
				
			IsSkillActive = true;
			float increasedAttackSpeed = operatorData.VariableData.CurrentAttakSpeed * m_IncreaseAttackSpeedValue;
			operatorData.VariableData.CurrentAttakSpeed += increasedAttackSpeed;
			OperatorSkillInfo.ActiveSkillTimer.Update();
			if (OperatorSkillInfo.ActiveSkillTimer.TimeCheck(false) == true)
			{
				operatorData.VariableData.CurrentAttakSpeed -= increasedAttackSpeed;
				IsSkillActive = false;
			}
			currentSP = 0;
		}
	}
}