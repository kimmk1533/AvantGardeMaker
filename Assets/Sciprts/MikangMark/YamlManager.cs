using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System;
using AvantGardeMaker.MikangMark.Enum;

namespace AvantGardeMaker.MikangMark
{
	public class YamlManager : SerializedSingleton<YamlManager>
	{
		#region 변수

		private string customFileName;
		public string m_FileSaveDirectory = "C:/Users/kimjh741963/Desktop/ARK3D/AvantGardeMaker/Assets/Sciprts/MikangMark/OperInfo_Yaml";
		//public string m_FilePath;
		public List<OperInfo> m_OperInfoList;
		#region 오퍼정보
		[SerializeField]
		OperInfo Fang = new()
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
			m_Job = E_Jop.VanGuard,

			m_MaxHp = 742,
			m_Atk = 157,
			m_Def = 132,
			m_Res = 0,
			m_ReSet = E_ResetSpeed.Slow,
			m_SetCost = 8,
			m_BlockCount = 2,
			m_AtkSpeed = E_AttackSpeed.Nomal,
			m_AtkRange = E_AttackRange.Close,
			m_Provocation = 0,
			m_SkillLevel = 1,
			m_OperPos = new (0, 3),
			m_RealAttackRange = new List<(int, int)> { new(1, 3) }
		};

		[SerializeField]
		OperInfo Plume = new()
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
			m_Job = E_Jop.VanGuard,

			m_MaxHp = 688,
			m_Atk = 230,
			m_Def = 148,
			m_Res = 0,
			m_ReSet = E_ResetSpeed.Slow,
			m_SetCost = 7,
			m_BlockCount = 1,
			m_AtkSpeed = E_AttackSpeed.VeryFast,
			m_AtkRange = E_AttackRange.Close,
			m_Provocation = 0,
			m_SkillLevel = 1,
			m_OperPos = (0, 3),
			m_RealAttackRange = new List<(int, int)> { (1, 3) }
		};

		OperInfo Melantha = new OperInfo
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
			m_Job = E_Jop.Guard,

			m_MaxHp = 1395,
			m_Atk = 396,
			m_Def = 83,
			m_Res = 0,
			m_ReSet = E_ResetSpeed.Slow,
			m_SetCost = 12,
			m_BlockCount = 1,
			m_AtkSpeed = E_AttackSpeed.Slow,
			m_AtkRange = E_AttackRange.Close
		};

		OperInfo Popukar = new OperInfo
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
			m_Job = E_Jop.Guard,

			m_MaxHp = 1130,
			m_Atk = 263,
			m_Def = 126,
			m_Res = 0,
			m_ReSet = E_ResetSpeed.Slow,
			m_SetCost = 17,
			m_BlockCount = 2,
			m_AtkSpeed = E_AttackSpeed.Nomal,
			m_AtkRange = E_AttackRange.Close
		};

		OperInfo Beagle = new OperInfo
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
			m_Job = E_Jop.Defender,

			m_MaxHp = 1144,
			m_Atk = 184,
			m_Def = 242,
			m_Res = 0,
			m_ReSet = E_ResetSpeed.Slow,
			m_SetCost = 17,
			m_BlockCount = 2,
			m_AtkSpeed = E_AttackSpeed.Nomal,
			m_AtkRange = E_AttackRange.Close
		};

		OperInfo Adnachiel = new OperInfo
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
			m_Job = E_Jop.Sniper,

			m_MaxHp = 531,
			m_Atk = 152,
			m_Def = 55,
			m_Res = 0,
			m_ReSet = E_ResetSpeed.Slow,
			m_SetCost = 9,
			m_BlockCount = 1,
			m_AtkSpeed = E_AttackSpeed.Fast,
			m_AtkRange = E_AttackRange.Far
		};

		OperInfo Kroos = new OperInfo
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
			m_Job = E_Jop.Sniper,

			m_MaxHp = 545,
			m_Atk = 154,
			m_Def = 52,
			m_Res = 0,
			m_ReSet = E_ResetSpeed.Slow,
			m_SetCost = 8,
			m_BlockCount = 1,
			m_AtkSpeed = E_AttackSpeed.Fast,
			m_AtkRange = E_AttackRange.Far
		};

		OperInfo Lava = new OperInfo
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
			m_Job = E_Jop.Caster,

			m_MaxHp = 614,
			m_Atk = 321,
			m_Def = 41,
			m_Res = 10,
			m_ReSet = E_ResetSpeed.Slow,
			m_SetCost = 27,
			m_BlockCount = 1,
			m_AtkSpeed = E_AttackSpeed.Slow,
			m_AtkRange = E_AttackRange.Far
		};

		OperInfo Steward = new OperInfo
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
			m_Job = E_Jop.Caster,

			m_MaxHp = 592,
			m_Atk = 249,
			m_Def = 38,
			m_Res = 10,
			m_ReSet = E_ResetSpeed.Slow,
			m_SetCost = 27,
			m_BlockCount = 1,
			m_AtkSpeed = E_AttackSpeed.Slow,
			m_AtkRange = E_AttackRange.Far
		};


		#endregion
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

		}


		#endregion
		public void SaveData(string directory, string fileName, OperInfo operData)
		{
			
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
			string m_FilePath = Path.Combine(directory, fileName);

			var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

			string yaml = serializer.Serialize(operData);
			File.WriteAllText(m_FilePath, yaml);
			//Debug.Log("YAML 저장 완료:\n" + m_FilePath);
		}

		public OperInfo LoadData(string directory, string fileName)
		{
			if (!File.Exists(directory))
			{
				Debug.LogError("YAML 파일을 찾을 수 없습니다!");
				return new OperInfo();
			}
			string yaml = File.ReadAllText(directory);
			var deserializer = new DeserializerBuilder()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.Build();

			OperInfo character = deserializer.Deserialize<OperInfo>(yaml);
			return character;
		}
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public virtual void Initialize()
		{
			m_OperInfoList = new List<OperInfo>();
			SaveData(m_FileSaveDirectory, "Fang.yaml", Fang);
			//LoadData(m_FilePath, "Fang.yaml");
			SaveData(m_FileSaveDirectory, "Plume.yaml", Plume);
			//LoadData(m_FilePath, "Plume.yaml");

			m_OperInfoList.Add(Fang);
			m_OperInfoList.Add(Plume);
		}
		public virtual void Finallize()
		{

		}

		/// <summary>
		/// 게임 초기화 함수 (본인 Main Scene 진입 시 호출)
		/// </summary>
		public virtual void InitializeGame()
		{

		}
		/// <summary>
		/// 게임 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public virtual void FinallizeGame()
		{

		}
	}
}