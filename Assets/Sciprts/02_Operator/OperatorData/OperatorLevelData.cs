using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	[System.Serializable]
	public struct OperatorLevelData<T>
	{
		/// <summary>
		/// 0정 1레벨 기준 데이터
		/// </summary>
		public T Elite0Level1;
		/// <summary>
		/// 1정 1레벨(0정 만렙) 기준 데이터
		/// </summary>
		public T Elite1Level1;
		/// <summary>
		/// 2정 1레벨(1정 만렙) 기준 데이터
		/// </summary>
		public T Elite2Level1;
		/// <summary>
		/// 2정 만렙 기준 데이터
		/// </summary>
		public T Elite2LevelFull;

		public T GetLevelData(int elite)
		{
			switch (elite)
			{
				case 0:
					return Elite0Level1;
				case 1:
					return Elite1Level1;
				case 2:
					return Elite2Level1;
				case 3:
					return Elite2LevelFull;
			}

			return default;
		}
	}
}