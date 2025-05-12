using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	public interface IOperatorSkill
	{
		public string SkillName { get; }
		public int SkillLevel { get; }
		public bool IsSkillActive { get; set; }
		public bool IsSkillEnd { get; set; }
		public bool OnSkillButton { get; set; }

		public void Activate(OperatorData operatorData);
		public void RecoverSP(int value);
		public void OnClickSkillEvent(OperatorData operatorData);

	}

	public interface ISPRecover
	{
		public E_SPGainType GainType { get; }
		//시간경과시 획득
		public void OnTick();
		//공격시 획득
		public void OnAttack();
		//피격시 획득
		public void OnTakeAttack();
	}

}