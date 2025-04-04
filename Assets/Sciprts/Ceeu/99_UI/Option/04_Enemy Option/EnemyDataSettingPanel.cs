using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.ad1a;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class EnemyDataSettingPanel : Panel
	{
		#region 변수
		private EnemySpawnDataUI m_CurrentEnemySpawnDataUI = null;

		#region Enemy Stat 변수
		#region Infos 변수
		private Image m_EnemyTypeImage = null;
		private TMP_Text m_RaceText = null;
		private TMP_Text m_CodeText = null;
		private TMP_Text m_NameText = null;
		private TMP_Text m_AttackInfoText = null;
		private Image m_LifeTypeImage = null;
		private TMP_Text m_LifeValueText = null;
		#endregion

		#region Datas 변수
		private Image m_PortraitImage = null;

		private TMP_Text m_WeightValueText = null;

		private DataUI m_HpDataUI = null;
		private DataUI m_AtkDataUI = null;
		private DataUI m_DefDataUI = null;
		private DataUI m_ResDataUI = null;
		private DataUI m_MovementSpeedDataUI = null;
		private DataUI m_AspdDataUI = null;
		private DataUI m_ElementalResDataUI = null;
		private DataUI m_EffectResistanceDataUI = null;
		#endregion

		#region Descriptions 변수
		private TMP_Text m_EnemyDescriptionText = null;
		private TMP_Text m_TraitDescriptionText = null;
		#endregion
		#endregion

		#region WayPoint 변수

		#region WayPoint Buttons 변수
		private Button m_AddButton = null;
		private Button m_CopyButton = null;
		private Button m_PasteButton = null;
		#endregion
		#endregion

		#region Buttons 변수
		private Button m_ConfirmButton = null;
		private Button m_CancleButton = null;
		#endregion
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트

		#region 이벤트 함수
		#region Enemy Stat
		private void OnStatInputFieldValueChanged(DataUI dataUI, string inputString)
		{
			int.TryParse(inputString.Replace(",", ""), out int inputValue);
			dataUI.inputField.text = string.Format("{0:#,###}", inputValue);
			dataUI.inputField.stringPosition = dataUI.inputField.text.Length;
		}
		private void OnHpInputFieldValueChanged(string inputString)
		{
			OnStatInputFieldValueChanged(m_HpDataUI, inputString);
		}
		private void OnAtkInputFieldValueChanged(string inputString)
		{
			OnStatInputFieldValueChanged(m_AtkDataUI, inputString);
		}
		private void OnDefInputFieldValueChanged(string inputString)
		{
			OnStatInputFieldValueChanged(m_DefDataUI, inputString);
		}
		private void OnResInputFieldValueChanged(string inputString)
		{
			OnStatInputFieldValueChanged(m_ResDataUI, inputString);
		}
		private void OnMovementSpeedInputFieldValueChanged(string inputString)
		{
			OnStatInputFieldValueChanged(m_MovementSpeedDataUI, inputString);
		}
		private void OnAspdInputFieldValueChanged(string inputString)
		{
			OnStatInputFieldValueChanged(m_AspdDataUI, inputString);
		}
		private void OnElementalResInputFieldValueChanged(string inputString)
		{
			OnStatInputFieldValueChanged(m_ElementalResDataUI, inputString);
		}
		private void OnEffectResistanceInputFieldValueChanged(string inputString)
		{
			OnStatInputFieldValueChanged(m_EffectResistanceDataUI, inputString);
		}
		#endregion

		#region WayPoint
		private void OnAddButtonClicked()
		{
			EnemyWayPointDataUI wayPointDataUI = M_MapEditor.GetBuilder("Enemy WayPoint Data UI")
				.SetScale(Vector3.one)
				.SetParent(M_MapEditor.enemyWayPointDataUIParent)
				.SetAutoInit(true)
				.SetActive(true)
				.Spawn() as EnemyWayPointDataUI;
		}
		private void OnCopyButtonClicked()
		{

		}
		private void OnPasteButtonClicked()
		{

		}
		#endregion

		#region Buttons
		private void OnConfirmButtonClicked()
		{
			// 새로운 적 데이터 저장
			SaveEnemyDataUI();

			gameObject.SetActive(false);
		}
		private void OnCancleButtonClicked()
		{
			// 기존 적 데이터 불러오기
			UpdateStatUI();
			UpdateWayPointUI();

			gameObject.SetActive(false);
		}
		#endregion
		#endregion
		#endregion

		#region 매니저
		private static MapEditorUIManager M_MapEditor => MapEditorUIManager.Instance;
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

			#region Enemy Stat 초기화
			#region Infos 초기화
			if (m_EnemyTypeImage == null)
				m_EnemyTypeImage = transform.FindInChildren<Image>("Enemy Type Image");
			if (m_RaceText == null)
				m_RaceText = transform.FindInChildren<TMP_Text>("Race Info Text");
			if (m_CodeText == null)
				m_CodeText = transform.FindInChildren<TMP_Text>("Code Info Text");
			if (m_NameText == null)
				m_NameText = transform.FindInChildren<TMP_Text>("Name Info Text");
			if (m_AttackInfoText == null)
				m_AttackInfoText = transform.FindInChildren<TMP_Text>("Attack Info Text");
			if (m_LifeTypeImage == null)
				m_LifeTypeImage = transform.FindInChildren<Image>("Life Type Image");
			if (m_LifeValueText == null)
				m_LifeValueText = transform.FindInChildren<TMP_Text>("Life Value Text");
			#endregion

			#region Datas 초기화
			if (m_PortraitImage == null)
				m_PortraitImage = transform.FindInChildren<Image>("Portrait Image");

			if (m_WeightValueText == null)
				m_WeightValueText = transform.FindInChildren<TMP_Text>("Weight Value Text");

			if (m_HpDataUI == null)
			{
				m_HpDataUI = new DataUI(transform, "HP");

				m_HpDataUI.inputField.onValueChanged.AddListener(OnHpInputFieldValueChanged);
			}
			if (m_AtkDataUI == null)
			{
				m_AtkDataUI = new DataUI(transform, "ATK");

				m_AtkDataUI.inputField.onValueChanged.AddListener(OnAtkInputFieldValueChanged);
			}
			if (m_DefDataUI == null)
			{
				m_DefDataUI = new DataUI(transform, "DEF");

				m_DefDataUI.inputField.onValueChanged.AddListener(OnDefInputFieldValueChanged);
			}
			if (m_ResDataUI == null)
			{
				m_ResDataUI = new DataUI(transform, "RES");

				m_ResDataUI.inputField.onValueChanged.AddListener(OnResInputFieldValueChanged);
			}
			if (m_MovementSpeedDataUI == null)
			{
				m_MovementSpeedDataUI = new DataUI(transform, "Movement Speed");

				m_MovementSpeedDataUI.inputField.onValueChanged.AddListener(OnMovementSpeedInputFieldValueChanged);
			}
			if (m_AspdDataUI == null)
			{
				m_AspdDataUI = new DataUI(transform, "ASPD");

				m_AspdDataUI.inputField.onValueChanged.AddListener(OnAspdInputFieldValueChanged);
			}
			if (m_ElementalResDataUI == null)
			{
				m_ElementalResDataUI = new DataUI(transform, "Elemental RES");

				m_ElementalResDataUI.inputField.onValueChanged.AddListener(OnElementalResInputFieldValueChanged);
			}
			if (m_EffectResistanceDataUI == null)
			{
				m_EffectResistanceDataUI = new DataUI(transform, "Effect Resistance");

				m_EffectResistanceDataUI.inputField.onValueChanged.AddListener(OnEffectResistanceInputFieldValueChanged);
			}
			#endregion

			#region Descriptions 초기화
			if (m_EnemyDescriptionText == null)
				m_EnemyDescriptionText = transform.FindInChildren<TMP_Text>("Enemy Description Text");
			if (m_TraitDescriptionText == null)
				m_TraitDescriptionText = transform.FindInChildren<TMP_Text>("Trait Description Text");
			#endregion
			#endregion

			#region WayPoint 초기화

			#region WayPoint Buttons 초기화
			if (m_AddButton == null)
			{
				m_AddButton = transform.FindInChildren<Button>("Add Button");

				m_AddButton.onClick.AddListener(OnAddButtonClicked);
			}
			if (m_CopyButton == null)
			{
				m_CopyButton = transform.FindInChildren<Button>("Copy Button");

				m_CopyButton.onClick.AddListener(OnCopyButtonClicked);
			}
			if (m_PasteButton == null)
			{
				m_PasteButton = transform.FindInChildren<Button>("Paste Button");

				m_PasteButton.onClick.AddListener(OnPasteButtonClicked);
			}
			#endregion
			#endregion

			#region Buttons 초기화
			if (m_ConfirmButton == null)
			{
				m_ConfirmButton = transform.FindInChildren<Button>("Confirm Button");

				m_ConfirmButton.onClick.AddListener(OnConfirmButtonClicked);
			}
			if (m_CancleButton == null)
			{
				m_CancleButton = transform.FindInChildren<Button>("Cancle Button");

				m_CancleButton.onClick.AddListener(OnCancleButtonClicked);
			}
			#endregion

			gameObject.SetActive(false);
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();

		}
		#endregion

		public void SetEnemySpawnDataUI(EnemySpawnDataUI enemySpawnDataUI)
		{
			m_CurrentEnemySpawnDataUI = enemySpawnDataUI;

			UpdateStatUI();
			UpdateWayPointUI();
		}
		private void UpdateStatUI()
		{
			EnemyData enemyData = m_CurrentEnemySpawnDataUI.enemyData;

			#region Infos
			//string enemyType = ad1a.Enum.EnumUtil.EnumToKorString(enemyData.FixedData.EnemyType);
			//m_EnemyTypeImage = enemyData.FixedData.EnemyType;

			string raceText = ad1a.Enum.EnumUtil.EnumToKorString(enemyData.FixedData.RaceType);
			m_RaceText.text = raceText;

			//string codeText = ad1a.Enum.EnumUtil.EnumToKorString(enemyData.FixedData.Code);
			//m_CodeText = null;

			m_NameText.text = enemyData.Name;

			string atkPattern = ad1a.Enum.EnumUtil.EnumToKorString(enemyData.FixedData.AtkPattern);
			string dmgType = ad1a.Enum.EnumUtil.EnumToKorString(enemyData.FixedData.DmgType);
			m_AttackInfoText.text = atkPattern + " " + dmgType;

			//m_LifeTypeImage = null;
			m_LifeValueText.text = enemyData.FixedData.LossHp.ToString();
			#endregion

			#region Datas
			//m_PortraitImage = null;

			m_WeightValueText.text = enemyData.FixedData.Weight.InitStat.ToString();

			m_HpDataUI.inputField.text = ((int)enemyData.VariableData.Hp.InitStat).ToString();
			//m_HpDataUI.rankText.text = enemyData.VariableData.Hp.Rank.ToString();
			m_HpDataUI.rankDropdown.value = (int)enemyData.VariableData.Hp.Rank;

			m_AtkDataUI.inputField.text = ((int)enemyData.VariableData.Atk.InitStat).ToString();
			//m_AtkDataUI.rankText.text = enemyData.VariableData.Atk.Rank.ToString();
			m_AtkDataUI.rankDropdown.value = (int)enemyData.VariableData.Atk.Rank;

			m_DefDataUI.inputField.text = ((int)enemyData.VariableData.Def.InitStat).ToString();
			//m_DefDataUI.rankText.text = enemyData.VariableData.Def.Rank.ToString();
			m_DefDataUI.rankDropdown.value = (int)enemyData.VariableData.Def.Rank;

			m_ResDataUI.inputField.text = ((int)enemyData.VariableData.Res.InitStat).ToString();
			//m_ResDataUI.rankText.text = enemyData.VariableData.Res.Rank.ToString();
			m_ResDataUI.rankDropdown.value = (int)enemyData.VariableData.Res.Rank;

			m_MovementSpeedDataUI.inputField.text = ((int)enemyData.VariableData.MovementSpeed.InitStat).ToString();
			//m_MovementSpeedDataUI.rankText.text = enemyData.VariableData.MovementSpeed.Rank.ToString();
			m_MovementSpeedDataUI.rankDropdown.value = (int)enemyData.VariableData.MovementSpeed.Rank;

			m_AspdDataUI.inputField.text = ((int)enemyData.VariableData.Aspd.InitStat).ToString();
			//m_AspdDataUI.rankText.text = enemyData.VariableData.Aspd.Rank.ToString();
			m_AspdDataUI.rankDropdown.value = (int)enemyData.VariableData.Aspd.Rank;

			m_ElementalResDataUI.inputField.text = ((int)enemyData.VariableData.ElementalRes.InitStat).ToString();
			//m_ElementalResDataUI.rankText.text = enemyData.VariableData.ElementalRes.Rank.ToString();
			m_ElementalResDataUI.rankDropdown.value = (int)enemyData.VariableData.ElementalRes.Rank;

			m_EffectResistanceDataUI.inputField.text = ((int)enemyData.VariableData.EffectResistance.InitStat).ToString();
			//m_EffectResistanceDataUI.rankText.text = enemyData.VariableData.EffectResistance.Rank.ToString();
			m_EffectResistanceDataUI.rankDropdown.value = (int)enemyData.VariableData.EffectResistance.Rank;
			#endregion

			#region Descriptions
			//m_EnemyDescriptionText = null;
			//m_TraitDescriptionText = null;
			#endregion
		}
		private void UpdateWayPointUI()
		{
			RectTransform wayPointDataUIParent = M_MapEditor.enemyWayPointDataUIParent;
			int count = wayPointDataUIParent.childCount;
			for (int i = 0; i < count; ++i)
			{
				EnemyWayPointDataUI wayPointDataUI = wayPointDataUIParent.GetChild<EnemyWayPointDataUI>(0);

				if (wayPointDataUI == null)
					continue;

				M_MapEditor.Despawn(wayPointDataUI);
			}

			count = m_CurrentEnemySpawnDataUI.enemyWayPointList.Count;
			for (int i = 0; i < count; ++i)
			{
				Vector2 wayPoint = m_CurrentEnemySpawnDataUI.enemyWayPointList[i];
				EnemyWayPointDataUI wayPointDataUI = M_MapEditor.GetBuilder("Enemy WayPoint Data UI")
					.SetParent(wayPointDataUIParent)
					.SetScale(Vector3.one)
					.SetActive(true)
					.SetAutoInit(true)
					.Spawn() as EnemyWayPointDataUI;

				wayPointDataUI.position = wayPoint;
			}
		}
		private void SaveEnemyDataUI()
		{
			#region Datas
			EnemyData enemyData = m_CurrentEnemySpawnDataUI.enemyData;

			string hpText = m_HpDataUI.inputField.text.Replace(",", "");
			float.TryParse(hpText, out float hpValue);
			ad1a.Enum.E_EnemyRankType hpRankType = (ad1a.Enum.E_EnemyRankType)m_HpDataUI.rankDropdown.value;
			enemyData.VariableData.Hp = new VariableCombatStatValue<float>(hpValue, hpRankType);

			string atkText = m_AtkDataUI.inputField.text.Replace(",", "");
			float.TryParse(atkText, out float atkValue);
			ad1a.Enum.E_EnemyRankType atkRankType = (ad1a.Enum.E_EnemyRankType)m_AtkDataUI.rankDropdown.value;
			enemyData.VariableData.Atk = new VariableCombatStatValue<float>(atkValue, atkRankType);

			string defText = m_DefDataUI.inputField.text.Replace(",", "");
			float.TryParse(defText, out float defValue);
			ad1a.Enum.E_EnemyRankType defRankType = (ad1a.Enum.E_EnemyRankType)m_DefDataUI.rankDropdown.value;
			enemyData.VariableData.Def = new VariableCombatStatValue<float>(defValue, defRankType);

			string resText = m_ResDataUI.inputField.text.Replace(",", "");
			float.TryParse(resText, out float resValue);
			ad1a.Enum.E_EnemyRankType resRankType = (ad1a.Enum.E_EnemyRankType)m_ResDataUI.rankDropdown.value;
			enemyData.VariableData.Res = new VariableCombatStatValue<float>(resValue, resRankType);

			string movementSpeedText = m_MovementSpeedDataUI.inputField.text.Replace(",", "");
			float.TryParse(movementSpeedText, out float movementSpeedValue);
			ad1a.Enum.E_EnemyRankType movementSpeedRankType = (ad1a.Enum.E_EnemyRankType)m_MovementSpeedDataUI.rankDropdown.value;
			enemyData.VariableData.MovementSpeed = new VariableCombatStatValue<float>(movementSpeedValue, movementSpeedRankType);

			string aspdText = m_AspdDataUI.inputField.text.Replace(",", "");
			float.TryParse(aspdText, out float aspdValue);
			ad1a.Enum.E_EnemyRankType aspdRankType = (ad1a.Enum.E_EnemyRankType)m_AspdDataUI.rankDropdown.value;
			enemyData.VariableData.Aspd = new VariableCombatStatValue<float>(aspdValue, aspdRankType);

			string elementalResText = m_ElementalResDataUI.inputField.text.Replace(",", "");
			float.TryParse(elementalResText, out float elementalResValue);
			ad1a.Enum.E_EnemyRankType elementalResRankType = (ad1a.Enum.E_EnemyRankType)m_ElementalResDataUI.rankDropdown.value;
			enemyData.VariableData.ElementalRes = new VariableCombatStatValue<float>(elementalResValue, elementalResRankType);

			string effectResistanceText = m_EffectResistanceDataUI.inputField.text.Replace(",", "");
			float.TryParse(effectResistanceText, out float effectResistanceValue);
			ad1a.Enum.E_EnemyRankType effectResistanceRankType = (ad1a.Enum.E_EnemyRankType)m_EffectResistanceDataUI.rankDropdown.value;
			enemyData.VariableData.EffectResistance = new VariableCombatStatValue<float>(effectResistanceValue, effectResistanceRankType);
			#endregion

			#region WayPoints
			m_CurrentEnemySpawnDataUI.enemyWayPointList.Clear();

			RectTransform wayPointDataUIParent = M_MapEditor.enemyWayPointDataUIParent;
			int count = wayPointDataUIParent.childCount;
			for (int i = 0; i < count; ++i)
			{
				EnemyWayPointDataUI wayPointDataUI = wayPointDataUIParent.GetChild<EnemyWayPointDataUI>(i);

				m_CurrentEnemySpawnDataUI.enemyWayPointList.Add(wayPointDataUI.position);
			}
			#endregion
		}

		private class DataUI
		{
			public TMP_InputField inputField { get; }
			[System.Obsolete]
			public TMP_Text rankText { get; }
			public TMP_Dropdown rankDropdown { get; }

			public DataUI(Transform transform, string dataName)
			{
				inputField = transform.FindInChildren<TMP_InputField>(dataName + " InputField");
				rankText = transform.FindInChildren<TMP_Text>(dataName + " Rank Text");
				rankDropdown = transform.FindInChildren<TMP_Dropdown>(dataName + " Rank Dropdown");
			}
		}
	}
}