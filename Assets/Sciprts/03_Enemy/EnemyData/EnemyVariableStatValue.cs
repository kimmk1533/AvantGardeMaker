using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.EnemySpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.EnemySpace
{
	/// <summary>
	/// 정보 창에서 랭크가 뜨는 스탯들임
	/// </summary>
	[System.Serializable]
	public struct EnemyVariableCombatStatValue<T>
	{
		public T InitStat;
		public T CurStat;
		public E_EnemyRankType Rank;

		public EnemyVariableCombatStatValue(T val, E_EnemyRankType rankType = E_EnemyRankType.E)
		{
			InitStat = CurStat = val;
			Rank = rankType;
		}
		public EnemyVariableCombatStatValue(T initVal, T curVal, E_EnemyRankType rankType = E_EnemyRankType.E)
		{
			InitStat = initVal;
			CurStat = curVal;
			Rank = rankType;
		}
	}
}