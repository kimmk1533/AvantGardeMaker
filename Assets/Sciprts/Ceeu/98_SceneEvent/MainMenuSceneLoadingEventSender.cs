using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class MainMenuSceneLoadingEventSender : SceneEventSender
	{
		#region 변수
		[SerializeField]
		private Button m_MapEditorButton = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트

		#region 이벤트 함수
		public void OnGameButtonClicked()
		{
			SceneLoader.LoadScene("Game Scene");
		}
		public void OnMapEditorButtonClicked()
		{
			SceneLoader.LoadScene("Map Editor Scene");
		}
		public void OnQuitButtonClicked()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.ExitPlaymode();
#else
			Application.Quit();
#endif
		}
		#endregion
		#endregion

		#region 매니저
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

			m_MapEditorButton.onClick.AddListener(OnMapEditorButtonClicked);
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