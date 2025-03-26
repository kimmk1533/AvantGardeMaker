using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AvantGardeMaker.MikangMark
{
	public class MouseControllScript : SerializedMonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        #region 변수
        private RectTransform m_RectTransform;
        private CanvasGroup m_CanvasGroup;
        private Vector2 m_ClickOffset;
        private Vector3 m_SavePos;
        #endregion

        #region 프로퍼티
        #endregion

        #region 이벤트
        #endregion

        #region 매니저
        #endregion

        #region 유니티 콜백 함수
        private void Start()
        {
            m_RectTransform = GetComponent<RectTransform>();
            m_CanvasGroup = GetComponent<CanvasGroup>();
        }
        #endregion

        /// <summary>
        /// 초기화 함수
        /// </summary>
        public void Initialize()
		{

		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{

		}

        public void OnPointerDown(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_RectTransform.parent.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out Vector2 localMousePosition);
            m_SavePos = m_RectTransform.localPosition;
            m_ClickOffset = (Vector2)m_RectTransform.localPosition - localMousePosition;
            m_CanvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // 마우스 드래그 이벤트가 발생한 위치를 UI 요소의 부모 객체의 좌표계로 변환합니다.
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_RectTransform.parent.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out Vector2 localMousePosition);

            // UI 요소의 위치를 마우스 드래그 이벤트가 발생한 위치로 이동합니다.
            m_RectTransform.localPosition = localMousePosition + m_ClickOffset;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);
            m_CanvasGroup.blocksRaycasts = true;

            //m_RectTransform.localPosition = m_SavePos;
        }
    }
}