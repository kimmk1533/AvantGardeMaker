using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace;
using AvantGardeMaker.CoreSpace.Enum;
using AvantGardeMaker.OperatorSpace.Enum;
using AvantGardeMaker.EnemySpace;
using AvantGardeMaker.TileSpace;
using AvantGardeMaker.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AvantGardeMaker.OperatorSpace
{
	public class Operator : ObjectPoolItemBase<Operator>
	{
		#region 기본 템플릿
		#region 변수
		#region 스탯
		protected OperatorData m_OperatorData = null;

		protected OperatorFixedData m_FixedData = null;
		protected OperatorVariableData m_VariableData = null;
		#endregion

		private SpriteRenderer m_SpriteRenderer = null;

		private Sprite m_FrontSprite = null;
		private Sprite m_BackSprite = null;

		// 배치중 방향(설정 중일 때 방향)
		private E_OperatorDirection m_SettingDirection = E_OperatorDirection.None;

		private bool m_IsDragging = false;
		private Vector2 m_DragStartPos;
		// 드래그로 인정할 최소 거리 (픽셀)
		private float m_DragThreshold = 150f;

		// 공격(평타) 쿨타임
		private UtilClass.Timer m_AttackCoolTimer = null;
		// 공격 범위 타일
		private List<Tile> m_AttackRangeInTileList = null;

		// 현재 저지 수
		private int m_CurrentBlock = 0;
		// 최대 저지 수
		private int m_MaxBlock = 0;

		private Enemy m_AttackTartgetEnemy = null;
		private bool isAttack = true;

		#region 스킬
		private bool isFinishedSkillSetting = false;
		private UtilClass.Timer m_AutoSPGainTimer = null;

		private IOperatorSkill m_OperatorSkill = null;
		private bool OnSkillButton = false;
		#endregion
		#endregion

		#region 프로퍼티
		public OperatorData operatorData
		{
			get => m_OperatorData;
			set
			{
				m_OperatorData = value;
				m_FixedData = value.FixedData;
				m_VariableData = value.VariableData;
				m_FrontSprite = M_Operator.GetOperatorFrontSprite(value.key);
				m_BackSprite = M_Operator.GetOperatorBackSprite(value.key);

				m_AttackCoolTimer.interval = value.VariableData.CurrentAttackSpeed / 100;
				m_MaxBlock = value.VariableData.BlockCount;
			}
		}
		public OperatorFixedData fixedData => m_FixedData;
		public OperatorVariableData variableData => m_VariableData;

		// 현재 방향
		public E_OperatorDirection currentDirection { get; protected set; }
		private bool isFinishedDirectionSetting => currentDirection != E_OperatorDirection.None;
		private float sqrDragThreshold => m_DragThreshold * m_DragThreshold;

		public int deploymentIndex { get; set; }
		public Tile deploymentTile { get; set; }
		public int redeployCount { get; set; }

		protected bool isAlive => m_OperatorData.VariableData.CurrentHp > 0;

		//public OperatorSkill operatorSkill { get; set; }
		#endregion

		#region 이벤트

		#region 이벤트 함수
		/// <summary>
		/// Enemy와 Operator가 접촉됬을 경우 호출되는 함수
		/// </summary>
		private void OnDetectedEnemy(Collider2D target)
		{
			if (!target.gameObject.CompareTag("Enemy"))
				return;
			Debug.Log("DetectedEnemy");
			Enemy lockOnEnemy = target.GetComponent<Enemy>();
			//에너미의 공격속도를 알수있는정보루트 만들어달라하기
			//에너미의 데미지타입 공격력 관통력알수있는 정보루트 만들어달라하기
			//TakeDamage(lockOnEnemy.damageType, lockOnEnemy.atk, lockOnEnemy.penetration);
		}

		/// <summary>
		/// 스킬 버튼을 눌렀을때 실행되는 함수
		/// </summary>
		public void OnSkillButtonClicked()
		{
			if (m_OperatorSkill.IsSkillActive)
				return;
			m_OperatorSkill.OnClickSkillEvent(operatorData);
		}
		#endregion
		#endregion

		#region 매니저
		private static OperatorManager M_Operator => OperatorManager.Instance;
		private static GamePlayingSceneUIManager M_GamePlayingUI => GamePlayingSceneUIManager.Instance;

		protected static GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;

		private static TileManager M_TileManager => TileManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			SetDirectionProcess();

			if (isFinishedDirectionSetting == false)
				return;

			AttackEnemyProcess();

			SkillSettingProcess();
			AutoSPGainProcess();

			AutoActivateSkill();
			MenualAcivateSkill();
		}

		//저지시킬때
		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.gameObject.CompareTag("Enemy"))
			{
				//현제 최대치로 저지하고있으면 리턴
				if (m_MaxBlock <= m_CurrentBlock)
					return;
				++m_CurrentBlock;

			}
		}

		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public override void InitializePoolItem()
		{
			base.InitializePoolItem();

			if (m_SpriteRenderer == null)
				m_SpriteRenderer = transform.Find<SpriteRenderer>("Renderer");
			if (m_AttackRangeInTileList == null)
				m_AttackRangeInTileList = new List<Tile>();
			if (m_AttackCoolTimer == null)
				m_AttackCoolTimer = new UtilClass.Timer();

			currentDirection = E_OperatorDirection.None;
			m_SettingDirection = E_OperatorDirection.None;
			redeployCount = 0;
			m_AutoSPGainTimer = new UtilClass.Timer(1f);

		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void FinallizePoolItem()
		{
			base.FinallizePoolItem();

			m_AttackRangeInTileList.Clear();
			m_AttackCoolTimer.Clear();
		}
		#endregion
		#endregion

		// 배치
		public void Deploy()
		{
			currentDirection = E_OperatorDirection.None;
			m_SettingDirection = E_OperatorDirection.None;
		}

		// 퇴각
		public void Retreat()
		{
			ResetDirection();

			// 퇴각 코스트(배치 코스트의 절반) 반환
			M_GamePlaying.currentCost += (variableData.DeploymentCost >> 1);

			// 배치 코스트 2회에 한해 절반 증가
			if (redeployCount++ < 2)
				variableData.DeploymentCost += variableData.DeploymentCost >> 1;

			gameObject.SetActive(false);
		}
		private void ResetDirection()
		{
			m_SpriteRenderer.sprite = m_FrontSprite;
			m_SpriteRenderer.flipX = false;

			currentDirection = E_OperatorDirection.None;
		}

		private void SetDirectionProcess()
		{
			if (isFinishedDirectionSetting == true)
				return;

			SetDirectionStart();
			SetDirection();
			SetDirectionEnd();
		}
		// 클릭했을 때
		private bool SetDirectionStart()
		{
			if (Mouse.current.leftButton.wasPressedThisFrame == false)
				return false;

			m_DragStartPos = Mouse.current.position.value;
			m_IsDragging = true;

			return true;
		}
		// 드래그중일 때
		private bool SetDirection()
		{
			if (Mouse.current.leftButton.isPressed == false)
				return false;
			if (m_IsDragging == false)
				return false;

			Vector2 currentPos = Mouse.current.position.value;
			Vector2 diff = currentPos - m_DragStartPos;

			// 일정거리이상 드래그하지못했을 때
			if (diff.sqrMagnitude < sqrDragThreshold)
				return false;

			UpdateDraggingDirection(diff);

			return true;
		}
		// 드래그를 끝냈을 때
		private bool SetDirectionEnd()
		{
			if (Mouse.current.leftButton.wasReleasedThisFrame == false)
				return false;
			if (m_IsDragging == false)
				return false;

			m_IsDragging = false;

			Vector2 currentPos = Mouse.current.position.value;
			Vector2 diff = currentPos - m_DragStartPos;

			// 드래그가 일정범위를 벗어나지않은상태에서 해제되었을 때
			if (diff.sqrMagnitude < sqrDragThreshold)
			{
				m_SettingDirection = E_OperatorDirection.None;

				M_GamePlayingUI.OnDeploymentCancelButtonClicked();

				return false;
			}

			// 방향지정까지 오퍼레이터 배치가 완료되었을 때
			currentDirection = m_SettingDirection;

			M_GamePlaying.DeployOperator(this);
			M_GamePlayingUI.OnSettingDirectionEnd();

			return true;
		}

		/// <summary>
		/// 좌표 리스트를 회전하여 리턴해주는 함수
		/// </summary>
		/// <returns></returns>
		public List<Vector2Int> RotatePosList(List<Vector2Int> tiles, Vector2Int pivot, E_OperatorDirection direction)
		{
			List<Vector2Int> rotated = new List<Vector2Int>();

			foreach (var tile in tiles)
			{
				Vector2Int relative = tile - pivot;

				Vector2Int rotatedRelative = direction switch
				{
					E_OperatorDirection.Left => new Vector2Int(-relative.x, -relative.y),
					E_OperatorDirection.Up => new Vector2Int(-relative.y, relative.x),
					E_OperatorDirection.Right => relative,
					E_OperatorDirection.Down => new Vector2Int(relative.y, -relative.x),
					_ => relative
				};

				rotated.Add(rotatedRelative + pivot);
			}

			return rotated;
		}
		private void UpdateDraggingDirection(Vector2 diff)
		{
			if (Mathf.Abs(diff.x) >= Mathf.Abs(diff.y))
			{
				m_SettingDirection = (diff.x > 0) ? E_OperatorDirection.Right : E_OperatorDirection.Down;
			}
			else
			{
				m_SettingDirection = (diff.y > 0) ? E_OperatorDirection.Up : E_OperatorDirection.Down;
			}

			m_SpriteRenderer.sprite = (diff.y > 0) ? m_BackSprite : m_FrontSprite;
			m_SpriteRenderer.flipX = m_SettingDirection == E_OperatorDirection.Right;
		}

		private void LinkTileDetectedEnemy()
		{
			foreach (var tile in m_AttackRangeInTileList)
			{
				tile.onColliderDetected += OnDetectedEnemy;
			}
		}
		private void UnlinkTileDetectedEnemy()
		{
			foreach (var tile in m_AttackRangeInTileList)
			{
				tile.onColliderDetected -= OnDetectedEnemy;
			}
		}
		//체력이 0되었을때
		private void UpdateDeadOperator()
		{
			if (isAlive == true)
				return;

			if (deploymentTile == null ||
				deploymentTile.currentOperator == null)
				return;

			deploymentTile.RetreatOperator();

			UnlinkTileDetectedEnemy();
		}

		public void FindedEnemyInAttackRangeTile()
		{
			UtilClass.Timer operatorAttackTimer = new UtilClass.Timer(m_VariableData.CurrentAttackSpeed / 100);
			operatorAttackTimer.interval = m_VariableData.CurrentAttackSpeed / 100;
		}

		/// <summary>
		/// 공격범위 타일 안에 들어온 Enemy를 공격하는 함수
		/// </summary>
		public void AttackEnemyProcess()
		{
			if (m_AttackRangeInTileList == null || m_AttackRangeInTileList.Count == 0 || !isAttack)
				return;

			foreach (var tile in m_AttackRangeInTileList)
			{
				m_AttackCoolTimer.Update();
				if (m_AttackCoolTimer.TimeCheck())
				{
					m_AttackTartgetEnemy = tile.GetFirstEnemy();
					m_AttackTartgetEnemy.TakeDamage(m_VariableData.DamageType, m_VariableData.Atk, m_VariableData.Penetration);
				}
			}
		}

		/// <summary>
		/// 물리딜: (상대 공격력 - 본인 방어력)
		/// 마법딜: (상대 공격력) / (본인 마법 저항)
		/// </summary>
		/// <param name="damageType">받는 대미지 타입</param>
		/// <param name="atk">상대 공격력</param>
		public void TakeDamage(E_DamageType damageType, float atk)
		{
			float damage = 0f;

			switch (damageType)
			{
				case E_DamageType.Physics:
					damage = atk - m_VariableData.Def;
					break;
				case E_DamageType.Magic:
					damage = atk * (100 - m_VariableData.Res) / 100;
					break;
				case E_DamageType.True:
					damage = atk;
					break;
			}

			// 최소 대미지 5%
			float minDamage = atk * 0.05f;

			DecreaseHp(Mathf.Max(minDamage, damage));
		}
		private void DecreaseHp(float value)
		{
			m_VariableData.CurrentHp -= value;

			UpdateDeadOperator();
		}

		// enum, switch 이용한 방법 쓰면 안됨
		// virtual or abstract 상속 구조 사용할 것
		// Operator: abstract UseSkill 함수 구현
		// Vanguard: 직군 공용 스킬 구현(코스트 획득 등)
		// 머틀: 머틀 고유 스킬 구현

		private void SkillSettingProcess()
		{
			if (isFinishedSkillSetting == true)
				return;
			m_OperatorSkill = operatorData.VariableData.SkillData.CreateSkill();
			isFinishedSkillSetting = true;
		}

		private void AutoSPGainProcess()
		{
			if (m_VariableData.SkillData.SPGainType != E_SPGainType.Auto)
				return;
			if (m_OperatorSkill.IsSkillActive == true)
				return;
			m_AutoSPGainTimer.Update();

			if (m_AutoSPGainTimer.TimeCheck(true) == true)
			{
				m_OperatorSkill.RecoverSP(1);
			}
		}
		/// <summary>
		/// 자동발동스킬함수
		/// </summary>
		public void AutoActivateSkill()
		{
			if (m_VariableData.SkillData.SkillActivationType != E_SkillActivationType.Auto)
				return;
			if (m_OperatorSkill.IsSkillEnd == false)
				return;
			m_OperatorSkill.Activate(m_OperatorData);
		}
		/// <summary>
		/// 수동발동스킬함수
		/// </summary>
		public void MenualAcivateSkill()
		{
			if (m_OperatorSkill.IsSkillActive)
			{
				m_OperatorSkill.Activate(m_OperatorData);
			}
		}
	}
}