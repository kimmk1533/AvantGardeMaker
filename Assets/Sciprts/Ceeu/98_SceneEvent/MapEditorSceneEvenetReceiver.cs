using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.Ceeu
{
	public class MapEditorSceneEvenetReceiver : SceneEventReceiver
	{
		#region 변수
		#region Map Editor Manager 관련 변수
		[SerializeField]
		private Camera m_MapEditorCamera = null;

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
		#endregion
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		private static GameManager M_Game => GameManager.Instance;

		private static MapEditorManager M_MapEditor => MapEditorManager.Instance;
		private static MapEditorUIManager M_MapEditorUI => MapEditorUIManager.Instance;
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

			#region 멤버 변수 링킹
			M_MapEditor.mapEditorCamera = m_MapEditorCamera;
			M_MapEditor.editModeCameraTransform = m_EditModeCameraTransform;
			M_MapEditor.gameModeCameraTransform = m_GameModeCameraTransform;

			M_MapEditorUI.menuPanel = m_MenuPanel;
			M_MapEditorUI.optionPanel = m_OptionPanel;

			M_MapEditorUI.enemyDataSettingPanel = m_EnemyDataSettingPanel;

			M_MapEditorUI.enemySpawnDataUIParent = m_EnemySpawnDataUIParent;
			M_MapEditorUI.enemyDataUIParent = m_EnemyDataUIParent;
			M_MapEditorUI.enemyWayPointDataUIParent = m_EnemyWayPointDataUIParent;
			#endregion

			M_Game.InitializeMapEditor();
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		protected override void Finallize()
		{
			base.Finallize();

			M_Game.FinallizeMapEditor();
		}
		#endregion
	}
}