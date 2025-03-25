using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.Ceeu.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class UIManager : SerializedSingleton<UIManager>
	{
		#region 변수
		#region 메뉴 패널 관련 변수
		[SerializeField]
		private MenuPanel m_MenuPanel = null;
		private bool m_MenuPanelMovingLock = false;
		#endregion

		#region 옵션 패널 관련 변수
		[SerializeField]
		private OptionPanel m_OptionPanel = null;
		#endregion
		#endregion

		#region 프로퍼티
		public bool menuPanelLock
		{
			get => m_MenuPanelMovingLock;
			set => m_MenuPanelMovingLock = value;
		}
		#endregion

		#region 이벤트
		public Button.ButtonClickedEvent onSystemMenuButtonClicked => m_MenuPanel.onSystemMenuButtonClicked;
		public Button.ButtonClickedEvent onTileMenuButtonClicked => m_MenuPanel.onTileMenuButtonClicked;
		public Button.ButtonClickedEvent onOperatorMenuButtonClicked => m_MenuPanel.onOperatorMenuButtonClicked;
		public Button.ButtonClickedEvent onEnemyMenuButtonClicked => m_MenuPanel.onEnemyMenuButtonClicked;

		#region 이벤트 함수
		public void OnMenuLockButtonClicked()
		{
			m_MenuPanelMovingLock = !m_MenuPanelMovingLock;
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
				m_OptionPanel.systemOptionViewport.OnMenuButtonClicked();
			else if (Input.GetKeyDown(KeyCode.Alpha2) == true)
				m_OptionPanel.tileOptionViewport.OnMenuButtonClicked();
			else if (Input.GetKeyDown(KeyCode.Alpha3) == true)
				m_OptionPanel.operatorOptionViewport.OnMenuButtonClicked();
			else if (Input.GetKeyDown(KeyCode.Alpha4) == true)
				m_OptionPanel.enemyOptionViewport.OnMenuButtonClicked();
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public virtual void Initialize()
		{
			if (m_MenuPanel == null)
				throw new System.NullReferenceException("m_MenuPanel is null.");
			if (m_OptionPanel == null)
				throw new System.NullReferenceException("m_OptionPanel is null.");
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public virtual void Finallize()
		{
		}

		/// <summary>
		/// 게임 초기화 함수 (Game Scene 진입 시 호출)
		/// </summary>
		public virtual void InitializeGame()
		{
			m_MenuPanel.Initialize();

			m_MenuPanel.OnPointerEnter(null);
			m_MenuPanelMovingLock = true;

			m_OptionPanel.Initialize();
		}
		/// <summary>
		/// 게임 마무리화 함수 (Game Scene 나갈 시 호출)
		/// </summary>
		public virtual void FinallizeGame()
		{
			m_MenuPanel.Finallize();
		}
		#endregion
	}
}