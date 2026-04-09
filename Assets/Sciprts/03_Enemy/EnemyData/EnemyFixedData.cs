using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace.Enum;
using AvantGardeMaker.EnemySpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.EnemySpace
{
	[System.Serializable]
	public struct EnemyFixedData
	{
		// 종족
		public E_EnemyRaceType RaceType;
		// 코드
		public string Code;
		// 원 / 근거리
		public E_EnemyAtkPatternType AtkPattern;
		// 물리 / 마법
		public E_EnemyDmgType DmgType;

		// 등급
		public E_EnemyGradeType EnemyType;
		// 보호 지점에 들어가면 깎이는 목표 HP
		public int LossHp;

		// 지상 / 공중
		public E_EnemyFlyable Flyable;

		//▲▲▲▲▲▲▲▲▲▲▲▲적 정보 상단▲▲▲▲▲▲▲▲▲▲▲▲//

		// 무게
		public int Weight;

		// 사정거리(근거리는 -1)
		public float Range;

		//▲▲▲▲▲▲▲▲▲▲▲▲적 정보 중단▲▲▲▲▲▲▲▲▲▲▲▲//

		// 적 설명 텍스트
		[TextArea(6, 20)]
		public string Description;

		//▲▲▲▲▲▲▲▲▲▲▲▲적 정보 하단▲▲▲▲▲▲▲▲▲▲▲▲//

		// 적 능력 텍스트
		[TextArea(6, 20)]
		public string Trait;

		//▲▲▲▲▲▲▲▲▲▲▲▲능력▲▲▲▲▲▲▲▲▲▲▲▲//

		// 기절 수면 빙결 공중 전율 공포 면역여부
		public E_EnemyImmuneType ImmuneType;

		//▲▲▲▲▲▲▲▲▲▲▲▲내성▲▲▲▲▲▲▲▲▲▲▲▲//

		// 필터에 표시되지 않는 공격 타입
		public E_DamageType DamageType;
	}
}