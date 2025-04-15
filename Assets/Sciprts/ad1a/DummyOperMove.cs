using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker
{
	public class DummyOperMove : MonoBehaviour
	{
		public float m_MoveSpeed = 1.0f;
		private void Update()
		{
			float Hor = Input.GetAxis("Horizontal");
			float Vert = Input.GetAxis("Vertical");

			Vector3 Pos = transform.position;

			Pos.x += Hor * Time.deltaTime * m_MoveSpeed;
			Pos.y += Vert * Time.deltaTime * m_MoveSpeed;

			transform.position = Pos;
		}
	}
}