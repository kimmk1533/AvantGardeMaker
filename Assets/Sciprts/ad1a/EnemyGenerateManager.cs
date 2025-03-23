using System.Collections;
using System.Collections.Generic;
using System.IO;
using AvantGardeMaker.ad1a.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker
{

	//적 스펙, 특성
	public class EnemyData
	{
		public E_EnemyGrade m_Grade;
		public int m_LossHp;    //보호 지점에 들어가면 깎이는 목표 HP

		public E_EnemyType m_TribeType;
		public E_EnemyFlyable m_Flyable;
		public E_EnemyAtkType m_AtkType;
		public E_EnemyDmgType m_DmgType;

		public string m_Name;   //이름
		public int m_Hp;        //체력
		public int m_Atk;       //공격력
		public float m_Def;     //방어력
		public float m_MagicRes;//마법 저항
		public float m_DmgRes;  //피해 감소
		public float m_MoveSpeed;//이동 속도(타일/s)
		public float m_AtkTime; //공격 간격(n초당 1회)
		public float m_Range;   //사정거리(근거리는 -1)
		public int m_MassLevel; //무게

		public bool[] m_Immune;//기절 수면 빙결 공중 전율 공포 면역여부
	}

	public class Point
	{
		public int x;
		public int y;
	}

	//적 소환, 이동 관련 정보
	public class EnemySpawnData
	{
		public string m_Name;
		public int m_Wave;      //웨이브(특정 몹이 죽어야 진행될 경우 사용)
		public int m_Amount;    //수량(일괄 스폰 시 사용) 
		public int m_Interval;  //생성 간격(일괄 스폰 시 사용)
		public int m_Time;      //작전 시작 후 n초에 스폰
		public Point m_StartPos;//최초 스폰 지점
		public List<Point> m_TargetPos; //목표 지점
		public List<int> m_WaitTime;    //목표 지점에서 n초 대기(0초면 딜레이 x)
	}

	//맵 정보 중 적 소환에 필요한 정보들
	public class StageData
	{
		public int m_Stage;
		public List<EnemyData> m_EnemyData;
		public List<EnemySpawnData> m_EnemySpawnData;
	}

	public class EnemyGenerateManager : SerializedSingleton<EnemyGenerateManager>
	{
		#region 변수
		private string m_FilePath;
		private StageData m_StageData;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수
		#endregion

		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public virtual void Initialize()
		{
			m_FilePath = Path.Combine(Application.persistentDataPath, "EnemyData.yaml");
			m_StageData = new StageData();
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public virtual void Finallize()
		{

		}

		/// <summary>
		/// 게임 초기화 함수 (Game Scene 진입 시 호출)
		/// </summary>
		public virtual void InitializeGame()
		{

		}
		/// <summary>
		/// 게임 마무리화 함수 (Game Scene 나갈 시 호출)
		/// </summary>
		public virtual void FinallizeGame()
		{

		}
	}
}