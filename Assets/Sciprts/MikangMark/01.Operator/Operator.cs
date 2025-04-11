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
		public OperInfo OperData;
		[SerializeField]
		public string OperName;

		private Image m_ThisImg;

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
			m_ThisImg = GetComponent<Image>();
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
					OperData = M_OperatorJson.m_OperInfoList[i].Clone();
				}
			}
			m_FilePath = M_OperatorJson.m_FileSaveDirectory;
			//Json파일 생성
			OperData.SaveJsonData(m_FilePath, OperName + ".Json", OperData);
			OperData = OperData.LoadJsonData(m_FilePath, OperName + ".Json");
		}
		public void SetDirection(E_OperatorDirection _Direction,E_OperatorDirection _LastDirection)
		{//수정
			float radian = 0;
			switch (_Direction)
			{
				case E_OperatorDirection.Left:
					if (_Direction == OperData.E_OperDirection_H)//같은방향으로드래그했을경우
					{
						return;
					}
					switch (_LastDirection)
					{
						case E_OperatorDirection.Right:
							radian = 180;
							break;
						case E_OperatorDirection.Up:
							radian = 90;
							break;
						case E_OperatorDirection.Down:
							radian = 270;
							break;
						case E_OperatorDirection.Left:
							//radian = 0;
							break;
					}
					//왼쪽을 드래그
					if (OperData.E_OperDirection_V == E_OperatorDirection.Up)
					{
						m_ThisImg.sprite = OperData.LeftUpImg;
					}
					else
					{
						m_ThisImg.sprite = OperData.LeftDownImg;
					}
					_LastDirection = E_OperatorDirection.Left;
					RotateAtkRange(OperData.AttackPos, radian);
					OperData.E_OperDirection_H = E_OperatorDirection.Left;
					M_UI.OperAtkRangeHighlight(M_UI.RotateViewAtkRange(OperData.AttackPos, radian), M_UI.m_CreatedATKRangeHighlights[0]);
					break;
				case E_OperatorDirection.Right:
					if (_Direction == OperData.E_OperDirection_H)
					{
						return;
					}
					switch (_LastDirection)
					{
						case E_OperatorDirection.Right:
							//radian = 0;
							break;
						case E_OperatorDirection.Up:
							radian = 90;
							break;
						case E_OperatorDirection.Down:
							radian = 180;
							break;
						case E_OperatorDirection.Left:
							radian = 180;
							break;
					}
					if (OperData.E_OperDirection_V == E_OperatorDirection.Up)
					{
						m_ThisImg.sprite = OperData.RightUpImg;
					}
					else
					{
						m_ThisImg.sprite = OperData.RightDownImg;
					}
					_LastDirection = E_OperatorDirection.Right;
					RotateAtkRange(OperData.AttackPos, radian);
					OperData.E_OperDirection_H = E_OperatorDirection.Right;
					M_UI.OperAtkRangeHighlight(M_UI.RotateViewAtkRange(OperData.AttackPos, radian), M_UI.m_CreatedATKRangeHighlights[0]);
					break;
				case E_OperatorDirection.Down:
					if (_Direction == OperData.E_OperDirection_V)
					{
						return;
					}
					switch (_LastDirection)
					{
						case E_OperatorDirection.Right:
							radian = 90;
							break;
						case E_OperatorDirection.Up:
							radian = 180;
							break;
						case E_OperatorDirection.Down:
							//radian = 0;
							break;
						case E_OperatorDirection.Left:
							radian = 270;
							break;
					}
					if (OperData.E_OperDirection_H == E_OperatorDirection.Left)
					{
						m_ThisImg.sprite = OperData.LeftDownImg;
					}
					else
					{
						m_ThisImg.sprite = OperData.RightDownImg;
					}
					_LastDirection = E_OperatorDirection.Down;
					RotateAtkRange(OperData.AttackPos, radian);
					OperData.E_OperDirection_V = E_OperatorDirection.Down;
					M_UI.OperAtkRangeHighlight(M_UI.RotateViewAtkRange(OperData.AttackPos, radian), M_UI.m_CreatedATKRangeHighlights[0]);
					break;
				case E_OperatorDirection.Up:
					if (_Direction == OperData.E_OperDirection_V)
					{
						return;
					}
					switch (_LastDirection)
					{
						case E_OperatorDirection.Right:
							radian = 270;
							break;
						case E_OperatorDirection.Up:
							//radian = 0;
							break;
						case E_OperatorDirection.Down:
							radian = 180;
							break;
						case E_OperatorDirection.Left:
							radian = 90;
							break;
					}
					if (OperData.E_OperDirection_H == E_OperatorDirection.Left)
					{
						m_ThisImg.sprite = OperData.LeftUpImg;
					}
					else
					{
						m_ThisImg.sprite = OperData.RightUpImg;
					}
					_LastDirection = E_OperatorDirection.Up;
					RotateAtkRange(OperData.AttackPos, radian);
					OperData.E_OperDirection_V = E_OperatorDirection.Up;
					M_UI.OperAtkRangeHighlight(M_UI.RotateViewAtkRange(OperData.AttackPos, radian), M_UI.m_CreatedATKRangeHighlights[0]);
					break;

			}
		}
		public void RotateAtkRange(Vector2[] _AtkRange, float _Direction)
		{
			//기본 오른쪽
			Debug.Log(_Direction);
			float angleRad = _Direction * Mathf.Deg2Rad; // 라디안으로 변환

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
			OperData.RealHp += _Value;
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