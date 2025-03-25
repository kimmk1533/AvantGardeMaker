using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class MenuPanel : SerializedMonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		#region 변수
		private RectTransform m_RectTransform = null;

		private float m_MovingOffset = 0f;

		private Button m_SystemMenuButton = null;
		private Button m_TileMenuButton = null;
		private Button m_OperatorMenuButton = null;
		private Button m_EnemyMenuButton = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		public Button.ButtonClickedEvent onSystemMenuButtonClicked => m_SystemMenuButton.onClick;
		public Button.ButtonClickedEvent onTileMenuButtonClicked => m_TileMenuButton.onClick;
		public Button.ButtonClickedEvent onOperatorMenuButtonClicked => m_OperatorMenuButton.onClick;
		public Button.ButtonClickedEvent onEnemyMenuButtonClicked => m_EnemyMenuButton.onClick;

		#region 이벤트 함수
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (M_UI.menuPanelLock == true)
				return;

			Vector3 position = m_RectTransform.anchoredPosition;
			position.y -= m_MovingOffset;
			m_RectTransform.anchoredPosition = position;
		}
		public void OnPointerExit(PointerEventData eventData)
		{
			if (M_UI.menuPanelLock == true)
				return;

			Vector3 position = m_RectTransform.anchoredPosition;
			position.y += m_MovingOffset;
			m_RectTransform.anchoredPosition = position;
		}
		#endregion
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
			m_RectTransform = GetComponent<RectTransform>();

			m_MovingOffset = m_RectTransform.sizeDelta.y * 0.5f + m_RectTransform.anchoredPosition.y;

			m_SystemMenuButton = transform.FindInChildren<Button>("System Menu");
			m_TileMenuButton = transform.FindInChildren<Button>("Tile Menu");
			m_OperatorMenuButton = transform.FindInChildren<Button>("Operator Menu");
			m_EnemyMenuButton = transform.FindInChildren<Button>("Enemy Menu");
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{
			onSystemMenuButtonClicked?.RemoveAllListeners();
			onTileMenuButtonClicked?.RemoveAllListeners();
			onOperatorMenuButtonClicked?.RemoveAllListeners();
			onEnemyMenuButtonClicked?.RemoveAllListeners();
		}
		#endregion
	}
}