using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.MikangMark.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.MikangMark
{
	public class OperatorSkill : SerializedMonoBehaviour
	{
		#region 변수
		//스킬코스트최대값
		private float m_SkillCostMax;
		//스킬코스트현재값
		private float m_SkillInitCost;
		//스킬코스트시작값
		private float m_StartSkillCost;
		//스킬 코스트 획득형식
		private E_SkillCostGainType m_SkillCostGainType;

		//스킬 발동타입
		private E_SkillActivationType m_SkillActivationType;

		//스킬 타입
		private List<E_OperatorSkillType> m_SkillType;
		private List<float> m_SkillValue;
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
	}
}