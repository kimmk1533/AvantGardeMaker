using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.CoreSpace.SceneEvent
{
	public class MapEditingSceneEventController : SceneEventController
	{
		#region 변수
		#region 버튼 관련 변수
		[SerializeField]
		[FoldoutGroup("버튼 관련")]
		private Button m_MainMenuButton = null;
		[SerializeField]
		[FoldoutGroup("버튼 관련")]
		private Button m_SaveButton = null;
		[SerializeField]
		[FoldoutGroup("버튼 관련")]
		private Button m_PlayButton = null;
		#endregion

		#region 카메라 관련 변수
		[SerializeField]
		[FoldoutGroup("카메라 관련")]
		private Camera m_MapEditorCamera = null;
		[SerializeField]
		[FoldoutGroup("카메라 관련")]
		private Camera m_ThumnailCamera = null;

		[SerializeField]
		[FoldoutGroup("카메라 관련")]
		private Transform m_EditViewCameraTransform = null;
		[SerializeField]
		[FoldoutGroup("카메라 관련")]
		private Transform m_GameViewCameraTransform = null;
		#endregion

		#region 컨트롤러 관련 변수
		[SerializeField]
		[FoldoutGroup("컨트롤러")]
		private MenuPanelController m_MenuPanelController = null;
		[SerializeField]
		[FoldoutGroup("컨트롤러")]
		private SettingPanelController m_SettingPanelController = null;
		#endregion

		#region 타일 설정 관련 변수
		[SerializeField]
		[FoldoutGroup("설정 관련")]
		[FoldoutGroup("설정 관련/타일")]
		private RectTransform m_TileDataUIContent = null;
		#endregion

		#region 오퍼레이터 설정 관련 변수
		[SerializeField]
		[FoldoutGroup("설정 관련/오퍼레이터")]
		private RectTransform m_OperatorDataUIContent = null;
		#endregion

		#region 적 설정 관련 변수
		[SerializeField]
		[FoldoutGroup("설정 관련/적")]
		private RectTransform m_EnemyDataUIContent = null;
		[SerializeField]
		[FoldoutGroup("설정 관련/적")]
		private RectTransform m_EnemySpawnDataUIParent = null;
		[SerializeField]
		[FoldoutGroup("설정 관련/적")]
		private RectTransform m_EnemyWayPointDataUIParent = null;
		[SerializeField]
		[FoldoutGroup("설정 관련/적")]
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
			#region 버튼 관련 변수 링킹
			M_MapEditingUI.mainMenuButton = m_MainMenuButton;
			M_MapEditingUI.saveButton = m_SaveButton;
			M_MapEditingUI.playButton = m_PlayButton;
			#endregion

			#region 카메라 관련 변수 링킹
			M_MapEditing.mapEditorCamera = m_MapEditorCamera;
			M_MapEditing.thumnailCamera = m_ThumnailCamera;

			M_MapEditing.editViewCameraTransform = m_EditViewCameraTransform;
			M_MapEditing.gameViewCameraTransform = m_GameViewCameraTransform;
			#endregion

			#region 컨트롤러 관련 변수 링킹
			M_MapEditingUI.menuPanelController = m_MenuPanelController;
			M_MapEditingUI.settingPanelController = m_SettingPanelController;
			#endregion

			#region 설정 관련 변수 링킹
			#region 타일
			M_MapEditingUI.tileDataUIContent = m_TileDataUIContent;
			#endregion

			#region 오퍼레이터
			M_MapEditingUI.operatorDataUIContent = m_OperatorDataUIContent;
			#endregion

			#region 적
			M_MapEditingUI.enemyDataUIContent = m_EnemyDataUIContent;
			M_MapEditingUI.enemySpawnDataUIParent = m_EnemySpawnDataUIParent;
			M_MapEditingUI.enemyWayPointDataUIParent = m_EnemyWayPointDataUIParent;
			M_MapEditingUI.enemyImmuneDescriptionParent = m_EnemyImmuneDescriptionParent;
			#endregion
			#endregion
			#endregion

			// Main Menu Scene 전환 전 이벤트
			AddBeforeEvent("Main Menu Scene", M_Game.FinallizeMapEditing);

			// Main Menu Scene 전환 후 이벤트
			AddAfterEvent("Main Menu Scene", M_Game.InitializeMainMenu);

			// Game Playing Scene 전환 전 이벤트
			AddBeforeEvent("Game Playing Scene", M_Game.FinallizeMapEditing);

			// Game Playing Scene 전환 후 이벤트
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