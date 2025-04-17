using System.Collections.Generic;
using AvantGardeMaker.ad1a.Enum;
using AvantGardeMaker.Ceeu;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
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


Time.deltaTime처럼 시간 재고 싶을 때
UtilClass.Timer라고 만들어놨음
new UtilClass.Timer(float interval)로 생성해주고
m_Interval이 설정한 시간
m_Time이 시작하고 지나간 시간
UtilClass.Timer.Update(float timeScale)하면 됨
TimeCheck(bool autoClear) 하면 new에서 설정한 시간이 지나면 true를 반환함
 */
namespace AvantGardeMaker.ad1a
{
	public class EnemyManager : ObjectManager<EnemyManager, Enemy>
	{
		#region 기본 템플릿
		#region 변수
		[SerializeField]
		private string m_ForderPath;

		private bool[,] m_TestMap;
		private bool m_IsStageStart;

		//생성한 enemy 목록
		[SerializeField]
		private List<Enemy> m_EnemyList = null;

		//스크립터블 오브젝트 추가용
		[SerializeField]
		private List<EnemyData> m_EnemyDataList = null;
		[SerializeField]
		private List<EnemySpawnData> m_EnemySpawnDataList = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수

		private void Start()
		{
			Initialize();
			InitializeMain();
		}

		private void Update()
		{
			if (!m_IsStageStart)
				return;

			if (m_EnemyList == null || m_EnemyDataList == null || m_EnemySpawnDataList == null)
				return;

			int spawnDataListCnt = m_EnemySpawnDataList.Count;

			//생성
			for (int i = 0; i < spawnDataListCnt; ++i)
			{
				if (true)//스테이지 시작 시 n초가 경과했다면
				{
					Enemy enemy = GetBuilder(m_EnemySpawnDataList[i].Name)
									.SetActive(true)
									.SetPosition(m_EnemySpawnDataList[i].startPos)
									.SetAutoInit(true)
									.Spawn();
					enemy.SetEnemyData(m_EnemyDataList.Find(n => n.EngName == m_EnemySpawnDataList[i].Name));
					enemy.SetRange(1.9f);
					enemy.SetTransitPosList(m_EnemySpawnDataList[i].TransitPosList);
					enemy.SetState(E_EnemyState.Move);

					//공격 범위 설정
					if (enemy.GetRange() > 0)
					{
						CircleCollider2D enemyCollider = enemy.GetComponent<CircleCollider2D>();
						enemyCollider.enabled = true;
						enemyCollider.radius = enemy.GetRange();
					}

					//히트박스 크기 설정
					switch (enemy.GetRank())
					{
						//기본 0.25
						default:
						case E_EnemyType.Normal:
							break;
						case E_EnemyType.Elite:
							{
								CircleCollider2D enemyCollider = enemy.GetComponent<CircleCollider2D>();
								enemyCollider.radius = 0.4f;
								break;
							}
						case E_EnemyType.Leader:
							{
								CircleCollider2D enemyCollider = enemy.GetComponent<CircleCollider2D>();
								enemyCollider.radius = 0.5f;
								break;
							}
					}

					m_EnemyList.Add(enemy);
					//list에서 제거했으니 i 감소, cnt 감소
					m_EnemySpawnDataList.RemoveAt(i--);
					--spawnDataListCnt;
					//Debug.Log("적 생성");
				}
			}
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			m_TestMap = new bool[7, 7]
			{ { true,true,true,true,true,true,true},
			 { true,true,true,true,true,true,true},
			 { true,true,true,true,true,true,true},
			 { true,true,true,true,true,true,true},
			 { true,true,true,true,true,true,true},
			 { true,true,true,true,true,true,true},
			 { true,true,true,true,true,true,true},};

			//스크립터블 오브젝트 경로
			m_ForderPath = "ad1a/Data/EnemyData";

			m_EnemyList = new List<Enemy>();
			m_EnemyDataList = new List<EnemyData>();
			m_EnemySpawnDataList = new List<EnemySpawnData>();

			//디버깅용//
			EnemySpawnData spawnData = new EnemySpawnData();

			spawnData.Name = "OriginiumSlug";
			spawnData.TransitPosList.Add(new Vector2(0, 0));
			spawnData.TransitPosList.Add(new Vector2(6, 6));
			spawnData.TransitPosList.Add(new Vector2(0, 0));
			spawnData.TransitPosList.Add(new Vector2(6, 6));

			m_EnemySpawnDataList.Add(spawnData);
			//디버깅용//

			LoadEnemyData();
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();
		}

		/// <summary>
		/// 게임 초기화 함수 (본인 Main Scene 진입 시 호출)
		/// </summary>
		public override void InitializeMain()
		{
			base.InitializeMain();

			//스테이지에서 사용할 복사용 적을 1체씩 미리 완성시켜놓아야 함
			m_IsStageStart = true;
		}
		/// <summary>
		/// 게임 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();
			//init에서 만들어둔 복사용 적 삭제

		}
		#endregion
		#endregion
		#region Save & Load
		[Button("Load EnemyData")]
		///<summary>
		/// Resources 폴더에 있는 EnemyData 스크립터블 오브젝트를 List에 저장
		/// </summary>
		public void LoadEnemyData()
		{
			m_EnemyDataList.Clear();
			m_EnemyDataList.AddRange(Resources.LoadAll<EnemyData>(m_ForderPath));
		}
		/// <summary>
		/// 스크립터블 데이터를 들고 있는 m_EnemyDataList에 stageData의 데이터를 덮어써 enemyData를 만듦
		/// </summary>
		public void LoadEnemyData(ref StageData stageData)
		{
			List<EnemyFixedData> fixedDataList = stageData.enemyFixedDataList;
			List<EnemyVariableData> variableDataList = stageData.enemyVariableDataList;

			int count = m_EnemyDataList.Count;

			if (count != fixedDataList.Count ||
				count != variableDataList.Count)
				throw new System.Exception("적 데이터 갯수 다름");

			for (int i = 0; i < count; ++i)
			{
				EnemyData enemyData = m_EnemyDataList[i];

				enemyData.FixedData = fixedDataList[i];
				enemyData.VariableData = variableDataList[i];
			}
		}
		public EnemyData GetEnemyData(string krName)
		{
			return m_EnemyDataList.Find(n => n.KorName == krName);
		}

		public List<EnemyData> GetAllEnemyData()
		{
			return m_EnemyDataList;
		}

		[Button]
		public void ClearEnemyDataList()
		{
			m_EnemyDataList.Clear();
		}
		#endregion

		/*///// <summary>
		///// 스테이지가 시작하면 Enemy에 관련된 코루틴을 실행시킴
		///// </summary>
		//public IEnumerator StartEnemyCoroutine()
		//{
		//	for (int i = 0; i < m_CurStageData.m_EnemySpawnDataList.Count; i++)//이번 스테이지에서 스폰할 적의 '무리' 수만큼 반복
		//	{
		//		StartCoroutine(GenerateEnemyGroup(m_CurStageData.m_EnemyDataList, m_CurStageData.m_EnemySpawnDataList));
		//	}
		//	yield return null;
		//}

		///// <summary>
		///// 적을 무리 단위로 스폰하는 코루틴을 실행시킴
		///// </summary>
		//public IEnumerator GenerateEnemyGroup(List<EnemyData> enemyData, List<EnemySpawnData> enemySpawnData)
		//{
		//	Debug.Log("GenerateEnemyGroup");
		//	EnemyData curEnemy;
		//	for (int i = 0; i < enemySpawnData.Count; i++)
		//	{
		//		yield return new WaitForSeconds(enemySpawnData[i].Time);
		//		curEnemy = enemyData.Find(n => n.EngName.Equals(enemySpawnData[i].Name));
		//		StartCoroutine(GenerateEnemy(curEnemy, enemySpawnData[i]));
		//	}
		//}

		///// <summary>
		///// 적을 오브젝트 풀에서 get해와 enemyData를 넣고 움직이는 코루틴을 실행시킴
		///// </summary>
		//public IEnumerator GenerateEnemy(EnemyData enemyData, EnemySpawnData enemySpawnData)
		//{
		//	Debug.Log("GenerateEnemy");
		//	enemySpawnData.StartPos = new Vector3(6, 0, 6);
		//	enemySpawnData.EndPos = new Vector3(0, 0, 6);
		//	for (int i = 0; i < enemySpawnData.Amount; i++)
		//	{
		//		if (i != 0)
		//			yield return new WaitForSeconds(enemySpawnData.Interval);

		//		Enemy newEnemy = GetBuilder(enemySpawnData.Name)  //이때 실제 적 오브젝트가 생성됨
		//			.SetActive(true)
		//			.SetPosition(enemySpawnData.StartPos)
		//			.SetAutoInit(true)
		//			.Spawn();

		//		newEnemy.m_EnemyData = enemyData;
		//		newEnemy.m_CurPos = enemySpawnData.StartPos;

		//		m_EnemyList.Add(newEnemy);

		//		StartCoroutine(EnemyMove(newEnemy, enemySpawnData));
		//	}
		//}

		///// <summary>
		///// 시작 지점에 나타나 목표 지점 List가 빌 때까지 이동과 대기(0초 가능) 반복
		///// </summary>
		//public IEnumerator EnemyMove(Enemy enemy, EnemySpawnData enemySpawnData)
		//{
		//	//경유 지점 추가
		//	List<Vector3> path = PathFinder.FindPath(enemySpawnData.StartPos, enemySpawnData.EndPos, m_TestMap);
		//	//path[0]은 시작지점임
		//	for (int i = 1; i < path.Count; i++)
		//		enemySpawnData.TransitPos.Add(path[i]);

		//	for (int i = 0; i < enemySpawnData.TransitPos.Count; i++)
		//	{
		//		enemy.m_TargetPos = enemySpawnData.TransitPos[i];
		//		enemy.m_CurEnemyState = E_EnemyState.Move;
		//		yield return new WaitUntil(() => enemy.m_CurEnemyState != E_EnemyState.Move);
		//	}
		//	yield break;
		//}*/
	}
}