using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.UI
{
	public class OperatorSettingPanel : SettingPanel
	{
		#region 기본 템플릿
		#region 변수
		private List<OperatorSettingSlotButton> m_SlotButtonList = null;
		#endregion

		#region 프로퍼티
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
		public override void Initialize()
		{
			base.Initialize();

			m_SlotButtonList = new List<OperatorSettingSlotButton>();
			Transform slotParent = transform.Find("Operator Setting Slots");
			int childCount = slotParent.childCount;
			for (int i = 0; i < childCount; ++i)
			{
				OperatorSettingSlotButton slotButton = slotParent.GetChild<OperatorSettingSlotButton>(i);

				m_SlotButtonList.Add(slotButton);

				slotButton.Initialize();
			}
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