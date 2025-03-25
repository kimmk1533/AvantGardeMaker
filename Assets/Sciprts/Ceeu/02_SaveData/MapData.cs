using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using AvantGardeMaker.ad1a;

namespace AvantGardeMaker.Ceeu
{
	[System.Serializable]
	public struct MapData
	{
		#region 변수
		[SerializeField]
		private int m_MapWidth;
		[SerializeField]
		private int m_MapHeight;
		#endregion

		#region 프로퍼티
		public int mapWidth
		{
			get => m_MapWidth;
			set => m_MapWidth = value;
		}
		public int mapHeight
		{
			get => m_MapHeight;
			set => m_MapHeight = value;
		}
		#endregion
	}
}