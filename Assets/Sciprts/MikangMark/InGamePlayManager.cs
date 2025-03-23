using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace AvantGardeMaker.MikangMark
{
    public class InGamePlayManager : SerializedSingleton<InGamePlayManager>
    {
        #region 변수
        int m_GameHp;
        int m_GameSpeed;

        float m_time = 0;

        int m_MaxCost = 99;
        int m_Cost = 0;
        int m_SetAbleCount;

        int m_StartCost;
        float m_ClearSecond = 0.0f;

        float m_RealTime = 0.0f;
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
            m_Cost = 5;
            SetStartCost(5);
            m_Cost = m_StartCost;
        }
        private void FixedUpdate()
        {
            m_RealTime += Time.deltaTime;
            m_ClearSecond = m_RealTime;
            m_ClearSecond = m_ClearSecond - ((int)m_ClearSecond);
            if (m_RealTime >= 1.0f)
            {
                m_Cost++;
                m_RealTime = 0.0f;
            }

        }
        #endregion

        /// <summary>
        /// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
        /// </summary>
        public virtual void Initialize()
        {
            Debug.Log("Initialize");
        }
        /// <summary>
        /// 마무리화 함수 (게임 종료 시 호출)
        /// </summary>
        public virtual void Finallize()
        {
            Debug.Log("Finallize");
        }

        /// <summary>
        /// 게임 초기화 함수 (Game Scene 진입 시 호출)
        /// </summary>
        public virtual void InitializeGame()
        {

            Debug.Log("InitializeGame");
        }
        /// <summary>
        /// 게임 마무리화 함수 (Game Scene 나갈 시 호출)
        /// </summary>
        public virtual void FinallizeGame()
        {
            Debug.Log("FinallizeGame");
        }

        public void SetStartCost(int _cost)
        {
            m_StartCost = _cost;
        }
        public int GetCost()
        {
            return m_Cost;
        }
        public int GetMaxCost()
        {
            return m_MaxCost;
        }

        public float GetRealTime()
        {
            return m_ClearSecond;
        }

    }
}
