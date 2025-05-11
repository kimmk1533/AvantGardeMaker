using System;
using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.EnemySpace
{
	public class EnemyAtkEffectRange : SerializedMonoBehaviour
	{
		#region 변수
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		public event Action<Operator> onOperatorEnterRange;
		public event Action<Operator> onOperatorExitRange;
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수
		private void OnTriggerEnter2D(Collider2D collider)
		{
			//공격 범위 내에 오퍼레이터가 있으면 공격
			if (collider.gameObject.CompareTag("Operator"))
			{
				onOperatorEnterRange?.Invoke(collider.GetComponent<Operator>());
			}
		}

		private void OnTriggerExit2D(Collider2D collider)
		{
			//오퍼레이터가 공격 범위를 벗어나면
			if (collider.gameObject.CompareTag("Operator"))
			{
				onOperatorExitRange?.Invoke(collider.GetComponent<Operator>());
			}
		}
		#endregion

		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public void Initialize()
		{

		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public void Finallize()
		{

		}
	}
}