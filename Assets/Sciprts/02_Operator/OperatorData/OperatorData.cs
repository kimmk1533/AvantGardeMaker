using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	[CreateAssetMenu(fileName = "OperatorInfo", menuName = "Scriptable Object/OperatorInfo", order = int.MinValue)]
	public class OperatorData : SerializedScriptableObject
	{
		//영어이름
		public string EngName;
		//한글이름
		public string KorName;

		public OperatorFixedData FixedData = new OperatorFixedData();
		public OperatorVariableData VariableData = new OperatorVariableData();

	}
}