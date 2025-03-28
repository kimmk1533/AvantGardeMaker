using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.ad1a.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.ad1a
{
	public class Enemy : ObjectPoolItemBase
	{
		#region 변수
		public EnemyData m_EnemyData;

		public Vector3 m_CurPos;
		public Vector3 m_TargetPos;
		public E_EnemyState m_EnemyState;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			m_CurPos = transform.position;
			if (m_EnemyState == E_EnemyState.Move)
			{
				Vector3 direction = m_TargetPos - m_CurPos;
				float moveAmount = m_EnemyData.m_MoveSpeed.m_CurStat * Time.deltaTime;
				if (direction.sqrMagnitude > moveAmount)//목표 지점에서 일정 거리 이상 떨어져있다면
				{
					direction.Normalize();
					Vector3 moveVector = direction * moveAmount;
					//direction 방향으로 moveAmount만큼 이동
					transform.position = m_CurPos + moveVector;
				}
				else
				{
					transform.position = m_CurPos = m_TargetPos;
					m_EnemyState = E_EnemyState.Idle;
				}
				Debug.Log(transform.position);
			}
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
	}
}