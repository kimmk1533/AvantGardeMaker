using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class EnemyWayPointDataUI : MapEditorUI
	{
		#region 변수
		private Button m_TileSelectButton = null;
		private TMP_InputField m_XInputField = null;
		private TMP_InputField m_YInputField = null;
		private TMP_InputField m_DelayTimeInputField = null;
		private Button m_DeleteButton = null;
		#endregion

		#region 프로퍼티
		public float x
		{
			get
			{
				if (m_XInputField.text == string.Empty)
					return 0f;

				if (float.TryParse(m_XInputField.text, out float xValue) == false)
					throw new System.Exception("Enemy WayPoint x InputField의 값을 float로 변환 하는데 실패");

				return xValue;
			}
			set
			{
				m_XInputField.text = value.ToString();
			}
		}
		public float y
		{
			get
			{
				if (m_YInputField.text == string.Empty)
					return 0f;

				if (float.TryParse(m_YInputField.text, out float yValue) == false)
					throw new System.Exception("Enemy WayPoint y InputField의 값을 float로 변환 하는데 실패");

				return yValue;
			}
			set
			{
				m_YInputField.text = value.ToString();
			}
		}
		public Vector2 position
		{
			get => new Vector2(x, y);
			set
			{
				x = value.x;
				y = value.y;
			}
		}
		public float delayTime
		{
			get => float.Parse(m_DelayTimeInputField.text);
			set => m_DelayTimeInputField.SetTextWithoutNotify(value.ToString());
		}
		#endregion

		#region 이벤트

		#region 이벤트 함수
		private void OnDeleteButtonClicked()
		{
			M_MapEditorUI.Despawn(this);
		}
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
		public override void InitializePoolItem()
		{
			base.InitializePoolItem();

			if (m_TileSelectButton == null)
			{
				m_TileSelectButton = transform.Find("Tile Select Button").Find<Button>("Button");
			}
			if (m_XInputField == null)
			{
				m_XInputField = transform.FindInChildren<TMP_InputField>("X InputField");
			}
			if (m_YInputField == null)
			{
				m_YInputField = transform.FindInChildren<TMP_InputField>("Y InputField");
			}
			if (m_DelayTimeInputField == null)
			{
				m_DelayTimeInputField = transform.FindInChildren<TMP_InputField>("DelayTime InputField");
			}
			if (m_DeleteButton == null)
			{
				m_DeleteButton = transform.Find<Button>("Delete Button");

				m_DeleteButton.onClick.AddListener(OnDeleteButtonClicked);
			}
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void FinallizePoolItem()
		{
			base.FinallizePoolItem();

			m_XInputField.text = string.Empty;
			m_YInputField.text = string.Empty;
			m_DelayTimeInputField.text = string.Empty;
		}
		#endregion
	}
}