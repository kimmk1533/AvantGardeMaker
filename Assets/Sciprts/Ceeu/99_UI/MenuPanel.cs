using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AvantGardeMaker.Ceeu
{
	public class MenuPanel : SerializedMonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		#region 변수
		private RectTransform m_RectTransform = null;

		private float m_MovingOffset = 0f;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		private static UIManager M_UI => UIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		#endregion

		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{
			m_RectTransform = GetComponent<RectTransform>();

			m_MovingOffset = m_RectTransform.sizeDelta.y * 0.5f + m_RectTransform.anchoredPosition.y;
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{
			m_RectTransform = null;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (M_UI.menuPanelLock == true)
				return;

			Vector3 position = m_RectTransform.anchoredPosition;
			position.y -= m_MovingOffset;
			m_RectTransform.anchoredPosition = position;
		}
		public void OnPointerExit(PointerEventData eventData)
		{
			if (M_UI.menuPanelLock == true)
				return;

			Vector3 position = m_RectTransform.anchoredPosition;
			position.y += m_MovingOffset;
			m_RectTransform.anchoredPosition = position;
		}
	}
}