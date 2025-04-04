using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

		//생성한 enemy 목록
		private List<Enemy> m_EnemyList;

		//스크립터블 오브젝트 추가용
		public List<EnemyData> m_EnemyDataList;

		//enemy별 <이름, 정보> 딕셔너리(저장, 불러오기 용)
		[SerializeField]
		private Dictionary<string, EnemyData> m_EnemyDataDictionary = null;
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

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.S))
			{
				SaveEnemyData();
				Debug.Log("데이터 저장 완료");
			}

			//죽은 적을 오브젝트 풀에 반환
			for (int i = 0; i < m_EnemyList.Count; i++)
			{
				if (m_EnemyList[i].m_EnemyData.VariableData.Hp.CurStat <= 0.0f)
				{
					Despawn(m_EnemyList[i]);
					m_EnemyList.RemoveAt(i);
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

			if (m_EnemyDataDictionary == null)
			{
				m_EnemyDataDictionary = new Dictionary<string, EnemyData>();
			}

			m_TestMap = new bool[7, 7]
			{ { true,true,true,false,true,true,true},
			  { true,false,true,false,true,false,true},
			  { true,false,true,false,true,false,true},
			  { true,false,true,false,true,false,true},
			  { true,false,true,false,true,false,true},
			  { true,false,true,false,true,false,true},
			  { true,false,true,true,true,false,true},};

			m_FilePath = Path.Combine(Application.dataPath, "..", "Data", "EnemyData.json");
			m_CurStageData = new StageData();

			//전부 기본값을 가진 DummyEnemy 1개를 소환하는 DummyStage
			m_CurStageData.m_Stage = "DummyStage";

			EnemyData enemyData = new EnemyData();
			m_CurStageData.m_EnemyDataList.Add(enemyData);

			EnemySpawnData enemySpawnData = new EnemySpawnData();
			m_CurStageData.m_EnemySpawnDataList.Add(enemySpawnData);

			m_EnemyList = new List<Enemy>();

			m_EnemyDataDictionary = new Dictionary<string, EnemyData>()
			{
				{"111111",null},
				{"222222",enemyData },
			};
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
			//StartCoroutine(StartEnemyCoroutine());
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

		#region Json
		[System.Serializable]
		public class JsonEnemyDataList
		{
			public EnemyData[] m_EnemyDataList = null;
			public JsonEnemyDataList(int count) { m_EnemyDataList = new EnemyData[count]; }
		}

		[Button("Save EnemyData")]
		///<summary>
		///EnemyDataDictionary에 있는 EnemyData를 json으로 저장
		/// </summary>
		public void SaveEnemyData()
		{
			JsonEnemyDataList enemyDataList = new JsonEnemyDataList(m_EnemyDataDictionary.Count);
			int index = 0;
			foreach (var item in m_EnemyDataDictionary)
			{
				enemyDataList.m_EnemyDataList[index++] = item.Value;
			}
			File.WriteAllText(m_FilePath, JsonUtility.ToJson(enemyDataList, true));
		}

		[Button("Load EnemyData")]
		///<summary>
		///json 파일에 있는 EnemyData를 저장
		/// </summary>
		public void LoadEnemyData()
		{
			string json = File.ReadAllText(m_FilePath);
			JsonEnemyDataList dataList = JsonUtility.FromJson<JsonEnemyDataList>(json);

			m_EnemyDataDictionary.Clear();
			for (int i = 0; i < dataList.m_EnemyDataList.Length; i++)
			{
				EnemyData enemyData = dataList.m_EnemyDataList[i];
				m_EnemyDataDictionary.Add(enemyData.Name, enemyData);
			}
		}
		#endregion
		public EnemyData GetEnemyData(string name)
		{
			if (m_EnemyDataDictionary.TryGetValue(name, out EnemyData enemyData) == false)
			{
				Debug.LogError("Enemy name " + name + " not found.");
				return null;
			}
			return m_EnemyDataDictionary[name];
		}

		[Button]
		public void AddScriptable2Dictionary()
		{
			if (m_EnemyDataList == null)
			{
				Debug.LogError("m_EnemyDataList is null");
				return;
			}

			if(m_EnemyDataDictionary == null)
				m_EnemyDataDictionary = new Dictionary<string, EnemyData>();

			for (int i = 0; i < m_EnemyDataList.Count; i++)
			{
				if(!m_EnemyDataDictionary.TryAdd(m_EnemyDataList[i].Name, m_EnemyDataList[i]))
				{
					Debug.LogError("Scriptable to Dictionary Failed");
					return;
				}
			}
		}

		[Button]
		public void ClearEnemyDictionary()
		{
			m_EnemyDataDictionary.Clear();
		}

		/// <summary>
		/// 스테이지가 시작하면 Enemy에 관련된 코루틴을 실행시킴
		/// </summary>
		public IEnumerator StartEnemyCoroutine()
		{
			if (m_CurStageData == null)//Init이 실행되지 않았다면 즉시 종료
			{
				Debug.LogError("Init doesn't run(CurStageData == null)");
				yield break;
			}
			if (m_CurStageData.m_EnemyDataList.Count == 0 ||
			m_CurStageData.m_EnemySpawnDataList.Count == 0)//현재 스테이지 정보가 비어있다면 즉시 종료
			{
				Debug.LogError("StageData is Empty(data.Count == 0)");
				yield break;
			}

			for (int i = 0; i < m_CurStageData.m_EnemySpawnDataList.Count; i++)//이번 스테이지에서 스폰할 적의 '무리' 수만큼 반복
			{
				StartCoroutine(GenerateEnemyGroup(m_CurStageData.m_EnemyDataList, m_CurStageData.m_EnemySpawnDataList));
			}
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
				yield return new WaitForSeconds(enemySpawnData[i].Time);
				curEnemy = enemyData.Find(n => n.Name.Equals(enemySpawnData[i].Name));
				StartCoroutine(GenerateEnemy(curEnemy, enemySpawnData[i]));
			}
		}

		/// <summary>
		/// 적을 오브젝트 풀에서 get해와 enemyData를 넣고 움직이는 코루틴을 실행시킴
		/// </summary>
		public IEnumerator GenerateEnemy(EnemyData enemyData, EnemySpawnData enemySpawnData)
		{
			Debug.Log("GenerateEnemy");
			enemySpawnData.StartPos = new Vector3(6, 0, 6);
			enemySpawnData.EndPos = new Vector3(0, 0, 6);
			for (int i = 0; i < enemySpawnData.Amount; i++)
			{
				if (i != 0)
					yield return new WaitForSeconds(enemySpawnData.Interval);

				Enemy newEnemy = GetBuilder(enemySpawnData.Name)  //이때 실제 적 오브젝트가 생성됨
					.SetActive(true)
					.SetPosition(enemySpawnData.StartPos)
					.SetAutoInit(true)
					.Spawn();

				newEnemy.m_EnemyData = enemyData;
				newEnemy.m_CurPos = enemySpawnData.StartPos;

				m_EnemyList.Add(newEnemy);

				StartCoroutine(EnemyMove(newEnemy, enemySpawnData));
			}
		}

		/// <summary>
		/// 시작 지점에 나타나 목표 지점 List가 빌 때까지 이동과 대기(0초 가능) 반복
		/// </summary>
		public IEnumerator EnemyMove(Enemy enemy, EnemySpawnData enemySpawnData)
		{
			//경유 지점 추가
			List<Vector3> path = PathFinder.FindPath(enemySpawnData.StartPos, enemySpawnData.EndPos, m_TestMap);
			//path[0]은 시작지점임
			for (int i = 1; i < path.Count; i++)
				enemySpawnData.TransitPos.Add(path[i]);

			for (int i = 0; i < enemySpawnData.TransitPos.Count; i++)
			{
				enemy.m_TargetPos = enemySpawnData.TransitPos[i];
				enemy.m_CurEnemyState = E_EnemyState.Move;
				yield return new WaitUntil(() => enemy.m_CurEnemyState != E_EnemyState.Move);
			}
			yield break;
		}
	}
}