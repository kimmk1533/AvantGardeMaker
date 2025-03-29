using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public abstract class SceneEventReceiver : SerializedMonoBehaviour
{
	#region 변수
	[SerializeField]
	[DictionaryDrawerSettings(KeyLabel = "이전 씬 이름", ValueLabel = "씬 로딩 후 호출할 이벤트")]
	protected Dictionary<string, UnityEvent> m_OnAfterSceneLoadingEventMap = new Dictionary<string, UnityEvent>();
	#endregion

	#region 유니티 콜백 함수
	private void Awake()
	{
		Initialize();
	}
	private void OnApplicationQuit()
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
		if (m_OnAfterSceneLoadingEventMap == null)
			m_OnAfterSceneLoadingEventMap = new Dictionary<string, UnityEvent>();
	}
	/// <summary>
	/// 마무리화 함수
	/// </summary>
	protected virtual void Finallize()
	{
		if (m_OnAfterSceneLoadingEventMap != null)
			m_OnAfterSceneLoadingEventMap.Clear();
	}
	#endregion

	private UnityEvent GetSceneEvent(string sceneName)
	{
		if (m_OnAfterSceneLoadingEventMap.TryGetValue(sceneName, out UnityEvent unityEvent) == false)
			return null;

		return unityEvent;
	}
	public void OnAfterSceneLoaded(string sceneName)
	{
		UnityEvent unityEvent = GetSceneEvent(sceneName);

		unityEvent?.Invoke();
	}
}