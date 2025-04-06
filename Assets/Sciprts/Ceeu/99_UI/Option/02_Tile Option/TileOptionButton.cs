using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.Ceeu.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class TileOptionButton : SerializedMonoBehaviour
	{
		#region 변수
		private Button m_Button = null;

		[SerializeField]
		private E_TileType m_TileType = E_TileType.LowGroundTile;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트

		#region 이벤트 함수
		public void OnButtonClicked()
		{
			M_MapEditor.SetTileType(m_TileType);
		}
		#endregion
		#endregion

		#region 매니저
		private static MapEditorManager M_MapEditor => MapEditorManager.Instance;
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
		public void Initialize()
		{
			m_Button = GetComponent<Button>();

			m_Button.onClick.AddListener(OnButtonClicked);
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{
			m_Button.onClick.RemoveListener(OnButtonClicked);

			m_Button = null;
		}
		#endregion
	}
}