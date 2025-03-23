using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker
{
	public sealed class EditModeManager : SerializedSingleton<EditModeManager>
	{
		#region Enum
		public enum E_TileType
		{
			// 타일
			Tile,
			// 보호 목표
			Protection_Objective,
			// 침입 포인트
			Incursion_Point,

			Max
		}
		#endregion

		#region 변수
		private bool m_IsEditMode = false;

		[Min(1)]
		[SerializeField]
		private int m_MapWidth = 1;
		[Min(1)]
		[SerializeField]
		private int m_MapHeight = 1;

		#region 타일 관련
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
		#endregion

		#region 프로퍼티
		public bool isEditMode => m_IsEditMode;
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		private static TileManager M_Tile => TileManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			// 마우스 위치 가져오기
			Vector3Int mousePosition = GetMousePositionInt();

			ChangeTilePreview();

			m_TilePreview.transform.position = mousePosition;

			// 타일 배치
			if (Input.GetMouseButtonDown(0) == true)
			{
				if (m_TileMap.TryGetValue(mousePosition, out (E_TileType tileType, Tile tile) value) == false)
					AddTile(mousePosition);
				else if (value.tileType != m_TileType)
					ReplaceTile(mousePosition);
			}
			// 타일 제거
			if (Input.GetMouseButtonDown(1) == true &&
				m_TileMap.ContainsKey(mousePosition) == true)
			{
				RemoveTile(mousePosition);
			}
		}
		#endregion

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
			m_TilePreview.gameObject.SetActive(true);
		}
		/// <summary>
		/// 게임 마무리화 함수 (Game Scene 나갈 시 호출)
		/// </summary>
		public void FinallizeGame()
		{

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
		private void ChangeTilePreview()
		{
			bool isKeyDown = false;

			if (Input.GetKeyDown(KeyCode.Alpha1) == true)
			{
				m_TileType = E_TileType.Tile;
				isKeyDown = true;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2) == true)
			{
				m_TileType = E_TileType.Protection_Objective;
				isKeyDown = true;
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3) == true)
			{
				m_TileType = E_TileType.Incursion_Point;
				isKeyDown = true;
			}

			if (isKeyDown == true)
			{
				m_TilePreview.gameObject.SetActive(false);

				m_TilePreview = m_TilePreviewMap[m_TileType];

				Vector3Int mousePosition = GetMousePositionInt();
				m_TilePreview.transform.position = mousePosition;

				m_TilePreview.gameObject.SetActive(true);
			}
		}
	}
}