using System;
using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.ad1a.Enum;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace AvantGardeMaker.ad1a
{
	public class Enemy : ObjectPoolItemBase
	{
		#region 변수
		public EnemyData m_EnemyData = null;

		public Vector3 m_CurPos = Vector3.zero;
		public Vector3 m_TargetPos = Vector3.zero;
		public E_EnemyState m_CurEnemyState;    //현재 상태
		public E_EnemyState m_PrevEnemyState;   //직전 상태(공격하기 전 상태)

		public BoxCollider m_Collider;
		public SphereCollider m_AtkRangeCollider;

		public List<GameObject> m_TargetOperList = null;   //공격 범위 내 오퍼들
		public GameObject m_TargetOper;             //공격할 오퍼


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
			if (m_EnemyData != null)
				//죽음
				if (m_EnemyData.VariableData.Hp.CurStat <= 0.0f)
				{
					m_CurEnemyState = E_EnemyState.Dead;
					Dead();
				}

			//이동
			m_CurPos = transform.position;
			if (m_CurEnemyState == E_EnemyState.Move)
			{
				Vector3 direction = m_TargetPos - m_CurPos;
				float moveAmount = m_EnemyData.VariableData.MovementSpeed.CurStat * Time.deltaTime;

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
					m_CurEnemyState = E_EnemyState.Idle;
				}
			}

			//공격
			if (m_CurEnemyState == E_EnemyState.Attack)
			{
				if (m_TargetOperList.Count == 0)
				{
					m_CurEnemyState = m_PrevEnemyState;
					m_PrevEnemyState = E_EnemyState.Attack;
					return;
				}

				//int targetIndex = 0;
				//int lastestDeploy = 0;
				//int maxProvoke = 0;
				//for (int i = 1; i < m_TargetOperList.Count; i++)
				//{
				//	if (m_TargetOperList[i].도발랭크 > maxProvoke)   //해당 오퍼레이터의 도발 랭크 확인
				//	{
				//		maxProvoke = m_TargetOperList[i].도발랭크;
				//		targetIndex = i;
				//	}
				//	else if (m_TargetOperList[i].배치순서 > lastestDeploy)   //해당 오퍼레이터의 배치 순서 확인
				//	{
				//		lastestDeploy = m_TargetOperList[i].배치순서;
				//		targetIndex = i;
				//	}
				//}

				//Attack(m_TargetOperList[targetIndex]);
			}

		}

		private void OnCollisionEnter(Collision collision)
		{
			//공격 범위 내에 오퍼레이터가 있으면 공격
			if (collision.collider.gameObject.CompareTag("Oper"))
			{
				//여러명일 경우 배치 순서와 도발 랭크를 확인해야 함
				m_TargetOperList.Add(collision.gameObject);

				if (m_CurEnemyState != E_EnemyState.Attack)
				{
					m_PrevEnemyState = m_CurEnemyState;
					m_CurEnemyState = E_EnemyState.Attack;
				}
			}
		}

		private void OnCollisionExit(Collision collision)
		{
			//오퍼레이터가 공격 범위를 벗어나면
			if (collision.collider.gameObject.CompareTag("Oper"))
			{
				//해당 오퍼레이터를 공격 목록에서 제거
				m_TargetOperList.Remove(collision.gameObject);

				//공격 범위 내에 오퍼레이터가 없으면 공격 상태 해제
				if (m_TargetOperList.Count == 0)
				{
					m_CurEnemyState = m_PrevEnemyState;
					m_PrevEnemyState = E_EnemyState.Attack;
				}
			}
		}
		#endregion

		/// <summary>
		/// 초기화 함수
		/// </summary>
		public override void InitializePoolItem()
		{
			base.InitializePoolItem();

			if (m_EnemyData == null)
				m_EnemyData = new EnemyData();
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void FinallizePoolItem()
		{
			base.FinallizePoolItem();
		}

		void Attack(GameObject target)
		{

		}

		void Dead()
		{
			this.gameObject.SetActive(false);
		}
	}
}