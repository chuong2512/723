using LitJson;
using System;
using UnityEngine;

public class PoleCaster : Primitives
{
	public float force = 100f;

	private Transform achorTrans;

	protected override void Awake()
	{
		base.Awake();
		this.achorTrans = this.mTrans.Find("Achor");
	}

	private void FixedUpdate()
	{
		if (!this.isSleep)
		{
			this.rigidbody2D.AddForceAtPosition(Vector2.up * this.force, this.mTrans.position - this.mTrans.right * this.originalScale.x / 2f, ForceMode2D.Force);
		}
	}

	public override void SetTransform()
	{
		base.SetTransform();
		this.achorTrans.rotation = Quaternion.identity;
		this.achorTrans.localPosition = new Vector3(this.originalScale.x / 2f, 0f, 0f);
	}

	public override string Serialize()
	{
		string arg = base.Serialize() + ",";
		return arg + "\"force\":" + this.force;
	}

	public override void Deserialization(JsonData json)
	{
		base.Deserialization(json);
		this.force = float.Parse(json["force"].ToString());
	}

	private void OnDrawGizmos()
	{
		if (this.mTrans == null)
		{
			return;
		}
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(this.mTrans.position - this.mTrans.right * this.originalScale.x / 2f, 0.2f);
	}
}
