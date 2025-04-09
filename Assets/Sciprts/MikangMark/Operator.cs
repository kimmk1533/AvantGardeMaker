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
		private static UIManager M_UI => UIManager.Instance;
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
		{//수정
			switch (_Direction)
			{
				case E_OperatorDirection.Left:
					if (m_OperData.E_OperDirection_V == E_OperatorDirection.Up)
					{
						GetComponent<Image>().sprite = m_OperData.LeftUpImg;
					}
					else
					{
						GetComponent<Image>().sprite = m_OperData.LeftDownImg;
					}
					RotateAtkRange(m_OperData.AttackPos, E_OperatorDirection.Left);
					m_OperData.E_OperDirection_H = E_OperatorDirection.Left;
					M_UI.OperAtkRangeHighlight(M_UI.RotateViewAtkRange(m_OperData.AttackPos, m_OperData.E_OperDirection_H), M_UI.m_CreatedATKRangeHighlights[0]);
					break;
				case E_OperatorDirection.Right:
					if (m_OperData.E_OperDirection_V == E_OperatorDirection.Up)
					{
						GetComponent<Image>().sprite = m_OperData.RightUpImg;
					}
					else
					{
						GetComponent<Image>().sprite = m_OperData.RightDownImg;
					}
					RotateAtkRange(m_OperData.AttackPos, E_OperatorDirection.Right);
					m_OperData.E_OperDirection_H = E_OperatorDirection.Right;
					M_UI.OperAtkRangeHighlight(M_UI.RotateViewAtkRange(m_OperData.AttackPos, m_OperData.E_OperDirection_H), M_UI.m_CreatedATKRangeHighlights[0]);
					break;
				case E_OperatorDirection.Down:
					if (m_OperData.E_OperDirection_H == E_OperatorDirection.Left)
					{
						GetComponent<Image>().sprite = m_OperData.LeftDownImg;
					}
					else
					{
						GetComponent<Image>().sprite = m_OperData.RightDownImg;
					}
					RotateAtkRange(m_OperData.AttackPos, E_OperatorDirection.Down);
					m_OperData.E_OperDirection_V = E_OperatorDirection.Down;
					M_UI.OperAtkRangeHighlight(M_UI.RotateViewAtkRange(m_OperData.AttackPos, m_OperData.E_OperDirection_V), M_UI.m_CreatedATKRangeHighlights[0]);
					break;
				case E_OperatorDirection.Up:
					if (m_OperData.E_OperDirection_H == E_OperatorDirection.Left)
					{
						GetComponent<Image>().sprite = m_OperData.LeftUpImg;
					}
					else
					{
						GetComponent<Image>().sprite = m_OperData.RightUpImg;
					}
					RotateAtkRange(m_OperData.AttackPos, E_OperatorDirection.Up);
					m_OperData.E_OperDirection_V = E_OperatorDirection.Up;
					M_UI.OperAtkRangeHighlight(M_UI.RotateViewAtkRange(m_OperData.AttackPos, m_OperData.E_OperDirection_V), M_UI.m_CreatedATKRangeHighlights[0]);
					break;

			}
		}
		public void RotateAtkRange(Vector2[] _AtkRange, E_OperatorDirection _Direction)
		{
			//기본 오른쪽
			float angleRad = (int)_Direction * Mathf.Deg2Rad; // 라디안으로 변환

			float cos = Mathf.Cos(angleRad);
			float sin = Mathf.Sin(angleRad);

			for (int i = 0; i < _AtkRange.Length; i++)
			{
				_AtkRange[i] = new Vector2(_AtkRange[i].x * cos - _AtkRange[i].y * sin, _AtkRange[i].x * sin + _AtkRange[i].y * cos);
			}
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