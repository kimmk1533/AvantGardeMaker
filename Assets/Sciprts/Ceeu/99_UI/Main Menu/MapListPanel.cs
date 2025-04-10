using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace AvantGardeMaker.Ceeu
{
	public class MapListPanel : Panel
	{
		#region 변수
		private TMP_Text m_LoadingText = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트

		#region 이벤트 함수
		#endregion
		#endregion

		#region 매니저
		private static MainMenuUIManager M_MainMenuUI => MainMenuUIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		protected override void OnEnable()
		{
			base.OnEnable();

			ClearMapListItem();

			CreateMapListItem();
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			if (m_LoadingText == null)
				m_LoadingText = transform.FindInChildren<TMP_Text>("Loading Text");
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();

			m_LoadingText.text = string.Empty;
		}
		#endregion

		private void ClearMapListItem()
		{
			Transform itemParent = M_MainMenuUI.mapListItemParent;
			int childCount = itemParent.childCount;

			for (int i = 0; i < childCount; ++i)
			{
				M_MainMenuUI.Despawn(itemParent.GetChild<MapListItem>(0));
			}

			m_LoadingText.gameObject.SetActive(true);
		}
		private async void CreateMapListItem()
		{
			Transform itemParent = M_MainMenuUI.mapListItemParent;
			List<StageData> stageDataList = await SaveLoadUtility.LoadAllStageData(M_MainMenuUI.testFilter);

			for (int i = 0; i < stageDataList.Count; ++i)
			{
				MapListItem mapListItem = M_MainMenuUI.GetBuilder("Map List Item")
					.SetParent(itemParent)
					.SetScale(Vector3.one)
					.SetLocalPosition(Vector3.zero)
					.SetActive(true)
					.SetAutoInit(true)
					.Spawn() as MapListItem;

				mapListItem.title = stageDataList[i].title;
			}

			m_LoadingText.gameObject.SetActive(false);
		}
	}
}