using System.Collections;
using System.Collections.Generic;
using System.IO;
using AvantGardeMaker.Ceeu.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using YamlDotNet.Serialization;

namespace AvantGardeMaker.Ceeu
{
	public sealed class EditModeManager : SerializedSingleton<EditModeManager>
	{
		#region 변수
		[SerializeField]
		private MapData m_EditingMapData = default;

		#region 편집 모드 관련 변수
		private bool m_IsEditMode = false;

		private E_EditModeType m_EditModeType = E_EditModeType.System;
		#endregion

		#region 타일 관련 변수
		// 타일 배치 가능 여부
		private bool m_TilePlacementFlag = true;

		// 생성한 타일 부모
		private GameObject m_TileParent = null;

		// 현재 타일 타입
		private E_TileType m_TileType = E_TileType.Tile;
		// 타일 프리뷰 오브젝트
		private Tile m_TilePreview = null;
		// 타일 프리뷰 오브젝트 맵
		private Dictionary<E_TileType, Tile> m_TilePreviewMap = null;

		// 머터리얼 정보 맵
		[SerializeField]
		private Dictionary<string, Material> m_MaterialMap = new Dictionary<string, Material>();

		// 생성한 타일 맵
		private Dictionary<Vector3Int, (E_TileType tileType, Tile tile)> m_TileMap = null;
		#endregion

		#region 저장 & 불러오기 관련 변수
		[SerializeField]
		private string m_MapDataSavingPath = string.Empty;
		#endregion
		#endregion

		#region 프로퍼티
		public MapData currentMapData => m_EditingMapData;

		public bool isEditMode => m_IsEditMode;

		public string mapDataSavingPath => Path.Combine(Application.persistentDataPath, m_MapDataSavingPath) + (m_MapDataSavingPath.EndsWith(".yaml") == false ? ".yaml" : "");
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		private static TileManager M_Tile => TileManager.Instance;
		private static YamlFileManager M_YamlFile => YamlFileManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			switch (m_EditModeType)
			{
				case E_EditModeType.System:
					CursorEditModeProcess();
					break;
				case E_EditModeType.Tile:
					TileEditModeProcess();
					break;
				case E_EditModeType.Enemy:
					break;
			}
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public void Initialize()
		{
			#region 프리뷰 타일 머터리얼 생성
			List<KeyValuePair<string, Material>> previewMaterialList = new List<KeyValuePair<string, Material>>();
			foreach (var item in m_MaterialMap)
			{
				string previewKey = item.Key + " Preview";
				Material previewMaterial = new Material(item.Value);

				previewMaterial.name = previewKey;

				Color previewColor = previewMaterial.color;
				previewColor.a = 0.3f;
				previewMaterial.color = previewColor;

				KeyValuePair<string, Material> keyValuePair = new KeyValuePair<string, Material>(previewKey, previewMaterial);
				previewMaterialList.Add(keyValuePair);
			}
			foreach (var item in previewMaterialList)
			{
				m_MaterialMap.Add(item.Key, item.Value);
			}
			#endregion

			m_TileMap = new Dictionary<Vector3Int, (E_TileType, Tile)>();
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public void Finallize()
		{

		}

		/// <summary>
		/// 게임 초기화 함수 (Game Scene 진입 시 호출)
		/// </summary>
		public void InitializeGame()
		{
			m_TilePlacementFlag = true;

			m_TileParent = new GameObject("Tile Parent");
			m_TileParent.transform.position = Vector3.zero;

			m_TilePreviewMap = new Dictionary<E_TileType, Tile>();

			for (E_TileType tileType = E_TileType.Tile; tileType < E_TileType.Max; ++tileType)
			{
				string key = tileType.ToString().Replace('_', ' ');
				string previewKey = key + " Preview";

				Tile previewTile = M_Tile.GetBuilder(key)
					.SetParent(transform)
					.SetName(previewKey)
					.SetAutoInit(true)
					.Spawn();

				previewTile.GetComponent<MeshRenderer>().material = m_MaterialMap[previewKey];

				m_TilePreviewMap.Add(tileType, previewTile);
			}

			m_TileType = E_TileType.Tile;

			m_TilePreview = m_TilePreviewMap[m_TileType];
		}
		/// <summary>
		/// 게임 마무리화 함수 (Game Scene 나갈 시 호출)
		/// </summary>
		public void FinallizeGame()
		{

		}
		#endregion

		public void SetEditModeType(E_EditModeType editModeType)
		{
			m_EditModeType = editModeType;

			m_TilePreview.gameObject.SetActive(editModeType == E_EditModeType.Tile);
		}

		#region 커서 편집 모드 관련 함수
		private void CursorEditModeProcess()
		{

		}
		#endregion

		#region 타일 편집 모드 관련 함수
		private void TileEditModeProcess()
		{
			// 마우스 위치 가져오기
			Vector3Int mousePosition = GetMousePositionInt();

			if (Input.GetMouseButtonUp(0) == true ||
				Input.GetMouseButtonUp(1) == true)
				m_TilePlacementFlag = true;

			// 마우스 포인터가 UI 위에 없는 지 확인
			if (UtilClass.IsPointerOnUI() == false)
			{
				if (m_TilePlacementFlag == false)
					return;

				m_TilePreview.transform.position = mousePosition;
				m_TilePreview.gameObject.SetActive(true);

				// 타일 배치
				if (Input.GetMouseButton(0) == true)
				{
					if (m_TileMap.TryGetValue(mousePosition, out (E_TileType tileType, Tile tile) value) == false)
						AddTile(mousePosition);
					else if (value.tileType != m_TileType)
						ReplaceTile(mousePosition);
				}
				// 타일 제거
				if (Input.GetMouseButton(1) == true &&
					m_TileMap.ContainsKey(mousePosition) == true)
				{
					RemoveTile(mousePosition);
				}
			}
			else
			{
				m_TilePreview.gameObject.SetActive(false);

				if (Input.GetMouseButtonDown(0) == true ||
					Input.GetMouseButtonDown(1) == true)
					m_TilePlacementFlag = false;
			}
		}

		private Vector3Int GetMousePositionInt()
		{
			Vector3 mousePosition = UtilClass.GetMouseWorldPosition3D();
			Vector3Int mousePositionInt = new Vector3Int(
				Mathf.RoundToInt(mousePosition.x),
				0,
				Mathf.RoundToInt(mousePosition.z)
				);

			return mousePositionInt;
		}
		private void AddTile(Vector3Int tilePos)
		{
			string tileKey = m_TileType.ToString().Replace('_', ' ');

			Tile newTile = M_Tile.GetBuilder(tileKey)
				.SetPosition(tilePos)
				.SetActive(true)
				.SetParent(m_TileParent.transform)
				.SetName(tilePos.ToString())
				.SetAutoInit(true)
				.Spawn();

			newTile.GetComponent<MeshRenderer>().material = m_MaterialMap[tileKey];

			m_TileMap.Add(tilePos, (m_TileType, newTile));
		}
		private void RemoveTile(Vector3Int tilePos)
		{
			Tile removeTile = m_TileMap[tilePos].tile;

			M_Tile.Despawn(removeTile);

			m_TileMap.Remove(tilePos);
		}
		private void ReplaceTile(Vector3Int tilePos)
		{
			RemoveTile(tilePos);

			AddTile(tilePos);
		}
		public void SetTileType(E_TileType tileType)
		{
			m_TileType = tileType;
			m_TilePreview = m_TilePreviewMap[m_TileType];
		}
		#endregion

		#region 저장 & 불러오기 관련 함수
		public void SaveData()
		{
			M_YamlFile.Serialize(mapDataSavingPath, m_EditingMapData);

			Debug.Log("YAML 저장 완료: " + mapDataSavingPath);
		}
		public void LoadData()
		{
			if (File.Exists(mapDataSavingPath) == false)
			{
				Debug.LogError("파일이 존재하지 않습니다. 경로: " + mapDataSavingPath);
				return;
			}

			m_EditingMapData = M_YamlFile.Deserialize<MapData>(mapDataSavingPath);
		}
		#endregion
	}
}