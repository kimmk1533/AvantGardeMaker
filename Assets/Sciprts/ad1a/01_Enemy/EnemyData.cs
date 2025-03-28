using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.ad1a.Enum;
using AvantGardeMaker.ad1a;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.ad1a
{
	//적 스펙, 특성
	public class EnemyData
	{
		public E_EnemyGrade m_Grade;
		public int m_LossHp;    //보호 지점에 들어가면 깎이는 목표 HP

		public E_EnemyType m_TribeType;
		public E_EnemyFlyable m_Flyable;
		public E_EnemyAtkType m_AtkType;
		public E_EnemyDmgType m_DmgType;

		public string m_Name;   //이름
		public CombatStatValue<float> m_Hp;        //체력
		public CombatStatValue<float> m_Atk;       //공격력
		public CombatStatValue<float> m_Def;     //방어력
		public CombatStatValue<float> m_MagicRes;//마법 저항
		public CombatStatValue<float> m_DmgRes;  //피해 감소
		public CombatStatValue<float> m_MoveSpeed;//이동 속도(타일/s)
		public CombatStatValue<float> m_AtkTime; //공격 간격(n초당 1회)
		public CombatStatValue<float> m_Range;   //사정거리(근거리는 -1)
		public CombatStatValue<int> m_MassLevel; //무게

		public bool[] m_Immune = new bool[6];//기절 수면 빙결 공중 전율 공포 면역여부

		public EnemyData()
		{
			SetFilterStat();
			SetStat("DummyEnemy");
			SetImmuneStat();
		}

		public void SetFilterStat(E_EnemyGrade grade = E_EnemyGrade.E_Normal, int lossHp = 1,
			E_EnemyType type = E_EnemyType.E_Ect, E_EnemyFlyable flyable = E_EnemyFlyable.E_Walk,
			E_EnemyAtkType atkType = E_EnemyAtkType.E_Melee, E_EnemyDmgType dmgType = E_EnemyDmgType.E_Physic)
		{
			m_Grade = grade;
			m_LossHp = lossHp;
			m_TribeType = type;
			m_Flyable = flyable;
			m_AtkType = atkType;
			m_DmgType = dmgType;
		}

		public void SetStat(string name, float hp = 500,
			float atk = 100, float def = 0, float magicRes = 0, float dmgRes = 0,
			float moveSpeed = 5.0f, float atkTime = 2.0f, float range = -1.0f, int massLevel = 1)
		{
			m_Name = name;
			m_Hp.m_CurStat = m_Hp.m_InitStat = hp;
			m_Atk.m_CurStat = m_Atk.m_InitStat = atk;
			m_Def.m_CurStat = m_Def.m_InitStat = def;
			m_MagicRes.m_CurStat = m_MagicRes.m_InitStat = magicRes;
			m_DmgRes.m_CurStat = m_DmgRes.m_InitStat = dmgRes;
			m_MoveSpeed.m_CurStat = m_MoveSpeed.m_InitStat = moveSpeed;
			m_AtkTime.m_CurStat = m_AtkTime.m_InitStat = atkTime;
			m_Range.m_CurStat = m_Range.m_InitStat = range;
			m_MassLevel.m_CurStat = m_MassLevel.m_InitStat = massLevel;
		}

		public void SetImmuneStat(bool stun = false, bool sleep = false, bool freeze = false, bool airborn = false, bool shiver = false, bool fear = false)
		{
			m_Immune[0] = stun;
			m_Immune[1] = sleep;
			m_Immune[2] = freeze;
			m_Immune[3] = airborn;
			m_Immune[4] = shiver;
			m_Immune[5] = fear;
		}
	}
}