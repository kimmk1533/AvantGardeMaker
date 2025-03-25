using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.MikangMark;
using Sirenix.OdinInspector;
using UnityEngine;
using System;

namespace AvantGardeMaker.MikangMark
{
	public class CharScript : SerializedMonoBehaviour
	{
		#region 변수
		[SerializeField]
		CharInfo m_CharData;

		Yaml m_Yaml;

		[SerializeField]
		string CharName;

		string m_FilePath;
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
            
            m_Yaml = GameObject.Find("GameManager").GetComponent<Yaml>();
            m_FilePath = m_Yaml.m_FilePath;
            m_CharData = new CharInfo();
            Debug.Log(CharName + ".yaml");
            Debug.Log(m_FilePath);
            m_CharData = m_Yaml.LoadData(m_FilePath, CharName + ".yaml");
            Debug.Log(m_CharData.m_EngCharName);
        }
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{

		}
	}
}