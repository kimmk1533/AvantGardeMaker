using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	// Main Scene: Main Menu Scene
	public class MainMenuUIManager : ObjectManager<MainMenuUIManager, MainMenuUI>
	{
		#region 변수
		[SerializeField]
		private string m_TestFilter = string.Empty;
		#endregion

		#region 프로퍼티
		public string testFilter => m_TestFilter;

		public Button mapListButton { get; set; }
		public Button mapEditorButton { get; set; }
		public Button optionButton { get; set; }
		public Button quitButton { get; set; }

		public MapListPanel mapListPanel { get; set; }
		public RectTransform optionPanel { get; set; }

		public RectTransform mapListItemParent { get; set; }
		#endregion

		#region 이벤트

		#region 이벤트 함수
		private void OnMapListButtonClicked()
		{
			mapListPanel.gameObject.SetActive(true);
		}
		private void OnMapEditorButtonClicked()
		{
			SceneLoader.LoadScene("Map Editor Scene");
		}
		private void OnOptionButtonClicked()
		{

		}
		private void OnQuitButtonClicked()
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
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();
		}

		/// <summary>
		/// 게임 초기화 함수 (본인 Main Scene 진입 시 호출)
		/// </summary>
		public override void InitializeMain()
		{
			base.InitializeMain();

			mapListButton.onClick.AddListener(OnMapListButtonClicked);
			mapEditorButton.onClick.AddListener(OnMapEditorButtonClicked);
			optionButton.onClick.AddListener(OnOptionButtonClicked);
			quitButton.onClick.AddListener(OnQuitButtonClicked);

			mapListPanel.Initialize();

			mapListPanel.gameObject.SetActive(false);
			optionPanel.gameObject.SetActive(false);
		}
		/// <summary>
		/// 게임 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();

			mapListPanel.Finallize();
		}
		#endregion
	}
}