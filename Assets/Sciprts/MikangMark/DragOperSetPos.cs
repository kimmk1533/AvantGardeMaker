using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.MikangMark
{
	public class DragOperSetPos : SerializedMonoBehaviour
	{
		private Vector2 startPos;
		private Vector2 lastPos;
		private string lastDirection = "";
		#region 변수
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
			
		}
		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				startPos = Input.mousePosition;
				lastPos = startPos;
				lastDirection = "";
			}

			// 드래그 중일 때
			if (Input.GetMouseButton(0))
			{
				Vector2 currentPos = Input.mousePosition;
				Vector2 delta = currentPos - lastPos;

				// 움직임이 어느 정도 있어야 방향 판정
				if (delta.magnitude > 10f)
				{
					string direction = GetDirection(delta);

					// 방향이 바뀐 경우에만 호출
					if (direction != lastDirection)
					{
						lastDirection = direction;
						TriggerDirectionEvent(direction);
					}

					lastPos = currentPos;
				}
			}
		}
		#endregion
		string GetDirection(Vector2 delta)
		{
			if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
				return delta.x > 0 ? "Right" : "Left";
			else
				return delta.y > 0 ? "Up" : "Down";
		}

		void TriggerDirectionEvent(string direction)
		{
			switch (direction)
			{
				case "Up":
					OnDragUp();
					break;
				case "Down":
					OnDragDown();
					break;
				case "Left":
					OnDragLeft();
					break;
				case "Right":
					OnDragRight();
					break;
			}
		}

		void OnDragUp() => Debug.Log("↑ 실시간 위쪽 드래그");
		void OnDragDown() => Debug.Log("↓ 실시간 아래쪽 드래그");
		void OnDragLeft() => Debug.Log("← 실시간 왼쪽 드래그");
		void OnDragRight() => Debug.Log("→ 실시간 오른쪽 드래그");
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