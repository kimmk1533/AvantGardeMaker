using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.Ceeu.Enum;
using AvantGardeMaker.MikangMark;
using Sirenix.OdinInspector;
using UnityEngine;
using YamlDotNet.Serialization;

namespace AvantGardeMaker.Ceeu
{
	public class Tile : ObjectPoolItemBase
	{
		#region 변수

		#endregion

		#region 프로퍼티
		#region YamlIgnore
		#region ObjectPoolItemBase YamlIgnore
		[YamlIgnore]
		public new string poolKey { get => base.poolKey; set => base.poolKey = value; }
		[YamlIgnore]
		public new bool isSpawning => base.isSpawning;
		#endregion

		#region MonoBehavior YamlIgnore
		[YamlIgnore]
		public new bool didStart => base.didStart;
		[YamlIgnore]
		public new bool useGUILayout { get => base.useGUILayout; set => base.useGUILayout = value; }
		[YamlIgnore]
		public new System.Threading.CancellationToken destroyCancellationToken => base.destroyCancellationToken;
		[YamlIgnore]
		public new bool didAwake => base.didAwake;
		[YamlIgnore]
		public new bool runInEditMode { get => base.runInEditMode; set => base.runInEditMode = value; }
		#endregion

		#region Behavior YamlIgnore
		[YamlIgnore]
		public new bool enabled { get => base.enabled; set => base.enabled = value; }
		[YamlIgnore]
		public new bool isActiveAndEnabled => base.isActiveAndEnabled;
		#endregion

		#region Component YamlIgnore
		[YamlIgnore]
		public new Component particleSystem { get; }
		[YamlIgnore]
		public new Transform transform => base.transform;
		[YamlIgnore]
		public new GameObject gameObject => base.gameObject;
		[YamlIgnore]
		public new string tag => base.tag;
		[YamlIgnore]
		public new Component rigidbody { get; }
		[YamlIgnore]
		public new Component rigidbody2D { get; }
		[YamlIgnore]
		public new Component camera { get; }
		[YamlIgnore]
		public new Component light { get; }
		[YamlIgnore]
		public new Component constantForce { get; }
		[YamlIgnore]
		public new Component renderer { get; }
		[YamlIgnore]
		public new Component audio { get; }
		[YamlIgnore]
		public new Component networkView { get; }
		[YamlIgnore]
		public new Component collider { get; }
		[YamlIgnore]
		public new Component collider2D { get; }
		[YamlIgnore]
		public new Component animation { get; }
		[YamlIgnore]
		public new Component hingeJoint { get; }
		#endregion

		#region Object YamlIgnore
		[YamlIgnore]
		public new HideFlags hideFlags { get => base.hideFlags; set => base.hideFlags = value; }
		[YamlIgnore]
		public new string name { get => base.name; set => base.name = value; }
		#endregion
		#endregion

		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수
		#endregion

		#region 초기화 & 마무리화 함수
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
		#endregion

		public void SetOperator(string operatorKey, E_Direction direction)
		{

		}
	}
}