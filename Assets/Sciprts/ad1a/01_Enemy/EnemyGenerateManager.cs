using System.Collections;
using System.Collections.Generic;
using System.IO;
using AvantGardeMaker.ad1a.Enum;
using Sirenix.OdinInspector;
using UnityEngine;


/*
 * 옵젝 매니저(풀링 되어있음)
ObjectManager<자신, 복사할_스크립트)
ObjectPoolItemBase

복사할_스크립트 = 매니저.GetBuilder(key 값)
SetAutoInit(true) 안해두면 따로 init안해주면 고장남

다쓰면
매니저.Despawn

오브젝트 풀 안에
public class ItemBuilder : ObjectPool<복사할_스크립트>.ItemBuilder
 */
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

		public string m_Name;   //이름
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
		public float m_Interval;  //생성 간격(일괄 스폰 시 사용)
		public float m_Time;      //작전 시작 후 n초에 스폰(최초 스폰까지 걸리는 시간)
		public Vector2Int m_StartPos;//최초 스폰 지점
		public List<Vector2Int> m_TargetPos; //목표 지점
		public List<float> m_WaitTime;    //목표 지점에서 n초 대기(0초면 딜레이 x)
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

	public class EnemyGenerateManager : ObjectManager<EnemyGenerateManager, Enemy>
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
		public override void Initialize()
		{
			base.Initialize();
			m_FilePath = Path.Combine(Application.persistentDataPath, "EnemyData.yaml");
			m_CurStageData = new StageData();
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();
		}

		/// <summary>
		/// 게임 초기화 함수 (Game Scene 진입 시 호출)
		/// </summary>
		public override void InitializeGame()
		{
			base.InitializeGame();
			//스테이지에서 사용할 복사용 적을 1체씩 미리 완성시켜놓아야 함
		}
		/// <summary>
		/// 게임 마무리화 함수 (Game Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeGame()
		{
			base.FinallizeGame();
			//init에서 만들어둔 복사용 적 삭제
		}
		#endregion
		/// <summary>
		/// 스테이지가 시작하면 Enemy에 관련된 코루틴을 실행시킴
		/// </summary>
		public IEnumerator StartEnemyCoroutine()
		{
			if (m_CurStageData == null ||
				m_CurStageData.m_EnemyData.Count == 0 ||
				m_CurStageData.m_EnemySpawnData.Count == 0)//현재 스테이지 정보가 비어있다면 즉시 종료
				yield break;

			for (int i = 0; i < m_CurStageData.m_EnemySpawnData.Count; i++)//이번 스테이지에서 스폰할 적의 '무리' 수만큼 반복
			{
				StartCoroutine(GenerateEnemyGroup(m_CurStageData.m_EnemyData, m_CurStageData.m_EnemySpawnData));
			}
		}

		/// <summary>
		/// 적을 무리 단위로 스폰하는 코루틴을 실행시킴
		/// </summary>
		public IEnumerator GenerateEnemyGroup(List<EnemyData> enemyData, List<EnemySpawnData> enemySpawnData)
		{
			EnemyData curEnemy;
			for (int i = 0; i < enemySpawnData.Count; i++)
			{
				yield return new WaitForSeconds(enemySpawnData[i].m_Time);
				curEnemy = enemyData.Find(n => n.m_Name.Equals(enemySpawnData[i].m_Name));
				StartCoroutine(GenerateEnemy(curEnemy, enemySpawnData[i]));
			}
		}

		/// <summary>
		/// 적을 오브젝트 풀에서 get해와 enemyData를 넣고 움직이는 코루틴을 실행시킴
		/// </summary>
		public IEnumerator GenerateEnemy(EnemyData enemyData, EnemySpawnData enemySpawnData)
		{
			for (int i = 0; i < enemySpawnData.m_Amount; i++)
			{
				if (i != 0)
					yield return new WaitForSeconds(enemySpawnData.m_Interval);

				Enemy newEnemy = GetBuilder(enemySpawnData.m_Name)
					.SetActive(true)
					.Spawn();
				newEnemy.m_EnemyData = enemyData;
				StartCoroutine(EnemyMove(newEnemy));
			}
		}

		/// <summary>
		/// 시작 지점에 나타나 목표 지점 List가 빌 때까지 이동과 대기(0초 가능) 반복
		/// </summary>
		public IEnumerator EnemyMove(Enemy enemy)
		{
			//

			yield break;
		}
	}
}