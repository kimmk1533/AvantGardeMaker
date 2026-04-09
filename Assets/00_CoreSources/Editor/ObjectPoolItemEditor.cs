using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ObjectPoolItemEditor
{
	[MenuItem("Assets/Create/Object Pool Item Script", false, -230)]
	private static void CreateCustomScript()
	{
		string unityEditorPath = Path.GetDirectoryName(EditorApplication.applicationPath);

#if UNITY_6000_0_OR_NEWER
		string path = Path.Combine(unityEditorPath, "Data", "Resources", "ScriptTemplates", "My Templates", "1-Scripting__Object Pool Item Script-NewObjectPoolItemScript.cs.txt");
#else
		string path = Path.Combine(unityEditorPath, "Data", "Resources", "ScriptTemplates", "81-C# Script-NewBehaviourScript.cs.txt");
#endif

		if (File.Exists(path) == false)
		{
			Debug.LogError(path + " File does not exist.");
			return;
		}

		ProjectWindowUtil.CreateScriptAssetFromTemplateFile(path, "NewObjectPoolItemScript.cs");
	}
}