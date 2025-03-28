using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.MikangMark
{
	public enum E_Jop
	{
		BangGard, Sniper, Gard, Caster,
		Defender, Medic, Specialist, Supporter
	}

	public enum E_AttackRange { Close, Far }
	public enum E_AttackSpeed { Very_Slow, Slow, Nomal, Fast, VeryFast }

	public enum E_ResetSpeed { Slow , Nomal, Fast }

	public class CharEnum : SerializedMonoBehaviour
	{
		#region 변수
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
	}
}