using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.EnemySpace;
using AvantGardeMaker.OperatorSpace;
using AvantGardeMaker.TileSpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.CoreSpace.SaveLoad
{
	[System.Serializable]
	public struct StageData
	{
		#region 변수
		#region 1. 시스템 관련 변수
		[SerializeField]
		[FoldoutGroup("시스템")]
		private string m_Title;
		[SerializeField]
		[FoldoutGroup("시스템")]
		private string m_CreatorNickName;
		[SerializeField]
		[FoldoutGroup("시스템")]
		private string m_CreatedPlayerId;
		[SerializeField]
		[FoldoutGroup("시스템")]
		private byte[] m_ThumnailTexture;
		[SerializeField]
		[FoldoutGroup("시스템")]
		private string m_Description;

		[PropertySpace(10)]
		[SerializeField]
		[FoldoutGroup("시스템")]
		private int m_InitCost;
		[SerializeField]
		[FoldoutGroup("시스템")]
		private int m_MaxCost;
		[SerializeField]
		[FoldoutGroup("시스템")]
		private float m_CostIncreaseTime;
		#endregion

		#region 2. 타일 관련 변수
		#region 저장&불러오기
		[SerializeField]
		[FoldoutGroup("타일")]
		private List<Vector2Int> m_TilePointList;
		[SerializeField]
		[FoldoutGroup("타일")]
		private List<E_TileType> m_TileTypeList;
		#endregion

		[SerializeField, RuntimeReadOnly]
		[FoldoutGroup("타일")]
		private Vector2Int m_MinTile;
		[SerializeField, RuntimeReadOnly]
		[FoldoutGroup("타일")]
		private Vector2Int m_MaxTile;
		#endregion

		#region 3. 오퍼레이터 관련 변수
		#region 저장&불러오기
		[SerializeField]
		[FoldoutGroup("오퍼레이터")]
		private List<string> m_OperatorKeyList;
		[SerializeField]
		[FoldoutGroup("오퍼레이터")]
		private List<OperatorFixedData> m_OperatorFixedDataList;
		[SerializeField]
		[FoldoutGroup("오퍼레이터")]
		private List<OperatorVariableData> m_OperatorVariableDataList;
		#endregion

		#endregion

		#region 4. 적 관련 변수
		#region 저장&불러오기
		[SerializeField]
		[FoldoutGroup("적")]
		private List<string> m_EnemyKeyList;
		[SerializeField]
		[FoldoutGroup("적")]
		private List<EnemyFixedData> m_EnemyFixedDataList;
		[SerializeField]
		[FoldoutGroup("적")]
		private List<EnemyVariableData> m_EnemyVariableDataList;
		[SerializeField]
		[FoldoutGroup("적")]
		private List<EnemySpawnData> m_EnemySpawnDataList;
		#endregion

		#endregion
		#endregion

		#region 프로퍼티
		#region 1. 시스템 관련 프로퍼티
		public string title
		{
			get => m_Title;
			set => m_Title = value;
		}
		public string creatorNickName
		{
			get => m_CreatorNickName;
			set => m_CreatorNickName = value;
		}
		public string createdPlayerId
		{
			get => m_CreatedPlayerId;
			set => m_CreatedPlayerId = value;
		}
		public byte[] thumnail
		{
			get => m_ThumnailTexture;
			set => m_ThumnailTexture = value;
		}
		public string description
		{
			get => m_Description;
			set => m_Description = value;
		}

		public int initCost => m_InitCost;
		public int maxCost => m_MaxCost;
		public float costIncreaseTime => m_CostIncreaseTime;
		#endregion

		#region 2. 타일 관련 프로퍼티
		public List<Vector2Int> tilePointList => new List<Vector2Int>(m_TilePointList);
		public List<E_TileType> tileTypeList => new List<E_TileType>(m_TileTypeList);

		public int mapWidth => m_MaxTile.x - m_MinTile.x + 1;
		public int mapHeight => m_MaxTile.y - m_MinTile.y + 1;

		public Vector2Int minTile => m_MinTile;

		public E_TileType[,] map
		{
			get
			{
				int mapWidth = this.mapWidth;
				int mapHeight = this.mapHeight;

				E_TileType[,] mapArray = new E_TileType[mapHeight, mapWidth];
				for (int y = 0; y < mapHeight; ++y)
				{
					for (int x = 0; x < mapWidth; ++x)
					{
						mapArray[y, x] = E_TileType.None;
					}
				}

				Vector2Int offset = -m_MinTile;

				for (int i = 0; i < m_TilePointList.Count; ++i)
				{
					Vector2Int tilePoint = m_TilePointList[i] + offset;

					mapArray[tilePoint.y, tilePoint.x] = m_TileTypeList[i];
				}

				return mapArray;
			}
		}
		#endregion

		#region 3. 오퍼레이터 관련 프로퍼티
		public List<string> operatorKeyList => new List<string>(m_OperatorKeyList);
		public List<OperatorFixedData> operatorFixedDataList => new List<OperatorFixedData>(m_OperatorFixedDataList);
		public List<OperatorVariableData> operatorVariableDataList => new List<OperatorVariableData>(m_OperatorVariableDataList);
		#endregion

		#region 4. 적 관련 프로퍼티
		public List<string> enemyKeyList => new List<string>(m_EnemyKeyList);
		public List<EnemyFixedData> enemyFixedDataList => new List<EnemyFixedData>(m_EnemyFixedDataList);
		public List<EnemyVariableData> enemyVariableDataList => new List<EnemyVariableData>(m_EnemyVariableDataList);
		public List<EnemySpawnData> enemySpawnDataList => new List<EnemySpawnData>(m_EnemySpawnDataList);
		#endregion
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public static void Initialize(ref StageData stageData)
		{
			#region 타일 관련 초기화
			if (stageData.m_TilePointList == null)
				stageData.m_TilePointList = new List<Vector2Int>();
			if (stageData.m_TileTypeList == null)
				stageData.m_TileTypeList = new List<E_TileType>();

			stageData.m_TilePointList.Clear();
			stageData.m_TileTypeList.Clear();

			stageData.m_MinTile = Vector2Int.one * int.MaxValue;
			stageData.m_MaxTile = Vector2Int.one * int.MinValue;
			#endregion

			#region 오퍼레이터 관련 초기화
			if (stageData.m_OperatorKeyList == null)
				stageData.m_OperatorKeyList = new List<string>();
			if (stageData.m_OperatorFixedDataList == null)
				stageData.m_OperatorFixedDataList = new List<OperatorFixedData>();
			if (stageData.m_OperatorVariableDataList == null)
				stageData.m_OperatorVariableDataList = new List<OperatorVariableData>();

			stageData.m_OperatorKeyList.Clear();
			stageData.m_OperatorFixedDataList.Clear();
			stageData.m_OperatorVariableDataList.Clear();
			#endregion

			#region 적 관련 초기화
			if (stageData.m_EnemyKeyList == null)
				stageData.m_EnemyKeyList = new List<string>();
			if (stageData.m_EnemyFixedDataList == null)
				stageData.m_EnemyFixedDataList = new List<EnemyFixedData>();
			if (stageData.m_EnemyVariableDataList == null)
				stageData.m_EnemyVariableDataList = new List<EnemyVariableData>();

			if (stageData.m_EnemySpawnDataList == null)
				stageData.m_EnemySpawnDataList = new List<EnemySpawnData>();

			stageData.m_EnemyKeyList.Clear();
			stageData.m_EnemyFixedDataList.Clear();
			stageData.m_EnemyVariableDataList.Clear();

			stageData.m_EnemySpawnDataList.Clear();
			#endregion
		}
		#endregion

		public void SaveTileData(Vector2Int pos, E_TileType tileType)
		{
			m_TilePointList.Add(pos);
			m_TileTypeList.Add(tileType);

			m_MinTile.x = Mathf.Min(m_MinTile.x, pos.x);
			m_MinTile.y = Mathf.Min(m_MinTile.y, pos.y);

			m_MaxTile.x = Mathf.Max(m_MaxTile.x, pos.x);
			m_MaxTile.y = Mathf.Max(m_MaxTile.y, pos.y);
		}
		public void SaveOperatorData(OperatorData operatorData)
		{
			m_OperatorKeyList.Add(operatorData.key);
			m_OperatorFixedDataList.Add(operatorData.FixedData);
			m_OperatorVariableDataList.Add(operatorData.VariableData);
		}
		public void SaveEnemyData(EnemyData enemyData)
		{
			m_EnemyKeyList.Add(enemyData.EngName);
			m_EnemyFixedDataList.Add(enemyData.FixedData);
			m_EnemyVariableDataList.Add(enemyData.VariableData);
		}
		public void SaveEnemySpawnData(EnemySpawnData enemySpawnData)
		{
			m_EnemySpawnDataList.Add(enemySpawnData);
		}

		public Texture2D GetThumnailTexture()
		{
			Texture2D thumnail = new Texture2D(256, 256);
			thumnail.LoadRawTextureData(m_ThumnailTexture);
			thumnail.Apply();

			return thumnail;
		}
	}
}