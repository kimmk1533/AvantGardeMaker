using System.Collections.Generic;
using AvantGardeMaker.ad1a.Enum;
using AvantGardeMaker.MikangMark;
using UnityEngine;

namespace AvantGardeMaker.ad1a
{
	/*최상위 개체에 콜라이더
	 * 아래에 렌더러
	 * 
	 */


	public class Enemy : ObjectPoolItemBase
	{
		#region 변수
		//자신의 스테이터스 정보
		private string m_EngName = string.Empty;
		private string m_KorName = string.Empty;

		private string m_PortraitImagePath = string.Empty;

		private EnemyFixedData m_FixedData = null;
		private EnemyVariableData m_VariableData = null;

		//경유지(wayPoint) 인덱스
		private int m_TransitPosIndex = 0;
		//경유지 목록(인덱스가 list.count와 같으면 도착)
		private List<Vector2> m_TransitPosList = null;

		//현재 상태
		private E_EnemyState m_CurEnemyState;
		//직전 상태(공격하기 전 상태)
		private E_EnemyState m_PrevEnemyState;

		//적과 오퍼간 저지를 위한 몸 크기만한 콜라이더
		private CircleCollider2D m_BodyCollider = null;
		//공격 오브젝터 콜라이더를 가진 자식
		private EnemyAtkRange m_AtkRange = null;
		//공격 범위 내 오퍼들
		public List<Operator> m_TargetOperList = null;
		//공격할 오퍼
		public GameObject m_TargetOper;
		#endregion

		#region 프로퍼티
		private Vector2 CurPos => new Vector2(transform.position.x, transform.position.y);
		private Vector2 TargetPos => m_TransitPosList[Mathf.Min(m_TransitPosIndex, m_TransitPosList.Count)];

		public bool IsAlive => m_VariableData.Hp.CurStat > 0f;
		#endregion

		#region 이벤트

		#region 이벤트 함수
		//공격범위 내에 적이 들어올 경우
		private void OnOperatorEnterRange(Operator oper)
		{
			if (oper == null)
				return;

			m_TargetOperList.Add(oper);
			Debug.Log("오퍼가 사정거리 내에 들어옴");
		}
		//공격범위 내에서 적이 나갈 경우
		private void OnOperatorExitRange(Operator oper)
		{
			if (oper == null)
				return;

			m_TargetOperList.Remove(oper);
			Debug.Log("오퍼가 사정거리에서 나감");
		}
		#endregion

		#endregion

		#region 매니저
		private EnemyManager M_Enemy => EnemyManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		//public void OnDrawGizmos()
		//{
		//	Gizmos.color = Color.red;
		//	Gizmos.DrawWireSphere(transform.position, m_FixedData.Range.CurStat);
		//}
		private void Update()
		{
			if (!IsAlive)
				Dead();

			Attack();
			Move();
		}

		//저지당할 때
		//private void OnTriggerEnter2D(Collider2D collider)
		//{
		//	if (collider.gameObject.CompareTag("Operator"))
		//	{
		//	}
		//}

		//저지가 풀렸을 때
		//private void OnTriggerExit2D(Collider2D collider)
		//{
		//	if (collider.gameObject.CompareTag("Operator"))
		//	{
		//	}
		//}
		#endregion

		#region 초기화 & 마무리
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public override void InitializePoolItem()
		{
			base.InitializePoolItem();

			if (m_FixedData == null)
				m_FixedData = new EnemyFixedData();
			if (m_VariableData == null)
				m_VariableData = new EnemyVariableData();

			if (m_BodyCollider == null)
				m_BodyCollider = gameObject.GetComponent<CircleCollider2D>();
			if (m_AtkRange == null)
				m_AtkRange = GetComponentInChildren<EnemyAtkRange>();

			m_AtkRange.onOperatorEnterRange += OnOperatorEnterRange;
			m_AtkRange.onOperatorExitRange += OnOperatorExitRange;

			if (m_TransitPosList == null)
			{
				m_TransitPosList = new List<Vector2>();
				m_TransitPosList.Add(CurPos);
			}
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void FinallizePoolItem()
		{
			base.FinallizePoolItem();
		}
		#endregion

		#region Get Set
		public void SetEnemyData(EnemyData enemyData)
		{
			m_EngName = enemyData.EngName;
			m_KorName = enemyData.KorName;

			m_PortraitImagePath = enemyData.PortraitImagePath;

			m_FixedData = enemyData.FixedData;
			m_VariableData = enemyData.VariableData;

			SetRange();
		}
		private void SetRange()
		{
			if (m_AtkRange == null)
				throw new System.Exception("m_AtkRange is null.");

			CircleCollider2D atkRangeCollider = m_AtkRange.GetComponent<CircleCollider2D>();
			atkRangeCollider.radius = m_FixedData.Range.CurStat;
		}
		public E_EnemyState GetState()
		{
			return m_CurEnemyState;
		}

		public void SetState(E_EnemyState state)
		{
			//최초로 공격 상태가 될 경우 직전 상태 저장
			if (state == E_EnemyState.Attack && m_CurEnemyState != E_EnemyState.Attack)
			{
				m_PrevEnemyState = m_CurEnemyState;
			}

			m_CurEnemyState = state;
		}

		public float GetRange()
		{
			return m_FixedData.Range.CurStat;
		}

		public E_EnemyType GetRank()
		{
			return m_FixedData.EnemyType;
		}
		#endregion

		public void Move()
		{
			//공격중 or 저지중이면 움직일 수 없음
			if (m_CurEnemyState == E_EnemyState.Attack ||
				m_CurEnemyState == E_EnemyState.Block)
				return;

			SetState(E_EnemyState.Move);

			Vector2 direction = TargetPos - CurPos;
			float moveAmount = m_VariableData.MovementSpeed.CurStat * Time.deltaTime;

			if (direction.sqrMagnitude > moveAmount)//목표 지점에서 일정 거리 이상 떨어져있다면
			{
				direction.Normalize();
				Vector2 moveVector = direction * moveAmount;
				//direction 방향으로 moveAmount만큼 이동
				transform.position = CurPos + moveVector;
				Debug.Log("이동");
			}
			else
			{
				transform.position = TargetPos;
				SetState(E_EnemyState.Idle);
				Debug.Log("경유지까지 이동 완료");
			}
		}

		public void Attack()
		{
			//어떤 경우에도 공격 상태로는 전환 가능

			//저지당한 경우
			if (m_CurEnemyState == E_EnemyState.Block)
			{
				SetState(E_EnemyState.Attack);
				Debug.Log("공격");
			}
			//공격범위 내 오퍼가 있을 경우 공격(= 원거리 공격)
			else if (m_TargetOperList.Count > 0)
			{
				SetState(E_EnemyState.Attack);
				Debug.Log("공격");
			}
			else
			{
				SetState(m_PrevEnemyState);
				Debug.Log("공격 중지");
			}
		}

		public void Dead()
		{
			Debug.Log("사망");
			M_Enemy.Despawn(this);
		}
	}
}