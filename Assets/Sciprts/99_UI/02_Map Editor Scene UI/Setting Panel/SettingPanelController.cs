using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class SettingPanelController : SerializedMonoBehaviour
	{
		#region 기본 템플릿
		#region 변수
		private Dictionary<string, SettingPanel> m_SettingPanelMap = null;
		private Dictionary<string, SettingPanel> m_DetailedSettingPanelMap = null;
		#endregion

		#region 프로퍼티
		public SettingPanel currentSettingPanel { get; set; }
		#endregion

		#region 이벤트

		#region 이벤트 함수
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
			m_SettingPanelMap = new Dictionary<string, SettingPanel>();
			Transform settingPanelParent = transform.Find("Setting Panels");
			int childCount = settingPanelParent.childCount;
			for (int i = 0; i < childCount; ++i)
			{
				SettingPanel settingPanel = settingPanelParent.GetChild<SettingPanel>(i);

				string key = settingPanel.name.Split(' ')[0];

				m_SettingPanelMap.Add(key, settingPanel);

				settingPanel.Initialize();
			}

			m_DetailedSettingPanelMap = new Dictionary<string, SettingPanel>();
			settingPanelParent = transform.Find("Detailed Setting Panels");
			childCount = settingPanelParent.childCount;
			for (int i = 0; i < childCount; ++i)
			{
				SettingPanel detailedSettingPanel = settingPanelParent.GetChild<SettingPanel>(i);

				string key = detailedSettingPanel.name.Split(' ')[0];

				m_DetailedSettingPanelMap.Add(key, detailedSettingPanel);

				detailedSettingPanel.Initialize();
			}
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{
			foreach (var item in m_SettingPanelMap)
			{
				item.Value.Finallize();
			}
			m_SettingPanelMap.Clear();
			m_SettingPanelMap = null;

			foreach (var item in m_DetailedSettingPanelMap)
			{
				item.Value.Finallize();
			}
			m_DetailedSettingPanelMap.Clear();
			m_DetailedSettingPanelMap = null;
		}
		#endregion
		#endregion

		public SettingPanel GetSettingPanel(string key)
		{
			if (m_SettingPanelMap.TryGetValue(key, out SettingPanel settingPanel) == false)
				return null;

			return settingPanel;
		}
		public SettingPanel GetDetailedSettingPanel(string key)
		{
			if (m_DetailedSettingPanelMap.TryGetValue(key, out SettingPanel settingPanel) == false)
				return null;

			return settingPanel;
		}

		public T GetSettingPanel<T>() where T : SettingPanel
		{
			string key = typeof(T).Name.Replace("SettingPanel", "");

			SettingPanel settingPanel = GetSettingPanel(key);

			return settingPanel as T;
		}
		public T GetDetailedSettingPanel<T>() where T : SettingPanel
		{
			string key = typeof(T).Name.Replace("SettingPanel", "");

			SettingPanel settingPanel = GetDetailedSettingPanel(key);

			return settingPanel as T;
		}
	}
}