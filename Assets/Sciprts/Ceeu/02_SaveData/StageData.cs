using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using AvantGardeMaker.Ceeu.Enum;
using YamlDotNet.Serialization;
using Sirenix.Serialization;
using AvantGardeMaker.ad1a;

namespace AvantGardeMaker.Ceeu
{
	[System.Serializable]
	public struct StageData
	{
		#region 변수
		#region 타일 관련 변수
		[SerializeField, ReadOnly]
		private Vector3Int m_MinTile;
		[SerializeField, ReadOnly]
		private Vector3Int m_MaxTile;
		#endregion
		#endregion

		#region 프로퍼티
		#region 타일 관련 프로퍼티
		[YamlIgnore]
		public int mapWidth => m_MaxTile.x - m_MinTile.x + 1;
		[YamlIgnore]
		public int mapHeight => m_MaxTile.z - m_MinTile.z + 1;
		#endregion

		#region 타일 저장 관련 프로퍼티
		[field: SerializeField, ReadOnly]
		public List<Point> tilePointList { get; set; }
		[field: SerializeField, ReadOnly]
		public List<E_TileType> tileTypeList { get; set; }
		#endregion

		#region 적 저장 관련 프로퍼티
		[field: SerializeField, ReadOnly]
		public List<EnemyData> enemyDataList { get; set; }
		[field: SerializeField, ReadOnly]
		public List<EnemySpawnData> enemySpawnDataList { get; set; }
		#endregion
		#endregion

		#region 매니져
		private static MapEditorManager M_EditMode => MapEditorManager.Instance;
		#endregion

		#region 초기화 & 마무리화 함수
		public void InitializeBeforeSave()
		{
			tilePointList = new List<Point>();
			tileTypeList = new List<E_TileType>();

			m_MinTile = Vector3Int.one * int.MaxValue;
			m_MinTile.y = 0;

			m_MaxTile = Vector3Int.one * int.MinValue;
			m_MaxTile.y = 0;
		}
		public void InitializeAfterLoad()
		{
			int count = tilePointList.Count;

			if (count != tileTypeList.Count)
				Debug.LogError("저장한 위치와 타일의 갯수가 다름");

			for (int i = 0; i < count; ++i)
			{
				Vector3Int tilePos = tilePointList[i];
				E_TileType tileType = tileTypeList[i];

				M_EditMode.AddTile(tilePos, tileType);
			}
		}
		#endregion

		public void AddTile(Vector3Int pos, E_TileType tileType)
		{
			tilePointList.Add(pos);
			tileTypeList.Add(tileType);

			m_MinTile.x = Mathf.Min(m_MinTile.x, pos.x);
			m_MinTile.z = Mathf.Min(m_MinTile.z, pos.z);

			m_MaxTile.x = Mathf.Max(m_MaxTile.x, pos.x);
			m_MaxTile.z = Mathf.Max(m_MaxTile.z, pos.z);
		}
	}
}