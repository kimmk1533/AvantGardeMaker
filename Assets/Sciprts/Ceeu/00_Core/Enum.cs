using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.Ceeu.Enum
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
		// 저지대 타일
		LowGroundTile,
		// 고지대 타일
		HighGroundTile,
		// 보호 목표
		Protection_Objective,
		// 침입 포인트
		Incursion_Point,

		Max
	}
	public enum E_CameraMode
	{
		GameMode,
		EditMode,
	}

	public class EnumUtil
	{

	}
}