using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class MapListItem : MainMenuUI
	{
		#region 변수
		private Image m_ThumnailImage = null;
		private TMP_Text m_TitleText = null;
		private Image[] m_RatingImageArr = null;
		#endregion

		#region 프로퍼티
		public string title
		{
			get
			{
				return m_TitleText.text;
			}
			set
			{
				m_TitleText.text = value;
			}
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

			if (m_ThumnailImage == null)
				m_ThumnailImage = transform.Find<Image>("Thumnail Image");
			if (m_TitleText == null)
				m_TitleText = transform.Find<TMP_Text>("Title Text");
			if (m_RatingImageArr == null)
			{
				m_RatingImageArr = new Image[5];

				for (int i = 0; i < 5; ++i)
				{
					m_RatingImageArr[i] = transform.Find("Rating Images").Find<Image>("Rating Image (" + i.ToString() + ")");
				}
			}
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void FinallizePoolItem()
		{
			base.FinallizePoolItem();

			m_ThumnailImage.sprite = null;
			m_TitleText.text = "";
			for (int i = 0; i < 5; ++i)
			{
				m_RatingImageArr[i].sprite = null;
			}
		}
		#endregion
	}
}