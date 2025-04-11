using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.MikangMark;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace AvantGardeMaker
{
	public class OperPoint : SerializedMonoBehaviour
	{
		#region 변수
		public bool IsOnTile = false;

		public OperDrag box;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수
		private void Awake()
		{
			Initialize();
		}
		void Update()
		{
			if (IsMouseOverObject(out RaycastHit hit))
			{
				IsOnTile = true;
				Vector3 screenPosition = Camera.main.WorldToScreenPoint(hit.transform.position);
				// UI 위치 업데이트
				if (box.IsDragging)
				{
					gameObject.transform.position = screenPosition;
				}
				else
				{
					if (hit.collider.gameObject == null)
					{
						box.gameObject.SetActive(false);
					}
				}
			}
			else
			{
				IsOnTile = false;
			}
		}
		#endregion

		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{
			box = GameObject.Find("Fang_InBox").GetComponent<OperDrag>();

		}
		bool IsMouseOverObject(out RaycastHit hitInfo)
		{
			// 마우스 위치에서 Ray 생성
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			// Raycast 실행 (충돌 여부 검사)
			return Physics.Raycast(ray, out hitInfo);
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{

		}
	}
}