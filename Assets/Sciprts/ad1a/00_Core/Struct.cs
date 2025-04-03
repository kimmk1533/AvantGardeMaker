using System.Collections;
using System.Collections.Generic;
using System.Text;
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
		public string Rank { get; set; }

		public VariableCombatStatValue(T val)
		{
			InitStat = CurStat = val;
			Rank = string.Empty;
		}
		public VariableCombatStatValue(T initVal, T curVal)
		{
			InitStat = initVal;
			CurStat = curVal;
			Rank = string.Empty;
		}

		public string GetJsonVal()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(JsonUtility.ToJson(InitStat));
			sb.Append(JsonUtility.ToJson(CurStat));
			sb.Append(JsonUtility.ToJson(Rank));
			return sb.ToString();
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
		public string GetJsonVal()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(JsonUtility.ToJson(InitStat));
			sb.Append(JsonUtility.ToJson(CurStat));
			return sb.ToString();
		}
	}
}