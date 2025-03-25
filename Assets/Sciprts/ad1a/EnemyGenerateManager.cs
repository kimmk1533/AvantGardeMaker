using System.Collections;
using System.Collections.Generic;
using System.IO;
using AvantGardeMaker.ad1a.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.ad1a
{
	public struct CombatStatValue<T>
	{
		public T m_InitStat;
		public T m_CurStat;
	}

	//적 스펙, 특성
	public class EnemyData
	{
		public E_EnemyGrade m_Grade;
		public int m_LossHp;    //보호 지점에 들어가면 깎이는 목표 HP

		public E_EnemyType m_TribeType;
		public E_EnemyFlyable m_Flyable;
		public E_EnemyAtkType m_AtkType;
		public E_EnemyDmgType m_DmgType;

		public CombatStatValue<string> m_Name;   //이름
		public CombatStatValue<int> m_Hp;        //체력
		public CombatStatValue<int> m_Atk;       //공격력
		public CombatStatValue<float> m_Def;     //방어력
		public CombatStatValue<float> m_MagicRes;//마법 저항
		public CombatStatValue<float> m_DmgRes;  //피해 감소
		public CombatStatValue<float> m_MoveSpeed;//이동 속도(타일/s)
		public CombatStatValue<float> m_AtkTime; //공격 간격(n초당 1회)
		public CombatStatValue<float> m_Range;   //사정거리(근거리는 -1)
		public CombatStatValue<int> m_MassLevel; //무게

		public bool[] m_Immune;//기절 수면 빙결 공중 전율 공포 면역여부
	}

	//적 소환, 이동 관련 정보
	public class EnemySpawnData
	{
		public string m_Name;   //스폰시킬 적의 이름
		public int m_Wave;      //웨이브(특정 몹이 죽어야 진행될 경우 사용)
		public int m_Amount;    //수량(일괄 스폰 시 사용) 
		public int m_Interval;  //생성 간격(일괄 스폰 시 사용)
		public int m_Time;      //작전 시작 후 n초에 스폰
		public Vector2Int m_StartPos;//최초 스폰 지점
		public List<Vector2Int> m_TargetPos; //목표 지점
		public List<int> m_WaitTime;    //목표 지점에서 n초 대기(0초면 딜레이 x)
	}

	//맵 정보 중 적 소환에 필요한 정보들
	public class StageData
	{
		public string m_Stage;      //스테이지 이름(검색 키 값)
		public List<EnemyData> m_EnemyData;
		public List<EnemySpawnData> m_EnemySpawnData;

		public StageData()
		{
			m_EnemyData = new List<EnemyData>();
			m_EnemySpawnData = new List<EnemySpawnData>();
		}
	}

	public class Enemy
	{
		EnemyData m_EnemyData;

		public Vector2 m_CurPos;
		public Vector2Int m_TargetPos;
	}

	public class EnemyGenerateManager : SerializedSingleton<EnemyGenerateManager>
	{
		#region 기본 템플릿
		#region 변수
		private string m_FilePath;
		private StageData m_CurStageData;
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
			m_CurStageData = new StageData();
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
		#endregion
		public IEnumerator GenerateEnemy()
		{
			if (m_CurStageData == null ||
				m_CurStageData.m_EnemyData.Count == 0 ||
				m_CurStageData.m_EnemySpawnData.Count == 0)//현재 스테이지 정보가 비어있다면 즉시 종료
				yield break;

			for (int i = 0; i < m_CurStageData.m_EnemySpawnData.Count; i++)//이번 스테이지에서 스폰할 적의 '무리' 수만큼 반복
			{
				StartCoroutine(EnemyMove(m_CurStageData.m_EnemyData, m_CurStageData.m_EnemySpawnData));
			}
		}

		public IEnumerator EnemyMove(List<EnemyData> enemyData, List<EnemySpawnData> enemySpawnData)
		{
			yield return null;
		}
	}
}