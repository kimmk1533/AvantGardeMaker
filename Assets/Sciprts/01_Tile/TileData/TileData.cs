using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.TileSpace
{
	[CreateAssetMenu(fileName = "TileData", menuName = "Scriptable Object/TileData", order = int.MinValue)]
	public class TileData : SerializedScriptableObject
	{
		#region 변수
		public string EngName = string.Empty;
		public string KorName = string.Empty;

		public TileFixedData FixedData = new TileFixedData();
		public TileVariableData VariableData = new TileVariableData();
		#endregion

		#region 프로퍼티
		public string key => EngName;
		#endregion
	}
}