using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.EnemySpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.EnemySpace
{
	[System.Serializable]
	public class EnemyVariableData
	{
		// 체력
		public EnemyVariableCombatStatValue<float> Hp = new EnemyVariableCombatStatValue<float>(0f, E_EnemyRankType.E);
		// 공격력
		public EnemyVariableCombatStatValue<float> Atk = new EnemyVariableCombatStatValue<float>(0f, E_EnemyRankType.E);
		// 방어력
		public EnemyVariableCombatStatValue<float> Def = new EnemyVariableCombatStatValue<float>(0f, E_EnemyRankType.E);
		// 마법 저항
		public EnemyVariableCombatStatValue<float> Res = new EnemyVariableCombatStatValue<float>(0f, E_EnemyRankType.E);
		// 이동 속도(타일/s)
		public EnemyVariableCombatStatValue<float> MovementSpeed = new EnemyVariableCombatStatValue<float>(0f, E_EnemyRankType.E);
		// 공격 간격(n초당 1회)
		public EnemyVariableCombatStatValue<float> Aspd = new EnemyVariableCombatStatValue<float>(0f, E_EnemyRankType.E);
		// 원소 내성
		public EnemyVariableCombatStatValue<float> ElementalRes = new EnemyVariableCombatStatValue<float>(0f, E_EnemyRankType.E);
		// 피해 감소
		public EnemyVariableCombatStatValue<float> EffectResistance = new EnemyVariableCombatStatValue<float>(0f, E_EnemyRankType.E);
	}
}