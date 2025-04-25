using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AvantGardeMaker.CoreSpace;
using AvantGardeMaker.CoreSpace.SaveLoad;
using AvantGardeMaker.EnemySpace;
using AvantGardeMaker.OperatorSpace;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class MapEditingUIManager : ObjectManager<MapEditingUIManager, MapEditingUIPoolItem>
	{
		#region 변수
		#region 메뉴 패널 관련 변수
		#endregion

		#region 옵션 패널 관련 변수
		private int m_MaxWave = -1;
		#endregion

		private Dictionary<string, OperatorDataUI> m_SpawnedOperatorDataUIMap = null;
		private List<EnemyDataUI> m_SpawnedEnemyDataUIList = null;

		#region 오퍼레이터 저장 관련 변수
		#endregion

		#region 적 저장 관련 변수
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
		public SettingPanelController settingPanelController { get; set; }
		public MenuPanelController menuPanelController { get; set; }
		#endregion

		#region 타일 설정 관련 프로퍼티
		public RectTransform tileDataUIParent { get; set; }

		public RectTransform tileSettingButtonParent { get; set; }
		#endregion

		#region 오퍼레이터 설정 관련 프로퍼티
		public RectTransform operatorDataUIParent { get; set; }
		#endregion

		#region 적 설정 관련 프로퍼티
		public RectTransform enemyDataUIParent { get; set; }
		public RectTransform enemySpawnDataUIParent { get; set; }
		public RectTransform enemyWayPointDataUIParent { get; set; }
		public RectTransform enemyImmuneDescriptionParent { get; set; }

		public int maxWave { get => m_MaxWave; }
		#endregion
		#endregion

		#region 이벤트

		#region 이벤트 함수
		private void OnMainMenuButtonClicked()
		{
			SceneLoader.LoadScene("Main Menu Scene");
		}
		private void OnSaveButtonClicked()
		{
			M_MapEditing.SaveData();

			M_MapEditing.SaveDataToCloud();
		}
		private void OnPlayButtonClicked()
		{
			M_MapEditing.SaveData();

			M_GamePlaying.SynchronizeStageData(M_MapEditing.currentStageData);

			SceneLoader.LoadScene("Game Playing Scene");
		}

		private void OnEnemySpawnDataUISpawned(MapEditingUIPoolItem mapEditingUI)
		{
			EnemySpawnDataUI enemySpawnDataUI = mapEditingUI as EnemySpawnDataUI;

			if (m_SpawnedEnemySpawnDataUIList.Contains(enemySpawnDataUI) == true)
				return;

			m_SpawnedEnemySpawnDataUIList.Add(enemySpawnDataUI);

			ReorderEnemySpawnDataUI();
		}
		private void OnEnemySpawnDataUIDespawned(MapEditingUIPoolItem mapEditingUI)
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

			settingPanelController.Initialize();
			menuPanelController.Initialize();

			m_MaxWave = 0;

			#region Operator Data UI 생성
			List<OperatorData> operatorDataList = M_Operator.GetAllOperatorDatas();
			for (int i = 0; i < operatorDataList.Count; ++i)
			{
				OperatorDataUI operatorDataUI = GetBuilder("Operator Data UI")
					.SetParent(operatorDataUIParent)
					.SetScale(Vector3.one)
					.SetAutoInit(true)
					.SetActive(true)
					.Spawn<OperatorDataUI>();

				operatorDataUI.operatorData = operatorDataList[i];

				m_SpawnedOperatorDataUIMap.Add(operatorDataList[i].EngName, operatorDataUI);
			}
			#endregion

			#region Spawn Enemy Data UI
			List<EnemyData> enemyDataList = M_Enemy.GetAllEnemyData();
			for (int i = 0; i < enemyDataList.Count; ++i)
			{
				EnemyDataUI enemyDataUI = GetBuilder("Enemy Data UI")
					.SetParent(enemyDataUIParent)
					.SetScale(Vector3.one)
					.SetAutoInit(true)
					.SetActive(true)
					.Spawn<EnemyDataUI>();

				enemyDataUI.enemyData = enemyDataList[i];
				enemyDataUI.debugText = enemyDataList[i].KorName;

				m_SpawnedEnemyDataUIList.Add(enemyDataUI);
			}
			#endregion

			GetPool("Enemy Spawn Data UI").onItemSpawned += OnEnemySpawnDataUISpawned;
			GetPool("Enemy Spawn Data UI").onItemDespawned += OnEnemySpawnDataUIDespawned;
		}
		/// <summary>
		/// 메인 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();

			m_SpawnedOperatorDataUIMap.Clear();
			m_SpawnedEnemyDataUIList.Clear();
			m_SpawnedEnemySpawnDataUIList.Clear();

			menuPanelController.Finallize();
			settingPanelController.Finallize();
		}
		#endregion

		//private void MenuShortcut()
		//{
		//	foreach (string key in m_KeyList)
		//	{
		//		OptionViewport optionViewport = settingPanelController.optionViewportController[key];
		//		KeyCode keyCode = optionViewport.shortcut;
		//		if (Input.GetKeyDown(keyCode) == true)
		//		{
		//			optionViewport.OnMenuButtonClicked();
		//			M_MapEditing.SetEditModeType(optionViewport.editModeType);
		//		}
		//	}
		//}

		#region Save
		public void SaveOperatorDataUI(ref StageData stageData)
		{
			OperatorSettingPanel operatorSettingPanel = settingPanelController.GetSettingPanel<OperatorSettingPanel>("Operator");

			operatorSettingPanel.SaveOperatorDataUI(ref stageData);
		}
		public void SaveEnemyDataUI(ref StageData stageData)
		{
			for (int i = 0; i < m_SpawnedEnemyDataUIList.Count; ++i)
			{
				EnemyDataUI enemyDataUI = m_SpawnedEnemyDataUIList[i];

				stageData.SaveEnemyData(enemyDataUI.enemyData);
			}
		}
		public void SaveEnemySpawnDataUI(ref StageData stageData)
		{
			for (int i = 0; i < m_SpawnedEnemySpawnDataUIList.Count; ++i)
			{
				EnemySpawnDataUI enemySpawnDataUI = m_SpawnedEnemySpawnDataUIList[i];

				stageData.SaveEnemySpawnData(enemySpawnDataUI.MakeSpawnData());
			}
		}
		#endregion

		#region Load
		public void LoadOperatorDataUI(StageData stageData)
		{
			OperatorSettingPanel operatorSettingPanel = settingPanelController.GetSettingPanel<OperatorSettingPanel>("Operator");

			operatorSettingPanel.LoadOperatorDataUI(stageData);
		}
		public void LoadEnemySpawnDataUI(StageData stageData)
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

				enemySpawnDataUI.enemyData = M_Enemy.GetEnemyData(enemySpawnData.Name);

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

		public void ClearOperatorDataUI()
		{
			foreach (KeyValuePair<string, OperatorDataUI> item in m_SpawnedOperatorDataUIMap)
			{
				Despawn(item.Value);
			}
			m_SpawnedOperatorDataUIMap.Clear();
		}
		public void ClearEnemyDataUI()
		{
			int count = m_SpawnedEnemyDataUIList.Count;
			for (int i = 0; i < count; ++i)
			{
				Despawn(m_SpawnedEnemyDataUIList[i]);
			}
			m_SpawnedEnemyDataUIList.Clear();
		}
		public void ClearEnemySpawnDataUI()
		{
			int count = m_SpawnedEnemySpawnDataUIList.Count;
			for (int i = 0; i < count; ++i)
			{
				Despawn(m_SpawnedEnemySpawnDataUIList[0]);
			}
			m_SpawnedEnemySpawnDataUIList.Clear();
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
					m_MaxWave = wave;
				}

				enemySpawnDataUI.time = waveTime;

				prevSpawnDataUI = enemySpawnDataUI;
			}
		}
	}
}