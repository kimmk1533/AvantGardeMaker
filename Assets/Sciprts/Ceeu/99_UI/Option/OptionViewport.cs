using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.Ceeu.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.Ceeu
{
	public class OptionViewport : SerializedMonoBehaviour
	{
		#region 변수
		[SerializeField]
		private string m_ViewportName = string.Empty;
		[SerializeField]
		private KeyCode m_Shortcut = KeyCode.Alpha1;
		[SerializeField]
		private E_EditModeType m_EditModeType = E_EditModeType.System;

		private OptionPanel m_OptionPanel = null;

		private RectTransform m_RectTransform = null;
		[SerializeField]
		private RectTransform m_Content = null;
		#endregion

		#region 프로퍼티
		public string viewportName => m_ViewportName;
		public KeyCode shortcut => m_Shortcut;
		public E_EditModeType editModeType => m_EditModeType;
		public RectTransform rectTransform => m_RectTransform;
		public RectTransform content => m_Content;

		private OptionViewport currentViewport => m_OptionPanel.currentViewport;
		#endregion

		#region 이벤트
		public event System.Action<OptionViewport> onViewportTurnOn = null;

		#region 이벤트 함수
		public void OnMenuButtonClicked()
		{
			if (currentViewport != null)
				currentViewport.gameObject.SetActive(false);

			if (m_OptionPanel.gameObject.activeSelf == false)
				ToggleOptionPanelActive();
			else if (currentViewport == this)
			{
				m_OptionPanel.ChangeCurrentViewport(null);
				ToggleOptionPanelActive();
				return;
			}

			m_OptionPanel.ChangeCurrentViewport(this);
			currentViewport.gameObject.SetActive(true);

			onViewportTurnOn?.Invoke(this);
		}
		#endregion
		#endregion

		#region 매니저
		private static MapEditorUIManager M_MapEditorUI => MapEditorUIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize(OptionPanel optionPanel)
		{
			m_OptionPanel = optionPanel;

			m_RectTransform = GetComponent<RectTransform>();

			if (m_Content == null)
				m_Content = (RectTransform)m_RectTransform.Find(name.Replace("Viewport", "Content"));

			M_MapEditorUI.menuPanel.menuButtonController[m_ViewportName].onClick.AddListener(OnMenuButtonClicked);

			gameObject.SetActive(false);
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{

		}
		#endregion

		private void ToggleOptionPanelActive()
		{
			m_OptionPanel.gameObject.SetActive(!m_OptionPanel.gameObject.activeSelf);
		}
	}
}