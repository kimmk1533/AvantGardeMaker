using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.TileSpace.Enum
{
	public enum E_TileType : byte
	{
		// 일반 타일
		Default,

		// 장식
		Decoration,
		// 구덩이
		Hole,
		// 울타리
		Fence,
		// 수풀
		Bush,
		// 치료 룬
		MedicalRune,
		// 방어 룬
		DefenseRune,
		// 방공 룬
		AntiAirRune,
		// 스페셜리스트 작전 포인트
		SpecialistTacticalPoint,
		// 활성 오리지늄
		ActiveOriginium,
		// 열펌프 통로
		HeatPumpPassage,
		// 통로 입구
		EnterPassage,
		// 통로 출구
		ExitPassage,

		Max
	}
	public enum E_TilePositionType : byte
	{
		// 지상 타일
		LowGround,
		// 언덕 타일
		HighGround,
	}
	[System.Flags]
	public enum E_TileDeployableTypeFlag : byte
	{
		// 배치 불가
		None = 0,

		// 지상 오퍼 배치 가능
		LowDeployable,
		// 언덕 오퍼 배치 가능
		HighDeployable,
	}

	public enum E_TileThemaType
	{
		Episode0,
		Episode1,
		//Episode2,
		//Episode3,
		//Episode4,
		//Episode5,
		//Episode6,
		//Episode7,
		//Episode8,
		//Episode9,
		//Episode10,

		//WalkInTheDust,
	}

	public static class TileEnumUtil
	{
		public static string GetTileThemaKorString(E_TileThemaType themaType)
		{
			switch (themaType)
			{
				case E_TileThemaType.Episode0:
					return "EPISODE 00 암흑시대 • 상";
				case E_TileThemaType.Episode1:
					return "EPISODE 01 암흑시대 • 하";
				//case E_TileThemaType.Episode2:
				//	break;
				//case E_TileThemaType.Episode3:
				//	break;
				//case E_TileThemaType.Episode4:
				//	break;
				//case E_TileThemaType.Episode5:
				//	break;
				//case E_TileThemaType.Episode6:
				//	break;
				//case E_TileThemaType.Episode7:
				//	break;
				//case E_TileThemaType.Episode8:
				//	break;
				//case E_TileThemaType.Episode9:
				//	break;
				//case E_TileThemaType.Episode10:
				//	break;
				//case E_TileThemaType.WalkInTheDust:
				//	break;
			}

			return string.Empty;
		}
	}
}