using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.Ceeu;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.MikangMark
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

		private List<Tile> m_PlayGroundTileList = new List<Tile>();
		[SerializeField]
		private Transform m_PlayGroundTileParent;

		private Tile m_SetPreviewOperatorOnTile = new Tile();

		private Operator m_SettedOperatorSelect = null;

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
			get => m_SetPreviewOperatorOnTile;
			set => m_SetPreviewOperatorOnTile = value;
		}

		public Operator settedOperatorSelect
		{
			get => m_SettedOperatorSelect;
			set => m_SettedOperatorSelect = value;
		}
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		private static GamePlayingUIManager M_GamePlayingUI => GamePlayingUIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Start()
		{
			Initialize();
		}
		private void Update()
		{
			CostIncreaseProcess();
			if (Input.GetMouseButtonDown(0)) // 좌클릭
			{
				m_SettedOperatorSelect = CheckTileInOperator();
				if (m_SettedOperatorSelect == null)
					return;
				if (M_GamePlayingUI.activeRetreateButton == true)
					M_GamePlayingUI.activeRetreateButton = false;
				else
					M_GamePlayingUI.activeRetreateButton = true;
				//오퍼레이터 스탯창열기
				//M_GamePlayingUI.m_OperStatUIParent.SetActive(true);
				//퇴각버튼 활성화하기
				M_GamePlayingUI.OperatorRetreateButtonActive(M_GamePlayingUI.activeRetreateButton);
			}
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public virtual void Initialize()
		{
			//임시 초기 코스트
			m_CurrentCost = 10;

			m_CostTimer = new UtilClass.Timer(1f);

			for (int i = 0; i < m_PlayGroundTileParent.childCount; i++)
			{
				m_PlayGroundTileList.Add(m_PlayGroundTileParent.GetChild(i).GetComponent<Tile>());
			}
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public virtual void Finallize()
		{

		}

		/// <summary>
		/// 게임 초기화 함수 (본인 Main Scene 진입 시 호출)
		/// </summary>
		public virtual void InitializeMain()
		{

		}
		/// <summary>
		/// 게임 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public virtual void FinallizeMain()
		{

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


		public void SettingOperatorOnTile(Operator operatorPreview)
		{
			m_SetPreviewOperatorOnTile.tileOnOperator = operatorPreview;
		}

		public Operator CheckTileInOperator()
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
				return tile.tileOnOperator;
				//tile.RetreatOperatorOnTile();
			}
			return null;

		}
	}
}
