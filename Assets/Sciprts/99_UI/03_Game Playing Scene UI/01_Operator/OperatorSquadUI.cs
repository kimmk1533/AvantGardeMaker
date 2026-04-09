using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace;
using AvantGardeMaker.OperatorSpace;
using AvantGardeMaker.TileSpace;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class OperatorSquadUI : GamePlayingSceneUIPoolItem, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		#region 기본 템플릿
		#region 변수
		private OperatorData m_OperatorData = null;

		private Operator m_PreviewOperator = null;

		private TextMeshProUGUI m_CostText = null;
		private RectTransform m_RedeploymentParent = null;
		private UtilClass.Timer m_RedeploymentTimer = null;
		private TextMeshProUGUI m_RedeploymentTimerText = null;
		private Image m_RedeploymentTimerImage = null;
		private Image m_DeployableScreenPanel = null;

		private bool m_IsDragging = false;
		#endregion

		#region 프로퍼티
		public OperatorData operatorData
		{
			get => m_OperatorData;
			set
			{
				m_OperatorData = value;
			}
		}

		private float deploymentCost => m_OperatorData.VariableData.DeploymentCost;
		private bool canDeploy => M_GamePlaying.currentCost >= deploymentCost;
		#endregion

		#region 매니저
		private static GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;
		private static GamePlayingSceneUIManager M_GamePlayingUI => GamePlayingSceneUIManager.Instance;

		private static OperatorManager M_Operator => OperatorManager.Instance;
		#endregion

		#region 이벤트
		public event System.Action<OperatorSquadUI> onClick = null;
		public event System.Action<OperatorSquadUI> onBeginDrag = null;
		public event System.Action<OperatorSquadUI> onEndDrag = null;
		public event System.Action<OperatorSquadUI, Tile> onSuccessDrag = null;
		public event System.Action<OperatorSquadUI> onFailedDrag = null;

		#region 이벤트 함수
		private void OnCostChanged(int currentCost)
		{
			if (m_OperatorData == null)
				return;

			m_DeployableScreenPanel.gameObject.SetActive(currentCost < deploymentCost);
		}
		#endregion
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (생성될 때)
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			m_CostText = transform.Find<TextMeshProUGUI>("Operator Info/Cost/Text");
			m_RedeploymentParent = transform.Find<RectTransform>("Redeployment Parent");
			m_RedeploymentTimer = new UtilClass.Timer();
			m_RedeploymentTimerImage = m_RedeploymentParent.Find<Image>("RedeploymentTimer Image");
			m_RedeploymentTimerText = m_RedeploymentParent.Find<TextMeshProUGUI>("RedeploymentTimer Text");
			m_DeployableScreenPanel = transform.Find<Image>("Deployable Screen Panel");
		}
		/// <summary>
		/// 마무리화 함수 (파괴될 때)
		/// </summary>
		public override void Finallize()
		{
			m_DeployableScreenPanel = null;
			m_RedeploymentTimerText = null;
			m_RedeploymentTimerImage = null;
			m_RedeploymentTimer = null;
			m_RedeploymentParent = null;
			m_CostText = null;

			base.Finallize();
		}

		/// <summary>
		/// 초기화 함수 (스폰될 때)
		/// </summary>
		public override void InitializePoolItem()
		{
			base.InitializePoolItem();

			m_CostText.text = deploymentCost.ToString();
			m_DeployableScreenPanel.gameObject.SetActive(M_GamePlaying.currentCost < deploymentCost);
			m_IsDragging = false;

			M_GamePlaying.onCostChanged += OnCostChanged;

			// 이 방법이 더 좋지 않을까?
			//onClick += M_GamePlayingUI.OnOperatorSquadUIClicked;
		}
		/// <summary>
		/// 마무리화 함수 (디스폰될 때)
		/// </summary>
		public override void FinallizePoolItem()
		{
			M_GamePlaying.onCostChanged -= OnCostChanged;

			onFailedDrag = null;
			onSuccessDrag = null;
			onEndDrag = null;
			onBeginDrag = null;
			onClick = null;

			m_IsDragging = false;

			base.FinallizePoolItem();
		}
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			UpdateRedeploymentTimer();
		}

		#region 유니티 인터페이스 함수
		public void OnPointerClick(PointerEventData eventData)
		{
			onClick?.Invoke(this);
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (canDeploy == false)
				return;

			if (M_GamePlayingUI.operatorStatusUI.selectedOperatorData != m_OperatorData)
			{
				M_GamePlayingUI.operatorStatusUI.selectedOperatorData = m_OperatorData;
				M_GamePlayingUI.operatorStatusUI.gameObject.SetActive(true);
			}

			m_PreviewOperator = M_Operator.GetBuilder(m_OperatorData.key)
				.SetAutoInit(true)
				.SetActive(true)
#if UNITY_EDITOR
				.SetParent(transform) // 디버깅용 부모 옮기기 (성능 때문에 나중에 제거하는게 좋음)
#endif
				.Spawn<PreviewOperator>();

			m_IsDragging = true;
			onBeginDrag?.Invoke(this);
		}
		public void OnDrag(PointerEventData eventData)
		{
			if (m_IsDragging == false)
				return;
			if (m_PreviewOperator == null)
				return;

			Vector3 previewOperatorPos = UtilClass.GetMouseWorldPosition3D();

			Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
			if (Physics.Raycast(ray, out RaycastHit hit) == true)
			{
				previewOperatorPos = hit.transform.position;
				previewOperatorPos.z = -0.49f;
			}

			m_PreviewOperator.transform.position = previewOperatorPos;
		}
		public void OnEndDrag(PointerEventData eventData)
		{
			if (m_IsDragging == false)
				return;
			m_IsDragging = false;

			if (m_PreviewOperator == null)
				return;

			Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
			// 배치할 타일을 못찾은 경우
			if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("Tile")) == false)
			{
				M_Operator.Despawn(m_PreviewOperator);
				m_PreviewOperator = null;

				onFailedDrag?.Invoke(this);
				onEndDrag?.Invoke(this);

				return;
			}
			
			// 배치할 타일을 찾은 경우
			Tile tile = hit.transform.GetComponent<Tile>();
			onSuccessDrag?.Invoke(this, tile);
			onEndDrag?.Invoke(this);

			// 방향 설정 단계가 남아있기 때문에 여기선 프리뷰 오퍼 디스폰 안함

			m_PreviewOperator = null;

			#region MikangMark
			//tile.DeployOperator(m_PreviewOperator);

			//M_GamePlayingUI.OnFindingTile(this, tile);
			#endregion
		}
		#endregion
		#endregion
		#endregion

		public void StartRedeployment()
		{
			m_RedeploymentParent.gameObject.SetActive(true);

			m_RedeploymentTimer.interval = m_PreviewOperator.variableData.RedeploymentInterval;
			m_RedeploymentTimer.Clear();
			m_RedeploymentTimer.Resume();

			m_CostText.text = deploymentCost.ToString();
		}
		private void UpdateRedeploymentTimer()
		{
			if (m_RedeploymentTimer.isPaused)
				return;

			if (m_RedeploymentTimer.Update(false))
			{
				//m_Button.interactable = M_GamePlaying.currentCost >= deploymentCost;

				m_RedeploymentParent.gameObject.SetActive(false);

				m_RedeploymentTimer.Pause();
				m_RedeploymentTimer.Clear();
			}

			// 실제 재배치 시간
			float redeploymentInterval = m_OperatorData.VariableData.RedeploymentInterval * (1f - m_RedeploymentTimer.progress);
			string redeploymentText = redeploymentInterval.ToString("F1");

			m_RedeploymentTimerText.text = redeploymentText;
			m_RedeploymentTimerImage.fillAmount = m_RedeploymentTimer.progress;
		}
	}
}