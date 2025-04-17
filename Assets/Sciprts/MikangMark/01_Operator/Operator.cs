using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.ad1a;
using AvantGardeMaker.Ceeu;
using AvantGardeMaker.MikangMark.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.MikangMark
{
	public class Operator : ObjectPoolItemBase
	{
		#region 변수
		private SpriteRenderer m_SpriteRenderer = null;

		private OperatorData m_OperatorData = null;

		private Sprite m_FrontSprite = null;
		private Sprite m_BackSprite = null;

		private E_OperatorDirection m_CurrentDirection = E_OperatorDirection.None;
		private E_OperatorDirection m_SettingDirection = E_OperatorDirection.None;

		private bool m_IsDragging = false;
		private Vector2 m_DragStartPos;
		// 드래그로 인정할 최소 거리 (픽셀)
		private float m_DragThreshold = 150f;

		private Tile m_DeploymentTile = null;

		private List<Tile> m_AttackRangeInTileList = null;

		private UtilClass.Timer m_AttackCoolTimer = null;
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
		public Tile deploymentTile
		{
			get => m_DeploymentTile;
			set => m_DeploymentTile = value;
		}
		#endregion

		#region 이벤트
		#endregion

		#region 이벤트 함수
		private void OnEnableTileDetectedEnemy()
		{
			for (int i = 0; i < m_AttackRangeInTileList.Count; i++)
			{
				m_AttackRangeInTileList[i].onColliderDetected += DetectedEnemy;
			}
		}
		private void OnDisableTileDetectedEnemy()
		{
			for (int i = 0; i < m_AttackRangeInTileList.Count; i++)
			{
				m_AttackRangeInTileList[i].onColliderDetected -= DetectedEnemy;
			}
		}
		#endregion

		#region 매니저
		private static OperatorManager M_Operator => OperatorManager.Instance;
		private static GamePlayingUIManager M_GamePlayingUI => GamePlayingUIManager.Instance;

		private static GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;
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

			m_AttackRangeInTileList = new List<Tile>();

			OnEnableTileDetectedEnemy();

			if (m_AttackCoolTimer == null)
			{
				m_AttackCoolTimer = new UtilClass.Timer();
			}
			m_AttackCoolTimer.interval = operatorData.VariableData.InitAttakSpeed;
			for (int i = 0; i < operatorData.VariableData.AttackPos.Count; i++)
			{
				//m_InAttackRangeTileList.Add();
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

				m_CurrentDirection = m_SettingDirection;
				M_GamePlayingUI.OnSettingDirectionEnd();
				M_GamePlaying.DeployOperator(this);
				M_GamePlaying.DeploymentOperatorOnTile(this);
			}
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
			if (m_DeploymentTile.tileOnOperator == null)
				return;
			m_DeploymentTile.RetreatOperatorOnTile();
			OnDisableTileDetectedEnemy();
		}

		public void ResetDirection()
		{
			m_SpriteRenderer.sprite = m_FrontSprite;
			m_SpriteRenderer.flipX = false;
		}
		
		private void DetectedEnemy(Collider2D target)
		{
			Enemy lockOnEnemy = target.GetComponent<Enemy>();
			m_AttackCoolTimer.Update();
			if (m_AttackCoolTimer.TimeCheck())
			{
				TakeAttack(lockOnEnemy);
			}
			
		}
		
		private void TakeAttack(Enemy target)
		{
			target.TakeDamage(operatorData.VariableData.DamageType, operatorData.VariableData.Atk, operatorData.VariableData.Penetration);
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
	}

}