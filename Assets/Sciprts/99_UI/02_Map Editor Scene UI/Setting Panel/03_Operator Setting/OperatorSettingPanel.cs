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
		private int m_UsedSlotCount = 0;
		private List<OperatorSettingSlot> m_OperatorSettingSlotList = null;
		#endregion

		#region 프로퍼티
		public int usedSlotCount
		{
			get => m_UsedSlotCount;
			set
			{
				m_UsedSlotCount = value;

				// 재정렬 인덱스 찾기
				int firstSlotIndex = -1;
				int lastSlotIndex = -1;
				for (int i = 0; i < m_OperatorSettingSlotList.Count; ++i)
				{
					OperatorSettingSlot settingSlot = m_OperatorSettingSlotList[i];

					if (settingSlot.operatorData == null)
					{
						if (firstSlotIndex == -1)
							firstSlotIndex = i;
						else
						{
							lastSlotIndex = i;
							break;
						}
					}
				}

				// 두 번 연속으로 opeartorData가 null인 경우 재정렬 필요X
				if (firstSlotIndex + 1 == lastSlotIndex)
					return;

				// 재정렬
				for (int i = firstSlotIndex; i < lastSlotIndex; ++i)
				{
					OperatorSettingSlot currSettingSlot = m_OperatorSettingSlotList[i];
					OperatorSettingSlot nextSettingSlot = m_OperatorSettingSlotList[i + 1];

					currSettingSlot.operatorData = nextSettingSlot.operatorData;
					nextSettingSlot.operatorData = null;
				}
			}
		}
		public OperatorSettingSlot currentSettingSlot => m_OperatorSettingSlotList[usedSlotCount];
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

		public void SaveOperatorData(ref StageData stageData)
		{
			for (int i = 0; i < m_OperatorSettingSlotList.Count; ++i)
			{
				OperatorSettingSlot settingSlot = m_OperatorSettingSlotList[i];

				if (settingSlot.operatorData == null)
					break;

				stageData.SaveOperatorData(settingSlot.operatorData);
			}
		}
		public void SaveOperatorSpawnData(ref StageData stageData)
		{
			for (int i = 0; i < m_OperatorSettingSlotList.Count; ++i)
			{
				OperatorSettingSlot settingSlot = m_OperatorSettingSlotList[i];

				if (settingSlot.operatorData == null)
					break;

				OperatorSpawnData operatorSpawnData = new OperatorSpawnData();

				operatorSpawnData.OperatorSpawnKey = settingSlot.operatorData.key;

				stageData.SaveOperatorSpawnData(operatorSpawnData);
			}
		}
		public void LoadOperatorData(in StageData stageData)
		{
			List<OperatorSpawnData> operatorSpawnDataList = stageData.operatorSpawnDataList;

			int count = operatorSpawnDataList.Count;

			if (count > m_OperatorSettingSlotList.Count)
				throw new System.Exception("편성한 오퍼레이터 데이터가 편성창 최대 갯수보다 많음");

			usedSlotCount = count;

			for (int i = 0; i < count; ++i)
			{
				m_OperatorSettingSlotList[i].operatorData = M_Operator.GetOperatorData(operatorSpawnDataList[i].OperatorSpawnKey);
				M_MapEditingUI.RemoveOperatorDataUI(operatorSpawnDataList[i].OperatorSpawnKey);
			}
		}
	}
}