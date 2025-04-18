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
		//스킬 이름
		private string m_SkillName;

		//스킬 코스트최대값
		private float m_SkillCostMax;
		//스킬 코스트현재값
		private float m_SkillInitCost;
		//스킬 코스트시작값
		private float m_StartSkillCost;
		//스킬 코스트 획득형식
		private E_SkillCostGainType m_SkillCostGainType;

		//스킬 발동타입
		private E_SkillActivationType m_SkillActivationType;

		//스킬 지속시간
		private UtilClass.Timer m_SkillActiveTime = null;

		//스킬 텍스트
		private string m_SkillText;
		#endregion

		#region 프로퍼티
		public string skillName
		{
			get => m_SkillName;
			set => m_SkillName = value;
		}
		public float skillCostMax
		{
			get => m_SkillCostMax;
			set => m_SkillCostMax = value;
		}
		public float skillInitCost
		{
			get => m_SkillInitCost;
			set => m_SkillInitCost = value;
		}
		public float startSkillCost
		{
			get => m_StartSkillCost;
			set => m_StartSkillCost = value;
		}
		public E_SkillCostGainType skillCostGainType
		{
			get => m_SkillCostGainType;
			set => m_SkillCostGainType = value;
		}
		public E_SkillActivationType skillActivationType
		{
			get => m_SkillActivationType;
			set => m_SkillActivationType = value;
		}
		public UtilClass.Timer skillActiveTime
		{
			get => m_SkillActiveTime;
			set => m_SkillActiveTime = value;
		}
		
		public string skillText
		{
			get=> m_SkillText;
			set => m_SkillText = value;
		}
		
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