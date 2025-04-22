using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class OperatorDetailedSettingPanel : Panel
	{
		#region 기본 템플릿
		#region 변수
		private OperatorSettingSlotButton m_CurrentSettingSlotButton = null;
		private OperatorData m_CurrentOperatorData = null;

		#region Info Panel
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
		#endregion

		#region 프로퍼티
		private int level
		{
			get => m_CurrentOperatorData.FixedData.Level;
			set
			{
				m_CurrentOperatorData.FixedData.Level = Mathf.Clamp(value, 1, maxLevel);

				m_LevelText.text = "<size=45><color=#00AFFF>" + level + "</color></size>/" + maxLevel;
			}
		}
		private int maxLevel => m_CurrentOperatorData.FixedData.MaxLevel.GetLevelData(m_CurrentOperatorData.FixedData.Elite);
		private int nextMaxLevel => m_CurrentOperatorData.FixedData.MaxLevel.GetLevelData(m_CurrentOperatorData.FixedData.Elite + 1);

		private float maxHp => m_CurrentOperatorData.FixedData.MaxHp.GetLevelData(m_CurrentOperatorData.FixedData.Elite);
		private float nextMaxHp => m_CurrentOperatorData.FixedData.MaxHp.GetLevelData(m_CurrentOperatorData.FixedData.Elite + 1);
		private float atk => m_CurrentOperatorData.FixedData.Atk.GetLevelData(m_CurrentOperatorData.FixedData.Elite);
		private float nextAtk => m_CurrentOperatorData.FixedData.Atk.GetLevelData(m_CurrentOperatorData.FixedData.Elite + 1);
		private float def => m_CurrentOperatorData.FixedData.Def.GetLevelData(m_CurrentOperatorData.FixedData.Elite);
		private float nextDef => m_CurrentOperatorData.FixedData.Def.GetLevelData(m_CurrentOperatorData.FixedData.Elite + 1);
		private float res => m_CurrentOperatorData.FixedData.Res.GetLevelData(m_CurrentOperatorData.FixedData.Elite);
		private float nextRes => m_CurrentOperatorData.FixedData.Res.GetLevelData(m_CurrentOperatorData.FixedData.Elite + 1);
		#endregion

		#region 이벤트

		#region 이벤트 함수
		public void OnOperatorDataUIClicked(OperatorDataUI operatorDataUI)
		{
			m_CurrentOperatorData = operatorDataUI.operatorData;

			m_EngNameText.text = m_CurrentOperatorData.EngName;
			m_KorNameText.text = m_CurrentOperatorData.KorName;

			m_MaxHpText.text = m_CurrentOperatorData.VariableData.MaxHp.ToString();
			m_AtkText.text = m_CurrentOperatorData.VariableData.Atk.ToString();
			m_DefText.text = m_CurrentOperatorData.VariableData.Def.ToString();
			m_ResText.text = m_CurrentOperatorData.VariableData.Res.ToString();
			m_RedeploymentText.text = m_CurrentOperatorData.VariableData.RedeploymentSpeed.ToString();
			m_DeploymentCostText.text = m_CurrentOperatorData.VariableData.InitDeploymentCost.ToString();
			m_BlockCountText.text = m_CurrentOperatorData.VariableData.BlockCount.ToString();
			m_AspdText.text = m_CurrentOperatorData.FixedData.AtkSpeed.ToString();

			level = m_CurrentOperatorData.FixedData.Level;

			m_NameParent.gameObject.SetActive(true);
			m_AttackRangeParent.gameObject.SetActive(true);
			m_StatParent.gameObject.SetActive(true);
			m_LevelParent.gameObject.SetActive(true);
		}

		private void OnLevelModifyButtonClicked(int value)
		{
			level += value;

			float maxHpDiff = (nextMaxHp - maxHp) / (maxLevel - 1);
			float atkDiff = (nextAtk - atk) / (maxLevel - 1);
			float defDiff = (nextDef - def) / (maxLevel - 1);
			float resDiff = (nextRes - res) / (maxLevel - 1);

			m_MaxHpText.text = Mathf.RoundToInt(maxHp + (maxHpDiff * (level - 1))).ToString();
			m_AtkText.text = Mathf.RoundToInt(atk + (atkDiff * (level - 1))).ToString();
			m_DefText.text = Mathf.RoundToInt(def + (defDiff * (level - 1))).ToString();
			m_ResText.text = Mathf.RoundToInt(res + (resDiff * (level - 1))).ToString();
		}
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

			m_LevelMinus10Button.onClick.AddListener(() => OnLevelModifyButtonClicked(-10));
			m_LevelMinus5Button.onClick.AddListener(() => OnLevelModifyButtonClicked(-5));
			m_LevelMinus1Button.onClick.AddListener(() => OnLevelModifyButtonClicked(-1));
			m_LevelPlus10Button.onClick.AddListener(() => OnLevelModifyButtonClicked(10));
			m_LevelPlus5Button.onClick.AddListener(() => OnLevelModifyButtonClicked(5));
			m_LevelPlus1Button.onClick.AddListener(() => OnLevelModifyButtonClicked(1));
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();

			m_LevelMinus10Button.onClick.RemoveAllListeners();
			m_LevelMinus5Button.onClick.RemoveAllListeners();
			m_LevelMinus1Button.onClick.RemoveAllListeners();
			m_LevelPlus10Button.onClick.RemoveAllListeners();
			m_LevelPlus5Button.onClick.RemoveAllListeners();
			m_LevelPlus1Button.onClick.RemoveAllListeners();
		}
		#endregion
		#endregion

		public void StartSetting(OperatorSettingSlotButton slotButton)
		{
			m_CurrentSettingSlotButton = slotButton;

			gameObject.SetActive(true);
		}
	}
}