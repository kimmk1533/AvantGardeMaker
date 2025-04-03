using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.Ceeu.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class MapEditorUIManager : ObjectManager<MapEditorUIManager, MapEditorUI>
	{
		#region 변수
		[PropertySpace]
		[SerializeField]
		private List<string> m_KeyList = new List<string>();

		#region 메뉴 패널 관련 변수
		#endregion

		#region 옵션 패널 관련 변수

		#endregion
		#endregion

		#region 프로퍼티
		public List<string> keyList => m_KeyList;

		#region 메뉴 패널 관련 프로퍼티
		public MenuPanel menuPanel { get; set; }
		#endregion

		#region 옵션 패널 관련 프로퍼티
		public OptionPanel optionPanel { get; set; }

		public EnemyDataSettingPanel enemyDataSettingPanel { get; set; }

		public RectTransform enemySpawnDataUIParent { get; set; }
		public RectTransform enemyDataUIParent { get; set; }
		public RectTransform enemyWayPointDataUIParent { get; set; }
		#endregion
		#endregion

		#region 이벤트

		#region 이벤트 함수
		#endregion
		#endregion

		#region 매니저
		private static MapEditorManager M_EditMode => MapEditorManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			//MenuShortcut();
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			gameObject.SetActive(false);
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();
		}

		/// <summary>
		/// 게임 초기화 함수 (본인 Main Scene 진입 시 호출)
		/// </summary>
		public override void InitializeMain()
		{
			base.InitializeMain();

			menuPanel.Initialize();

			optionPanel.Initialize();
			enemyDataSettingPanel.Initialize();

			EnemyDataUI enemyDataUI = GetBuilder("Enemy Data UI")
				.SetScale(Vector3.one)
				.SetParent(enemyDataUIParent)
				.SetAutoInit(true)
				.SetActive(true)
				.Spawn() as EnemyDataUI;

			enemyDataUI.enemyData = new ad1a.EnemyData();

			gameObject.SetActive(true);
		}
		/// <summary>
		/// 게임 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();

			menuPanel.Finallize();

			optionPanel.Finallize();
			enemyDataSettingPanel.Finallize();
		}
		#endregion

		private void MenuShortcut()
		{
			foreach (string key in m_KeyList)
			{
				OptionViewport optionViewport = optionPanel.optionViewportController[key];
				KeyCode keyCode = optionViewport.shortcut;
				if (Input.GetKeyDown(keyCode) == true)
				{
					optionViewport.OnMenuButtonClicked();
					M_EditMode.SetEditModeType(optionViewport.editModeType);
				}
			}
		}
	}
}