using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.TileSpace.Enum
{
	public enum E_Direction
	{
		Up,
		Down,
		Left,
		Right,

		Max
	}
	public enum E_EditModeType
	{
		System,
		Tile,
		Operator,
		Enemy
	}
	public enum E_TileType
	{
		None,

		// y offset 0
		// 저지대 타일
		LowGround,
		// 고지대 타일
		HighGround,
		//// 구덩이
		//Hole,

		// y offset 1
		// 보호 목표
		ProtectionObjective,
		// 침입 포인트
		IncursionPoint,
		//// 울타리
		//Fence,
		//// 수풀
		//Bush,
		//// 치료 룬
		//MedicalRune,
		//// 방어 룬
		//DefenseRune,
		//// 방공 룬
		//AntiAirRune,
		//// 스페셜리스트 작전 포인트
		//SpecialistTacticalPoint,
		//// 활성 오리지늄
		//ActiveOriginium,
		//// 열펌프 통로
		//HeatPumpPassage,
		//// 배치 불가 타일
		//
		//// 장식
		//Decoration,

		Max
	}

	public class EnumUtil
	{

	}
}