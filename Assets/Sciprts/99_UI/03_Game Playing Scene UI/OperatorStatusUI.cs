using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class OperatorStatusUI : SerializedSingleton<OperatorStatusUI>
	{
		#region 기본 템플릿
		#region 변수
		[SerializeField, ReadOnly]
		private OperatorData m_SelectedOperator = null;

		private TextMeshProUGUI m_OperatorKorNameText = null;
		private TextMeshProUGUI m_OperatorHpText = null;
		private TextMeshProUGUI m_OperatorLevelText = null;
		private TextMeshProUGUI m_AtkText = null;
		private TextMeshProUGUI m_DefText = null;
		private TextMeshProUGUI m_ResText = null;
		private TextMeshProUGUI m_BlockText = null;
		private Image OperImg = null;
		private Image JobImg = null;
		private Image Arousal = null;
		private Image m_AttackRangeFieldParent;
		[SerializeField]
		private Image m_OperatorPosAttackRangeUI;
		[SerializeField]
		private Image m_ATKPos;
		private List<Image> m_ATKPosList = null;
		/*
		[SerializeField]
		private GameObject m_AttackRangeHighlight;
		private List<GameObject> m_CreatedAttackRangeHighlightList;
		[SerializeField]
		private GameObject m_AttackRangeHighlightParent;
		*/
		private float minChildren_x = 0;
		private float maxChildren_x = 0;
		private float m_offset = 0;
		#endregion

		#region 프로퍼티
		public OperatorData selectedOperatorData
		{
			get => m_SelectedOperator;
			set
			{
				m_SelectedOperator = value;

				m_OperatorKorNameText.text = value.KorName;
				m_OperatorLevelText.text = value.FixedData.Level.ToString();
				m_AtkText.text = value.VariableData.Atk.ToString();
				m_DefText.text = value.VariableData.Def.ToString();
				m_ResText.text = value.VariableData.Res.ToString();
				m_BlockText.text = value.VariableData.BlockCount.ToString();
				OperImg.sprite = M_GamePlayingUI.GetOperatorFullshotSprite(value.key);
				m_OperatorHpText.text = value.VariableData.CurrentHp + "/" + value.VariableData.MaxHp;

				ResetATKRangeUI();
				OperATKRangeCreate(value.VariableData.AttackPos);
			}
		}
		#endregion

		#region 매니저
		private static GamePlayingSceneUIManager M_GamePlayingUI => GamePlayingSceneUIManager.Instance;
		#endregion

		#region 이벤트

		#region 이벤트 함수
		#endregion
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();


		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();


		}
		#endregion

		#region 유니티 콜백 함수
		#endregion
		#endregion


		//공격범위 UI생성함수
		private void OperATKRangeCreate(List<Vector2Int> _ATKRange)
		{
			//공격범위 이미지 사이의 간격
			int intervalOpset = 6;
			m_ATKPosList = new List<Image>();
			for (int i = 0; i < m_SelectedOperator.VariableData.AttackPos.Count; i++)
			{
				m_ATKPosList.Add(Instantiate(m_ATKPos, m_AttackRangeFieldParent.transform));
				m_ATKPosList[i].rectTransform.anchoredPosition = m_OperatorPosAttackRangeUI.rectTransform.anchoredPosition;
			}
			for (int i = 0; i < _ATKRange.Count; i++)
			{
				m_ATKPosList[i].rectTransform.anchoredPosition += _ATKRange[i] * intervalOpset;
			}
			AlignChildren();
		}
		private void ResetATKRangeUI()
		{
			if (m_ATKPosList == null)
				return;
			for (int i = 0; i < m_ATKPosList.Count; i++)
			{
				Destroy(m_ATKPosList[i]);
				m_ATKPosList = new List<Image>();
			}
		}
		//생성된 공격범위 가운데 정렬
		private void AlignChildren()
		{
			int childCount = m_AttackRangeFieldParent.transform.childCount;
			if (childCount == 0)
				return;
			if (childCount > 1)
			{
				for (int i = 0; i < childCount; i++)
				{
					if (m_AttackRangeFieldParent.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition.x < minChildren_x)
					{
						minChildren_x = m_AttackRangeFieldParent.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition.x;
					}
					if (m_AttackRangeFieldParent.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition.x > maxChildren_x)
					{
						maxChildren_x = m_AttackRangeFieldParent.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition.x;
					}
				}

				m_offset = (maxChildren_x + minChildren_x) / 2.0f;
				Vector2 temp = new Vector2(m_offset, 0);
				for (int i = 0; i < childCount; i++)
				{
					m_AttackRangeFieldParent.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition -= temp;
				}
			}
		}
	}
}