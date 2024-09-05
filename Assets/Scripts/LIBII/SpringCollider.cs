using System;
using UnityEngine;

namespace LIBII
{
	[Serializable]
	public class SpringCollider
	{
		public float Radius = 0.5f;

		public Transform transform;

		public void DrawDebug()
		{
			if (this.transform)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawSphere(this.transform.position, this.Radius);
				Gizmos.DrawWireSphere(this.transform.position, this.Radius);
			}
		}
	}
}
