using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.TileSpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.TileSpace
{
	[System.Serializable]
	public class TileSpawnData
	{
		#region 변수
		// 스폰시킬 타일의 키
		public string TileSpawnKey = string.Empty;

		// 타일 위치
		public Vector2Int TilePos = Vector2Int.zero;
		// 타일 위치 오프셋
		public Vector3 TileOffset = Vector3.zero;
		// 타일 종류
		public E_TileType TileType;
		// 타일 위치 종류
		public E_TilePositionType TilePositionType;
		// 타일 배치 가능 타입
		public E_TileDeployableTypeFlag TileDeployableTypeFlag = E_TileDeployableTypeFlag.None;
		#endregion

		#region 프로퍼티
		#endregion
	}
}