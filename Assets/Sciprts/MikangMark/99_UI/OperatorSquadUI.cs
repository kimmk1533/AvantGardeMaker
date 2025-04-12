using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.Ceeu;
using AvantGardeMaker.Ceeu.Enum;
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

		private Operator m_PreviewOperator = null;
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
		private static OperatorManager M_Operator => OperatorManager.Instance;
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
			Vector3 mousePos = Input.mousePosition;
			mousePos = Camera.main.ScreenToWorldPoint(mousePos);
			mousePos.z = 0f;

			m_PreviewOperator = M_Operator.GetBuilder(operatorData.EngName)
				.SetPosition(mousePos)
				.SetAutoInit(false)
				.SetActive(true)
				.Spawn();

			m_PreviewOperator.SetOperatorData(operatorData);
			m_PreviewOperator.InitializePoolItem();

			M_GamePlayingUI.OnOperatorSquadUIClicked(this);
		}
		public void OnDrag(PointerEventData eventData)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out RaycastHit hit) == false)
			{
				Vector3 mousePos = Input.mousePosition;
				mousePos = Camera.main.ScreenToWorldPoint(mousePos);
				mousePos.z = 0f;

				m_PreviewOperator.transform.position = mousePos;
			}
			else
			{
				m_PreviewOperator.transform.position = hit.transform.position;
			}
		}
		public void OnEndDrag(PointerEventData eventData)
		{
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

			m_PreviewOperator.transform.position = tile.transform.position;
		}

		public void CancelDeployment()
		{
			M_Operator.Despawn(m_PreviewOperator);
			m_PreviewOperator = null;
		}
	}
}