using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.MikangMark
{
	public class GamePlayingManager : SerializedSingleton<GamePlayingManager>
	{
		#region 변수
		private int m_GameHp;
		private int m_GameSpeed;

		private int m_InitCost;
		private int m_CurrentCost = 0;
		private int m_MaxCost = 99;
		
		private int m_PlacementCount;
		
		private UtilClass.Timer m_CostTimer = null;

		//게임들어오기전 편성한 오퍼레이터들의 이름 받기
		[SerializeField]
		private List<string> m_OperatorSquadKeyList = new List<string>();

		[SerializeField]
		private GameObject m_OperPannel = null;
		[SerializeField]
		private Transform m_OperatorParent = null;

		//[SerializeField, ReadOnly]
		//private List<Operator> m_PlayingOperatorList = null;
		[SerializeField, ReadOnly]
		private List<Operator> m_OperatorSquadList = null;
		#endregion

		#region 프로퍼티
		public int initCost 
		{
			get => m_InitCost;
			set => m_InitCost = value;
		}
		public int currentCost
		{
			get => m_CurrentCost;
		}
		public int maxCost
		{
			get => m_MaxCost;
		}

		public float costIncreaseTime
		{
			get => m_CostTimer.time;
		}
		public float costIncreaseInterval
		{
			get =>m_CostTimer.interval;
			set => m_CostTimer.interval = value;
		}
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		private static OperatorJsonManager M_OperatorJson => OperatorJsonManager.Instance;
		private static OperatorManager M_Operator => OperatorManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Start()
		{
			Initialize();
		}
		private void Update()
		{
			/*
			m_CostTimer += Time.deltaTime;
			//m_ClearSecond = m_RealTime - ((int)m_RealTime);
			if (m_CostTimer >= 1.0f)
			{
				++m_CurrentCost;
				m_CostTimer = 0.0f;
			}
			*/

			CostIncreaseProcess();
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public virtual void Initialize()
		{
			//임시
			m_InitCost = 5;

			m_CurrentCost = m_InitCost;

			//m_PlayingOperatorList = new List<Operator>();

			if(m_OperatorParent == null)
				m_OperatorParent = GameObject.Find("OperBox").transform;

			m_OperatorSquadList = new List<Operator>();
			CreateOperatorUI(m_OperatorSquadKeyList.Count);
			
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
		
		private void CreateOperatorUI(int _OpCount)
		{
			for (int i = 0; i < _OpCount; i++)
			{
				string operatorKey = m_OperatorSquadKeyList[i];
				Operator newOperator = M_Operator.GetBuilder(operatorKey)
					.SetParent(m_OperatorParent)
					.SetAutoInit(false)
					.SetActive(true)
					.SetName(operatorKey+"_InSquad")
					.Spawn();
				m_OperatorSquadList.Add(Instantiate(m_OperPannel, m_OperatorParent).GetComponent<Operator>());
				m_OperatorSquadList[i].OperName = m_OperatorSquadKeyList[i];
				m_OperatorSquadList[i].gameObject.name = m_OperatorSquadKeyList[i] + "_InBox";
				m_OperatorSquadList[i].OperData = M_OperatorJson.m_OperInfoList[i].Clone();
			}
			for (int i = 0; i < m_OperatorSquadList.Count; i++)
			{
				m_OperatorSquadList[i].SetData();
			}
		}

		public Operator GetOperInfo(int _index)
		{
			return m_OperatorSquadList[_index];
		}
	}
}
