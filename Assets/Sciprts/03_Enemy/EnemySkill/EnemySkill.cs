using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.EnemySpace
{
	public class EnemySkill : SerializedBehaviour
	{
		//공격 시
		public interface IOnAttackSkill
		{
			public void OnAttackSkill(Operator targetOper,float damage);
			public void Initialize();
			public void Finallize();
		}

		//사망 시
		public interface IOnDeadSkill
		{
			public void OnDeadSkill(float damage);
			public void Initialize();
			public void Finallize();
		}
	}
}