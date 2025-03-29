using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class EnemySpawnDataUI : ObjectPoolItemBase
	{
		#region 변수
		private RectTransform m_RectTransform = null;

		[SerializeField]
		private TMP_InputField m_IntervalInputField = null;
		[SerializeField]
		private TMP_InputField m_TimeStampInputField = null;
		[SerializeField]
		private TMP_InputField m_WaveTimeInputField = null;
		[SerializeField]
		private Button m_DeleteButton = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트

		#region 이벤트 함수
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
			M_EnemySpawnDataUI.Despawn(this);
		}
		#endregion
		#endregion

		#region 매니저
		private static EnemySpawnDataUIManager M_EnemySpawnDataUI => EnemySpawnDataUIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{
			m_RectTransform = transform as RectTransform;

			if (m_IntervalInputField == null)
				m_IntervalInputField = transform.Find<TMP_InputField>("Interval InputField");
			if (m_TimeStampInputField == null)
				m_TimeStampInputField = transform.Find<TMP_InputField>("Time Stamp InputField");
			if (m_WaveTimeInputField == null)
				m_WaveTimeInputField = transform.Find<TMP_InputField>("Wave Time InputField");
			if (m_DeleteButton == null)
				m_DeleteButton = transform.Find("Delete Button").Find<Button>("Button");

			m_IntervalInputField.onSelect.AddListener(OnIntervalInputFieldFocused);
			m_IntervalInputField.onDeselect.AddListener(OnIntervalInputFieldUnfocused);
			m_IntervalInputField.onEndEdit.AddListener(OnIntervalInputFieldUnfocused);

			m_TimeStampInputField.onSelect.AddListener(OnTimeStampInputFieldFocused);
			m_TimeStampInputField.onDeselect.AddListener(OnTimeStampInputFieldUnfocused);
			m_TimeStampInputField.onEndEdit.AddListener(OnTimeStampInputFieldUnfocused);

			m_WaveTimeInputField.onSelect.AddListener(OnWaveTimeInputFieldFocused);
			m_WaveTimeInputField.onDeselect.AddListener(OnWaveTimeInputFieldUnfocused);
			m_WaveTimeInputField.onEndEdit.AddListener(OnWaveTimeInputFieldUnfocused);

			m_DeleteButton.onClick.AddListener(OnDeleteButtonClicked);
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{

		}
		#endregion
	}
}