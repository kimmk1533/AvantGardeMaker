using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class MenuButton : SerializedMonoBehaviour
	{
		#region 변수
		private Button m_Button = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		public Button.ButtonClickedEvent onClick => m_Button.onClick;
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
			m_Button = GetComponent<Button>();
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{
			m_Button.onClick.RemoveAllListeners();
		}
		#endregion
	}
}