using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.MikangMark
{
	public class UIManager : SerializedSingleton<UIManager>
	{
		#region 변수
		public TextMeshProUGUI m_Cost;
		public Image m_CostImage;

		float m_Timer = 0f;

		public GameObject m_OperStatUI;

		public Image m_OperImg;
		public Image m_JobImg;
		public Image m_Arousal;
		public TextMeshProUGUI m_Name;
		public TextMeshProUGUI m_OperLevelValue;
		public TextMeshProUGUI m_StatAttackValue;
		public TextMeshProUGUI m_StatDefence;
		public TextMeshProUGUI m_StatMagicDefence;
		public TextMeshProUGUI m_StatBlock;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수
		private void Awake()
		{
			OperStatUISetActive(false);
		}
		private void FixedUpdate()
		{
			m_Timer = InGamePlayManager.Instance.GetRealTime();
			m_CostImage.fillAmount = m_Timer;
			m_Cost.text = InGamePlayManager.Instance.GetCost().ToString();
		}
		#endregion

		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		/// 
		public void OperStatUISetActive(bool is_Active)
		{
			m_OperStatUI.SetActive(is_Active);
		}
		public void OperStatUISetting(OperInfo _operInfo)
		{

		}
		public virtual void Initialize()
		{
			
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public virtual void Finallize()
		{

		}

		/// <summary>
		/// 게임 초기화 함수 (Game Scene 진입 시 호출)
		/// </summary>
		public virtual void InitializeGame()
		{

		}
		/// <summary>
		/// 게임 마무리화 함수 (Game Scene 나갈 시 호출)
		/// </summary>
		public virtual void FinallizeGame()
		{

		}
	}
}