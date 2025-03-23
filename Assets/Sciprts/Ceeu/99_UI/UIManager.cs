using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.Ceeu
{
	public class UIManager : SerializedSingleton<UIManager>
	{
		#region 변수
		[SerializeField]
		private MenuPanel m_MenuPanel = null;
		private bool m_MenuPanelMovingLock = false;

		[SerializeField]
		private RectTransform m_OptionPanel = null;

		[SerializeField]
		private RectTransform m_CurrentViewPort = null;
		private RectTransform m_TileOptionViewPort = null;
		private RectTransform m_EnemyOptionViewPort = null;
		#endregion

		#region 프로퍼티
		public bool menuPanelLock
		{
			get => m_MenuPanelMovingLock;
			set => m_MenuPanelMovingLock = value;
		}
		#endregion

		#region 이벤트

		#region 이벤트 함수
		public void OnCursorMenuButtonClicked()
		{
			M_EditMode.SetEditModeType(Enum.E_EditModeType.Cursor);

			OnOptionCloseButtonClicked();
		}
		public void OnTileMenuButtonClicked()
		{
			M_EditMode.SetEditModeType(Enum.E_EditModeType.Tile);

			if (m_CurrentViewPort != null)
				m_CurrentViewPort.gameObject.SetActive(false);

			if (m_OptionPanel.gameObject.activeSelf == false)
				ToggleOptionPanelActive();
			else if (m_CurrentViewPort == m_TileOptionViewPort)
			{
				m_CurrentViewPort = null;
				ToggleOptionPanelActive();
				return;
			}

			m_CurrentViewPort = m_TileOptionViewPort;
			m_CurrentViewPort.gameObject.SetActive(true);
		}
		public void OnEnemyMenuButtonClicked()
		{
			M_EditMode.SetEditModeType(Enum.E_EditModeType.Enemy);

			if (m_CurrentViewPort != null)
				m_CurrentViewPort.gameObject.SetActive(false);

			if (m_OptionPanel.gameObject.activeSelf == false)
				ToggleOptionPanelActive();
			else if (m_CurrentViewPort == m_EnemyOptionViewPort)
			{
				m_CurrentViewPort = null;
				ToggleOptionPanelActive();
				return;
			}

			m_CurrentViewPort = m_EnemyOptionViewPort;
			m_CurrentViewPort.gameObject.SetActive(true);
		}

		public void ToggleMenuPanelLocking()
		{
			m_MenuPanelMovingLock = !m_MenuPanelMovingLock;
		}
		private void ToggleOptionPanelActive()
		{
			m_OptionPanel.gameObject.SetActive(!m_OptionPanel.gameObject.activeSelf);
		}
		public void OnOptionCloseButtonClicked()
		{
			m_OptionPanel.gameObject.SetActive(false);

			if (m_CurrentViewPort != null)
				m_CurrentViewPort.gameObject.SetActive(false);
			m_CurrentViewPort = null;
		}
		#endregion
		#endregion

		#region 매니저
		private static EditModeManager M_EditMode => EditModeManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1) == true)
				OnCursorMenuButtonClicked();
			else if (Input.GetKeyDown(KeyCode.Alpha2) == true)
				OnTileMenuButtonClicked();
			else if (Input.GetKeyDown(KeyCode.Alpha3) == true)
				OnEnemyMenuButtonClicked();
		}
		#endregion

		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public virtual void Initialize()
		{
			if (m_MenuPanel == null)
				throw new System.NullReferenceException("m_MenuPanel is null.");
			if (m_OptionPanel == null)
				throw new System.NullReferenceException("m_OptionPanel is null.");

			m_CurrentViewPort = null;
			m_TileOptionViewPort = m_OptionPanel.FindInChilderen("Tile Option Viewport");
			m_TileOptionViewPort.gameObject.SetActive(false);
			m_EnemyOptionViewPort = m_OptionPanel.FindInChilderen("Enemy Option Viewport");
			m_EnemyOptionViewPort.gameObject.SetActive(false);
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public virtual void Finallize()
		{
			m_EnemyOptionViewPort = null;
			m_TileOptionViewPort = null;
		}

		/// <summary>
		/// 게임 초기화 함수 (Game Scene 진입 시 호출)
		/// </summary>
		public virtual void InitializeGame()
		{
			m_MenuPanel.Initialize();

			m_MenuPanel.OnPointerEnter(null);
			m_MenuPanelMovingLock = true;
		}
		/// <summary>
		/// 게임 마무리화 함수 (Game Scene 나갈 시 호출)
		/// </summary>
		public virtual void FinallizeGame()
		{
			m_MenuPanel.Finallize();
		}

	}
}