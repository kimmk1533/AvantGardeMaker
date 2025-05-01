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
			operatorSkill = CreateReChargeSkill();
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{

		}
		#endregion

		#region 직업스킬
		public OperatorSkill CreateReChargeSkill()
		{
			SkillInfo skillData = new SkillInfo(variableData.SkillData);

			return new SkillReChargeCost(skillData);
		}
		#endregion
	}

	public class SkillReChargeCost : OperatorSkill
	{
		public int reChargeCostValue { get; private set; }
		private SkillInfo skillInfo;

		public SkillReChargeCost(SkillInfo skillData) : base(skillData)
		{
			skillInfo = skillData;
			reChargeCostValue = (int)skillInfo.SkillAbilityInfoList[E_OperatorSkillType.GainCost].SkillValue;
		}
		public override void Activate()
		{
			if (skillInfo == null)
				return;
			if (currentSP >= maxSP)
			{
				M_GamePlaying.currentCost += reChargeCostValue;
				currentSP = 0;
			}
			
		}


	}
}