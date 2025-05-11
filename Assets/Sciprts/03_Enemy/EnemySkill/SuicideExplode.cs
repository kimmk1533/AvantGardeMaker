using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.CoreSpace.Enum;
using AvantGardeMaker.OperatorSpace;
using Sirenix.OdinInspector;
using UnityEngine;
using static AvantGardeMaker.EnemySpace.EnemySkill;

namespace AvantGardeMaker.EnemySpace
{
	public class SuicideExplode : EnemySkill
	{
		#region 변수
		private EnemyDeadEffectRange m_AoeRange = null;

		private float m_AoeRadius;
		private float m_DamageCoefficient;
		private E_DamageType m_DamageType;
		#endregion

		#region 프로퍼티
		public float AoeRadius
		{
			get { return m_AoeRadius; }
			set { m_AoeRadius = value; }
		}
		public float DamageCoefficient
		{
			get { return m_DamageCoefficient; }
			set { m_DamageCoefficient = value; }
		}
		public E_DamageType DamageType
		{
			get { return m_DamageType; }
			set { m_DamageType = value; }
		}
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수
		#endregion

		#region 초기화
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{
			m_AoeRange = GetComponent<EnemyDeadEffectRange>();
			if (m_AoeRange == null)
				throw new System.Exception("m_AoeRange is null.");

			m_AoeRange.Initialize();
			CircleCollider2D circleCollider2D = m_AoeRange.GetComponent<CircleCollider2D>();
			circleCollider2D.radius = m_AoeRadius;
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{
			m_AoeRange = null;
		}
		#endregion

		#region GetSet
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
		#endregion

		public void OnDeadSkill(float damage)
		{
			Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, m_AoeRadius);
			for (int i = 0; i < hits.Length; i++)
			{
				Operator oper = hits[i].GetComponent<Operator>();
				if (oper == null)
					continue;

				oper.TakeDamage(m_DamageType, damage * m_DamageCoefficient);
			}
		}
	}
}