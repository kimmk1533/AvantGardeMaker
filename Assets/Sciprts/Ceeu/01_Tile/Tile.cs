using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.Ceeu.Enum;
using AvantGardeMaker.MikangMark;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.Ceeu
{
	public class Tile : ObjectPoolItemBase
	{
		#region 변수
		[SerializeField, ReadOnly]
		private Operator m_TileOnOperator = null;
		#endregion

		#region 프로퍼티
		public Operator tileOnOperator
		{
			get => m_TileOnOperator;
			set => m_TileOnOperator = value;
		}
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		private OperatorManager M_Operator => OperatorManager.Instance;
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
			if (m_TileOnOperator == null)
				return;
			M_Operator.Despawn(m_TileOnOperator);
		}
	}
}