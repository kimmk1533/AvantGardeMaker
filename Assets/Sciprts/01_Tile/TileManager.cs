using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using AvantGardeMaker.CoreSpace;
using AvantGardeMaker.CoreSpace.SaveLoad;
using AvantGardeMaker.TileSpace.Enum;
using AvantGardeMaker.UI;
using TMPro;
using UnityEngine;

namespace AvantGardeMaker.TileSpace
{
	public class TileManager : ObjectManager<TileManager, Tile>
	{
		#region 변수
		private const string c_TileDataPath = "Datas\\01_Tile Datas";

		// 생성한 타일 부모
		private GameObject m_TileParent = null;

		// 생성한 타일 맵
		private Dictionary<Vector2Int, Tile> m_TileMap = null;
		// 타일 데이터 저장용 딕셔너리
		private Dictionary<string, TileData> m_TileDataMap = null;
		#endregion

		#region 프로퍼티
		public Transform tileParent => m_TileParent.transform;
		#endregion

		#region 이벤트

		#region 이벤트 함수
		private void OnTileDespawned(Tile tile)
		{
			// 기존 텍스트 제거
			TextMeshPro[] textMeshs = tile.transform.GetComponentsInChildren<TextMeshPro>();
			foreach (var item in textMeshs)
			{
				Destroy(item.gameObject);
			}
		}
		#endregion
		#endregion

		#region 매니저
		private static MapEditingManager M_MapEditing => MapEditingManager.Instance;
		private static MapEditingSceneUIManager M_MapEditingUI => MapEditingSceneUIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			m_TileMap = new Dictionary<Vector2Int, Tile>();
			m_TileDataMap = new Dictionary<string, TileData>();

			LoadTileData();
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();
		}

		/// <summary>
		/// 메인 초기화 함수 (본인 Main Scene 진입 시 호출)
		/// </summary>
		public override void InitializeMain()
		{
			base.InitializeMain();

			m_TileParent = new GameObject("Tile Parent");
			m_TileParent.transform.position = Vector3.zero;

			foreach (var item in m_ObjectPoolMap)
			{
				item.Value.onItemDespawned += OnTileDespawned;
			}
		}
		/// <summary>
		/// 메인 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();

			ClearTile();

			foreach (var item in m_ObjectPoolMap)
			{
				item.Value.onItemDespawned -= OnTileDespawned;
			}

			m_TileParent = null;
		}
		#endregion

		public void ClearTile()
		{
			foreach (var item in m_TileMap)
			{
				Despawn(item.Value);
			}
			m_TileMap.Clear();
		}

		///<summary>
		/// Resources 폴더에 있는 TileData 스크립터블 오브젝트를 딕셔너리에 저장
		/// </summary>
		public void LoadTileData()
		{
			m_TileDataMap.Clear();

			TileData[] tileDatas = Resources.LoadAll<TileData>(c_TileDataPath);

			for (int i = 0; i < tileDatas.Length; ++i)
			{
				string key = tileDatas[i].key;

				m_TileDataMap.Add(key, tileDatas[i]);
			}
		}

		public void AddTile(TileSpawnData tileSpawnData)
		{
			string tileKey = tileSpawnData.TileSpawnKey;
			Vector2Int tilePos = tileSpawnData.TilePos;
			E_TileType tileType = tileSpawnData.TileType;
			E_TilePositionType tilePositionType = tileSpawnData.TilePositionType;
			E_TileDeployableTypeFlag tileDeployableTypeFlag = tileSpawnData.TileDeployableTypeFlag;

			if (m_TileMap.TryGetValue(tilePos, out Tile tile) == false)
			{
				AddTile_Internal(tileSpawnData);

				return;
			}

			if (tile.poolKey == tileKey &&
				tile.tileType == tileType &&
				tile.tilePositionType == tilePositionType &&
				tile.tileDeployableTypeFlag == tileDeployableTypeFlag)
				return;

			RemoveTile(tilePos);

			AddTile_Internal(tileSpawnData);
		}
		private void AddTile_Internal(TileSpawnData tileSpawnData)
		{
			string tileKey = tileSpawnData.TileSpawnKey;
			Vector2Int tilePos = tileSpawnData.TilePos;
			Vector3 tileOffset = tileSpawnData.TileOffset;
			E_TileType tileType = tileSpawnData.TileType;
			E_TilePositionType tilePositionType = tileSpawnData.TilePositionType;
			E_TileDeployableTypeFlag tileDeployableTypeFlag = tileSpawnData.TileDeployableTypeFlag;

			Vector3 tilePosition = (Vector3)(Vector2)tileSpawnData.TilePos + tileOffset;

			Tile newTile = GetBuilder(tileKey)
				.SetPosition(tilePosition)
				.SetActive(true)
				.SetParent(m_TileParent.transform)
				.SetName(tilePos.ToString())
				.SetAutoInit(true)
				.Spawn();

			newTile.tileType = tileType;
			newTile.tilePositionType = tilePositionType;
			newTile.tileDeployableTypeFlag = tileDeployableTypeFlag;

			m_TileMap.Add(tilePos, newTile);

			#region 디버깅
			if (M_MapEditing.isEditMode == false)
				return;

			// 현재 텍스트 생성
			TextMeshPro textMesh = UtilClass.CreateWorldText(newTile.transform, tilePos.ToString(), new UtilClass.WorldTMP_TextOption()
			{
				tmpFont = M_MapEditingUI.uiFont,
				fontSize = 1.5f,
				textAlignment = TextAlignmentOptions.Midline,
				color = Color.black,
			});
			textMesh.transform.position = tilePosition + (Vector3.back * 0.49f);
			textMesh.transform.rotation = M_MapEditing.mapEditorCamera.transform.rotation;
			#endregion
		}
		public void RemoveTile(Vector2Int tilePos)
		{
			if (m_TileMap.TryGetValue(tilePos, out Tile tile) == false)
				return;

			Despawn(tile);

			m_TileMap.Remove(tilePos);
		}

		public void SaveTileData(ref StageData stageData)
		{
			foreach (var item in m_TileMap)
			{
				string tileKey = item.Value.poolKey;

				if (m_TileDataMap.TryGetValue(tileKey, out TileData tileData) == false)
					continue;

				stageData.SaveTileData(tileData);
			}
		}
		public void SaveSpawnTileData(ref StageData stageData)
		{
			foreach (var item in m_TileMap)
			{
				Tile tile = item.Value;
				TileSpawnData tileSpawnData = new TileSpawnData();

				tileSpawnData.TileSpawnKey = tile.poolKey;

				tileSpawnData.TilePos = item.Key;
				tileSpawnData.TileDeployableTypeFlag = tile.tileDeployableTypeFlag;

				stageData.SaveTileSpawnData(tileSpawnData);
			}
		}
		public void LoadTileData(in StageData stageData)
		{
			List<TileSpawnData> spawnDataList = stageData.tileSpawnDataList;
			List<TileFixedData> fixedDataList = stageData.tileFixedDataList;
			List<TileVariableData> variableDataList = stageData.tileVariableDataList;

			int count = spawnDataList.Count;

			for (int i = 0; i < count; ++i)
			{
				TileSpawnData tileSpawnData = spawnDataList[i];

				AddTile_Internal(tileSpawnData);
			}
		}

		public Tile GetTile(Vector2Int tilePos)
		{
			if (m_TileMap.TryGetValue(tilePos, out Tile tile) == false)
				return null;

			return tile;
		}
		public List<TileData> GetAllTileDatas()
		{
			return new List<TileData>(m_TileDataMap.Values);
		}
	}
}