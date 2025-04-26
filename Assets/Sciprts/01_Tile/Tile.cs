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

namespace AvantGardeMaker.TileSpace
{
	public class Tile : ObjectPoolItemBase
	{
		#region 변수
		[SerializeField, RuntimeReadOnly]
		private Operator m_OperatorOnTile = null;

		private OperatorSquadUI m_OperatorSquadUI = null;
		//현제 타일의 위에 있는 에너미 리스트 먼저들어온 Enemy가 앞순서의 인덱스를 가짐
		[SerializeField, ReadOnly]
		private List<Enemy> m_EnemyOnTileList = null;
		#endregion

		#region 프로퍼티
		public Operator operatorOnTile
		{
			get => m_OperatorOnTile;
			set => m_OperatorOnTile = value;
		}
		public OperatorSquadUI operatorSquadUI
		{
			get => m_OperatorSquadUI;
			set => m_OperatorSquadUI = value;
		}
		public List<Enemy> enemyOnTileList
		{
			get => m_EnemyOnTileList;
			set => m_EnemyOnTileList = value;
		}

		#endregion

		#region 이벤트
		public event Action<Collider2D> onColliderDetected;
		#endregion

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

		#region 매니저
		private OperatorManager M_Operator => OperatorManager.Instance;
		private GamePlayingUIManager M_GamePlayingUI => GamePlayingUIManager.Instance;
		private GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Awake()
		{
			Initialize();
		}
		private void OnTriggerEnter2D(Collider2D collider)
		{
			UpdateDetectedEnemyProcess(collider);
		}
		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.gameObject.CompareTag("Enemy") == false)
				return;
			//적이 충돌한 타일을 벗어났을때
			onColliderDetected = null;
			m_EnemyOnTileList.Remove(collision.GetComponent<Enemy>());
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{
			m_EnemyOnTileList = new List<Enemy>();
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{

		}
		#endregion

		public void RetreatOperatorOnTile()
		{
			if (m_OperatorOnTile == null)
				return;

			M_GamePlayingUI.activeRetreatButton = false;
			M_GamePlayingUI.OperatorSquadUIReDeploymentActive(m_OperatorSquadUI);

			m_OperatorOnTile.Retreat();

			M_Operator.Despawn(m_OperatorOnTile);
		}
	}
}