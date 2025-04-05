using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace AvantGardeMaker.MikangMark
{
	public class InGamePlayManager : SerializedSingleton<InGamePlayManager>
	{
		#region 변수
		int m_GameHp;
		int m_GameSpeed;

		float m_time = 0;

		int m_MaxCost = 99;
		int m_Cost = 0;
		int m_SetAbleCount;

		int m_StartCost;
		float m_ClearSecond = 0.0f;

		float m_RealTime = 0.0f;
		//게임들어오기전 편성한 캐릭터들의 이름 받기
		public List<string> m_ReceivePlayOperator;

		[SerializeField]
		GameObject m_OperPannel;
		[SerializeField]
		Transform m_OperBox;

		public List<Operator> m_PlayingOpers;

		
		public List<Operator> m_ReadyOperator;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수
		private void Start()
		{
			Initialize();
			Debug.Log("InGamePlayManager");
		}
		private void FixedUpdate()
		{
			m_RealTime += Time.deltaTime;
			m_ClearSecond = m_RealTime;
			m_ClearSecond = m_ClearSecond - ((int)m_ClearSecond);
			if (m_RealTime >= 1.0f)
			{
				m_Cost++;
				m_RealTime = 0.0f;
			}

		}
		#endregion

		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public virtual void Initialize()
		{
			m_Cost = 5;
			SetStartCost(5);
			m_Cost = m_StartCost;
			m_PlayingOpers = new List<Operator>();
			m_ReadyOperator = new List<Operator>();
			m_OperBox = GameObject.Find("OperBox").transform;
			CreateOperBox(m_ReceivePlayOperator.Count);
			for (int i = 0; i < m_ReadyOperator.Count; i++)
			{
				m_ReadyOperator[i].SetData();
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

		public void SetStartCost(int _cost)
		{
			m_StartCost = _cost;
		}
		public int GetCost()
		{
			return m_Cost;
		}
		public int GetMaxCost()
		{
			return m_MaxCost;
		}
		public float GetRealTime()
		{
			return m_ClearSecond;
		}
		public void CreateOperBox(int _OpCount)
		{
			for (int i = 0; i < _OpCount; i++) 
			{
				
				m_ReadyOperator.Add(Instantiate(m_OperPannel, m_OperBox).GetComponent<Operator>());
				m_ReadyOperator[i].OperName = m_ReceivePlayOperator[i];
				m_ReadyOperator[i].gameObject.name = m_ReadyOperator[i].GetComponent<Operator>().OperName + "_InBox";
				m_ReadyOperator[i].m_OperData = OperatorJsonManager.Instance.m_OperInfoList[i];
			}
		}

		public Operator GetOperInfo(int _index)
		{
			return m_ReadyOperator[_index];
		}
	}
}
