using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.EnemySpace
{
	public class EnemySkillInterface :MonoBehaviour
	{
		//공격 시
		public interface IOnAttackSkill
		{
			void OnAttackSkill();
		}

		//사망 시
		public interface IOnDeadSkill
		{
			void OnDeadSkill();
		}
	}
}