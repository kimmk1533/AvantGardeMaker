using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.ad1a
{
	//적 소환, 이동 관련 정보
	public class EnemySpawnData
	{
		//스폰시킬 적의 이름
		public string Name { get; set; }
		//웨이브(특정 몹이 죽어야 진행될 경우 사용)
		public int Wave { get; set; }
		//수량(일괄 스폰 시 사용) 
		public int Amount { get; set; }
		//생성 간격(일괄 스폰 시 사용)
		public float Interval { get; set; }
		//작전 시작 후 n초에 스폰(최초 스폰까지 걸리는 시간)
		public float Time { get; set; }
		//최초 스폰 지점
		public Vector3 StartPos { get; set; }
		//최종 도착 지점
		public Vector3 EndPos { get; set; }
		//경유 지점
		public List<Vector3> TransitPos { get; }
		//경유 지점에서 n초 대기(0초면 딜레이 x)
		public List<float> WaitTime { get; }

		public EnemySpawnData()
		{
			Name = "DummyEnemy";
			Wave = 0;
			Amount = 1;
			Interval = 0;
			Time = 0;

			StartPos = EndPos = Vector3.zero;
		}
	}
}