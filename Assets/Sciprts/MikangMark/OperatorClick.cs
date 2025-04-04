using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AvantGardeMaker.MikangMark
{
	public class OperatorClick : SerializedMonoBehaviour
	{
		#region 변수
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{

		}
		public void OnPointerDown()
		{
			UIManager.Instance.m_SelectOperator.m_OperData = GetComponent<Operator>().m_OperData;
			if (UIManager.Instance.m_OperStatUI.activeSelf == false)
			{
				UIManager.Instance.OperStatUISetActive(true);
			}
			else
			{
				UIManager.Instance.OperStatUISetActive(false);
			}
			
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