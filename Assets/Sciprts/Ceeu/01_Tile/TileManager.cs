using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.Ceeu.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using TileValue = System.ValueTuple<AvantGardeMaker.Ceeu.Enum.E_TileType, AvantGardeMaker.Ceeu.Tile>;

namespace AvantGardeMaker.Ceeu
{
	public class TileManager : ObjectManager<TileManager, Tile>
	{
		#region 변수
		// 생성한 타일 부모
		private GameObject m_TileParent = null;

		// 생성한 타일 맵
		private Dictionary<Vector2Int, TileValue> m_TileMap = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		private static MapEditorManager M_MapEditor => MapEditorManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			m_TileMap = new Dictionary<Vector2Int, TileValue>();

			gameObject.SetActive(false);
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

			m_TileParent = new GameObject("Tile Parent");
			m_TileParent.transform.position = Vector3.zero;

			gameObject.SetActive(true);
		}
		/// <summary>
		/// 게임 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();

			m_TileParent = null;
		}
		#endregion

		public void AddTile(Vector2Int tilePos, E_TileType tileType)
		{
			string tileKey = tileType.ToString().Replace('_', ' ');

			Vector3 tilePosition = new Vector3(tilePos.x, tilePos.y) + M_MapEditor.GetTileOffset(tileType);

			Tile newTile = GetBuilder(tileKey)
				.SetPosition(tilePosition)
				.SetActive(true)
				.SetParent(m_TileParent.transform)
				.SetName(tilePos.ToString())
				.SetAutoInit(true)
				.Spawn();

			m_TileMap.Add(tilePos, (tileType, newTile));
		}
		public void RemoveTile(Vector2Int tilePos)
		{
			Tile removeTile = m_TileMap[tilePos].Item2;

			Despawn(removeTile);

			m_TileMap.Remove(tilePos);
		}
		public void ReplaceTile(Vector2Int tilePos, E_TileType tileType)
		{
			RemoveTile(tilePos);

			AddTile(tilePos, tileType);
		}
		public TileValue GetTileValue(Vector2Int tilePos)
		{
			if (m_TileMap.TryGetValue(tilePos, out TileValue tileValue) == false)
				return default;

			return tileValue;
		}
		public void ClearTile()
		{
			int count = m_TileMap.Count;
			for (int i = 0; i < count; ++i)
			{
				Despawn(m_TileParent.transform.GetChild<Tile>(0));
			}
			m_TileMap.Clear();
		}

		public void SaveTileData(ref StageData stageData)
		{
			foreach (var item in m_TileMap)
			{
				stageData.AddTile(item.Key, item.Value.Item1);
			}
		}
		public void LoadTileData(ref StageData stageData)
		{
			ClearTile();

			int count = stageData.tilePointList.Count;

			if (count != stageData.tileTypeList.Count)
				Debug.LogError("저장한 위치와 타일의 갯수가 다름");

			for (int i = 0; i < count; ++i)
			{
				Vector2Int tilePos = stageData.tilePointList[i];
				E_TileType tileType = stageData.tileTypeList[i];

				AddTile(tilePos, tileType);
			}
		}
	}
}