using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using AvantGardeMaker.MikangMark.Enum;

namespace AvantGardeMaker.MikangMark
{
	[SerializeField]
	public class OperInfo
	{
		public string m_EngOperName;//영어이름
		public string m_KorOperName;//한글이름
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

		public int[,] m_AttackRange = new int[7, 7];//이차원배열로 오퍼위치(1), 오퍼의 공격범위(0), 오퍼의공격권외(-1) 등을 정수형으로 저장

		public List<int[,]> m_RealAttackRange = new List<int[,]>();//공격범위 좌표의 리스트
		public int[,] m_OperPos = new int[0, 0];//오퍼레이터의 위치

		public void AttackRangeSetting(int _X, int _Y, E_TileAttackRange _AttackType)
		{
			m_AttackRange[_X, _Y] = (int)_AttackType;
		}
	}
	public class OperatorManager : ObjectManager<OperatorManager, Operator>
	{
		#region 변수
		List<Operator> m_ReciveOper = new List<Operator>();//대기실에서 가져온 오퍼레이터들의 모든정보
		List<Operator> m_ActiveOper = new List<Operator>();//인게임에서 배치가 완료된 오퍼레이터의 모든정보

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
			//base.InitializeMain();
		}
		/// <summary>
		/// 게임 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();
		}
		#endregion
	}
}