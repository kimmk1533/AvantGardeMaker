using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public abstract class SceneEventSender : SerializedMonoBehaviour
{
	#region 변수
	[SerializeField]
	[DictionaryDrawerSettings(KeyLabel = "전환될 씬 이름", ValueLabel = "씬 로딩 전 호출할 이벤트")]
	protected Dictionary<string, UnityEvent> m_OnBeforeSceneLoadingEventMap = new Dictionary<string, UnityEvent>();
	#endregion

	#region 유니티 콜백 함수
	protected virtual void Awake()
	{
		Initialize();
	}
	protected virtual void OnDestroy()
	{
		Finallize();
	}
	#endregion

	#region 초기화 & 마무리화 함수
	/// <summary>
	/// 초기화 함수
	/// </summary>
	protected virtual void Initialize()
	{
		if (m_OnBeforeSceneLoadingEventMap == null)
			m_OnBeforeSceneLoadingEventMap = new Dictionary<string, UnityEvent>();
	}
	/// <summary>
	/// 마무리화 함수
	/// </summary>
	protected virtual void Finallize()
	{
		if (m_OnBeforeSceneLoadingEventMap != null)
			m_OnBeforeSceneLoadingEventMap.Clear();
	}
	#endregion

	private UnityEvent GetSceneEvent(string sceneName)
	{
		if (m_OnBeforeSceneLoadingEventMap.TryGetValue(sceneName, out UnityEvent unityEvent) == false)
			return null;

		return unityEvent;
	}
	public void OnBeforeSceneLoading(string sceneName)
	{
		UnityEvent unityEvent = GetSceneEvent(sceneName);

		unityEvent?.Invoke();
	}
}