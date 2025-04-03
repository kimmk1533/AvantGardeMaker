using System.Collections;
using System.Collections.Generic;
using System.Text;
using AvantGardeMaker.ad1a.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.ad1a
{
	[System.Serializable]
	/// <summary>
	/// 정보 창에서 랭크가 뜨는 스탯들임
	/// </summary>
	public struct VariableCombatStatValue<T>
	{
		[field: SerializeField]
		public T InitStat { get; set; }
		[field: SerializeField]
		public T CurStat { get; set; }
		[field: SerializeField]
		public E_EnemyRankType Rank { get; set; }

		public VariableCombatStatValue(T val, E_EnemyRankType rankType)
		{
			InitStat = CurStat = val;
			Rank = rankType;
		}
		public VariableCombatStatValue(T initVal, T curVal, E_EnemyRankType rankType)
		{
			InitStat = initVal;
			CurStat = curVal;
			Rank = rankType;
		}
	}

	[System.Serializable]
	/// <summary>
	/// 랭크가 필요 없는 스탯들임
	/// </summary>
	public struct FixedCombatStatValue<T>
	{
		[field: SerializeField]
		public T InitStat { get; set; }
		[field: SerializeField]
		public T CurStat { get; set; }

		public FixedCombatStatValue(T val)
		{
			InitStat = CurStat = val;
		}
		public FixedCombatStatValue(T initVal, T curVal)
		{
			InitStat = initVal;
			CurStat = curVal;
		}
	}
}