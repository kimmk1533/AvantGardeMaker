using System.Collections;
using System.Collections.Generic;
using CoreSources;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class GamePlayingSceneUIPoolItem : ObjectPoolItem<GamePlayingSceneUIPoolItem>
	{
		#region 변수

		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트

		#region 이벤트 함수
		#endregion
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (생성될 때)
		/// </summary>
		public override void Initialize()
		{

		}
		/// <summary>
		/// 마무리화 함수 (파괴될 때)
		/// </summary>
		public override void Finallize()
		{

		}

		/// <summary>
		/// 초기화 함수 (스폰될 때)
		/// </summary>
		public override void InitializePoolItem()
		{
			base.InitializePoolItem();


		}
		/// <summary>
		/// 마무리화 함수 (디스폰될 때)
		/// </summary>
		public override void FinallizePoolItem()
		{

			base.FinallizePoolItem();
		}
		#endregion
	}
}