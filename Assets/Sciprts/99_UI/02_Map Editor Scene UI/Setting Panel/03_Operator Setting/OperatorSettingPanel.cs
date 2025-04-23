using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace.SaveLoad;
using AvantGardeMaker.OperatorSpace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.UI
{
	public class OperatorSettingPanel : SettingPanel
	{
		#region 기본 템플릿
		#region 변수
		private List<OperatorSettingSlot> m_OperatorSettingSlotList = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트

		#region 이벤트 함수
		#endregion
		#endregion

		#region 매니저
		private static MapEditingUIManager M_MapEditingUI => MapEditingUIManager.Instance;

		private static OperatorManager M_Operator => OperatorManager.Instance;
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

			m_OperatorSettingSlotList = new List<OperatorSettingSlot>();
			Transform slotParent = transform.Find("Operator Setting Slots");
			int childCount = slotParent.childCount;
			for (int i = 0; i < childCount; ++i)
			{
				OperatorSettingSlot settingSlot = slotParent.GetChild<OperatorSettingSlot>(i);

				m_OperatorSettingSlotList.Add(settingSlot);

				settingSlot.Initialize();
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

		public void SaveOperatorDataUI(ref StageData stageData)
		{
			for (int i = 0; i < m_OperatorSettingSlotList.Count; ++i)
			{
				OperatorSettingSlot settingSlot = m_OperatorSettingSlotList[i];

				if (settingSlot.operatorData == null)
					continue;

				stageData.SaveOperatorData(settingSlot.operatorData);
			}
		}
		public void LoadOperatorDataUI(StageData stageData)
		{
			List<string> operatorKeyList = stageData.operatorKeyList;

			int count = operatorKeyList.Count;

			if (count > m_OperatorSettingSlotList.Count)
				throw new System.Exception("편성한 오퍼레이터 데이터가 편성창 최대 갯수보다 많음");

			for (int i = 0; i < count; ++i)
			{
				m_OperatorSettingSlotList[i].operatorData = M_Operator.GetOperatorData(operatorKeyList[i]);
				M_MapEditingUI.RemoveOperatorDataUI(operatorKeyList[i]);
			}
		}
	}
}