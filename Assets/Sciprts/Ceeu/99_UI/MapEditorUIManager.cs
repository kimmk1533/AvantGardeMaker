using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AvantGardeMaker.ad1a;
using AvantGardeMaker.Ceeu.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class MapEditorUIManager : ObjectManager<MapEditorUIManager, MapEditorUI>
	{
		#region 변수
		[PropertySpace]
		[SerializeField]
		private List<string> m_KeyList = new List<string>();

		#region 메뉴 패널 관련 변수
		#endregion

		#region 옵션 패널 관련 변수
		private int m_MaxWave;
		#endregion

		private Stack<Panel> m_PanelStack = null;

		private List<EnemyDataUI> m_SpawnedEnemyDataUIList = null;
		private List<EnemySpawnDataUI> m_SpawnedEnemySpawnDataUIList = null;
		#endregion

		#region 프로퍼티
		public List<string> keyList => m_KeyList;

		#region 메뉴 패널 관련 프로퍼티
		public MenuPanel menuPanel { get; set; }
		#endregion

		#region 옵션 패널 관련 프로퍼티
		public OptionPanel optionPanel { get; set; }

		public EnemyDataSettingPanel enemyDataSettingPanel { get; set; }

		public RectTransform enemySpawnDataUIParent { get; set; }
		public RectTransform enemyDataUIParent { get; set; }
		public RectTransform enemyWayPointDataUIParent { get; set; }
		public RectTransform enemyImmuneDescriptionParent { get; set; }

		public int maxWave { get => m_MaxWave; }
		#endregion
		#endregion

		#region 이벤트

		#region 이벤트 함수
		private void OnEnemySpawnDataUISpawned(ObjectPoolItemBase objectPoolItem)
		{
			EnemySpawnDataUI enemySpawnDataUI = objectPoolItem as EnemySpawnDataUI;

			if (m_SpawnedEnemySpawnDataUIList.Contains(enemySpawnDataUI) == true)
				return;

			m_SpawnedEnemySpawnDataUIList.Add(enemySpawnDataUI);

			ReorderEnemySpawnDataUI();
		}
		private void OnEnemySpawnDataUIDespawned(ObjectPoolItemBase objectPoolItem)
		{
			EnemySpawnDataUI enemySpawnDataUI = objectPoolItem as EnemySpawnDataUI;

			m_SpawnedEnemySpawnDataUIList.Remove(enemySpawnDataUI);

			ReorderEnemySpawnDataUI();
		}
		#endregion
		#endregion

		#region 매니저
		private static MapEditorManager M_MapEditor => MapEditorManager.Instance;
		private static EnemyManager M_Enemy => EnemyManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			//MenuShortcut();
			PanelStackShortcut();
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			m_PanelStack = new Stack<Panel>();

			m_SpawnedEnemyDataUIList = new List<EnemyDataUI>();
			m_SpawnedEnemySpawnDataUIList = new List<EnemySpawnDataUI>();

			GetPool("Enemy Spawn Data UI").onSpawned += OnEnemySpawnDataUISpawned;
			GetPool("Enemy Spawn Data UI").onDespawned += OnEnemySpawnDataUIDespawned;

			gameObject.SetActive(false);
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();
		}

		/// <summary>
		/// 게임 초기화 함수 (본인 Main Scene 진입 시 호출)
		/// </summary>
		public override void InitializeMain()
		{
			base.InitializeMain();

			menuPanel.Initialize();

			optionPanel.Initialize();
			enemyDataSettingPanel.Initialize();

			m_MaxWave = 0;

			List<EnemyData> enemyDataList = M_Enemy.GetAllEnemyData();
			for (int i = 0; i < enemyDataList.Count; ++i)
			{
				EnemyDataUI enemyDataUI = GetBuilder("Enemy Data UI")
					.SetParent(enemyDataUIParent)
					.SetScale(Vector3.one)
					.SetAutoInit(true)
					.SetActive(true)
					.Spawn() as EnemyDataUI;

				enemyDataUI.enemyData = enemyDataList[i];
				enemyDataUI.debugText = enemyDataList[i].KrName;

				m_SpawnedEnemyDataUIList.Add(enemyDataUI);
			}
			gameObject.SetActive(true);
		}
		/// <summary>
		/// 게임 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();

			m_PanelStack.Clear();

			for (int i = 0; i < m_SpawnedEnemyDataUIList.Count; ++i)
			{
				Despawn(m_SpawnedEnemyDataUIList[i]);
			}
			m_SpawnedEnemyDataUIList.Clear();
			for (int i = 0; i < m_SpawnedEnemySpawnDataUIList.Count; ++i)
			{
				Despawn(m_SpawnedEnemySpawnDataUIList[i]);
			}
			m_SpawnedEnemySpawnDataUIList.Clear();

			menuPanel.Finallize();

			optionPanel.Finallize();
			enemyDataSettingPanel.Finallize();
		}
		#endregion

		private void MenuShortcut()
		{
			foreach (string key in m_KeyList)
			{
				OptionViewport optionViewport = optionPanel.optionViewportController[key];
				KeyCode keyCode = optionViewport.shortcut;
				if (Input.GetKeyDown(keyCode) == true)
				{
					optionViewport.OnMenuButtonClicked();
					M_MapEditor.SetEditModeType(optionViewport.editModeType);
				}
			}
		}
		private void PanelStackShortcut()
		{
			if (m_PanelStack.Count <= 0)
				return;
			if (Input.GetKeyDown(KeyCode.Escape) == false)
				return;

			Panel panel = m_PanelStack.Peek();
			while (m_PanelStack.Count > 0 &&
				panel != null &&
				panel.gameObject.activeSelf == false)
				panel = m_PanelStack.Pop();

			panel.gameObject.SetActive(false);
		}
		public void RegisterPanel(Panel panel)
		{
			if (m_PanelStack.Contains(panel) == true)
				return;

			m_PanelStack.Push(panel);
		}

		public void SaveEnemyDataUI(ref StageData stageData)
		{
			for (int i = 0; i < m_SpawnedEnemyDataUIList.Count; ++i)
			{
				EnemyDataUI enemyDataUI = m_SpawnedEnemyDataUIList[i];

				stageData.AddEnemyData(enemyDataUI.enemyData);
			}
		}
		public void SaveEnemySpawnDataUI(ref StageData stageData)
		{
			for (int i = 0; i < m_SpawnedEnemySpawnDataUIList.Count; ++i)
			{
				EnemySpawnDataUI enemySpawnDataUI = m_SpawnedEnemySpawnDataUIList[i];

				stageData.AddEnemySpawnData(enemySpawnDataUI.MakeSpawnData());
			}
		}

		public void LoadEnemySpawnDataUI(ref StageData stageData)
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
					.Spawn() as EnemySpawnDataUI;

				EnemySpawnData enemySpawnData = enemySpawnDataList[i];

				enemySpawnDataUI.enemyData = M_Enemy.GetEnemyData(enemySpawnData.Name);

				enemySpawnDataUI.debugText = enemySpawnData.Name;
				enemySpawnDataUI.count = enemySpawnData.Amount;
				enemySpawnDataUI.interval = enemySpawnData.Interval;
				enemySpawnDataUI.time = enemySpawnData.Time;
				enemySpawnDataUI.wave = enemySpawnData.Wave;
				enemySpawnDataUI.waveTime = enemySpawnData.WaveTime;

				enemySpawnDataUI.enemyWayPointList = enemySpawnData.TransitPosList;
				enemySpawnDataUI.enemyWayPointDelayTimeList = enemySpawnData.DelayTimeList;
				enemySpawnDataUI.LoadWayPointUI();
			}
			ReorderEnemySpawnDataUI();
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