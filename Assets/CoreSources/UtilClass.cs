using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UtilClass
{
	public static void NullCheckGetComponent<T>(this Component com, ref T obj) where T : Component
	{
		if (obj == null)
		{
			obj = com.GetComponent<T>();

			if (obj == null)
				Debug.LogError("없는 컴포넌트를 GetComponent함");
		}
	}
	public static void NullCheckGetComponentInParent<T>(this Component com, ref T obj) where T : Component
	{
		if (obj == null)
		{
			obj = com.GetComponentInParent<T>();

			if (obj == null)
				Debug.LogError("없는 컴포넌트를 GetComponentInParent함");
		}
	}
	public static void NullCheckGetComponentInChilderen<T>(this Component com, ref T obj) where T : Component
	{
		if (obj == null)
		{
			obj = com.GetComponentInChildren<T>();

			if (obj == null)
				Debug.LogError("없는 컴포넌트를 GetComponentInChildren함");
		}
	}

	public static Vector2 GetMouseWorldPosition2D()
	{
		return GetMouseWorldPosition2D(Input.mousePosition, Camera.main);
	}
	public static Vector2 GetMouseWorldPosition2D(Camera worldCamera)
	{
		return GetMouseWorldPosition2D(Input.mousePosition, worldCamera);
	}
	public static Vector2 GetMouseWorldPosition2D(Vector3 screenPosition, Camera worldCamera)
	{
		Vector2 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
		return worldPosition;
	}
	public static Vector3 GetMouseWorldPosition3D()
	{
		return GetMouseWorldPosition3D(Input.mousePosition, Camera.main);
	}
	public static Vector3 GetMouseWorldPosition3D(Camera worldCamera)
	{
		return GetMouseWorldPosition3D(Input.mousePosition, worldCamera);
	}
	public static Vector3 GetMouseWorldPosition3D(Vector3 screenPosition, Camera worldCamera)
	{
		Ray ray = worldCamera.ScreenPointToRay(screenPosition);

		RaycastHit hit;
		Physics.Raycast(ray, out hit);

		return hit.point;
	}

	public static bool IsPointerOnUI()
	{
		return EventSystem.current.IsPointerOverGameObject();
	}

	public static TextMesh CreateWorldText(object text, Transform parent = null, Vector3 localPosition = default, float characterSize = 0.1f, Font font = null, int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.LowerLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000, float duration = -1f)
	{
		return CreateWorldText(text.ToString(), parent, localPosition, characterSize, font, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder, duration);
	}
	public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default, float characterSize = 0.1f, Font font = null, int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.LowerLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000, float duration = -1f)
	{
		if (color == null)
			color = Color.white;

		WorldTextOption option = new WorldTextOption()
		{
			localPosition = localPosition,
			characterSize = characterSize,
			font = font,
			fontSize = fontSize,
			color = color.Value,
			textAnchor = textAnchor,
			textAlignment = textAlignment,
			sortingOrder = sortingOrder,
			duration = duration
		};

		return CreateWorldText(parent, text, option);
	}
	public static TextMesh CreateWorldText(Transform parent, string text, WorldTextOption option)
	{
		GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));

		Transform transform = gameObject.transform;
		transform.SetParent(parent, false);
		transform.localPosition = option.localPosition;

		TextMesh textMesh = gameObject.GetComponent<TextMesh>();
		textMesh.anchor = option.textAnchor;
		textMesh.alignment = option.textAlignment;
		textMesh.text = text;
		textMesh.characterSize = option.characterSize;
		textMesh.font = option.font;
		textMesh.fontSize = option.fontSize;
		textMesh.color = option.color;
		textMesh.GetComponent<MeshRenderer>().sortingOrder = option.sortingOrder;
		if (option.duration >= 0f)
			GameObject.Destroy(gameObject, option.duration);

		return textMesh;
	}

	public static TextMeshPro CreateWorldText(object text, Transform parent = null, Vector3 localPosition = default, TMP_FontAsset font = null, int fontSize = 40, Color? color = null, TextAlignmentOptions textAlignment = TextAlignmentOptions.Left, int sortingOrder = 5000, float duration = -1f)
	{
		return CreateWorldText(text.ToString(), parent, localPosition, font, fontSize, color, textAlignment, sortingOrder, duration);
	}
	public static TextMeshPro CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default, TMP_FontAsset font = null, int fontSize = 40, Color? color = null, TextAlignmentOptions textAlignment = TextAlignmentOptions.Left, int sortingOrder = 5000, float duration = -1f)
	{
		if (color == null)
			color = Color.white;

		WorldTMP_TextOption option = new WorldTMP_TextOption()
		{
			localPosition = localPosition,
			tmpFont = font,
			fontSize = fontSize,
			color = color.Value,
			textAlignment = textAlignment,
			sortingOrder = sortingOrder,
			duration = duration,
		};

		return CreateWorldText(parent, text, option);
	}
	public static TextMeshPro CreateWorldText(Transform parent, string text, WorldTMP_TextOption option)
	{
		GameObject gameObject = new GameObject("World_TMP_Text", typeof(TextMeshPro));

		Transform transform = gameObject.transform;
		transform.SetParent(parent, false);
		transform.localPosition = option.localPosition;

		TextMeshPro textMesh = gameObject.GetComponent<TextMeshPro>();
		textMesh.alignment = option.textAlignment;
		textMesh.text = text;
		textMesh.font = option.tmpFont;
		textMesh.fontSize = option.fontSize;
		textMesh.color = option.color;
		textMesh.sortingOrder = option.sortingOrder;
		if (option.duration >= 0f)
			GameObject.Destroy(gameObject, option.duration);

		return textMesh;
	}

	public class WorldTextOption
	{
		public Vector3 localPosition = default;
		public float characterSize = 0.1f;
		public Font font = null;
		public int fontSize = 40;
		public Color color = Color.white;
		public TextAnchor textAnchor = TextAnchor.LowerLeft;
		public TextAlignment textAlignment = TextAlignment.Left;
		public int sortingOrder = 5000;
		public float duration = -1f;
	}
	public class WorldTMP_TextOption
	{
		public Vector3 localPosition = default;
		public TMP_FontAsset tmpFont = null;
		public int fontSize = 40;
		public Color color = Color.white;
		public TextAlignmentOptions textAlignment = TextAlignmentOptions.Left;
		public int sortingOrder = 5000;
		public float duration = -1f;
	}

	[System.Serializable]
	public class Timer
	{
		#region 변수
		[SerializeField]
		private float m_Interval = 0f;
		[SerializeField, ReadOnly]
		private float m_Time = 0f;

		private bool m_IsSimulating = true;
		#endregion

		#region 프로퍼티
		public float interval
		{
			get => m_Interval;
			set
			{
				m_Interval = value;
				if (m_Interval < m_Time)
					m_Time = m_Interval;
			}
		}
		public float time
		{
			get => m_Time;
			set => m_Time = value;
		}
		public bool isPaused => m_IsSimulating == false;
		#endregion

		#region 생성자
		public Timer()
		{
			m_Time = m_Interval = 0f;
			m_IsSimulating = true;
		}
		public Timer(float interval, bool filled = false)
		{
			m_Interval = interval;

			if (filled)
				m_Time = interval;
			else
				m_Time = 0f;

			m_IsSimulating = true;
		}
		#endregion

		/// <summary>
		/// 설정한 시간이 되었는 지 확인하는 함수
		/// </summary>
		/// <param name="autoClear">자동으로 다시 시작 여부</param>
		/// <returns>설정한 시간이 되었는 지</returns>
		public bool TimeCheck(bool autoClear = false)
		{
			if (m_Time >= m_Interval)
			{
				if (autoClear)
					Clear();

				return true;
			}

			return false;
		}

		/// <summary>
		/// 시간 경과
		/// </summary>
		public void Update()
		{
			Update(1f);
		}
		/// <summary>
		/// 시간 경과
		/// </summary>
		/// <param name="timeScale">시간 배율</param>
		public void Update(float timeScale)
		{
			if (m_IsSimulating == false)
				return;

			if (m_Time >= m_Interval)
				return;

			m_Time += Time.deltaTime * timeScale;
		}

		/// <summary>
		/// 시간 초기화
		/// </summary>
		public void Clear()
		{
			m_Time = 0f;
		}

		/// <summary>
		/// 일시정지
		/// </summary>
		public void Pause()
		{
			m_IsSimulating = false;
		}
		/// <summary>
		/// 다시 시작
		/// </summary>
		public void Resume()
		{
			m_IsSimulating = true;
		}
	}
}