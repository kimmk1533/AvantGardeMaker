using System.Collections;
using System.Collections.Generic;
using System.IO;
using AvantGardeMaker.CoreSpace.SaveLoad;
using CoreSources;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.OperatorSpace
{
	public class OperatorManager : ObjectManager<OperatorManager, Operator>
	{
		private const string c_OperatorDataPath = "Datas\\02_Operator Datas";
		private const string c_OperatorSpritePath = "Textures\\02_Operator Textures";

		#region 변수
		// 현재 게임에서 사용하는 오퍼레이터 데이터 모음
		private Dictionary<string, OperatorData> m_OperatorDataMap = null;
		// 원본 오퍼레이터 데이터 모음
		private Dictionary<string, (OperatorFixedData fixedData, OperatorVariableData variableData)> m_OperatorDataOriginMap = null;

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
			m_OperatorDataOriginMap = new Dictionary<string, (OperatorFixedData fixedData, OperatorVariableData variableData)>();

			m_OperatorFrontSpriteMap = new Dictionary<string, Sprite>();
			m_OperatorBackSpriteMap = new Dictionary<string, Sprite>();
			m_OperatorPortraitMap = new Dictionary<string, Sprite>();
			m_OperatorFullshotMap = new Dictionary<string, Sprite>();

			LoadOriginOperatorData();
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
		private void LoadOriginOperatorData()
		{
			OperatorData[] operatorDatas = Resources.LoadAll<OperatorData>(c_OperatorDataPath);
			foreach (var operatorData in operatorDatas)
			{
				string key = operatorData.key;

				m_OperatorDataMap.Add(key, operatorData.Clone());
				m_OperatorDataOriginMap.Add(key, (operatorData.FixedData, operatorData.VariableData));
				
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
			List<OperatorFixedData> fixedDataList = stageData.operatorFixedDataList;
			List<OperatorVariableData> variableDataList = stageData.operatorVariableDataList;

			if (fixedDataList.Count != variableDataList.Count)
				throw new System.Exception(stageData.title + "의 OperatorFixedData와 OperatorVariableData의 갯수가 다름");

			// 추후 최적화 더 생각해봐야 할 듯 (현재 생각 중인건 로드 언로드 방식. 사용하는 데이터만 로드하고 이전 씬으로 돌아갈 때 언로드(원본 데이터로 복구) 하는 방식)
			Dictionary<string, (OperatorFixedData fixedData, OperatorVariableData variableData)> tempMap = new Dictionary<string, (OperatorFixedData, OperatorVariableData)>(m_OperatorDataOriginMap);
			for (int i = 0; i < fixedDataList.Count; ++i)
			{
				tempMap[fixedDataList[i].EngName] = (fixedDataList[i], variableDataList[i]);
			}

			foreach (var item in m_OperatorDataMap)
			{
				item.Value.FixedData = tempMap[item.Key].fixedData;
				item.Value.VariableData = tempMap[item.Key].variableData;
			}
		}

		public List<OperatorData> GetAllOperatorDatas()
		{
			return new List<OperatorData>(m_OperatorDataMap.Values);
		}
	}
}