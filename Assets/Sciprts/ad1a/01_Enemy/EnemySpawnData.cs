using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.ad1a
{
	//적 소환, 이동 관련 정보
	public class EnemySpawnData
	{
		public string m_Name;   //스폰시킬 적의 이름
		public int m_Wave;      //웨이브(특정 몹이 죽어야 진행될 경우 사용)
		public int m_Amount;    //수량(일괄 스폰 시 사용) 
		public float m_Interval;  //생성 간격(일괄 스폰 시 사용)
		public float m_Time;      //작전 시작 후 n초에 스폰(최초 스폰까지 걸리는 시간)
		public Vector3 m_StartPos;//최초 스폰 지점
		public Vector3 m_EndPos; //최종 도착 지점
		public List<Vector3> m_TransitPos; //경유 지점
		public List<float> m_WaitTime;    //경유 지점에서 n초 대기(0초면 딜레이 x)

		public EnemySpawnData()
		{
			m_Name = "DummyEnemy";
			m_Wave = 0;
			m_Amount = 1;
			m_Interval = 0;
			m_Time = 0;
		}
	}
}