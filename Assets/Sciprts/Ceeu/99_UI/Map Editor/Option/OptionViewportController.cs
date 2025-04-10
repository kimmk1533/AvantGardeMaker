using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class OptionViewportController : SerializedMonoBehaviour
	{
		#region 변수
		private OptionPanel m_OptionPanel = null;

		[SerializeField, ReadOnly]
		private Dictionary<string, OptionViewport> m_OptionViewportMap = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 인덱서
		public OptionViewport this[string key] => m_OptionViewportMap[key];
		#endregion

		#region 매니져
		private static MapEditingManager M_MapEditing => MapEditingManager.Instance;
		private static MapEditingUIManager M_MapEditingUI => MapEditingUIManager.Instance;
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

			m_OptionViewportMap = new Dictionary<string, OptionViewport>();

			foreach (string key in M_MapEditingUI.keyList)
			{
				string viewportName = key + " Option Viewport";

				OptionViewport optionViewport = transform.Find<OptionViewport>(viewportName);
				optionViewport.Initialize(m_OptionPanel);

				m_OptionViewportMap.Add(key, optionViewport);
			}
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{
			foreach (var item in m_OptionViewportMap)
			{
				item.Value.Finallize();
			}
			m_OptionViewportMap.Clear();
		}
		#endregion

		public IEnumerable<OptionViewport> GetOptionViewportEnumerator()
		{
			foreach (var item in m_OptionViewportMap)
			{
				yield return item.Value;
			}
		}
	}
}