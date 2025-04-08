using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using AvantGardeMaker.MikangMark.Enum;

namespace AvantGardeMaker.MikangMark
{
	public class OperatorManager : ObjectManager<OperatorManager, Operator>
	{
		#region 변수
		//대기실에서 가져온 오퍼레이터들의 모든정보
		public List<OperInfo> m_ReciveOper = new List<OperInfo>();
		//인게임에서 배치가 완료된 오퍼레이터의 모든정보
		public List<OperInfo> m_ActiveOper = new List<OperInfo>();

		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		private static OperatorJsonManager M_OperatorJson => OperatorJsonManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Start()
		{
			for (int i = 0; i < M_OperatorJson.m_OperInfoList.Count; i++)
			{
				m_ReciveOper.Add(M_OperatorJson.m_OperInfoList[i]);
			}
			//Initialize();
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();
			/*
			for(int i=0;i< m_ReciveOper.Count; i++)
			{
				m_ReciveOper[i] = M_OperatorJson.m_OperInfoList[i];
			}
			*/
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
	}
}