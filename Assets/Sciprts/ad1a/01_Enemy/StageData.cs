using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.ad1a
{
	//맵 정보 중 적 소환에 필요한 정보들
	public class StageData
	{
		public string m_Stage;      //스테이지 이름(검색 키 값)
		public List<EnemyData> m_EnemyDataList;
		public List<EnemySpawnData> m_EnemySpawnDataList;

		public StageData()
		{
			m_EnemyDataList = new List<EnemyData>();
			m_EnemySpawnDataList = new List<EnemySpawnData>();
		}
	}
}