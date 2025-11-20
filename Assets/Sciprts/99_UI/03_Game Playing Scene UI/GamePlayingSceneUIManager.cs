using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AvantGardeMaker.CoreSpace;
using AvantGardeMaker.TileSpace;
using AvantGardeMaker.OperatorSpace;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class GamePlayingSceneUIManager : ObjectManager<GamePlayingSceneUIManager, GamePlayingSceneUIPoolItem>
	{
		#region 변수
		[SerializeField, ReadOnly]
		private OperatorSquadUI m_SelectedOperatorSquadUI = null;
		[SerializeField, ReadOnly]
		private List<OperatorSquadUI> m_OperatorSquadUIList = null;

		private Dictionary<string, Sprite> m_OperatorFullshotSpriteMap = null;
		private Dictionary<Tile, OperatorSquadUI> m_TileOperatorSquadUIMap = null;
		#endregion

		#region 프로퍼티
		public Camera gamePlaingCamera { get; set; }

		public Button optionButton { get; set; }

		// 현재 코스트 텍스트
		public TextMeshProUGUI costValueText { get; set; }
		// 코스트 게이지 이미지
		public Image costFillImage { get; set; }

		// 오퍼레이터 배치 UI 부모
		public RectTransform operatorSquadUIParent { get; set; }
		// 오퍼레이터 배치 취소 버튼
		public Button deploymentCancelButton { get; set; }

		// 오퍼레이터 상태 UI
		public OperatorStatusUI operatorStatusUI { get; set; }
		// 오퍼레이터 퇴각 버튼
		public Button operatorRetreatButton { get; set; }
		// 오퍼레이터 스킬 버튼
		public Button operatorSkillButton { get; set; }

		public bool activeRetreatButton
		{
			get => operatorRetreatButton.gameObject.activeSelf;
			set => operatorRetreatButton.gameObject.SetActive(value);
		}
		public bool activeSkillButton
		{
			get => operatorSkillButton.gameObject.activeSelf;
			set => operatorSkillButton.gameObject.SetActive(value);
		}
		public OperatorSquadUI selectedOperatorSquadUI
		{
			get => m_SelectedOperatorSquadUI;
		}

		public OperatorStatusUI operatorStatus => OperatorStatusUI.Instance;
		#endregion

		#region 이벤트

		#region 이벤트 함수
		//오퍼레이터 초상화 클릭
		public void OnOperatorSquadUIClicked(OperatorSquadUI operatorSquadUI)
		{
			// 이미 선택된 오퍼레이터 UI 클릭 시 클릭 취소
			if (m_SelectedOperatorSquadUI == operatorSquadUI)
			{
				operatorStatusUI.gameObject.SetActive(false);

				m_SelectedOperatorSquadUI = null;

				return;
			}

			m_SelectedOperatorSquadUI = operatorSquadUI;

			operatorStatusUI.selectedOperatorData = operatorSquadUI.operatorData;
			operatorStatusUI.gameObject.SetActive(true);
		}
		// 타일 클릭
		public void OnTileClicked(Tile tile)
		{
			// 버튼 위치 구하기
			Vector3 worldPos = tile.currentOperator.transform.position;
			Vector3 screenPos = gamePlaingCamera.WorldToScreenPoint(worldPos);

			// 퇴각 버튼 위치 & 활성화
			operatorRetreatButton.transform.position = new Vector3(screenPos.x - 200, screenPos.y + 200);
			operatorRetreatButton.gameObject.SetActive(true);

			// 스킬 버튼 위치 & 활성화
			operatorSkillButton.transform.position = new Vector3(screenPos.x + 200, screenPos.y - 200);
			operatorSkillButton.gameObject.SetActive(true);

			// 퇴각 버튼 이벤트
			operatorRetreatButton.onClick.RemoveAllListeners();
			operatorRetreatButton.onClick.AddListener(tile.RetreatOperator);

			// 스킬 버튼 이벤트
			operatorSkillButton.onClick.RemoveAllListeners();
			operatorSkillButton.onClick.AddListener(tile.currentOperator.OnSkillButtonClicked);

			// 오퍼레이터 스탯 창
			operatorStatusUI.selectedOperatorData = tile.currentOperator.operatorData;
			operatorStatusUI.gameObject.SetActive(true);
		}
		//배치취소버튼클릭
		public void OnDeploymentCancelButtonClicked()
		{
			deploymentCancelButton.gameObject.SetActive(false);

			m_SelectedOperatorSquadUI.CancelDeployment();
			m_SelectedOperatorSquadUI = null;

			operatorStatusUI.gameObject.gameObject.SetActive(false);
		}
		#endregion
		#endregion

		#region 매니저
		private static MapEditingManager M_MapEditing => MapEditingManager.Instance;
		private static GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;
		private static OperatorManager M_Operator => OperatorManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			UpdateUI();

			//if (m_IsDragging)
			//{
			//	MouseRealPoint();
			//	m_TurnOff = true;
			//}
			//else
			//{
			//	if (m_TurnOff)
			//	{
			//		for (int i = 0; i < m_CreatedAttackRangeHighlightList.Count; i++)
			//		{
			//			m_CreatedAttackRangeHighlightList[i].SetActive(m_TurnOff = false);
			//		}
			//	}

			//}
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			m_OperatorSquadUIList = new List<OperatorSquadUI>();

			m_OperatorFullshotSpriteMap = new Dictionary<string, Sprite>();
			Sprite[] operatorFullshotSprites = Resources.LoadAll<Sprite>("Textures/02_Operator Textures/OperatorFullImg");
			for (int i = 0; i < operatorFullshotSprites.Length; i++)
			{
				m_OperatorFullshotSpriteMap.Add(operatorFullshotSprites[i].name, operatorFullshotSprites[i]);
			}

			m_TileOperatorSquadUIMap = new Dictionary<Tile, OperatorSquadUI>();
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

			CreateOperatorSquadUI();

			optionButton.onClick.AddListener(LoadPrevScene);
			deploymentCancelButton.onClick.AddListener(OnDeploymentCancelButtonClicked);
		}
		/// <summary>
		/// 메인 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();

			DestroyOperatorSquadUI();

			deploymentCancelButton.onClick.RemoveListener(OnDeploymentCancelButtonClicked);
			optionButton.onClick.RemoveAllListeners();

			m_TileOperatorSquadUIMap.Clear();
		}
		#endregion

		private void LoadPrevScene()
		{
			if (SceneLoader.prevSceneName.Equals("Map Editing Scene") == true)
				M_MapEditing.SynchronizeStageData(M_GamePlaying.currentStageData);

			Debug.Log("이전 씬: " + SceneLoader.prevSceneName);

			SceneLoader.LoadScene(SceneLoader.prevSceneName);
		}

		private void CreateOperatorSquadUI()
		{
			List<OperatorSpawnData> operatorSpawnDataList = M_GamePlaying.operatorSpawnDataList;

			// 생성
			for (int i = 0; i < operatorSpawnDataList.Count; ++i)
			{
				OperatorSpawnData operatorSpawnData = operatorSpawnDataList[i];
				string key = operatorSpawnData.OperatorSpawnKey;

				OperatorSquadUI operatorSquadUI = GetBuilder("Operator Squad UI")
					.SetAutoInit(true)
					.SetActive(true)
					.SetName(key)
					.Spawn<OperatorSquadUI>();

				operatorSquadUI.operatorData = M_Operator.GetOperatorData(key);

				operatorSquadUI.onOperatorSquadUIClicked += OnOperatorSquadUIClicked;

				m_OperatorSquadUIList.Add(operatorSquadUI);
			}

			// 정렬
			m_OperatorSquadUIList = m_OperatorSquadUIList
				.OrderBy(squadUI => squadUI.operatorData.VariableData.DeploymentCost)
				.ToList();

			// 정렬 적용
			for (int i = 0; i < m_OperatorSquadUIList.Count; ++i)
			{
				m_OperatorSquadUIList[i].transform.SetParent(operatorSquadUIParent);
			}
		}
		private void DestroyOperatorSquadUI()
		{
			for (int i = 0; i < m_OperatorSquadUIList.Count; ++i)
			{
				m_OperatorSquadUIList[i].onOperatorSquadUIClicked -= OnOperatorSquadUIClicked;

				Despawn(m_OperatorSquadUIList[i]);
			}
			m_OperatorSquadUIList.Clear();
		}

		private void UpdateUI()
		{
			#region 코스트 관련 UI
			costFillImage.fillAmount = M_GamePlaying.costTimer.progress;
			costValueText.text = M_GamePlaying.currentCost.ToString();
			#endregion

			#region 활성화된 오퍼레이터 스탯 정보 UI

			#endregion
		}

		public void OnFindingTile(OperatorSquadUI operatorSquadUI, Tile tile)
		{
			deploymentCancelButton.gameObject.SetActive(true);

			if (m_TileOperatorSquadUIMap.ContainsKey(tile) == false)
				m_TileOperatorSquadUIMap.Add(tile, operatorSquadUI);

			m_TileOperatorSquadUIMap[tile] = operatorSquadUI;
		}
		public void OnSettingDirectionEnd()
		{
			deploymentCancelButton.gameObject.SetActive(false);

			m_SelectedOperatorSquadUI.gameObject.SetActive(false);
			m_SelectedOperatorSquadUI = null;

			operatorStatusUI.gameObject.SetActive(false);
		}

		#region MikangMark
		public OperatorSquadUI GetOperatorSquadUI(Tile key)
		{
			if (m_TileOperatorSquadUIMap.TryGetValue(key, out OperatorSquadUI squadUI) == false)
				return null;

			return squadUI;
		}

		public Sprite GetOperatorFullshotSprite(string key)
		{
			return m_OperatorFullshotSpriteMap[key];
		}
		#endregion
	}
}