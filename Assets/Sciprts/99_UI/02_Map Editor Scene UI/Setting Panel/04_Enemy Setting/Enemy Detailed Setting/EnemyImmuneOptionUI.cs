using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class EnemyImmuneOptionUI : MapEditingUIPoolItem
	{
		#region 변수
		private Image m_ImmuneIconImage = null;
		private TMP_Text m_ImmuneText = null;
		#endregion

		#region 프로퍼티
		public string text
		{
			get => m_ImmuneText.text;
			set => m_ImmuneText.text = value;
		}
		#endregion

		#region 이벤트

		#region 이벤트 함수
		#endregion
		#endregion

		#region 매니저
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

			if (m_ImmuneIconImage == null)
				m_ImmuneIconImage = transform.Find<Image>("Immune Icon Image");
			if (m_ImmuneText == null)
				m_ImmuneText = transform.Find<TMP_Text>("Immune Description Text");
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void FinallizePoolItem()
		{
			base.FinallizePoolItem();


		}
		#endregion
	}
}