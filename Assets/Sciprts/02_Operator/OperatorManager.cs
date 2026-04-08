using System.Collections;
using System.Collections.Generic;
using System.IO;
using AvantGardeMaker.CoreSpace.SaveLoad;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	public class OperatorManager : ObjectManager<OperatorManager, Operator>
	{
		private const string c_OperatorDataPath = "Datas\\02_Operator Datas";
		private const string c_OperatorSpritePath = "Textures\\02_Operator Textures" + "";

		#region 변수
		private Dictionary<string, OperatorData> m_OperatorDataMap = null;
		// 오퍼레이터 정면 이미지 (SD 이미지) 모음
		private Dictionary<string, Sprite> m_OperatorFrontSpriteMap = null;
		// 오퍼레이터 후면 이미지 (SD 이미지) 모음
		private Dictionary<string, Sprite> m_OperatorBackSpriteMap = null;
		// 오퍼레이터 초상화 이미지 모음
		private Dictionary<string, Sprite> m_OperatorPortraitMap = null;
		// 오퍼레이터 풀샷 이미지 모음
		private Dictionary<string, Sprite> m_OperatorFullshotMap = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
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

			m_OperatorDataMap = new Dictionary<string, OperatorData>();
			m_OperatorFrontSpriteMap = new Dictionary<string, Sprite>();
			m_OperatorBackSpriteMap = new Dictionary<string, Sprite>();
			m_OperatorPortraitMap = new Dictionary<string, Sprite>();
			m_OperatorFullshotMap = new Dictionary<string, Sprite>();

			LoadOperatorData();
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public override void Finallize()
		{
			m_OperatorDataMap.Clear();
			m_OperatorPortraitMap.Clear();

			m_OperatorDataMap = null;
			m_OperatorPortraitMap = null;

			base.Finallize();
		}

		/// <summary>
		/// 메인 초기화 함수 (본인 Main Scene 진입 시 호출)
		/// </summary>
		public override void InitializeMain()
		{
			base.InitializeMain();
		}
		/// <summary>
		/// 메인 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();
		}
		#endregion

		public OperatorData GetOperatorData(string key)
		{
			if (m_OperatorDataMap.TryGetValue(key, out OperatorData operatorData) == false)
				return null;

			return operatorData;
		}
		public Sprite GetOperatorFrontSprite(string key)
		{
			if (m_OperatorFrontSpriteMap.TryGetValue(key, out Sprite frontSprite) == false)
				return null;

			return frontSprite;
		}
		public Sprite GetOperatorBackSprite(string key)
		{
			if (m_OperatorBackSpriteMap.TryGetValue(key, out Sprite backSprite) == false)
				return null;

			return backSprite;
		}
		public Sprite GetOperatorPortrait(string key)
		{
			if (m_OperatorPortraitMap.TryGetValue(key, out Sprite portrait) == false)
				return null;

			return portrait;
		}
		public Sprite GetOperatorFullshot(string key)
		{
			if (m_OperatorFullshotMap.TryGetValue(key, out Sprite fullshot) == false)
				return null;

			return fullshot;
		}

		///<summary>
		/// Resources 폴더에 있는 OperatorData 스크립터블 오브젝트를 List에 저장
		/// </summary>
		[Button("Load OperatorData")]
		public void LoadOperatorData()
		{
			m_OperatorDataMap.Clear();
			m_OperatorFrontSpriteMap.Clear();
			m_OperatorBackSpriteMap.Clear();
			m_OperatorPortraitMap.Clear();
			m_OperatorFullshotMap.Clear();

			OperatorData[] operatorDatas = Resources.LoadAll<OperatorData>(c_OperatorDataPath);
			foreach (var operatorData in operatorDatas)
			{
				string key = operatorData.key;

				m_OperatorDataMap.Add(key, new OperatorData(operatorData));
				m_OperatorFrontSpriteMap.Add(key, Resources.Load<Sprite>(Path.Combine(c_OperatorSpritePath, key, key + "_Front")));
				m_OperatorBackSpriteMap.Add(key, Resources.Load<Sprite>(Path.Combine(c_OperatorSpritePath, key, key + "_Back")));
				m_OperatorPortraitMap.Add(key, Resources.Load<Sprite>(operatorData.FixedData.PortraitImagePath));
				m_OperatorFullshotMap.Add(key, Resources.Load<Sprite>(operatorData.FixedData.FullShotImagePath));
			}
		}
		/// <summary>
		/// 스크립터블 데이터를 들고 있는 m_OperatorDataMap에 stageData의 데이터를 덮어써 operatorData를 만듦
		/// </summary>
		public void LoadOperatorData(in StageData stageData)
		{
			List<OperatorSpawnData> spawnDataList = stageData.operatorSpawnDataList;
			List<OperatorFixedData> fixedDataList = stageData.operatorFixedDataList;
			List<OperatorVariableData> variableDataList = stageData.operatorVariableDataList;

			int count = spawnDataList.Count;

			for (int i = 0; i < count; ++i)
			{
				OperatorSpawnData spawnData = spawnDataList[i];

				OperatorData operatorData = m_OperatorDataMap[spawnData.OperatorSpawnKey];

				operatorData.FixedData = fixedDataList[i];
				operatorData.VariableData = variableDataList[i];
			}
		}

		public List<OperatorData> GetAllOperatorDatas()
		{
			return new List<OperatorData>(m_OperatorDataMap.Values);
		}
	}
}