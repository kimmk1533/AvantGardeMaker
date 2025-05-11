using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace;
using AvantGardeMaker.OperatorSpace;
using AvantGardeMaker.EnemySpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using AvantGardeMaker.CoreSpace.Enum;
using static AvantGardeMaker.EnemySpace.EnemySkill;

namespace AvantGardeMaker.EnemySpace
{
	/*최상위 개체에 콜라이더
	 아래에 렌더러	 */

	public class Enemy : ObjectPoolItemBase<Enemy>
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

		#region 길찾기
		//경유지(wayPoint) 인덱스
		private int m_WayPointIndex = 0;
		//경유지 리스트(인덱스가 list.count와 같으면 도착)
		private List<Vector2> m_WayPointList = null;
		//경유지로 가기 위한 경로 리스트
		private Stack<Vector2> m_PathPointStack = null;

		//공격 딜레이
		private UtilClass.Timer m_AtkIntervalTimer;

		//경유지 대기시간 리스트(경유지 목록과 크기가 같음)
		//interval[n]은 waypoint[n]에서 waypoint[n+1]로 가기 전 대기시간을 의미함
		private List<float> m_WayPointIntervalList;

		//현재 출발 전 대기시간
		private UtilClass.Timer m_CurrentWayPointIntervalTimer;
		#endregion
		//현재 상태
		[SerializeField]
		private E_EnemyState m_CurEnemyState;
		private bool m_IsBlocked = false;

		#region 스킬
		private bool m_HasStealth = false;
		private IOnAttackSkill m_OnAttackSkill = null;
		private IOnDeadSkill m_OnDeadSkill = null;
		#endregion

		#region 콜라이더
		//적과 오퍼간 저지를 위한 몸 크기만한 콜라이더
		private CircleCollider2D m_BodyCollider = null;
		//공격 오브젝터 콜라이더를 가진 자식
		[SerializeField]
		private EnemyAtkRange m_AtkRange = null;

		//투사체 만들려다가 안하는게 낫다는걸 깨우침//

		//공격 효과 범위
		private EnemyAtkEffectRange m_AtkSkillRange = null;
		//사망 효과 범위
		private EnemyDeadEffectRange m_DeadSkillRange = null;

		#endregion
		//공격 범위 내 오퍼들
		public List<Operator> m_TargetOperList = null;
		//공격할 오퍼
		public Operator m_TargetOper;
		#endregion

		#region 프로퍼티
		public E_EnemyState state { get { return m_CurEnemyState; } set { m_CurEnemyState = value; } }
		public E_EnemyGradeType grade { get { return m_FixedData.EnemyType; } }
		public bool isAlive => m_VariableData.Hp.CurStat > 0f;
		public bool isBlocked => m_IsBlocked == true;
		private float moveSpeed => m_VariableData.MovementSpeed.CurStat;
		private Vector2 curPos => new Vector2(transform.position.x, transform.position.y);
		private Vector2 targetPos => m_WayPointList[Mathf.Min(m_WayPointIndex, m_WayPointList.Count - 1)];
		private Vector2 startPos => m_WayPointList[0];
		private Vector2 endPos => m_WayPointList[m_WayPointList.Count - 1];
		#endregion

		#region 이벤트

		#region 이벤트 함수
		//공격범위 내에 적이 들어올 경우
		private void OnOperatorEnterAtkRange(Operator oper)
		{
			//공격범위 내 들어온 대상이 operator가 아닐 경우
			if (oper == null)
				return;

			state = E_EnemyState.Attack;
			m_TargetOperList.Add(oper);
			Debug.Log("오퍼가 사정거리 내에 들어옴");
		}
		//공격범위 내에서 적이 나갈 경우
		private void OnOperatorExitAtkRange(Operator oper)
		{
			if (oper == null)
				return;

			m_TargetOperList.Remove(oper);
			if (m_TargetOperList.Count == 0)
				state = E_EnemyState.Move;
			Debug.Log("오퍼가 사정거리에서 나감");
		}
		#endregion

		#endregion

		#region 매니저
		private static EnemyManager M_Enemy => EnemyManager.Instance;
		private static GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;
		private static GameManager M_Game => GameManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			if (!isAlive)
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

			m_AtkRange.onOperatorEnterRange += OnOperatorEnterAtkRange;
			m_AtkRange.onOperatorExitRange += OnOperatorExitAtkRange;

			if (m_WayPointList == null)
			{
				m_WayPointList = new List<Vector2>();
				//list 0이라 터지는것 방지
				m_WayPointList.Add(curPos);
			}

			if (m_PathPointStack == null)
				m_PathPointStack = new Stack<Vector2>();

			m_WayPointIntervalList = new List<float>();

			m_AtkIntervalTimer = new UtilClass.Timer(1.0f);
			m_CurrentWayPointIntervalTimer = new UtilClass.Timer(0f);
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void FinallizePoolItem()
		{
			base.FinallizePoolItem();

			m_FixedData = null;
			m_VariableData = null;

			m_AtkRange.onOperatorEnterRange -= OnOperatorEnterAtkRange;
			m_AtkRange.onOperatorExitRange -= OnOperatorExitAtkRange;

			m_BodyCollider = null;
			m_AtkRange = null;

			m_WayPointList.Clear();
			m_PathPointStack.Clear();
			m_WayPointIndex = 0;
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

			for (int i = 0; i < m_FixedData.SkillDataList.Count; i++)
			{
				SetTag(m_FixedData.Tag[i]);
			}

			SetSkill(m_FixedData.SkillDataList);
		}
		private void SetTag(string tag)
		{
			switch (tag)
			{
				default:
					break;
				case "Stealth":
					m_HasStealth = true;
					break;
			}
		}
		public void SetSkill(List<EnemySkillData> skillDataList)
		{
			for (int i = 0; i < skillDataList.Count; i++)
			{
				switch (skillDataList[i].Type)
				{
					default:
						break;
					case E_EnemySkillType.OnAttack:
						if (m_AtkSkillRange == null)
						{
							m_AtkSkillRange = GetComponentInChildren<EnemyAtkEffectRange>();
						}

						m_OnAttackSkill = (IOnAttackSkill)skillDataList[i].CreateSkill();
						m_OnAttackSkill.Initialize();

						//이제 공격 스킬 범위 설정해야 함
						break;
					case E_EnemySkillType.OnDead:
						m_OnDeadSkill = (IOnDeadSkill)skillDataList[i].CreateSkill();
						break;
				}
			}
		}
		public void SetWayPointList(List<Vector2> wayPointList)
		{
			m_WayPointList.Clear();
			m_WayPointList.AddRange(wayPointList);

			m_WayPointIndex = 0;

			//경로 최신화
			UpdatePathPointList();
		}
		public void SetWayPointIntervalList(List<float> wayPointIntervalList)
		{
			m_WayPointIntervalList.Clear();
			m_WayPointIntervalList.AddRange(wayPointIntervalList);
		}
		public float GetRange()
		{
			return m_FixedData.Range.CurStat;
		}
		public void SetRange(float range)
		{
			if (m_AtkRange == null)
				throw new System.Exception("m_AtkRange is null.");

			CircleCollider2D atkRangeCollider = m_AtkRange.GetComponent<CircleCollider2D>();
			atkRangeCollider.radius = range;
		}
		#endregion

		#region 기본 행동
		public void Move()
		{
			//공격중 or 저지중이면 움직일 수 없음
			if (m_CurEnemyState == E_EnemyState.Attack || isBlocked)
				return;

			m_CurrentWayPointIntervalTimer.Update();
			//경유지에서 대기중이면 움직이지 않음
			if (m_CurrentWayPointIntervalTimer.TimeCheck() == false)
				return;

			if (m_WayPointIntervalList.Count != m_WayPointList.Count)
				throw new System.Exception("경유지 목록과 경유지 대기시간의 크기가 다름!");

			Vector2 direction = m_PathPointStack.Peek() - curPos;
			float moveAmount = moveSpeed * Time.deltaTime;

			//목표 지점에서 일정 거리 이상 떨어져있다면
			if (direction.sqrMagnitude > moveAmount * moveAmount)
			{
				state = E_EnemyState.Move;
				direction.Normalize();
				Vector2 moveVector = direction * moveAmount;
				//direction 방향으로 moveAmount만큼 이동
				transform.position = curPos + moveVector;
				Debug.Log("이동");
			}
			else
			{
				//////경로 타일 도착 처리//////

				//도착 위치로 순간이동
				transform.position = m_PathPointStack.Pop();

				//아직 경로가 남은 것이므로 return
				if (m_PathPointStack.Count != 0)
					return;

				//////경유지 도착 처리//////

				//다음 경유지로 출발하기 전 대기시간
				if (m_WayPointIntervalList[m_WayPointIndex] > 0)
				{
					m_CurrentWayPointIntervalTimer.Clear();
					m_CurrentWayPointIntervalTimer.interval = m_WayPointIntervalList[m_WayPointIndex];
					//대기시간이 존재한다면 move가 아닌 idle상태
					state = E_EnemyState.Idle;
				}

				//경유지 인덱스 +1
				++m_WayPointIndex;

				//경유지의 끝에 다다르면 소멸 처리
				if (m_WayPointIndex >= m_WayPointList.Count)
				{
					Debug.Log("이동 완료");
					//Dead();
					return;
				}
				//경로 최신화
				UpdatePathPointList();
			}
		}

		public void Attack()
		{
			m_AtkIntervalTimer.Update();
			//여기 timecheck을 쓰지 않는건 저지당했을 때와 안당했을때 처리가 달라서 그럼


			//저지당한 경우
			if (isBlocked)
			{
				//공격 간격이 다 지나지 않았을 경우 attack하지 않음
				if (m_AtkIntervalTimer.TimeCheck(true) == false)
				{
					Debug.Log("공격 쿨타임임");
					return;
				}

				if (m_OnAttackSkill != null)
					//공격 스킬이 있다면 스킬 공격
					m_OnAttackSkill.OnAttackSkill(m_TargetOper, m_VariableData.Atk.CurStat);
				else
					//BodyTrigger에 맞닿은 오퍼를 공격
					m_TargetOper.TakeDamage(m_FixedData.DamageType, m_VariableData.Atk.CurStat);
				Debug.Log("저지 공격");
			}
			//공격범위 내 오퍼가 있을 경우 공격(= 원거리 공격)
			else if (m_TargetOperList.Count > 0)
			{
				//공격 간격이 다 지나지 않았을 경우 attack하지 않음
				if (m_AtkIntervalTimer.TimeCheck(true) == false)
				{
					//공격 범위내 적이 들어올 때 attack상태가 되어서 move로 되돌려주기
					state = E_EnemyState.Move;
					return;
				}

				//공격 범위 내 적이 있었는데 쿨타임이라 안때렸을 경우 move상태므로 attack로 변경
				if (state != E_EnemyState.Attack)
					state = E_EnemyState.Attack;

				m_TargetOper = M_GamePlaying.SortOperatorListByAttackIndex(m_TargetOperList)[0];

				if (m_OnAttackSkill != null)
					m_OnAttackSkill.OnAttackSkill(m_TargetOper, m_VariableData.Atk.CurStat);
				else
					m_TargetOper.TakeDamage(m_FixedData.DamageType, m_VariableData.Atk.CurStat);

				Debug.Log("원거리 공격");
			}
			else
			{
				state = E_EnemyState.Move;
				Debug.Log("공격 중지");
			}
		}

		public void Dead()
		{
			Debug.Log("사망");

			if (m_OnAttackSkill != null && !isAlive)
				m_OnDeadSkill.OnDeadSkill(m_VariableData.Atk.CurStat);

			M_Enemy.Despawn(this);
		}
		#endregion

		/// <summary>
		/// Type의 데미지를 val만큼 입음(방어력 pierce% 무시)
		/// </summary>
		public void TakeDamage(E_DamageType dmgType, float val, float piercePercentage = 0)
		{
			//은신이고 저지당하지 않았다면 데미지를 입지 않으나
			//이건 오퍼가 때릴 때 처리하는게 맞지 않나?
			if (m_HasStealth && !isBlocked)
				return;

			switch (dmgType)
			{
				case E_DamageType.Physics:
					//물리딜: 공격력 - 방어력/방어 관통 vs 공격력의 5%
					DecreaseHp(Mathf.Max(val - m_VariableData.Def.CurStat * (piercePercentage / 100), val * 0.05f));
					break;
				case E_DamageType.Magic:
					//마법딜: 공격력 / 마법 저항 vs 공격력의 5%
					DecreaseHp(Mathf.Max(val / m_VariableData.Res.CurStat * (piercePercentage / 100), val * 0.05f));
					break;
				case E_DamageType.True:
					DecreaseHp(val);
					break;
			}
		}
		private void DecreaseHp(float val)
		{
			m_VariableData.Hp.CurStat -= val;
		}
		/// <summary>
		/// 현재 위치에서 경유지[m_WayPointIndex]로 가기 위한 경로인 m_PathPointStack 최신화
		/// <br></br>
		/// 현재 위치가 경유지라면 curPos만 stack에 담고 return
		/// </summary>
		private void UpdatePathPointList()
		{
			float[,] weightMap = PathFinder.GetWeightMap(M_GamePlaying.currentMap);
			List<Vector2> pathList = PathFinder.FindPath(curPos, targetPos, weightMap);
			//List<Vector2> pathList = PathFinder.FindPath(curPos, targetPos, m_testMap);
			m_PathPointStack.Clear();
			if (pathList == null)
			{
				m_PathPointStack.Push(curPos);
				return;
			}
			for (int i = 0; i < pathList.Count; i++)
			{
				m_PathPointStack.Push(pathList[i]);
			}
		}
	}
}