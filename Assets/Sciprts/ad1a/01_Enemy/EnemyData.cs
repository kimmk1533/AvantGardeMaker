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
		//이름(키 값임)
		public string Name { get; set; }
		[field: SerializeField]
		public E_EnemyGrade Grade { get; set; }
		[field: SerializeField]
		//보호 지점에 들어가면 깎이는 목표 HP
		public int LossHp { get; set; }
		[field: SerializeField]

		public E_EnemyRaceType TribeType { get; set; }
		[field: SerializeField]
		public E_EnemyFlyable Flyable { get; set; }
		[field: SerializeField]
		public E_EnemyAtkType AtkType { get; set; }
		[field: SerializeField]
		public E_EnemyDmgType DmgType { get; set; }

		[field: SerializeField]
		//이동 속도(타일/s)
		public VariableCombatStatValue<float> MoveSpeed { get; set; }
		[field: SerializeField]
		//공격 간격(n초당 1회)
		public VariableCombatStatValue<float> AtkTime { get; set; }
		[field: SerializeField]
		//사정거리(근거리는 -1)
		public FixedCombatStatValue<float> Range { get; set; }
		[field: SerializeField]
		//무게
		public FixedCombatStatValue<int> MassLevel { get; set; }
		[field: SerializeField]
		//기절 수면 빙결 공중 전율 공포 면역여부
		public E_EnemyImmuneType ImmuneType { get; set; }

		public string GetJsonVal()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(JsonUtility.ToJson(Name, true));
			sb.Append(JsonUtility.ToJson(Grade, true));
			sb.Append(JsonUtility.ToJson(LossHp, true));
			sb.Append(JsonUtility.ToJson(TribeType, true));
			sb.Append(JsonUtility.ToJson(Flyable, true));
			sb.Append(JsonUtility.ToJson(AtkType, true));
			sb.Append(JsonUtility.ToJson(DmgType, true));
			sb.Append(MoveSpeed.GetJsonVal());
			sb.Append(AtkTime.GetJsonVal());
			sb.Append(Range.GetJsonVal());
			sb.Append(MassLevel.GetJsonVal());
			sb.Append(JsonUtility.ToJson(ImmuneType, true));
			return sb.ToString();
		}
	}

	[System.Serializable]
	public class EnemyVariableData
	{
		[field: SerializeField]
		//이름(키 값임)
		public string Name { get; set; }
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
		public VariableCombatStatValue<float> MagicRes { get; set; }
		[field: SerializeField]
		//원소 내성
		public VariableCombatStatValue<float> ElementRes { get; set; }
		[field: SerializeField]
		//피해 감소
		public VariableCombatStatValue<float> DmgRes { get; set; }

		public string GetJsonVal()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(JsonUtility.ToJson(Name));
			sb.Append(Hp.GetJsonVal());
			sb.Append(Atk.GetJsonVal());
			sb.Append(Def.GetJsonVal());
			sb.Append(MagicRes.GetJsonVal());
			sb.Append(ElementRes.GetJsonVal());
			sb.Append(DmgRes.GetJsonVal());
			return sb.ToString();
		}
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

		public string GetJsonVal()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(JsonUtility.ToJson(Name));
			sb.Append(FixedData.GetJsonVal());
			sb.Append(VariableData.GetJsonVal());
			return sb.ToString();
		}
	}
}