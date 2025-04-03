using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.ad1a;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.Ceeu
{
	public class EnemySaveData
	{
		#region 변수
		#endregion

		#region 프로퍼티
		/// <summary>
		/// 적 이름
		/// </summary>
		public string EnemyName { get; set; }
		/// <summary>
		/// 적 체력
		/// </summary>
		public VariableCombatStatValue<float> Hp { get; set; }
		/// <summary>
		/// 적 공격력
		/// </summary>
		public VariableCombatStatValue<float> AtkPoint { get; set; }
		/// <summary>
		/// 적 방어력
		/// </summary>
		public VariableCombatStatValue<float> DefPoint { get; set; }
		/// <summary>
		/// 적 마법저항력
		/// </summary>
		public VariableCombatStatValue<float> MagicRes { get; set; }
		/// <summary>
		/// 적 피해 감소율
		/// </summary>
		public VariableCombatStatValue<float> DmgRes { get; set; }
		#endregion
	}
}