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
		//종족
		public E_EnemyRaceType RaceType;
		public string Code;
		//원 / 근거리
		public E_EnemyAtkPatternType AtkPattern;
		//물리 / 마법
		public E_EnemyDmgType DmgType;

		//등급
		public E_EnemyType EnemyType;
		//보호 지점에 들어가면 깎이는 목표 HP
		public int LossHp;

		//지상 / 공중
		public E_EnemyFlyable Flyable;

		//▲▲▲▲▲▲▲▲▲▲▲▲적 정보 상단▲▲▲▲▲▲▲▲▲▲▲▲//

		//적 초상화 경로
		public string PortraitImagePath;
		//무게
		public FixedCombatStatValue<int> Weight;

		//사정거리(근거리는 -1)
		public FixedCombatStatValue<float> Range;

		//▲▲▲▲▲▲▲▲▲▲▲▲적 정보 중단▲▲▲▲▲▲▲▲▲▲▲▲//

		//적 설명
		public string Lore;

		//▲▲▲▲▲▲▲▲▲▲▲▲적 정보 하단▲▲▲▲▲▲▲▲▲▲▲▲//

		//적 능력
		public string Ability;

		//▲▲▲▲▲▲▲▲▲▲▲▲능력▲▲▲▲▲▲▲▲▲▲▲▲//

		//기절 수면 빙결 공중 전율 공포 면역여부
		public E_EnemyImmuneType ImmuneType;

		//▲▲▲▲▲▲▲▲▲▲▲▲내성▲▲▲▲▲▲▲▲▲▲▲▲//
	}

	[System.Serializable]
	public class EnemyVariableData
	{
		//체력
		public VariableCombatStatValue<float> Hp;
		//공격력
		public VariableCombatStatValue<float> Atk;
		//방어력
		public VariableCombatStatValue<float> Def;
		//마법 저항
		public VariableCombatStatValue<float> Res;
		//이동 속도(타일/s)
		public VariableCombatStatValue<float> MovementSpeed;
		//공격 간격(n초당 1회)
		public VariableCombatStatValue<float> Aspd;
		//원소 내성
		public VariableCombatStatValue<float> ElementalRes;
		//피해 감소
		public VariableCombatStatValue<float> EffectResistance;
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
		public EnemyFixedData FixedData;
		[field: SerializeField]
		//전투중 바뀔 수 있는 값
		public EnemyVariableData VariableData;

		public EnemyData()
		{
			Name = string.Empty;
			FixedData = new EnemyFixedData();
			VariableData = new EnemyVariableData();
		}
	}
}