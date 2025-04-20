using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.EnemySpace
{
	/// <summary>
	/// 랭크가 필요 없는 스탯들임
	/// </summary>
	[System.Serializable]
	public struct EnemyFixedCombatStatValue<T>
	{
		public T InitStat;
		public T CurStat;

		public EnemyFixedCombatStatValue(T val)
		{
			InitStat = CurStat = val;
		}
		public EnemyFixedCombatStatValue(T initVal, T curVal)
		{
			InitStat = initVal;
			CurStat = curVal;
		}
	}
}