using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.Ceeu.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class UIManager : SerializedSingleton<UIManager>
	{
		#region 변수
		[SerializeField]
		private List<string> m_KeyList = new List<string>();

		#region 메뉴 패널 관련 변수
		[SerializeField]
		private MenuPanel m_MenuPanel = null;
		private bool m_MenuPanelMovingLock = false;
		private bool m_CanUseMenuShortcut = true;
		#endregion

		#region 옵션 패널 관련 변수
		[SerializeField]
		private OptionPanel m_OptionPanel = null;
		#endregion
		#endregion

		#region 프로퍼티
		public List<string> keyList => m_KeyList;

		public bool menuPanelLock
		{
			get => m_MenuPanelMovingLock;
			set => m_MenuPanelMovingLock = value;
		}
		public MenuButtonController menuButtonController => m_MenuPanel.menuButtonController;
		#endregion

		#region 이벤트

		#region 이벤트 함수
		public void OnMenuLockButtonClicked()
		{
			m_MenuPanelMovingLock = !m_MenuPanelMovingLock;
		}
		public void OnMapNameInputFieldFocused()
		{
			m_CanUseMenuShortcut = false;
		}
		public void OnMapNameInputFieldUnfocused()
		{
			m_CanUseMenuShortcut = true;
		}
		#endregion
		#endregion

		#region 매니저
		private static EditModeManager M_EditMode => EditModeManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			MenuShortcut();
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public virtual void Initialize()
		{
			if (m_MenuPanel == null)
				throw new System.NullReferenceException("m_MenuPanel is null.");
			if (m_OptionPanel == null)
				throw new System.NullReferenceException("m_OptionPanel is null.");
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
			m_MenuPanel.Initialize();
			m_MenuPanel.OnPointerEnter(null);
			m_MenuPanelMovingLock = true;

			m_OptionPanel.Initialize();
		}
		/// <summary>
		/// 게임 마무리화 함수 (Game Scene 나갈 시 호출)
		/// </summary>
		public virtual void FinallizeGame()
		{
			m_MenuPanel.Finallize();
		}
		#endregion

		private void MenuShortcut()
		{
			if (m_CanUseMenuShortcut == false)
				return;

			foreach (string key in m_KeyList)
			{
				OptionViewport optionViewport = m_OptionPanel.optionViewportController[key];
				KeyCode keyCode = optionViewport.shortcut;
				if (Input.GetKeyDown(keyCode) == true)
				{
					optionViewport.OnMenuButtonClicked();
					M_EditMode.SetEditModeType(optionViewport.editModeType);
				}
			}
		}
	}
}