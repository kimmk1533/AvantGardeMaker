using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AvantGardeMaker.CoreSpace;
using AvantGardeMaker.CoreSpace.SaveLoad;
using AvantGardeMaker.EnemySpace;
using AvantGardeMaker.OperatorSpace;
using AvantGardeMaker.TileSpace;
using AvantGardeMaker.TileSpace.Enum;
using CoreSources;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class MapEditingSceneUIManager : ObjectManager<MapEditingSceneUIManager, MapEditingSceneUIPoolItem>
	{
		#region 변수
		#region 시스템 설정 관련 변수

		#endregion

		#region 타일 설정 관련 변수
		private Dictionary<string, TileDataUI> m_SpawnedTileDataUIMap = null;

		private Dictionary<E_TileThemaType, RectTransform> m_TileDataUIParentMap = null;
		#endregion

		#region 오퍼레이터 설정 관련 변수
		private Dictionary<string, OperatorDataUI> m_SpawnedOperatorDataUIMap = null;
		#endregion

		#region 적 설정 관련 변수
		private List<EnemyDataUI> m_SpawnedEnemyDataUIList = null;
		private List<EnemySpawnDataUI> m_SpawnedEnemySpawnDataUIList = null;
		#endregion
		#endregion

		#region 프로퍼티
		#region 폰트 관련 프로퍼티
		[field: SerializeField]
		public TMP_FontAsset uiFont { get; }
		#endregion

		#region 버튼 관련 프로퍼티
		public Button mainMenuButton { get; set; }
		public Button saveButton { get; set; }
		public Button playButton { get; set; }
		#endregion

		#region 컨트롤러 관련 프로퍼티
		public MenuPanelController menuPanelController { get; set; }
		public SettingPanelController settingPanelController { get; set; }
		#endregion

		#region 타일 설정 관련 프로퍼티
		public RectTransform tileDataUIContent { get; set; }
		#endregion

		#region 오퍼레이터 설정 관련 프로퍼티
		public RectTransform operatorDataUIContent { get; set; }
		#endregion

		#region 적 설정 관련 프로퍼티
		public RectTransform enemyDataUIContent { get; set; }

		public RectTransform enemySpawnDataUIParent { get; set; }
		public RectTransform enemyWayPointDataUIParent { get; set; }
		public RectTransform enemyImmuneDescriptionParent { get; set; }

		public int maxWave { get; private set; }
		#endregion
		#endregion

		#region 이벤트

		#region 이벤트 함수
		private void OnMainMenuButtonClicked()
		{
			SceneLoader.LoadScene("Main Menu Scene");
		}
		private async void OnSaveButtonClicked()
		{
			SystemSettingPanel systemSettingPanel = settingPanelController.GetSettingPanel<SystemSettingPanel>();

			if (string.IsNullOrEmpty(systemSettingPanel.stageTitle) == true)
			{
				Debug.LogError("스테이지명 비어있음");
				return;
			}

			await M_MapEditing.SaveData();

			await M_MapEditing.SaveDataToCloud();
		}
		private async void OnPlayButtonClicked()
		{
			await M_MapEditing.SaveData();

			M_GamePlaying.SynchronizeStageData(M_MapEditing.currentStageData);

			SceneLoader.LoadScene("Game Playing Scene");
		}

		private void OnEnemySpawnDataUISpawned(MapEditingSceneUIPoolItem mapEditingUI)
		{
			EnemySpawnDataUI enemySpawnDataUI = mapEditingUI as EnemySpawnDataUI;

			if (m_SpawnedEnemySpawnDataUIList.Contains(enemySpawnDataUI) == true)
				return;

			m_SpawnedEnemySpawnDataUIList.Add(enemySpawnDataUI);

			ReorderEnemySpawnDataUI();
		}
		private void OnEnemySpawnDataUIDespawned(MapEditingSceneUIPoolItem mapEditingUI)
		{
			EnemySpawnDataUI enemySpawnDataUI = mapEditingUI as EnemySpawnDataUI;

			m_SpawnedEnemySpawnDataUIList.Remove(enemySpawnDataUI);

			ReorderEnemySpawnDataUI();
		}
		#endregion
		#endregion

		#region 매니저
		private static MapEditingManager M_MapEditing => MapEditingManager.Instance;
		private static GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;

		private static TileManager M_Tile => TileManager.Instance;
		private static OperatorManager M_Operator => OperatorManager.Instance;
		private static EnemyManager M_Enemy => EnemyManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		//private void Update()
		//{
		//	MenuShortcut();
		//}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			m_SpawnedTileDataUIMap = new Dictionary<string, TileDataUI>();
			m_TileDataUIParentMap = new Dictionary<E_TileThemaType, RectTransform>();

			m_SpawnedOperatorDataUIMap = new Dictionary<string, OperatorDataUI>();

			m_SpawnedEnemyDataUIList = new List<EnemyDataUI>();
			m_SpawnedEnemySpawnDataUIList = new List<EnemySpawnDataUI>();
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

			mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
			saveButton.onClick.AddListener(OnSaveButtonClicked);
			playButton.onClick.AddListener(OnPlayButtonClicked);

			menuPanelController.Initialize();
			settingPanelController.Initialize();

			maxWave = 0;

			CreateTileDataUI();
			CreateOperatorDataUI();
			CreateEnemyDataUI();

			GetPool("Enemy Spawn Data UI").onItemSpawned += OnEnemySpawnDataUISpawned;
			GetPool("Enemy Spawn Data UI").onItemDespawned += OnEnemySpawnDataUIDespawned;
		}
		/// <summary>
		/// 메인 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();

			ClearTileDataUI();
			ClearOperatorDataUI();
			ClearEnemyDataUI();
			ClearEnemySpawnDataUI();

			menuPanelController.Finallize();
			settingPanelController.Finallize();

			GetPool("Enemy Spawn Data UI").onItemSpawned -= OnEnemySpawnDataUISpawned;
			GetPool("Enemy Spawn Data UI").onItemDespawned -= OnEnemySpawnDataUIDespawned;
		}
		#endregion

		private void CreateTileDataUI()
		{
			List<TileData> tileDataList = M_Tile.GetAllTileDatas();
			RectTransform template = tileDataUIContent.Find<RectTransform>("Tile Data UI Group Template");

			//tileDataList = tileDataList
			//	.OrderBy(tileData => tileData.FixedData.TileType)
			//	.ToList();

			for (int i = 0; i < tileDataList.Count; ++i)
			{
				TileData tileData = tileDataList[i];
				E_TileThemaType tileThemaType = tileData.FixedData.ThemaType;

				// 타일 테마 그룹 생성
				if (m_TileDataUIParentMap.TryGetValue(tileThemaType, out RectTransform tileDataUIParent) == false)
				{
					tileDataUIParent = Instantiate(template, tileDataUIContent);
					tileDataUIParent.name = tileThemaType.ToString();

					TextMeshProUGUI titleTextMesh = tileDataUIParent.Find<TextMeshProUGUI>("Title/Text");
					titleTextMesh.text = TileEnumUtil.GetTileThemaKorString(tileThemaType);

					tileDataUIParent.gameObject.SetActive(true);

					m_TileDataUIParentMap.Add(tileThemaType, tileDataUIParent);
				}

				TileDataUI tileDataUI = GetBuilder("Tile Data UI")
					.SetParent(tileDataUIParent)
					.SetScale(Vector3.one)
					.SetAutoInit(true)
					.SetActive(true)
					.Spawn<TileDataUI>();

				tileDataUI.tileData = tileData;

				m_SpawnedTileDataUIMap.Add(tileData.key, tileDataUI);
			}
		}
		private void CreateOperatorDataUI()
		{
			List<OperatorData> operatorDataList = M_Operator.GetAllOperatorDatas();

			for (int i = 0; i < operatorDataList.Count; ++i)
			{
				OperatorDataUI operatorDataUI = GetBuilder("Operator Data UI")
					.SetParent(operatorDataUIContent)
					.SetScale(Vector3.one)
					.SetAutoInit(true)
					.SetActive(true)
					.Spawn<OperatorDataUI>();

				operatorDataUI.operatorData = operatorDataList[i];

				m_SpawnedOperatorDataUIMap.Add(operatorDataList[i].key, operatorDataUI);
			}
		}
		private void CreateEnemyDataUI()
		{
			List<EnemyData> enemyDataList = M_Enemy.GetAllEnemyData();

			for (int i = 0; i < enemyDataList.Count; ++i)
			{
				EnemyDataUI enemyDataUI = GetBuilder("Enemy Data UI")
					.SetParent(enemyDataUIContent)
					.SetScale(Vector3.one)
					.SetAutoInit(true)
					.SetActive(true)
					.Spawn<EnemyDataUI>();

				enemyDataUI.enemyData = enemyDataList[i];

				m_SpawnedEnemyDataUIList.Add(enemyDataUI);
			}
		}

		private void ClearTileDataUI()
		{
			foreach (var item in m_SpawnedTileDataUIMap)
			{
				Despawn(item.Value);
			}
			m_SpawnedTileDataUIMap.Clear();

			m_TileDataUIParentMap.Clear();
		}
		private void ClearOperatorDataUI()
		{
			foreach (KeyValuePair<string, OperatorDataUI> item in m_SpawnedOperatorDataUIMap)
			{
				Despawn(item.Value);
			}
			m_SpawnedOperatorDataUIMap.Clear();
		}
		private void ClearEnemyDataUI()
		{
			int count = m_SpawnedEnemyDataUIList.Count;
			for (int i = 0; i < count; ++i)
			{
				Despawn(m_SpawnedEnemyDataUIList[i]);
			}
			m_SpawnedEnemyDataUIList.Clear();
		}
		private void ClearEnemySpawnDataUI()
		{
			int count = m_SpawnedEnemySpawnDataUIList.Count;
			for (int i = 0; i < count; ++i)
			{
				Despawn(m_SpawnedEnemySpawnDataUIList[0]);
			}
			m_SpawnedEnemySpawnDataUIList.Clear();
		}

		#region Save
		public void SaveOperatorData(ref StageData stageData)
		{
			OperatorSettingPanel operatorSettingPanel = settingPanelController.GetSettingPanel<OperatorSettingPanel>();

			operatorSettingPanel.SaveOperatorData(ref stageData);
		}
		public void SaveOperatorSpawnData(ref StageData stageData)
		{
			OperatorSettingPanel operatorSettingPanel = settingPanelController.GetSettingPanel<OperatorSettingPanel>();

			operatorSettingPanel.SaveOperatorSpawnData(ref stageData);
		}

		public void SaveEnemyData(ref StageData stageData)
		{
			for (int i = 0; i < m_SpawnedEnemyDataUIList.Count; ++i)
			{
				EnemyDataUI enemyDataUI = m_SpawnedEnemyDataUIList[i];

				stageData.SaveEnemyData(enemyDataUI.enemyData);
			}
		}
		public void SaveEnemySpawnData(ref StageData stageData)
		{
			for (int i = 0; i < m_SpawnedEnemySpawnDataUIList.Count; ++i)
			{
				EnemySpawnDataUI enemySpawnDataUI = m_SpawnedEnemySpawnDataUIList[i];

				stageData.SaveEnemySpawnData(enemySpawnDataUI.MakeSpawnData());
			}
		}
		#endregion

		#region Load
		public void LoadSystemSetting(in StageData stageData)
		{
			SystemSettingPanel systemSettingPanel = settingPanelController.GetSettingPanel<SystemSettingPanel>();

			systemSettingPanel.stageTitle = stageData.title;
			systemSettingPanel.lifePoint = stageData.lifePoint;
			systemSettingPanel.initCost = stageData.initCost;
			systemSettingPanel.maxCost = stageData.maxCost;
			systemSettingPanel.costIncreaseTime = stageData.costIncreaseTime;
			systemSettingPanel.description = stageData.description;
		}
		public void LoadOperatorSetting(in StageData stageData)
		{
			OperatorSettingPanel operatorSettingPanel = settingPanelController.GetSettingPanel<OperatorSettingPanel>();

			operatorSettingPanel.LoadOperatorData(stageData);
		}
		public void LoadEnemySetting(in StageData stageData)
		{
			ClearEnemySpawnDataUI();

			List<EnemySpawnData> enemySpawnDataList = stageData.enemySpawnDataList;
			for (int i = 0; i < enemySpawnDataList.Count; ++i)
			{
				EnemySpawnDataUI enemySpawnDataUI = GetBuilder("Enemy Spawn Data UI")
					.SetParent(enemySpawnDataUIParent)
					.SetScale(Vector3.one)
					.SetActive(true)
					.SetAutoInit(true)
					.Spawn<EnemySpawnDataUI>();

				EnemySpawnData enemySpawnData = enemySpawnDataList[i];

				enemySpawnDataUI.enemyData = M_Enemy.GetEnemyData(enemySpawnData.EnemySpawnKey);

				enemySpawnDataUI.debugText = enemySpawnDataUI.enemyData.KorName;
				enemySpawnDataUI.count = enemySpawnData.Amount;
				enemySpawnDataUI.interval = enemySpawnData.Interval;
				enemySpawnDataUI.time = enemySpawnData.Time;
				enemySpawnDataUI.wave = enemySpawnData.Wave;
				enemySpawnDataUI.waveTime = enemySpawnData.WaveTime;

				enemySpawnDataUI.enemyWayPointList = enemySpawnData.WayPointList;
				enemySpawnDataUI.enemyWayPointDelayTimeList = enemySpawnData.DelayTimeList;
				enemySpawnDataUI.LoadWayPointUI();
			}

			ReorderEnemySpawnDataUI();
		}
		#endregion

		public void RemoveOperatorDataUI(string key)
		{
			if (m_SpawnedOperatorDataUIMap.TryGetValue(key, out OperatorDataUI operatorDataUI) == false)
				return;

			operatorDataUI.gameObject.SetActive(false);
		}
		public void RespawnOperatorDataUI(string key)
		{
			if (m_SpawnedOperatorDataUIMap.TryGetValue(key, out OperatorDataUI operatorDataUI) == false)
				return;

			operatorDataUI.gameObject.SetActive(true);
		}

		public void ReorderEnemySpawnDataUI()
		{
			float waveTime = 0f;
			int wave = 0;
			EnemySpawnDataUI prevSpawnDataUI = null;

			m_SpawnedEnemySpawnDataUIList = m_SpawnedEnemySpawnDataUIList
				.OrderBy(enemySpawnDataUI => enemySpawnDataUI.wave)
				.ThenBy(enemySpawnDataUI => enemySpawnDataUI.time)
				.ToList();

			for (int i = 0; i < m_SpawnedEnemySpawnDataUIList.Count; ++i)
			{
				EnemySpawnDataUI enemySpawnDataUI = m_SpawnedEnemySpawnDataUIList[i];

				enemySpawnDataUI.transform.SetSiblingIndex(i);
				enemySpawnDataUI.index = i;

				waveTime = enemySpawnDataUI.waveTime;
				if (wave < enemySpawnDataUI.wave)
				{
					if (wave + 1 != enemySpawnDataUI.wave)
						enemySpawnDataUI.wave = wave + 1;
					wave = enemySpawnDataUI.wave;
					waveTime += (prevSpawnDataUI == null) ? 0f : prevSpawnDataUI.time;
					maxWave = wave;
				}

				enemySpawnDataUI.time = waveTime;

				prevSpawnDataUI = enemySpawnDataUI;
			}
		}
	}
}