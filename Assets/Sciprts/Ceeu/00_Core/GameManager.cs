using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker
{
	public sealed class GameManager : SerializedSingleton<GameManager>
	{
		#region 변수
		private bool m_IsGameMode = false;
		#endregion

		#region 프로퍼티
		public bool isGameMode => m_IsGameMode;
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		private static EditManager M_Edit => EditManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Awake()
		{
			Initialize();
		}
		private void OnApplicationQuit()
		{
			Finallize();
		}
		#endregion

		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public void Initialize()
		{
			M_Edit.Initialize();
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public void Finallize()
		{
			M_Edit.Finallize();
		}

		/// <summary>
		/// 게임 초기화 함수 (Game Scene 진입 시 호출)
		/// </summary>
		public void InitializeGame()
		{
			m_IsGameMode = true;
		}
		/// <summary>
		/// 게임 마무리화 함수 (Game Scene 나갈 시 호출)
		/// </summary>
		public void FinallizeGame()
		{
			m_IsGameMode = false;
		}
	}
}