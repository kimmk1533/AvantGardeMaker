using System.Collections;
using System.Collections.Generic;
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
		#endregion

		#region 프로퍼티
		private bool isSettedDirection => m_CurrentDirection != E_OperatorDirection.None;
		#endregion

		#region 이벤트
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
				//tile.tileOnOperator = m_PreviewOperator;
				M_GamePlaying.SettingOperatorOnTile(this);
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

		#region MikangMark
		/*
		public void SetDirection(E_OperatorDirection direction, E_OperatorDirection lastDirection)
		{
			float radian = 0;
			switch (direction)
			{
				case E_OperatorDirection.Left:
					if (direction == m_OperatorData.FixedData.HorizontalDirection)//같은방향으로드래그했을경우
					{
						return;
					}
					switch (lastDirection)
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
					if (m_OperatorData.FixedData.VerticalDirection == E_OperatorDirection.Up)
					{
						m_SpriteRenderer.sprite = m_OperatorData.LeftUpImg;
					}
					else
					{
						m_SpriteRenderer.sprite = m_OperatorData.LeftDownImg;
					}
					lastDirection = E_OperatorDirection.Left;
					RotateAtkRange(m_OperatorData.AttackPos, radian);
					m_OperatorData.HorizontalDirection = E_OperatorDirection.Left;
					M_GamePlayingUI.OperAtkRangeHighlight(M_GamePlayingUI.RotateViewAtkRange(m_OperatorData.AttackPos, radian), M_GamePlayingUI.m_CreatedAttackRangeHighlightList[0]);
					break;
				case E_OperatorDirection.Right:
					if (direction == m_OperatorData.HorizontalDirection)
					{
						return;
					}
					switch (lastDirection)
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
					if (m_OperatorData.VerticalDirection == E_OperatorDirection.Up)
					{
						m_SpriteRenderer.sprite = m_OperatorData.RightUpImg;
					}
					else
					{
						m_SpriteRenderer.sprite = m_OperatorData.RightDownImg;
					}
					lastDirection = E_OperatorDirection.Right;
					RotateAtkRange(m_OperatorData.AttackPos, radian);
					m_OperatorData.HorizontalDirection = E_OperatorDirection.Right;
					M_GamePlayingUI.OperAtkRangeHighlight(M_GamePlayingUI.RotateViewAtkRange(m_OperatorData.AttackPos, radian), M_GamePlayingUI.m_CreatedAttackRangeHighlightList[0]);
					break;
				case E_OperatorDirection.Down:
					if (direction == m_OperatorData.VerticalDirection)
					{
						return;
					}
					switch (lastDirection)
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
					if (m_OperatorData.HorizontalDirection == E_OperatorDirection.Left)
					{
						m_SpriteRenderer.sprite = m_OperatorData.LeftDownImg;
					}
					else
					{
						m_SpriteRenderer.sprite = m_OperatorData.RightDownImg;
					}
					lastDirection = E_OperatorDirection.Down;
					RotateAtkRange(m_OperatorData.AttackPos, radian);
					m_OperatorData.VerticalDirection = E_OperatorDirection.Down;
					M_GamePlayingUI.OperAtkRangeHighlight(M_GamePlayingUI.RotateViewAtkRange(m_OperatorData.AttackPos, radian), M_GamePlayingUI.m_CreatedAttackRangeHighlightList[0]);
					break;
				case E_OperatorDirection.Up:
					if (direction == m_OperatorData.VerticalDirection)
					{
						return;
					}
					switch (lastDirection)
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
					if (m_OperatorData.HorizontalDirection == E_OperatorDirection.Left)
					{
						m_SpriteRenderer.sprite = m_OperatorData.LeftUpImg;
					}
					else
					{
						m_SpriteRenderer.sprite = m_OperatorData.RightUpImg;
					}
					lastDirection = E_OperatorDirection.Up;
					RotateAtkRange(m_OperatorData.AttackPos, radian);
					m_OperatorData.VerticalDirection = E_OperatorDirection.Up;
					M_GamePlayingUI.OperAtkRangeHighlight(M_GamePlayingUI.RotateViewAtkRange(m_OperatorData.AttackPos, radian), M_GamePlayingUI.m_CreatedAttackRangeHighlightList[0]);
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
			m_OperatorData.RealHp += _Value;
		}
		*/
		#endregion
	}

}