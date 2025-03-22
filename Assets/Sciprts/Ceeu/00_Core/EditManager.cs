using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker
{
	public sealed class EditManager : SerializedSingleton<EditManager>
	{
		#region 변수
		private bool m_IsEditMode = false;

		[Min(1)]
		[SerializeField]
		private int m_MapWidth = 1;
		[Min(1)]
		[SerializeField]
		private int m_MapHeight = 1;

		[SerializeField]
		private GameObject m_TestTile = null;
		#endregion

		#region 프로퍼티
		public bool isEditMode => m_IsEditMode;
		#endregion

		#region 이벤트
		#endregion

		#region 매니저

		#endregion

		#region 유니티 콜백 함수
		#endregion

		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public void Initialize()
		{

		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public void Finallize()
		{

		}

		/// <summary>
		/// 게임 초기화 함수 (Game Scene 진입 시 호출)
		/// </summary>
		public void InitializeGame()
		{

		}
		/// <summary>
		/// 게임 마무리화 함수 (Game Scene 나갈 시 호출)
		/// </summary>
		public void FinallizeGame()
		{

		}
	}
}