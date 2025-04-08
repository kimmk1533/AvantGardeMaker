using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.MikangMark;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using System.IO;
using AvantGardeMaker.MikangMark.Enum;
using UnityEngine.UI;

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
		public void SetDirection(E_OperatorDirection _Direction)
		{
			switch (_Direction)
			{
				case E_OperatorDirection.Left:
					GetComponent<Image>().sprite = m_OperData.LeftDownImg;
					break;
				case E_OperatorDirection.Right:
					break;
				case E_OperatorDirection.Down:
					break;
				case E_OperatorDirection.Up:
					break;

			}
		}
		public void RotateAtkRange(Vector2[] _AtkRange, E_OperatorDirection _Direction)
		{
			//시작
			/*
			float angleRad = angleDeg * Mathf.Deg2Rad; // 라디안으로 변환

			float cos = Mathf.Cos(angleRad);
			float sin = Mathf.Sin(angleRad);


			Vector2[] temp = new Vector2[_OperAtkRange.Length];
			for (int i = 0; i < temp.Length; i++)
			{
				temp[i] = new Vector2(_OperAtkRange[i].x * cos - _OperAtkRange[i].y * sin, _OperAtkRange[i].x * sin + _OperAtkRange[i].y * cos);
			}
			return temp;
			*/
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