using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class Rating : SerializedMonoBehaviour
	{
		#region 변수
		private Image[] m_RatingImageArr = null;

		private float m_Rating = 0f;
		#endregion

		#region 프로퍼티
		#region 인덱서
		public Image this[int index]
		{
			get => m_RatingImageArr[index];
		}
		#endregion

		public float value
		{
			get => m_Rating;
			set
			{
				m_Rating = Mathf.Clamp(value, 0f, m_RatingImageArr.Length);

				SetRating(m_Rating);
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
		public void Initialize()
		{
			if (m_RatingImageArr == null)
			{
				m_RatingImageArr = new Image[5];

				for (int i = 0; i < 5; ++i)
				{
					m_RatingImageArr[i] = transform.Find("Rating Image (" + i.ToString() + ")").Find<Image>("Rating Image");
				}
			}

			value = 0f;
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{
			for (int i = 0; i < 5; ++i)
			{
				m_RatingImageArr[i].sprite = null;
			}
		}
		#endregion

		private void ClearRating()
		{
			// 초기화
			for (int i = 0; i < 5; ++i)
				m_RatingImageArr[i].fillAmount = 0f;
		}
		private void SetRating(float value)
		{
			ClearRating();

			if (value <= 0f)
				return;

			int index = 0;

			// 값 대입
			for (index = 0; index < value; ++index)
			{
				m_RatingImageArr[index].fillAmount = 1.0f;
			}
			m_RatingImageArr[index].fillAmount = value - (int)value;
		}
	}
}