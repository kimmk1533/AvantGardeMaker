using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CoreSources
{
	public abstract class ObjectPoolItem<TItem> : SerializedMonoBehaviour where TItem : ObjectPoolItem<TItem>
	{
		#region 기본 템플릿
		#region 변수
		#endregion

		#region 프로퍼티
		public string poolKey { get; set; }
		public bool isSpawning { get; private set; }
		#endregion

		#region 이벤트
		public event System.Action<TItem> onSpawn = null;
		public event System.Action<TItem> onDespawn = null;

		#region 이벤트 함수
		#endregion
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (생성될 때)
		/// </summary>
		public abstract void Initialize();
		/// <summary>
		/// 마무리화 함수 (파괴될 때)
		/// </summary>
		public abstract void Finallize();

		/// <summary>
		/// 초기화 함수 (스폰될 때)
		/// </summary>
		public virtual void InitializePoolItem()
		{
			isSpawning = true;

			onSpawn?.Invoke(this as TItem);
		}
		/// <summary>
		/// 마무리화 함수 (디스폰될 때)
		/// </summary>
		public virtual void FinallizePoolItem()
		{
			onDespawn?.Invoke(this as TItem);

			onSpawn = null;
			onDespawn = null;

			isSpawning = false;
		}
		#endregion

		#region 유니티 콜백 함수
		#endregion
		#endregion
	}
}