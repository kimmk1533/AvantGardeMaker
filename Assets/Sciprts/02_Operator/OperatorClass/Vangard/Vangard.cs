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

		#region 직업스킬
		public OperatorSkill CreateReChargeSkill()
		{
			SkillInfo skillData = new SkillInfo(variableData.SkillData);

			return new ReChargeCostSkill(skillData);
		}
		#endregion
	}

	public class ReChargeCostSkill : OperatorSkill
	{
		public int reChargeCostValue { get; private set; }

		public ReChargeCostSkill(SkillInfo skillData) : base(skillData)
		{
			reChargeCostValue = (int)skillData.SkillAbilityInfoList[0].SkillValue;
		}
		public override void Activate()
		{
			M_GamePlaying.currentCost += reChargeCostValue;
		}
	}
}