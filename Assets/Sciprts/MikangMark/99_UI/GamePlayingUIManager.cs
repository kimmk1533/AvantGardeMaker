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
		#region 코스트 관련 UI
		public TextMeshProUGUI m_CostText = null;
		public Image m_CostImage = null;
		#endregion

		[SerializeField]
		private RectTransform m_OperatorSquadUIParent = null;
		[SerializeField, ReadOnly]
		private OperatorSquadUI m_SelectedOperatorSquadUI = null;
		[SerializeField, ReadOnly]
		private List<OperatorSquadUI> m_OperatorSquadUIList = null;

		[SerializeField]
		private OperatorStatusUI m_OperatorStatusUI = null;

		[SerializeField]
		private Button m_DeploymentCancelButton = null;

		#region 오퍼레이터 스탯 관련 UI

		[SerializeField]
		private Button OperatorRetreateButton = null;
		

		[SerializeField]
		private Camera mainCamera = null;

		private Dictionary<string, Sprite> m_OperatorFullshotSpriteMap = null;
		#endregion

		[SerializeField]
		private bool isActiveRetreateButton = false;

		#region MikangMark
		

		
		#endregion
		#endregion

		#region 프로퍼티
		public bool activeRetreateButton
		{
			get => isActiveRetreateButton;
			set => isActiveRetreateButton = value;
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
				m_OperatorStatusUI.gameObject.SetActive(false);

				m_SelectedOperatorSquadUI = null;

				return;
			}

			m_SelectedOperatorSquadUI = operatorSquadUI;
			m_OperatorStatusUI.selectedOperator = m_SelectedOperatorSquadUI.operatorData;
			m_OperatorStatusUI.ChangeOperatorStatusUI(m_OperatorStatusUI.selectedOperator);
			m_OperatorStatusUI.gameObject.SetActive(true);
		}
		//배치취소버튼클릭
		public void OnDeploymentCancelButtonClicked()
		{
			m_DeploymentCancelButton.gameObject.SetActive(false);

			m_SelectedOperatorSquadUI.CancelDeployment();
			m_SelectedOperatorSquadUI = null;

			m_OperatorStatusUI.gameObject.gameObject.SetActive(false);
		}
		#endregion
		#endregion

		#region 매니저
		private static GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;
		private static OperatorManager M_Operator => OperatorManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Start()
		{
			//m_DeploymentCancelButton.gameObject.SetActive(false);
			//m_CreatedAttackRangeHighlightList = new List<GameObject>();
			//SetActiveOperatorStatUI(false);

			//OperATKRangeCreate(m_SelectedOperatorSquadUI.operatorData.AttackPos);
			//AlignChildren();

			Initialize();
			InitializeMain();
		}
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
			Sprite[] m_OperatorFullshotImg = null;
			if (m_OperatorSquadUIParent == null)
				m_OperatorSquadUIParent = GameObject.Find("OperBox").transform as RectTransform;

			m_OperatorSquadUIList = new List<OperatorSquadUI>();
			m_OperatorFullshotSpriteMap = new Dictionary<string, Sprite>();
			m_OperatorFullshotImg = Resources.LoadAll<Sprite>("MikangMark/Test Images/OperatorFullImg");
			for(int i = 0;i< m_OperatorFullshotImg.Length; i++)
			{
				m_OperatorFullshotSpriteMap.Add(m_OperatorFullshotImg[i].name, m_OperatorFullshotImg[i]);
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
		/// 게임 초기화 함수 (본인 Main Scene 진입 시 호출)
		/// </summary>
		public override void InitializeMain()
		{
			base.InitializeMain();

			CreateOperatorSquadUI();

			m_DeploymentCancelButton.onClick.AddListener(OnDeploymentCancelButtonClicked);
		}
		/// <summary>
		/// 게임 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();

			DestroyOperatorSquadUI();

			m_DeploymentCancelButton.onClick.RemoveListener(OnDeploymentCancelButtonClicked);
		}
		#endregion

		private void CreateOperatorSquadUI()
		{
			List<string> operatorSquadKeyList = M_GamePlaying.operatorSquadKeyList;

			for (int i = 0; i < operatorSquadKeyList.Count; ++i)
			{
				string operatorKey = operatorSquadKeyList[i];
				OperatorSquadUI operatorSquadUI = GetBuilder("Operator Squad UI")
					.SetParent(m_OperatorSquadUIParent)
					.SetAutoInit(false)
					.SetActive(true)
					.SetName(operatorKey)
					.Spawn<OperatorSquadUI>();

				operatorSquadUI.onOperatorSquadUIClicked += OnOperatorSquadUIClicked;
				operatorSquadUI.operatorData = M_Operator.GetOperatorData(operatorKey);

				operatorSquadUI.InitializePoolItem();

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
			m_CostImage.fillAmount = M_GamePlaying.costTimer.progress;
			m_CostText.text = M_GamePlaying.currentCost.ToString();
			#endregion

			#region 활성화된 오퍼레이터 스탯 정보 UI
			
			#endregion
		}

		public void OnSettingDirectionStart()
		{
			m_DeploymentCancelButton.gameObject.SetActive(true);
		}
		public void OnSettingDirectionEnd()
		{
			m_DeploymentCancelButton.gameObject.SetActive(false);

			m_SelectedOperatorSquadUI.gameObject.SetActive(false);
			m_SelectedOperatorSquadUI = null;

			m_OperatorStatusUI.gameObject.SetActive(false);
		}

		#region MikangMark
		public void OperatorRetreateButtonSetPosition()
		{
			Vector3 screenPos = mainCamera.WorldToScreenPoint(M_GamePlaying.settedOperatorSelect.transform.position);
			OperatorRetreateButton.GetComponent<RectTransform>().position = new Vector3(screenPos.x - 200, screenPos.y + 200);
		}

		public void OperatorRetreateButtonActive(bool activeFlag)
		{
			OperatorRetreateButton.gameObject.SetActive(activeFlag);
		}

		public void SettingOperatorRetreateButton(Tile selectedTile)
		{
			OperatorRetreateButton.onClick.AddListener(selectedTile.RetreatOperatorOnTile);
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