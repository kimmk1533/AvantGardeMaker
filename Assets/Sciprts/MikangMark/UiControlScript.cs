using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace AvantGardeMaker.MikangMark
{
	public class UiControlScript : SerializedMonoBehaviour
	{
		#region 변수
		public TextMeshProUGUI m_Cost;
		public Image m_CostImage;

		float m_Timer = 0f;
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
		}
		private void FixedUpdate()
		{
			m_Timer = InGamePlayManager.Instance.GetRealTime();
			m_CostImage.fillAmount = m_Timer;
			m_Cost.text = InGamePlayManager.Instance.GetCost().ToString();
		}
		#endregion

		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{
			
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{

		}
	}
}