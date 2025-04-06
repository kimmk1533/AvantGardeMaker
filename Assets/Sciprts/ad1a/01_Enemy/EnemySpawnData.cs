using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.ad1a
{
	//적 소환, 이동 관련 정보
	[System.Serializable]
	public class EnemySpawnData
	{
		//스폰시킬 적의 이름
		public string Name = string.Empty;

		//수량(일괄 스폰 시 사용)
		public int Amount = 1;
		//생성 간격(일괄 스폰 시 사용)
		public float Interval = 0f;
		//웨이브(특정 몹이 죽어야 진행될 경우 사용)
		public int Wave = 0;
		//웨이브 시간(같은 웨이브에서 스폰까지 걸리는 시간)
		public float WaveTime = 0f;
		//작전 시작 후 n초에 스폰(최초 스폰까지 걸리는 시간)
		public float Time = 0f;
		//최초 스폰 지점
		public Vector3 StartPos = Vector3.zero;
		//최종 도착 지점
		public Vector3 EndPos = Vector3.zero;
		//경유 지점
		public List<Vector3> TransitPos = null;
		//경유 지점에서 n초 대기(0초면 딜레이 x)
		public List<float> WaitTime = null;

		public EnemySpawnData()
		{
			Name = "DummyEnemy";

			Amount = 1;
			Interval = 0f;
			Wave = 0;
			WaveTime = 0f;
			Time = 0f;

			StartPos = EndPos = Vector3.zero;

			TransitPos = new List<Vector3>();
			WaitTime = new List<float>();
		}
	}
}