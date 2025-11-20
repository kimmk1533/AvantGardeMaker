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
		private bool m_IsLock = false;
		private Button m_MenuLockButton = null;
		private Image m_MenuLockButtonImage = null;

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

		public bool isLock
		{
			get => m_IsLock;
			set
			{
				m_IsLock = value;

				m_MenuLockButtonImage.sprite = (m_IsLock == true) ? m_LockSprite : m_UnlockSprite;
			}
		}
		#endregion

		#region 이벤트

		#region 이벤트 함수
		#region 메뉴 버튼 관련 이벤트 함수
		private void OnMenuButtonClicked(string key)
		{
			SettingPanelController settingPanelController = M_MapEditingUI.settingPanelController;

			SettingPanel currentSettingPanel = settingPanelController.currentSettingPanel;
			SettingPanel clickedSettingPanel = settingPanelController.GetSettingPanel(key);

			if (currentSettingPanel != null)
				currentSettingPanel.gameObject.SetActive(false);

			clickedSettingPanel.OnMenuButtonClicked();
		}
		#endregion

		#region 잠금 설정 관련 이벤트 함수
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (m_IsLock == true)
				return;

			rectTransform.anchoredPosition = m_EnterPosition;
		}
		public void OnPointerExit(PointerEventData eventData)
		{
			if (m_IsLock == true)
				return;

			rectTransform.anchoredPosition = m_ExitPosition;
		}
		#endregion
		#endregion
		#endregion

		#region 매니저
		private static MapEditingSceneUIManager M_MapEditingUI => MapEditingSceneUIManager.Instance;
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
			m_MenuLockButton = rectTransform.Find<Button>("Lock Button");
			m_MenuLockButton.onClick.AddListener(() => isLock = !isLock);

			m_MenuLockButtonImage = m_MenuLockButton.transform.Find<Image>("Lock Button Image");

			m_EnterPosition = rectTransform.anchoredPosition;
			m_ExitPosition = rectTransform.anchoredPosition;
			m_ExitPosition.y *= -1f;

			isLock = true;
			rectTransform.anchoredPosition = m_IsLock ? m_EnterPosition : m_ExitPosition;
			#endregion
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{
			foreach (var item in m_MenuButtonMap)
			{
				item.Value.onClick.RemoveAllListeners();
			}
			m_MenuButtonMap.Clear();
			m_MenuButtonMap = null;
		}
		#endregion
		#endregion

	}
}