using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.MikangMark.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.MikangMark
{
	public class DragOperSetPos : SerializedMonoBehaviour
	{
		private Vector2 m_DragStartPos;
		private bool m_IsDragging = false;
		private bool m_IsValidDrag = false;

		public float DragThreshold = 150f; // 드래그로 인정할 최소 거리 (픽셀)
		public E_OperatorDirection m_CurrentDirection = E_OperatorDirection.None;
		public E_OperatorDirection m_LastDirection = E_OperatorDirection.Left;

		private static Operator m_thisOper;

		#region 변수
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
			m_thisOper = GetComponent<Operator>();
		}
		private void Update()
		{
			if (Input.GetMouseButtonDown(0))//클릭했을때
			{
				m_DragStartPos = Input.mousePosition;
				m_IsDragging = true;
				m_IsValidDrag = false;
				m_CurrentDirection = E_OperatorDirection.None;
			}

			if (Input.GetMouseButton(0) && m_IsDragging)//드래그중일때
			{
				Vector2 currentPos = Input.mousePosition;
				Vector2 diff = currentPos - m_DragStartPos;

				if (!m_IsValidDrag)
				{
					if (diff.magnitude >= DragThreshold)//일정거리이상 드래그했을떄
					{
						m_IsValidDrag = true;
					}
				}
				if (m_IsValidDrag)
				{
					if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
					{
						m_CurrentDirection = (diff.x > 0) ? E_OperatorDirection.Right : E_OperatorDirection.Left;
					}
					else
					{
						m_CurrentDirection = (diff.y > 0) ? E_OperatorDirection.Up : E_OperatorDirection.Down;
					}
					M_UI.CancelSetOperBtn.gameObject.SetActive(false);
					m_thisOper.SetDirection(m_CurrentDirection, m_LastDirection);
				}
			}
			if (Input.GetMouseButtonUp(0))
			{
				m_IsDragging = false;
				m_IsValidDrag = false;
				m_CurrentDirection = E_OperatorDirection.None;
				M_UI.CancelSetOperBtn.gameObject.SetActive(true);
			}
		}
		#endregion
		
		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{

		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{

		}
		#endregion
	}
}