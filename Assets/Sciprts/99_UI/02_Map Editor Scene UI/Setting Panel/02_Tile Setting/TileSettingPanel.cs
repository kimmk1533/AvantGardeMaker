using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace;
using AvantGardeMaker.TileSpace.Enum;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class TileSettingPanel : SettingPanel
	{
		#region 기본 템플릿
		#region 변수
		private ScrollRect m_TileSettingScrollRect = null;
		private TMP_Dropdown m_TileTypeSettingDropdown = null;
		private TMP_Dropdown m_TilePositionTypeSettingDropdown = null;
		private Toggle m_LowDeployableToggle = null;
		private Toggle m_HighDeployableToggle = null;

		private ScrollRect m_TileDataUIScrollRect = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트

		#region 이벤트 함수
		private void OnTileTypeSettingDropdownValueChanged(int value)
		{
			E_TileType tileType = (E_TileType)value;

			M_MapEditing.tileType = tileType;

			if (tileType == E_TileType.Decoration)
			{
				m_LowDeployableToggle.isOn = false;
				m_HighDeployableToggle.isOn = false;
			}

			m_LowDeployableToggle.interactable = tileType != E_TileType.Decoration;
			m_HighDeployableToggle.interactable = tileType != E_TileType.Decoration;
		}
		private void OnTilePositionTypeSettingDropdwonValueChanged(int value)
		{
			M_MapEditing.tilePositionType = (E_TilePositionType)value;
		}
		private void OnLowDeployableTypeFlagToggleValueChanged(bool value)
		{
			if (M_MapEditing.HasTileDeployableTypeFlag(E_TileDeployableTypeFlag.LowDeployable) == true)
				M_MapEditing.RemoveTileDeployableTypeFlag(E_TileDeployableTypeFlag.LowDeployable);
			else
				M_MapEditing.AddTileDeployableTypeFlag(E_TileDeployableTypeFlag.LowDeployable);
		}
		private void OnHighDeployableTypeFlagToggleValueChanged(bool value)
		{
			if (M_MapEditing.HasTileDeployableTypeFlag(E_TileDeployableTypeFlag.HighDeployable) == true)
				M_MapEditing.RemoveTileDeployableTypeFlag(E_TileDeployableTypeFlag.HighDeployable);
			else
				M_MapEditing.AddTileDeployableTypeFlag(E_TileDeployableTypeFlag.HighDeployable);
		}
		#endregion
		#endregion

		#region 매니저
		private static MapEditingManager M_MapEditing => MapEditingManager.Instance;
		private static MapEditingSceneUIManager M_MapEditingUI => MapEditingSceneUIManager.Instance;
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

			m_TileSettingScrollRect = transform.Find<ScrollRect>("Tile Setting Scroll Rect");
			m_TileTypeSettingDropdown = m_TileSettingScrollRect.content.Find<TMP_Dropdown>("Tile Type Setting/Dropdown");
			m_TilePositionTypeSettingDropdown = m_TileSettingScrollRect.content.Find<TMP_Dropdown>("Tile Position Type Setting/Dropdown");
			m_LowDeployableToggle = m_TileSettingScrollRect.content.Find<Toggle>("Tile Deployable Type Flag Setting/Low Deployable Toggle");
			m_HighDeployableToggle = m_TileSettingScrollRect.content.Find<Toggle>("Tile Deployable Type Flag Setting/High Deployable Toggle");

			m_TileDataUIScrollRect = transform.Find<ScrollRect>("Tile Data UI Scroll Rect");

			m_TileTypeSettingDropdown.onValueChanged.AddListener(OnTileTypeSettingDropdownValueChanged);
			m_TilePositionTypeSettingDropdown.onValueChanged.AddListener(OnTilePositionTypeSettingDropdwonValueChanged);
			m_LowDeployableToggle.onValueChanged.AddListener(OnLowDeployableTypeFlagToggleValueChanged);
			m_HighDeployableToggle.onValueChanged.AddListener(OnHighDeployableTypeFlagToggleValueChanged);
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();

			m_TileTypeSettingDropdown.onValueChanged.RemoveAllListeners();
			m_TilePositionTypeSettingDropdown.onValueChanged.RemoveAllListeners();
			m_LowDeployableToggle.onValueChanged.RemoveAllListeners();
			m_HighDeployableToggle.onValueChanged.RemoveAllListeners();
		}
		#endregion
		#endregion
	}
}