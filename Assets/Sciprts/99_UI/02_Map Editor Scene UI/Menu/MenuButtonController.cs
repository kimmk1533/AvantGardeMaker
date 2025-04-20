using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.UI
{
	public class MenuButtonController : SerializedMonoBehaviour
	{
		#region 변수
		private MenuPanel m_MenuPanel = null;

		private Dictionary<string, MenuButton> m_ButtonMap = null;
		#endregion

		#region 프로퍼티

		#endregion

		#region 인덱서
		public MenuButton this[string key] => m_ButtonMap[key];
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
		public void Initialize(MenuPanel menuPanel)
		{
			m_MenuPanel = menuPanel;

			m_ButtonMap = new Dictionary<string, MenuButton>();

			foreach (string key in M_MapEditingUI.keyList)
			{
				string menuName = key + " Menu";

				MenuButton button = transform.Find<MenuButton>(menuName);
				button.Initialize();

				m_ButtonMap.Add(key, button);
			}
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{
			foreach (var item in m_ButtonMap)
			{
				item.Value.Finallize();
			}
			m_ButtonMap.Clear();
		}
		#endregion
	}
}