using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.Ceeu
{
	public class Panel : SerializedMonoBehaviour
	{
		#region 변수
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트

		#region 이벤트 함수
		#endregion
		#endregion

		#region 매니저
		private static MapEditorUIManager M_MapEditorUI => MapEditorUIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void OnEnable()
		{
			M_MapEditorUI.RegisterPanel(this);
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public virtual void Initialize()
		{

		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public virtual void Finallize()
		{

		}
		#endregion
	}
}