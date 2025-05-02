using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace;
using AvantGardeMaker.TileSpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class TileSettingPanel : SettingPanel
	{
		#region 기본 템플릿
		#region 변수
		private RectTransform m_TileSettingButtonParent = null;
		#endregion

		#region 프로퍼티
		private RectTransform tileSettingButtonParent
		{
			get
			{
				if (m_TileSettingButtonParent == null)
					m_TileSettingButtonParent = M_MapEditingUI.tileSettingButtonParent;

				return m_TileSettingButtonParent;
			}
		}
		#endregion

		#region 이벤트

		#region 이벤트 함수
		private void OnTileSettingButtonClicked(E_TileType tileType)
		{
			M_MapEditing.SetTileType(tileType);
		}
		#endregion
		#endregion

		#region 매니저
		private static MapEditingManager M_MapEditing => MapEditingManager.Instance;
		private static MapEditingUIManager M_MapEditingUI => MapEditingUIManager.Instance;
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

			int childCount = tileSettingButtonParent.childCount;
			for (int i = 0; i < childCount; ++i)
			{
				Button settingButton = tileSettingButtonParent.GetChild<Button>(i);

				E_TileType tileType = (E_TileType)i;

				settingButton.onClick.AddListener(() => OnTileSettingButtonClicked(tileType));
			}
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();
		}
		#endregion
		#endregion
	}
}