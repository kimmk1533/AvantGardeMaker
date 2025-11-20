using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace;
using AvantGardeMaker.CoreSpace.SaveLoad;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class MapListPanel : Panel
	{
		#region 변수
		#region Map Item List 관련 변수
		private TextMeshProUGUI m_LoadingText = null;

		private MapListItem m_SelectedMapListItem = null;
		#endregion

		#region Map Info 관련 변수
		private RawImage m_ThumnailImage = null;
		private TextMeshProUGUI m_TitleText = null;
		private TextMeshProUGUI m_CreatorText = null;
		//private Rating m_Rating = null;
		private Button m_OperatorInfoButton = null;
		private Button m_EnemyInfoButton = null;
		private TextMeshProUGUI m_DescriptionText = null;
		#endregion

		private Button m_PlayButton = null;
		private Button m_EditButton = null;
		private Button m_DeleteButton = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트

		#region 이벤트 함수
		private void OnMapListItemClicked(MapListItem mapListItem)
		{
			m_SelectedMapListItem = mapListItem;
			StageData stageData = mapListItem.stageData;

			m_ThumnailImage.texture = mapListItem.thumnailImage;
			m_TitleText.text = mapListItem.titleText;
			m_CreatorText.text = mapListItem.creatorText;
			//m_Rating.value = mapListItem.rating.value;
			m_DescriptionText.text = stageData.description;

			string playerId = SaveLoadUtility.GetPlayerId();

			m_PlayButton.interactable = true;
			m_EditButton.interactable = true;//playerId.Equals(stageData.createdPlayerId);
			m_DeleteButton.interactable = playerId.Equals(stageData.createdPlayerId);
		}

		private void OnOperatorInfoButtonClicked()
		{
			Debug.Log("Operator Info Button Clicked.");
		}
		private void OnEnemyInfoButtonClicked()
		{
			Debug.Log("Enemy Info Button Clicked.");
		}

		private void OnPlayButtonClicked()
		{
			M_GamePlaying.SynchronizeStageData(m_SelectedMapListItem.stageData);

			SceneLoader.LoadScene("Game Playing Scene");
		}
		private void OnEditButtonClicked()
		{
			M_MapEditing.SynchronizeStageData(m_SelectedMapListItem.stageData);

			SceneLoader.LoadScene("Map Editing Scene");
		}
		private async void OnDeleteButtonClicked()
		{
			await SaveLoadUtility.DeleteStageData(m_SelectedMapListItem.stageData.title);

			UpdateMapListItem();

			m_SelectedMapListItem = null;

			m_ThumnailImage.texture = null;
			m_TitleText.text = string.Empty;
			m_CreatorText.text = string.Empty;
			//m_Rating.value = 0f;
			m_DescriptionText.text = string.Empty;

			m_PlayButton.interactable = false;
			m_EditButton.interactable = false;
			m_DeleteButton.interactable = false;
		}
		#endregion
		#endregion

		#region 매니저
		private static MainMenuSceneUIManager M_MainMenuUI => MainMenuSceneUIManager.Instance;
		private static MapEditingManager M_MapEditing => MapEditingManager.Instance;
		private static GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		protected override void OnEnable()
		{
			base.OnEnable();

			UpdateMapListItem();
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			m_LoadingText = transform.Find<TextMeshProUGUI>("Map List Item Panel/Loading Text");

			m_ThumnailImage = transform.Find<RawImage>("Map Info Panel/Thumnail Image");
			m_TitleText = transform.Find<TextMeshProUGUI>("Map Info Panel/Title Text");
			m_CreatorText = transform.Find<TextMeshProUGUI>("Map Info Panel/Creator Text");
			//m_Rating = transform.Find<Rating>("Map Info Panel/Rating");
			m_OperatorInfoButton = transform.Find<Button>("Map Info Panel/Operator Info Button");
			m_EnemyInfoButton = transform.Find<Button>("Map Info Panel/Enemy Info Button");
			m_DescriptionText = transform.Find<TextMeshProUGUI>("Map Info Panel/Description Scroll Rect/Description Viewport/Description Content/Description Text");

			m_PlayButton = transform.Find<Button>("Buttons/Play Button");
			m_EditButton = transform.Find<Button>("Buttons/Edit Button");
			m_DeleteButton = transform.Find<Button>("Buttons/Delete Button");

			m_OperatorInfoButton.onClick.AddListener(OnOperatorInfoButtonClicked);
			m_EnemyInfoButton.onClick.AddListener(OnEnemyInfoButtonClicked);

			m_PlayButton.onClick.AddListener(OnPlayButtonClicked);
			m_EditButton.onClick.AddListener(OnEditButtonClicked);
			m_DeleteButton.onClick.AddListener(OnDeleteButtonClicked);

			m_PlayButton.interactable = false;
			m_EditButton.interactable = false;
			m_DeleteButton.interactable = false;

			//m_Rating.Initialize();
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();

			m_LoadingText.text = string.Empty;

			m_ThumnailImage.texture = null;

			//m_Rating.Finallize();
		}
		#endregion

		private void ClearMapListItem()
		{
			Transform itemParent = M_MainMenuUI.mapListItemParent;
			int childCount = itemParent.childCount;

			for (int i = 0; i < childCount; ++i)
			{
				MapListItem mapListItem = itemParent.GetChild<MapListItem>(0);

				M_MainMenuUI.Despawn(mapListItem);
			}

			m_LoadingText.text = "Loading…";
			m_LoadingText.gameObject.SetActive(true);
		}
		private async Awaitable CreateMapListItem(string filter)
		{
			Transform itemParent = M_MainMenuUI.mapListItemParent;
			List<StageData> stageDataList = null;

			if (string.IsNullOrEmpty(filter) == true)
				stageDataList = await SaveLoadUtility.LoadAllStageData();
			else
				stageDataList = await SaveLoadUtility.LoadAllStageData(filter);

			for (int i = 0; i < stageDataList.Count; ++i)
			{
				MapListItem mapListItem = M_MainMenuUI.GetBuilder("Map List Item")
					.SetParent(itemParent)
					.SetScale(Vector3.one)
					.SetLocalPosition(Vector3.zero)
					.SetActive(true)
					.SetAutoInit(true)
					.Spawn<MapListItem>();

				StageData stageData = stageDataList[i];

				mapListItem.stageData = stageData;

				mapListItem.UpdateUI();

				mapListItem.onClick += OnMapListItemClicked;
			}

			if (stageDataList.Count == 0)
				m_LoadingText.text = "No Item Founded";
			else
				m_LoadingText.gameObject.SetActive(false);
		}
		public async void UpdateMapListItem(string filter = "")
		{
			ClearMapListItem();
			await CreateMapListItem(filter);
		}
	}
}