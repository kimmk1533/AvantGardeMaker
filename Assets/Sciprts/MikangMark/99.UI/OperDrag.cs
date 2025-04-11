using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;


namespace AvantGardeMaker.MikangMark
{
	public class OperDrag : SerializedMonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		#region 변수
		public GameObject OperatorPrefab;
		public GameObject Canvas;
		// 따라다닐 UI 오브젝트
		private RectTransform m_CreatedOperator;
		public bool IsDragging = false;
		GameObject m_NewCreatedOperator;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		private static UIManager M_UI => UIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Start()
		{
			Canvas = GameObject.Find("Canvas");

			Initialize();
		}
		#endregion

		/// <summary>
		/// 초기화 함수
		/// </summary>
		/// 
		// UI 생성
		public void Initialize()
		{
			
		}
		
		public void OnBeginDrag(PointerEventData eventData)
		{
			IsDragging = true;
			M_UI.m_Setting = IsDragging;
			// m_CreatedOperator가 없으면 생성 (한 번만)
			if (m_CreatedOperator == null)
			{
				m_NewCreatedOperator = Instantiate(OperatorPrefab, Canvas.transform);
				m_NewCreatedOperator.GetComponent<DragOperSetPos>().enabled = false;
				m_CreatedOperator = m_NewCreatedOperator.GetComponent<RectTransform>();
				m_NewCreatedOperator.GetComponent<Operator>().OperData = GetComponent<Operator>().OperData.Clone();
				m_NewCreatedOperator.GetComponent<Operator>().OperName = GetComponent<Operator>().OperName;
				m_NewCreatedOperator.name = GetComponent<Operator>().OperName;
				M_UI.OperStatUISetActive(true);
			}
		}
		public void OnDrag(PointerEventData eventData)
		{
			if (!IsDragging) return;
			m_CreatedOperator.position = eventData.position;

		}

		public void OnEndDrag(PointerEventData eventData)
		{
			IsDragging = false;
			M_UI.m_Setting = IsDragging;
			if (GameObject.Find("Fang").GetComponent<OperPoint>().IsOnTile)
			{
				gameObject.SetActive(false);
				M_UI.OperStatUISetActive(false);
				m_NewCreatedOperator.GetComponent<DragOperSetPos>().enabled = true;
				M_UI.CancelSetOperBtn.gameObject.SetActive(true);
				M_UI.CancelSetOperBtn.GetComponent<RectTransform>().position = new Vector3(m_NewCreatedOperator.GetComponent<RectTransform>().position.x - 300, m_NewCreatedOperator.GetComponent<RectTransform>().position.y + 300);
			}
			else
			{
				Destroy(m_NewCreatedOperator);
				M_UI.OperStatUISetActive(false);
			}

		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{

		}


	}
}