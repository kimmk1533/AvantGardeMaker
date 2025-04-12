using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class MapListPanel : Panel
	{
		#region 변수
		#region Map Item List 관련 변수
		private TMP_Text m_LoadingText = null;

		private StageData m_CurrentStageData = default;
		#endregion

		#region Map Info 관련 변수
		private RawImage m_ThumnailImage = null;
		private TMP_Text m_TitleText = null;
		private TMP_Text m_CreatorText = null;
		//private Rating m_Rating = null;
		private Button m_OperatorInfoButton = null;
		private Button m_EnemyInfoButton = null;
		private TMP_Text m_DescriptionText = null;
		#endregion

		private Button m_PlayButton = null;
		private Button m_EditButton = null;
		private Button m_DeleteButton = null;
		#endregion

		#region 프로퍼티
		public StageData currentStageData
		{
			get => m_CurrentStageData;
			set => m_CurrentStageData = value;
		}
		#endregion

		#region 이벤트

		#region 이벤트 함수
		private void OnMapListItemClicked(MapListItem mapListItem)
		{
			m_CurrentStageData = mapListItem.stageData;

			m_ThumnailImage.texture = mapListItem.thumnailImage;
			m_TitleText.text = mapListItem.titleText;
			m_CreatorText.text = mapListItem.creatorText;
			//m_Rating.value = mapListItem.rating.value;
			m_DescriptionText.text = mapListItem.stageData.description;

			string playerId = SaveLoadUtility.GetPlayerId();

			m_PlayButton.interactable = true;
			m_EditButton.interactable = playerId.Equals(m_CurrentStageData.createdPlayerId);
			m_DeleteButton.interactable = playerId.Equals(m_CurrentStageData.createdPlayerId);
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

		}
		private void OnEditButtonClicked()
		{
			M_MapEditing.SynchronizeStageData(m_CurrentStageData);

			SceneLoader.LoadScene("Map Editing Scene");
		}
		private async void OnDeleteButtonClicked()
		{
			await SaveLoadUtility.DeleteStageData(m_CurrentStageData.title);

			UpdateMapListItem();

			m_CurrentStageData = default;

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
		private static MainMenuUIManager M_MainMenuUI => MainMenuUIManager.Instance;
		private static MapEditingManager M_MapEditing => MapEditingManager.Instance;
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

			if (m_LoadingText == null)
				m_LoadingText = transform.FindInChildren<TMP_Text>("Loading Text");

			if (m_ThumnailImage == null)
				m_ThumnailImage = transform.Find("Map Info Panel").Find<RawImage>("Thumnail Image");
			if (m_TitleText == null)
			{
				m_TitleText = transform.Find("Map Info Panel").Find<TMP_Text>("Title Text");
			}
			if (m_CreatorText == null)
			{
				m_CreatorText = transform.Find("Map Info Panel").Find<TMP_Text>("Creator Text");
			}
			//if (m_Rating == null)
			//{
			//	m_Rating = transform.Find("Map Info Panel").Find<Rating>("Rating");
			//}
			//m_Rating.Initialize();
			if (m_OperatorInfoButton == null)
			{
				m_OperatorInfoButton = transform.Find("Map Info Panel").Find<Button>("Operator Info Button");

				m_OperatorInfoButton.onClick.AddListener(OnOperatorInfoButtonClicked);
			}
			if (m_EnemyInfoButton == null)
			{
				m_EnemyInfoButton = transform.Find("Map Info Panel").Find<Button>("Enemy Info Button");

				m_EnemyInfoButton.onClick.AddListener(OnEnemyInfoButtonClicked);
			}
			if (m_DescriptionText == null)
			{
				m_DescriptionText = transform.Find("Map Info Panel").FindInChildren<TMP_Text>("Description Text");
			}

			if (m_PlayButton == null)
			{
				m_PlayButton = transform.Find("Buttons").Find<Button>("Play Button");

				m_PlayButton.onClick.AddListener(OnPlayButtonClicked);
				m_PlayButton.interactable = false;
			}
			if (m_EditButton == null)
			{
				m_EditButton = transform.Find("Buttons").Find<Button>("Edit Button");

				m_EditButton.onClick.AddListener(OnEditButtonClicked);
				m_EditButton.interactable = false;
			}
			if (m_DeleteButton == null)
			{
				m_DeleteButton = transform.Find("Buttons").Find<Button>("Delete Button");

				m_DeleteButton.onClick.AddListener(OnDeleteButtonClicked);
				m_DeleteButton.interactable = false;
			}
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

				mapListItem.onClick -= OnMapListItemClicked;

				M_MainMenuUI.Despawn(mapListItem);
			}

			m_LoadingText.text = "Loading…";
			m_LoadingText.gameObject.SetActive(true);
		}
		private async Awaitable CreateMapListItem()
		{
			Transform itemParent = M_MainMenuUI.mapListItemParent;
			List<StageData> stageDataList = await SaveLoadUtility.LoadAllStageData(M_MainMenuUI.testFilter);

			for (int i = 0; i < stageDataList.Count; ++i)
			{
				MapListItem mapListItem = M_MainMenuUI.GetBuilder("Map List Item")
					.SetParent(itemParent)
					.SetScale(Vector3.one)
					.SetLocalPosition(Vector3.zero)
					.SetActive(true)
					.SetAutoInit(true)
					.Spawn() as MapListItem;

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
		private async void UpdateMapListItem()
		{
			ClearMapListItem();
			await CreateMapListItem();
		}
	}
}