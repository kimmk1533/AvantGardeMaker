using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;


namespace AvantGardeMaker.OperatorSpace
{
	public abstract class OperatorSkill : IOperatorSkill, ISPRecover
	{

		#region 변수
		#endregion

		#region 프로퍼티
		public abstract string SkillName { get; protected set; }
		public abstract OperatorSkillData OperatorSkillInfo { get; set; }
		public abstract int SkillLevel { get; }
		public abstract int SkillValue { get; }
		public abstract bool IsSkillActive { get; set; }
		public float currentSP { get; protected set; }

		public E_SPGainType GainType { get; protected set; }

		private readonly System.Action _onActivate;

		#endregion

		#region 생성자

		#endregion

		#region 매니져

		#endregion

		public abstract void Activate(OperatorData operatorData);

		public void OnAttack()
		{
			if (GainType != E_SPGainType.Attack)
				return;
			TryActivate();
		}

		public void OnTakeAttack()
		{
			if (GainType != E_SPGainType.TakeAttack)
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
			if (currentSP < OperatorSkillInfo.MaxSP)
			{
				if (currentSP + value > OperatorSkillInfo.MaxSP)
				{
					currentSP = OperatorSkillInfo.MaxSP;
				}
				else
				{
					currentSP += value;
				}
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
	}
}