using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.MikangMark.Enum
{
	public enum E_JopType
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

	public enum E_RedeploySpeed
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
}