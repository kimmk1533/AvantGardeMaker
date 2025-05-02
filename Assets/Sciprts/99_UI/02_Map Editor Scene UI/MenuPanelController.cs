using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class MenuPanelController : SerializedMonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		#region 기본 템플릿
		#region 변수
		#region 메뉴 버튼 관련 변수
		private Dictionary<string, Button> m_MenuButtonMap = null;
		#endregion

		#region 잠금 설정 관련 변수
		private bool m_IsMovable = false;
		private Button m_MenuMovingLockButton = null;
		private Image m_MenuMovingLockButtonImage = null;

		private Vector3 m_EnterPosition = default;
		private Vector3 m_ExitPosition = default;

		[SerializeField]
		[FoldoutGroup("잠금 설정 이미지")]
		private Sprite m_LockSprite = null;
		[SerializeField]
		[FoldoutGroup("잠금 설정 이미지")]
		private Sprite m_UnlockSprite = null;
		#endregion
		#endregion

		#region 프로퍼티
		public RectTransform rectTransform => transform as RectTransform;
		#endregion

		#region 이벤트

		#region 이벤트 함수
		#region 메뉴 버튼 관련 이벤트 함수
		private void OnMenuButtonClicked(string key)
		{
			SettingPanelController settingPanelController = M_MapEditingUI.settingPanelController;
			SettingPanel prevSettingPanel = settingPanelController.currentSettingPanel;

			if (prevSettingPanel != null)
				prevSettingPanel.gameObject.SetActive(false);

			SettingPanel currSettingPanel = settingPanelController.GetSettingPanel(key);

			currSettingPanel.OnMenuButtonClicked();
		}
		#endregion

		#region 잠금 설정 관련 이벤트 함수
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (m_IsMovable == false)
				return;

			rectTransform.anchoredPosition = m_EnterPosition;
		}
		public void OnPointerExit(PointerEventData eventData)
		{
			if (m_IsMovable == false)
				return;

			rectTransform.anchoredPosition = m_ExitPosition;
		}

		private void OnMenuLockButtonClicked()
		{
			m_IsMovable = !m_IsMovable;

			if (m_IsMovable)
				m_MenuMovingLockButtonImage.sprite = m_UnlockSprite;
			else
				m_MenuMovingLockButtonImage.sprite = m_LockSprite;
		}
		#endregion
		#endregion
		#endregion

		#region 매니저
		private static MapEditingUIManager M_MapEditingUI => MapEditingUIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{
			#region 메뉴 버튼 관련 초기화
			m_MenuButtonMap = new Dictionary<string, Button>();
			Transform buttonParent = transform.Find("Menu Buttons");
			int childCount = buttonParent.childCount;
			for (int i = 0; i < childCount; ++i)
			{
				Button menuButton = buttonParent.GetChild<Button>(i);

				if (menuButton == null)
					continue;

				string key = menuButton.name.Split(' ')[0];

				m_MenuButtonMap.Add(key, menuButton);

				menuButton.onClick.AddListener(() => OnMenuButtonClicked(key));
			}
			#endregion

			#region 잠금 설정 관련 초기화
			m_IsMovable = false;

			m_MenuMovingLockButton = rectTransform.Find<Button>("Lock Button");
			m_MenuMovingLockButton.onClick.AddListener(OnMenuLockButtonClicked);

			m_MenuMovingLockButtonImage = m_MenuMovingLockButton.transform.Find<Image>("Lock Button Image");
			m_MenuMovingLockButtonImage.sprite = m_LockSprite;

			m_EnterPosition = rectTransform.anchoredPosition;
			m_ExitPosition = rectTransform.anchoredPosition;
			m_ExitPosition.y *= -1f;
			#endregion
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{

		}
		#endregion
		#endregion

	}
}