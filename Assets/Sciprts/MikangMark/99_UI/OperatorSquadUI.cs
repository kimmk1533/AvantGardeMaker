using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.MikangMark.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AvantGardeMaker.MikangMark
{
	public class OperatorSquadUI : GamePlayingUI, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		#region 변수
		private Button m_Button = null;
		#endregion

		#region 프로퍼티
		public OperatorData operatorData { get; set; }
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
		#endregion

		#region 유니티 콜백 함수
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

		public void OnBeginDrag(PointerEventData eventData)
		{
			Debug.Log("Begin Drag");

			//M_GamePlayingUI.m_IsDragging = IsDragging;

			//// m_CreatedOperator가 없으면 생성 (한 번만)
			//if (m_CreatedOperator == null)
			//{
			//	m_NewCreatedOperator = Instantiate(OperatorPrefab, Canvas.transform);

			//	m_NewCreatedOperator.GetComponent<DragOperSetPos>().enabled = false;

			//	m_CreatedOperator = m_NewCreatedOperator.GetComponent<RectTransform>();

			//	m_NewCreatedOperator.GetComponent<Operator>().m_OperatorData = GetComponent<Operator>().m_OperData.Clone();
			//	m_NewCreatedOperator.GetComponent<Operator>().operatorName = GetComponent<Operator>().operatorName;
			//	m_NewCreatedOperator.name = GetComponent<Operator>().operatorName;

			//	M_GamePlayingUI.SetActiveOperatorStatUI(true);
			//}
		}
		public void OnDrag(PointerEventData eventData)
		{
			Debug.Log("Drag");
			//m_CreatedOperator.position = eventData.position;
		}
		public void OnEndDrag(PointerEventData eventData)
		{
			Debug.Log("End Drag");
			//M_GamePlayingUI.m_IsDragging = IsDragging;

			//if (GameObject.Find("Fang").GetComponent<OperPoint>().IsOnTile)
			//{
			//	gameObject.SetActive(false);
			//	M_GamePlayingUI.SetActiveOperatorStatUI(false);
			//	m_NewCreatedOperator.GetComponent<DragOperSetPos>().enabled = true;
			//	M_GamePlayingUI.m_DeploymentCancelButton.gameObject.SetActive(true);
			//	M_GamePlayingUI.m_DeploymentCancelButton.GetComponent<RectTransform>().position = new Vector3(m_NewCreatedOperator.GetComponent<RectTransform>().position.x - 300, m_NewCreatedOperator.GetComponent<RectTransform>().position.y + 300);
			//}
			//else
			//{
			//	Destroy(m_NewCreatedOperator);
			//	M_GamePlayingUI.SetActiveOperatorStatUI(false);
			//}
		}

		private bool IsMouseOverObject(out RaycastHit hitInfo)
		{
			// 마우스 위치에서 Ray 생성
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			// Raycast 실행 (충돌 여부 검사)
			return Physics.Raycast(ray, out hitInfo);
		}
	}
}