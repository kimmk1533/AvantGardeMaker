using System.Collections;
using System.Collections.Generic;
using AvantGardeMaker.OperatorSpace;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AvantGardeMaker.UI
{
	public class OperatorStatusUI : SerializedMonoBehaviour
	{
		#region 기본 템플릿
		#region 변수
		// 현재 선택한 오퍼레이터 데이터
		[PropertySpace(10)]
		[SerializeField]
		private OperatorData m_SelectedOperatorData = null;

		// 오퍼레이터 이미지
		private Image m_OperatorImage = null;

		// 직군 이미지
		private Image m_JobImage = null;
		// 오퍼레이터 이름 텍스트
		private TextMeshProUGUI m_OperatorNameText = null;
		// 정예화 이미지
		private Image m_ArousalImage = null;
		// 오퍼레이터 레벨 텍스트
		private TextMeshProUGUI m_OperatorLevelText = null;

		// 오퍼레이터 공격력 텍스트
		private TextMeshProUGUI m_OperatorAtkText = null;
		// 오퍼레이터 방어력 텍스트
		private TextMeshProUGUI m_OperatorDefText = null;
		// 오퍼레이터 마법 저항력 텍스트
		private TextMeshProUGUI m_OperatorResText = null;
		// 오퍼레이터 저지 수 텍스트
		private TextMeshProUGUI m_OperatorBlockText = null;

		//// 공격 범위 관련
		//private Image m_AttackRangeFieldParent;
		//[SerializeField]
		//private Image m_OperatorPosAttackRangeUI;
		//[SerializeField]
		//private Image m_ATKPos;
		//private List<Image> m_ATKPosList = null;
		////

		// 오퍼레이터 체력 이미지
		private Image m_OperatorHpImage = null;
		// 오퍼레이터 체력 이미지 초기 가로 사이즈
		private float m_OperatorHpInitWidth = 0f;
		// 오퍼레이터 체력 텍스트
		private TextMeshProUGUI m_OperatorHpText = null;

		#endregion

		#region 프로퍼티
		public OperatorData selectedOperatorData
		{
			get => m_SelectedOperatorData;
			set
			{
				m_SelectedOperatorData = value;

				// 오퍼레이터 이미지
				m_OperatorImage.sprite = M_Operator.GetOperatorFullshot(value.key);

				// 직군 이미지
				//m_JobImage = ;
				// 오퍼레이터 이름 텍스트
				m_OperatorNameText.text = value.KorName;
				// 정예화 이미지
				//m_ArousalImage = ;
				// 오퍼레이터 레벨 텍스트
				m_OperatorLevelText.text = value.FixedData.Level.ToString();

				// 오퍼레이터 공격력 텍스트
				m_OperatorAtkText.text = "<font=\"Noto Sans KR SDF\">공격</font> " + value.VariableData.Atk.ToString();
				// 오퍼레이터 방어력 텍스트
				m_OperatorDefText.text = "<font=\"Noto Sans KR SDF\">방어</font> " + value.VariableData.Def.ToString();
				// 오퍼레이터 마법 저항력 텍스트
				m_OperatorResText.text = "<font=\"Noto Sans KR SDF\">마항</font> " + value.VariableData.Res.ToString();
				// 오퍼레이터 저지 수 텍스트
				m_OperatorBlockText.text = "<font=\"Noto Sans KR SDF\">저지</font> " + value.VariableData.BlockCount.ToString();

				// 오퍼레이터 체력 이미지
				Vector2 size = m_OperatorHpImage.rectTransform.sizeDelta;
				size.x = m_OperatorHpInitWidth * (value.VariableData.CurrentHp / value.VariableData.MaxHp);
				m_OperatorHpImage.rectTransform.sizeDelta = size;
				// 오퍼레이터 체력 텍스트
				m_OperatorHpText.text = value.VariableData.CurrentHp.ToString() + "/" + value.VariableData.MaxHp.ToString();

				//ResetATKRangeUI();
				//OperATKRangeCreate(value.VariableData.AttackPos);
			}
		}
		#endregion

		#region 매니저
		private static OperatorManager M_Operator => OperatorManager.Instance;
		#endregion

		#region 이벤트

		#region 이벤트 함수
		#endregion
		#endregion

		#region 초기화 & 마무리화 함수
		/// <summary>
		/// 초기화 함수
		/// </summary>
		public void Initialize()
		{
			// 오퍼레이터 이미지
			m_OperatorImage = GetComponent<Image>();

			// 직군 이미지
			m_JobImage = transform.Find<Image>("Operator Info/Job Image");
			// 오퍼레이터 이름 텍스트
			m_OperatorNameText = transform.Find<TextMeshProUGUI>("Operator Info/Name Text");
			// 정예화 이미지
			m_ArousalImage = transform.Find<Image>("Operator Info/Arousal Image");
			// 오퍼레이터 레벨 텍스트
			m_OperatorLevelText = transform.Find<TextMeshProUGUI>("Operator Info/Operator Level Value Text");

			// 오퍼레이터 공격력 텍스트
			m_OperatorAtkText = transform.Find<TextMeshProUGUI>("Status/Atk Status Text");
			// 오퍼레이터 방어력 텍스트
			m_OperatorDefText = transform.Find<TextMeshProUGUI>("Status/Def Status Text");
			// 오퍼레이터 마법 저항력 텍스트
			m_OperatorResText = transform.Find<TextMeshProUGUI>("Status/Res Status Text");
			// 오퍼레이터 저지 수 텍스트
			m_OperatorBlockText = transform.Find<TextMeshProUGUI>("Status/Block Status Text");

			// 오퍼레이터 체력 이미지
			m_OperatorHpImage = transform.Find<Image>("Hp Image BG/Hp Image");
			// 오퍼레이터 체력 이미지 초기 가로 사이즈
			m_OperatorHpInitWidth = m_OperatorHpImage.rectTransform.sizeDelta.x;
			// 오퍼레이터 체력 텍스트
			m_OperatorHpText = transform.Find<TextMeshProUGUI>("Hp Image BG/Hp Image/Hp Text/Hp Value Text");
		}
		/// <summary>
		/// 마무리화 함수
		/// </summary>
		public void Finallize()
		{
			Vector2 size = m_OperatorHpImage.rectTransform.sizeDelta;
			size.x = m_OperatorHpInitWidth;
			m_OperatorHpImage.rectTransform.sizeDelta = size;
		}
		#endregion

		#region 유니티 콜백 함수
		#endregion
		#endregion
	}
}