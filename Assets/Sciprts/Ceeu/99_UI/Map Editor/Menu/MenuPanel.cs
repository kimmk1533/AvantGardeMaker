using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class MenuPanel : SerializedMonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		#region 변수
		private RectTransform m_RectTransform = null;

		private float m_MovingOffset = 0f;

		private MenuButtonController m_MenuButtonController = null;

		private Button m_MenuLockButton = null;
		private bool m_MenuPanelMovingLock = false;
		#endregion

		#region 프로퍼티
		public MenuButtonController menuButtonController => m_MenuButtonController;
		#endregion

		#region 이벤트
		#region 이벤트 함수
		public void OnMenuLockButtonClicked()
		{
			m_MenuPanelMovingLock = !m_MenuPanelMovingLock;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (m_MenuPanelMovingLock == true)
				return;

			Vector3 position = m_RectTransform.anchoredPosition;
			position.y -= m_MovingOffset;
			m_RectTransform.anchoredPosition = position;
		}
		public void OnPointerExit(PointerEventData eventData)
		{
			if (m_MenuPanelMovingLock == true)
				return;

			Vector3 position = m_RectTransform.anchoredPosition;
			position.y += m_MovingOffset;
			m_RectTransform.anchoredPosition = position;
		}
		#endregion
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{
			m_RectTransform = GetComponent<RectTransform>();

			m_MovingOffset = m_RectTransform.sizeDelta.y * 0.5f + m_RectTransform.anchoredPosition.y;

			m_MenuButtonController = m_RectTransform.FindInChildren<MenuButtonController>("Menu Content");
			m_MenuButtonController.Initialize(this);

			m_MenuLockButton = m_RectTransform.Find<Button>("Lock Button");
			m_MenuLockButton.onClick.AddListener(OnMenuLockButtonClicked);
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{
			m_MenuLockButton.onClick.RemoveAllListeners();
			m_MenuButtonController.Finallize();
		}
		#endregion

	}
}