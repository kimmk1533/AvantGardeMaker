using System;
using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.Ceeu;
using AvantGardeMaker.MikangMark.Enum;
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

		public GameObject m_AttackRangeHighlight;
		public List<GameObject> m_CreatedATKRangeHighlights;
		public GameObject m_AttackRangeHighlight_Parent;
		//OperDrag에서 컨트롤중
		public bool m_Setting = false;
		private GameObject m_TileTarget;

		private bool m_TurnOff = false;

		public Button CancelSetOperBtn;

		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트
		#endregion

		#region 매니저

		private static InGamePlayManager M_InGamePlay => InGamePlayManager.Instance;
		private static OperatorJsonManager M_OperatorJson => OperatorJsonManager.Instance;
		#endregion

		#region 유니티 콜백 함수

		private void Start()
		{
			CancelSetOperBtn.gameObject.SetActive(false);
			m_CreatedATKRangeHighlights = new List<GameObject>();
			OperStatUISetActive(false);
			//디폴트로 선택된 첫번째 오퍼
			m_SelectOperator = new Operator();
			m_SelectOperator = M_InGamePlay.GetOperInfo(0);
			for (int i = 0; i < M_OperatorJson.m_OperInfoList.Count; i++)
			{
				if (m_SelectOperator.OperName == M_OperatorJson.m_OperInfoList[i].EngOperName)
				{
					m_SelectOperator.m_OperData = M_OperatorJson.m_OperInfoList[i];
				}
			}
			OperATKRangeCreate(m_SelectOperator.m_OperData.AttackPos);
			AlignChildren();
		}
		private void Update()
		{
			UpdateUI();
			if (m_Setting)
			{
				MouseRealPoint();
				m_TurnOff = true;
			}
			else
			{
				if (m_TurnOff)
				{
					for (int i = 0; i < m_CreatedATKRangeHighlights.Count; i++)
					{
						m_CreatedATKRangeHighlights[i].SetActive(m_TurnOff = false);
					}
				}
				
			}
		}
		#endregion

		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		/// 
		public void MouseRealPoint()
		{
			// 마우스 왼쪽 버튼을 누르고 있는 동안 (드래그)
			if (Input.GetMouseButton(0)) 
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit))
				{
					Tile target = hit.collider.GetComponent<Tile>();
					if (target != null)
					{
						if(m_TileTarget != hit.collider.gameObject)
						{
							//한번만들어오도록 수정
							OperAtkRangeHighlight(RotateViewAtkRange(m_SelectOperator.m_OperData.AttackPos, 180), hit.collider.gameObject);
						}
						m_TileTarget = hit.collider.gameObject;
					}
				}
			}
		}
		public Vector2[] RotateViewAtkRange(Vector2[] _OperAtkRange, float angleDeg)//회전각도 ex)90
		{
			float angleRad = angleDeg * Mathf.Deg2Rad; // 라디안으로 변환

			float cos = Mathf.Cos(angleRad);
			float sin = Mathf.Sin(angleRad);

			
			Vector2[] temp = new Vector2[_OperAtkRange.Length];
			for(int i=0; i<temp.Length; i++)
			{
				temp[i] = new Vector2((int)(_OperAtkRange[i].x * cos - _OperAtkRange[i].y * sin), (int)(_OperAtkRange[i].x * sin + _OperAtkRange[i].y * cos));
			}
			return temp;
		}
		public void OperAtkRangeHighlight(Vector2[] _OperAtkRange, GameObject _FindTile)
		{
			if (m_CreatedATKRangeHighlights.Count > 0)
			{
				for (int i = 0; i < m_CreatedATKRangeHighlights.Count; i++)
				{
					Destroy(m_CreatedATKRangeHighlights[i]);
				}
				m_CreatedATKRangeHighlights = null;
				m_CreatedATKRangeHighlights = new List<GameObject>();
			}
			for (int i = 0; i < _OperAtkRange.Length + 1; i++)
			{
				m_CreatedATKRangeHighlights.Add(Instantiate(m_AttackRangeHighlight, m_AttackRangeHighlight_Parent.transform));
				if (i == 0)
				{
					m_CreatedATKRangeHighlights[i].transform.position = new Vector3(_FindTile.transform.position.x, 0.2f, _FindTile.transform.position.z);
				}
				else
				{
					m_CreatedATKRangeHighlights[i].transform.position = new Vector3(_FindTile.transform.position.x + _OperAtkRange[i - 1].x, 0.2f, _FindTile.transform.position.z + _OperAtkRange[i - 1].y);
				}
			}
		}
		//공격범위 UI생성함수
		public void OperATKRangeCreate(Vector2[] _ATKRange)
		{
			//공격범위 이미지 사이의 간격
			int intervalOpset = 6;
			m_ATKPosList = new List<Image>();
			for (int i = 0; i < m_SelectOperator.m_OperData.AttackPos.Length; i++)
			{
				m_ATKPosList.Add(Instantiate(m_ATKPos, m_ATKRangeField.transform));
				m_ATKPosList[i].rectTransform.anchoredPosition = m_OperatorPos_InRange.rectTransform.anchoredPosition;
			}
			for (int i=0;i< _ATKRange.Length; i++)
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
			m_Timer = M_InGamePlay.GetRealTime();
			m_CostImage.fillAmount = m_Timer;
			m_Cost.text = M_InGamePlay.GetCost().ToString();
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