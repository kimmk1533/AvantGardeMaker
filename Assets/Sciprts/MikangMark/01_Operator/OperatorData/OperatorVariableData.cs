using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.MikangMark.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.MikangMark
{
	[System.Serializable]
	public class OperatorVariableData
	{
		//최대체력
		public float MaxHp;
		//현재체력
		public float RealHp;
		//공격력
		public float Atk;
		//방어력
		public float Def;
		//마항
		public float Res;
		//재배치속도
		public E_RedeploySpeed RedeploySpeed;
		//재배치 실제 속도
		public float RedeploymentInterval;
		//배치코스트
		public int DeploymentCost;
		//저지
		public int BlockCount;
		//공격속도 실제값
		public float InitAttakSpeed;
		//도발
		public int Provocation;
		//공격범위좌표
		public List<Vector2> AttackPos;
		//공격타입
		public E_DamageType DamageType;
		//관통력
		public float Penetration;
		//스킬 종류와 그에대한값의 리스트
		public List<SkillInfo> SkillInfoList;
	}
}