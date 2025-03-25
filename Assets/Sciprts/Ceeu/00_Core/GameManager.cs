using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.Ceeu
{
	public sealed class GameManager : SerializedSingleton<GameManager>
	{
		#region 변수
		#region 게임 관련 변수
		private MapData m_GameMapData = default;
		private bool m_IsGameMode = false;
		#endregion
		#endregion

		#region 프로퍼티
		public MapData currentMapData => m_GameMapData;
		public bool isGameMode => m_IsGameMode;
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		private static EditModeManager M_EditMode => EditModeManager.Instance;
		private static TileManager M_Tile => TileManager.Instance;
		private static UIManager M_UI => UIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Awake()
		{
			Initialize();

			InitializeGame();
		}
		private void OnApplicationQuit()
		{
			FinallizeGame();

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
			M_EditMode.Initialize();

			M_UI.Initialize();
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public void Finallize()
		{
			M_UI.Finallize();

			M_EditMode.Finallize();
			M_Tile.Finallize();
		}

		/// <summary>
		/// 게임 초기화 함수 (Game Scene 진입 시 호출)
		/// </summary>
		public void InitializeGame()
		{
			m_GameMapData = M_EditMode.currentMapData;
			m_IsGameMode = true;

			M_Tile.InitializeGame();
			M_EditMode.InitializeGame();

			M_UI.InitializeGame();
		}
		/// <summary>
		/// 게임 마무리화 함수 (Game Scene 나갈 시 호출)
		/// </summary>
		public void FinallizeGame()
		{
			M_UI.FinallizeGame();

			M_EditMode.FinallizeGame();
			M_Tile.FinallizeGame();

			m_IsGameMode = false;
		}
		#endregion
	}
}