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
		private Button m_DeleteButton = null;
		#endregion

		#region 프로퍼티
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


		}
		#endregion
	}
}