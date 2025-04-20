using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using AvantGardeMaker.CoreSpace;
using AvantGardeMaker.CoreSpace.SaveLoad;
using AvantGardeMaker.TileSpace.Enum;
using AvantGardeMaker.UI;
using TMPro;
using UnityEngine;
using TileValue = System.ValueTuple<AvantGardeMaker.TileSpace.Enum.E_TileType, AvantGardeMaker.TileSpace.Tile>;

namespace AvantGardeMaker.TileSpace
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
		public Transform tileParent => m_TileParent.transform;
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		private static MapEditingManager M_MapEditing => MapEditingManager.Instance;
		private static MapEditingUIManager M_MapEditingUI => MapEditingUIManager.Instance;
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

			m_TileParent = new GameObject("Tile Parent");
			m_TileParent.transform.position = Vector3.zero;

			foreach (var item in m_ObjectPoolMap)
			{
				item.Value.onItemDespawned += (Tile tile) =>
				{
					// 기존 텍스트 제거
					TextMeshPro[] textMeshs = tile.transform.GetComponentsInChildren<TextMeshPro>();
					foreach (var item in textMeshs)
					{
						GameObject.Destroy(item.gameObject);
					}
				};
			}
		}
		/// <summary>
		/// 메인 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();

			m_TileMap.Clear();

			m_TileParent = null;
		}
		#endregion

		public void AddTile(Vector2Int tilePos, E_TileType tileType)
		{
			string tileKey = tileType.ToString().Replace('_', ' ');

			Vector3 tilePosition = new Vector3(tilePos.x, tilePos.y) + M_MapEditing.GetTileOffset(tileType);

			Tile newTile = GetBuilder(tileKey)
				.SetPosition(tilePosition)
				.SetActive(true)
				.SetParent(m_TileParent.transform)
				.SetName(tilePos.ToString())
				.SetAutoInit(true)
				.Spawn();

			newTile.gameObject.layer = LayerMask.NameToLayer("Tile");

			m_TileMap.Add(tilePos, (tileType, newTile));

			#region 디버깅
			if (M_MapEditing.isEditMode == false)
				return;

			// 현재 텍스트 생성
			TextMeshPro textMesh = UtilClass.CreateWorldText(newTile.transform, tilePos.ToString(), new UtilClass.WorldTMP_TextOption()
			{
				tmpFont = M_MapEditingUI.uiFont,
				fontSize = 1.5f,
				textAlignment = TextAlignmentOptions.Midline,
				color = Color.black,
			});
			textMesh.transform.position = tilePosition;
			textMesh.transform.rotation = M_MapEditing.mapEditorCamera.transform.rotation;
			#endregion
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
				stageData.SaveTileData(item.Key, item.Value.Item1);
			}
		}
		public void LoadTileData(StageData stageData)
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