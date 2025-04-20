using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	public class Vangard : Operator
	{
		#region 변수
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
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
		public void SkillSetting()
		{
			operatorPositionSkill = new List<OperatorSkill>();
			OperatorSkill temp = new OperatorSkill();
			temp.skillName = m_OperatorData.FixedData.SkillName;
			temp.skillText = "";
			for (int i = 0; i < m_OperatorData.VariableData.SkillInfoList.Count; i++)
			{
				switch (m_OperatorData.VariableData.SkillInfoList[i].SkillType)
				{
					case E_OperatorSkillType.StatusBuff:
						break;
					case E_OperatorSkillType.ChargeCost:
						CostCargeSkillSetting(temp);
						break;
					case E_OperatorSkillType.MultipleShot:
						break;
					case E_OperatorSkillType.StopAttack:
						break;
					case E_OperatorSkillType.ChangeAttackRange:
						break;
					default:
						break;
				}
				if (i != m_OperatorData.VariableData.SkillInfoList.Count - 1)
				{
					temp.skillText += ",";
				}
				operatorPositionSkill.Add(temp);
			}
		}
		public void CostCargeSkillSetting(OperatorSkill tempSkill)
		{
			tempSkill.skillText += "배치 코스트 " + m_OperatorData.VariableData.SkillInfoList + " 즉시 획득";
		}

		public void ActiveSkill()
		{
			for (int i = 0; i < m_OperatorData.VariableData.SkillInfoList.Count; i++)
			{
				switch (m_OperatorData.VariableData.SkillInfoList[i].SkillType)
				{
					case E_OperatorSkillType.StatusBuff:
						break;
					case E_OperatorSkillType.ChargeCost:
						break;
					case E_OperatorSkillType.MultipleShot:
						break;
					case E_OperatorSkillType.StopAttack:
						break;
					case E_OperatorSkillType.ChangeAttackRange:
						break;
					default:
						break;
				}
			}
		}

	}
}