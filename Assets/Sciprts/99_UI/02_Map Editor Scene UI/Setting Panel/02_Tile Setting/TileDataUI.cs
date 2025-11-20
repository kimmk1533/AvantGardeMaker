using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace;
using AvantGardeMaker.TileSpace;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class TileDataUI : MapEditingSceneUIPoolItem
	{
		#region 기본 템플릿
		#region 변수
		private TileData m_TileData = null;

		private Button m_Button = null;

		private TextMeshProUGUI m_TileTypeText = null;
		#endregion

		#region 프로퍼티
		public TileData tileData
		{
			get => m_TileData;
			set
			{
				m_TileData = value;

				m_TileTypeText.text = value.KorName;
			}
		}
		#endregion

		#region 이벤트

		#region 이벤트 함수
		private void OnButtonClicked()
		{
			if (M_MapEditing.tileKey == m_TileData.key)
				M_MapEditing.tileKey = string.Empty;
			else
				M_MapEditing.tileKey = m_TileData.key;
		}
		#endregion
		#endregion

		#region 매니저
		private static MapEditingManager M_MapEditing => MapEditingManager.Instance;
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

			m_Button = GetComponent<Button>();
			m_TileTypeText = transform.Find<TextMeshProUGUI>("Tile Type Text");

			m_Button.onClick.AddListener(OnButtonClicked);
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void FinallizePoolItem()
		{
			base.FinallizePoolItem();


		}
		#endregion
		#endregion
	}
}