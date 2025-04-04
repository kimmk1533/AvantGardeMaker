using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.ad1a.Enum
{
	public enum E_EnemyType
	{
		//일반
		Normal,
		//정예
		Elite,
		//리더
		Leader,
	}

	public enum E_EnemyRaceType
	{
		//기타
		None,
		//감염생물
		InfectedCreature,
		//드론
		Drone,
		//살카즈
		Sarkaz,
		//숙주
		Possessed,
		//바다 괴물
		SeaMonster,
		//아츠 피조물
		ArtsCreation,
		//요괴
		Apparition,
		//기계
		Machina,
		//야생동물
		WildBeast,
		//붕괴체
		Collapsal,
	}

	public enum E_EnemyFlyable
	{
		//지상
		Walk,
		//공중
		Flying,       
	}

	public enum E_EnemyAtkPatternType
	{
		//비공격
		Disable,
		//근거리
		Melee,
		//원거리
		Range,
	}

	public enum E_EnemyDmgType
	{
		//물리
		Physic,
		//마법
		Magic,
		//치료
		Heal,
		//없음
		None,
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

	public enum E_EnemyState
	{
		None,
		Idle,
		Move,
		Attack,
		Dead,
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

				case "E_EnemyFlyable":
					if (System.Enum.TryParse<E_EnemyFlyable>(enumValue.ToString(), out E_EnemyFlyable flyable) == false)
						break;

					return EnumToKorString_Flyable(flyable);

				case "E_EnemyAtkPatternType":
					if (System.Enum.TryParse<E_EnemyAtkPatternType>(enumValue.ToString(), out E_EnemyAtkPatternType atkPatternType) == false)
						break;

					return EnumToKorString_AtkPatternType(atkPatternType);

				case "E_EnemyDmgType":
					if (System.Enum.TryParse<E_EnemyDmgType>(enumValue.ToString(), out E_EnemyDmgType dmgType) == false)
						break;

					return EnumToKorString_DmgType(dmgType);
			}

			return string.Empty;
		}
		private static string EnumToKorString_RaceType(E_EnemyRaceType raceType)
		{
			switch (raceType)
			{
				default:
					break;
				case E_EnemyRaceType.InfectedCreature:
					return "감염생물";
				case E_EnemyRaceType.Drone:
					return "드론";
				case E_EnemyRaceType.Sarkaz:
					return "살카즈";
				case E_EnemyRaceType.Possessed:
					return "숙주";
				case E_EnemyRaceType.SeaMonster:
					return "바다 괴물";
				case E_EnemyRaceType.ArtsCreation:
					return "아츠 피조물";
				case E_EnemyRaceType.Apparition:
					return "요괴";
				case E_EnemyRaceType.Machina:
					return "기계";
				case E_EnemyRaceType.WildBeast:
					return "야생동물";
				case E_EnemyRaceType.Collapsal:
					return "붕괴체";
				case E_EnemyRaceType.Ect:
					return "기타";
			}

			return string.Empty;
		}

		private static string EnumToKorString_Flyable(E_EnemyFlyable flyable)
		{
			switch (flyable)
			{
				default:
					break;
				case E_EnemyFlyable.Walk:
					return "지상";
				case E_EnemyFlyable.Flying:
					return "공중";
			}

			return string.Empty;
		}

		private static string EnumToKorString_AtkPatternType(E_EnemyAtkPatternType atkPatternType)
		{
			switch (atkPatternType)
			{
				default:
					break;
				case E_EnemyAtkPatternType.Disable:
					return "비공격";
				case E_EnemyAtkPatternType.Melee:
					return "근거리";
				case E_EnemyAtkPatternType.Range:
					return "원거리";
			}

			return string.Empty;
		}

		private static string EnumToKorString_DmgType(E_EnemyDmgType dmgType)
		{
			switch (dmgType)
			{
				default:
					break;
				case E_EnemyDmgType.Physic:
					return "물리";
				case E_EnemyDmgType.Magic:
					return "마법";
				case E_EnemyDmgType.Heal:
					return "치료";
			}

			return string.Empty;
		}
	}
}