using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.ad1a.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.ad1a
{
	[System.Serializable]
	public class EnemyVariableData
	{
		//체력
		public VariableCombatStatValue<float> Hp = new VariableCombatStatValue<float>(0f, E_EnemyRankType.E);
		//공격력
		public VariableCombatStatValue<float> Atk = new VariableCombatStatValue<float>(0f, E_EnemyRankType.E);
		//방어력
		public VariableCombatStatValue<float> Def = new VariableCombatStatValue<float>(0f, E_EnemyRankType.E);
		//마법 저항
		public VariableCombatStatValue<float> Res = new VariableCombatStatValue<float>(0f, E_EnemyRankType.E);
		//이동 속도(타일/s)
		public VariableCombatStatValue<float> MovementSpeed = new VariableCombatStatValue<float>(0f, E_EnemyRankType.E);
		//공격 간격(n초당 1회)
		public VariableCombatStatValue<float> Aspd = new VariableCombatStatValue<float>(0f, E_EnemyRankType.E);
		//원소 내성
		public VariableCombatStatValue<float> ElementalRes = new VariableCombatStatValue<float>(0f, E_EnemyRankType.E);
		//피해 감소
		public VariableCombatStatValue<float> EffectResistance = new VariableCombatStatValue<float>(0f, E_EnemyRankType.E);
	}
}