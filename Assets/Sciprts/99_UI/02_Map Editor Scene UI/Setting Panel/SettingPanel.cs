using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class SettingPanel : Panel
	{
		#region 기본 템플릿
		#region 변수
		private Button m_CloseButton = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트

		#region 이벤트 함수
		public void OnMenuButtonClicked()
		{
			SettingPanel settingPanel = M_MapEditingUI.settingPanelController.currentSettingPanel;

			if (settingPanel == this &&
				settingPanel.gameObject.activeSelf == false)
			{
				M_MapEditingUI.settingPanelController.currentSettingPanel = null;
				return;
			}

			gameObject.SetActive(!gameObject.activeSelf);

			M_MapEditingUI.settingPanelController.currentSettingPanel = gameObject.activeSelf ? this : null;
		}
		private void OnCloseButtonClicked()
		{
			gameObject.SetActive(false);

			//if (m_CurrentViewport != null)
			//	m_CurrentViewport.gameObject.SetActive(false);
			//m_CurrentViewport = null;
		}
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
		public override void Initialize()
		{
			base.Initialize();

			if (m_CloseButton == null)
			{
				m_CloseButton = transform.Find<Button>("Close Button");
				m_CloseButton.onClick.AddListener(OnCloseButtonClicked);
			}

			gameObject.SetActive(false);
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();


		}
		#endregion
		#endregion
	}
}