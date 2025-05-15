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
		#endregion

		#region 매니저
		public GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;
		#endregion
		#endregion
		public void SetOperatorSkillData(OperatorSkillData skillData)
		{
			OperatorSkillInfo = skillData;
			IsSkillActive = false;
			IsSkillEnd = true;
		}

		public void SetRecoverCostValue(int value)
		{
			m_RecoverCostValue = value;
		}

		public override void Activate(OperatorData operatorData)
		{
			base.Activate(operatorData);
			Debug.Log("Activate");
			if (OperatorSkillInfo.MaxSP > currentSP)
				return;
			Debug.Log("코스트회복");
			currentSP = 0;
			M_GamePlaying.currentCost += m_RecoverCostValue;
		}

		public override void OnClickSkillEvent(OperatorData operatorData)
		{
			base.OnClickSkillEvent(operatorData);
			return;
		}
	}
}