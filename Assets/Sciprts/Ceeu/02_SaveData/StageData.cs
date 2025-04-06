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
		private List<string> m_EnemyKeyList;
		[SerializeField, ReadOnly]
		private List<EnemyFixedData> m_EnemyFixedDataList;
		[SerializeField, ReadOnly]
		private List<EnemyVariableData> m_EnemyVariableDataList;
		[SerializeField, ReadOnly]
		private List<EnemySpawnData> m_EnemySpawnDataList;
		#endregion

		#endregion
		#endregion

		#region 프로퍼티
		#region 타일 관련 프로퍼티
		public List<Vector2Int> tilePointList => new List<Vector2Int>(m_TilePointList);
		public List<E_TileType> tileTypeList => new List<E_TileType>(m_TileTypeList);

		public int mapWidth => m_MaxTile.x - m_MinTile.x + 1;
		public int mapHeight => m_MaxTile.y - m_MinTile.y + 1;
		#endregion

		#region 적 관련 프로퍼티
		public List<EnemySpawnData> enemySpawnDataList => new List<EnemySpawnData>(m_EnemySpawnDataList);
		#endregion
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
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
			if (m_EnemyKeyList == null)
				m_EnemyKeyList = new List<string>();
			if (m_EnemyFixedDataList == null)
				m_EnemyFixedDataList = new List<EnemyFixedData>();
			if (m_EnemyVariableDataList == null)
				m_EnemyVariableDataList = new List<EnemyVariableData>();

			if (m_EnemySpawnDataList == null)
				m_EnemySpawnDataList = new List<EnemySpawnData>();

			m_EnemyKeyList.Clear();
			m_EnemyFixedDataList.Clear();
			m_EnemyVariableDataList.Clear();

			m_EnemySpawnDataList.Clear();
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
			m_EnemyKeyList.Add(enemyData.KrName);
			m_EnemyFixedDataList.Add(enemyData.FixedData);
			m_EnemyVariableDataList.Add(enemyData.VariableData);
		}
		public void AddEnemySpawnData(EnemySpawnData enemySpawnData)
		{
			m_EnemySpawnDataList.Add(enemySpawnData);
		}

		public List<EnemyData> GetEnemyDataList()
		{
			List<EnemyData> enemyDataList = new List<EnemyData>();

			int count = m_EnemyKeyList.Count;

			if (count != m_EnemyFixedDataList.Count ||
				count != m_EnemyVariableDataList.Count)
				throw new System.Exception("EnemyData 갯수 다름");

			for (int i = 0; i < count; ++i)
			{
				EnemyData enemyData = ScriptableObject.CreateInstance<EnemyData>();

				//enemyData.EngName = m_EnemyKeyList[i];
				enemyData.KrName = m_EnemyKeyList[i];
				enemyData.FixedData = m_EnemyFixedDataList[i];
				enemyData.VariableData = m_EnemyVariableDataList[i];

				enemyDataList.Add(enemyData);
			}

			return enemyDataList;
		}
	}
}