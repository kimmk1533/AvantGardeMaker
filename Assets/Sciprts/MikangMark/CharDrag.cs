using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;


namespace AvantGardeMaker.MikangMark
{
	public class CharDrag : SerializedMonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IBeginDragHandler
    {
		#region 변수
		public CharInfo m_OpInfo;
        public GameObject m_Operator;
        public GameObject m_OperParent;
        int m_Once = 0;

        private RectTransform m_RectTransform;
        private CanvasGroup m_CanvasGroup;
        private Vector2 m_ClickOffset;
        private Vector3 m_SavePos;

        GameObject CreatedOper;
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
            Initialize();
        }
        #endregion

        /// <summary>
        /// 초기화 함수
        /// </summary>
        public void Initialize()
		{
            m_RectTransform = GetComponent<RectTransform>();
            m_CanvasGroup = GetComponent<CanvasGroup>();
            m_OpInfo = gameObject.GetComponent<CharScript>().m_CharData;
            m_OperParent = GameObject.Find("MapTileObjects_Empty").gameObject;
            //m_Operator.GetComponent<>
        }
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{

		}
        public void OnPointerDown(PointerEventData eventData)
        {
            /*
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_RectTransform.parent.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out Vector2 localMousePosition);
            m_SavePos = m_RectTransform.localPosition;
            m_ClickOffset = (Vector2)m_RectTransform.localPosition - localMousePosition;
            m_CanvasGroup.blocksRaycasts = false;
            */
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log(1);
            if (CreatedOper == null) // 처음 드래그할 때만 생성
            {
                // 마우스 위치에서 레이 쏘기
                Ray ray = Camera.main.ScreenPointToRay(eventData.position);
                RaycastHit hit;
                Debug.Log(2);
                if (Physics.Raycast(ray, out hit))
                {
                    // 충돌한 위치에 오브젝트 생성
                    Debug.Log(3);
                    CreatedOper = Instantiate(m_Operator, hit.collider.gameObject.transform);
                    //CreatedOper = Instantiate(m_Operator, m_OperParent.transform);
                }
                else
                {
                    Debug.Log(4);
                    // 바닥이 없는 경우 기본 위치에 생성
                    CreatedOper = Instantiate(m_Operator, m_OperParent.transform);
                }
                
            }
        }
        public void OnDrag(PointerEventData eventData)//배치목록에서 캐릭드래그시작하면 캐릭생성
		{
            if (CreatedOper != null)
            {
                // 마우스 위치에서 레이 쏘기
                Ray ray = Camera.main.ScreenPointToRay(eventData.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(5);
                    // 생성된 오브젝트 위치 이동
                    CreatedOper.transform.position = hit.point;
                }
            }

        }
        public void OnPointerUp(PointerEventData eventData)
        {
            //m_Once = 0;
        }
    }
}