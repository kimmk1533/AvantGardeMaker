using System.Collections.Generic;
using AvantGardeMaker;
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
		#region 스탯
		private string m_EngName = string.Empty;
		private string m_KorName = string.Empty;

		private string m_PortraitImagePath = string.Empty;

		private EnemyFixedData m_FixedData = null;
		private EnemyVariableData m_VariableData = null;
		#endregion
		//경유지(wayPoint) 인덱스
		private int m_TransitPosIndex = 0;
		//경유지 목록(인덱스가 list.count와 같으면 도착)
		private List<Vector2> m_TransitPosList = null;

		//공격 딜레이
		private UtilClass.Timer m_AtkIntervalTimer;

		//현재 상태
		[SerializeField]
		private E_EnemyState m_CurEnemyState;
		private bool m_IsBlocked = false;

		//적과 오퍼간 저지를 위한 몸 크기만한 콜라이더
		private CircleCollider2D m_BodyCollider = null;
		public float m_BodySize;
		//공격 오브젝터 콜라이더를 가진 자식
		[SerializeField]
		private EnemyAtkRange m_AtkRange = null;
		public float m_RangeSize;
		//공격 범위 내 오퍼들
		public List<Operator> m_TargetOperList = null;
		//공격할 오퍼
		public Operator m_TargetOper;
		#endregion

		#region 프로퍼티
		private Vector2 CurPos => new Vector2(transform.position.x, transform.position.y);
		private Vector2 TargetPos => m_TransitPosList[Mathf.Min(m_TransitPosIndex, m_TransitPosList.Count)];
		public bool IsAlive => m_VariableData.Hp.CurStat > 0f;
		public bool IsBlocked => m_IsBlocked == true;
		#endregion

		#region 이벤트

		#region 이벤트 함수
		//공격범위 내에 적이 들어올 경우
		private void OnOperatorEnterRange(Operator oper)
		{
			//공격범위 내 들어온 대상이 operator가 아닐 경우
			if (oper == null)
				return;

			SetState(E_EnemyState.Attack);
			m_TargetOperList.Add(oper);
			Debug.Log("오퍼가 사정거리 내에 들어옴");
		}
		//공격범위 내에서 적이 나갈 경우
		private void OnOperatorExitRange(Operator oper)
		{
			if (oper == null)
				return;

			m_TargetOperList.Remove(oper);
			if (m_TargetOperList.Count == 0)
				SetState(E_EnemyState.Move);
			Debug.Log("오퍼가 사정거리에서 나감");
		}
		#endregion

		#endregion

		#region 매니저
		private static EnemyManager M_Enemy => EnemyManager.Instance;
		private static GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			m_AtkIntervalTimer.Update();

			if (!IsAlive)
				Dead();

			Attack();
			Move();
		}

		//저지당할 때
		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.gameObject.CompareTag("Operator"))
			{
				m_IsBlocked = true;
				//오퍼의 배치 순서에 관계없이 본인을 저지한 오퍼를 때림
				m_TargetOper = collider.gameObject.GetComponent<Operator>();
			}
		}

		//저지가 풀렸을 때
		private void OnTriggerExit2D(Collider2D collider)
		{
			if (collider.gameObject.CompareTag("Operator"))
			{
				m_IsBlocked = false;
				m_TargetOper = null;
			}
		}
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
				m_BodyCollider = GetComponent<CircleCollider2D>();
			if (m_AtkRange == null)
				m_AtkRange = GetComponentInChildren<EnemyAtkRange>();

			m_AtkRange.onOperatorEnterRange += OnOperatorEnterRange;
			m_AtkRange.onOperatorExitRange += OnOperatorExitRange;

			if (m_TransitPosList == null)
			{
				m_TransitPosList = new List<Vector2>();
				//list 0이라 터지는것 방지
				m_TransitPosList.Add(CurPos);
			}

			m_AtkIntervalTimer = new UtilClass.Timer(1.0f);
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

			//SetRange(m_FixedData.Range.CurStat);

			//m_AtkIntervalTimer.interval = m_VariableData.Aspd.CurStat;
		}
		public void SetTransitPosList(List<Vector2> transitPosList)
		{
			m_TransitPosList = transitPosList;
		}
		public void SetRange(float range)
		{
			if (m_AtkRange == null)
				throw new System.Exception("m_AtkRange is null.");

			CircleCollider2D atkRangeCollider = m_AtkRange.GetComponent<CircleCollider2D>();
			atkRangeCollider.radius = range;

			m_BodySize = m_BodyCollider.radius;
			m_RangeSize = m_AtkRange.GetComponent<CircleCollider2D>().radius;
		}
		public E_EnemyState GetState()
		{
			return m_CurEnemyState;
		}

		public void SetState(E_EnemyState state)
		{
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

		#region 기본 행동
		public void Move()
		{
			//공격중 or 저지중이면 움직일 수 없음
			if (m_CurEnemyState == E_EnemyState.Attack || IsBlocked)
				return;

			Vector2 direction = TargetPos - CurPos;
			float moveAmount = m_VariableData.MovementSpeed.CurStat * Time.deltaTime;

			if (direction.sqrMagnitude > moveAmount)//목표 지점에서 일정 거리 이상 떨어져있다면
			{
				SetState(E_EnemyState.Move);
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

				++m_TransitPosIndex;
				if (m_TransitPosIndex >= m_TransitPosList.Count)
				{
					Debug.Log("이동 완료");
					Dead();
					return;
				}
			}
		}

		public void Attack()
		{
			//어떤 경우에도 공격 상태로는 전환 가능

			//저지당한 경우
			if (IsBlocked)
			{
				//공격 간격이 다 지나지 않았을 경우 attack하지 않음
				if (m_AtkIntervalTimer.TimeCheck(true) == false)
				{
					Debug.Log("공격 쿨타임임");
					return;
				}

				//BodyTrigger에 맞닿은 오퍼를 공격(인데 operatorData null이라 터져서 봉인)
				//m_TargetOper.TakeDamage(m_FixedData.DamageType, m_VariableData.Atk.CurStat, 0);
				Debug.Log("저지 공격");
			}
			//공격범위 내 오퍼가 있을 경우 공격(= 원거리 공격)
			else if (m_TargetOperList.Count > 0)
			{
				//공격 간격이 다 지나지 않았을 경우 attack하지 않음
				if (m_AtkIntervalTimer.TimeCheck(true) == false)
				{
					//공격 범위내 적이 들어올 때 attack상태가 되어서 move로 되돌려주기
					SetState(E_EnemyState.Move);
					return;
				}

				//공격 범위 내 적이 있었는데 쿨타임이라 안때렸을 경우 move상태므로 attack로 변경
				if (GetState() != E_EnemyState.Attack)
					SetState(E_EnemyState.Attack);

				//ad1a 씬에 오퍼 매니저가 없어서 터짐. 임시 봉인
				//m_TargetOper = M_GamePlaying.CompareDeployOrder(m_TargetOperList)[0];
				m_TargetOper = m_TargetOperList[0];

				Debug.Log("원거리 공격");
			}
			else
			{
				SetState(E_EnemyState.Move);
				Debug.Log("공격 중지");
			}
		}

		public void Dead()
		{
			Debug.Log("사망");
			M_Enemy.Despawn(this);
		}
		#endregion

		/// <summary>
		/// Type의 데미지를 val만큼 입음(방어력 pierce% 무시)
		/// </summary>
		public void TakeDamage(E_DamageType dmgType, float val, float piercePercentage = 0)
		{
			switch (dmgType)
			{
				case E_DamageType.Physics:
					//물리딜: 공격력 - 방어력/방어 관통 vs 공격력의 5%
					SubHp(Mathf.Max(val - m_VariableData.Def.CurStat * (piercePercentage / 100), val * 0.05f));
					break;
				case E_DamageType.Magic:
					//마법딜: 공격력 / 마법 저항 vs 공격력의 5%
					SubHp(Mathf.Max(val / m_VariableData.Res.CurStat * (piercePercentage / 100), val * 0.05f));
					break;
				case E_DamageType.True:
					SubHp(val);
					break;
			}
		}

		private void SubHp(float val)
		{
			m_VariableData.Hp.CurStat -= val;
		}
	}
}