using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace;
using AvantGardeMaker.TileSpace;
using AvantGardeMaker.OperatorSpace;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace AvantGardeMaker.UI
{
	public class OperatorSquadUI : GamePlayingUI, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		#region 변수
		private Button m_Button = null;

		private OperatorData m_OperatorData = null;

		private Operator m_PreviewOperator = null;
		private RectTransform m_RedeploymentParent = null;

		private UtilClass.Timer m_ReDeploymentTimer = null;
		private TextMeshProUGUI m_ReDeploymentTimerText = null;
		[SerializeField, RuntimeReadOnly]
		private Image m_ReDeploymentTimerImage = null;

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

				m_PreviewOperator = M_Operator.GetBuilder(operatorData.key)
					.SetAutoInit(true)
					.SetActive(false)
					.Spawn();

				m_PreviewOperator.operatorData = operatorData;
			}
		}

		private bool isWaitingRedeploy => m_Button.interactable;
		private int deployRequiredCost => m_PreviewOperator.deploymentCost;
		private bool isDeployable => isWaitingRedeploy && M_GamePlaying.currentCost >= deployRequiredCost;
		#endregion

		#region 이벤트
		public event System.Action<OperatorSquadUI> onOperatorSquadUIClicked;

		#region 이벤트 함수
		private void OnCostIncreased()
		{
			if (operatorData == null)
				return;

			m_Button.interactable = M_GamePlaying.currentCost >= deployRequiredCost;
		}
		private void OnOperatorSquadUIButtonClicked()
		{
			onOperatorSquadUIClicked?.Invoke(this);
		}
		#endregion
		#endregion

		#region 매니저
		private static GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;
		private static GamePlayingUIManager M_GamePlayingUI => GamePlayingUIManager.Instance;

		private static OperatorManager M_Operator => OperatorManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			UpdateReDeploymentTimer();
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
			if (m_RedeploymentParent == null)
				m_RedeploymentParent = transform.Find<RectTransform>("Redeployment Parent");
			if (m_ReDeploymentTimerImage == null)
				m_ReDeploymentTimerImage = m_RedeploymentParent.transform.Find<Image>("ResponeTimer Image");
			if (m_ReDeploymentTimerText == null)
				m_ReDeploymentTimerText = m_RedeploymentParent.transform.Find<TextMeshProUGUI>("ResponeTimer Text");
			if (m_ReDeploymentTimer == null)
				m_ReDeploymentTimer = new UtilClass.Timer();

			m_Button.onClick.AddListener(OnOperatorSquadUIButtonClicked);
			M_GamePlaying.onCostIncreased += OnCostIncreased;
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void FinallizePoolItem()
		{
			base.FinallizePoolItem();

			m_Button.onClick.RemoveListener(OnOperatorSquadUIButtonClicked);
			M_GamePlaying.onCostIncreased -= OnCostIncreased;

			M_Operator.Despawn(m_PreviewOperator);
		}
		#endregion

		//오퍼레이터 프리뷰 생성
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (isDeployable == false)
				return;

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

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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

			Vector3 tilePos = tile.transform.position;
			tilePos.z = -0.49f;
			m_PreviewOperator.transform.position = tilePos;

			tile.operatorSquadUI = this;
			M_GamePlaying.setPreViewOperatorOnTile = tile;

			M_GamePlayingUI.OnSettingDirectionStart();
		}

		public void CancelDeployment()
		{
			m_PreviewOperator.gameObject.SetActive(false);
		}

		public void ReDeploymentActiveObject()
		{
			m_RedeploymentParent.gameObject.SetActive(true);

			m_Button.interactable = M_GamePlaying.currentCost >= deployRequiredCost;
		}

		private void UpdateReDeploymentTimer()
		{
			if (isWaitingRedeploy == true)
				return;

			float redeploymentInterval = (1f - m_ReDeploymentTimer.progress) * operatorData.VariableData.RedeploymentInterval;
			string redeploymentText = redeploymentInterval.ToString("F1");

			m_ReDeploymentTimer.Update();
			m_ReDeploymentTimerImage.fillAmount = m_ReDeploymentTimer.progress;
			m_ReDeploymentTimerText.text = redeploymentText;
			if (m_ReDeploymentTimer.TimeCheck(true))
			{
				Debug.Log("ReadyOperator");
				m_Button.interactable = M_GamePlaying.currentCost >= deployRequiredCost;
				m_RedeploymentParent.gameObject.SetActive(false);
			}
		}
	}
}