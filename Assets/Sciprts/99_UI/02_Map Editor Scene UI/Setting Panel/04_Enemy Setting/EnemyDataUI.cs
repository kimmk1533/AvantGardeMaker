using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.EnemySpace;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class EnemyDataUI : MapEditingUIPoolItem
	{
		#region 변수
		private Button m_Button = null;

		private TMP_Text m_DebugText = null;
		#endregion

		#region 프로퍼티
		[field: SerializeField]
		public EnemyData enemyData { get; set; }

		public string debugText
		{
			get => m_DebugText.text;
			set => m_DebugText.text = value;
		}
		#endregion

		#region 이벤트

		#region 이벤트 함수
		private void OnAddButtonClicked()
		{
			EnemySpawnDataUI enemySpawnDataUI = M_MapEditingUI.GetBuilder("Enemy Spawn Data UI")
				.SetParent(M_MapEditingUI.enemySpawnDataUIParent.transform)
				.SetScale(Vector3.one)
				.SetActive(true)
				.SetAutoInit(true)
				.Spawn<EnemySpawnDataUI>();

			enemySpawnDataUI.enemyData = enemyData;
			enemySpawnDataUI.debugText = enemyData.KorName;
		}
		#endregion
		#endregion

		#region 매니저
		private static MapEditingUIManager M_MapEditingUI => MapEditingUIManager.Instance;
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

			if (m_DebugText == null)
			{
				m_DebugText = transform.GetComponentInChildren<TMP_Text>();
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