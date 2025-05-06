using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.TileSpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.EnemySpace
{
	public class PathFinder
	{
		#region 변수
		public bool[,] testMap;
		#endregion

		#region 프로퍼티
		public static Vector2Int offset { get; set; }
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

		#region 초기화 & 마무리화 함수
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
		#endregion

		//위(12시)부터 시계방향으로 탐색
		private static readonly Vector2[] m_Directions = {
			Vector2.up,
			Vector2.up + Vector2.right,
			Vector2.right,
			Vector2.right + Vector2.down,
			Vector2.down,
			Vector2.down + Vector2.left,
			Vector2.left,
			Vector2.left + Vector2.up,
		};

		public class Node
		{
			public Vector2 Position;
			public Node Parent;
			// 시작점 -> 현재 위치
			public float G;
			// 현재 위치 -> 도착점
			public float H;
			public float F => G + H;

			public Node(Vector2 position, Node parent, float g, float h)
			{
				Position = position;
				Parent = parent;
				G = g;
				H = h;
			}
		}

		public static float[,] GetWeightMap(in (E_TileType tileType, E_TilePositionType tilePositionType)[,] map, bool isFlyable = false)
		{
			float[,] weightMap = new float[map.GetLength(0), map.GetLength(1)];

			if (isFlyable == true)
				return weightMap;

			for (int y = 0; y < map.GetLength(0); ++y)
			{
				for (int x = 0; x < map.GetLength(1); ++x)
				{
					// 이동 불가 타일인 경우(울타리이거나 언덕 타일인 경우)
					if (map[y, x].tileType == E_TileType.Fence ||
						map[y, x].tilePositionType == E_TilePositionType.HighGround)
						// 가중치 1백만 추가
						weightMap[y, x] += 1000000f;
					// 즉사 타일인 경우(구덩이인 경우)
					if (map[y, x].tileType == E_TileType.Hole)
						// 가중치 1만 추가
						weightMap[y, x] += 10000f;
				}
			}

			return weightMap;
		}

		public static List<Vector2> FindPath(Vector2 start, Vector2 goal, in float[,] weightMap)
		{
			//열린 노드
			PriorityQueue<Node, float> openPriorityQueue = new PriorityQueue<Node, float>();
			//닫힌 노드(중복x라 해시셋)
			HashSet<Vector2> closedSet = new HashSet<Vector2>();
			// gCost 맵
			Dictionary<Vector2, float> gCostMap = new Dictionary<Vector2, float>();

			//시작 노드(부모x)
			Node startNode = new Node(start, null, 0, Vector2.Distance(start, goal));
			//열린 노드에 시작 노드 삽입
			openPriorityQueue.Enqueue(startNode, startNode.F);
			// gCost 저장
			gCostMap.Add(start, 0f);

			while (openPriorityQueue.Count > 0)
			{
				//현재 노드 선택
				Node currentNode = openPriorityQueue.Dequeue();
				//닫힌 노드에 좌표 추가
				closedSet.Add(currentNode.Position);

				//현재 노드가 도착 지점이라면
				if (currentNode.Position == goal)
					//parent를 따라 경로 완성 후 return
					return ReconstructPath(currentNode);

				//8방향 탐색
				foreach (var direction in m_Directions)
				{
					//이웃 노드 선택
					Vector2 neighborPos = currentNode.Position + direction;

					// 생략 가능 여부 확인
					if (!IsValidPosition(neighborPos.x, neighborPos.y, weightMap) || // 이동이 불가하거나
						closedSet.Contains(neighborPos)) // 닫힌 노드에 있는 노드면 생략
						continue;

					//x와 y가 모두 움직이는 경우(=대각선의 경우)
					if (direction.x * direction.y != 0)
					{
						//이동 방향의 x축이 이동 불가 지형인 경우 생략
						if (!IsValidPosition(currentNode.Position.x + direction.x, currentNode.Position.y, weightMap))
							continue;
						//이동 방향의 y축이 이동 불가 지형인 경우 생략
						if (!IsValidPosition(currentNode.Position.x, currentNode.Position.y + direction.y, weightMap))
							continue;
					}

					float weight = weightMap[Mathf.RoundToInt(neighborPos.y), Mathf.RoundToInt(neighborPos.x)];
					//G(시작~자신) = 부모(curNode)의 G + 부모에서 자신까지의 거리 + 가중치 합산
					float gCost = currentNode.G + Vector2.Distance(currentNode.Position, neighborPos) + weight;
					//H(자신~도착) = Distance(맨해튼 x, 단순 거리로 했음)
					float hCost = Vector2.Distance(neighborPos, goal);
					//list에 넣기 위해 새 노드 생성
					Node neighborNode = new Node(neighborPos, currentNode, gCost, hCost);

					//만약 열린 노드에 G 소모값이 더 낮은 노드가 이미 존재할 경우 생략
					if (gCostMap.TryGetValue(neighborPos, out float g) == true)
					{
						// 해당 노드까지 오기 위한 비용이 더 낮은 길이 이미 존재할 경우 생략
						if (g < gCost)
							continue;

						// 현재 길의 비용이 더 낮은 경우 비용 저장
						gCostMap[neighborPos] = g;
					}
					else
						// gCost 저장
						gCostMap.Add(neighborPos, gCost);

					//위 조건에 해당하지 않으면 열린 노드에 추가
					openPriorityQueue.Enqueue(neighborNode, neighborNode.F);
				}
			}

			return null; //while 내에서 return되지 않았다면 경로 없음
		}

		/// <summary>
		/// 시작지점 ~ node까지의 list 반환
		/// </summary>
		private static List<Vector2> ReconstructPath(Node node)
		{
			List<Vector2> path = new List<Vector2>();
			while (node != null)//부모가 null인 시작 노드까지 가기 위함
			{
				path.Add(node.Position);
				node = node.Parent;
			}
			//path.Reverse(); //도착점부터 add했기 때문에 전체 순서를 뒤집어야 함
			return path;
		}
		/// <summary>
		/// 이동 가능 여부
		/// </summary>
		private static bool IsValidPosition(float posX, float posY, in float[,] weightMap)
		{
			int posXInt = Mathf.RoundToInt(posX);
			int posYInt = Mathf.RoundToInt(posY);

			int x = posXInt + offset.x;
			int y = posYInt + offset.y;

			return x >= 0 && x < weightMap.GetLength(1) &&   //x값이 grid 안에 있는지
				y >= 0 && y < weightMap.GetLength(0) &&      //y값이 grid 안에 있는지
				weightMap[y, x] < 1000000f;                             //현재 좌표가 grid에서 이동 가능한지
		}
	}
}