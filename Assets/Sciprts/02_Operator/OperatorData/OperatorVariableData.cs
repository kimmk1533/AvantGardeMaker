using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace.Enum;
using AvantGardeMaker.OperatorSpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	[System.Serializable]
	public struct OperatorVariableData
	{
		// 최대 체력
		public float MaxHp;
		// 현재 체력
		public float CurrentHp;
		// 공격력
		public float Atk;
		// 방어력
		public float Def;
		// 마항
		public float Res;
		// 재배치 속도
		public E_RedeploymentSpeed RedeploymentSpeed;
		// 재배치 실제 속도
		public float RedeploymentInterval;
		// 초기 배치 코스트
		public int DeploymentCost;
		// 저지
		public int BlockCount;
		// 공격 속도 실제 값
		public float CurrentAttackSpeed;
		// 도발
		public int Provocation;
		// 공격 범위 좌표
		public List<Vector2Int> AttackPos;
		// 공격 타입
		public E_DamageType DamageType;
		// 관통력
		public float Penetration;
		// 스킬 정보
		public OperatorSkillCreate SkillData;
	}
}