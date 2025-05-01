using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;
using AvantGardeMaker.CoreSpace;

namespace AvantGardeMaker.OperatorSpace
{
	public abstract class OperatorSkill
	{
		
		#region 변수
		#endregion

		#region 프로퍼티
		public string skillName { get; protected set; }
		
		public float maxSP { get; protected set; }
		public float currentSP { get; protected set; }
		public float startSP { get; protected set; }
		public E_SPGainType gainSPType { get; protected set; }
		public E_SkillActivationType skillActivationType { get; protected set; }
		public UtilClass.Timer activeSkillTime { get; protected set; }
		public string skillText { get; protected set; }

		#endregion

		#region 생성자
		protected OperatorSkill(SkillInfo skillData)
		{
			maxSP = skillData.MaxSP;
			currentSP = skillData.CurrentSP;
			startSP = skillData.StartSP;
			gainSPType = skillData.GainSPType;
			skillActivationType = skillData.ActivationSkillType;
			activeSkillTime = skillData.ActiveSkillTime;
			skillText = skillData.SkillText;
		}
		#endregion

		#region 매니져
		public GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;
		#endregion

		public abstract void Activate();
		
		public void RecoverSP(int value)
		{
			if (currentSP < maxSP)
			{
				if (currentSP + value > maxSP)
				{
					currentSP = maxSP;
				}
				else
				{
					currentSP += value;
				}
				
			}
				
		}
		public void DeployGainCost(int gainCostValue)
		{
			M_GamePlaying.currentCost += gainCostValue;
		}

		private void AttackBuff(Operator targetOperator, float buffSkillValue)
		{
			if (buffSkillValue < 1)
			{
				Debug.Log("ATK" + targetOperator.variableData.Atk);
				targetOperator.variableData.Atk = targetOperator.variableData.Atk * (1 + buffSkillValue);
				Debug.Log("ATK" + targetOperator.variableData.Atk);
			}
			else
			{
				Debug.Log("ATK" + targetOperator.variableData.Atk);
				targetOperator.variableData.Atk += buffSkillValue;
				Debug.Log("ATK" + targetOperator.variableData.Atk);
			}
		}

		private void AttackSpeedBuff(Operator targetOperator, float buffSkillValue)
		{
			Debug.Log("AttackSpeed" + targetOperator.variableData.InitAttakSpeed);
			targetOperator.variableData.InitAttakSpeed += buffSkillValue;
			Debug.Log("AttackSpeed" + targetOperator.variableData.InitAttakSpeed);
		}

		
	}


}