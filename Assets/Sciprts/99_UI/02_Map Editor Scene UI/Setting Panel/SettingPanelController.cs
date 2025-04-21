using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class SettingPanelController : Panel
	{
		#region 기본 템플릿
		#region 변수
		private Dictionary<string, SettingPanel> m_SettingPanelMap = null;
		#endregion

		#region 프로퍼티
		public SettingPanel this[string key] => m_SettingPanelMap[key];
		public SettingPanel currentSettingPanel { get; set; }
		#endregion

		#region 이벤트

		#region 이벤트 함수
		//public void OnMenuButtonClicked()
		//{
		//	if (currentViewport != null)
		//		currentViewport.gameObject.SetActive(false);

		//	if (m_SettingPanelController.gameObject.activeSelf == false)
		//		ToggleOptionPanelActive();
		//	else if (currentViewport == this)
		//	{
		//		m_SettingPanelController.ChangeCurrentViewport(null);
		//		ToggleOptionPanelActive();
		//		return;
		//	}

		//	m_SettingPanelController.ChangeCurrentViewport(this);
		//	currentViewport.gameObject.SetActive(true);

		//	onViewportTurnOn?.Invoke(this);
		//}
		public void OnMenuButtonClicked()
		{
			currentSettingPanel?.gameObject.SetActive(false);
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
		public override void Initialize()
		{
			base.Initialize();

			m_SettingPanelMap = new Dictionary<string, SettingPanel>();
			Transform settingPanelParent = transform.Find("Setting Panels");
			int childCount = settingPanelParent.childCount;
			for (int i = 0; i < childCount; ++i)
			{
				SettingPanel settingPanel = settingPanelParent.GetChild<SettingPanel>(i);

				if (settingPanel == null)
					continue;

				string key = settingPanel.name.Split(' ')[0];

				m_SettingPanelMap.Add(key, settingPanel);

				settingPanel.Initialize();
			}
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();

			foreach (var item in m_SettingPanelMap)
			{
				item.Value.Finallize();
			}
			m_SettingPanelMap.Clear();
			m_SettingPanelMap = null;
		}
		#endregion
		#endregion
	}
}