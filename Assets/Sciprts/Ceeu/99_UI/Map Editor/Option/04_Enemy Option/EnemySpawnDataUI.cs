using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.ad1a;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.Ceeu
{
	public class EnemySpawnDataUI : MapEditingUI
	{
		#region 변수
		private Button m_OptionButton = null;
		private TMP_Text m_IndexText = null;
		private Image m_EnemyPortraitImage = null;
		private TMP_Text m_DebugText = null;
		private TMP_InputField m_EnemyCountInputField = null;
		private TMP_InputField m_IntervalInputField = null;
		private TMP_InputField m_WaveInputField = null;
		private TMP_InputField m_WaveTimeInputField = null;
		private TMP_Text m_TimeStampText = null;
		private Button m_DeleteButton = null;

		private List<Vector2> m_EnemyWayPointList = null;
		private List<float> m_EnemyWayPointDelayTimeList = null;
		#endregion

		#region 프로퍼티
		public EnemyData enemyData { get; set; }

		public int index
		{
			get => int.Parse(m_IndexText.text);
			set => m_IndexText.text = value.ToString();
		}
		public string debugText
		{
			get => m_DebugText.text;
			set => m_DebugText.text = value;
		}
		public int count
		{
			get => int.Parse(m_EnemyCountInputField.text);
			set => m_EnemyCountInputField.text = value.ToString();
		}
		public float interval
		{
			get => float.Parse(m_IntervalInputField.text);
			set => m_IntervalInputField.SetTextWithoutNotify(value.ToString());
		}
		public int wave
		{
			get => int.Parse(m_WaveInputField.text);
			set => m_WaveInputField.SetTextWithoutNotify(value.ToString());
		}
		public float waveTime
		{
			get => float.Parse(m_WaveTimeInputField.text);
			set => m_WaveTimeInputField.SetTextWithoutNotify(value.ToString());
		}
		public float time
		{
			get => float.Parse(m_TimeStampText.text);
			set => m_TimeStampText.text = value.ToString();
		}

		public List<Vector2> enemyWayPointList
		{
			get => new List<Vector2>(m_EnemyWayPointList);
			set
			{
				m_EnemyWayPointList.Clear();
				m_EnemyWayPointList.AddRange(value);
			}
		}
		public List<float> enemyWayPointDelayTimeList
		{
			get => new List<float>(m_EnemyWayPointDelayTimeList);
			set
			{
				m_EnemyWayPointDelayTimeList.Clear();
				m_EnemyWayPointDelayTimeList.AddRange(value);
			}
		}
		#endregion

		#region 이벤트

		#region 이벤트 함수
		private void OnOptionButtonClicked()
		{
			EnemyDataSettingPanel settingPanel = M_MapEditingUI.enemyDataSettingPanel;

			settingPanel.SetEnemySpawnDataUI(this);

			settingPanel.gameObject.SetActive(true);
		}

		private void OnEnemyCountInputFieldSubmit(string inputString)
		{
			int.TryParse(inputString, out int inputValue);

			m_EnemyCountInputField.SetTextWithoutNotify(Mathf.Max(1, inputValue).ToString("#,###"));
		}
		private void OnIntervalInputFieldSubmit(string inputString)
		{
			int.TryParse(inputString, out int inputValue);

			m_IntervalInputField.SetTextWithoutNotify(Mathf.Max(1, inputValue).ToString("#,###"));
		}
		private void OnWaveInputFieldSubmint(string inputString)
		{
			int.TryParse(inputString, out int inputValue);

			if (inputValue <= M_MapEditingUI.maxWave)
				m_WaveInputField.SetTextWithoutNotify(inputValue.ToString());

			M_MapEditingUI.ReorderEnemySpawnDataUI();
		}
		private void OnWaveTimeInputFieldSubmit(string inputString)
		{
			float.TryParse(inputString, out float inputValue);
			float.TryParse(m_TimeStampText.text, out float time);
			m_TimeStampText.text = (time + inputValue).ToString();

			M_MapEditingUI.ReorderEnemySpawnDataUI();
		}

		private void OnDeleteButtonClicked()
		{
			M_MapEditingUI.Despawn(this);
		}
		#endregion
		#endregion

		#region 매니저
		private static MapEditingUIManager M_MapEditingUI => MapEditingUIManager.Instance;
		private static EnemyManager M_Enemy => EnemyManager.Instance;
		#endregion

		#region 유니티 콜백 함수
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public override void InitializePoolItem()
		{
			base.InitializePoolItem();

			if (m_OptionButton == null)
			{
				m_OptionButton = transform.Find<Button>("Option Button");

				m_OptionButton.onClick.AddListener(OnOptionButtonClicked);
			}

			if (m_IndexText == null)
			{
				m_IndexText = transform.Find<TMP_Text>("Index Text");
			}
			if (m_EnemyPortraitImage == null)
			{
				m_EnemyPortraitImage = transform.Find("Enemy Portrait & Count").Find<Image>("Enemy Portrait");

				//m_EnemyPortraitImage.sprite = 
			}
			if (m_DebugText == null)
			{
				m_DebugText = m_EnemyPortraitImage.GetComponentInChildren<TMP_Text>();
			}
			if (m_EnemyCountInputField == null)
			{
				m_EnemyCountInputField = transform.Find("Enemy Portrait & Count").Find<TMP_InputField>("Count InputField");

				m_EnemyCountInputField.onSubmit.AddListener(OnEnemyCountInputFieldSubmit);
			}
			m_EnemyCountInputField.SetTextWithoutNotify(1.ToString());

			if (m_IntervalInputField == null)
			{
				m_IntervalInputField = transform.Find<TMP_InputField>("Interval InputField");

				m_IntervalInputField.onSubmit.AddListener(OnIntervalInputFieldSubmit);
			}
			if (m_TimeStampText == null)
			{
				m_TimeStampText = transform.Find("Time Stamp").Find<TMP_Text>("Time Stamp Text");
			}
			if (m_WaveInputField == null)
			{
				m_WaveInputField = transform.Find<TMP_InputField>("Wave InputField");

				m_WaveInputField.onSubmit.AddListener(OnWaveInputFieldSubmint);
			}
			if (m_WaveTimeInputField == null)
			{
				m_WaveTimeInputField = transform.Find<TMP_InputField>("Wave Time InputField");

				m_WaveTimeInputField.onSubmit.AddListener(OnWaveTimeInputFieldSubmit);
			}
			if (m_DeleteButton == null)
			{
				m_DeleteButton = transform.Find<Button>("Delete Button");

				m_DeleteButton.onClick.AddListener(OnDeleteButtonClicked);
			}

			if (m_EnemyWayPointList == null)
				m_EnemyWayPointList = new List<Vector2>();
			if (m_EnemyWayPointDelayTimeList == null)
				m_EnemyWayPointDelayTimeList = new List<float>();
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public override void FinallizePoolItem()
		{
			base.FinallizePoolItem();

			m_IndexText.text = "0";
			//m_EnemyPortraitImage.sprite = 
			m_EnemyCountInputField.SetTextWithoutNotify("1");
			m_IntervalInputField.SetTextWithoutNotify("0");
			m_TimeStampText.text = "0";
			m_WaveInputField.SetTextWithoutNotify("0");
			m_WaveTimeInputField.SetTextWithoutNotify("0");

			m_EnemyWayPointList.Clear();
			m_EnemyWayPointDelayTimeList.Clear();
		}
		#endregion

		public EnemySpawnData MakeSpawnData()
		{
			EnemySpawnData enemySpawnData = new EnemySpawnData();

			// 스폰시킬 적의 이름
			enemySpawnData.Name = enemyData.EngName;

			// 수량(일괄 스폰 시 사용)
			enemySpawnData.Amount = count;

			// 생성 간격(일괄 스폰 시 사용)
			enemySpawnData.Interval = interval;

			// 작전 시작 후 n초에 스폰(최초 스폰까지 걸리는 시간)
			enemySpawnData.Time = time;

			// 웨이브(특정 몹이 죽어야 진행될 경우 사용)
			enemySpawnData.Wave = wave;

			// 웨이브 시간(같은 웨이브에서 스폰까지 걸리는 시간)
			enemySpawnData.WaveTime = waveTime;

			// 경유 지점
			for (int i = 0; i < m_EnemyWayPointList.Count; ++i)
			{
				enemySpawnData.TransitPosList.Add(m_EnemyWayPointList[i]);
			}

			// 경유 지점에서 n초 대기(0초면 딜레이 x)
			for (int i = 0; i < m_EnemyWayPointDelayTimeList.Count; ++i)
			{
				enemySpawnData.DelayTimeList.Add(m_EnemyWayPointDelayTimeList[i]);
			}

			return enemySpawnData;
		}
		public void LoadWayPointUI()
		{
			int count = m_EnemyWayPointList.Count;

			if (count != m_EnemyWayPointDelayTimeList.Count)
				throw new System.Exception("WayPoint List 갯수 다름");

			for (int i = 0; i < count; ++i)
			{
				EnemyWayPointDataUI wayPointDataUI = M_MapEditingUI.GetBuilder("Enemy WayPoint Data UI")
					.SetParent(M_MapEditingUI.enemyWayPointDataUIParent)
					.SetScale(Vector3.one)
					.SetActive(true)
					.SetAutoInit(true)
					.Spawn<EnemyWayPointDataUI>();

				Vector2 wayPoint = m_EnemyWayPointList[i];
				wayPointDataUI.position = wayPoint;

				float delayTime = m_EnemyWayPointDelayTimeList[i];
				wayPointDataUI.delayTime = delayTime;
			}
		}
		public void SaveChildWayPointUI()
		{
			m_EnemyWayPointList.Clear();
			m_EnemyWayPointDelayTimeList.Clear();

			RectTransform wayPointDataUIParent = M_MapEditingUI.enemyWayPointDataUIParent;
			int count = wayPointDataUIParent.childCount;
			for (int i = 0; i < count; ++i)
			{
				EnemyWayPointDataUI wayPointDataUI = wayPointDataUIParent.GetChild<EnemyWayPointDataUI>(i);

				m_EnemyWayPointList.Add(wayPointDataUI.position);
				m_EnemyWayPointDelayTimeList.Add(wayPointDataUI.delayTime);
			}
		}
	}
}