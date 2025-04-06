using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using AvantGardeMaker.ad1a;
using AvantGardeMaker.Ceeu.Enum;

namespace AvantGardeMaker.Ceeu
{
	[System.Serializable]
	public struct StageData
	{
		#region 변수
		#region 타일 관련 변수
		#region 저장&불러오기
		[SerializeField, ReadOnly]
		[FoldoutGroup("Tiles")]
		private List<Vector2Int> m_TilePointList;
		[SerializeField, ReadOnly]
		[FoldoutGroup("Tiles")]
		private List<E_TileType> m_TileTypeList;
		#endregion

		[SerializeField, ReadOnly]
		[FoldoutGroup("Infos")]
		private Vector2Int m_MinTile;
		[SerializeField, ReadOnly]
		[FoldoutGroup("Infos")]
		private Vector2Int m_MaxTile;
		#endregion

		#region 적 관련 변수
		#region 저장&불러오기
		[SerializeField, ReadOnly]
		private List<EnemyData> m_EnemyDataList;
		[SerializeField, ReadOnly]
		private List<EnemySpawnData> m_EnemySpawnDataList;
		#endregion

		private Dictionary<string, EnemyData> m_EnemyDataMap;
		#endregion
		#endregion

		#region 프로퍼티
		#region 타일 관련 프로퍼티
		public int mapWidth => m_MaxTile.x - m_MinTile.x + 1;
		public int mapHeight => m_MaxTile.y - m_MinTile.y + 1;
		#endregion

		#region 적 관련 프로퍼티

		#endregion
		#endregion

		#region 매니져
		private static MapEditorManager M_MapEditor => MapEditorManager.Instance;
		private static MapEditorUIManager M_MapEditorUI => MapEditorUIManager.Instance;
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 저장 이전의 초기화 (기존 데이터 삭제)
		/// </summary>
		public void InitializeBeforeSave()
		{
			#region 타일 관련 초기화
			if (m_TilePointList == null)
				m_TilePointList = new List<Vector2Int>();
			if (m_TileTypeList == null)
				m_TileTypeList = new List<E_TileType>();

			m_TilePointList.Clear();
			m_TileTypeList.Clear();

			m_MinTile = Vector2Int.one * int.MaxValue;
			m_MaxTile = Vector2Int.one * int.MinValue;
			#endregion

			#region 적 관련 초기화
			if (m_EnemyDataList == null)
				m_EnemyDataList = new List<EnemyData>();
			if (m_EnemySpawnDataList == null)
				m_EnemySpawnDataList = new List<EnemySpawnData>();
			if (m_EnemyDataMap == null)
				m_EnemyDataMap = new Dictionary<string, EnemyData>();

			m_EnemyDataList.Clear();
			m_EnemySpawnDataList.Clear();
			m_EnemyDataMap.Clear();
			#endregion
		}
		/// <summary>
		/// 불러오기 이후의 초기화 (새로운 데이터 적용)
		/// </summary>
		public void InitializeAfterLoad()
		{
			#region 타일 배치
			int count = m_TilePointList.Count;

			if (count != m_TileTypeList.Count)
				Debug.LogError("저장한 위치와 타일의 갯수가 다름");

			for (int i = 0; i < count; ++i)
			{
				Vector2Int tilePos = m_TilePointList[i];
				E_TileType tileType = m_TileTypeList[i];

				M_MapEditor.AddTile(tilePos, tileType);
			}
			#endregion

			#region 적 정보 불러오기
			foreach (var item in m_EnemyDataList)
			{
				m_EnemyDataMap.Add(item.EngName, item);
			}
			#endregion
		}
		#endregion

		public void AddTile(Vector2Int pos, E_TileType tileType)
		{
			m_TilePointList.Add(pos);
			m_TileTypeList.Add(tileType);

			m_MinTile.x = Mathf.Min(m_MinTile.x, pos.x);
			m_MinTile.y = Mathf.Min(m_MinTile.y, pos.y);

			m_MaxTile.x = Mathf.Max(m_MaxTile.x, pos.x);
			m_MaxTile.y = Mathf.Max(m_MaxTile.y, pos.y);
		}
		public void AddEnemyData(EnemyData enemyData)
		{

		}
	}
}