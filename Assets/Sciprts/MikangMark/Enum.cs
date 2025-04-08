using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.MikangMark.Enum
{
	public enum E_Jop
	{
		VanGuard,
		Guard,
		Defender,
		Sniper,
		Caster,
		Medic,
		Supporter,
		Specialist
	}

	public enum E_AttackRange { Close, Far }
	public enum E_AttackSpeed { Very_Slow, Slow, Nomal, Fast, VeryFast }

	public enum E_ResetSpeed { Slow, Nomal, Fast }

	public enum E_TileAttackRange { OutRange=1, OperPos, InRange }

	public enum E_OperatorDirection { Right, Left, Down, Up }
}