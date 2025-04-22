using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class OperatorSettingSlotButton : SerializedMonoBehaviour
	{
		#region 기본 템플릿
		#region 변수
		private Button m_SlotButton = null;

		private Image m_PlusImage = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트

		#region 이벤트 함수
		private void OnSlotButtonClicked()
		{
			M_MapEditingUI.operatorDetailedSettingPanel.StartSetting(this);
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
		public void Initialize()
		{
			m_SlotButton = GetComponent<Button>();
			m_SlotButton.onClick.AddListener(OnSlotButtonClicked);

			m_PlusImage = transform.Find<Image>("Plus Image");
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