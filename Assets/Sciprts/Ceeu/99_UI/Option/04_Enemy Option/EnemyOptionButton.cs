using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.ad1a;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class EnemyOptionButton : SerializedMonoBehaviour
	{
		#region 변수
		private EnemyData m_EnemyData = null;

		[SerializeField]
		private Button m_Button = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		private static EnemySpawnDataUIManager M_EnemySpawnDataUI => EnemySpawnDataUIManager.Instance;
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

		public void OnAddButtonClicked()
		{
			EnemySpawnDataUI enemySpawnDataUI = M_EnemySpawnDataUI.GetBuilder("Enemy Spawn Data UI")
				.SetParent(M_EnemySpawnDataUI.enemySpawnDataUIParent.transform)
				.SetScale(Vector3.one)
				.SetActive(true)
				.SetAutoInit(true)
				.Spawn();
			enemySpawnDataUI.Initialize();
		}
	}
}