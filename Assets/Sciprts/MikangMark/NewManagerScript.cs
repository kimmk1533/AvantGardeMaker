using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class NewManagerScript : Singleton<NewManagerScript>
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
	/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
	/// </summary>
	public virtual void Initialize()
	{

	}
	/// <summary>
	/// 마무리화 함수 (게임 종료 시 호출)
	/// </summary>
	public virtual void Finallize()
	{

	}

	/// <summary>
	/// 게임 초기화 함수 (Game Scene 진입 시 호출)
	/// </summary>
	public virtual void InitializeGame()
	{

	}
	/// <summary>
	/// 게임 마무리화 함수 (Game Scene 나갈 시 호출)
	/// </summary>
	public virtual void FinallizeGame()
	{

	}
}