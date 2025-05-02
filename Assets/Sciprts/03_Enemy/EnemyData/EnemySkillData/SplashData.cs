using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using AvantGardeMaker.CoreSpace.Enum;

namespace AvantGardeMaker.EnemySpace
{
	[CreateAssetMenu(fileName = "ScriptableEnemySplashData", menuName = "Scriptable Object/ScriptableEnemySkill/SplashSkillData", order = int.MinValue)]
	public class SplashData : EnemySkillData
	{
		public float AoeRadius;
		public float DamageCoefficient;
		public E_DamageType DamageType;

		public override IEnemySkill CreateSkill()
		{
			Splash newSplash = new Splash();
			newSplash.SetAoeRadius(AoeRadius);
			newSplash.SetDamageCoef(DamageCoefficient);
			newSplash.SetDamageType(DamageType);
			return newSplash;
		}
	}
}