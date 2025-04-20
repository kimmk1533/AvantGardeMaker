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
		public StageData currentStageData => m_GameStageData;
		public bool isGameMode => m_IsGameMode;
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		// Main Menu Scene Manager
		private static PanelManager M_Panel => PanelManager.Instance;
		private static MainMenuUIManager M_MainMenuUI => MainMenuUIManager.Instance;

		// Map Editing Scene Manager
		private static MapEditingManager M_MapEditing => MapEditingManager.Instance;
		private static MapEditingUIManager M_MapEditingUI => MapEditingUIManager.Instance;

		// Game Playing Scene Manager
		private static GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;
		private static GamePlayingUIManager M_GamePlayingUI => GamePlayingUIManager.Instance;

		// Object Manager
		private static TileManager M_Tile => TileManager.Instance;
		private static OperatorManager M_Operator => OperatorManager.Instance;
		private static EnemyManager M_Enemy => EnemyManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public new void Initialize()
		{
			SaveLoadUtility.Initialize();

			// Main Menu Scene Manager
			M_Panel.Initialize();
			M_MainMenuUI.Initialize();

			// Map Editing Scene Manager
			M_MapEditing.Initialize();
			M_MapEditingUI.Initialize();

			// Game Playing Scene Manager
			M_GamePlaying.Initialize();
			M_GamePlayingUI.Initialize();

			// Object Manager
			M_Tile.Initialize();
			M_Operator.Initialize();
			M_Enemy.Initialize();

			Debug.Log("Initialize");
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public new void Finallize()
		{
			SaveLoadUtility.Finallize();

			// Object Manager
			M_Enemy.Finallize();
			M_Operator.Finallize();
			M_Tile.Finallize();

			// Game Playing Scene Manager
			M_GamePlayingUI.Finallize();
			M_GamePlaying.Finallize();

			// Map Editing Scene Manager
			M_MapEditingUI.Finallize();
			M_MapEditing.Finallize();

			// Main Menu Scene Manager
			M_MainMenuUI.Finallize();
			M_Panel.Finallize();

			Debug.Log("Finallize");
		}

		/// <summary>
		/// 메인 초기화 함수 (Main Menu Scene 진입 시 호출)
		/// </summary>
		public void InitializeMainMenu()
		{
			M_Panel.InitializeMain();
			M_MainMenuUI.InitializeMain();

			Debug.Log("Initialize Main Menu");
		}
		/// <summary>
		/// 메인 마무리화 함수 (Main Menu Scene 나갈 시 호출)
		/// </summary>
		public void FinallizeMainMenu()
		{
			M_MainMenuUI.FinallizeMain();
			M_Panel.FinallizeMain();

			Debug.Log("Finallize Main Menu");
		}

		/// <summary>
		/// 메인 초기화 함수 (Game Playing Scene 진입 시 호출)
		/// </summary>
		public void InitializeGamePlaying()
		{
			m_IsGameMode = true;

			M_Tile.InitializeMain();
			M_Operator.InitializeMain();
			M_Enemy.InitializeMain();

			M_GamePlaying.InitializeMain();
			M_GamePlayingUI.InitializeMain();

			M_Tile.LoadTileData(m_GameStageData);
			M_Enemy.LoadEnemyData(m_GameStageData);

			Debug.Log("Initialize Game Playing");
		}
		/// <summary>
		/// 메인 마무리화 함수 (Game Playing Scene 나갈 시 호출)
		/// </summary>
		public void FinallizeGamePlaying()
		{
			m_IsGameMode = false;

			M_GamePlayingUI.FinallizeMain();
			M_GamePlaying.FinallizeMain();

			M_Enemy.FinallizeMain();
			M_Operator.FinallizeMain();
			M_Tile.FinallizeMain();

			Debug.Log("Finallize Game Playing");
		}

		/// <summary>
		/// 메인 초기화 함수 (Map Editing Scene 진입 시 호출)
		/// </summary>
		public void InitializeMapEditing()
		{
			M_Tile.InitializeMain();

			M_MapEditing.InitializeMain();
			M_MapEditingUI.InitializeMain();

			M_MapEditing.LoadData();

			Debug.Log("Initialize Map Editing");
		}
		/// <summary>
		/// 메인 마무리화 함수 (Map Editing Scene 나갈 시 호출)
		/// </summary>
		public void FinallizeMapEditing()
		{
			M_MapEditingUI.FinallizeMain();
			M_MapEditing.FinallizeMain();

			M_Tile.FinallizeMain();

			Debug.Log("Finallize Map Editing");
		}
		#endregion

		public void SynchronizeStageData(StageData stageData)
		{
			m_GameStageData = stageData;
		}
	}
}