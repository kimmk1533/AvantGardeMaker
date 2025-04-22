using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.TileSpace;
using AvantGardeMaker.OperatorSpace;
using Sirenix.OdinInspector;
using UnityEngine;
using AvantGardeMaker.UI;
using AvantGardeMaker.TileSpace.Enum;

namespace AvantGardeMaker.CoreSpace
{
	public class GamePlayingManager : SerializedSingleton<GamePlayingManager>
	{
		#region 변수
		private int m_GameHp;
		private int m_GameSpeed;

		private int m_MaxCost = 99;
		private int m_CurrentCost = 0;

		private UtilClass.Timer m_CostTimer = null;

		private int m_MaxLocationCount;
		private int m_LocationCount;

		[SerializeField, ReadOnly]
		private List<Operator> m_PlayingOperatorList = null;

		private Tile m_DeployPreviewOperatorOnTile = new Tile();
		//선택된 오퍼레이터
		private Operator m_SettedOperatorSelect = null;
		//선택된 오퍼레이터가있는 타일
		private Tile m_SelectedTile = null;

		//게임들어오기전 편성한 오퍼레이터들의 이름 받기
		[SerializeField]
		private List<string> m_OperatorSquadKeyList = new List<string>();
		#endregion

		#region 프로퍼티
		public int maxCost
		{
			get => m_MaxCost;
		}
		public int currentCost
		{
			get => m_CurrentCost;
		}

		public UtilClass.Timer costTimer => m_CostTimer;

		public int maxLocationCount
		{
			get => m_MaxLocationCount;
		}
		public int locationCount
		{
			get => m_LocationCount;
		}

		public List<string> operatorSquadKeyList => new List<string>(m_OperatorSquadKeyList);

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

		public E_TileType[,] currentMap
		{
			get;
			private set;
		}

		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		private static TileManager M_Tile => TileManager.Instance;
		private static GamePlayingUIManager M_GamePlayingUI => GamePlayingUIManager.Instance;
		private static GameManager M_Game => GameManager.Instance;
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

			//임시 초기 코스트
			m_CurrentCost = 10;

			m_CostTimer = new UtilClass.Timer(1f);
			m_PlayingOperatorList = new List<Operator>();
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

			currentMap = M_Game.currentStageData.map;
		}
		/// <summary>
		/// 메인 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();


		}
		#endregion

		private void CostIncreaseProcess()
		{
			m_CostTimer.Update();
			if (m_CostTimer.TimeCheck(true) == true)
			{
				++m_CurrentCost;
			}
		}

		private void ClickTileProcess()
		{
			if (Input.GetMouseButtonDown(0)) // 좌클릭
			{
				m_SettedOperatorSelect = CheckTileInOperator();

				if (m_SettedOperatorSelect == null)
					return;

				M_GamePlayingUI.activeRetreatButton = !M_GamePlayingUI.activeRetreatButton;

				//오퍼레이터 스탯창열기
				//M_GamePlayingUI.m_OperStatUIParent.SetActive(true);
				//퇴각버튼 활성화하기
				M_GamePlayingUI.OperatorRetreateButtonSetPosition();
				M_GamePlayingUI.SettingOperatorRetreateButton(m_SelectedTile);
			}
		}
		public void DeploymentOperatorOnTile(Operator operatorPreview)
		{
			m_DeployPreviewOperatorOnTile.tileOnOperator = operatorPreview;
			operatorPreview.deploymentTile = m_DeployPreviewOperatorOnTile;
		}

		private Operator CheckTileInOperator()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				GameObject target = hit.collider.gameObject;
				Tile tile = target.GetComponent<Tile>();
				if (tile == null)
					return null;
				if (tile.tileOnOperator == null)
					return null;
				m_SelectedTile = tile;
				return tile.tileOnOperator;
			}
			return null;

		}
		/// <summary>
		/// 요청한 오퍼레이터리스트를 오퍼레이터 배치순서의 역순으로 리턴
		/// </summary>
		/// <returns></returns>
		public List<Operator> CompareDeployOrder(List<Operator> orderOperatorList)
		{
			List<Operator> sortingOperatorList = new List<Operator>();
			List<int> temp = new List<int>();
			for (int i = 0; i < orderOperatorList.Count; i++)
			{
				int temp2 = m_PlayingOperatorList.FindIndex(n => n.operatorData.EngName == orderOperatorList[i].operatorData.EngName);
				temp.Add(temp2);//231
			}
			//temp에는 오더에서준 m_PlayingOperatorList의 인덱스가 무작위순서로 저장되어있다
			//내림차순정렬

			temp.Sort((a, b) => b.CompareTo(a));
			for (int i = 0; i < orderOperatorList.Count; i++)
			{
				sortingOperatorList.Add(m_PlayingOperatorList[temp[i]]);
			}

			return sortingOperatorList;
		}

		public void DeployOperator(Operator deployOperator)
		{
			m_PlayingOperatorList.Add(deployOperator);
		}

		public void GaintCost(int gainCost)
		{
			m_CurrentCost += gainCost;
		}
	}
}
