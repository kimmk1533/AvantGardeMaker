using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.EnemySpace
{
	public class EnemySkill : SerializedMonoBehaviour, Stealth, Splash, SuicideExplode
	{
		#region 변수
		#endregion

		#region 프로퍼티
		bool Stealth.isInfinity { get; set; }
		float Stealth.duration { get; set; }
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
	}
}