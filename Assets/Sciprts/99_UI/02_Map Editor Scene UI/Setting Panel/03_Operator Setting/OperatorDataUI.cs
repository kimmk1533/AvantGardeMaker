using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class OperatorDataUI : MapEditingSceneUIPoolItem
	{
		#region 기본 템플릿
		#region 변수
		private OperatorData m_OperatorData = null;

		private Button m_Button = null;
		private TextMeshProUGUI m_OperatorKorNameText = null;
		#endregion

		#region 프로퍼티
		public OperatorData operatorData
		{
			get => m_OperatorData;
			set
			{
				m_OperatorData = value;

				m_OperatorKorNameText.text = value.KorName;
			}
		}
		#endregion

		#region 이벤트

		#region 이벤트 함수
		#endregion
		#endregion

		#region 매니저
		private static MapEditingSceneUIManager M_MapEditingUI => MapEditingSceneUIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public override void InitializePoolItem()
		{
			base.InitializePoolItem();

			if (m_Button == null)
				m_Button = GetComponent<Button>();
			if (m_OperatorKorNameText == null)
				m_OperatorKorNameText = transform.Find<TextMeshProUGUI>("Name Text");

			OperatorDetailedSettingPanel settingPanel = M_MapEditingUI.settingPanelController.GetDetailedSettingPanel<OperatorDetailedSettingPanel>();

			m_Button.onClick.AddListener(() => settingPanel.OnOperatorDataUIClicked(this));
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void FinallizePoolItem()
		{
			base.FinallizePoolItem();

			m_Button.onClick.RemoveAllListeners();
		}
		#endregion
		#endregion
	}
}