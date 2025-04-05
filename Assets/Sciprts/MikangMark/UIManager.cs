using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.MikangMark
{
	public class UIManager : SerializedSingleton<UIManager>
	{
		#region 변수
		public TextMeshProUGUI m_Cost;
		public Image m_CostImage;

		float m_Timer = 0f;

		public GameObject m_OperStatUI;

		public Operator m_SelectOperator;

		public Image m_OperImg;
		public Image m_JobImg;
		public Image m_Arousal;
		public TextMeshProUGUI m_OperKorName;
		public TextMeshProUGUI m_OperLevelValue;
		public TextMeshProUGUI m_StatAttackValue;
		public TextMeshProUGUI m_StatDefence;
		public TextMeshProUGUI m_StatMagicDefence;
		public TextMeshProUGUI m_StatBlock;

		public Image m_ATKRangeField;
		public Image m_OperatorPos_InRange;
		public Image m_ATKPos;
		public List<Image> m_ATKPosList;

		public float minChildren_x = 0;
		public float maxChildren_x = 0;
		public float opset = 0;
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
			Debug.Log("UIManger");
			OperStatUISetActive(false);
			m_SelectOperator = new Operator();
			m_SelectOperator = InGamePlayManager.Instance.GetOperInfo(0);
			for (int i = 0; i < OperatorJsonManager.Instance.m_OperInfoList.Count; i++)
			{
				if (m_SelectOperator.OperName == OperatorJsonManager.Instance.m_OperInfoList[i].EngOperName)
				{
					m_SelectOperator.m_OperData = OperatorJsonManager.Instance.m_OperInfoList[i];
				}
			}
			m_ATKPosList = new List<Image>();
			for (int i = 0;i< m_SelectOperator.m_OperData.AttackPos.Length; i++)
			{
				m_ATKPosList.Add(Instantiate(m_ATKPos, m_ATKRangeField.transform));
				m_ATKPosList[i].rectTransform.anchoredPosition = m_OperatorPos_InRange.rectTransform.anchoredPosition;
			}
			OperATKRangeCreate(m_SelectOperator.m_OperData.AttackPos);
			AlignChildren();
		}
		private void FixedUpdate()
		{
			UpdateUI();
		}
		#endregion

		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		/// 
		public void OperATKRangeCreate(Vector2[] _ATKRange)
		{
			//공격범위 이미지 사이의 간격
			int intervalOpset = 6;
			for(int i=0;i< _ATKRange.Length; i++)
			{
				m_ATKPosList[i].rectTransform.anchoredPosition += _ATKRange[i] * intervalOpset;
			}
		}
		//생성된 공격범위 가운데 정렬
		public void AlignChildren()
		{

			int childCount = m_ATKRangeField.transform.childCount;
			if (childCount == 0)
				return;
			if (childCount > 1)
			{
				for (int i = 0; i < childCount; i++)
				{
					if (m_ATKRangeField.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition.x < minChildren_x)
					{
						minChildren_x = m_ATKRangeField.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition.x;
					}
					if (m_ATKRangeField.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition.x > maxChildren_x)
					{
						maxChildren_x = m_ATKRangeField.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition.x;
					}
				}

				opset = (maxChildren_x + minChildren_x) / 2.0f;
				Vector2 temp = new Vector2(opset, 0);
				for (int i = 0; i < childCount; i++)
				{
					m_ATKRangeField.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition -= temp;
				}
			}
		}
		public void UpdateUI()
		{
			#region 코스트 관련UI
			m_Timer = InGamePlayManager.Instance.GetRealTime();
			m_CostImage.fillAmount = m_Timer;
			m_Cost.text = InGamePlayManager.Instance.GetCost().ToString();
			#endregion
			#region 활성화된 오퍼스텟정보UI
			m_OperKorName.text = m_SelectOperator.m_OperData.KorOperName;
			m_OperLevelValue.text = m_SelectOperator.m_OperData.Level.ToString();
			m_StatAttackValue.text = m_SelectOperator.m_OperData.Atk.ToString();
			m_StatDefence.text = m_SelectOperator.m_OperData.Def.ToString();
			m_StatMagicDefence.text = m_SelectOperator.m_OperData.Res.ToString();
			m_StatBlock.text = m_SelectOperator.m_OperData.BlockCount.ToString();
			#endregion
		}
		public void OperStatUISetActive(bool is_Active)
		{
			m_OperStatUI.SetActive(is_Active);
		}
		public void OperStatUISetting(OperInfo _operInfo)
		{

		}
		public virtual void Initialize()
		{
			
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public virtual void Finallize()
		{

		}

		/// <summary>
		/// 게임 초기화 함수 (Game Scene 진입 시 호출)
		/// </summary>
		public virtual void InitializeGame()
		{

		}
		/// <summary>
		/// 게임 마무리화 함수 (Game Scene 나갈 시 호출)
		/// </summary>
		public virtual void FinallizeGame()
		{

		}
	}
}