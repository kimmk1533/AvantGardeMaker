using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.EnemySpace.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.EnemySpace
{
	[System.Serializable]
	public class EnemySkillData : SerializedScriptableObject
	{
		public string Name;
		public E_EnemySkillType Type;
	}
}