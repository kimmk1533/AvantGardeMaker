using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class MainMenuSceneLoadingEventReceiver : SceneEventReceiver
	{
		#region 변수
		[SerializeField]
		private Button m_MapListButton = null;
		[SerializeField]
		private Button m_MapEditorButton = null;
		[SerializeField]
		private Button m_OptionButton = null;
		[SerializeField]
		private Button m_QuitButton = null;

		[SerializeField]
		private MapListPanel m_MapListPanel = null;
		[SerializeField]
		private RectTransform m_OptionPanel = null;

		[SerializeField]
		private RectTransform m_MapListItemParent = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		private static GameManager M_Game => GameManager.Instance;
		private static MainMenuUIManager M_MainMenuUI => MainMenuUIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			M_MainMenuUI.mapListButton = m_MapListButton;
			M_MainMenuUI.mapEditorButton = m_MapEditorButton;
			M_MainMenuUI.optionButton = m_OptionButton;
			M_MainMenuUI.quitButton = m_QuitButton;

			M_MainMenuUI.mapListPanel = m_MapListPanel;
			M_MainMenuUI.optionPanel = m_OptionPanel;

			M_MainMenuUI.mapListItemParent = m_MapListItemParent;

			M_Game.InitializeMainMenu();
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		protected override void Finallize()
		{
			base.Finallize();

		}
		#endregion
	}
}