using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.MikangMark
{
	/// <summary>
	/// 정예화 또는 레벨로 인해 변하는 스탯 데이터
	/// </summary>
	/// <typeparam name="T">데이터 타입</typeparam>
	[System.Serializable]
	// ex. 최대 체력, 공격력, 방어력, 배치 코스트, 저지 가능 수
	public class OperatorStatData<T>
	{
		// 0정 1렙 스탯
		public T Level0Value;
		// 1정 1렙 스탯
		public T Level1Value;
		// 2정 1렙 스탯
		public T Level2Value;
		// 2정 만렙 스탯
		public T Level3Value;
	}
}