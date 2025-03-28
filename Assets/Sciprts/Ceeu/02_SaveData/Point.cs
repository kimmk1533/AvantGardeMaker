using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AvantGardeMaker.Ceeu
{
	[System.Serializable]
	public struct Point
	{
		public int x;
		public int y;
		public int z;

		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
			this.z = 0;
		}
		public Point(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public static implicit operator Vector3(Point point)
		{
			return new Vector3Int(point.x, point.y, point.z);
		}
		public static implicit operator Vector3Int(Point point)
		{
			return new Vector3Int(point.x, point.y, point.z);
		}
		public static implicit operator Point(Vector3Int vector)
		{
			return new Point(vector.x, vector.y, vector.z);
		}
	}
}