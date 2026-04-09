using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.TileSpace;
using AvantGardeMaker.OperatorSpace;
using Sirenix.OdinInspector;
using UnityEngine;
using AvantGardeMaker.UI;
using AvantGardeMaker.TileSpace.Enum;
using AvantGardeMaker.CoreSpace.SaveLoad;
using AvantGardeMaker.EnemySpace;
using System.Linq;
using CoreSources;

namespace AvantGardeMaker.CoreSpace
{
	public class GamePlayingManager : SerializedSingleton<GamePlayingManager>
	{
		#region 변수
		[SerializeField, ReadOnly]
		private StageData m_GameStageData = default;

		private int m_LifePoint = 3;
		private int m_GameSpeed = 1;

		#region 코스트 관련 변수
		private int m_CurrentCost = 10;

		private UtilClass.Timer m_CostTimer = null;
		#endregion

		#region 오퍼레이터 관련 변수
		[SerializeField, ReadOnly]
		private List<Operator> m_DeployingOperatorList = null;
		#endregion

		#region 적 관련 변수

		#endregion
		#endregion

		#region 프로퍼티
		public StageData gameStageData => m_GameStageData;

		public int currentCost
		{
			get => m_CurrentCost;
			set
			{
				m_CurrentCost = value;

				onCostChanged?.Invoke(m_CurrentCost);
			}
		}
		public int maxCost { get; private set; }

		public int lifePoint
		{
			get => m_LifePoint;
			set
			{
				m_LifePoint = value;

				if (m_LifePoint <= 0)
				{
					LoadPrevScene();
				}
			}
		}

		public UtilClass.Timer costTimer => m_CostTimer;

		public (E_TileType tileType, E_TilePositionType tilePositionType)[,] currentMap { get; private set; }
		public List<OperatorSpawnData> operatorSpawnDataList { get; private set; }
		#endregion

		#region 이벤트
		public event System.Action<int> onCostChanged = null;

		#region 이벤트 함수
		#endregion
		#endregion

		#region 매니저
		private static MapEditingManager M_MapEditing => MapEditingManager.Instance;
		private static GamePlayingSceneUIManager M_GamePlayingUI => GamePlayingSceneUIManager.Instance;

		private static TileManager M_Tile => TileManager.Instance;
		private static OperatorManager M_Operator => OperatorManager.Instance;
		private static EnemyManager M_Enemy => EnemyManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			CostIncreaseProcess();
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			m_CostTimer = new UtilClass.Timer();
			m_DeployingOperatorList = new List<Operator>();

			currentMap = null;
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

			if (m_CostTimer.interval <= 0f)
				return;

			m_CostTimer.Clear();
			m_CostTimer.Resume();
		}
		/// <summary>
		/// 메인 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			currentMap = null;
			operatorSpawnDataList.Clear();
			operatorSpawnDataList = null;

			base.FinallizeMain();
		}
		#endregion

		private void CostIncreaseProcess()
		{
			if (currentCost >= maxCost)
				return;

			if (m_CostTimer.Update() == true)
			{
				++currentCost;
			}
		}

		public void DeployOperator(Operator deployingOperator)
		{
			m_DeployingOperatorList.Add(deployingOperator);

			currentCost -= deployingOperator.variableData.DeploymentCost;
		}

		/// <summary>
		/// 도발, 배치 순서를 이용하여 오퍼레이터 리스트(보통 적의 공격 범위에 들어온)를 내림차순으로 정렬해주는 함수
		/// </summary>
		/// <returns>정렬된 오퍼레이터 리스트</returns>
		public List<Operator> SortOperatorListByAttackIndex(List<Operator> operatorList)
		{
			List<Operator> sortingOperatorList = new List<Operator>(operatorList);

			sortingOperatorList = sortingOperatorList
				.OrderByDescending(oper => oper.variableData.Provocation)
				.ThenByDescending(oper => oper.deploymentIndex)
				.ToList();

			return sortingOperatorList;
		}

		public void LoadData()
		{
			M_Tile.LoadTileData(m_GameStageData);
			M_Operator.LoadOperatorData(m_GameStageData);
			M_Enemy.LoadEnemyData(m_GameStageData);
		}
		public void SynchronizeStageData(in StageData stageData)
		{
			m_GameStageData = stageData;

			currentMap = stageData.map;
			operatorSpawnDataList = stageData.operatorSpawnDataList;

			currentCost = stageData.initCost;
			maxCost = stageData.maxCost;
			m_CostTimer.interval = stageData.costIncreaseTime;
			m_CostTimer.Pause();

			m_LifePoint = stageData.lifePoint;

			PathFinder.offset = -stageData.minTile;
		}
		/// <summary>
		/// 이전 씬으로 돌아가는 함수
		/// </summary>
		public void LoadPrevScene()
		{
			if (SceneLoader.prevSceneName.Equals("Map Editing Scene") == true)
				M_MapEditing.SynchronizeStageData(m_GameStageData);

			Debug.Log("이전 씬: " + SceneLoader.prevSceneName);

			SceneLoader.LoadScene(SceneLoader.prevSceneName);
		}
	}
}