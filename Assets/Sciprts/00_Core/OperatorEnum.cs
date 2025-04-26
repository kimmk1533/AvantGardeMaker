using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace.Enum
{
	public enum E_ClassType
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

	public enum E_AttackRange
	{
		Close,
		Far,
	}

	public enum E_AttackSpeed
	{
		VerySlow,
		Slow,
		Nomal,
		Fast,
		VeryFast,
	}

	public enum E_RedeploymentSpeed
	{
		Slow,
		Nomal,
		Fast,
	}

	public enum E_TileAttackRange
	{
		OutRange = 1,
		OperPos,
		InRange,
	}

	public enum E_OperatorDirection
	{
		None = -1,

		Right,
		Down = 90,
		Left = 180,
		Up = 270,
	}

	public enum E_OperatorSkillType
	{
		StatusBuff,
		AttackBuff,
		AttackSpeedBuff,
		DeployGainCost,
		MultipleShot,
		StopAttack,
		ChangeAttackRange,
	}

	public enum E_SkillCostGainType
	{
		Auto,
		Attack,
		TakeAttack,

	}

	public enum E_SkillActivationType
	{
		AutoActive,
		MenualActive,
		PessiveActive,
	}
}