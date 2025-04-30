using System;
using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace;
using AvantGardeMaker.OperatorSpace;
using AvantGardeMaker.OperatorSpace.Enum;
using AvantGardeMaker.EnemySpace;
using AvantGardeMaker.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AvantGardeMaker.TileSpace
{
	public class Tile : ObjectPoolItemBase, IPointerClickHandler
	{
		#region 변수
		//현제 타일의 위에 있는 에너미 리스트 먼저들어온 Enemy가 앞순서의 인덱스를 가짐
		[SerializeField, ReadOnly]
		private List<Enemy> m_EnemyOnTileList = null;
		#endregion

		#region 프로퍼티
		[field: SerializeField, ReadOnly]
		public Operator currentOperator { get; private set; }
		public List<Enemy> enemyOnTileList
		{
			get => m_EnemyOnTileList;
			set => m_EnemyOnTileList = value;
		}
		#endregion

		#region 이벤트
		public event Action<Collider2D> onColliderDetected = null;
		public event Action<Tile> onTileClicked = null;

		#region 이벤트 함수
		private void UpdateDetectedEnemyProcess(Collider2D collider)
		{
			if (collider.gameObject.CompareTag("Enemy") == false)
				return;

			//적이 타일 콜리더와충돌했을때
			Debug.Log("UpdateDetectedEnemyProcess");
			onColliderDetected?.Invoke(collider);
			m_EnemyOnTileList.Add(collider.GetComponent<Enemy>());
		}
		#endregion
		#endregion

		#region 매니저
		private GamePlayingUIManager M_GamePlayingUI => GamePlayingUIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void OnTriggerEnter2D(Collider2D collider)
		{
			UpdateDetectedEnemyProcess(collider);
		}
		private void OnTriggerExit2D(Collider2D collider)
		{
			if (collider.gameObject.CompareTag("Enemy") == false)
				return;

			//적이 충돌한 타일을 벗어났을때
			onColliderDetected = null;
			m_EnemyOnTileList.Remove(collider.GetComponent<Enemy>());
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public override void InitializePoolItem()
		{
			base.InitializePoolItem();

			if (m_EnemyOnTileList == null)
				m_EnemyOnTileList = new List<Enemy>();

			onTileClicked += M_GamePlayingUI.OnTileClicked;
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void FinallizePoolItem()
		{
			base.FinallizePoolItem();

			m_EnemyOnTileList.Clear();

			onTileClicked -= M_GamePlayingUI.OnTileClicked;
		}
		#endregion

		// 오퍼레이터 배치
		public void DeployOperator(Operator oper)
		{
			currentOperator = oper;

			currentOperator.Deploy();
		}
		// 오퍼레이터 퇴각
		public void RetreatOperator()
		{
			if (currentOperator == null)
				return;

			M_GamePlayingUI.activeRetreatButton = false;
			M_GamePlayingUI.activeSkillButton = false;
			M_GamePlayingUI.operatorStatusUI.gameObject.SetActive(false);

			currentOperator.Retreat();
			currentOperator = null;

			OperatorSquadUI squadUI = M_GamePlayingUI.GetOperatorSquadUI(this);
			squadUI.gameObject.SetActive(true);
			squadUI.StartRedeployment();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (currentOperator == null)
				return;

			onTileClicked?.Invoke(this);
		}
	}
}