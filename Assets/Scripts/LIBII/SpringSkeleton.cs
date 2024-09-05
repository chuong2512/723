using System;
using UnityEngine;

namespace LIBII
{
	public class SpringSkeleton : MonoBehaviour
	{
		public Vector3 Gravity = new Vector3(0f, -0.0001f, 0f);

		public Vector3 RandomGravity = Vector3.zero;

		public float RandomGravitySpeed = 1f;

		public float StiffnessScale = 0.01f;

		public float DynamicRatio = 1f;

		public float DragScale = 0.4f;

		[SerializeField]
		private SpringCollider[] mSpringColliders = new SpringCollider[0];

		[SerializeField]
		private SpringBone[] mSpringBones = new SpringBone[0];

		public SpringCollider[] SpringColliders
		{
			get
			{
				return this.mSpringColliders;
			}
		}

		public SpringBone[] SpringBones
		{
			get
			{
				return this.mSpringBones;
			}
		}

		private void Awake()
		{
			for (int i = 0; i < this.mSpringBones.Length; i++)
			{
				this.mSpringBones[i].ResetPosture();
			}
		}

		private void LateUpdate()
		{
			Vector3 gravity = this.RandomGravity * (Mathf.PerlinNoise(Time.time * this.RandomGravitySpeed, 0f) - 0.5f) + this.Gravity;
			for (int i = 0; i < this.mSpringBones.Length; i++)
			{
				this.mSpringBones[i].UpdateSpring(this.DynamicRatio, gravity, this.DragScale, this.StiffnessScale, this.mSpringColliders);
			}
		}

		public void SetEnable(bool isEnable)
		{
			for (int i = 0; i < this.mSpringBones.Length; i++)
			{
				this.mSpringBones[i].IsEnable = isEnable;
			}
		}

		public void SetScale(float scale)
		{
			for (int i = 0; i < this.mSpringBones.Length; i++)
			{
				this.mSpringBones[i].Radius *= scale;
			}
			for (int j = 0; j < this.mSpringColliders.Length; j++)
			{
				this.mSpringColliders[j].Radius *= scale;
			}
		}
	}
}
