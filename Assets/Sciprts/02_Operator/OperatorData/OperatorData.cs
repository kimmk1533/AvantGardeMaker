using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	[CreateAssetMenu(fileName = "OperatorInfo", menuName = "Scriptable Object/OperatorInfo", order = int.MinValue)]
	public class OperatorData : SerializedScriptableObject
	{
		#region 변수
		//영어이름
		public string EngName;
		//한글이름
		public string KorName;

		public OperatorFixedData FixedData = new OperatorFixedData();
		public OperatorVariableData VariableData = new OperatorVariableData();
		#endregion

		#region 프로퍼티
		public string key => EngName;
		#endregion
	}
}