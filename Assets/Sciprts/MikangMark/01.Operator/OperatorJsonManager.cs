using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using AvantGardeMaker.MikangMark.Enum;

namespace AvantGardeMaker.MikangMark
{
	public class OperatorJsonManager : SerializedSingleton<OperatorJsonManager>
	{
		#region 변수

		public string m_FileSaveDirectory = "C:/Users/kimjh741963/Desktop/ARK3D/AvantGardeMaker/Assets/Sciprts/MikangMark/OperInfo_Json";
		//public string m_FilePath;
		[SerializeField]
		public List<OperInfo> m_OperInfoList;
		/*
		#region 오퍼정보

		[SerializeField]
		public OperInfo Fang = new OperInfo()
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
			m_AttackRange = new E_TileAttackRange[7, 7],
		};

		[SerializeField]
		public OperInfo Plume = new()
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
		*/
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
		public virtual void Initialize()
		{

		}
		public virtual void Finallize()
		{

		}

		/// <summary>
		/// 게임 초기화 함수 (본인 Main Scene 진입 시 호출)
		/// </summary>
		public virtual void InitializeMain()
		{

		}
		/// <summary>
		/// 게임 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public virtual void FinallizeMain()
		{

		}
	}
}