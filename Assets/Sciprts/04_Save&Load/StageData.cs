using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.EnemySpace;
using AvantGardeMaker.OperatorSpace;
using AvantGardeMaker.TileSpace;
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
		private int m_LifePoint;

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
		public List<TileSpawnData> m_TileSpawnDataList;
		[SerializeField]
		[FoldoutGroup("타일")]
		private List<TileFixedData> m_TileFixedDataList;
		[SerializeField]
		[FoldoutGroup("타일")]
		public List<TileVariableData> m_TileVariableDataList;
		#endregion

		[SerializeField, ReadOnly]
		[FoldoutGroup("타일")]
		private Vector2Int m_MinTile;
		[SerializeField, ReadOnly]
		[FoldoutGroup("타일")]
		private Vector2Int m_MaxTile;
		#endregion

		#region 3. 오퍼레이터 관련 변수
		#region 저장&불러오기
		[SerializeField]
		[FoldoutGroup("오퍼레이터")]
		private List<OperatorSpawnData> m_OperatorSpawnDataList;
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
		private List<EnemyFixedData> m_EnemyFixedDataList;
		[SerializeField]
		[FoldoutGroup("적")]
		private List<EnemyVariableData> m_EnemyVariableDataList;

		[SerializeField]
		[FoldoutGroup("적")]
		private List<EnemySpawnData> m_EnemySpawnDataList;
		#endregion

		[SerializeField]
		[FoldoutGroup("적")]
		private int m_EnemyMaxCount;
		#endregion
		#endregion

		#region 프로퍼티
		#region 1. 시스템 관련 프로퍼티
		public string title
		{
			readonly get => m_Title;
			set => m_Title = value;
		}
		public string creatorNickName
		{
			readonly get => m_CreatorNickName;
			set => m_CreatorNickName = value;
		}
		public string createdPlayerId
		{
			readonly get => m_CreatedPlayerId;
			set => m_CreatedPlayerId = value;
		}
		public byte[] thumnail
		{
			readonly get => m_ThumnailTexture;
			set => m_ThumnailTexture = value;
		}
		public string description
		{
			readonly get => m_Description;
			set => m_Description = value;
		}

		public int lifePoint
		{
			readonly get => m_LifePoint;
			set => m_LifePoint = value;
		}

		public int initCost
		{
			readonly get => m_InitCost;
			set => m_InitCost = value;
		}
		public int maxCost
		{
			readonly get => m_MaxCost;
			set => m_MaxCost = value;
		}
		public float costIncreaseTime
		{
			readonly get => m_CostIncreaseTime;
			set => m_CostIncreaseTime = value;
		}
		#endregion

		#region 2. 타일 관련 프로퍼티
		public readonly List<TileSpawnData> tileSpawnDataList => new List<TileSpawnData>(m_TileSpawnDataList);
		public readonly List<TileFixedData> tileFixedDataList => new List<TileFixedData>(m_TileFixedDataList);
		public readonly List<TileVariableData> tileVariableDataList => new List<TileVariableData>(m_TileVariableDataList);

		public readonly int mapWidth => m_MaxTile.x - m_MinTile.x + 1;
		public readonly int mapHeight => m_MaxTile.y - m_MinTile.y + 1;

		public readonly Vector2Int minTile => m_MinTile;

		public readonly (E_TileType tileType, E_TilePositionType tilePositionType)[,] map
		{
			get
			{
				int mapWidth = this.mapWidth;
				int mapHeight = this.mapHeight;

				(E_TileType tileType, E_TilePositionType tilePositionType)[,] mapArray = new (E_TileType, E_TilePositionType)[mapHeight, mapWidth];
				for (int y = 0; y < mapHeight; ++y)
				{
					for (int x = 0; x < mapWidth; ++x)
					{
						mapArray[y, x] = (E_TileType.Hole, E_TilePositionType.LowGround);
					}
				}

				Vector2Int offset = -m_MinTile;

				for (int i = 0; i < m_TileSpawnDataList.Count; ++i)
				{
					Vector2Int tilePoint = m_TileSpawnDataList[i].TilePos + offset;

					E_TileType tileType = m_TileSpawnDataList[i].TileType;
					E_TilePositionType tilePositionType = m_TileSpawnDataList[i].TilePositionType;

					mapArray[tilePoint.y, tilePoint.x] = (tileType, tilePositionType);
				}

				return mapArray;
			}
		}
		#endregion

		#region 3. 오퍼레이터 관련 프로퍼티
		public readonly List<OperatorSpawnData> operatorSpawnDataList => new List<OperatorSpawnData>(m_OperatorSpawnDataList);
		public readonly List<OperatorFixedData> operatorFixedDataList => new List<OperatorFixedData>(m_OperatorFixedDataList);
		public readonly List<OperatorVariableData> operatorVariableDataList => new List<OperatorVariableData>(m_OperatorVariableDataList);
		#endregion

		#region 4. 적 관련 프로퍼티
		public readonly List<EnemySpawnData> enemySpawnDataList => new List<EnemySpawnData>(m_EnemySpawnDataList);
		public readonly List<EnemyFixedData> enemyFixedDataList => new List<EnemyFixedData>(m_EnemyFixedDataList);
		public readonly List<EnemyVariableData> enemyVariableDataList => new List<EnemyVariableData>(m_EnemyVariableDataList);
		#endregion
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public static void Initialize(ref StageData stageData)
		{
			#region 타일 관련 초기화
			if (stageData.m_TileSpawnDataList == null)
				stageData.m_TileSpawnDataList = new List<TileSpawnData>();
			if (stageData.m_TileFixedDataList == null)
				stageData.m_TileFixedDataList = new List<TileFixedData>();
			if (stageData.m_TileVariableDataList == null)
				stageData.m_TileVariableDataList = new List<TileVariableData>();

			stageData.m_TileSpawnDataList.Clear();
			stageData.m_TileFixedDataList.Clear();
			stageData.m_TileVariableDataList.Clear();

			stageData.m_MinTile = Vector2Int.one * int.MaxValue;
			stageData.m_MaxTile = Vector2Int.one * int.MinValue;
			#endregion

			#region 오퍼레이터 관련 초기화
			if (stageData.m_OperatorSpawnDataList == null)
				stageData.m_OperatorSpawnDataList = new List<OperatorSpawnData>();
			if (stageData.m_OperatorFixedDataList == null)
				stageData.m_OperatorFixedDataList = new List<OperatorFixedData>();
			if (stageData.m_OperatorVariableDataList == null)
				stageData.m_OperatorVariableDataList = new List<OperatorVariableData>();

			stageData.m_OperatorSpawnDataList.Clear();
			stageData.m_OperatorFixedDataList.Clear();
			stageData.m_OperatorVariableDataList.Clear();
			#endregion

			#region 적 관련 초기화
			if (stageData.m_EnemyFixedDataList == null)
				stageData.m_EnemyFixedDataList = new List<EnemyFixedData>();
			if (stageData.m_EnemyVariableDataList == null)
				stageData.m_EnemyVariableDataList = new List<EnemyVariableData>();

			if (stageData.m_EnemySpawnDataList == null)
				stageData.m_EnemySpawnDataList = new List<EnemySpawnData>();

			stageData.m_EnemySpawnDataList.Clear();
			stageData.m_EnemyFixedDataList.Clear();

			stageData.m_EnemyVariableDataList.Clear();
			#endregion
		}
		#endregion

		public void SaveTileData(TileData tileData)
		{
			m_TileFixedDataList.Add(tileData.FixedData);
			m_TileVariableDataList.Add(tileData.VariableData);
		}
		public void SaveTileSpawnData(TileSpawnData tileSpawnData)
		{
			m_TileSpawnDataList.Add(tileSpawnData);

			Vector2Int tilePos = tileSpawnData.TilePos;

			m_MinTile.x = Mathf.Min(m_MinTile.x, tilePos.x);
			m_MinTile.y = Mathf.Min(m_MinTile.y, tilePos.y);

			m_MaxTile.x = Mathf.Max(m_MaxTile.x, tilePos.x);
			m_MaxTile.y = Mathf.Max(m_MaxTile.y, tilePos.y);
		}
		public void SaveOperatorData(OperatorData operatorData)
		{
			m_OperatorFixedDataList.Add(operatorData.FixedData);
			m_OperatorVariableDataList.Add(operatorData.VariableData);
		}
		public void SaveOperatorSpawnData(OperatorSpawnData operatorSpawnData)
		{
			m_OperatorSpawnDataList.Add(operatorSpawnData);
		}
		public void SaveEnemyData(EnemyData enemyData)
		{
			m_EnemyFixedDataList.Add(enemyData.FixedData);
			m_EnemyVariableDataList.Add(enemyData.VariableData);
		}
		public void SaveEnemySpawnData(EnemySpawnData enemySpawnData)
		{
			m_EnemySpawnDataList.Add(enemySpawnData);
		}

		public Texture2D GetThumnailTexture()
		{
			Texture2D thumnail = new Texture2D(580, 326, TextureFormat.RGBA32, false);
			thumnail.LoadRawTextureData(m_ThumnailTexture);
			thumnail.Apply();

			return thumnail;
		}
	}
}