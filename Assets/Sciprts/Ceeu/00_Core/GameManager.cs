using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.ad1a;
using AvantGardeMaker.MikangMark;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.Ceeu
{
	public sealed class GameManager : SerializedSingleton<GameManager>
	{
		#region 변수
		#region 게임 관련 변수
		private StageData m_GameStageData = default;
		private bool m_IsGameMode = false;
		#endregion
		#endregion

		#region 프로퍼티
		public StageData currentMapData => m_GameStageData;
		public bool isGameMode => m_IsGameMode;
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		private static PanelManager M_Panel => PanelManager.Instance;

		private static MainMenuUIManager M_MainMenuUI => MainMenuUIManager.Instance;

		private static TileManager M_Tile => TileManager.Instance;
		private static MapEditingManager M_MapEditing => MapEditingManager.Instance;
		private static MapEditingUIManager M_MapEditingUI => MapEditingUIManager.Instance;

		private static EnemyManager M_EnemyManager => EnemyManager.Instance;

		//private static InGamePlayManager M_GamePlaying => InGamePlayManager.Instance;

		//private static OperatorManager M_Operator=>OperatorManager.Instance;

		//private static OperatorYamlManager M_Yaml => OperatorYamlManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		protected override void Awake()
		{
			base.Awake();

			Initialize();
		}
		private void OnApplicationQuit()
		{
			Finallize();
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public void Initialize()
		{
			SaveLoadUtility.Initialize();

			M_Panel.Initialize();

			M_MainMenuUI.Initialize();

			M_Tile.Initialize();
			M_MapEditing.Initialize();
			M_MapEditingUI.Initialize();

			//ad1a
			M_EnemyManager.Initialize();

			//MikangMark
			//M_Operator.Initialize();
			//M_Yaml.Initialize();
			//M_GamePlaying.Initialize();

			Debug.Log("Initialize");
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public void Finallize()
		{
			SaveLoadUtility.Finallize();

			M_MapEditingUI.Finallize();
			M_MapEditing.Finallize();
			M_Tile.Finallize();

			//ad1a
			M_EnemyManager.Finallize();

			//MikangMark
			//M_Operator.Finallize();
			//M_Yaml.Finallize();
			//M_GamePlaying.Finallize();

			M_MainMenuUI.Finallize();

			M_Panel.Finallize();

			Debug.Log("Finallize");
		}

		/// <summary>
		/// 게임 초기화 함수 (Main Menu Scene 진입 시 호출)
		/// </summary>
		public void InitializeMainMenu()
		{
			M_Panel.InitializeMain();

			M_MainMenuUI.InitializeMain();

			Debug.Log("Initialize Main Menu");
		}
		/// <summary>
		/// 게임 마무리화 함수 (Main Menu Scene 나갈 시 호출)
		/// </summary>
		public void FinallizeMainMenu()
		{
			M_MainMenuUI.FinallizeMain();

			M_Panel.FinallizeMain();

			Debug.Log("Finallize Main Menu");
		}

		/// <summary>
		/// 게임 초기화 함수 (Game Playing Scene 진입 시 호출)
		/// </summary>
		public void InitializeGamePlaying()
		{
			SynchronizeStageData();
			m_IsGameMode = true;

			//ad1a
			//M_EnemyManager.InitializeMain();

			//MikangMark
			//M_Operator.InitializeMain();
			//M_Yaml.InitializeMain();
			//M_GamePlaying.InitializeMain();

			Debug.Log("Initialize Game Playing");
		}
		/// <summary>
		/// 게임 마무리화 함수 (Game Playing Scene 나갈 시 호출)
		/// </summary>
		public void FinallizeGamePlaying()
		{
			m_IsGameMode = false;

			//ad1a
			//M_EnemyManager.FinallizeMain();

			//MikangMark
			//M_Operator.FinallizeMain();
			//M_Yaml.FinallizeMain();
			//M_GamePlaying.FinallizeMain();

			Debug.Log("Finallize Game Playing");
		}

		/// <summary>
		/// 게임 초기화 함수 (Map Editing Scene 진입 시 호출)
		/// </summary>
		public void InitializeMapEditing()
		{
			M_Panel.InitializeMain();

			M_Tile.InitializeMain();

			M_MapEditingUI.InitializeMain();
			M_MapEditing.InitializeMain();

			M_EnemyManager.gameObject.SetActive(false);

			Debug.Log("Initialize Map Editing");
		}
		/// <summary>
		/// 게임 마무리화 함수 (Map Editing Scene 나갈 시 호출)
		/// </summary>
		public void FinallizeMapEditing()
		{
			M_MapEditing.FinallizeMain();
			M_MapEditingUI.FinallizeMain();

			M_EnemyManager.FinallizeMain();
			M_Tile.FinallizeMain();

			M_Panel.FinallizeMain();

			Debug.Log("Finallize Map Editing");
		}
		#endregion

		public void SynchronizeStageData()
		{
			m_GameStageData = M_MapEditing.currentStageData;
		}
	}
}