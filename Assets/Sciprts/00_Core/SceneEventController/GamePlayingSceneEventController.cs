using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.CoreSpace.SceneEvent
{
	public class GamePlayingSceneEventController : SceneEventController
	{
		#region 변수
		[SerializeField]
		private Button m_OptionButton = null;

		[SerializeField]
		private TextMeshProUGUI m_CostValueText = null;
		[SerializeField]
		private Image m_CostFillImage = null;

		[SerializeField]
		private RectTransform m_OperatorSquadUIParent = null;
		[SerializeField]
		private OperatorStatusUI m_OperatorStatusUI = null;
		[SerializeField]
		private Button m_OperatorDeploymentCancelButton = null;
		[SerializeField]
		private Button m_OperatorRetreatButton = null;
		[SerializeField]
		private Button m_OperatorSkillButton = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트

		#region 이벤트 함수
		#endregion
		#endregion

		#region 매니저
		private static GameManager M_Game => GameManager.Instance;

		private static GamePlayingUIManager M_GamePlayingUI => GamePlayingUIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		protected override void OnApplicationQuit()
		{
			M_Game.FinallizeGamePlaying();

			M_Game.Finallize();
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			#region 멤버 변수 링킹
			M_GamePlayingUI.optionButton = m_OptionButton;

			M_GamePlayingUI.costValueText = m_CostValueText;
			M_GamePlayingUI.costFillImage = m_CostFillImage;

			M_GamePlayingUI.operatorSquadUIParent = m_OperatorSquadUIParent;
			M_GamePlayingUI.operatorStatusUI = m_OperatorStatusUI;
			M_GamePlayingUI.deploymentCancelButton = m_OperatorDeploymentCancelButton;
			M_GamePlayingUI.operatorRetreatButton = m_OperatorRetreatButton;
			M_GamePlayingUI.operatorSkillButton = m_OperatorSkillButton;
			#endregion

			// Main Menu Scene 전환 전 이벤트
			AddBeforeEvent("Main Menu Scene", M_Game.FinallizeGamePlaying);

			// Main Menu Scene 전환 후 이벤트
			AddAfterEvent("Main Menu Scene", M_Game.InitializeMainMenu);

			// Map Editing Scene 전환 전 이벤트
			AddBeforeEvent("Map Editing Scene", M_Game.FinallizeGamePlaying);

			// Map Editing Scene 전환 후 이벤트
			AddAfterEvent("Map Editing Scene", M_Game.InitializeMapEditing);
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		protected override void Finallize()
		{
			base.Finallize();


		}
		#endregion
	}
}