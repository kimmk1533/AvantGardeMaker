using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace.Enum;
using AvantGardeMaker.EnemySpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.EnemySpace
{
	[System.Serializable]
	public class EnemyFixedData
	{
		//종족
		public E_EnemyRaceType RaceType = E_EnemyRaceType.None;
		//코드
		public string Code = string.Empty;
		//원 / 근거리
		public E_EnemyAtkPatternType AtkPattern = E_EnemyAtkPatternType.Melee;
		//물리 / 마법
		public E_EnemyDmgType DmgType = E_EnemyDmgType.Physic;

		//등급
		public E_EnemyGradeType EnemyType = E_EnemyGradeType.Normal;
		//보호 지점에 들어가면 깎이는 목표 HP
		public int LossHp = 1;

		//지상 / 공중
		public E_EnemyFlyable Flyable = E_EnemyFlyable.Walk;

		//▲▲▲▲▲▲▲▲▲▲▲▲적 정보 상단▲▲▲▲▲▲▲▲▲▲▲▲//

		//무게
		public EnemyFixedCombatStatValue<int> Weight = new EnemyFixedCombatStatValue<int>(0);

		//사정거리(근거리는 -1)
		public EnemyFixedCombatStatValue<float> Range = new EnemyFixedCombatStatValue<float>(-1f);

		//▲▲▲▲▲▲▲▲▲▲▲▲적 정보 중단▲▲▲▲▲▲▲▲▲▲▲▲//

		//적 설명 텍스트
		[TextArea(6, 20)]
		public string Description = string.Empty;

		//▲▲▲▲▲▲▲▲▲▲▲▲적 정보 하단▲▲▲▲▲▲▲▲▲▲▲▲//

		//적 능력 텍스트
		[TextArea(6, 20)]
		public string Trait = string.Empty;

		//▲▲▲▲▲▲▲▲▲▲▲▲능력▲▲▲▲▲▲▲▲▲▲▲▲//

		//기절 수면 빙결 공중 전율 공포 면역여부
		public E_EnemyImmuneType ImmuneType = 0;

		//▲▲▲▲▲▲▲▲▲▲▲▲내성▲▲▲▲▲▲▲▲▲▲▲▲//

		//필터에 표시되지 않는 공격 타입
		public E_DamageType DamageType = E_DamageType.Physics;

		//EnemySkill로 구현하기 애매한 능력
		public List<string> Tag;

		public List<EnemySkillData> SkillDataList = new List<EnemySkillData>();

		//▲▲▲▲▲▲▲▲▲▲▲▲스킬▲▲▲▲▲▲▲▲▲▲▲▲//

	}
}