using System.Collections;
using System.Collections.Generic;
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
		private bool m_IsEditView = true;

		[SerializeField, ReadOnly]
		private bool m_IsCameraSwitching = false;
		#endregion

		#region 편집 모드 관련 변수
		private bool m_IsEditMode = false;
		#endregion

		#region 타일 관련 변수
		private Vector3 m_HighGroundTileOffset = Vector3.back * 0.2f;

		// 프리뷰 타일 맵
		private Dictionary<string, (Tile previewTile, Material originMaterial)> m_PreviewTileMap = null;
		// 타일 배치 가능 여부
		private bool m_TilePlacementFlag = true;
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

		public Transform editViewCameraTransform { get; set; }
		public Transform gameViewCameraTransform { get; set; }

		private bool isEditView => m_IsEditView;
		private bool isGameView => !m_IsEditView;
		#endregion

		#region 편집 모드 관련 프로퍼티
		public bool isEditMode => m_IsEditMode;
		#endregion

		#region 타일 관련 프로퍼티
		public string tileKey { get; set; }
		public E_TileType tileType { get; set; }
		public E_TilePositionType tilePositionType { get; set; }
		public E_TileDeployableTypeFlag tileDeployableTypeFlag { get; set; }
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

		#region 이벤트 함수
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

			TileEditModeProcess();
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			m_PreviewTileMap = new Dictionary<string, (Tile previewTile, Material originMaterial)>();
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();

			m_PreviewTileMap = null;
		}

		/// <summary>
		/// 메인 초기화 함수 (본인 Main Scene 진입 시 호출)
		/// </summary>
		public override void InitializeMain()
		{
			base.InitializeMain();

			m_IsEditMode = true;

			m_TilePlacementFlag = true;
			
			// 프리뷰 타일 생성
			List<TileData> tileDataList = M_Tile.GetAllTileDatas();
			for (int i = 0; i < tileDataList.Count; ++i)
			{
				string key = tileDataList[i].key;

				Tile previewTile = M_Tile.GetBuilder(key)
					.SetParent(transform)
					.SetName(key + " Preview")
					.SetAutoInit(true)
					.SetActive(false)
					.Spawn();

				MeshRenderer meshRenderer = previewTile.GetComponent<MeshRenderer>();

				Material originMaterial = meshRenderer.material;
				Material previewMaterial = new Material(originMaterial);

				Color previewColor = previewMaterial.color;
				previewColor.a = 0.3f;
				previewMaterial.color = previewColor;

				meshRenderer.material = previewMaterial;

				m_PreviewTileMap.Add(key, (previewTile, originMaterial));
			}

			tileKey = string.Empty;
			tileType = E_TileType.Default;
			tilePositionType = E_TilePositionType.LowGround;
			tileDeployableTypeFlag = E_TileDeployableTypeFlag.None;
		}
		/// <summary>
		/// 메인 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();

			foreach (var item in m_PreviewTileMap)
			{
				item.Value.previewTile.GetComponent<MeshRenderer>().material = item.Value.originMaterial;

				M_Tile.Despawn(item.Value.previewTile);
			}
			m_PreviewTileMap.Clear();

			m_EditingStageData = default;
			stageTitle = string.Empty;

			m_IsEditMode = false;
		}
		#endregion

		#region 카메라 관련 함수
		private void SwitchCameraMode()
		{
			if (EventSystem.current.currentSelectedGameObject != null)
				return;
			if (m_IsCameraSwitching == true)
				return;

			Transform targetTransform = (m_IsEditView == true) ? gameViewCameraTransform : editViewCameraTransform;

			StartCoroutine(MoveCamera(targetTransform));
			if (m_IsEditView == true)
			{
				mapEditorCamera.orthographic = false;
				//m_TilePreview.gameObject.SetActive(false);
			}

			m_IsCameraSwitching = true;
			m_IsEditView = !m_IsEditView;
		}

		private IEnumerator MoveCamera(Transform targetTransform)
		{
			if (m_CameraSwitchDuration <= 0f)
			{
				mapEditorCamera.transform.position = targetTransform.position;
				mapEditorCamera.transform.rotation = targetTransform.rotation;
				m_IsCameraSwitching = false;

				if (m_IsEditView)
					mapEditorCamera.orthographic = true;
				yield break;
			}

			Vector3 initPosition = mapEditorCamera.transform.position;
			Quaternion initRotation = mapEditorCamera.transform.rotation;
			float t = 0f;

			for (float time = 0f; time < m_CameraSwitchDuration; time += Time.deltaTime)
			{
				yield return null;

				t = Mathf.Clamp01(time / m_CameraSwitchDuration);

				mapEditorCamera.transform.position = Vector3.Lerp(initPosition, targetTransform.position, t);
				mapEditorCamera.transform.rotation = Quaternion.Lerp(initRotation, targetTransform.rotation, t);
			}

			mapEditorCamera.transform.position = targetTransform.position;
			mapEditorCamera.transform.rotation = targetTransform.rotation;
			m_IsCameraSwitching = false;

			if (m_IsEditView)
				mapEditorCamera.orthographic = true;
		}
		#endregion

		#region 타일 편집 모드 관련 함수
		private void TileEditModeProcess()
		{
			if (isEditView == false ||
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

				if (string.IsNullOrEmpty(tileKey) == false)
				{
					Tile previewTile = m_PreviewTileMap[tileKey].previewTile;

					previewTile.transform.position = (Vector3Int)mousePositionInt;
					previewTile.gameObject.SetActive(true);
				}

				// 타일 배치
				if (Input.GetMouseButton(0) == true &&
					string.IsNullOrEmpty(tileKey) == false)
				{
					TileSpawnData tileSpawnData = new TileSpawnData()
					{
						TileSpawnKey = tileKey,
						TilePos = mousePositionInt,
						TileType = tileType,
						TilePositionType = tilePositionType,
						TileDeployableTypeFlag = tileDeployableTypeFlag,
					};

					M_Tile.AddTile(tileSpawnData);
				}
				// 타일 제거
				if (Input.GetMouseButton(1) == true)
				{
					M_Tile.RemoveTile(mousePositionInt);
				}
			}
			// 마우스 포인터가 UI 위에 있을 때
			else
			{
				if (string.IsNullOrEmpty(tileKey) == false)
				{
					Tile previewTile = m_PreviewTileMap[tileKey].previewTile;

					previewTile?.gameObject.SetActive(false);
				}

				if (Input.GetMouseButtonDown(0) == true ||
					Input.GetMouseButtonDown(1) == true)
					m_TilePlacementFlag = false;
			}
		}

		public Vector3 GetTileOffset(E_TilePositionType tileType)
		{
			if (tileType == E_TilePositionType.HighGround)
				return m_HighGroundTileOffset;

			return Vector3.zero;
		}

		public void AddTileDeployableTypeFlag(E_TileDeployableTypeFlag flag)
		{
			tileDeployableTypeFlag |= flag;
		}
		public void RemoveTileDeployableTypeFlag(E_TileDeployableTypeFlag flag)
		{
			tileDeployableTypeFlag &= ~flag;
		}
		public bool HasTileDeployableTypeFlag(E_TileDeployableTypeFlag flag)
		{
			return tileDeployableTypeFlag.HasFlag(flag);
		}
		#endregion

		#region 적 관련 함수
		#endregion

		#region 저장 & 불러오기 관련 함수
		private async Awaitable<byte[]> CreateThumnailRawTextureData()
		{
			// Main Menu Scene Thumanil Image 크기
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

			#region Data 저장
			M_Tile.SaveTileData(ref m_EditingStageData);
			M_MapEditingUI.SaveOperatorData(ref m_EditingStageData);
			M_MapEditingUI.SaveEnemyData(ref m_EditingStageData);
			#endregion

			#region SpawnData 저장
			M_Tile.SaveSpawnTileData(ref m_EditingStageData);
			M_MapEditingUI.SaveOperatorSpawnData(ref m_EditingStageData);
			M_MapEditingUI.SaveEnemySpawnData(ref m_EditingStageData);
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
			M_MapEditingUI.LoadOperatorSetting(m_EditingStageData);
			M_MapEditingUI.LoadEnemySetting(m_EditingStageData);

			#region Debug
			if (string.IsNullOrEmpty(stageTitle) == false)
			{
				TextMeshPro textMesh = UtilClass.CreateWorldText(null, stageTitle + " 로드 완료", new UtilClass.WorldTMP_TextOption()
				{
					tmpFont = M_MapEditingUI.uiFont,
					fontSize = 20,
					textAlignment = TextAlignmentOptions.Midline,
					duration = 1f,
				});
				textMesh.transform.rotation = mapEditorCamera.transform.rotation;
			}
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