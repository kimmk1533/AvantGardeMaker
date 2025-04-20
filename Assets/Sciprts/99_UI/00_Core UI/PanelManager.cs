using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.UI
{
	public class PanelManager : SerializedSingleton<PanelManager>
	{
		#region 변수
		private Stack<Panel> m_PanelStack = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트

		#region 이벤트 함수
		#endregion
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			PanelStackShortcut();
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			m_PanelStack = new Stack<Panel>();
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();


		}

		/// <summary>
		/// 메인 초기화 함수 (본인 Main Scene 진입 시 호출)
		/// </summary>
		public override void InitializeMain()
		{
			base.InitializeMain();


		}
		/// <summary>
		/// 메인 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();

			m_PanelStack.Clear();
		}
		#endregion

		private void PanelStackShortcut()
		{
			if (m_PanelStack.Count <= 0)
				return;
			if (Input.GetKeyDown(KeyCode.Escape) == false)
				return;

			Panel panel = m_PanelStack.Peek();
			while (m_PanelStack.Count > 0 &&
				panel != null &&
				panel.gameObject.activeSelf == false)
				panel = m_PanelStack.Pop();

			panel.gameObject.SetActive(false);
		}

		public void RegisterPanel(Panel panel)
		{
			if (m_PanelStack.Contains(panel) == true)
				return;

			m_PanelStack.Push(panel);
		}
	}
}