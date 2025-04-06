using System.Collections;
using System.Collections.Generic;
using System.IO;
using AvantGardeMaker.ad1a;
using AvantGardeMaker.Ceeu.Enum;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace AvantGardeMaker.Ceeu
{
	public sealed class MapEditorManager : SerializedSingleton<MapEditorManager>
	{
		#region 변수
		#region 카메라 관련 변수
		[SerializeField, Min(0f)]
		private float m_CameraSwitchDuration = 1f;

		[SerializeField, ReadOnly]
		private E_CameraMode m_CameraMode = E_CameraMode.EditMode;

		[SerializeField, ReadOnly]
		private bool m_IsCameraSwitching = false;
		#endregion

		#region 편집 모드 관련 변수
		private bool m_IsEditMode = false;

		private E_EditModeType m_EditModeType = E_EditModeType.Tile;
		#endregion

		#region 타일 관련 변수
		// 타일 배치 가능 여부
		private bool m_TilePlacementFlag = true;

		private Vector3 m_HighGroundTileOffset = Vector3.back * 0.2f;

		// 현재 타일 타입
		private E_TileType m_CurrentTileType = E_TileType.LowGroundTile;
		// 타일 프리뷰 오브젝트
		private Tile m_TilePreview = null;
		// 타일 프리뷰 오브젝트 맵
		private Dictionary<E_TileType, Tile> m_TilePreviewMap = null;

		// 머터리얼 정보 맵
		[SerializeField]
		private Dictionary<string, Material> m_MaterialMap = new Dictionary<string, Material>();
		#endregion

		#region 적 관련 변수
		#endregion

		#region 저장 & 불러오기 관련 변수
		[SerializeField]
		private StageData m_EditingStageData = default;

		[SerializeField]
		private string m_StageName = string.Empty;

		[SerializeField]
		private TMP_FontAsset m_UIFont = null;
		#endregion
		#endregion

		#region 프로퍼티
		#region 카메라 관련 프로퍼티
		public Camera mapEditorCamera { get; set; }

		public Transform editModeCameraTransform { get; set; }
		public Transform gameModeCameraTransform { get; set; }
		#endregion

		#region 편집 모드 관련 프로퍼티
		public bool isEditMode => m_IsEditMode;
		#endregion

		#region 타일 관련 프로퍼티
		private bool tilePreviewActive =>
			m_CameraMode == E_CameraMode.EditMode &&
			m_IsCameraSwitching == false;
		#endregion

		#region 저장 & 불러오기 관련 프로퍼티
		public StageData currentStageData => m_EditingStageData;

		public string stageName { get => m_StageName; set => m_StageName = value; }
		private string mapDataSavingFilePath => Path.Combine(Application.dataPath, "..", "Data", m_StageName) + (m_StageName.EndsWith(".json") == false ? ".json" : "");
		#endregion
		#endregion

		#region 이벤트
		private event System.Action onCameraSwitcingFinished = null;

		#region 이벤트 함수
		private void OnCameraSwitchingFinished()
		{
			switch (m_CameraMode)
			{
				case E_CameraMode.GameMode:
					//m_EditModeCamera.orthographic = false;
					break;
				case E_CameraMode.EditMode:
					mapEditorCamera.orthographic = true;
					break;
				default:
					break;
			}
		}
		#endregion
		#endregion

		#region 매니저
		private static TileManager M_Tile => TileManager.Instance;
		private static EnemyManager M_Enemy => EnemyManager.Instance;
		private static MapEditorUIManager M_MapEditorUI => MapEditorUIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space) == true)
				SwitchCameraMode();

			switch (m_EditModeType)
			{
				case E_EditModeType.System:
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

			onCameraSwitcingFinished += OnCameraSwitchingFinished;

			gameObject.SetActive(false);
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public void Finallize()
		{
			onCameraSwitcingFinished = null;
		}

		/// <summary>
		/// 게임 초기화 함수 (본인 Main Scene 진입 시 호출)
		/// </summary>
		public void InitializeMain()
		{
			m_TilePlacementFlag = true;

			if (m_TilePreviewMap == null)
			{
				m_TilePreviewMap = new Dictionary<E_TileType, Tile>();

				for (E_TileType tileType = E_TileType.LowGroundTile; tileType < E_TileType.Max; ++tileType)
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
			}

			m_EditModeType = E_EditModeType.Tile;
			m_CurrentTileType = E_TileType.LowGroundTile;

			m_TilePreview = m_TilePreviewMap[m_CurrentTileType];

			LoadData();

			gameObject.SetActive(true);
		}
		/// <summary>
		/// 게임 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public void FinallizeMain()
		{

		}
		#endregion

		public void SetEditModeType(E_EditModeType editModeType)
		{
			m_EditModeType = editModeType;

			m_TilePreview.gameObject.SetActive(editModeType == E_EditModeType.Tile &&
				tilePreviewActive);
		}

		#region 카메라 관련 함수
		private void SwitchCameraMode()
		{
			if (m_IsCameraSwitching == true)
				return;

			m_IsCameraSwitching = true;

			switch (m_CameraMode)
			{
				case E_CameraMode.GameMode:
					m_CameraMode = E_CameraMode.EditMode;

					StartCoroutine(MoveCamera(editModeCameraTransform));
					//m_EditModeCamera.orthographic = true;
					break;
				case E_CameraMode.EditMode:
					m_CameraMode = E_CameraMode.GameMode;

					StartCoroutine(MoveCamera(gameModeCameraTransform));
					mapEditorCamera.orthographic = false;
					m_TilePreview.gameObject.SetActive(false);
					break;
				default:
					return;
			}

		}

		private IEnumerator MoveCamera(Transform targetTransform)
		{
			if (m_CameraSwitchDuration <= 0f)
			{
				mapEditorCamera.transform.position = targetTransform.position;
				mapEditorCamera.transform.rotation = targetTransform.rotation;
				m_IsCameraSwitching = false;
				onCameraSwitcingFinished?.Invoke();
				yield break;
			}

			Vector3 initPosition = mapEditorCamera.transform.position;
			Quaternion initRotation = mapEditorCamera.transform.rotation;
			float t = 0f;

			for (float time = 0f; time <= m_CameraSwitchDuration; time += Time.deltaTime)
			{
				yield return null;

				t = Mathf.Clamp01(time / m_CameraSwitchDuration);

				mapEditorCamera.transform.position = Vector3.Lerp(initPosition, targetTransform.position, t);
				mapEditorCamera.transform.rotation = Quaternion.Lerp(initRotation, targetTransform.rotation, t);
			}

			mapEditorCamera.transform.position = targetTransform.position;
			mapEditorCamera.transform.rotation = targetTransform.rotation;
			m_IsCameraSwitching = false;
			onCameraSwitcingFinished?.Invoke();
		}
		#endregion

		#region 타일 편집 모드 관련 함수
		private void TileEditModeProcess()
		{
			if (m_CameraMode == E_CameraMode.GameMode ||
				m_IsCameraSwitching == true)
				return;

			// 마우스 위치 가져오기
			Vector2Int mousePosition = GetMousePositionInt();

			if (Input.GetMouseButtonUp(0) == true ||
				Input.GetMouseButtonUp(1) == true)
				m_TilePlacementFlag = true;

			// 마우스 포인터가 UI 위에 없는 지 확인
			if (UtilClass.IsPointerOnUI() == false)
			{
				if (m_TilePlacementFlag == false)
					return;

				m_TilePreview.transform.position = (Vector3Int)mousePosition;
				m_TilePreview.gameObject.SetActive(true);

				(E_TileType tileType, Tile tile) tileValue = M_Tile.GetTileValue(mousePosition);

				// 타일 배치
				if (Input.GetMouseButton(0) == true)
				{
					if (tileValue.tile == null)
						M_Tile.AddTile(mousePosition, m_CurrentTileType);
					else if (tileValue.tileType != m_CurrentTileType)
						M_Tile.ReplaceTile(mousePosition, m_CurrentTileType);
				}
				// 타일 제거
				if (Input.GetMouseButton(1) == true &&
					tileValue.tile != null)
				{
					M_Tile.RemoveTile(mousePosition);
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

		private Vector2Int GetMousePositionInt()
		{
			Vector3 mousePosition = UtilClass.GetMouseWorldPosition3D();
			Vector2Int mousePositionInt = new Vector2Int(
				Mathf.RoundToInt(mousePosition.x),
				Mathf.RoundToInt(mousePosition.y)
				);

			return mousePositionInt;
		}
		public Vector3 GetTileOffset(E_TileType tileType)
		{
			if (tileType == E_TileType.HighGroundTile)
				return m_HighGroundTileOffset;

			return Vector3.zero;
		}

		public void SetTileType(E_TileType tileType)
		{
			m_CurrentTileType = tileType;
			m_TilePreview = m_TilePreviewMap[m_CurrentTileType];
		}
		#endregion

		#region 적 관련 함수
		#endregion

		#region 저장 & 불러오기 관련 함수
		[Button]
		public void SaveData()
		{
			m_EditingStageData.Initialize();

			#region 타일 저장
			M_Tile.SaveTileData(ref m_EditingStageData);
			#endregion

			#region 적 저장
			M_MapEditorUI.SaveEnemyDataUI(ref m_EditingStageData);
			M_MapEditorUI.SaveEnemySpawnDataUI(ref m_EditingStageData);
			#endregion

			JsonBuilder.Serialize<StageData>(mapDataSavingFilePath, m_EditingStageData);

			#region Debug
			TextMeshPro textMesh = UtilClass.CreateWorldText(null, m_StageName + " 저장 완료", new UtilClass.WorldTMP_TextOption()
			{
				tmpFont = m_UIFont,
				fontSize = 20,
				textAlignment = TextAlignmentOptions.Midline,
				duration = 1f,
			});
			textMesh.transform.rotation = mapEditorCamera.transform.rotation;

			Debug.Log("[Json 저장 완료]: " + mapDataSavingFilePath);
			#endregion
		}
		[Button]
		public void LoadData()
		{
			if (File.Exists(mapDataSavingFilePath) == false)
			{
				Debug.LogError("파일이 존재하지 않습니다. 경로: " + mapDataSavingFilePath);
				return;
			}

			m_EditingStageData = JsonBuilder.Deserialize<StageData>(mapDataSavingFilePath);

			M_Tile.LoadTileData(ref m_EditingStageData);
			M_Enemy.LoadEnemyData(ref m_EditingStageData);
			M_MapEditorUI.LoadEnemySpawnDataUI(ref m_EditingStageData);

			#region Debug
			TextMeshPro textMesh = UtilClass.CreateWorldText(null, m_StageName + " 로드 완료", new UtilClass.WorldTMP_TextOption()
			{
				tmpFont = m_UIFont,
				fontSize = 20,
				textAlignment = TextAlignmentOptions.Midline,
				duration = 1f,
			});
			textMesh.transform.rotation = mapEditorCamera.transform.rotation;

			Debug.Log("[Json 로드 완료]: " + mapDataSavingFilePath);
			#endregion
		}
		#endregion
	}
}