using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace;
using AvantGardeMaker.TileSpace;
using AvantGardeMaker.OperatorSpace;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

namespace AvantGardeMaker.UI
{
	public class OperatorSquadUI : GamePlayingSceneUIPoolItem, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		#region 변수
		private Button m_Button = null;

		private OperatorData m_OperatorData = null;

		private Operator m_PreviewOperator = null;

		private TextMeshProUGUI m_CostText = null;

		private RectTransform m_RedeploymentParent = null;
		private UtilClass.Timer m_RedeploymentTimer = null;
		private TextMeshProUGUI m_RedeploymentTimerText = null;
		private Image m_RedeploymentTimerImage = null;

		private bool m_IsDragging = false;
		#endregion

		#region 프로퍼티
		public OperatorData operatorData
		{
			get => m_OperatorData;
			set
			{
				m_OperatorData = value;

				if (m_PreviewOperator != null)
					M_Operator.Despawn(m_PreviewOperator);

				m_PreviewOperator = M_Operator.GetBuilder(value.key)
					.SetAutoInit(true)
					.SetActive(false)
					.Spawn();
				m_PreviewOperator.operatorData = value;

				UpdateInfoUI();
			}
		}

		private bool isWaitingRedeployment
		{
			get => m_RedeploymentParent.gameObject.activeSelf;
			set => m_RedeploymentParent.gameObject.SetActive(value);
		}
		private int deployRequiredCost => m_PreviewOperator.variableData.DeploymentCost;
		private bool isDeployable => !isWaitingRedeployment && M_GamePlaying.currentCost >= deployRequiredCost;
		#endregion

		#region 이벤트
		public event System.Action<OperatorSquadUI> onOperatorSquadUIClicked = null;

		#region 이벤트 함수
		private void OnCostChanged(int currentCost)
		{
			if (operatorData == null)
				return;

			UpdateButtonInteractable();
		}
		#endregion
		#endregion

		#region 매니저
		private static GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;
		private static GamePlayingSceneUIManager M_GamePlayingUI => GamePlayingSceneUIManager.Instance;

		private static OperatorManager M_Operator => OperatorManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			UpdateRedeploymentTimer();
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public override void InitializePoolItem()
		{
			base.InitializePoolItem();

			if (m_Button == null)
				m_Button = GetComponent<Button>();

			if (m_CostText == null)
				m_CostText = transform.Find<TextMeshProUGUI>("Operator Info/Cost/Text");

			if (m_RedeploymentParent == null)
				m_RedeploymentParent = transform.Find<RectTransform>("Redeployment Parent");
			if (m_RedeploymentTimerImage == null)
				m_RedeploymentTimerImage = m_RedeploymentParent.Find<Image>("RedeploymentTimer Image");
			if (m_RedeploymentTimerText == null)
				m_RedeploymentTimerText = m_RedeploymentParent.Find<TextMeshProUGUI>("RedeploymentTimer Text");
			if (m_RedeploymentTimer == null)
				m_RedeploymentTimer = new UtilClass.Timer();

			M_GamePlaying.onCostChanged += OnCostChanged;
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void FinallizePoolItem()
		{
			base.FinallizePoolItem();

			M_GamePlaying.onCostChanged -= OnCostChanged;

			M_Operator.Despawn(m_PreviewOperator);
		}
		#endregion

		public void OnPointerClick(PointerEventData eventData)
		{
			onOperatorSquadUIClicked?.Invoke(this);
		}
		//오퍼레이터 프리뷰 생성
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (isDeployable == false)
				return;

			if (M_GamePlayingUI.selectedOperatorSquadUI != this)
				M_GamePlayingUI.OnOperatorSquadUIClicked(this);

			Vector3 mousePos = UtilClass.GetMouseWorldPosition3D();

			m_PreviewOperator.transform.position = mousePos;
			m_PreviewOperator.gameObject.SetActive(true);

			m_IsDragging = true;
		}
		public void OnDrag(PointerEventData eventData)
		{
			if (m_IsDragging == false)
				return;
			if (isDeployable == false)
				return;

			Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);

			if (Physics.Raycast(ray, out RaycastHit hit) == false)
			{
				Vector3 mousePos = UtilClass.GetMouseWorldPosition3D();

				m_PreviewOperator.transform.position = mousePos;
			}
			else
			{
				Vector3 tilePos = hit.transform.position;
				tilePos.z = -0.49f;

				m_PreviewOperator.transform.position = tilePos;
			}
		}
		public void OnEndDrag(PointerEventData eventData)
		{
			if (m_IsDragging == false)
				return;

			m_IsDragging = false;

			if (isDeployable == false)
				return;

			Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
			if (Physics.Raycast(ray, out RaycastHit hit) == false)
			{
				m_PreviewOperator.gameObject.SetActive(false);

				M_GamePlayingUI.OnOperatorSquadUIClicked(this);

				return;
			}

			GameObject hitObj = hit.collider.gameObject;

			Tile tile = hitObj.GetComponent<Tile>();
			if (tile == null)
				return;

			tile.DeployOperator(m_PreviewOperator);

			M_GamePlayingUI.OnFindingTile(this, tile);
		}

		public void StartRedeployment()
		{
			m_RedeploymentParent.gameObject.SetActive(true);

			isWaitingRedeployment = true;

			m_RedeploymentTimer.interval = m_PreviewOperator.variableData.RedeploymentInterval;
			m_RedeploymentTimer.Clear();
			m_RedeploymentTimer.Resume();

			UpdateInfoUI();
		}
		public void CancelDeployment()
		{
			m_PreviewOperator.gameObject.SetActive(false);
		}

		private void UpdateInfoUI()
		{
			UpdateButtonInteractable();

			m_CostText.text = deployRequiredCost.ToString();
		}
		private void UpdateButtonInteractable()
		{
			if (isWaitingRedeployment == true)
				m_Button.interactable = false;
			else
				m_Button.interactable = M_GamePlaying.currentCost >= deployRequiredCost;
		}
		private void UpdateRedeploymentTimer()
		{
			if (m_RedeploymentTimer.isPaused)
				return;

			float redeploymentInterval = (1f - m_RedeploymentTimer.progress) * m_PreviewOperator.variableData.RedeploymentInterval;
			string redeploymentText = redeploymentInterval.ToString("F1");

			m_RedeploymentTimer.Update();
			m_RedeploymentTimerImage.fillAmount = m_RedeploymentTimer.progress;
			m_RedeploymentTimerText.text = redeploymentText;

			if (m_RedeploymentTimer.TimeCheck(true))
			{
				m_Button.interactable = M_GamePlaying.currentCost >= deployRequiredCost;

				isWaitingRedeployment = false;

				m_RedeploymentTimer.Pause();
			}
		}
	}
}