using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using AvantGardeMaker.CoreSpace;

namespace AvantGardeMaker.OperatorSpace
{
	public class OperatorSkill
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

		#region 매니져
		public GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;
		#endregion
		public void UsingThisSkill(SkillInfo thisSkillInfo)
		{
			switch (thisSkillInfo.SkillType)
			{
				case E_OperatorSkillType.StatusBuff:
					break;
				case E_OperatorSkillType.ChargeCost:
					SkillGainCost((int)thisSkillInfo.SkillValue);
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

		public void SkillGainCost(int gainCostValue)
		{
			M_GamePlaying.GaintCost(gainCostValue);
		}
	}
}