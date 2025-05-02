using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using static AvantGardeMaker.EnemySpace.IEnemySkill;

namespace AvantGardeMaker.EnemySpace
{
	public class SuicideExplode : IOnDeadSkill, IEnemySkill
	{
		#region 변수
		private float m_AoeRadius;
		private float m_DamageCoefficient;
		private E_DamageType m_DamageType;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수
		#endregion

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
		public void SetAoeRadius(float radius)
		{
			m_AoeRadius = radius;
		}
		public void SetDamageCoef(float coef)
		{
			m_DamageCoefficient = coef;
		}
		public void SetDamageType(E_DamageType damageType)
		{
			m_DamageType = damageType;
		}

		public void OnDeadSkill()
		{
			throw new System.NotImplementedException();
		}
	}
}