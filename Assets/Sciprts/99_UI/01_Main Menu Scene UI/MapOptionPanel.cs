using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AvantGardeMaker.CoreSpace;
using AvantGardeMaker.CoreSpace.SaveLoad;
using CoreSources;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class MapOptionPanel : Panel
	{
		#region 기본 템플릿
		#region 변수
		private TMP_InputField m_StageTitleInputField = null;
		private TMP_InputField m_LifePointInputField = null;
		private TMP_InputField m_InitCostInputField = null;
		private TMP_InputField m_MaxCostInputField = null;
		private TMP_InputField m_CostIncreaseTimeInputField = null;

		private Button m_ConfirmButton = null;
		private Button m_CancleButton = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 매니저
		private static MapEditingManager M_MapEditing => MapEditingManager.Instance;
		#endregion

		#region 이벤트

		#region 이벤트 함수
		private void OnStageTitleInputFieldEndEdit(string value)
		{
			string regexTitle = Regex.Replace(value, @"[\\/:*?""<>|]", "");
			m_StageTitleInputField.SetTextWithoutNotify(regexTitle);
		}
		private void OnLifePointInputFieldEndEdit(string value)
		{
			if (int.TryParse(value, out int lifePoint) == false)
				return;

			lifePoint = Mathf.Clamp(lifePoint, 1, 999);

			M_MapEditing.lifePoint = lifePoint;
			m_LifePointInputField.SetTextWithoutNotify(lifePoint.ToString());
		}
		private void OnInitCostInputFieldEndEdit(string value)
		{
			if (int.TryParse(value, out int initCost) == false)
				return;

			initCost = Mathf.Clamp(initCost, 0, 999);

			M_MapEditing.initCost = initCost;
			m_InitCostInputField.SetTextWithoutNotify(initCost.ToString());
		}
		private void OnMaxCostInputFieldEndEdit(string value)
		{
			if (int.TryParse(value, out int maxCost) == false)
				return;

			maxCost = Mathf.Clamp(maxCost, 0, 999);

			M_MapEditing.maxCost = maxCost;
			m_MaxCostInputField.SetTextWithoutNotify(maxCost.ToString());
		}
		private void OnCostIncreaseTimeInputFieldEndEdit(string value)
		{
			if (float.TryParse(value, out float costIncreaseTime) == false)
				return;

			costIncreaseTime = Mathf.Clamp(costIncreaseTime, 0f, 99f);

			M_MapEditing.costIncreaseTime = costIncreaseTime;
			m_CostIncreaseTimeInputField.SetTextWithoutNotify(costIncreaseTime.ToString());
		}

		private void OnConfirmButtonClicked()
		{
			StageData stageData = new StageData();
			StageData.Initialize(ref stageData);

			stageData.title = m_StageTitleInputField.text;
			stageData.lifePoint = GetInputFieldIntValue(m_LifePointInputField);
			stageData.initCost = GetInputFieldIntValue(m_InitCostInputField);
			stageData.maxCost = GetInputFieldIntValue(m_MaxCostInputField);
			stageData.costIncreaseTime = GetInputFieldFloatValue(m_CostIncreaseTimeInputField);

			M_MapEditing.SynchronizeStageData(stageData);

			SceneLoader.LoadScene("Map Editing Scene");
		}
		private void OnCancleButtonClicked()
		{
			// 인풋필드 초기화 하고
			InitializeInputFields();

			// 패널창 닫기
			gameObject.SetActive(false);
		}
		#endregion
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			m_StageTitleInputField = transform.Find<TMP_InputField>("Stage Title/Input Field");
			m_LifePointInputField = transform.Find<TMP_InputField>("Life Point/Input Field");
			m_InitCostInputField = transform.Find<TMP_InputField>("Cost/Init Cost/Input Field");
			m_MaxCostInputField = transform.Find<TMP_InputField>("Cost/Max Cost/Input Field");
			m_CostIncreaseTimeInputField = transform.Find<TMP_InputField>("Cost/Cost Increase Time/Input Field");

			m_ConfirmButton = transform.Find<Button>("Confirm Button");
			m_CancleButton = transform.Find<Button>("Cancle Button");

			InitializeInputFields();

			m_StageTitleInputField.onEndEdit.AddListener(OnStageTitleInputFieldEndEdit);
			m_LifePointInputField.onEndEdit.AddListener(OnLifePointInputFieldEndEdit);
			m_InitCostInputField.onEndEdit.AddListener(OnInitCostInputFieldEndEdit);
			m_MaxCostInputField.onEndEdit.AddListener(OnMaxCostInputFieldEndEdit);
			m_CostIncreaseTimeInputField.onEndEdit.AddListener(OnCostIncreaseTimeInputFieldEndEdit);

			m_ConfirmButton.onClick.AddListener(OnConfirmButtonClicked);
			m_CancleButton.onClick.AddListener(OnCancleButtonClicked);
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();


		}

		private void InitializeInputFields()
		{
			m_StageTitleInputField.SetTextWithoutNotify(string.Empty);
			m_LifePointInputField.SetTextWithoutNotify("3");
			m_InitCostInputField.SetTextWithoutNotify("10");
			m_MaxCostInputField.SetTextWithoutNotify("99");
			m_CostIncreaseTimeInputField.SetTextWithoutNotify("1.0");
		}
		#endregion

		#region 유니티 콜백 함수
		#endregion
		#endregion

		private async Awaitable CheckStageTitle()
		{
			List<string> titleList = await SaveLoadUtility.GetMakingMapTitleList();

			//if ()
		}
		private int GetInputFieldIntValue(TMP_InputField inputField)
		{
			if (int.TryParse(inputField.text, out int result) == false)
				return default;

			return result;
		}
		private float GetInputFieldFloatValue(TMP_InputField inputField)
		{
			if (float.TryParse(inputField.text, out float result) == false)
				return default;

			return result;
		}
	}
}