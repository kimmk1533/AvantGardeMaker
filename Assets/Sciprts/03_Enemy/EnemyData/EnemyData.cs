using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.EnemySpace
{
	[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Object/EnemyData", order = int.MinValue)]
	//적 스펙, 특성
	public class EnemyData : SerializedScriptableObject
	{
		#region 변수
		//이름(키 값임)
		public string EngName = string.Empty;
		public string KorName = string.Empty;

		//적 초상화 경로
		public string PortraitImagePath = string.Empty;

		//고정된 값
		public EnemyFixedData FixedData = new EnemyFixedData();
		//전투중 바뀔 수 있는 값
		public EnemyVariableData VariableData = new EnemyVariableData();
		#endregion

		#region 프로퍼티
		public string key => EngName;
		#endregion
	}

}