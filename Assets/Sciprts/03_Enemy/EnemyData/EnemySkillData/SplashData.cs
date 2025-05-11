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

		public override EnemySkill CreateSkill()
		{
			Splash newSplash = new Splash();
			newSplash.AoeRadius = AoeRadius;
			newSplash.DamageCoefficient = DamageCoefficient;
			newSplash.DamageType = DamageType;
			return newSplash;
		}
	}
}