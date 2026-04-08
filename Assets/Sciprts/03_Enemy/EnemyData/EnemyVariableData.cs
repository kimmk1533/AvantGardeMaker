using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.EnemySpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.EnemySpace
{
	[System.Serializable]
	public struct EnemyVariableData
	{
		// 체력
		public EnemyVariableCombatStatValue<float> Hp;
		// 공격력
		public EnemyVariableCombatStatValue<float> Atk;
		// 방어력
		public EnemyVariableCombatStatValue<float> Def;
		// 마법 저항
		public EnemyVariableCombatStatValue<float> Res;
		// 이동 속도(타일/s)
		public EnemyVariableCombatStatValue<float> MovementSpeed;
		// 공격 간격(n초당 1회)
		public EnemyVariableCombatStatValue<float> Aspd;
		// 원소 내성
		public EnemyVariableCombatStatValue<float> ElementalRes;
		// 손상 저항
		public EnemyVariableCombatStatValue<float> EffectResistance;
	}
}