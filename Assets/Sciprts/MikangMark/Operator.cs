using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.MikangMark;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using System.IO;

namespace AvantGardeMaker.MikangMark
{
	public class Operator : ObjectPoolItemBase
	{
		#region 변수
		public OperInfo m_OperData;
		[SerializeField]
		public string OperName;

		
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		private static OperatorJsonManager M_OperatorJson => OperatorJsonManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Start()
		{

		}
		#endregion

		/// <summary>
		/// 초기화 함수
		/// </summary>
		public override void InitializePoolItem()
		{
			base.InitializePoolItem();
		}
		public void SetData()
		{
			string m_FilePath;
			for (int i = 0; i < M_OperatorJson.m_OperInfoList.Count; i++)
			{
				if (OperName == M_OperatorJson.m_OperInfoList[i].EngOperName)
				{
					m_OperData = M_OperatorJson.m_OperInfoList[i];
				}
			}
			m_FilePath = M_OperatorJson.m_FileSaveDirectory;
			//Json파일 생성
			m_OperData.SaveJsonData(m_FilePath, OperName + ".Json", m_OperData);
			m_OperData = m_OperData.LoadJsonData(m_FilePath, OperName + ".Json");
		}
		//오퍼가 공격 및 힐을 당했을경우
		public void ChangeHp(float _Value)
		{
			m_OperData.RealHp += _Value;
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void FinallizePoolItem()
		{
			base.FinallizePoolItem();
		}
	}
}