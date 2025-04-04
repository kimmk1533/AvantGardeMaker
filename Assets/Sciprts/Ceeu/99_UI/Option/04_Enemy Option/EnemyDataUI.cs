using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.ad1a;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class EnemyDataUI : MapEditorUI
	{
		#region 변수
		private Button m_Button = null;
		#endregion

		#region 프로퍼티
		[field: SerializeField]
		public EnemyData enemyData { get; set; }
		#endregion

		#region 이벤트

		#region 이벤트 함수
		public void OnAddButtonClicked()
		{
			EnemySpawnDataUI enemySpawnDataUI = M_MapEditorUI.GetBuilder("Enemy Spawn Data UI")
				.SetParent(M_MapEditorUI.enemySpawnDataUIParent.transform)
				.SetScale(Vector3.one)
				.SetActive(true)
				.SetAutoInit(true)
				.Spawn() as EnemySpawnDataUI;

			enemySpawnDataUI.enemyData = enemyData;
			M_MapEditor.AddEnemyData(enemyData);
		}
		#endregion
		#endregion

		#region 매니저
		private static MapEditorUIManager M_MapEditorUI => MapEditorUIManager.Instance;
		private static MapEditorManager M_MapEditor => MapEditorManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public override void InitializePoolItem()
		{
			base.InitializePoolItem();

			if (m_Button == null)
			{
				m_Button = GetComponent<Button>();

				m_Button.onClick.AddListener(OnAddButtonClicked);
			}
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void FinallizePoolItem()
		{
			base.FinallizePoolItem();


		}
		#endregion
	}
}