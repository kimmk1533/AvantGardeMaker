using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class MapEditingSceneEventController : SceneEventController
	{
		#region 변수
		#region UI Button 관련 변수
		[SerializeField]
		private Button m_MainMenuButton = null;
		[SerializeField]
		private Button m_SaveButton = null;
		[SerializeField]
		private Button m_PlayButton = null;
		#endregion

		#region Map Editor Manager 관련 변수
		[SerializeField]
		private Camera m_MapEditorCamera = null;
		[SerializeField]
		private Camera m_ThumnailCamera = null;

		[SerializeField]
		private Transform m_EditModeCameraTransform = null;
		[SerializeField]
		private Transform m_GameModeCameraTransform = null;
		#endregion

		#region Map Editor UI Manager 관련 변수
		[SerializeField]
		private MenuPanel m_MenuPanel = null;
		[SerializeField]
		private OptionPanel m_OptionPanel = null;

		[SerializeField]
		private EnemyDataSettingPanel m_EnemyDataSettingPanel = null;

		[SerializeField]
		private RectTransform m_EnemySpawnDataUIParent = null;
		[SerializeField]
		private RectTransform m_EnemyDataUIParent = null;
		[SerializeField]
		private RectTransform m_EnemyWayPointDataUIParent = null;
		[SerializeField]
		private RectTransform m_EnemyImmuneDescriptionParent = null;
		#endregion
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트

		#region 이벤트 함수
		#endregion
		#endregion

		#region 매니저
		private static GameManager M_Game => GameManager.Instance;

		private static MapEditingManager M_MapEditing => MapEditingManager.Instance;
		private static MapEditingUIManager M_MapEditingUI => MapEditingUIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		protected override void OnApplicationQuit()
		{
			M_Game.FinallizeMapEditing();

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

			#region 멤버 변수 링킹
			M_MapEditing.mapEditorCamera = m_MapEditorCamera;
			M_MapEditing.thumnailCamera = m_ThumnailCamera;

			M_MapEditing.editModeCameraTransform = m_EditModeCameraTransform;
			M_MapEditing.gameModeCameraTransform = m_GameModeCameraTransform;

			M_MapEditingUI.mainMenuButton = m_MainMenuButton;
			M_MapEditingUI.saveButton = m_SaveButton;
			M_MapEditingUI.playButton = m_PlayButton;

			M_MapEditingUI.menuPanel = m_MenuPanel;
			M_MapEditingUI.optionPanel = m_OptionPanel;

			M_MapEditingUI.enemyDataSettingPanel = m_EnemyDataSettingPanel;

			M_MapEditingUI.enemySpawnDataUIParent = m_EnemySpawnDataUIParent;
			M_MapEditingUI.enemyDataUIParent = m_EnemyDataUIParent;
			M_MapEditingUI.enemyWayPointDataUIParent = m_EnemyWayPointDataUIParent;
			M_MapEditingUI.enemyImmuneDescriptionParent = m_EnemyImmuneDescriptionParent;
			#endregion

			AddBeforeEvent("Main Menu Scene", M_Game.FinallizeMapEditing);
			AddBeforeEvent("Game Playing Scene", M_Game.FinallizeMapEditing);

			AddAfterEvent("Main Menu Scene", M_Game.InitializeMainMenu);
			AddAfterEvent("Game Playing Scene", M_Game.InitializeGamePlaying);
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