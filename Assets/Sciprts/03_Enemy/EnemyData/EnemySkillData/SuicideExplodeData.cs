using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using AvantGardeMaker.CoreSpace.Enum;

namespace AvantGardeMaker.EnemySpace
{
	[CreateAssetMenu(fileName = "ScriptableEnemySuicideExplodeData", menuName = "Scriptable Object/ScriptableEnemySkill/SuicideExplodeSkillData", order = int.MinValue)]
	public class SuicideExplodeData : EnemySkillData
	{
		public float AoeRadius;
		public float DamageCoefficient;
		public E_DamageType DamageType;

		public override IEnemySkill CreateSkill()
		{
			SuicideExplode newSuicideExplode = new SuicideExplode();
			newSuicideExplode.SetAoeRadius(AoeRadius);
			newSuicideExplode.SetDamageCoef(DamageCoefficient);
			newSuicideExplode.SetDamageType(DamageType);
			return newSuicideExplode;
		}
	}
}