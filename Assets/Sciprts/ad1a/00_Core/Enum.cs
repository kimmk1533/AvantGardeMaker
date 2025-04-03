using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.ad1a.Enum
{
	public enum E_EnemyType
	{
		E_Normal,
		E_Elite,
		E_Leader
	}

	public enum E_EnemyRaceType
	{
		E_InfectedCreature, //감염생물
		E_Drone,            //드론
		E_Sarkaz,           //살카즈
		E_Possessed,        //숙주
		E_SeaMonster,       //바다 괴물
		E_ArtsCreation,     //아츠 피조물
		E_Apparition,       //요괴
		E_Machina,          //기계
		E_WildBeast,        //야생동물
		E_Collapsal,        //붕괴체
		E_Ect               //기타
	}

	public enum E_EnemyFlyable
	{
		E_Walk,         //지상
		E_Flying,       //공중
	}

	public enum E_EnemyAtkPatternType
	{
		E_Disable,
		E_Melee,
		E_Range,
	}

	public enum E_EnemyDmgType
	{
		E_Physic,
		E_Magic,
		E_Heal,
		E_None,
	}

	public enum E_EnemyState
	{
		None,
		Idle,
		Move,
		Attack,
		Dead,
	}

	[System.Flags]
	public enum E_EnemyImmuneType
	{
		Stun = 1 << 0,
		Sleep = 1 << 1,
		Freeze = 1 << 2,
		Airborn = 1 << 3,
		Shiver = 1 << 4,
		Fear = 1 << 5,
	}

	public enum E_EnemyRankType
	{
		None,

		E,
		D,
		C,
		B,
		Bplus,
		A,
		Aplus,
		S,
		Splus,
		SS,

		Question,
	}

	public class EnumUtil
	{
		public static string EnumToKorString<TEnum>(TEnum enumValue) where TEnum : System.Enum
		{
			switch (typeof(TEnum).Name)
			{
				default:
					break;
				case "E_EnemyRaceType":
					if (System.Enum.TryParse<E_EnemyRaceType>(enumValue.ToString(), out E_EnemyRaceType raceType) == false)
						break;

					return EnumToKorString_RaceType(raceType);
			}

			return string.Empty;
		}
		private static string EnumToKorString_RaceType(E_EnemyRaceType raceType)
		{
			switch (raceType)
			{
				default:
					break;
				case E_EnemyRaceType.E_InfectedCreature:
					return "감염생물";
				case E_EnemyRaceType.E_Drone:
					return "드론";
				case E_EnemyRaceType.E_Sarkaz:
					return "살카즈";
				case E_EnemyRaceType.E_Possessed:
					return "숙주";
				case E_EnemyRaceType.E_SeaMonster:
					return "바다 괴물";
				case E_EnemyRaceType.E_ArtsCreation:
					return "아츠 피조물";
				case E_EnemyRaceType.E_Apparition:
					return "요괴";
				case E_EnemyRaceType.E_Machina:
					return "기계";
				case E_EnemyRaceType.E_WildBeast:
					return "야생동물";
				case E_EnemyRaceType.E_Collapsal:
					return "붕괴체";
				case E_EnemyRaceType.E_Ect:
					return "기타";
			}

			return string.Empty;
		}
	}
}