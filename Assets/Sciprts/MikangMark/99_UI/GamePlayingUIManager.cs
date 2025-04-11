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
	public class GamePlayingUIManager : ObjectManager<GamePlayingUIManager, GamePlayingUI>
	{
		#region 변수
		public TextMeshProUGUI m_CostText = null;
		public Image m_CostImage = null;

		public GameObject m_OperStatUIParent = null;

		public OperatorSquadUI m_SelectedOperatorSquadUI = null;

		private Image OperImg = null;
		private Image JobImg = null;
		private Image Arousal = null;

		#region 오퍼레이터 스탯 UI
		[SerializeField]
		private TextMeshProUGUI m_OperatorKorNameText = null;
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
		#endregion

		public Image m_ATKRangeField;
		public Image m_OperatorPos_InRange;
		public Image m_ATKPos;
		public List<Image> m_ATKPosList;

		public float minChildren_x = 0;
		public float maxChildren_x = 0;
		public float m_offset = 0;

		public GameObject m_AttackRangeHighlight;
		public List<GameObject> m_CreatedAttackRangeHighlightList;
		public GameObject m_AttackRangeHighlightParent;
		//OperDrag에서 컨트롤중
		public bool m_IsDragging = false;
		private GameObject m_TileTarget;

		private bool m_TurnOff = false;

		public Button m_DeploymentCancelButton;
		private Tile _cachedTile;

		[SerializeField]
		private RectTransform m_OperatorSquadUIParent = null;

		[SerializeField, ReadOnly]
		private List<OperatorSquadUI> m_OperatorSquadUIList = null;
		#endregion

		#region 프로퍼티
		#endregion

		#region 이벤트

		#region 이벤트 함수
		private void OnOperatorSquadUIClicked(OperatorSquadUI operatorSquadUI)
		{
			// 이미 선택된 오퍼레이터 UI 클릭 시 클릭 취소
			if (m_SelectedOperatorSquadUI == operatorSquadUI)
			{
				SetActiveOperatorStatUI(false);
				m_SelectedOperatorSquadUI = null;
				return;
			}

			m_SelectedOperatorSquadUI = operatorSquadUI;

			SetActiveOperatorStatUI(true);
		}
		#endregion
		#endregion

		#region 매니저
		private static GamePlayingManager M_GamePlaying => GamePlayingManager.Instance;
		private static OperatorManager M_Operator => OperatorManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		private void Start()
		{
			//m_DeploymentCancelButton.gameObject.SetActive(false);
			//m_CreatedAttackRangeHighlightList = new List<GameObject>();
			//SetActiveOperatorStatUI(false);

			m_SelectedOperatorSquadUI = null;

			//OperATKRangeCreate(m_SelectedOperatorSquadUI.operatorData.AttackPos);
			//AlignChildren();

			Initialize();
			InitializeMain();
		}
		private void Update()
		{
			UpdateUI();

			if (m_IsDragging)
			{
				MouseRealPoint();
				m_TurnOff = true;
			}
			else
			{
				if (m_TurnOff)
				{
					for (int i = 0; i < m_CreatedAttackRangeHighlightList.Count; i++)
					{
						m_CreatedAttackRangeHighlightList[i].SetActive(m_TurnOff = false);
					}
				}

			}
		}
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수 (Init Scene 진입 시, 즉 게임 실행 시 호출)
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			if (m_OperatorSquadUIParent == null)
				m_OperatorSquadUIParent = GameObject.Find("OperBox").transform as RectTransform;

			m_OperatorSquadUIList = new List<OperatorSquadUI>();
		}
		/// <summary>
		/// 마무리화 함수 (게임 종료 시 호출)
		/// </summary>
		public override void Finallize()
		{
			base.Finallize();


		}

		/// <summary>
		/// 게임 초기화 함수 (본인 Main Scene 진입 시 호출)
		/// </summary>
		public override void InitializeMain()
		{
			base.InitializeMain();

			CreateOperatorSquadUI();
		}
		/// <summary>
		/// 게임 마무리화 함수 (본인 Main Scene 나갈 시 호출)
		/// </summary>
		public override void FinallizeMain()
		{
			base.FinallizeMain();

			DestroyOperatorSquadUI();
		}
		#endregion

		private void CreateOperatorSquadUI()
		{
			List<string> operatorSquadKeyList = M_GamePlaying.operatorSquadKeyList;

			for (int i = 0; i < operatorSquadKeyList.Count; ++i)
			{
				string operatorKey = operatorSquadKeyList[i];
				OperatorSquadUI operatorSquadUI = GetBuilder("Operator Squad UI")
					.SetParent(m_OperatorSquadUIParent)
					.SetAutoInit(false)
					.SetActive(true)
					.SetName(operatorKey)
					.Spawn() as OperatorSquadUI;

				operatorSquadUI.onOperatorSquadUIClicked += OnOperatorSquadUIClicked;
				operatorSquadUI.operatorData = M_Operator.GetOperatorData(operatorKey);

				operatorSquadUI.InitializePoolItem();

				m_OperatorSquadUIList.Add(operatorSquadUI);
			}
		}
		private void DestroyOperatorSquadUI()
		{
			for (int i = 0; i < m_OperatorSquadUIList.Count; ++i)
			{
				m_OperatorSquadUIList[i].onOperatorSquadUIClicked -= OnOperatorSquadUIClicked;

				Despawn(m_OperatorSquadUIList[i]);
			}
			m_OperatorSquadUIList.Clear();
		}

		public void MouseRealPoint()
		{
			// 마우스 왼쪽 버튼을 누르고 있는 동안 (드래그)
			if (Input.GetMouseButton(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit))
				{
					GameObject hitObj = hit.collider.gameObject;

					// 타겟이 바뀌었을 때만 처리
					if (m_TileTarget != hitObj)
					{
						// 타일 컴포넌트를 캐싱
						_cachedTile = hit.collider.GetComponent<Tile>();

						if (_cachedTile != null)
						{
							OperAtkRangeHighlight(
								RotateViewAtkRange(m_SelectedOperatorSquadUI.operatorData.AttackPos, 180),
								hitObj
							);
						}

						m_TileTarget = hitObj;
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
			for (int i = 0; i < temp.Length; i++)
			{
				temp[i] = new Vector2((int)(_OperAtkRange[i].x * cos - _OperAtkRange[i].y * sin), (int)(_OperAtkRange[i].x * sin + _OperAtkRange[i].y * cos));
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
		//공격범위 UI생성함수
		public void OperATKRangeCreate(Vector2[] _ATKRange)
		{
			//공격범위 이미지 사이의 간격
			int intervalOpset = 6;
			m_ATKPosList = new List<Image>();
			for (int i = 0; i < m_SelectedOperatorSquadUI.operatorData.AttackPos.Length; i++)
			{
				m_ATKPosList.Add(Instantiate(m_ATKPos, m_ATKRangeField.transform));
				m_ATKPosList[i].rectTransform.anchoredPosition = m_OperatorPos_InRange.rectTransform.anchoredPosition;
			}
			for (int i = 0; i < _ATKRange.Length; i++)
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

				m_offset = (maxChildren_x + minChildren_x) / 2.0f;
				Vector2 temp = new Vector2(m_offset, 0);
				for (int i = 0; i < childCount; i++)
				{
					m_ATKRangeField.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition -= temp;
				}
			}
		}
		public void UpdateUI()
		{
			#region 코스트 관련UI
			m_CostImage.fillAmount = M_GamePlaying.costTimer.progress;
			m_CostText.text = M_GamePlaying.currentCost.ToString();
			#endregion

			#region 활성화된 오퍼스텟정보UI
			//m_OperatorKorNameText.text = m_SelectedOperatorSquadUI.operatorData.KorName;
			//m_OperatorLevelText.text = m_SelectedOperatorSquadUI.operatorData.Level.ToString();
			//m_AtkText.text = m_SelectedOperatorSquadUI.operatorData.Atk.ToString();
			//m_DefText.text = m_SelectedOperatorSquadUI.operatorData.Def.ToString();
			//m_ResText.text = m_SelectedOperatorSquadUI.operatorData.Res.ToString();
			//m_BlockText.text = m_SelectedOperatorSquadUI.operatorData.BlockCount.ToString();
			#endregion
		}
		public void SetActiveOperatorStatUI(bool active)
		{
			m_OperStatUIParent.SetActive(active);
		}
	}
}