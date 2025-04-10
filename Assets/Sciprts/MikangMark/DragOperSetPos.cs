using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.MikangMark.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.MikangMark
{
	public class DragOperSetPos : SerializedMonoBehaviour
	{
		//private Vector2 startPos;
		//private Vector2 lastPos;
		//private string lastDirection = "";

		private Vector2 dragStartPos;
		private bool isDragging = false;
		private bool isValidDrag = false;

		public float dragThreshold = 150f; // 드래그로 인정할 최소 거리 (픽셀)
		public E_OperatorDirection currentDirection = E_OperatorDirection.None;
		public E_OperatorDirection lastDirection = E_OperatorDirection.None;

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
			
		}
		private void Update()
		{
			if (Input.GetMouseButtonDown(0))//클릭했을때
			{
				dragStartPos = Input.mousePosition;
				isDragging = true;
				isValidDrag = false;
				currentDirection = E_OperatorDirection.None;
			}

			if (Input.GetMouseButton(0) && isDragging)//드래그중일때
			{
				Vector2 currentPos = Input.mousePosition;
				Vector2 diff = currentPos - dragStartPos;

				if (!isValidDrag)
				{
					if (diff.magnitude >= dragThreshold)//일정거리이상 드래그했을떄
					{
						isValidDrag = true;
					}
				}
				if (isValidDrag)
				{
					if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
					{
						currentDirection = (diff.x > 0) ? E_OperatorDirection.Right : E_OperatorDirection.Left;
					}
					else
					{
						currentDirection = (diff.y > 0) ? E_OperatorDirection.Up : E_OperatorDirection.Down;
					}
					M_UI.CancelSetOperBtn.gameObject.SetActive(false);
					lastDirection = currentDirection;
					GetComponent<Operator>().SetDirection(currentDirection, lastDirection);
				}
			}
			if (Input.GetMouseButtonUp(0))
			{
				isDragging = false;
				isValidDrag = false;
				currentDirection = E_OperatorDirection.None;
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