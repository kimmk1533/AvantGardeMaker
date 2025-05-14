using System.Collections.Generic;
using AvantGardeMaker.CoreSpace.SaveLoad;
using AvantGardeMaker.EnemySpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using static AvantGardeMaker.EnemySpace.EnemySkill;

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


Time.deltaTime처럼 시간 재고 싶을 때
UtilClass.Timer라고 만들어놨음
new UtilClass.Timer(float interval)로 생성해주고
m_Interval이 설정한 시간
m_Time이 시작하고 지나간 시간
UtilClass.Timer.Update(float timeScale)하면 됨
TimeCheck(bool autoClear) 하면 new에서 설정한 시간이 지나면 true를 반환함
 */
namespace AvantGardeMaker.EnemySpace
{
	public class EnemyManager : ObjectManager<EnemyManager, Enemy>
	{
		#region 기본 템플릿
		#region 변수
		private const string c_EnemyDataPath = "Datas\\03_Enemy Datas";
		private const string c_EnemySkillDataPath = "Datas\\03_Enemy Datas\\EnemySkillDatas";
		private bool m_IsStageStart;

		//생성한 enemy 목록
		[SerializeField]
		private List<Enemy> m_EnemyList = null;

		//스크립터블 오브젝트 추가용
		[SerializeField]
		private Dictionary<string, EnemyData> m_EnemyDataMap = null;
		[SerializeField]
		private Queue<EnemySpawnData> m_EnemySpawnDataQueue = null;
		[SerializeField]
		private List<EnemySkillData> m_EnemySkillDataList = null;

		[SerializeField, ReadOnly]
		private UtilClass.Timer m_EnemySpawnTimer = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수
		private void Update()
		{
			SpawnEnemy();
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			m_EnemyList = new List<Enemy>();
			m_EnemyDataMap = new Dictionary<string, EnemyData>();
			m_EnemySpawnDataQueue = new Queue<EnemySpawnData>();

			m_EnemySkillDataList = new List<EnemySkillData>();

			m_EnemySpawnTimer = new UtilClass.Timer();

			LoadEnemyData();
			LoadEnemySkillData();
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();
		}

		/// <summary>
		/// 메인 초기화 함수 (본인 Main Scene 진입 시 호출)
		/// </summary>
		public override void InitializeMain()
		{
			base.InitializeMain();

			//스테이지에서 사용할 복사용 적을 1체씩 미리 완성시켜놓아야 함
			m_IsStageStart = true;
		}
		/// <summary>
		/// 메인 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();
			//init에서 만들어둔 복사용 적 삭제

			m_EnemySpawnDataQueue.Clear();

			int enemyCount = m_EnemyList.Count;
			for (int i = 0; i < enemyCount; ++i)
			{
				Despawn(m_EnemyList[i]);
			}
			m_EnemyList.Clear();

			m_EnemySpawnTimer.Clear();
			m_EnemySpawnTimer.interval = 0f;
		}
		#endregion
		#endregion

		#region Save & Load
		///<summary>
		/// Resources 폴더에 있는 EnemyData 스크립터블 오브젝트를 List에 저장
		/// </summary>
		[Button("Load EnemyData")]
		public void LoadEnemyData()
		{
			m_EnemyDataMap.Clear();

			EnemyData[] enemyDatas = Resources.LoadAll<EnemyData>(c_EnemyDataPath);

			for (int i = 0; i < enemyDatas.Length; ++i)
			{
				string key = enemyDatas[i].key;

				m_EnemyDataMap.Add(key, enemyDatas[i]);
			}
		}
		/// <summary>
		/// 스크립터블 데이터를 들고 있는 m_EnemyDataList에 stageData의 데이터를 덮어써 enemyData를 만듦
		/// </summary>
		public void LoadEnemyData(in StageData stageData)
		{
			List<EnemySpawnData> spawnDataList = stageData.enemySpawnDataList;
			List<EnemyFixedData> fixedDataList = stageData.enemyFixedDataList;
			List<EnemyVariableData> variableDataList = stageData.enemyVariableDataList;

			int count = spawnDataList.Count;

			for (int i = 0; i < count; ++i)
			{
				EnemySpawnData spawnData = spawnDataList[i];

				EnemyData enemyData = m_EnemyDataMap[spawnData.EnemySpawnKey];

				enemyData.FixedData = fixedDataList[i];
				enemyData.VariableData = variableDataList[i];
			}

			m_EnemySpawnDataQueue.Clear();
			m_EnemySpawnDataQueue.EnqueueRange(stageData.enemySpawnDataList);

			if (m_EnemySpawnDataQueue.Count != 0)
				m_EnemySpawnTimer.interval = m_EnemySpawnDataQueue.Peek().Time;
		}

		public void LoadEnemySkillData()
		{
			EnemySkillData[] enemySkillDatas = Resources.LoadAll<EnemySkillData>(c_EnemySkillDataPath);
			m_EnemySkillDataList.Clear();
			m_EnemySkillDataList.AddRange(enemySkillDatas);
		}
		#endregion

		private void SpawnEnemy()
		{
			if (!m_IsStageStart)
				return;

			if (m_EnemyList == null || m_EnemyDataMap == null || m_EnemySpawnDataQueue == null)
				return;

			if (m_EnemySpawnDataQueue.Count == 0)
				return;

			m_EnemySpawnTimer.Update();

			if (m_EnemySpawnTimer.TimeCheck() == false)
				return;

			EnemySpawnData enemySpawnData = m_EnemySpawnDataQueue.Peek();

			m_EnemySpawnTimer.interval = enemySpawnData.Time;

			if (m_EnemySpawnTimer.TimeCheck() == false)
				return;

			//스테이지 시작 시 n초가 경과했다면
			Enemy enemy = GetBuilder(enemySpawnData.EnemySpawnKey)
							.SetPosition(enemySpawnData.startPos)
							.SetAutoInit(false)
							.SetActive(true)
							.Spawn();

			enemy.SetEnemyData(m_EnemyDataMap[enemySpawnData.EnemySpawnKey]);
			enemy.state = E_EnemyState.Move;
			enemy.InitializePoolItem();
			enemy.SetRange(1.9f);
			enemy.SetWayPointList(enemySpawnData.WayPointList);
			enemy.SetWayPointIntervalList(enemySpawnData.DelayTimeList);

			//공격 범위 설정
			if (enemy.GetRange() > 0)
			{
				CircleCollider2D enemyCollider = enemy.GetComponent<CircleCollider2D>();
				enemyCollider.enabled = true;
				enemyCollider.radius = enemy.GetRange();
			}

			//히트박스 크기 설정
			switch (enemy.grade)
			{
				//기본 0.25
				default:
				case E_EnemyGradeType.Normal:
					break;
				case E_EnemyGradeType.Elite:
				{
					CircleCollider2D enemyCollider = enemy.GetComponent<CircleCollider2D>();
					enemyCollider.radius = 0.4f;
					break;
				}
				case E_EnemyGradeType.Leader:
				{
					CircleCollider2D enemyCollider = enemy.GetComponent<CircleCollider2D>();
					enemyCollider.radius = 0.5f;
					break;
				}
			}

			m_EnemyList.Add(enemy);
			m_EnemySpawnDataQueue.Dequeue();
		}

		public EnemyData GetEnemyData(string enName)
		{
			if (m_EnemyDataMap.TryGetValue(enName, out EnemyData enemyData) == false)
				return null;

			return enemyData;
		}

		public List<EnemyData> GetAllEnemyData()
		{
			return new List<EnemyData>(m_EnemyDataMap.Values);
		}
	}
}