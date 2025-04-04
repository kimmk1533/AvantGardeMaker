using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.Ceeu.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class OptionPanel : Panel
	{
		#region 변수
		private RectTransform m_RectTransform = null;

		private ScrollRect m_ViewportParent = null;
		private OptionViewportController m_ViewportController = null;
		private OptionViewport m_CurrentViewport = null;

		private Scrollbar m_ScrollBar = null;
		private Button m_CloseButton = null;

		private SaveButton m_SaveButton = null;
		private LoadButton m_LoadButton = null;
		#endregion

		#region 프로퍼티
		public OptionViewportController optionViewportController => m_ViewportController;
		public OptionViewport currentViewport => m_CurrentViewport;
		#endregion

		#region 이벤트

		#region 이벤트 함수
		public void OnChangedViewport(OptionViewport viewport)
		{
			m_ViewportParent.viewport = viewport.rectTransform;
			m_ViewportParent.content = viewport.content;
		}
		public void OnCloseButtonClicked()
		{
			gameObject.SetActive(false);

			if (m_CurrentViewport != null)
				m_CurrentViewport.gameObject.SetActive(false);
			m_CurrentViewport = null;
		}
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
		public override void Initialize()
		{
			base.Initialize();

			if (m_RectTransform == null)
				m_RectTransform = GetComponent<RectTransform>();

			if (m_ViewportParent == null)
				m_ViewportParent = m_RectTransform.Find<ScrollRect>("Option Viewports");

			if (m_ViewportController == null)
			{
				m_ViewportController = m_ViewportParent.GetComponent<OptionViewportController>();
				m_ViewportController.Initialize(this);
			}

			m_CurrentViewport = null;

			if (m_ScrollBar == null)
				m_ScrollBar = m_RectTransform.Find<Scrollbar>("Scrollbar Vertical");
			if (m_CloseButton == null)
			{
				m_CloseButton = m_RectTransform.Find<Button>("Close Button");
				m_CloseButton.onClick.AddListener(OnCloseButtonClicked);
			}

			if (m_SaveButton == null)
			{
				m_SaveButton = m_RectTransform.FindInChildren<SaveButton>("Save Button");

				m_SaveButton.Initialize();
			}
			if (m_LoadButton == null)
			{
				m_LoadButton = m_RectTransform.FindInChildren<LoadButton>("Load Button");

				m_LoadButton.Initialize();
			}

			gameObject.SetActive(false);
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{
			base.Finallize();

			m_LoadButton.Finallize();
			m_SaveButton.Finallize();

			m_ViewportController.Finallize();
		}
		#endregion

		public void ChangeCurrentViewport(OptionViewport viewport)
		{
			m_CurrentViewport = viewport;
		}
	}
}