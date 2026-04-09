using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.UI;
using CoreSources;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.CoreSpace.SceneEvent
{
	public class MainMenuSceneEventController : SceneEventController
	{
		#region 변수
		[SerializeField]
		private RectTransform m_MainMenuInitPanel = null;
		[SerializeField]
		private RectTransform m_MainMenuButtonsPanel = null;

		[SerializeField]
		private MapListPanel m_MapListPanel = null;
		[SerializeField]
		private MapOptionPanel m_MapOptionPanel = null;
		[SerializeField]
		private RectTransform m_SystemOptionPanel = null;

		[SerializeField]
		private RectTransform m_MapListItemParent = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트

		#region 이벤트 함수
		#endregion
		#endregion

		#region 매니저
		private static GameManager M_Game => GameManager.Instance;

		private static MainMenuSceneUIManager M_MainMenuUI => MainMenuSceneUIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		protected override void OnApplicationQuit()
		{
			M_Game.FinallizeMainMenuScene();

			M_Game.Finallize();
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			#region 변수 링킹
			M_MainMenuUI.mainMenuInitPanel = m_MainMenuInitPanel;
			M_MainMenuUI.mainMenuButtonsPanel = m_MainMenuButtonsPanel;

			M_MainMenuUI.mapListPanel = m_MapListPanel;
			M_MainMenuUI.mapOptionPanel = m_MapOptionPanel;
			M_MainMenuUI.systemOptionPanel = m_SystemOptionPanel;

			M_MainMenuUI.mapListItemParent = m_MapListItemParent;
			#endregion

			// Map Editing Scene 전환 전 이벤트
			AddBeforeEvent("Map Editing Scene", M_Game.FinallizeMainMenuScene);

			// Map Editing Scene 전환 후 이벤트
			AddAfterEvent("Map Editing Scene", M_Game.InitializeMapEditingScene);

			// Game Playing Scene 전환 전 이벤트
			AddBeforeEvent("Game Playing Scene", M_Game.FinallizeMainMenuScene);

			// Game Playing Scene 전환 후 이벤트
			AddAfterEvent("Game Playing Scene", M_Game.InitializeGamePlayingScene);
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