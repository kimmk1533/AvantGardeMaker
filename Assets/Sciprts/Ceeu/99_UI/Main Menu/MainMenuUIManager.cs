using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
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

		#region Init Panel
		private TMP_InputField m_NickNameInputField = null;
		private Button m_NickNameConfirmButton = null;
		#endregion

		#region MainMenu Buttons
		private Button m_MapListButton = null;
		private Button m_MapEditorButton = null;
		private Button m_OptionButton = null;
		private Button m_QuitButton = null;
		#endregion
		#endregion

		#region 프로퍼티
		public string testFilter => m_TestFilter;

		public RectTransform mainMenuInitPanel { get; set; }
		public RectTransform mainMenuButtonsPanel { get; set; }

		public MapListPanel mapListPanel { get; set; }
		public RectTransform optionPanel { get; set; }

		public RectTransform mapListItemParent { get; set; }
		#endregion

		#region 이벤트

		#region 이벤트 함수
		private async void OnNicknameConfirmButtonClicked()
		{
			if (CheckNickName() == false)
				return;

			string nickName = m_NickNameInputField.text;
			await SaveLoadUtility.SaveData("nickName", nickName);
			M_MapEditing.creatorNickName = nickName;

			mainMenuInitPanel.gameObject.SetActive(false);
			mainMenuButtonsPanel.gameObject.SetActive(true);
		}

		private void OnMapListButtonClicked()
		{
			mapListPanel.gameObject.SetActive(true);
		}
		private void OnMapEditorButtonClicked()
		{
			SceneLoader.LoadScene("Map Editing Scene");
		}
		private void OnOptionButtonClicked()
		{
			optionPanel.gameObject.SetActive(true);
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
		private static MapEditingManager M_MapEditing => MapEditingManager.Instance;
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

			gameObject.SetActive(false);
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

			m_MapListButton = mainMenuButtonsPanel.Find<Button>("Map List Button");
			m_MapEditorButton = mainMenuButtonsPanel.Find<Button>("Map Editor Button");
			m_OptionButton = mainMenuButtonsPanel.Find<Button>("Option Button");
			m_QuitButton = mainMenuButtonsPanel.Find<Button>("Quit Button");

			m_MapListButton.onClick.AddListener(OnMapListButtonClicked);
			m_MapEditorButton.onClick.AddListener(OnMapEditorButtonClicked);
			m_OptionButton.onClick.AddListener(OnOptionButtonClicked);
			m_QuitButton.onClick.AddListener(OnQuitButtonClicked);

			mainMenuInitPanel.gameObject.SetActive(false);
			mainMenuButtonsPanel.gameObject.SetActive(false);

			m_NickNameInputField = mainMenuInitPanel.Find<TMP_InputField>("NickName InputField");
			m_NickNameConfirmButton = mainMenuInitPanel.Find<Button>("Confirm Button");

			m_NickNameConfirmButton.onClick.AddListener(OnNicknameConfirmButtonClicked);

			InitProcess();

			mapListPanel.Initialize();

			mapListPanel.gameObject.SetActive(false);
			optionPanel.gameObject.SetActive(false);

			gameObject.SetActive(true);
		}
		/// <summary>
		/// 게임 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();

			mapListPanel.Finallize();

			m_NickNameConfirmButton.onClick.RemoveListener(OnNicknameConfirmButtonClicked);

			m_NickNameConfirmButton = null;
			m_NickNameInputField = null;

			m_MapListButton.onClick.RemoveListener(OnMapListButtonClicked);
			m_MapEditorButton.onClick.RemoveListener(OnMapEditorButtonClicked);
			m_OptionButton.onClick.RemoveListener(OnOptionButtonClicked);
			m_QuitButton.onClick.RemoveListener(OnQuitButtonClicked);

			m_MapListButton = null;
			m_MapEditorButton = null;
			m_OptionButton = null;
			m_QuitButton = null;

			gameObject.SetActive(false);
		}
		#endregion

		private async void InitProcess()
		{
			string nickName = await SaveLoadUtility.LoadData<string>("nickName");

			if (string.IsNullOrEmpty(nickName) == true)
			{
				mainMenuInitPanel.gameObject.SetActive(true);
				return;
			}

			mainMenuButtonsPanel.gameObject.SetActive(true);
			M_MapEditing.creatorNickName = nickName;
		}
		private bool CheckNickName()
		{
			return true;
		}
	}
}