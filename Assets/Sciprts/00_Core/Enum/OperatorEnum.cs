using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace.Enum
{
	public enum E_OperatorClass
	{
		VanGuard,
		Guard,
		Defender,
		Sniper,
		Caster,
		Medic,
		Supporter,
		Specialist,
	}

	// 배치 방향
	public enum E_OperatorDirection
	{
		None = -1,

		Setting,
		Left,
		Up,
		Right,
		Down,
	}

	// 표기 공격 속도
	public enum E_AttackSpeed
	{
		// 느림 (1.75초 이상)
		VerySlow,
		// 다소 느림 (1.25초 이상)
		Slow,
		// 중간 (1초 초과)
		Normal,
		// 빠름 (0.8초 초과)
		Fast,
		// 매우 빠름 (0.8초 이하)
		VeryFast,
	}
	// 표기 재배치 속도
	public enum E_RedeploymentSpeed
	{
		// 매우 느림 (101초 이상)
		VerySlow,
		// 느림 (100초 이하)
		Slow,
		// 중간 (60초 이하)
		Normal,
		// 빠름 (20초 이하)
		Fast,
	}
	// SP(스킬 포인트) 획득 타입
	public enum E_SPGainType
	{
		// 자동 획득
		Auto,
		// 공격시 획득
		Attack,
		// 피격시 획득
		Hit,
	}
	// 스킬 발동 타입
	public enum E_SkillActivationType
	{
		// 자동 발동
		Auto,
		// 수동 발동
		Menual,
		// 패시브
		Passive,
	}
}