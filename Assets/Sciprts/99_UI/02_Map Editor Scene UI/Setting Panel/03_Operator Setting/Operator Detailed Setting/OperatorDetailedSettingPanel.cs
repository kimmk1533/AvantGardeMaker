using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class OperatorDetailedSettingPanel : SettingPanel
	{
		#region 기본 템플릿
		#region 변수
		private OperatorSettingSlot m_CurrentSettingSlot = null;
		private OperatorData m_CurrentOperatorData = null;

		#region Info Panel
		private RectTransform m_InfoPanel = null;

		private RectTransform m_NameParent = null;
		private TextMeshProUGUI m_EngNameText = null;
		private TextMeshProUGUI m_KorNameText = null;

		private RectTransform m_AttackRangeParent = null;

		private RectTransform m_StatParent = null;
		private TextMeshProUGUI m_MaxHpText = null;
		private TextMeshProUGUI m_AtkText = null;
		private TextMeshProUGUI m_DefText = null;
		private TextMeshProUGUI m_ResText = null;
		private TextMeshProUGUI m_RedeploymentText = null;
		private TextMeshProUGUI m_DeploymentCostText = null;
		private TextMeshProUGUI m_BlockCountText = null;
		private TextMeshProUGUI m_AspdText = null;

		private RectTransform m_LevelParent = null;
		private TextMeshProUGUI m_LevelText = null;
		private Button m_LevelMinus10Button = null;
		private Button m_LevelMinus5Button = null;
		private Button m_LevelMinus1Button = null;
		private Button m_LevelPlus10Button = null;
		private Button m_LevelPlus5Button = null;
		private Button m_LevelPlus1Button = null;
		#endregion

		private Button m_ConfirmButton = null;
		#endregion

		#region 프로퍼티
		private float maxHp
		{
			get => m_CurrentOperatorData.FixedData.MaxHp.GetLevelData(m_CurrentOperatorData.FixedData.Elite);
			set
			{
				m_CurrentOperatorData.VariableData.MaxHp = Mathf.Clamp(value, maxHp, nextMaxHp);
				m_CurrentOperatorData.VariableData.CurrentHp = m_CurrentOperatorData.VariableData.MaxHp;

				m_MaxHpText.text = value.ToString();
			}
		}
		private float atk
		{
			get => m_CurrentOperatorData.FixedData.Atk.GetLevelData(m_CurrentOperatorData.FixedData.Elite);
			set
			{
				m_CurrentOperatorData.VariableData.Atk = Mathf.Clamp(value, atk, nextAtk);

				m_AtkText.text = value.ToString();
			}
		}
		private float def
		{
			get => m_CurrentOperatorData.FixedData.Def.GetLevelData(m_CurrentOperatorData.FixedData.Elite);
			set
			{
				m_CurrentOperatorData.VariableData.Def = Mathf.Clamp(value, def, nextDef);

				m_DefText.text = value.ToString();
			}
		}
		private float res
		{
			get => m_CurrentOperatorData.FixedData.Res.GetLevelData(m_CurrentOperatorData.FixedData.Elite);
			set
			{
				m_CurrentOperatorData.VariableData.Res = Mathf.Clamp(value, res, nextRes);

				m_ResText.text = value.ToString();
			}
		}
		private int level
		{
			get => m_CurrentOperatorData.FixedData.Level;
			set
			{
				m_CurrentOperatorData.FixedData.Level = Mathf.Clamp(value, 1, maxLevel);

				m_LevelText.text = "<size=45><color=#00AFFF>" + level + "</color></size>/" + maxLevel;
			}
		}
		private int maxLevel
		{
			get => m_CurrentOperatorData.FixedData.MaxLevel.GetLevelData(m_CurrentOperatorData.FixedData.Elite);
		}

		private float nextMaxHp => m_CurrentOperatorData.FixedData.MaxHp.GetLevelData(m_CurrentOperatorData.FixedData.Elite + 1);
		private float nextAtk => m_CurrentOperatorData.FixedData.Atk.GetLevelData(m_CurrentOperatorData.FixedData.Elite + 1);
		private float nextDef => m_CurrentOperatorData.FixedData.Def.GetLevelData(m_CurrentOperatorData.FixedData.Elite + 1);
		private float nextRes => m_CurrentOperatorData.FixedData.Res.GetLevelData(m_CurrentOperatorData.FixedData.Elite + 1);
		private int nextMaxLevel => m_CurrentOperatorData.FixedData.MaxLevel.GetLevelData(m_CurrentOperatorData.FixedData.Elite + 1);

		private bool isSlotHasOperatorData => m_CurrentSettingSlot != null && m_CurrentSettingSlot.operatorData != null;
		#endregion

		#region 이벤트

		#region 이벤트 함수
		public void OnOperatorDataUIClicked(OperatorDataUI operatorDataUI)
		{
			if (m_CurrentOperatorData == operatorDataUI.operatorData)
			{
				ChangeParentsActive(false);
				m_CurrentOperatorData = null;

				return;
			}

			m_CurrentOperatorData = operatorDataUI.operatorData;

			UpdateUI(m_CurrentOperatorData);

			ChangeParentsActive(true);
		}
		private void OnLevelModifyButtonClicked(int value)
		{
			level += value;

			float maxHpDiff = (nextMaxHp - maxHp) / (maxLevel - 1);
			float atkDiff = (nextAtk - atk) / (maxLevel - 1);
			float defDiff = (nextDef - def) / (maxLevel - 1);
			float resDiff = (nextRes - res) / (maxLevel - 1);

			maxHp = Mathf.RoundToInt(maxHp + (maxHpDiff * (level - 1)));
			atk = Mathf.RoundToInt(atk + (atkDiff * (level - 1)));
			def = Mathf.RoundToInt(def + (defDiff * (level - 1)));
			res = Mathf.RoundToInt(res + (resDiff * (level - 1)));
		}

		private void OnConfirmButtonClicked()
		{
			OperatorSettingSlot settingSlot = m_CurrentSettingSlot;
			OperatorData slotOperatorData = settingSlot.operatorData;
			OperatorData seletedOperatorData = m_CurrentOperatorData;

			m_CurrentSettingSlot = null;
			m_CurrentOperatorData = null;

			gameObject.SetActive(false);

			settingSlot.operatorData = seletedOperatorData;

			if (slotOperatorData != null)
				M_MapEditingUI.RespawnOperatorDataUI(slotOperatorData.key);
			if (seletedOperatorData != null)
				M_MapEditingUI.RemoveOperatorDataUI(seletedOperatorData.key);
		}
		#endregion
		#endregion

		#region 매니저
		private static MapEditingUIManager M_MapEditingUI => MapEditingUIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void OnDisable()
		{
			if (m_CurrentSettingSlot != null &&
				m_CurrentSettingSlot.operatorData != null)
				M_MapEditingUI.RemoveOperatorDataUI(m_CurrentSettingSlot.operatorData.key);

			m_CurrentSettingSlot = null;
			m_CurrentOperatorData = null;
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			m_InfoPanel = transform.Find("Operator Info Panel").Find("Info Panel") as RectTransform;

			m_NameParent = transform.Find("Operator Info Panel").Find("Name") as RectTransform;
			m_EngNameText = m_NameParent.Find<TextMeshProUGUI>("Eng Name Text");
			m_KorNameText = m_NameParent.Find<TextMeshProUGUI>("Kor Name Text");

			m_AttackRangeParent = transform.Find("Operator Info Panel").Find("Attack Range") as RectTransform;

			m_StatParent = transform.Find("Operator Info Panel").Find("Stat") as RectTransform;
			m_MaxHpText = m_StatParent.Find("MaxHp").Find<TextMeshProUGUI>("Stat Text");
			m_AtkText = m_StatParent.Find("Atk").Find<TextMeshProUGUI>("Stat Text");
			m_DefText = m_StatParent.Find("Def").Find<TextMeshProUGUI>("Stat Text");
			m_ResText = m_StatParent.Find("Res").Find<TextMeshProUGUI>("Stat Text");
			m_RedeploymentText = m_StatParent.Find("Redeployment Time").Find<TextMeshProUGUI>("Stat Text");
			m_DeploymentCostText = m_StatParent.Find("Deployment Cost").Find<TextMeshProUGUI>("Stat Text");
			m_BlockCountText = m_StatParent.Find("Block Count").Find<TextMeshProUGUI>("Stat Text");
			m_AspdText = m_StatParent.Find("ASPD").Find<TextMeshProUGUI>("Stat Text");

			m_LevelParent = transform.Find("Operator Info Panel").Find("Level") as RectTransform;
			m_LevelText = m_LevelParent.Find<TextMeshProUGUI>("Level Value Text");
			m_LevelMinus10Button = m_LevelParent.Find<Button>("Minus 10 Button");
			m_LevelMinus5Button = m_LevelParent.Find<Button>("Minus 5 Button");
			m_LevelMinus1Button = m_LevelParent.Find<Button>("Minus 1 Button");
			m_LevelPlus10Button = m_LevelParent.Find<Button>("Plus 10 Button");
			m_LevelPlus5Button = m_LevelParent.Find<Button>("Plus 5 Button");
			m_LevelPlus1Button = m_LevelParent.Find<Button>("Plus 1 Button");

			m_ConfirmButton = transform.Find<Button>("Confirm Button");

			m_LevelMinus10Button.onClick.AddListener(() => OnLevelModifyButtonClicked(-10));
			m_LevelMinus5Button.onClick.AddListener(() => OnLevelModifyButtonClicked(-5));
			m_LevelMinus1Button.onClick.AddListener(() => OnLevelModifyButtonClicked(-1));
			m_LevelPlus10Button.onClick.AddListener(() => OnLevelModifyButtonClicked(10));
			m_LevelPlus5Button.onClick.AddListener(() => OnLevelModifyButtonClicked(5));
			m_LevelPlus1Button.onClick.AddListener(() => OnLevelModifyButtonClicked(1));

			m_ConfirmButton.onClick.AddListener(OnConfirmButtonClicked);

			ChangeParentsActive(false);
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();

			m_CurrentSettingSlot = null;
			m_CurrentOperatorData = null;

			m_LevelMinus10Button.onClick.RemoveAllListeners();
			m_LevelMinus5Button.onClick.RemoveAllListeners();
			m_LevelMinus1Button.onClick.RemoveAllListeners();
			m_LevelPlus10Button.onClick.RemoveAllListeners();
			m_LevelPlus5Button.onClick.RemoveAllListeners();
			m_LevelPlus1Button.onClick.RemoveAllListeners();

			m_ConfirmButton.onClick.RemoveAllListeners();
		}
		#endregion
		#endregion

		public void StartSetting(OperatorSettingSlot settingSlot)
		{
			m_CurrentSettingSlot = settingSlot;

			if (isSlotHasOperatorData == true)
			{
				m_CurrentOperatorData = settingSlot.operatorData;

				UpdateUI(m_CurrentOperatorData);

				M_MapEditingUI.RespawnOperatorDataUI(m_CurrentOperatorData.key);
			}

			ChangeParentsActive(isSlotHasOperatorData);

			gameObject.SetActive(true);
		}
		private void UpdateUI(OperatorData operatorData)
		{
			m_EngNameText.text = operatorData.EngName;
			m_KorNameText.text = operatorData.KorName;

			m_MaxHpText.text = operatorData.VariableData.MaxHp.ToString();
			m_AtkText.text = operatorData.VariableData.Atk.ToString();
			m_DefText.text = operatorData.VariableData.Def.ToString();
			m_ResText.text = operatorData.VariableData.Res.ToString();
			m_RedeploymentText.text = operatorData.VariableData.RedeploymentSpeed.ToString();
			//_DeploymentCostText.text = operatorData.VariableData.InitDeploymentCost.ToString();
			m_BlockCountText.text = operatorData.VariableData.BlockCount.ToString();
			m_AspdText.text = operatorData.FixedData.AtkSpeed.ToString();

			m_LevelText.text = "<size=45><color=#00AFFF>" + operatorData.FixedData.Level + "</color></size>/" + operatorData.FixedData.MaxLevel.GetLevelData(operatorData.FixedData.Elite);
		}
		private void ChangeParentsActive(bool showParents)
		{
			m_InfoPanel.gameObject.SetActive(!showParents);

			m_NameParent.gameObject.SetActive(showParents);
			m_AttackRangeParent.gameObject.SetActive(showParents);
			m_StatParent.gameObject.SetActive(showParents);
			m_LevelParent.gameObject.SetActive(showParents);
		}
	}
}