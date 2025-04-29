using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using AvantGardeMaker.CoreSpace.Enum;

namespace AvantGardeMaker.EnemySpace
{
	public class SuicideExplodeData:EnemySkillData
	{
		public float AoeRadius;
		public float DamageCoefficient;
		public E_DamageType DmgType;
	}
}