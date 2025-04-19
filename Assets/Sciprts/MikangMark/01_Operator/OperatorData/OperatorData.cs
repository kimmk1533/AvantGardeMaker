using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.MikangMark
{
	[CreateAssetMenu(fileName = "OperatorInfo", menuName = "Scriptable Object/OperatorInfo", order = int.MinValue)]
	public class OperatorData : SerializedScriptableObject
	{
		//영어이름
		public string EngName;
		//한글이름
		public string KorName;

		public OperatorFixedData FixedData = new OperatorFixedData();
		public OperatorVariableData VariableData = new OperatorVariableData();

		/*
		public OperatorData Clone()
		{
			OperatorData newInfo = CreateInstance<OperatorData>();

			newInfo.EngName = EngName;
			newInfo.KorName = KorName;
			newInfo.Rate = Rate;
			newInfo.MaxLevel = MaxLevel;
			newInfo.Level = Level;
			newInfo.MaxExp = MaxExp;
			newInfo.Exp = Exp;
			newInfo.Elite = Elite;
			newInfo.Potential = Potential;
			newInfo.Job = Job;
			newInfo.MaxHp = MaxHp;
			newInfo.RealHp = RealHp;
			newInfo.Atk = Atk;
			newInfo.Def = Def;
			newInfo.Res = Res;
			newInfo.RedeploySpeed = RedeploySpeed;
			newInfo.DeploymentCost = DeploymentCost;
			newInfo.BlockCount = BlockCount;
			newInfo.AtkSpeed = AtkSpeed;
			newInfo.AtkRange = AtkRange;
			newInfo.Provocation = Provocation;
			newInfo.SkillLevel = SkillLevel;
			newInfo.AttackPos = (Vector2[])AttackPos.Clone(); // 배열은 복사
			newInfo.HorizontalDirection = HorizontalDirection;
			newInfo.VerticalDirection = VerticalDirection;
			newInfo.PortraitPath = PortraitPath;

			return newInfo;
		}
		*/
	}
}