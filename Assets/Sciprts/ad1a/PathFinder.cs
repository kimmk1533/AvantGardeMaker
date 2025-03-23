using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.ad1a
{
	public class PathFinder : SerializedMonoBehaviour
	{
		#region 변수
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

		private static readonly Vector2Int[] m_Directions =	{
		new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0), // 상하좌우
        new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1), new Vector2Int(-1, 1) // 대각선
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
			List<Node> openList = new List<Node>();
			HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

			Node startNode = new Node(start, null, 0, Vector2Int.Distance(start, goal));
			openList.Add(startNode);

			while (openList.Count > 0)
			{
				openList.Sort((a, b) => a.m_F.CompareTo(b.m_F)); // F 값이 작은 순으로 정렬
				Node currentNode = openList[0];
				openList.RemoveAt(0);
				closedSet.Add(currentNode.m_Position);

				if (currentNode.m_Position == goal)
					return ReconstructPath(currentNode);

				foreach (var direction in m_Directions)
				{
					Vector2Int neighborPos = currentNode.m_Position + direction;
					if (!IsValidPosition(neighborPos, grid) || closedSet.Contains(neighborPos))
						continue;

					float gCost = currentNode.m_G + Vector2Int.Distance(currentNode.m_Position, neighborPos);
					float hCost = Vector2Int.Distance(neighborPos, goal);
					Node neighborNode = new Node(neighborPos, currentNode, gCost, hCost);

					if (openList.Exists(n => n.m_Position == neighborPos && n.m_G <= gCost))
						continue;

					openList.Add(neighborNode);
				}
			}
			return null; // 경로 없음
		}

		private static List<Vector2Int> ReconstructPath(Node node)
		{
			List<Vector2Int> path = new List<Vector2Int>();
			while (node != null)
			{
				path.Add(node.m_Position);
				node = node.m_Parent;
			}
			path.Reverse();
			return path;
		}

		private static bool IsValidPosition(Vector2Int pos, bool[,] grid)
		{
			return pos.x >= 0 && pos.x < grid.GetLength(0) && pos.y >= 0 && pos.y < grid.GetLength(1) && grid[pos.x, pos.y];
		}
	}
}