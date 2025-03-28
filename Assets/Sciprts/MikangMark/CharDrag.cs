using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;


namespace AvantGardeMaker.MikangMark
{
	public class CharDrag : SerializedMonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region 변수
        public RectTransform A;  // 드래그할 UI 오브젝트 (고정)
        public GameObject BPrefab; // B의 프리팹 (없을 경우 생성)
        public Camera UICamera;  // UI 카메라 (Canvas용)
        public Camera MainCamera; // 3D 오브젝트가 있는 메인 카메라
        public LayerMask targetLayer; // C 오브젝트가 포함된 레이어
        public GameObject Canvas;

        private RectTransform B;  // 따라다닐 UI 오브젝트
        private bool isDragging = false;
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
            A = GetComponent<RectTransform>();
            UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
            MainCamera = GameObject.Find("PlayingCamera").GetComponent<Camera>();
            targetLayer = GameObject.Find("Tile").layer;
            Canvas = GameObject.Find("Canvas");
            Initialize();
		}
		#endregion

		/// <summary>
		/// 초기화 함수
		/// </summary>
		/// 
		// UI 생성
		public void Initialize()
		{

		}

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;

            // B가 없으면 생성 (한 번만)
            if (B == null)
            {
                GameObject newB = Instantiate(BPrefab, Canvas.transform); // A의 부모 (Canvas) 아래에 생성
                
                B = newB.GetComponent<RectTransform>();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDragging) return;

            // A는 고정, B만 마우스를 따라다님
            B.position = eventData.position;

            // 3D 오브젝트 위에 있는지 검사
            CheckIfOver3DObject(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
        }

        private void CheckIfOver3DObject(PointerEventData eventData)
        {
            // UI 좌표를 3D 월드 좌표로 변환하여 Raycast 실행
            Ray ray = MainCamera.ScreenPointToRay(eventData.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayer))
            {
                Debug.Log("B가 C 위에 있음: " + hit.collider.name);

                // C 오브젝트의 월드 좌표
                Vector3 C_WorldPos = hit.collider.transform.position;

                // C의 월드 좌표를 UI 좌표로 변환
                Vector3 C_UIPos = WorldToUISpace(UICamera, C_WorldPos);

                // B의 위치를 C의 UI 중심으로 이동
                B.position = C_UIPos;
            }
        }

        private Vector3 WorldToUISpace(Camera uiCamera, Vector3 worldPos)
        {
            Vector3 screenPos = MainCamera.WorldToScreenPoint(worldPos); // 3D 월드 -> 스크린 좌표
            Vector3 uiPos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(B.parent as RectTransform, screenPos, uiCamera, out uiPos);
            return uiPos;
        }

        /// <summary>
        /// 마무리화 함수
        /// </summary>
        public void Finallize()
		{

		}
		
		
	}
}