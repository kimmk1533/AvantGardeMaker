using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace.Enum;
using AvantGardeMaker.CoreSpace.SaveLoad;
using AvantGardeMaker.OperatorSpace;
using AvantGardeMaker.TileSpace;
using AvantGardeMaker.TileSpace.Enum;
using AvantGardeMaker.EnemySpace;
using AvantGardeMaker.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace AvantGardeMaker.CoreSpace
{
	public sealed class MapEditingManager : SerializedSingleton<MapEditingManager>
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
		private E_TileType m_CurrentTileType = E_TileType.LowGround;
		// 타일 프리뷰 오브젝트
		private Tile m_TilePreview = null;
		// 타일 프리뷰 오브젝트 맵
		private Dictionary<E_TileType, Tile> m_TilePreviewMap = null;

		// 머터리얼 정보 맵
		[SerializeField]
		private Dictionary<string, Material> m_MaterialMap = null;
		#endregion

		#region 적 관련 변수
		#endregion

		#region 저장 & 불러오기 관련 변수
		[SerializeField, ReadOnly]
		private StageData m_EditingStageData = default;
		#endregion
		#endregion

		#region 프로퍼티
		#region 카메라 관련 프로퍼티
		public Camera mapEditorCamera { get; set; }
		public Camera thumnailCamera { get; set; }

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

		[field: SerializeField, ReadOnly]
		[field: FoldoutGroup("Info")]
		public string stageTitle { get; set; }
		[field: SerializeField, ReadOnly]
		[field: FoldoutGroup("Info")]
		public string creatorNickName { get; set; }
		[field: SerializeField, ReadOnly]
		[field: FoldoutGroup("Info")]
		public string description { get; set; }

		[field: SerializeField, ReadOnly]
		[field: FoldoutGroup("Info")]
		public int lifePoint { get; set; }

		[field: SerializeField, ReadOnly]
		[field: FoldoutGroup("Info")]
		public int initCost { get; set; }
		[field: SerializeField, ReadOnly]
		[field: FoldoutGroup("Info")]
		public int maxCost { get; set; }
		[field: SerializeField, ReadOnly]
		[field: FoldoutGroup("Info")]
		public float costIncreaseTime { get; set; }
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
		private static MapEditingUIManager M_MapEditingUI => MapEditingUIManager.Instance;

		private static TileManager M_Tile => TileManager.Instance;
		private static OperatorManager M_Operator => OperatorManager.Instance;
		private static EnemyManager M_Enemy => EnemyManager.Instance;
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
		public override void Initialize()
		{
			base.Initialize();

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
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();

			onCameraSwitcingFinished -= OnCameraSwitchingFinished;
		}

		/// <summary>
		/// 메인 초기화 함수 (본인 Main Scene 진입 시 호출)
		/// </summary>
		public override void InitializeMain()
		{
			base.InitializeMain();

			m_IsEditMode = true;

			m_TilePlacementFlag = true;

			if (m_TilePreviewMap == null)
			{
				m_TilePreviewMap = new Dictionary<E_TileType, Tile>();

				for (E_TileType tileType = E_TileType.LowGround; tileType < E_TileType.Max; ++tileType)
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
			m_CurrentTileType = E_TileType.LowGround;

			m_TilePreview = m_TilePreviewMap[m_CurrentTileType];
		}
		/// <summary>
		/// 메인 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();

			foreach (var item in m_TilePreviewMap)
			{
				string key = item.Key.ToString().Replace('_', ' ');

				item.Value.GetComponent<MeshRenderer>().material = m_MaterialMap[key];

				M_Tile.Despawn(item.Value);
			}
			m_TilePreviewMap.Clear();
			m_TilePreviewMap = null;

			m_EditingStageData = default;
			stageTitle = string.Empty;

			m_IsEditMode = false;
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
			if (EventSystem.current.currentSelectedGameObject != null)
				return;
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
			if (m_CurrentTileType == E_TileType.None ||
				m_CameraMode == E_CameraMode.GameMode ||
				m_IsCameraSwitching == true)
				return;

			// 마우스 위치 가져오기
			Vector2 mousePosition = UtilClass.GetMouseWorldPosition2D(mapEditorCamera);
			Vector2Int mousePositionInt = new Vector2Int(
				Mathf.RoundToInt(mousePosition.x),
				Mathf.RoundToInt(mousePosition.y)
				);

			if (Input.GetMouseButtonUp(0) == true ||
				Input.GetMouseButtonUp(1) == true)
				m_TilePlacementFlag = true;

			// 마우스 포인터가 UI 위에 없는 지 확인
			if (UtilClass.IsPointerOnUI() == false)
			{
				if (m_TilePlacementFlag == false)
					return;

				m_TilePreview.transform.position = (Vector3Int)mousePositionInt;
				m_TilePreview.gameObject.SetActive(true);

				Tile tile = M_Tile.GetTile(mousePositionInt);

				// 타일 배치
				if (Input.GetMouseButton(0) == true)
				{
					if (tile == null)
						M_Tile.AddTile(mousePositionInt, m_CurrentTileType);
					else if (tile.tileType != m_CurrentTileType)
						M_Tile.ReplaceTile(mousePositionInt, m_CurrentTileType);
				}
				// 타일 제거
				if (Input.GetMouseButton(1) == true &&
					tile != null)
				{
					M_Tile.RemoveTile(mousePositionInt);
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

		public Vector3 GetTileOffset(E_TileType tileType)
		{
			if (tileType == E_TileType.HighGround)
				return m_HighGroundTileOffset;

			return Vector3.zero;
		}

		public void SetTileType(E_TileType tileType)
		{
			m_CurrentTileType = tileType;
			m_TilePreview = m_TilePreviewMap[m_CurrentTileType];

			m_TilePreview.gameObject.SetActive(tileType != E_TileType.None);
		}
		#endregion

		#region 적 관련 함수
		#endregion

		#region 저장 & 불러오기 관련 함수
		private async Awaitable<byte[]> CreateThumnailRawTextureData()
		{
			int resWidth = 580;
			int resHeight = 326;

			RenderTexture renderTexture = RenderTexture.GetTemporary(resWidth, resHeight, 32);
			thumnailCamera.targetTexture = renderTexture; //Create new renderTexture and assign to camera
			RenderTexture.active = renderTexture;
			thumnailCamera.Render();

			AsyncGPUReadbackRequest request = await AsyncGPUReadback.RequestAsync(renderTexture, 0, 0, resWidth, 0, resHeight, 0, 1, TextureFormat.RGBA32);

			while (!request.done)
			{
				InfiniteLoopDetector.Run();

				await Awaitable.NextFrameAsync();
			}

			RenderTexture.active = null; //Clean
			thumnailCamera.targetTexture = null;
			RenderTexture.ReleaseTemporary(renderTexture);

			Unity.Collections.NativeArray<byte> data = request.GetData<byte>();

			return data.ToArray();
		}

		public async Awaitable SaveData()
		{
			StageData.Initialize(ref m_EditingStageData);

			m_EditingStageData.title = stageTitle;
			m_EditingStageData.creatorNickName = creatorNickName;
			m_EditingStageData.createdPlayerId = SaveLoadUtility.GetPlayerId();
			m_EditingStageData.description = description;

			m_EditingStageData.lifePoint = lifePoint;
			m_EditingStageData.initCost = initCost;
			m_EditingStageData.maxCost = maxCost;
			m_EditingStageData.costIncreaseTime = costIncreaseTime;

			m_EditingStageData.thumnail = await CreateThumnailRawTextureData();

			#region 타일 저장
			M_Tile.SaveTileData(ref m_EditingStageData);
			#endregion

			#region 오퍼레이터 저장
			M_MapEditingUI.SaveOperatorDataUI(ref m_EditingStageData);
			#endregion

			#region 적 저장
			M_MapEditingUI.SaveEnemyDataUI(ref m_EditingStageData);
			M_MapEditingUI.SaveEnemySpawnDataUI(ref m_EditingStageData);
			#endregion
		}
		public async Awaitable SaveDataToCloud()
		{
			float t1, t2;

			t1 = Time.realtimeSinceStartup;
			await SaveLoadUtility.SaveStageData(stageTitle, m_EditingStageData);
			t2 = Time.realtimeSinceStartup;
			Debug.Log("[SaveDataToCloud]: " + (t2 - t1));

			#region Debug
			TextMeshPro textMesh = UtilClass.CreateWorldText(null, stageTitle + " 저장 완료", new UtilClass.WorldTMP_TextOption()
			{
				tmpFont = M_MapEditingUI.uiFont,
				fontSize = 20,
				textAlignment = TextAlignmentOptions.Midline,
				duration = 1f,
			});
			textMesh.transform.rotation = mapEditorCamera.transform.rotation;
			#endregion
		}
		public void LoadData()
		{
			if (m_EditingStageData.title == null ||
				m_EditingStageData.title.Equals(string.Empty) == true)
				return;

			stageTitle = m_EditingStageData.title;
			description = m_EditingStageData.description;

			lifePoint = m_EditingStageData.lifePoint;
			initCost = m_EditingStageData.initCost;
			maxCost = m_EditingStageData.maxCost;
			costIncreaseTime = m_EditingStageData.costIncreaseTime;

			M_Tile.LoadTileData(m_EditingStageData);
			M_Operator.LoadOperatorData(m_EditingStageData);
			M_Enemy.LoadEnemyData(m_EditingStageData);

			M_MapEditingUI.LoadSystemSetting(m_EditingStageData);
			M_MapEditingUI.LoadOperatorDataUI(m_EditingStageData);
			M_MapEditingUI.LoadEnemySpawnDataUI(m_EditingStageData);

			#region Debug
			TextMeshPro textMesh = UtilClass.CreateWorldText(null, stageTitle + " 로드 완료", new UtilClass.WorldTMP_TextOption()
			{
				tmpFont = M_MapEditingUI.uiFont,
				fontSize = 20,
				textAlignment = TextAlignmentOptions.Midline,
				duration = 1f,
			});
			textMesh.transform.rotation = mapEditorCamera.transform.rotation;
			#endregion
		}

		public void SynchronizeStageData(in StageData stageData)
		{
			m_EditingStageData = stageData;

			stageTitle = stageData.title;
		}
		#endregion
	}
}