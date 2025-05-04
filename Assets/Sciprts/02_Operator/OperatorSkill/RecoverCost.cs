using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using AvantGardeMaker.CoreSpace;

namespace AvantGardeMaker.OperatorSpace
{
	public class RecoverCost : OperatorSkill
	{
		#region 기본 템플릿
		#region 변수
		private int m_RecoverCostValue;

		public override string SkillName { get; protected set; }

		public override OperatorSkillData OperatorSkillInfo { get; }

		public override int SkillLevel { get; }

		public override int SkillValue { get; }

		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트

		#region 이벤트 함수
		#endregion
		#endregion

		#region 매니저
		public GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;
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

		public void SetRecoverCostValue(int value)
		{
			m_RecoverCostValue = value;
		}

		public override void Activate()
		{
			if (OperatorSkillInfo.MaxSP > currentSP)
				return;
			currentSP = 0;
			M_GamePlaying.currentCost += m_RecoverCostValue;
		}

	}
}