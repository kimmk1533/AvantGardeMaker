using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.ad1a;
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
		private static MapEditorManager M_MapEditor => MapEditorManager.Instance;
		private static TileManager M_Tile => TileManager.Instance;
		private static MapEditorUIManager M_MapEditorUI => MapEditorUIManager.Instance;

		private static EnemyManager M_EnemyGenerate => EnemyManager.Instance;
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
			M_Tile.Initialize();
			M_MapEditor.Initialize();
			M_MapEditorUI.Initialize();

			//ad1a
			M_EnemyGenerate.Initialize();
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public void Finallize()
		{
			M_MapEditorUI.Finallize();
			M_MapEditor.Finallize();
			M_Tile.Finallize();

			//ad1a
			M_EnemyGenerate.Finallize();
		}

		/// <summary>
		/// 게임 초기화 함수 (In Game Scene 진입 시 호출)
		/// </summary>
		public void InitializeGame()
		{
			m_GameStageData = M_MapEditor.currentStageData;
			m_IsGameMode = true;

			//ad1a
			M_EnemyGenerate.InitializeMain();
		}
		/// <summary>
		/// 게임 마무리화 함수 (In Game Scene 나갈 시 호출)
		/// </summary>
		public void FinallizeGame()
		{
			m_IsGameMode = false;

			//ad1a
			M_EnemyGenerate.FinallizeMain();
		}

		/// <summary>
		/// 게임 초기화 함수 (Map Editor Scene 진입 시 호출)
		/// </summary>
		public void InitializeMapEditor()
		{
			M_Tile.InitializeMain();
			M_MapEditor.InitializeMain();
			M_MapEditorUI.InitializeMain();
		}
		/// <summary>
		/// 게임 마무리화 함수 (Map Editor Scene 나갈 시 호출)
		/// </summary>
		public void FinallizeMapEditor()
		{
			M_MapEditorUI.FinallizeGame();
			M_MapEditor.FinallizeGame();
			M_Tile.FinallizeMain();
		}
		#endregion

		public void SynchronizeStageData()
		{
			m_GameStageData = M_MapEditor.currentStageData;
		}
	}
}