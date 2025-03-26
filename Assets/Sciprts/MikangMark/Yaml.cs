using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System;

namespace AvantGardeMaker.MikangMark
{
    [SerializeField]
	public class CharInfo
	{
        public string m_EngCharName;//영어이름
        public string m_KorCharName;//한글이름
        public int m_Rate;//레어도
		
        public int m_MaxLevel;//최대레벨
        public int m_Level;//현재레벨
        public int m_MaxExp;//현재레벨 최대경험치
        public int m_Exp;//현재경험치
		
        public int m_Elite;//특성
        public int m_Potential;//재능
        public E_Jop m_Job;//직군
		
        public int m_MaxHp;//최대체력
        public int m_Atk;//공격력
        public int m_Def;//방어력
        public int m_Res;//마항
        public E_ResetSpeed m_ReSet;//재배치속도
        public int m_SetCost;//배치코스트
        public int m_BlockCount;//저지
        public E_AttackSpeed m_AtkSpeed;//공격속도
        public E_AttackRange m_AtkRange;//공격범위

        public int m_Provocation;//도발
        public int m_SkillLevel;//스킬레벨
    }



	public class Yaml : SerializedMonoBehaviour
	{
		#region 변수

		private string customFileName;
        public string m_FileSaveDirectory = "C:/Users/kimjh741963/Desktop/ARK3D/AvantGardeMaker/Assets/Sciprts/MikangMark/CharInfo_Yaml";
        //public string m_FilePath;
        public List<CharInfo> m_CharInfoList;
        #region 오퍼정보
        [SerializeField]
        CharInfo Fang = new CharInfo
        {
            m_EngCharName = "Fang",
            m_KorCharName = "팽",
            m_Rate = 3,

            m_MaxLevel = 40,
            m_Level = 1,
            m_MaxExp = 40,
            m_Exp = 0,

            m_Elite = 0,
            m_Potential = 0,
            m_Job = E_Jop.BangGard,

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
            m_SkillLevel = 1
        };

        [SerializeField]
        CharInfo Plume = new CharInfo
        {
            m_EngCharName = "Plume",
            m_KorCharName = "플룸",
            m_Rate = 3,

            m_MaxLevel = 40,
            m_Level = 1,
            m_MaxExp = 40,
            m_Exp = 0,

            m_Elite = 0,
            m_Potential = 0,
            m_Job = E_Jop.BangGard,

            m_MaxHp = 688,
            m_Atk = 230,
            m_Def = 148,
            m_Res = 0,
            m_ReSet = E_ResetSpeed.Slow,
            m_SetCost = 7,
            m_BlockCount = 1,
            m_AtkSpeed = E_AttackSpeed.VeryFast,
            m_AtkRange = E_AttackRange.Close
        };

        CharInfo Melantha = new CharInfo
        {
            m_EngCharName = "Melantha",
            m_KorCharName = "멜란사",
            m_Rate = 3,

            m_MaxLevel = 40,
            m_Level = 1,
            m_MaxExp = 40,
            m_Exp = 0,

            m_Elite = 0,
            m_Potential = 0,
            m_Job = E_Jop.Gard,

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

        CharInfo Popukar = new CharInfo
        {
            m_EngCharName = "Popukar",
            m_KorCharName = "포푸카",
            m_Rate = 3,

            m_MaxLevel = 40,
            m_Level = 1,
            m_MaxExp = 40,
            m_Exp = 0,

            m_Elite = 0,
            m_Potential = 0,
            m_Job = E_Jop.Gard,

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

        CharInfo Beagle = new CharInfo
        {
            m_EngCharName = "Beagle",
            m_KorCharName = "비글",
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

        CharInfo Adnachiel = new CharInfo
        {
            m_EngCharName = "Adnachiel",
            m_KorCharName = "아드나키엘",
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

        CharInfo Kroos = new CharInfo
        {
            m_EngCharName = "Kroos",
            m_KorCharName = "크루스",
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

        CharInfo Lava = new CharInfo
        {
            m_EngCharName = "Lava",
            m_KorCharName = "라바",
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

        CharInfo Steward = new CharInfo
        {
            m_EngCharName = "Steward",
            m_KorCharName = "스튜어드",
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
            m_CharInfoList = new List<CharInfo>();
            SaveData(m_FileSaveDirectory, "Fang.yaml", Fang);
            //LoadData(m_FilePath, "Fang.yaml");
            SaveData(m_FileSaveDirectory, "Plume.yaml", Plume);
            //LoadData(m_FilePath, "Plume.yaml");

            m_CharInfoList.Add(Fang);
            m_CharInfoList.Add(Plume);
        }


        #endregion
        public void SaveData(string directory, string fileName,CharInfo charData)
        {
            
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string m_FilePath = Path.Combine(directory, fileName);

            var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

            string yaml = serializer.Serialize(charData);
            File.WriteAllText(m_FilePath, yaml);
            //Debug.Log("YAML 저장 완료:\n" + m_FilePath);
        }

        public CharInfo LoadData(string directory, string fileName)
        {
            if (!File.Exists(directory))
            {
                Debug.LogError("YAML 파일을 찾을 수 없습니다!");
                return new CharInfo();
            }
            string yaml = File.ReadAllText(directory);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            CharInfo character = deserializer.Deserialize<CharInfo>(yaml);
            //Debug.Log($"불러온 데이터: Name={character.m_EngCharName}, Health={character.m_MaxHp}, Mana={character.m_Level}");
            return character;
        }
        /// <summary>
        /// 초기화 함수
        /// </summary>
        public void Initialize()
		{
            /*
            m_FilePath = Path.Combine(Application.persistentDataPath, "CharInfo.yaml");
            Debug.Log(m_FilePath);
            SaveData();
            LoadData();
            */
        }
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{

		}
	}
}