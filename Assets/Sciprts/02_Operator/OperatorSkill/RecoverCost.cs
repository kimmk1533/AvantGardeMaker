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

		#region 매니저
		public GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;
		#endregion
		#endregion
		public void SetOperatorSkillData(OperatorSkillData skillData)
		{
			OperatorSkillInfo = skillData;
			IsSkillActive = false;
		}

		public void SetRecoverCostValue(int value)
		{
			m_RecoverCostValue = value;
		}

		public override void Activate(OperatorData operatorData)
		{
			if (OperatorSkillInfo.MaxSP > currentSP)
				return;
			currentSP = 0;
			M_GamePlaying.currentCost += m_RecoverCostValue;
		}

		public override void OnClickSkillEvent(OperatorData operatorData, float value)
		{
			throw new System.NotImplementedException();
		}
	}
}