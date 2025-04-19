using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.Ceeu;
using AvantGardeMaker.MikangMark.Enum;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.MikangMark
{
	public class GamePlayingUIManager : ObjectManager<GamePlayingUIManager, GamePlayingUI>
	{
		#region 변수

		[SerializeField, ReadOnly]
		private OperatorSquadUI m_SelectedOperatorSquadUI = null;
		[SerializeField, ReadOnly]
		private List<OperatorSquadUI> m_OperatorSquadUIList = null;

		private Dictionary<string, Sprite> m_OperatorFullshotSpriteMap = null;

		#endregion

		#region 프로퍼티
		// 현재 코스트 텍스트
		public TextMeshProUGUI costValueText { get; set; }
		// 코스트 게이지 이미지
		public Image costFillImage { get; set; }
		// 게임 플레잉 카메라
		public Camera gamePlayingCamera { get; set; }
		// 오퍼레이터 배치 UI 부모
		public RectTransform operatorSquadUIParent { get; set; }
		// 오퍼레이터 상태 UI
		public OperatorStatusUI operatorStatusUI { get; set; }
		// 배치 취소 버튼
		public Button deploymentCancelButton { get; set; }
		// 오퍼레이터 퇴각 버튼
		public Button operatorRetreatButton { get; set; }

		public bool activeRetreatButton
		{
			get => operatorRetreatButton.gameObject.activeSelf;
			set => operatorRetreatButton.gameObject.SetActive(value);
		}
		public OperatorSquadUI selectedOperatorSquadUI
		{
			get => m_SelectedOperatorSquadUI;
		}
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
			operatorStatusUI.selectedOperator = m_SelectedOperatorSquadUI.operatorData;
			operatorStatusUI.ChangeOperatorStatusUI(operatorStatusUI.selectedOperator);
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
			Sprite[] operatorFullshotSprites = Resources.LoadAll<Sprite>("MikangMark/Test Images/OperatorFullImg");
			for (int i = 0; i < operatorFullshotSprites.Length; i++)
			{
				m_OperatorFullshotSpriteMap.Add(operatorFullshotSprites[i].name, operatorFullshotSprites[i]);
			}
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
		}
		#endregion

		private void CreateOperatorSquadUI()
		{
			List<string> operatorSquadKeyList = M_GamePlaying.operatorSquadKeyList;

			for (int i = 0; i < operatorSquadKeyList.Count; ++i)
			{
				string operatorKey = operatorSquadKeyList[i];

				OperatorSquadUI operatorSquadUI = GetBuilder("Operator Squad UI")
					.SetParent(operatorSquadUIParent)
					.SetAutoInit(false)
					.SetActive(true)
					.SetName(operatorKey)
					.Spawn<OperatorSquadUI>();

				operatorSquadUI.operatorData = M_Operator.GetOperatorData(operatorKey);

				operatorSquadUI.InitializePoolItem();

				operatorSquadUI.onOperatorSquadUIClicked += OnOperatorSquadUIClicked;

				m_OperatorSquadUIList.Add(operatorSquadUI);
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

		public void OnSettingDirectionStart()
		{
			deploymentCancelButton.gameObject.SetActive(true);
		}
		public void OnSettingDirectionEnd()
		{
			deploymentCancelButton.gameObject.SetActive(false);

			m_SelectedOperatorSquadUI.gameObject.SetActive(false);
			m_SelectedOperatorSquadUI = null;

			operatorStatusUI.gameObject.SetActive(false);
		}

		#region MikangMark
		public void OperatorRetreateButtonSetPosition()
		{
			Vector3 screenPos = gamePlayingCamera.WorldToScreenPoint(M_GamePlaying.settedOperatorSelect.transform.position);
			operatorRetreatButton.transform.position = new Vector3(screenPos.x - 200, screenPos.y + 200);
		}

		public void SettingOperatorRetreateButton(Tile selectedTile)
		{
			operatorRetreatButton.onClick.AddListener(selectedTile.RetreatOperatorOnTile);
		}

		public void OperatorSquadUIReDeploymentActive(OperatorSquadUI targetOperatorSquadUI)
		{
			targetOperatorSquadUI.gameObject.SetActive(true);
			targetOperatorSquadUI.ReDeploymentActiveObject();
		}

		public Sprite GetOperatorFullshotSprite(string key)
		{
			return m_OperatorFullshotSpriteMap[key];
		}
		#endregion
	}
}