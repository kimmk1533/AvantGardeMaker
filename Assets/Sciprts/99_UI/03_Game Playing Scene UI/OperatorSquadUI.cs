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

		private Operator m_PreviewOperator = null;
		private RectTransform m_ReDeploymentParent = null;
		private bool m_CanDeployment = true;

		private UtilClass.Timer m_ReDeploymentTimer = null;
		private TextMeshProUGUI m_ReDeploymentTimerText = null;
		[SerializeField, RuntimeReadOnly]
		private Image m_ReDeploymentTimerImage = null;

		private OperatorData m_OperatorData = null;

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
		#endregion

		#region 이벤트
		public event System.Action<OperatorSquadUI> onOperatorSquadUIClicked;

		#region 이벤트 함수
		private void OnOperatorSquadUIButtonClicked()
		{
			onOperatorSquadUIClicked?.Invoke(this);
		}
		#endregion
		#endregion

		#region 매니저
		private static GamePlayingUIManager M_GamePlayingUI => GamePlayingUIManager.Instance;
		private static OperatorManager M_Operator => OperatorManager.Instance;
		private static GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;
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
			m_Button.onClick.AddListener(OnOperatorSquadUIButtonClicked);
			if (m_ReDeploymentParent == null)
				m_ReDeploymentParent = transform.Find<RectTransform>("Redeployment Parent");
			m_ReDeploymentTimerImage = m_ReDeploymentParent.transform.Find<Image>("ResponeTimer Image");
			m_ReDeploymentTimerText = m_ReDeploymentParent.transform.Find<TextMeshProUGUI>("ResponeTimer Text");
			if (m_ReDeploymentTimer == null)
			{
				m_ReDeploymentTimer = new UtilClass.Timer();
			}
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void FinallizePoolItem()
		{
			base.FinallizePoolItem();

			m_Button.onClick.RemoveListener(OnOperatorSquadUIButtonClicked);
		}
		#endregion
		//오퍼레이터 프리뷰 생성
		public void OnBeginDrag(PointerEventData eventData)
		{
			M_GamePlayingUI.OnOperatorSquadUIClicked(this);

			if (m_CanDeployment == false)
				return;
			if (operatorData.VariableData.CurrentDeploymentCost > M_GamePlaying.currentCost)
				return;

			Vector3 mousePos = UtilClass.GetMouseWorldPosition2D();

			m_PreviewOperator = M_Operator.GetBuilder(operatorData.EngName)
				.SetPosition(mousePos)
				.SetAutoInit(false)
				.SetActive(true)
				.Spawn();

			m_PreviewOperator.SetOperatorData(operatorData);
			m_PreviewOperator.InitializePoolItem();
		}
		public void OnDrag(PointerEventData eventData)
		{
			if (m_CanDeployment == false)
				return;

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out RaycastHit hit) == false)
			{
				Vector3 mousePos = UtilClass.GetMouseWorldPosition2D();

				m_PreviewOperator.transform.position = mousePos;
			}
			else
			{
				m_PreviewOperator.transform.position = hit.transform.position;
			}
		}
		public void OnEndDrag(PointerEventData eventData)
		{
			if (m_CanDeployment == false)
				return;

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out RaycastHit hit) == false)
			{
				M_Operator.Despawn(m_PreviewOperator);

				return;
			}

			GameObject hitObj = hit.collider.gameObject;

			Tile tile = hitObj.GetComponent<Tile>();
			if (tile == null)
				return;
			tile.operatorSquadUI = this;
			m_PreviewOperator.transform.position = tile.transform.position;
			M_GamePlaying.setPreViewOperatorOnTile = tile;
			M_GamePlayingUI.OnSettingDirectionStart();
		}

		public void CancelDeployment()
		{
			M_Operator.Despawn(m_PreviewOperator);
			m_PreviewOperator = null;
		}

		public void ReDeploymentActiveObject()
		{
			m_ReDeploymentParent.gameObject.SetActive(true);
			m_CanDeployment = false;
		}

		private void UpdateReDeploymentTimer()
		{
			if (m_CanDeployment == true)
				return;

			m_ReDeploymentTimer.Update();
			m_ReDeploymentTimerImage.fillAmount = (operatorData.VariableData.RedeploymentInterval / operatorData.VariableData.RedeploymentInterval) - m_ReDeploymentTimer.progress;//1~0 실제 5.0~0.0
			m_ReDeploymentTimerText.text = (operatorData.VariableData.RedeploymentInterval - (m_ReDeploymentTimer.progress * operatorData.VariableData.RedeploymentInterval)).ToString("F1");
			if (m_ReDeploymentTimer.TimeCheck(true))
			{
				Debug.Log("ReadyOperator");
				m_CanDeployment = true;
				m_ReDeploymentParent.gameObject.SetActive(false);
			}
		}
	}
}