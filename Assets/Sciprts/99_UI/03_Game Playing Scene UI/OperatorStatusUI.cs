using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class OperatorStatusUI : GamePlayingUI
	{
		#region 변수
		[SerializeField, RuntimeReadOnly]
		private OperatorData m_SelectedOperator = null;

		[SerializeField]
		private TextMeshProUGUI m_OperatorKorNameText = null;
		[SerializeField]
		private TextMeshProUGUI m_OperatorHpText = null;
		[SerializeField]
		private TextMeshProUGUI m_OperatorLevelText = null;
		[SerializeField]
		private TextMeshProUGUI m_AtkText = null;
		[SerializeField]
		private TextMeshProUGUI m_DefText = null;
		[SerializeField]
		private TextMeshProUGUI m_ResText = null;
		[SerializeField]
		private TextMeshProUGUI m_BlockText = null;
		[SerializeField]
		private Image OperImg = null;
		[SerializeField]
		private Image JobImg = null;
		[SerializeField]
		private Image Arousal = null;
		[SerializeField]
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
		public OperatorData selectedOperator
		{
			get => m_SelectedOperator;
			set => m_SelectedOperator = value;
		}
		#endregion

		#region 이벤트
		#endregion

		#region 매니저
		private static GamePlayingUIManager M_GamePlayingUI => GamePlayingUIManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Start()
		{
			Initialize();
		}
		private void Update()
		{

		}
		#endregion
		/*
		public Vector2[] RotateViewAtkRange(Vector2[] operatorAttackRange, float angleDeg)//회전각도 ex)90
		{
			float angleRad = angleDeg * Mathf.Deg2Rad; // 라디안으로 변환

			float cos = Mathf.Cos(angleRad);
			float sin = Mathf.Sin(angleRad);


			Vector2[] temp = new Vector2[operatorAttackRange.Length];
			for (int i = 0; i < temp.Length; i++)
			{
				temp[i] = new Vector2((int)(operatorAttackRange[i].x * cos - operatorAttackRange[i].y * sin), (int)(operatorAttackRange[i].x * sin + operatorAttackRange[i].y * cos));
			}
			return temp;
		}
		
		public void OperAtkRangeHighlight(Vector2[] _OperAtkRange, GameObject _FindTile)
		{
			if (m_CreatedAttackRangeHighlightList.Count > 0)
			{
				for (int i = 0; i < m_CreatedAttackRangeHighlightList.Count; i++)
				{
					Destroy(m_CreatedAttackRangeHighlightList[i]);
				}
				m_CreatedAttackRangeHighlightList = null;
				m_CreatedAttackRangeHighlightList = new List<GameObject>();
			}
			for (int i = 0; i < _OperAtkRange.Length + 1; i++)
			{
				m_CreatedAttackRangeHighlightList.Add(Instantiate(m_AttackRangeHighlight, m_AttackRangeHighlightParent.transform));
				if (i == 0)
				{
					m_CreatedAttackRangeHighlightList[i].transform.position = new Vector3(_FindTile.transform.position.x, 0.2f, _FindTile.transform.position.z);
				}
				else
				{
					m_CreatedAttackRangeHighlightList[i].transform.position = new Vector3(_FindTile.transform.position.x + _OperAtkRange[i - 1].x, 0.2f, _FindTile.transform.position.z + _OperAtkRange[i - 1].y);
				}
			}
		}
		*/
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
		public void ChangeOperatorStatusUI(OperatorData selectedOperatorData)
		{
			m_OperatorKorNameText.text = selectedOperatorData.KorName;
			m_OperatorLevelText.text = selectedOperatorData.FixedData.Level.ToString();
			m_AtkText.text = selectedOperatorData.VariableData.Atk.ToString();
			m_DefText.text = selectedOperatorData.VariableData.Def.ToString();
			m_ResText.text = selectedOperatorData.VariableData.Res.ToString();
			m_BlockText.text = selectedOperatorData.VariableData.BlockCount.ToString();
			OperImg.sprite = M_GamePlayingUI.GetOperatorFullshotSprite(selectedOperatorData.key);
			m_OperatorHpText.text = selectedOperatorData.VariableData.CurrentHp + "/" + selectedOperatorData.VariableData.MaxHp;

			ResetATKRangeUI();
			OperATKRangeCreate(m_SelectedOperator.VariableData.AttackPos);
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
	}
}