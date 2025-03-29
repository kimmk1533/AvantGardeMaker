using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.Ceeu.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class MapEditorUIManager : SerializedSingleton<MapEditorUIManager>
	{
		#region 변수
		[SerializeField]
		private List<string> m_KeyList = new List<string>();

		#region 메뉴 패널 관련 변수
		private bool m_CanUseMenuShortcut = true;
		#endregion

		#region 옵션 패널 관련 변수

		#endregion
		#endregion

		#region 프로퍼티
		public List<string> keyList => m_KeyList;

		#region 메뉴 패널 관련 프로퍼티
		public MenuPanel menuPanel { get; set; }
		public MenuButtonController menuButtonController => menuPanel.menuButtonController;
		#endregion

		#region 옵션 패널 관련 프로퍼티
		public OptionPanel optionPanel { get; set; }
		#endregion
		#endregion

		#region 이벤트

		#region 이벤트 함수
		public void OnMapNameInputFieldFocused(string inputString)
		{
			m_CanUseMenuShortcut = false;
		}
		public void OnMapNameInputFieldUnfocused(string inputString)
		{
			m_CanUseMenuShortcut = true;
		}
		#endregion
		#endregion

		#region 매니저
		private static MapEditorManager M_EditMode => MapEditorManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			MenuShortcut();
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public virtual void Initialize()
		{

			gameObject.SetActive(false);
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public virtual void Finallize()
		{
		}

		/// <summary>
		/// 게임 초기화 함수 (본인 Main Scene 진입 시 호출)
		/// </summary>
		public virtual void InitializeMain()
		{
			menuPanel.Initialize();

			optionPanel.Initialize();

			gameObject.SetActive(true);
		}
		/// <summary>
		/// 게임 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public virtual void FinallizeMain()
		{
			menuPanel.Finallize();
		}
		#endregion

		private void MenuShortcut()
		{
			if (m_CanUseMenuShortcut == false)
				return;

			foreach (string key in m_KeyList)
			{
				OptionViewport optionViewport = optionPanel.optionViewportController[key];
				KeyCode keyCode = optionViewport.shortcut;
				if (Input.GetKeyDown(keyCode) == true)
				{
					optionViewport.OnMenuButtonClicked();
					M_EditMode.SetEditModeType(optionViewport.editModeType);
				}
			}
		}
	}
}