using System;
using UnityEngine;

namespace LIBII
{
	[Serializable]
	public class SpringBone
	{
		public Transform FixTransform;

		public Transform transform;

		public float Stiffness = 1f;

		public float Radius = 0.05f;

		public float Drag = 1f;

		public float DynamicRatio = 1f;

		public bool IsEnable = true;

		private Quaternion mInitialLocalRotation = Quaternion.identity;

		private Vector3 mInitialLocalRecover = Vector3.zero;

		private Vector3 mCurrReferencePos = Vector3.zero;

		private Vector3 mPrevReferencePos = Vector3.zero;

		private float mInitialSpringLength;

		public void ResetPosture()
		{
			this.mInitialLocalRotation = this.transform.localRotation;
			this.mInitialLocalRecover = this.FixTransform.localPosition.normalized;
			this.mInitialSpringLength = Vector3.Distance(this.transform.position, this.FixTransform.position);
			this.mCurrReferencePos = this.FixTransform.position;
			this.mPrevReferencePos = this.FixTransform.position;
			this.IsEnable = true;
		}

		public void UpdateSpring(float dynamicRatio, Vector3 gravity, float dragScale, float StiffnessScale, SpringCollider[] colliders)
		{
			float num = dynamicRatio * this.DynamicRatio;
			if (num > 0f)
			{
				this.transform.localRotation = this.mInitialLocalRotation;
				Vector3 vector = gravity + (this.mPrevReferencePos - this.mCurrReferencePos) * this.Drag * dragScale;
				vector += this.transform.rotation * this.mInitialLocalRecover * this.Stiffness * StiffnessScale;
				Vector3 vector2 = this.mCurrReferencePos;
				this.mCurrReferencePos += this.mCurrReferencePos - this.mPrevReferencePos + vector;
				this.mCurrReferencePos = (this.mCurrReferencePos - this.transform.position).normalized * this.mInitialSpringLength + this.transform.position;
				for (int i = 0; i < colliders.Length; i++)
				{
					float num2 = this.Radius + colliders[i].Radius;
					Vector3 position = colliders[i].transform.position;
					if ((this.mCurrReferencePos - position).sqrMagnitude < num2 * num2)
					{
						this.mCurrReferencePos = (this.mCurrReferencePos - position).normalized * num2 + position;
						this.mCurrReferencePos = (this.mCurrReferencePos - this.transform.position).normalized * this.mInitialSpringLength + this.transform.position;
					}
				}
				this.mPrevReferencePos = vector2;
				Vector3 fromDirection = this.transform.localToWorldMatrix.MultiplyVector(this.mInitialLocalRecover);
				Quaternion lhs = Quaternion.FromToRotation(fromDirection, this.mCurrReferencePos - this.transform.position);
				if (this.IsEnable)
				{
					this.transform.rotation = Quaternion.Lerp(this.transform.rotation, lhs * this.transform.rotation, num);
				}
			}
		}

		public void DrawDebug()
		{
			if (this.transform)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere(this.mCurrReferencePos, this.Radius);
				Gizmos.DrawWireSphere(this.mCurrReferencePos, this.Radius);
				Gizmos.color = Color.black;
				Gizmos.DrawCube(this.transform.position, Vector3.one * 0.03f);
				Gizmos.DrawLine(this.transform.position, this.mCurrReferencePos);
			}
		}
	}
}
