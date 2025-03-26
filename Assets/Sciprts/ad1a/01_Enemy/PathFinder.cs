using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.ad1a
{
	public class PathFinder : SerializedMonoBehaviour
	{
		#region 변수
		bool[,] testMap;
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

			List<Vector2Int> path = FindPath(new Vector2Int(1, 0), new Vector2Int(3, 2), map);
			for (int i = 0; i < path.Count; i++)
			{
				Debug.Log(path[i]);
			}
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

		private static readonly Vector2Int[] m_Directions = {
		new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0),	 //상하좌우
        new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1), new Vector2Int(-1, 1)	 //대각선
    };

		public class Node
		{
			public Vector2Int m_Position;
			public Node m_Parent;
			public float m_G, m_H;
			public float m_F => m_G + m_H;

			public Node(Vector2Int position, Node parent, float g, float h)
			{
				m_Position = position;
				m_Parent = parent;
				m_G = g;
				m_H = h;
			}
		}

		public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal, bool[,] grid)
		{
			List<Node> openList = new List<Node>();                     //열린 노드
			HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();  //닫힌 노드(중복x라 해시셋)

			Node startNode = new Node(start, null, 0, Vector2Int.Distance(start, goal));    //시작 노드(부모x)
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
					Vector2Int neighborPos = currentNode.m_Position + direction;                //이웃 노드 선택
					if (!IsValidPosition(neighborPos, grid) || closedSet.Contains(neighborPos)) //이동이 불가하거나 닫힌 노드에 있는 노드면 생략
						continue;

					float gCost = currentNode.m_G + Vector2Int.Distance(currentNode.m_Position, neighborPos);   //G(시작~자신) = 부모(curNode)의 G+부모에서 자신까지의 거리 합산
					float hCost = Vector2Int.Distance(neighborPos, goal);                                       //H(자신~도착) = Distance(맨해튼 x, 단순 거리로 했음)
					Node neighborNode = new Node(neighborPos, currentNode, gCost, hCost);                       //list에 넣기 위해 새 노드 생성

					if (openList.Exists(n => n.m_Position == neighborPos && n.m_G <= gCost))    //만약 열린 노드에 G 소모값이 더 낮은 노드가 이미 존재할 경우 생략
						continue;

					if (direction.x * direction.y != 0) //x와 y가 모두 움직이는 경우(=대각선의 경우)
					{
						if (!IsValidPosition(currentNode.m_Position.x + direction.x, currentNode.m_Position.y, grid))//이동 방향의 x축이 이동 불가 지형인 경우 생략
							continue;
						if (!IsValidPosition(currentNode.m_Position.x, currentNode.m_Position.y + direction.y, grid))//이동 방향의 y축이 이동 불가 지형인 경우 생략
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
		private static List<Vector2Int> ReconstructPath(Node node)
		{
			List<Vector2Int> path = new List<Vector2Int>();
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
		private static bool IsValidPosition(Vector2Int pos, bool[,] grid)
		{
			return pos.x >= 0 && pos.x < grid.GetLength(0) &&   //x값이 grid 안에 있는지
				pos.y >= 0 && pos.y < grid.GetLength(1) &&      //y값이 grid 안에 있는지
				grid[pos.x, pos.y];                             //현재 좌표가 grid에서 이동 가능한지
		}
		/// <summary>
		/// 이동 가능 여부
		/// </summary>
		private static bool IsValidPosition(int posX, int posY, bool[,] grid)
		{
			return posX >= 0 && posX < grid.GetLength(0) &&   //x값이 grid 안에 있는지
				posY >= 0 && posY < grid.GetLength(1) &&      //y값이 grid 안에 있는지
				grid[posX, posY];                             //현재 좌표가 grid에서 이동 가능한지
		}
	}
}