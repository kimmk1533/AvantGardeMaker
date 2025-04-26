using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace;
using AvantGardeMaker.OperatorSpace.Enum;
using AvantGardeMaker.EnemySpace;
using AvantGardeMaker.TileSpace;
using AvantGardeMaker.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using AvantGardeMaker.CoreSpace.Enum;
using TileValue = System.ValueTuple<AvantGardeMaker.TileSpace.Enum.E_TileType, AvantGardeMaker.TileSpace.Tile>;

namespace AvantGardeMaker.OperatorSpace
{
	public class Operator : ObjectPoolItemBase
	{
		#region 변수
		private SpriteRenderer m_SpriteRenderer = null;
		protected OperatorData m_OperatorData = null;

		private Sprite m_FrontSprite = null;
		private Sprite m_BackSprite = null;

		private E_OperatorDirection m_CurrentDirection = E_OperatorDirection.None;
		private E_OperatorDirection m_SettingDirection = E_OperatorDirection.None;

		private List<Vector2Int> m_currentAttackRangePosList = null;

		private bool m_IsDragging = false;
		private Vector2 m_DragStartPos;
		// 드래그로 인정할 최소 거리 (픽셀)
		private float m_DragThreshold = 150f;

		private TileValue m_DeploymentTile;
		[SerializeField, ReadOnly]
		private Dictionary<Vector2Int, Tile> m_AttackRangeInTileMap = null;

		private UtilClass.Timer m_AttackCoolTimer = null;

		protected List<OperatorSkill> m_OperatorSkillList = null;

		private int m_ReDeployCount = 0;

		private int m_CurrentDeploymentCost = 0;


		#endregion

		#region 프로퍼티
		private bool isSettedDirection => m_CurrentDirection != E_OperatorDirection.None;
		public E_OperatorDirection currentDirection
		{
			get => m_CurrentDirection;
			set => m_CurrentDirection = value;
		}
		public OperatorData operatorData
		{
			get => m_OperatorData;
			set => m_OperatorData = value;
		}
		public int currentDeploymentCost
		{
			get => m_CurrentDeploymentCost;
			set => m_CurrentDeploymentCost = value;
		}
		public TileValue deploymentTile
		{
			get => m_DeploymentTile;
			set => m_DeploymentTile = value;
		}
		public List<OperatorSkill> operatorSkillList
		{
			get => m_OperatorSkillList;
			set => m_OperatorSkillList = value;
		}
		public int redeployCount
		{
			get => m_ReDeployCount;
			set => m_ReDeployCount = value;
		}
		#endregion

		#region 이벤트
		#endregion

		#region 이벤트 함수
		private void OnEnableTileDetectedEnemy()
		{
			foreach(var map in m_AttackRangeInTileMap)
			{
				map.Value.onColliderDetected += DetectedEnemy;
			}
		}
		private void OnDisableTileDetectedEnemy()
		{
			foreach (var map in m_AttackRangeInTileMap)
			{
				map.Value.onColliderDetected -= DetectedEnemy;
			}
		}
		#endregion

		#region 매니저
		private static OperatorManager M_Operator => OperatorManager.Instance;
		private static GamePlayingUIManager M_GamePlayingUI => GamePlayingUIManager.Instance;

		protected static GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;

		private static TileManager M_TileManager => TileManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			SettingDirectionProcess();
			UpdateDeadOperator();

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

			m_FrontSprite = M_Operator.GetOperatorFrontSprite(m_OperatorData.EngName);
			m_BackSprite = M_Operator.GetOperatorBackSprite(m_OperatorData.EngName);

			m_AttackRangeInTileMap = new Dictionary<Vector2Int, Tile>();

			OnEnableTileDetectedEnemy();

			if (m_AttackCoolTimer == null)
			{
				m_AttackCoolTimer = new UtilClass.Timer();
			}
			m_AttackCoolTimer.interval = operatorData.VariableData.InitAttakSpeed;
			m_CurrentDeploymentCost = operatorData.VariableData.DeploymentCost;
			SkillSetting();
			for (int i = 0; i < operatorData.VariableData.AttackPos.Count; i++)
			{
				//임시
				m_AttackRangeInTileMap.Add(operatorData.VariableData.AttackPos[i], M_TileManager.GetTileValue(operatorData.VariableData.AttackPos[i]).Item2);
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

		public void SetOperatorData(OperatorData operatorData)
		{
			m_OperatorData = operatorData;
		}

		private void SettingDirectionProcess()
		{
			if (isSettedDirection == true)
				return;

			//클릭했을때
			if (Input.GetMouseButtonDown(0))
			{
				m_DragStartPos = Input.mousePosition;
				m_IsDragging = true;

				M_GamePlayingUI.OnSettingDirectionStart();
			}

			//드래그중일때
			if (Input.GetMouseButton(0) && m_IsDragging)
			{
				Vector2 currentPos = Input.mousePosition;
				Vector2 diff = currentPos - m_DragStartPos;

				//일정거리이상 드래그하지못했을때
				if (diff.sqrMagnitude < m_DragThreshold * m_DragThreshold)
					return;

				UpdateDraggingDirection(diff);
			}

			if (Input.GetMouseButtonUp(0) && m_IsDragging)
			{
				m_IsDragging = false;

				Vector2 currentPos = Input.mousePosition;
				Vector2 diff = currentPos - m_DragStartPos;

				//드래그가 일정범위를 벗어나지않은상태에서 해제되었을때
				if (diff.sqrMagnitude < m_DragThreshold * m_DragThreshold)
				{
					m_SettingDirection = E_OperatorDirection.None;

					M_GamePlayingUI.OnDeploymentCancelButtonClicked();
					return;
				}
				//방향지정까지 오퍼레이터 배치가 완료되었을때
				m_CurrentDirection = m_SettingDirection;
				M_GamePlayingUI.OnSettingDirectionEnd();
				M_GamePlaying.DeployOperator(this);
				M_GamePlaying.DeploymentOperatorOnTile(this);

				for (int i = 0; i < m_OperatorSkillList.Count; i++)
				{
					if (operatorData.VariableData.SkillInfoList[i].SkillType == E_OperatorSkillType.DeployGainCost)
					{
						M_GamePlaying.GainCost((int)operatorData.VariableData.SkillInfoList[i].SkillValue);
					}
				}
				m_AttackRangeInTileMap = new Dictionary<Vector2Int, Tile>();
				m_currentAttackRangePosList = new List<Vector2Int>();
				for(int i=0; i < operatorData.VariableData.AttackPos.Count; i++)
				{
					Vector2Int copyPos = new Vector2Int (operatorData.VariableData.AttackPos[i].x, operatorData.VariableData.AttackPos[i].y);
					m_currentAttackRangePosList.Add(copyPos);
				}
				for(int i=0;i< m_currentAttackRangePosList.Count; i++)
				{
					m_currentAttackRangePosList[i] = new Vector2Int(m_currentAttackRangePosList[i].x + (int)transform.position.x, m_currentAttackRangePosList[i].y + (int)transform.position.y);
				}
				m_currentAttackRangePosList = RotatePosList(m_currentAttackRangePosList, m_currentAttackRangePosList[0], m_CurrentDirection);
				for (int i = 0; i < operatorData.VariableData.AttackPos.Count; i++)
				{
					//임시 공격범위 타일회전
					m_AttackRangeInTileMap.Add(m_currentAttackRangePosList[i], M_TileManager.GetTileValue(m_currentAttackRangePosList[i]).Item2);
					/*
					if (M_TileManager.GetTileValue(m_currentAttackRangePosList[i]).Item2.operatorAttackRangeTileList == null)
					{
						M_TileManager.GetTileValue(m_currentAttackRangePosList[i]).Item2.operatorAttackRangeTileList = new List<Operator>();
					}
					M_TileManager.GetTileValue(m_currentAttackRangePosList[i]).Item2.operatorAttackRangeTileList.Add(this);
					*/
				}
			}
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
					E_OperatorDirection.Right => relative,
					E_OperatorDirection.Left => new Vector2Int(-relative.x, -relative.y),
					E_OperatorDirection.Down => new Vector2Int(relative.y, -relative.x),
					E_OperatorDirection.Up => new Vector2Int(-relative.y, relative.x),
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
			m_SpriteRenderer.flipX = diff.x > 0;
		}

		private bool UpdateIsAlive()
		{
			if (m_OperatorData.VariableData.RealHp <= 0)
				return false;
			else
				return true;
		}
		private void UpdateDeadOperator()
		{
			//체력이 0되었을때
			if (UpdateIsAlive() == true)
				return;
			if (m_DeploymentTile.Item2.tileOnOperator == null)
				return;
			m_DeploymentTile.Item2.RetreatOperatorOnTile();
			OnDisableTileDetectedEnemy();
		}

		public void ResetDirection()
		{
			m_SpriteRenderer.sprite = m_FrontSprite;
			m_SpriteRenderer.flipX = false;
		}
		public void FindedEnemyInAttackRangeTile()
		{
			UtilClass.Timer operatorAttackTimer = new UtilClass.Timer(operatorData.VariableData.InitAttakSpeed / 100);
			operatorAttackTimer.interval = operatorData.VariableData.InitAttakSpeed / 100;
		}

		/// <summary>
		/// 공격범위 타일 안에 들어온 Enemy를 공격하는 함수
		/// </summary>
		public void AttackEnemy()
		{
			foreach (var tileMap in m_AttackRangeInTileMap)
			{
				if (tileMap.Value.enemyOnTileList == null)
					continue;

				m_AttackCoolTimer.Update();
				if (m_AttackCoolTimer.TimeCheck())
				{
					tileMap.Value.enemyOnTileList[0].TakeDamage(operatorData.VariableData.DamageType, operatorData.VariableData.Atk, operatorData.VariableData.Penetration);
				}
			}
		}

		/// <summary>
		/// Enemy와 Operator가 접촉됬을 경우 호출되는 함수
		/// </summary>
		private void DetectedEnemy(Collider2D target)
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
		/// 물리딜: 공격력 - 방어력/방어 관통
		/// 마법딜: 공격력 / 마법 저항
		/// </summary>
		public void TakeDamage(E_DamageType damageType, float value, float penetration)
		{
			switch (damageType)
			{
				case E_DamageType.Physics:
					DecreaseHp(Mathf.Max(0, operatorData.VariableData.Atk - operatorData.VariableData.Def / penetration));
					break;
				case E_DamageType.Magic:
					DecreaseHp(Mathf.Max(0, operatorData.VariableData.Atk / (operatorData.VariableData.Res / penetration)));
					break;
				case E_DamageType.True:
					DecreaseHp(Mathf.Max(0, operatorData.VariableData.Atk));
					break;
				default:
					break;
			}

		}

		private void DecreaseHp(float value)
		{
			m_OperatorData.VariableData.RealHp -= value;
		}


		public void OnClickSkillButton()
		{
			for (int i = 0; i < m_OperatorSkillList.Count; i++)
			{
				m_OperatorSkillList[i].UsingThisSkill(operatorData.VariableData.SkillInfoList[i], this);
			}
		}

		public void SkillSetting()
		{
			m_OperatorSkillList = new List<OperatorSkill>();
			OperatorSkill newSkill = new OperatorSkill();
			newSkill.skillName = m_OperatorData.FixedData.SkillName;
			newSkill.skillText = "";
			for (int i = 0; i < m_OperatorData.VariableData.SkillInfoList.Count; i++)
			{
				switch (m_OperatorData.VariableData.SkillInfoList[i].SkillType)
				{
					case E_OperatorSkillType.AttackBuff:
						break;
					case E_OperatorSkillType.DeployGainCost:
						CostCargeSkillSetting(newSkill);
						break;
					case E_OperatorSkillType.MultipleShot:
						break;
					case E_OperatorSkillType.StopAttack:
						break;
					case E_OperatorSkillType.ChangeAttackRange:
						break;
					default:
						break;
				}
				if (i != m_OperatorData.VariableData.SkillInfoList.Count - 1)
				{
					newSkill.skillText += ",";
				}
			}
		}
		public void CostCargeSkillSetting(OperatorSkill takeSkill)
		{
			takeSkill.skillText += "배치 코스트 " + m_OperatorData.VariableData.SkillInfoList + " 즉시 획득";
			m_OperatorSkillList.Add(takeSkill);
		}
	}
}