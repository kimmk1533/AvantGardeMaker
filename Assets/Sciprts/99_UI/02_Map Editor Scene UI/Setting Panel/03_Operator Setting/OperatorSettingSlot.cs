using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class OperatorSettingSlot : SerializedMonoBehaviour
	{
		#region 기본 템플릿
		#region 변수
		private OperatorData m_OperatorData = null;

		private Button m_SlotButton = null;

		private Image m_PlusImage = null;

		#region Operator Image
		private RectTransform m_OperatorImageParent = null;
		private Image m_OperatorImage = null;
		#endregion

		#region Potential
		private RectTransform m_PotentialParent = null;
		private TextMeshProUGUI m_PotentialText = null;
		#endregion

		#region Elite
		private RectTransform m_EliteParent = null;
		private TextMeshProUGUI m_EliteText = null;
		#endregion

		#region Level
		private RectTransform m_LevelParent = null;
		private TextMeshProUGUI m_LevelText = null;
		#endregion

		#region Module
		private int m_ModuleLevel = 0;

		private RectTransform m_ModuleParent = null;
		private Button m_ModuleLevelMinusButton = null;
		private Button m_ModuleButton = null;
		private Image m_ModuleImage = null;
		private TextMeshProUGUI m_ModuleLevelText = null;
		private Button m_ModuleLevelPlusButton = null;
		#endregion

		#region Skill
		private int m_SkillLevel = 1;

		private RectTransform m_SkillParent = null;
		private Button m_SkillLevelMinusButton = null;
		private Button m_SkillButton = null;
		private Image m_SkillImage = null;
		private TextMeshProUGUI m_SkillLevelText = null;
		private Button m_SkillLevelPlusButton = null;
		#endregion
		#endregion

		#region 프로퍼티
		public OperatorData operatorData
		{
			get => m_OperatorData;
			set
			{
				m_OperatorData = value;

				ChangeParentsActive(value != null);

				if (value == null)
				{
					m_ModuleLevel = 0;
					m_SkillLevel = 1;

					return;
				}

				m_SkillLevel = value.FixedData.SkillLevel;

				m_OperatorImage.sprite = M_Operator.GetOperatorPortrait(value.key);
				m_PotentialText.text = value.FixedData.Potential.ToString();
				m_EliteText.text = value.FixedData.Elite.ToString();
				m_LevelText.text = "<size=13>LV</size>\n" + value.FixedData.Level.ToString();
				m_ModuleImage.sprite = null;
				m_SkillImage.sprite = null;
			}
		}
		#endregion

		#region 이벤트

		#region 이벤트 함수
		private void OnSlotButtonClicked()
		{
			OperatorDetailedSettingPanel settingPanel = M_MapEditingUI.settingPanelController.GetDetailedSettingPanel<OperatorDetailedSettingPanel>("Operator");

			settingPanel.StartSetting(this);
		}
		private void OnMinusButtonClicked(int currentLevel, TextMeshProUGUI text)
		{

		}
		private void OnPlusButtonClicked(int currentLevel, TextMeshProUGUI text)
		{

		}
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
		public void Initialize()
		{
			m_SlotButton = GetComponent<Button>();
			m_SlotButton.onClick.AddListener(OnSlotButtonClicked);

			m_PlusImage = transform.Find<Image>("Plus Image");

			m_OperatorImageParent = transform.Find("Operator Image") as RectTransform;
			m_OperatorImage = transform.Find<Image>("Operator Image");

			m_PotentialParent = transform.Find("Potential") as RectTransform;
			m_PotentialText = m_PotentialParent.Find<TextMeshProUGUI>("Potential Text");

			m_EliteParent = transform.Find("Elite") as RectTransform;
			m_EliteText = m_EliteParent.Find<TextMeshProUGUI>("Elite Text");

			m_LevelParent = transform.Find("Level") as RectTransform;
			m_LevelText = m_LevelParent.Find<TextMeshProUGUI>("Level Text");

			m_ModuleParent = transform.Find("Module") as RectTransform;
			m_ModuleLevelMinusButton = m_ModuleParent.Find<Button>("Minus Button");
			m_ModuleButton = m_ModuleParent.Find<Button>("Module Button");
			m_ModuleImage = m_ModuleButton.transform.Find<Image>("Module Image");
			m_ModuleLevelText = m_ModuleButton.transform.Find("Module Level").Find<TextMeshProUGUI>("Module Level Text");
			m_ModuleLevelPlusButton = m_ModuleParent.Find<Button>("Plus Button");

			m_SkillParent = transform.Find("Skill") as RectTransform;
			m_SkillLevelMinusButton = m_SkillParent.Find<Button>("Minus Button");
			m_SkillButton = m_SkillParent.Find<Button>("Skill Button");
			m_SkillImage = m_SkillButton.transform.Find<Image>("Skill Image");
			m_SkillLevelText = m_SkillButton.transform.Find("Skill Level").Find<TextMeshProUGUI>("Skill Level Text");
			m_SkillLevelPlusButton = m_SkillParent.Find<Button>("Plus Button");

			m_ModuleLevelMinusButton.onClick.AddListener(() => OnMinusButtonClicked(m_ModuleLevel, m_ModuleLevelText));
			m_ModuleLevelPlusButton.onClick.AddListener(() => OnPlusButtonClicked(m_ModuleLevel, m_ModuleLevelText));

			m_SkillLevelMinusButton.onClick.AddListener(() => OnMinusButtonClicked(m_SkillLevel, m_SkillLevelText));
			m_SkillLevelPlusButton.onClick.AddListener(() => OnPlusButtonClicked(m_SkillLevel, m_SkillLevelText));

			ChangeParentsActive(false);
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{
			m_SlotButton.onClick.RemoveAllListeners();
		}
		#endregion
		#endregion

		private void ChangeParentsActive(bool showParents)
		{
			m_PlusImage.gameObject.SetActive(!showParents);

			m_OperatorImageParent.gameObject.SetActive(showParents);
			m_PotentialParent.gameObject.SetActive(showParents);
			m_EliteParent.gameObject.SetActive(showParents);
			m_LevelParent.gameObject.SetActive(showParents);
			m_ModuleParent.gameObject.SetActive(showParents);
			m_SkillParent.gameObject.SetActive(showParents);
		}
	}
}