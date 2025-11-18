using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;


namespace AvantGardeMaker.OperatorSpace
{
	public class OperatorSkill : IOperatorSkill, ISPRecover
	{
		#region 변수
		#endregion

		#region 프로퍼티
		public string SkillName { get; protected set; }
		public OperatorSkillData OperatorSkillInfo { get; set; }
		public int SkillLevel { get; }
		public bool IsSkillActive { get; set; }
		public bool IsSkillEnd { get; set; }
		public bool OnSkillButton { get; set; }
		public float currentSP { get; protected set; }

		public E_SPGainType GainType { get; protected set; }
		

		private readonly System.Action _onActivate;

		#endregion

		#region 생성자

		#endregion

		#region 매니져

		#endregion

		public void OnAttack()
		{
			if (GainType != E_SPGainType.Attack)
				return;
			TryActivate();
		}

		public void OnTakeAttack()
		{
			if (GainType != E_SPGainType.Hit)
				return;
			TryActivate();
		}

		public void OnTick()
		{
			if (GainType != E_SPGainType.Auto)
				return;
			TryActivate();
		}

		public void RecoverSP(int value)
		{
			if (currentSP + value > OperatorSkillInfo.MaxSP)
			{
				Debug.Log("Sp충전완료 Sp: " + currentSP);
				currentSP = OperatorSkillInfo.MaxSP;
			}
			else
			{
				Debug.Log("Sp: " + currentSP);
				currentSP += value;
			}
		}

		public void TryActivate()
		{
			if (currentSP >= OperatorSkillInfo.MaxSP)
			{
				_onActivate?.Invoke();
				currentSP = 0;
			}
		}

		public virtual void Activate(OperatorData operatorData)
		{
		
		}

		public virtual void OnClickSkillEvent(OperatorData operatorData)
		{
			
		}
	}
}