using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.EnemySpace
{
	[CreateAssetMenu(menuName ="EnemySkill/Stealth",order =int.MinValue)]
	public class StealthData : EnemySkillData
	{
		//은신 지속시간(음수: 무한)
		public bool IsInfinity;
		public float Duration;
	}
}