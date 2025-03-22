using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public abstract class SceneReceiver : SerializedMonoBehaviour
{
	private void Awake()
	{
		Initialize();
	}
	private void OnDestroy()
	{
		Finallize();
	}
	protected virtual void Initialize()
	{
		SceneManager.UnloadSceneAsync("Loading Scene");
	}
	protected virtual void Finallize()
	{

	}
}