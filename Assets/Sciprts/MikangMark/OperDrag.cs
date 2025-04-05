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
		public GameObject m_OperatorPrefab;
		public GameObject m_Canvas;
		// 따라다닐 UI 오브젝트
		private RectTransform B;
		public bool isDragging = false;
		GameObject newB;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수
		private void Start()
		{
			m_Canvas = GameObject.Find("Canvas");

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
			isDragging = true;

			// B가 없으면 생성 (한 번만)
			if (B == null)
			{
				newB = Instantiate(m_OperatorPrefab, m_Canvas.transform);
				newB.GetComponent<DragOperSetPos>().enabled = false;
				B = newB.GetComponent<RectTransform>();
				newB.GetComponent<Operator>().m_OperData = GetComponent<Operator>().m_OperData;
				newB.GetComponent<Operator>().OperName = GetComponent<Operator>().OperName;
				newB.name = GetComponent<Operator>().OperName;
				UIManager.Instance.OperStatUISetActive(true);
			}
		}
		public void OnDrag(PointerEventData eventData)
		{
			if (!isDragging) return;
			B.position = eventData.position;

		}

		public void OnEndDrag(PointerEventData eventData)
		{
			isDragging = false;
			if (GameObject.Find("Fang").GetComponent<OperPoint>().IsOnTile)
			{
				gameObject.SetActive(false);
				UIManager.Instance.OperStatUISetActive(false);
				newB.GetComponent<DragOperSetPos>().enabled = true;
			}
			else
			{
				Destroy(newB);
				UIManager.Instance.OperStatUISetActive(false);
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