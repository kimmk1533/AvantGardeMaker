using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.EnemySpace
{
	// 적 스펙, 특성
	[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Object/EnemyData", order = int.MinValue)]
	public class EnemyData : SerializedScriptableObject
	{
		#region 변수
		// 이름(키 값임)
		public string EngName = string.Empty;
		public string KorName = string.Empty;

		// 고정된 값
		public EnemyFixedData FixedData = new EnemyFixedData();
		// 전투중 바뀔 수 있는 값
		public EnemyVariableData VariableData = new EnemyVariableData();
		#endregion

		#region 프로퍼티
		public string key => FixedData.Code;
		#endregion

		public EnemyData Clone()
		{
			EnemyData enemyData = CreateInstance<EnemyData>();

			enemyData.EngName = EngName;
			enemyData.KorName = KorName;

			enemyData.FixedData = FixedData;
			enemyData.VariableData = VariableData;

			return enemyData;
		}
	}

}