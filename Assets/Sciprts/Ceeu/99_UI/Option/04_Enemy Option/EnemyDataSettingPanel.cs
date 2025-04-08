using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.ad1a;
using AvantGardeMaker.ad1a.Enum;
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

		private StatDataUI m_HpDataUI = null;
		private StatDataUI m_AtkDataUI = null;
		private StatDataUI m_DefDataUI = null;
		private StatDataUI m_ResDataUI = null;
		private StatDataUI m_MovementSpeedDataUI = null;
		private StatDataUI m_AspdDataUI = null;
		private StatDataUI m_ElementalResDataUI = null;
		private StatDataUI m_EffectResistanceDataUI = null;
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
		#region WayPoint
		private void OnAddButtonClicked()
		{
			EnemyWayPointDataUI wayPointDataUI = M_MapEditorUI.GetBuilder("Enemy WayPoint Data UI")
				.SetScale(Vector3.one)
				.SetParent(M_MapEditorUI.enemyWayPointDataUIParent)
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
		private static MapEditorUIManager M_MapEditorUI => MapEditorUIManager.Instance;
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
				m_HpDataUI = new StatDataUI(transform, "HP");


			}
			if (m_AtkDataUI == null)
			{
				m_AtkDataUI = new StatDataUI(transform, "ATK");


			}
			if (m_DefDataUI == null)
			{
				m_DefDataUI = new StatDataUI(transform, "DEF");


			}
			if (m_ResDataUI == null)
			{
				m_ResDataUI = new StatDataUI(transform, "RES");


			}
			if (m_MovementSpeedDataUI == null)
			{
				m_MovementSpeedDataUI = new StatDataUI(transform, "Movement Speed");


			}
			if (m_AspdDataUI == null)
			{
				m_AspdDataUI = new StatDataUI(transform, "ASPD");


			}
			if (m_ElementalResDataUI == null)
			{
				m_ElementalResDataUI = new StatDataUI(transform, "Elemental RES");


			}
			if (m_EffectResistanceDataUI == null)
			{
				m_EffectResistanceDataUI = new StatDataUI(transform, "Effect Resistance");


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
			/// 한섭 기준: 지위
			/// 
			/// 값: 일반, 정예, 리더
			string enemyType = EnumUtil.EnumToKorString(enemyData.FixedData.EnemyType);
			m_EnemyTypeImage.sprite = null;

			/// 
			/// 한섭 기준: 종족
			/// 
			/// 값: 감염생물, 드론, 살카즈, 숙주, 바다 괴물, 아츠 피조물, 요괴, 기계, 야생동물, 붕괴체, 기타
			/// 
			string raceText = EnumUtil.EnumToKorString(enemyData.FixedData.RaceType);
			m_RaceText.text = raceText;

			/// 
			/// 한섭 기준: 코드
			/// 
			m_CodeText.text = enemyData.FixedData.Code.ToUpper();

			/// 
			/// 한섭 기준: 이름
			/// 
			m_NameText.text = enemyData.KrName;

			/// 
			/// 한섭 기준: 공격 방식
			/// 
			/// 값: 비공격, 근거리, 원거리
			/// 
			string atkPattern = EnumUtil.EnumToKorString(enemyData.FixedData.AtkPattern);
			/// 
			/// 한섭 기준: 대미지 타입
			/// 
			/// 값: 물리, 마법, 치료, 없음
			/// 
			string dmgType = EnumUtil.EnumToKorString(enemyData.FixedData.DmgType);
			m_AttackInfoText.text = atkPattern + " " + dmgType;

			/// 
			/// 한섭 기준: 목표 생명
			/// 
			m_LifeValueText.text = enemyData.FixedData.LossHp.ToString();
			m_LifeValueText.transform.parent.gameObject.SetActive(enemyData.FixedData.LossHp != 1);
			if (enemyData.FixedData.LossHp == 0)
			{
				//m_LifeTypeImage.sprite = 파란색 라이프 이미지;

				ColorUtility.TryParseHtmlString("#035E88", out Color color);
				m_LifeValueText.color = color;
			}
			else
			{
				//m_LifeTypeImage.sprite = 빨간색 라이프 이미지;

				ColorUtility.TryParseHtmlString("#991517", out Color color);
				m_LifeValueText.color = color;
			}
			#endregion

			#region Datas
			// 초상화
			//m_PortraitImage.sprite = null;

			// 무게
			m_WeightValueText.text = enemyData.FixedData.Weight.InitStat.ToString();

			// 체력
			m_HpDataUI.UpdateUI(enemyData.VariableData.Hp);
			// 공격력
			m_AtkDataUI.UpdateUI(enemyData.VariableData.Atk);
			// 방어력
			m_DefDataUI.UpdateUI(enemyData.VariableData.Def);
			// 마법 저항
			m_ResDataUI.UpdateUI(enemyData.VariableData.Res);
			// 이동 속도
			m_MovementSpeedDataUI.UpdateUI(enemyData.VariableData.MovementSpeed);
			// 공격 속도
			m_AspdDataUI.UpdateUI(enemyData.VariableData.Aspd);
			// 원소 저항
			m_ElementalResDataUI.UpdateUI(enemyData.VariableData.ElementalRes);
			// 피해 저항
			m_EffectResistanceDataUI.UpdateUI(enemyData.VariableData.EffectResistance);
			#endregion

			#region Descriptions
			Transform immuneParent = M_MapEditorUI.enemyImmuneDescriptionParent;

			int count = immuneParent.childCount;
			for (int i = 0; i < count; ++i)
			{
				M_MapEditorUI.Despawn(immuneParent.GetChild<EnemyImmuneOption>(0));
			}

			m_EnemyDescriptionText.text = enemyData.FixedData.Description;
			m_TraitDescriptionText.text = enemyData.FixedData.Trait;

			// 능력 패널 On / Off
			m_TraitDescriptionText.transform.parent.gameObject.SetActive(enemyData.FixedData.Trait != "");
			// 내성 패널 On / Off
			immuneParent.parent.gameObject.SetActive(enemyData.FixedData.ImmuneType > 0);

			string[] immuneKorStringArr = EnumUtil.EnumFlagToKorString(enemyData.FixedData.ImmuneType);

			for (int i = 0; i < immuneKorStringArr.Length; ++i)
			{
				EnemyImmuneOption enemyImmuneOption = M_MapEditorUI.GetBuilder("Enemy Immune Option")
					.SetParent(immuneParent)
					.SetScale(Vector3.one)
					.SetActive(true)
					.SetAutoInit(true)
					.Spawn() as EnemyImmuneOption;

				enemyImmuneOption.text = immuneKorStringArr[i] + " 면역";
			}
			#endregion
		}
		private void UpdateWayPointUI()
		{
			ClearWayPointUI();

			m_CurrentEnemySpawnDataUI.LoadWayPointUI();
		}
		private void ClearWayPointUI()
		{
			RectTransform wayPointDataUIParent = M_MapEditorUI.enemyWayPointDataUIParent;
			int count = wayPointDataUIParent.childCount;
			for (int i = 0; i < count; ++i)
			{
				EnemyWayPointDataUI wayPointDataUI = wayPointDataUIParent.GetChild<EnemyWayPointDataUI>(0);

				if (wayPointDataUI == null)
					continue;

				M_MapEditorUI.Despawn(wayPointDataUI);
			}
		}

		private void SaveEnemyDataUI()
		{
			#region Datas
			EnemyData enemyData = m_CurrentEnemySpawnDataUI.enemyData;

			string hpText = m_HpDataUI.inputField.text.Replace(",", "");
			float.TryParse(hpText, out float hpValue);
			E_EnemyRankType hpRankType = (E_EnemyRankType)m_HpDataUI.rankDropdown.value;
			enemyData.VariableData.Hp = new VariableCombatStatValue<float>(hpValue, hpRankType);

			string atkText = m_AtkDataUI.inputField.text.Replace(",", "");
			float.TryParse(atkText, out float atkValue);
			E_EnemyRankType atkRankType = (E_EnemyRankType)m_AtkDataUI.rankDropdown.value;
			enemyData.VariableData.Atk = new VariableCombatStatValue<float>(atkValue, atkRankType);

			string defText = m_DefDataUI.inputField.text.Replace(",", "");
			float.TryParse(defText, out float defValue);
			E_EnemyRankType defRankType = (E_EnemyRankType)m_DefDataUI.rankDropdown.value;
			enemyData.VariableData.Def = new VariableCombatStatValue<float>(defValue, defRankType);

			string resText = m_ResDataUI.inputField.text.Replace(",", "");
			float.TryParse(resText, out float resValue);
			E_EnemyRankType resRankType = (E_EnemyRankType)m_ResDataUI.rankDropdown.value;
			enemyData.VariableData.Res = new VariableCombatStatValue<float>(resValue, resRankType);

			string movementSpeedText = m_MovementSpeedDataUI.inputField.text.Replace(",", "");
			float.TryParse(movementSpeedText, out float movementSpeedValue);
			E_EnemyRankType movementSpeedRankType = (E_EnemyRankType)m_MovementSpeedDataUI.rankDropdown.value;
			enemyData.VariableData.MovementSpeed = new VariableCombatStatValue<float>(movementSpeedValue, movementSpeedRankType);

			string aspdText = m_AspdDataUI.inputField.text.Replace(",", "");
			float.TryParse(aspdText, out float aspdValue);
			E_EnemyRankType aspdRankType = (E_EnemyRankType)m_AspdDataUI.rankDropdown.value;
			enemyData.VariableData.Aspd = new VariableCombatStatValue<float>(aspdValue, aspdRankType);

			string elementalResText = m_ElementalResDataUI.inputField.text.Replace(",", "");
			float.TryParse(elementalResText, out float elementalResValue);
			E_EnemyRankType elementalResRankType = (E_EnemyRankType)m_ElementalResDataUI.rankDropdown.value;
			enemyData.VariableData.ElementalRes = new VariableCombatStatValue<float>(elementalResValue, elementalResRankType);

			string effectResistanceText = m_EffectResistanceDataUI.inputField.text.Replace(",", "");
			float.TryParse(effectResistanceText, out float effectResistanceValue);
			E_EnemyRankType effectResistanceRankType = (E_EnemyRankType)m_EffectResistanceDataUI.rankDropdown.value;
			enemyData.VariableData.EffectResistance = new VariableCombatStatValue<float>(effectResistanceValue, effectResistanceRankType);
			#endregion

			#region WayPoints
			m_CurrentEnemySpawnDataUI.SaveWayPointUI();
			#endregion
		}

		private class StatDataUI
		{
			public TMP_InputField inputField { get; }
			[System.Obsolete("현재는 사용X, 이후 추가 가능성 있음")]
			public TMP_Text rankText { get; }
			public TMP_Dropdown rankDropdown { get; }

			public StatDataUI(Transform transform, string dataName)
			{
				inputField = transform.FindInChildren<TMP_InputField>(dataName + " InputField");
				inputField.onValueChanged.AddListener(OnInputFieldValueChanged);

				rankText = transform.FindInChildren<TMP_Text>(dataName + " Rank Text");
				rankDropdown = transform.FindInChildren<TMP_Dropdown>(dataName + " Rank Dropdown");
			}

			private void OnInputFieldValueChanged(string inputString)
			{
				//int.TryParse(inputString.Replace(",", ""), out int inputValue);
				//inputField.text = string.Format("{0:#,###}", inputValue);
				//inputField.stringPosition = inputField.text.Length;

				int stringPosition = inputField.selectionStringAnchorPosition;

				float.TryParse(inputString, out float inputValue);
				inputField.SetTextWithoutNotify(inputValue.ToString("#,###"));

				int stringOffset = 0; //inputField.text.Split(',').Length - 1;

				inputField.selectionStringAnchorPosition = stringPosition + stringOffset;
				inputField.selectionStringFocusPosition = stringPosition + stringOffset;
				//inputField.characterLimit
			}
			public void UpdateUI(VariableCombatStatValue<float> statValue)
			{
				inputField.SetTextWithoutNotify(statValue.InitStat.ToString("N"));
				rankText.text = statValue.Rank.ToString().Replace("plus", "+");
				rankDropdown.value = (int)statValue.Rank;
			}
		}
	}
}