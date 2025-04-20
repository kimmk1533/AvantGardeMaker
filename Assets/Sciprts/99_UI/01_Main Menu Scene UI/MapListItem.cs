using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace.SaveLoad;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class MapListItem : MainMenuUI
	{
		#region 변수
		private Button m_SelfButton = null;
		private RawImage m_ThumnailImage = null;
		private TMP_Text m_TitleText = null;
		private TMP_Text m_CreatorText = null;
		//private Rating m_Rating = null;

		private StageData m_StageData = default;
		#endregion

		#region 프로퍼티
		public StageData stageData
		{
			get => m_StageData;
			set => m_StageData = value;
		}

		public Texture thumnailImage => m_ThumnailImage.texture;
		public string titleText => m_TitleText.text;
		public string creatorText => m_CreatorText.text;
		//public Rating rating => m_Rating;
		#endregion

		#region 이벤트
		public event System.Action<MapListItem> onClick;

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

			if (m_SelfButton == null)
			{
				m_SelfButton = GetComponent<Button>();

				m_SelfButton.onClick.AddListener(() =>
				{
					onClick?.Invoke(this);
				});
			}
			if (m_ThumnailImage == null)
				m_ThumnailImage = transform.Find<RawImage>("Thumnail Image");
			if (m_TitleText == null)
				m_TitleText = transform.Find<TMP_Text>("Title Text");
			if (m_CreatorText == null)
				m_CreatorText = transform.Find<TMP_Text>("Creator Text");
			//if (m_Rating == null)
			//{
			//	m_Rating = transform.Find<Rating>("Rating");
			//}
			//m_Rating.Initialize();
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void FinallizePoolItem()
		{
			base.FinallizePoolItem();

			m_ThumnailImage.texture = null;
			m_TitleText.text = "";
			m_CreatorText.text = "";
			//m_Rating.Finallize();

			onClick = null;
		}
		#endregion

		public void UpdateUI()
		{
			string creator = m_StageData.creatorNickName;

			if (creator == string.Empty)
				creator = "Unknown Creator";

			if (creator.StartsWith("by. ") == false)
				creator = creator.Insert(0, "by. ");

			m_ThumnailImage.texture = m_StageData.GetThumnailTexture();
			m_TitleText.text = m_StageData.title;
			m_CreatorText.text = creator;
		}
	}
}