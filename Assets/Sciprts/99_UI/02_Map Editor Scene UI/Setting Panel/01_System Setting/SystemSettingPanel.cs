using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace AvantGardeMaker.UI
{
	public class SystemSettingPanel : SettingPanel
	{
		#region 기본 템플릿
		#region 변수
		private TMP_InputField m_StageTitleInputField = null;
		private TMP_InputField m_LifePointInputField = null;
		private TMP_InputField m_InitCostInputField = null;
		private TMP_InputField m_MaxCostInputField = null;
		private TMP_InputField m_CostIncreaseTimeInputField = null;
		private TMP_InputField m_DescriptionInputField = null;
		#endregion

		#region 프로퍼티
		public string stageTitle
		{
			get => m_StageTitleInputField.text;
			set => m_StageTitleInputField.SetTextWithoutNotify(value);
		}
		public int lifePoint
		{
			get => int.Parse(m_LifePointInputField.text);
			set => m_LifePointInputField.SetTextWithoutNotify(value.ToString());
		}
		public int initCost
		{
			get => int.Parse(m_InitCostInputField.text);
			set => m_InitCostInputField.SetTextWithoutNotify(value.ToString());
		}
		public int maxCost
		{
			get => int.Parse(m_MaxCostInputField.text);
			set => m_MaxCostInputField.SetTextWithoutNotify(value.ToString());
		}
		public float costIncreaseTime
		{
			get => float.Parse(m_CostIncreaseTimeInputField.text);
			set => m_CostIncreaseTimeInputField.SetTextWithoutNotify(value.ToString());
		}
		public string description
		{
			get => m_DescriptionInputField.text;
			set => m_DescriptionInputField.SetTextWithoutNotify(value);
		}
		#endregion

		#region 이벤트

		#region 이벤트 함수
		private void OnStageTitleInputFieldEndEdit(string value)
		{
			M_MapEditing.stageTitle = value;
		}
		private void OnDescriptionInputFieldEndEdit(string value)
		{
			M_MapEditing.description = value;
		}
		private void OnLifePointInputFieldEndEdit(string value)
		{
			if (int.TryParse(value, out int lifePoint) == false)
				return;

			M_MapEditing.lifePoint = lifePoint;
			m_LifePointInputField.SetTextWithoutNotify(lifePoint.ToString());
		}
		private void OnInitCostInputFieldEndEdit(string value)
		{
			if (int.TryParse(value, out int initCost) == false)
				return;

			M_MapEditing.initCost = initCost;
			m_InitCostInputField.SetTextWithoutNotify(initCost.ToString());
		}
		private void OnMaxCostInputFieldEndEdit(string value)
		{
			if (int.TryParse(value, out int maxCost) == false)
				return;

			M_MapEditing.maxCost = maxCost;
			m_MaxCostInputField.SetTextWithoutNotify(maxCost.ToString());
		}
		private void OnCostIncreaseTimeInputFieldEndEdit(string value)
		{
			if (float.TryParse(value, out float costIncreaseTime) == false)
				return;

			M_MapEditing.costIncreaseTime = costIncreaseTime;
			m_CostIncreaseTimeInputField.SetTextWithoutNotify(costIncreaseTime.ToString());
		}
		#endregion
		#endregion

		#region 매니저
		private static MapEditingManager M_MapEditing => MapEditingManager.Instance;
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

			m_StageTitleInputField = transform.Find<TMP_InputField>("Stage Title/Input Field");
			m_DescriptionInputField = transform.Find<TMP_InputField>("Description/Input Field");
			m_LifePointInputField = transform.Find<TMP_InputField>("Life Point/Input Field");
			m_InitCostInputField = transform.Find<TMP_InputField>("Cost/Init Cost/Input Field");
			m_MaxCostInputField = transform.Find<TMP_InputField>("Cost/Max Cost/Input Field");
			m_CostIncreaseTimeInputField = transform.Find<TMP_InputField>("Cost/Cost Increase Time/Input Field");

			m_StageTitleInputField.onEndEdit.AddListener(OnStageTitleInputFieldEndEdit);
			m_DescriptionInputField.onEndEdit.AddListener(OnDescriptionInputFieldEndEdit);
			m_LifePointInputField.onEndEdit.AddListener(OnLifePointInputFieldEndEdit);
			m_InitCostInputField.onEndEdit.AddListener(OnInitCostInputFieldEndEdit);
			m_MaxCostInputField.onEndEdit.AddListener(OnMaxCostInputFieldEndEdit);
			m_CostIncreaseTimeInputField.onEndEdit.AddListener(OnCostIncreaseTimeInputFieldEndEdit);
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();

			FinallizeInputField(ref m_StageTitleInputField);
			FinallizeInputField(ref m_DescriptionInputField);
			FinallizeInputField(ref m_LifePointInputField);
			FinallizeInputField(ref m_InitCostInputField);
			FinallizeInputField(ref m_MaxCostInputField);
			FinallizeInputField(ref m_CostIncreaseTimeInputField);
		}
		#endregion
		#endregion

		private void FinallizeInputField(ref TMP_InputField inputField)
		{
			inputField.onEndEdit.RemoveAllListeners();
			inputField = null;
		}
	}
}