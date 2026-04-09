using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	[CreateAssetMenu(fileName = "OperatorData", menuName = "Scriptable Object/OperatorData", order = int.MinValue)]
	public class OperatorData : SerializedScriptableObject
	{
		#region 변수
		public OperatorFixedData FixedData = new OperatorFixedData();
		public OperatorVariableData VariableData = new OperatorVariableData();
		#endregion

		#region 프로퍼티
		public string key => FixedData.EngName;
		#endregion

		public OperatorData Clone()
		{
			OperatorData operatorData = CreateInstance<OperatorData>();

			operatorData.FixedData = FixedData;
			operatorData.VariableData = VariableData;

			return operatorData;
		}
	}
}