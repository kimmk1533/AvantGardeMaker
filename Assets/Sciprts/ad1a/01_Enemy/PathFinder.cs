using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.ad1a
{
	public class PathFinder : SerializedMonoBehaviour
	{
		#region 변수
		public bool[,] testMap;
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
			bool[,] map = new bool[5, 5]
			{ { true,true,true,true,true},
			 { true,false,true,true,true},
			 { true,false,false,true,true},
			 { false,true,true,false,true},
			 { false,true,true,true,true}};

			//List<Vector2Int> path = FindPath(new Vector2Int(1, 0), new Vector2Int(3, 2), map);
			//for (int i = 0; i < path.Count; i++)
			//{
			//	Debug.Log(path[i]);
			//}
		}

		#endregion

		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{

		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{

		}

		private static readonly Vector3[] m_Directions = {
		new Vector3(0,0, 1), new Vector3(1,0, 0), new Vector3(0,0, -1), new Vector3(-1,0, 0),	 //상하좌우
        new Vector3(1,0, 1), new Vector3(1,0, -1), new Vector3(-1,0, -1), new Vector3(-1,0, 1)	 //대각선
    };

		public class Node
		{
			public Vector3 m_Position;
			public Node m_Parent;
			public float m_G, m_H;
			public float m_F => m_G + m_H;

			public Node(Vector3 position, Node parent, float g, float h)
			{
				m_Position = position;
				m_Parent = parent;
				m_G = g;
				m_H = h;
			}
		}

		public static List<Vector3> FindPath(Vector3 start, Vector3 goal, bool[,] grid)
		{
			Debug.Log("시작지점: " + start);
			Debug.Log("목표지점: " + goal);

			List<Node> openList = new List<Node>();                     //열린 노드
			HashSet<Vector3> closedSet = new HashSet<Vector3>();  //닫힌 노드(중복x라 해시셋)

			Node startNode = new Node(start, null, 0, Vector3.Distance(start, goal));    //시작 노드(부모x)
			openList.Add(startNode);                                                        //열린 노드에 시작 노드 삽입

			while (openList.Count > 0)
			{
				openList.Sort((a, b) => a.m_F.CompareTo(b.m_F));//F 값이 작은 순으로 정렬
				Node currentNode = openList[0];                 //현재 노드 선택
				openList.RemoveAt(0);                           //열린 노드에서 제거
				closedSet.Add(currentNode.m_Position);          //닫힌 노드에 좌표 추가

				if (currentNode.m_Position == goal)             //현재 노드가 도착 지점이라면
					return ReconstructPath(currentNode);        //parent를 따라 경로 완성 후 return

				foreach (var direction in m_Directions)         //8방향 탐색
				{
					Vector3 neighborPos = currentNode.m_Position + direction;                //이웃 노드 선택
					if (!IsValidPosition(Mathf.FloorToInt(neighborPos.x), Mathf.FloorToInt(neighborPos.z), grid) || closedSet.Contains(neighborPos)) //이동이 불가하거나 닫힌 노드에 있는 노드면 생략
						continue;

					float gCost = currentNode.m_G + Vector3.Distance(currentNode.m_Position, neighborPos);   //G(시작~자신) = 부모(curNode)의 G+부모에서 자신까지의 거리 합산
					float hCost = Vector3.Distance(neighborPos, goal);                                       //H(자신~도착) = Distance(맨해튼 x, 단순 거리로 했음)
					Node neighborNode = new Node(neighborPos, currentNode, gCost, hCost);                       //list에 넣기 위해 새 노드 생성

					if (openList.Exists(n => n.m_Position == neighborPos && n.m_G <= gCost))    //만약 열린 노드에 G 소모값이 더 낮은 노드가 이미 존재할 경우 생략
						continue;

					if (direction.x * direction.z != 0) //x와 y가 모두 움직이는 경우(=대각선의 경우)
					{
						if (!IsValidPosition(Mathf.FloorToInt(currentNode.m_Position.x + direction.x), Mathf.FloorToInt(currentNode.m_Position.z), grid))//이동 방향의 x축이 이동 불가 지형인 경우 생략
							continue;
						if (!IsValidPosition(Mathf.FloorToInt(currentNode.m_Position.x), Mathf.FloorToInt(currentNode.m_Position.z + direction.z), grid))//이동 방향의 y축이 이동 불가 지형인 경우 생략
							continue;
					}

					openList.Add(neighborNode);                                                 //위 조건에 해당하지 않으면 열린 노드에 추가
				}
			}
			return null; //while 내에서 return되지 않았다면 경로 없음
		}

		/// <summary>
		/// 시작지점 ~ node까지의 list 반환
		/// </summary>
		private static List<Vector3> ReconstructPath(Node node)
		{
			List<Vector3> path = new List<Vector3>();
			while (node != null)//부모가 null인 시작 노드까지 가기 위함
			{
				path.Add(node.m_Position);
				node = node.m_Parent;
			}
			path.Reverse(); //도착점부터 add했기 때문에 전체 순서를 뒤집어야 함
			return path;
		}
		/// <summary>
		/// 이동 가능 여부
		/// </summary>
		private static bool IsValidPosition(int posX, int posZ, bool[,] grid)
		{
			return posX >= 0 && posX < grid.GetLength(1) &&   //x값이 grid 안에 있는지
				posZ >= 0 && posZ < grid.GetLength(0) &&      //y값이 grid 안에 있는지
				grid[posZ, posX];                             //현재 좌표가 grid에서 이동 가능한지
		}
	}
}