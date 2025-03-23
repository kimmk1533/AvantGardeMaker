using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.ad1a.Enum
{
	public enum E_EnemyGrade
	{
		E_Normal,
		E_Elite,
		E_Leader
	}

	public enum E_EnemyType
	{
		E_InfectedCreature, //감염생물
		E_Drone,            //드론
		E_Sarkaz,           //살카즈
		E_Possessed,        //숙주
		E_SeaMonster,       //바다 괴물
		E_ArtsCreation,     //아츠 피조물
		E_Apparition,       //요괴
		E_Machina,          //기계
		E_WildBeast,        //야생동물
		E_Collapsal,        //붕괴체
		E_Ect               //기타
	}

	public enum E_EnemyFlyable
	{
		E_Walk,         //지상
		E_Flying,       //공중
	}

	public enum E_EnemyAtkType
	{
		E_Disable,
		E_Melee,
		E_Range,
	}

	public enum E_EnemyDmgType
	{
		E_Physic,
		E_Magic,
		E_Heal,
		E_None,
	}
}