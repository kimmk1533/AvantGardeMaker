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

namespace AvantGardeMaker.CoreSpace
{
	public class GamePlayingManager : SerializedSingleton<GamePlayingManager>
	{
		#region 변수
		[SerializeField, ReadOnly]
		private StageData m_GameStageData = default;

		private int m_GameHp;
		private int m_GameSpeed;

		#region 코스트 관련 변수
		private UtilClass.Timer m_CostTimer = null;
		#endregion

		#region 오퍼레이터 관련 변수
		[SerializeField, ReadOnly]
		private List<Operator> m_DeployingOperatorList = null;

		private Tile m_DeployPreviewOperatorOnTile = null;
		//선택된 오퍼레이터
		private Operator m_SettedOperatorSelect = null;
		//선택된 오퍼레이터가있는 타일
		private Tile m_SelectedTile = null;
		#endregion

		#region 적 관련 변수

		#endregion
		#endregion

		#region 프로퍼티
		public StageData currentStageData => m_GameStageData;

		public int currentCost { get; set; }
		public int maxCost { get; private set; }

		public UtilClass.Timer costTimer => m_CostTimer;

		public Tile setPreViewOperatorOnTile
		{
			get => m_DeployPreviewOperatorOnTile;
			set => m_DeployPreviewOperatorOnTile = value;
		}

		public Operator settedOperatorSelect
		{
			get => m_SettedOperatorSelect;
			set => m_SettedOperatorSelect = value;
		}

		public E_TileType[,] currentMap { get; private set; }
		public List<string> operatorSquadKeyList { get; private set; }
		#endregion

		#region 이벤트
		public event System.Action onCostIncreased = null;
		#endregion

		#region 매니저
		private static GamePlayingUIManager M_GamePlayingUI => GamePlayingUIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			CostIncreaseProcess();
			ClickTileProcess();
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			m_CostTimer = new UtilClass.Timer(1f);
			m_DeployingOperatorList = new List<Operator>();

			currentMap = null;
			operatorSquadKeyList = new List<string>();
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

			m_CostTimer.Clear();
			m_CostTimer.Resume();
		}
		/// <summary>
		/// 메인 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();

			currentMap = default;
			operatorSquadKeyList.Clear();
			operatorSquadKeyList = null;
		}
		#endregion

		private void CostIncreaseProcess()
		{
			if (currentCost >= maxCost)
				return;

			m_CostTimer.Update();

			if (m_CostTimer.TimeCheck(true) == true)
			{
				++currentCost;

				onCostIncreased?.Invoke();
			}
		}

		public void DeployOperator(Operator deployingOperator)
		{
			m_DeployingOperatorList.Add(deployingOperator);
		}
		public void DeploymentOperatorOnTile(Operator operatorPreview)
		{
			m_DeployPreviewOperatorOnTile.operatorOnTile = operatorPreview;

			operatorPreview.deploymentTile = m_DeployPreviewOperatorOnTile;
		}

		private void ClickTileProcess()
		{
			if (Input.GetMouseButtonDown(0)) // 좌클릭
			{
				m_SettedOperatorSelect = GetOperatorOnTile();

				if (m_SettedOperatorSelect == null)
					return;

				M_GamePlayingUI.activeRetreatButton = !M_GamePlayingUI.activeRetreatButton;
				//작업
				M_GamePlayingUI.activeSkillButton = !M_GamePlayingUI.activeSkillButton;

				//오퍼레이터 스탯창열기
				//M_GamePlayingUI.m_OperStatUIParent.SetActive(true);
				//퇴각버튼 활성화하기
				M_GamePlayingUI.OperatorRetreateButtonSetPosition();
				M_GamePlayingUI.SettingOperatorRetreateButton(m_SelectedTile);

				//스킬 버튼 활성화
				M_GamePlayingUI.ActiveSkillButton();
			}
		}
		private Operator GetOperatorOnTile()
		{
			Vector2 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero);
			if (hit.collider != null)
			{
				GameObject target = hit.collider.gameObject;
				Tile tile = target.GetComponent<Tile>();

				if (tile == null)
					return null;

				if (tile.operatorOnTile == null)
					return null;

				m_SelectedTile = tile;

				return tile.operatorOnTile;
			}

			return null;
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

		public void SynchronizeStageData(StageData stageData)
		{
			m_GameStageData = stageData;

			currentMap = stageData.map;
			operatorSquadKeyList = stageData.operatorKeyList;

			currentCost = stageData.initCost;
			maxCost = 99;//stageData.maxCost;
			m_CostTimer.interval = 1f;//stageData.costIncreaseTime;
			m_CostTimer.Pause();

			PathFinder.offset = -stageData.minTile;
		}
	}
}