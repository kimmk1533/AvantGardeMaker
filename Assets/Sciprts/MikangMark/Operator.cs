using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.MikangMark;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using System.IO;

namespace AvantGardeMaker.MikangMark
{
	public class Operator : ObjectPoolItemBase
	{
		#region 변수
		public OperInfo m_OperData;

		OperatorYamlManager m_Yaml;

		[SerializeField]
		public string OperName;

		string m_FilePath;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		#endregion

		#region 유니티 콜백 함수
		private void Start()
		{
			Initialize();
		}
		#endregion

		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{
			
		}
		public void SetData()
		{
			Debug.Log("Operator.cs");
			m_Yaml = GameObject.Find("GameManager").GetComponent<OperatorYamlManager>();
			m_FilePath = Path.Combine(m_Yaml.m_FileSaveDirectory, OperName + ".yaml");
			//m_OperData = m_Yaml.LoadData(m_FilePath, OperName + ".yaml");
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{

		}
	}
}