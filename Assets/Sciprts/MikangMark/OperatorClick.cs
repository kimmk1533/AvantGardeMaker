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
		private static UIManager M_UI => UIManager.Instance;
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
			M_UI.m_SelectOperator.m_OperData = GetComponent<Operator>().m_OperData;
			if (M_UI.m_OperStatUI.activeSelf == false)
			{
				M_UI.OperStatUISetActive(true);
			}
			else
			{
				M_UI.OperStatUISetActive(false);
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