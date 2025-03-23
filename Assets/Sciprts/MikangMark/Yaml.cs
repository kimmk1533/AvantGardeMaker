using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AvantGardeMaker.MikangMark
{
	public class CharInfo
	{
        public string m_EngCharName;
        public string m_KorCharName;
        public int m_Rate;
		
        public int m_MaxLevel;
        public int m_Level;
        public int m_MaxExp;
        public int m_Exp;
		
        public int m_Elite;
        public int m_Potential;
		
        public int m_MaxHp;
        public int m_Atk;
        public int m_Def;
        public int m_Res;
        public int m_ReSet;
        public int m_SetCost;
        public int m_BlockCount;
        public int m_AtkSpeed;
    }

	public class Yaml : SerializedMonoBehaviour
	{
		#region 변수

		private string m_FilePath;

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
            m_FilePath = Path.Combine(Application.persistentDataPath, "CharInfo.yaml");
            Debug.Log(m_FilePath);
            SaveData();
            LoadData();
        }
        #endregion
        public void SaveData()
        {
            CharInfo charInfo = new CharInfo
            {
                m_EngCharName = "Mountain",
                m_KorCharName = "마운틴",
                m_Rate = 6,

                m_MaxLevel = 50,
                m_Level = 1,
                m_MaxExp = 170,
                m_Exp = 0,

                m_Elite = 0,
                m_Potential = 0,

                m_MaxHp = 1298,
                m_Atk = 242,
                m_Def = 154,
                m_Res = 0,
                m_ReSet = -1,
                m_SetCost = 9,
                m_BlockCount = 1,
                m_AtkSpeed = 2
            };
            var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

            string yaml = serializer.Serialize(charInfo);
            File.WriteAllText(m_FilePath, yaml);
            Debug.Log("YAML 저장 완료:\n" + yaml);
        }

        public void LoadData()
        {
            if (!File.Exists(m_FilePath))
            {
                Debug.LogError("YAML 파일을 찾을 수 없습니다!");
                return;
            }
            string yaml = File.ReadAllText(m_FilePath);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            CharInfo character = deserializer.Deserialize<CharInfo>(yaml);
            Debug.Log($"불러온 데이터: Name={character.m_EngCharName}, Health={character.m_MaxHp}, Mana={character.m_Level}");
        }

        

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
	}
}