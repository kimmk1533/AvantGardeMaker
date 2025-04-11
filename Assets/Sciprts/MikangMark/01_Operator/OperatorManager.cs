using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.MikangMark
{
	public class OperatorManager : ObjectManager<OperatorManager, Operator>
	{
		/*
		#region 오퍼정보

		[SerializeField]
		public OperatorData Fang = new OperatorData()
		{
			m_EngOperName = "Fang",
			m_KorOperName = "팽",
			m_Rate = 3,

			m_MaxLevel = 40,
			m_Level = 1,
			m_MaxExp = 40,
			m_Exp = 0,

			m_Elite = 0,
			m_Potential = 0,
			m_Job = E_JopType.VanGuard,

			m_MaxHp = 742,
			m_Atk = 157,
			m_Def = 132,
			m_Res = 0,
			m_ReSet = E_RedeploySpeed.Slow,
			m_SetCost = 8,
			m_BlockCount = 2,
			m_AtkSpeed = E_AttackSpeed.Nomal,
			m_AtkRange = E_AttackRange.Close,
			m_Provocation = 0,
			m_SkillLevel = 1,
			m_AttackRange = new E_TileAttackRange[7, 7],
		};

		[SerializeField]
		public OperatorData Plume = new()
		{
			m_EngOperName = "Plume",
			m_KorOperName = "플룸",
			m_Rate = 3,

			m_MaxLevel = 40,
			m_Level = 1,
			m_MaxExp = 40,
			m_Exp = 0,

			m_Elite = 0,
			m_Potential = 0,
			m_Job = E_JopType.VanGuard,

			m_MaxHp = 688,
			m_Atk = 230,
			m_Def = 148,
			m_Res = 0,
			m_ReSet = E_RedeploySpeed.Slow,
			m_SetCost = 7,
			m_BlockCount = 1,
			m_AtkSpeed = E_AttackSpeed.VeryFast,
			m_AtkRange = E_AttackRange.Close,
			m_Provocation = 0,
			m_SkillLevel = 1,
		};

		OperatorData Melantha = new OperatorData
		{
			m_EngOperName = "Melantha",
			m_KorOperName = "멜란사",
			m_Rate = 3,

			m_MaxLevel = 40,
			m_Level = 1,
			m_MaxExp = 40,
			m_Exp = 0,

			m_Elite = 0,
			m_Potential = 0,
			m_Job = E_JopType.Guard,

			m_MaxHp = 1395,
			m_Atk = 396,
			m_Def = 83,
			m_Res = 0,
			m_ReSet = E_RedeploySpeed.Slow,
			m_SetCost = 12,
			m_BlockCount = 1,
			m_AtkSpeed = E_AttackSpeed.Slow,
			m_AtkRange = E_AttackRange.Close
		};

		OperatorData Popukar = new OperatorData
		{
			m_EngOperName = "Popukar",
			m_KorOperName = "포푸카",
			m_Rate = 3,

			m_MaxLevel = 40,
			m_Level = 1,
			m_MaxExp = 40,
			m_Exp = 0,

			m_Elite = 0,
			m_Potential = 0,
			m_Job = E_JopType.Guard,

			m_MaxHp = 1130,
			m_Atk = 263,
			m_Def = 126,
			m_Res = 0,
			m_ReSet = E_RedeploySpeed.Slow,
			m_SetCost = 17,
			m_BlockCount = 2,
			m_AtkSpeed = E_AttackSpeed.Nomal,
			m_AtkRange = E_AttackRange.Close
		};

		OperatorData Beagle = new OperatorData
		{
			m_EngOperName = "Beagle",
			m_KorOperName = "비글",
			m_Rate = 3,

			m_MaxLevel = 40,
			m_Level = 1,
			m_MaxExp = 40,
			m_Exp = 0,

			m_Elite = 0,
			m_Potential = 0,
			m_Job = E_JopType.Defender,

			m_MaxHp = 1144,
			m_Atk = 184,
			m_Def = 242,
			m_Res = 0,
			m_ReSet = E_RedeploySpeed.Slow,
			m_SetCost = 17,
			m_BlockCount = 2,
			m_AtkSpeed = E_AttackSpeed.Nomal,
			m_AtkRange = E_AttackRange.Close
		};

		OperatorData Adnachiel = new OperatorData
		{
			m_EngOperName = "Adnachiel",
			m_KorOperName = "아드나키엘",
			m_Rate = 3,

			m_MaxLevel = 40,
			m_Level = 1,
			m_MaxExp = 40,
			m_Exp = 0,

			m_Elite = 0,
			m_Potential = 0,
			m_Job = E_JopType.Sniper,

			m_MaxHp = 531,
			m_Atk = 152,
			m_Def = 55,
			m_Res = 0,
			m_ReSet = E_RedeploySpeed.Slow,
			m_SetCost = 9,
			m_BlockCount = 1,
			m_AtkSpeed = E_AttackSpeed.Fast,
			m_AtkRange = E_AttackRange.Far
		};

		OperatorData Kroos = new OperatorData
		{
			m_EngOperName = "Kroos",
			m_KorOperName = "크루스",
			m_Rate = 3,

			m_MaxLevel = 40,
			m_Level = 1,
			m_MaxExp = 40,
			m_Exp = 0,

			m_Elite = 0,
			m_Potential = 0,
			m_Job = E_JopType.Sniper,

			m_MaxHp = 545,
			m_Atk = 154,
			m_Def = 52,
			m_Res = 0,
			m_ReSet = E_RedeploySpeed.Slow,
			m_SetCost = 8,
			m_BlockCount = 1,
			m_AtkSpeed = E_AttackSpeed.Fast,
			m_AtkRange = E_AttackRange.Far
		};

		OperatorData Lava = new OperatorData
		{
			m_EngOperName = "Lava",
			m_KorOperName = "라바",
			m_Rate = 3,

			m_MaxLevel = 40,
			m_Level = 1,
			m_MaxExp = 40,
			m_Exp = 0,

			m_Elite = 0,
			m_Potential = 0,
			m_Job = E_JopType.Caster,

			m_MaxHp = 614,
			m_Atk = 321,
			m_Def = 41,
			m_Res = 10,
			m_ReSet = E_RedeploySpeed.Slow,
			m_SetCost = 27,
			m_BlockCount = 1,
			m_AtkSpeed = E_AttackSpeed.Slow,
			m_AtkRange = E_AttackRange.Far
		};

		OperatorData Steward = new OperatorData
		{
			m_EngOperName = "Steward",
			m_KorOperName = "스튜어드",
			m_Rate = 3,

			m_MaxLevel = 40,
			m_Level = 1,
			m_MaxExp = 40,
			m_Exp = 0,

			m_Elite = 0,
			m_Potential = 0,
			m_Job = E_JopType.Caster,

			m_MaxHp = 592,
			m_Atk = 249,
			m_Def = 38,
			m_Res = 10,
			m_ReSet = E_RedeploySpeed.Slow,
			m_SetCost = 27,
			m_BlockCount = 1,
			m_AtkSpeed = E_AttackSpeed.Slow,
			m_AtkRange = E_AttackRange.Far
		};


		#endregion
		*/
		private static readonly string s_DataPath = "MikangMark/Datas/OperatorDatas";

		#region 변수
		private Dictionary<string, OperatorData> m_OperatorDataMap = null;
		private Dictionary<string, Sprite> m_OperatorPortraitMap = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수
		protected override void Awake()
		{
			base.Awake();

			Initialize();
			InitializeMain();
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			m_OperatorDataMap = new Dictionary<string, OperatorData>();
			m_OperatorPortraitMap = new Dictionary<string, Sprite>();

			OperatorData[] operatorDatas = Resources.LoadAll<OperatorData>(s_DataPath);

			for (int i = 0; i < operatorDatas.Length; ++i)
			{
				string key = operatorDatas[i].EngName;

				m_OperatorDataMap.Add(key, operatorDatas[i]);
				m_OperatorPortraitMap.Add(key, Resources.Load<Sprite>(operatorDatas[i].PortraitPath));
			}
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();

			m_OperatorDataMap.Clear();
			m_OperatorPortraitMap.Clear();

			m_OperatorDataMap = null;
			m_OperatorPortraitMap = null;
		}

		/// <summary>
		/// 게임 초기화 함수 (본인 Main Scene 진입 시 호출)
		/// </summary>
		public override void InitializeMain()
		{
			base.InitializeMain();
		}
		/// <summary>
		/// 게임 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();
		}
		#endregion

		public OperatorData GetOperatorData(string key)
		{
			if (m_OperatorDataMap.TryGetValue(key, out OperatorData info) == false)
				return null;

			return info;
		}
		public Sprite GetOperatorPortrait(string key)
		{
			if (m_OperatorPortraitMap.TryGetValue(key, out Sprite portrait) == false)
				return null;

			return portrait;
		}
	}
}