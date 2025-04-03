using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.ad1a.Enum;
using AvantGardeMaker.ad1a;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Text;
using System.Linq;

namespace AvantGardeMaker.ad1a
{
	[System.Serializable]
	public class EnemyFixedData
	{
		[field: SerializeField]
		public E_EnemyType EnemyType { get; set; }
		[field: SerializeField]
		//보호 지점에 들어가면 깎이는 목표 HP
		public int LossHp { get; set; }
		[field: SerializeField]

		public E_EnemyRaceType RaceType { get; set; }
		[field: SerializeField]
		public E_EnemyFlyable Flyable { get; set; }
		[field: SerializeField]
		public E_EnemyAtkPatternType AtkPattern { get; set; }
		[field: SerializeField]
		public E_EnemyDmgType DmgType { get; set; }

		[field: SerializeField]
		//사정거리(근거리는 -1)
		public FixedCombatStatValue<float> Range { get; set; }
		[field: SerializeField]
		//무게
		public FixedCombatStatValue<int> Weight { get; set; }
		[field: SerializeField]
		//기절 수면 빙결 공중 전율 공포 면역여부
		public E_EnemyImmuneType ImmuneType { get; set; }
	}

	[System.Serializable]
	public class EnemyVariableData
	{
		[field: SerializeField]
		//체력
		public VariableCombatStatValue<float> Hp { get; set; }
		[field: SerializeField]
		//공격력
		public VariableCombatStatValue<float> Atk { get; set; }
		[field: SerializeField]
		//방어력
		public VariableCombatStatValue<float> Def { get; set; }
		[field: SerializeField]
		//마법 저항
		public VariableCombatStatValue<float> Res { get; set; }
		[field: SerializeField]
		//이동 속도(타일/s)
		public VariableCombatStatValue<float> MovementSpeed { get; set; }
		[field: SerializeField]
		//공격 간격(n초당 1회)
		public VariableCombatStatValue<float> Aspd { get; set; }
		[field: SerializeField]
		//원소 내성
		public VariableCombatStatValue<float> ElementalRes { get; set; }
		[field: SerializeField]
		//피해 감소
		public VariableCombatStatValue<float> EffectResistance { get; set; }
	}

	/*[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Object/EnemyData", order = int.MinValue)]*/
	[System.Serializable]
	//적 스펙, 특성
	public class EnemyData
	{
		//이름(키 값임)
		public string Name;

		[field: SerializeField]
		//고정된 값
		public EnemyFixedData FixedData { get; set; }
		[field: SerializeField]
		//전투중 바뀔 수 있는 값
		public EnemyVariableData VariableData { get; set; }

		public EnemyData()
		{
			Name = string.Empty;
			FixedData = new EnemyFixedData();
			VariableData = new EnemyVariableData();
		}
	}
}