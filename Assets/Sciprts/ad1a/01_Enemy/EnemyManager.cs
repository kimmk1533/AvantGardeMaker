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
	public class EnemyManager : ObjectManager<EnemyManager, Enemy>
	{
		#region 기본 템플릿
		#region 변수
		private string m_FilePath;
		private StageData m_CurStageData;

		private bool[,] m_TestMap;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수
		//private void Start()
		//{
		//	Initialize();
		//	InitializeGame();
		//}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			m_TestMap = new bool[7, 7]
			{ { true,true,true,false,true,true,true},
			  { true,false,true,false,true,false,true},
			  { true,false,true,false,true,false,true},
			  { true,false,true,false,true,false,true},
			  { true,false,true,false,true,false,true},
			  { true,false,true,false,true,false,true},
			  { true,false,true,true,true,false,true},};

			m_FilePath = Path.Combine(Application.persistentDataPath, "EnemyData.yaml");
			m_CurStageData = new StageData();

			//전부 기본값을 가진 DummyEnemy 1개를 소환하는 DummyStage
			m_CurStageData.m_Stage = "DummyStage";

			EnemyData enemyData = new EnemyData();
			m_CurStageData.m_EnemyData.Add(enemyData);

			EnemySpawnData enemySpawnData = new EnemySpawnData();
			//enemySpawnData.m_Amount = 3;
			m_CurStageData.m_EnemySpawnData.Add(enemySpawnData);
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

			StartCoroutine(StartEnemyCoroutine());
			//스테이지에서 사용할 복사용 적을 1체씩 미리 완성시켜놓아야 함
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
		/// <summary>
		/// 스테이지가 시작하면 Enemy에 관련된 코루틴을 실행시킴
		/// </summary>
		public IEnumerator StartEnemyCoroutine()
		{
			Debug.Log("EnemyGenerator.StartEnemyCoroutine Start");
			if (m_CurStageData == null)//Init이 실행되지 않았다면 즉시 종료
			{
				Debug.LogError("Init doesn't run(CurStageData == null)");
				yield break;
			}
			if (m_CurStageData.m_EnemyData.Count == 0 ||
			m_CurStageData.m_EnemySpawnData.Count == 0)//현재 스테이지 정보가 비어있다면 즉시 종료
			{
				Debug.LogError("StageData is Empty(data.Count == 0)");
				yield break;
			}

			for (int i = 0; i < m_CurStageData.m_EnemySpawnData.Count; i++)//이번 스테이지에서 스폰할 적의 '무리' 수만큼 반복
			{
				StartCoroutine(GenerateEnemyGroup(m_CurStageData.m_EnemyData, m_CurStageData.m_EnemySpawnData));
			}
			Debug.Log("EnemyGenerator.StartEnemyCoroutine Done");
		}

		/// <summary>
		/// 적을 무리 단위로 스폰하는 코루틴을 실행시킴
		/// </summary>
		public IEnumerator GenerateEnemyGroup(List<EnemyData> enemyData, List<EnemySpawnData> enemySpawnData)
		{
			Debug.Log("GenerateEnemyGroup");
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
			Debug.Log("GenerateEnemy");
			enemySpawnData.m_StartPos = new Vector3(6, 0, 6);
			enemySpawnData.m_EndPos = new Vector3(0, 0, 6);
			for (int i = 0; i < enemySpawnData.m_Amount; i++)
			{
				if (i != 0)
					yield return new WaitForSeconds(enemySpawnData.m_Interval);

				Enemy newEnemy = GetBuilder(enemySpawnData.m_Name)  //이때 실제 적 오브젝트가 생성됨
					.SetActive(true)
					.SetPosition(enemySpawnData.m_StartPos)
					.Spawn();

				newEnemy.m_EnemyData = enemyData;
				newEnemy.m_CurPos = enemySpawnData.m_StartPos;

				StartCoroutine(EnemyMove(newEnemy, enemySpawnData));
			}
		}

		/// <summary>
		/// 시작 지점에 나타나 목표 지점 List가 빌 때까지 이동과 대기(0초 가능) 반복
		/// </summary>
		public IEnumerator EnemyMove(Enemy enemy, EnemySpawnData enemySpawnData)
		{
			Debug.Log("EnemyMove");
			Debug.Log("CurPos: [" + enemy.m_CurPos.x + ", " + enemy.m_CurPos.z + "]");
			Debug.Log("TargetPos: [" + enemySpawnData.m_EndPos.x + ", " + enemySpawnData.m_EndPos.z + "]");
			enemySpawnData.m_TransitPos = PathFinder.FindPath(enemySpawnData.m_StartPos, enemySpawnData.m_EndPos, m_TestMap);
			for (int i = 0; i < enemySpawnData.m_TransitPos.Count; i++)
			{
				enemy.m_TargetPos = enemySpawnData.m_TransitPos[i];
				enemy.m_EnemyState = E_EnemyState.Move;
				yield return new WaitUntil(() => enemy.m_EnemyState != E_EnemyState.Move);
			}
			yield break;
		}
	}
}