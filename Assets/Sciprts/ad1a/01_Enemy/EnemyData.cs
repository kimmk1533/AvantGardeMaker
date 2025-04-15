using AvantGardeMaker.ad1a.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.ad1a
{
	[System.Serializable]
	public class EnemyFixedData
	{
		//종족
		public E_EnemyRaceType RaceType = E_EnemyRaceType.None;
		// 코드
		public string Code = string.Empty;
		//원 / 근거리
		public E_EnemyAtkPatternType AtkPattern = E_EnemyAtkPatternType.Melee;
		//물리 / 마법
		public E_EnemyDmgType DmgType = E_EnemyDmgType.Physic;

		//등급
		public E_EnemyType EnemyType = E_EnemyType.Normal;
		//보호 지점에 들어가면 깎이는 목표 HP
		public int LossHp = 1;

		//지상 / 공중
		public E_EnemyFlyable Flyable = E_EnemyFlyable.Walk;

		//▲▲▲▲▲▲▲▲▲▲▲▲적 정보 상단▲▲▲▲▲▲▲▲▲▲▲▲//

		//무게
		public FixedCombatStatValue<int> Weight = new FixedCombatStatValue<int>(0);

		//사정거리(근거리는 -1)
		public FixedCombatStatValue<float> Range = new FixedCombatStatValue<float>(-1f);

		//▲▲▲▲▲▲▲▲▲▲▲▲적 정보 중단▲▲▲▲▲▲▲▲▲▲▲▲//

		//적 설명
		[TextArea(6, 20)]
		public string Description = string.Empty;

		//▲▲▲▲▲▲▲▲▲▲▲▲적 정보 하단▲▲▲▲▲▲▲▲▲▲▲▲//

		//적 능력
		[TextArea(6, 20)]
		public string Trait = string.Empty;

		//▲▲▲▲▲▲▲▲▲▲▲▲능력▲▲▲▲▲▲▲▲▲▲▲▲//

		//기절 수면 빙결 공중 전율 공포 면역여부
		public E_EnemyImmuneType ImmuneType = 0;

		//▲▲▲▲▲▲▲▲▲▲▲▲내성▲▲▲▲▲▲▲▲▲▲▲▲//

		//필터에 표시되지 않는 공격 타입
		public E_DamageType DamageType = E_DamageType.Physics;
	}

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

	/*[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Object/EnemyData", order = int.MinValue)]*/
	[System.Serializable]
	[CreateAssetMenu(fileName = "ScriptableEnemyData", menuName = "Scriptable Object/ScriptableEnemyData", order = int.MinValue)]
	//적 스펙, 특성
	public class EnemyData : SerializedScriptableObject
	{
		//이름(키 값임)
		public string EngName = string.Empty;
		public string KorName = string.Empty;

		//적 초상화 경로
		public string PortraitImagePath = string.Empty;

		//고정된 값
		public EnemyFixedData FixedData = new EnemyFixedData();
		//전투중 바뀔 수 있는 값
		public EnemyVariableData VariableData = new EnemyVariableData();
	}

}