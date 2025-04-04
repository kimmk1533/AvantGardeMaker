using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.ad1a;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class EnemySpawnDataUI : MapEditorUI
	{
		#region 변수
		private Button m_OptionButton = null;
		private TMP_Text m_EnemyNameText = null;
		private TMP_InputField m_IntervalInputField = null;
		private TMP_InputField m_TimeStampInputField = null;
		private TMP_InputField m_WaveTimeInputField = null;
		private Button m_DeleteButton = null;

		private List<Vector2> m_EnemyWayPointList = null;
		#endregion

		#region 프로퍼티
		public EnemyData enemyData { get; set; }
		public EnemySpawnData enemySpawnData { get; set; }
		public List<Vector2> enemyWayPointList => m_EnemyWayPointList;
		#endregion

		#region 이벤트

		#region 이벤트 함수
		private void OnOptionButtonClicked()
		{
			EnemyDataSettingPanel settingPanel = M_MapEditorUI.enemyDataSettingPanel;

			settingPanel.SetEnemySpawnDataUI(this);

			settingPanel.gameObject.SetActive(true);
		}

		private void OnInputFieldFocused(string inputString, TMP_InputField inputField)
		{
			int index = inputString.IndexOf('s');
			if (index == -1)
				return;

			int count = inputString.LastIndexOf('s') - index + 1;

			inputField.text = inputString.Remove(index, count);
		}
		private void OnIntervalInputFieldFocused(string inputString)
		{
			OnInputFieldFocused(inputString, m_IntervalInputField);
		}
		private void OnTimeStampInputFieldFocused(string inputString)
		{
			OnInputFieldFocused(inputString, m_TimeStampInputField);
		}
		private void OnWaveTimeInputFieldFocused(string inputString)
		{
			OnInputFieldFocused(inputString, m_WaveTimeInputField);
		}

		private void OnInputFieldUnfocused(string inputString, TMP_InputField inputField)
		{
			if (inputField.text.EndsWith('s') == true)
				return;

			inputField.text = inputString + "s";
		}
		private void OnIntervalInputFieldUnfocused(string inputString)
		{
			OnInputFieldUnfocused(inputString, m_IntervalInputField);
		}
		private void OnTimeStampInputFieldUnfocused(string inputString)
		{
			OnInputFieldUnfocused(inputString, m_TimeStampInputField);
		}
		private void OnWaveTimeInputFieldUnfocused(string inputString)
		{
			OnInputFieldUnfocused(inputString, m_WaveTimeInputField);
		}

		private void OnDeleteButtonClicked()
		{
			M_MapEditorUI.Despawn(this);
		}
		#endregion
		#endregion

		#region 매니저
		private static MapEditorUIManager M_MapEditorUI => MapEditorUIManager.Instance;
		private static EnemyManager M_Enemy => EnemyManager.Instance;
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

			if (m_OptionButton == null)
			{
				m_OptionButton = transform.Find<Button>("Option Button");

				m_OptionButton.onClick.AddListener(OnOptionButtonClicked);
			}
			if (m_EnemyNameText == null)
			{
				m_EnemyNameText = transform.Find<TMP_Text>("Enemy Name Text");
			}
			if (m_IntervalInputField == null)
			{
				m_IntervalInputField = transform.Find<TMP_InputField>("Interval InputField");

				m_IntervalInputField.onSelect.AddListener(OnIntervalInputFieldFocused);
				m_IntervalInputField.onDeselect.AddListener(OnIntervalInputFieldUnfocused);
				m_IntervalInputField.onEndEdit.AddListener(OnIntervalInputFieldUnfocused);
			}
			if (m_TimeStampInputField == null)
			{
				m_TimeStampInputField = transform.Find<TMP_InputField>("Time Stamp InputField");

				m_TimeStampInputField.onSelect.AddListener(OnTimeStampInputFieldFocused);
				m_TimeStampInputField.onDeselect.AddListener(OnTimeStampInputFieldUnfocused);
				m_TimeStampInputField.onEndEdit.AddListener(OnTimeStampInputFieldUnfocused);
			}
			if (m_WaveTimeInputField == null)
			{
				m_WaveTimeInputField = transform.Find<TMP_InputField>("Wave Time InputField");

				m_WaveTimeInputField.onSelect.AddListener(OnWaveTimeInputFieldFocused);
				m_WaveTimeInputField.onDeselect.AddListener(OnWaveTimeInputFieldUnfocused);
				m_WaveTimeInputField.onEndEdit.AddListener(OnWaveTimeInputFieldUnfocused);
			}
			if (m_DeleteButton == null)
			{
				m_DeleteButton = transform.Find<Button>("Delete Button");

				m_DeleteButton.onClick.AddListener(OnDeleteButtonClicked);
			}

			if (m_EnemyWayPointList == null)
				m_EnemyWayPointList = new List<Vector2>();
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void FinallizePoolItem()
		{
			base.FinallizePoolItem();

			//for (int i = 0; i < m_EnemyWayPointDataUIList.Count; ++i)
			//{
			//	M_MapEditorUI.Despawn(m_EnemyWayPointDataUIList[i]);
			//}
			m_EnemyWayPointList.Clear();
		}
		#endregion
	}
}