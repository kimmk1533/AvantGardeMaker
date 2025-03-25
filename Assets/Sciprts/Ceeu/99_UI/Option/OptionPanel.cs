using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.Ceeu.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class OptionPanel : SerializedMonoBehaviour
	{
		#region 변수
		private RectTransform m_RectTransform = null;

		private ScrollRect m_ViewportParent = null;

		private OptionViewport m_CurrentViewport = null;

		private OptionViewport m_SystemOptionViewport = null;
		private OptionViewport m_TileOptionViewport = null;
		private OptionViewport m_OperatorOptionViewport = null;
		private OptionViewport m_EnemyOptionViewport = null;

		private Scrollbar m_ScrollBar = null;
		private Button m_CloseButton = null;
		#endregion

		#region 프로퍼티
		public OptionViewport currentViewport => m_CurrentViewport;

		public OptionViewport systemOptionViewport => m_SystemOptionViewport;
		public OptionViewport tileOptionViewport => m_TileOptionViewport;
		public OptionViewport operatorOptionViewport => m_OperatorOptionViewport;
		public OptionViewport enemyOptionViewport => m_EnemyOptionViewport;
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
		private static UIManager M_UI => UIManager.Instance;
		private static EditModeManager M_EditMode => EditModeManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{
			m_RectTransform = GetComponent<RectTransform>();

			m_ViewportParent = m_RectTransform.Find<ScrollRect>("Viewports");

			m_CurrentViewport = null;

			m_SystemOptionViewport = m_ViewportParent.transform.Find<OptionViewport>("System Option Viewport");
			m_TileOptionViewport = m_ViewportParent.transform.Find<OptionViewport>("Tile Option Viewport");
			m_OperatorOptionViewport = m_ViewportParent.transform.Find<OptionViewport>("Operator Option Viewport");
			m_EnemyOptionViewport = m_ViewportParent.transform.Find<OptionViewport>("Enemy Option Viewport");

			m_SystemOptionViewport.Initialize(this);
			m_TileOptionViewport.Initialize(this);
			m_OperatorOptionViewport.Initialize(this);
			m_EnemyOptionViewport.Initialize(this);

			M_UI.onSystemMenuButtonClicked.AddListener(m_SystemOptionViewport.OnMenuButtonClicked);
			M_UI.onTileMenuButtonClicked.AddListener(m_TileOptionViewport.OnMenuButtonClicked);
			M_UI.onOperatorMenuButtonClicked.AddListener(m_OperatorOptionViewport.OnMenuButtonClicked);
			M_UI.onEnemyMenuButtonClicked.AddListener(m_EnemyOptionViewport.OnMenuButtonClicked);

			M_UI.onSystemMenuButtonClicked.AddListener(() => M_EditMode.SetEditModeType(E_EditModeType.System));
			M_UI.onTileMenuButtonClicked.AddListener(() => M_EditMode.SetEditModeType(E_EditModeType.Tile));
			M_UI.onOperatorMenuButtonClicked.AddListener(() => M_EditMode.SetEditModeType(E_EditModeType.Operator));
			M_UI.onEnemyMenuButtonClicked.AddListener(() => M_EditMode.SetEditModeType(E_EditModeType.Enemy));

			m_ScrollBar = m_RectTransform.Find<Scrollbar>("Scrollbar Vertical");
			m_CloseButton = m_RectTransform.Find<Button>("Close Button");
			m_CloseButton.onClick.AddListener(OnCloseButtonClicked);

			gameObject.SetActive(false);
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{
			m_EnemyOptionViewport = null;
			m_TileOptionViewport = null;
		}
		#endregion

		public void ChangeCurrentViewport(OptionViewport viewport)
		{
			m_CurrentViewport = viewport;
		}
	}
}