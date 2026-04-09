using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoreSources
{
	public class MainMenuSceneEventControllerBase : SceneEventController
	{
		#region 기본 템플릿
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

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			//AddBeforeEvent("Game Playing Scene", M_Game.FinallizeMainMenuScene);

			//AddAfterEvent("Game Playing Scene", M_Game.InitializeGamePlayingScene);
			AddAfterEvent("Game Playing Scene", Physics2D.SyncTransforms);
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		protected override void Finallize()
		{
			base.Finallize();
		}
		#endregion

		#region 유니티 콜백 함수
		protected override void OnApplicationQuit()
		{
			//M_Game.FinallizeMainMenuScene();
			//M_Game.Finallize();
		}
		#endregion
		#endregion
	}
}