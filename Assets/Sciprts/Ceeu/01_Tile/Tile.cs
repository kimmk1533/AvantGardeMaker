using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.Ceeu.Enum;
using AvantGardeMaker.MikangMark;
using AvantGardeMaker.MikangMark.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.Ceeu
{
	public class Tile : ObjectPoolItemBase
	{
		#region 변수
		[SerializeField, ReadOnly]
		private Operator m_OperatorOnTile = null;

		private OperatorSquadUI m_OperatorSquadUI = null;

		//private List<Operator> m_OperatorsTargetingThisTile = null;
		#endregion

		#region 프로퍼티
		public Operator tileOnOperator
		{
			get => m_OperatorOnTile;
			set => m_OperatorOnTile = value;
		}
		public OperatorSquadUI operatorSquadUI
		{
			get => m_OperatorSquadUI;
			set => m_OperatorSquadUI = value;
		}
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		private OperatorManager M_Operator => OperatorManager.Instance;
		private GamePlayingUIManager M_GamePlayingUI => GamePlayingUIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{

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
			M_GamePlayingUI.OperatorRetreateButtonActive(false);
			M_GamePlayingUI.OperatorSquadUIReDeploymentActive(m_OperatorSquadUI);
			m_OperatorOnTile.ResetDirection();
			m_OperatorOnTile.currentDirection = E_OperatorDirection.None;
			M_Operator.Despawn(m_OperatorOnTile);
		}
	}
}