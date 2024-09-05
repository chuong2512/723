using LitJson;
using System;
using System.Text;
using UnityEngine;

public class Portal : Primitives
{
	public Portal other;

	public PortalTransfer portalTransfer;

	public Collider2D collider;

	public Transform point;

	public int transferType;

	protected override void Awake()
	{
		base.Awake();
		this.point = this.mTrans.Find("Point");
		Transform transform = this.mTrans.Find("Trigger");
		this.portalTransfer = transform.gameObject.AddComponent<PortalTransfer>();
		this.portalTransfer.portal = this;
		this.collider = this.mTrans.Find("Collider").GetComponent<Collider2D>();
	}

	public override string Serialize()
	{
		StringBuilder stringBuilder = new StringBuilder();
		this.originalPos = this.mTrans.localPosition;
		this.originalRotation = this.mTrans.localEulerAngles;
		this.other.originalPos = this.other.mTrans.localPosition;
		this.other.originalRotation = this.other.mTrans.localEulerAngles;
		stringBuilder.Append("\"r1\":" + this.OriginalRotation.z.ToString() + ",");
		stringBuilder.Append("\"px1\":" + this.originalPos.x.ToString() + ",");
		stringBuilder.Append("\"py1\":" + this.originalPos.y.ToString() + ",");
		stringBuilder.Append("\"r2\":" + this.other.OriginalRotation.z.ToString() + ",");
		stringBuilder.Append("\"px2\":" + this.other.originalPos.x.ToString() + ",");
		stringBuilder.Append("\"py2\":" + this.other.originalPos.y.ToString() + ",");
		stringBuilder.Append("\"prefabType\":\"" + this.prefabType + "\",");
		stringBuilder.Append("\"type\":\"" + base.GetType().Name + "\"");
		return stringBuilder.ToString();
	}

	public override void Deserialization(JsonData json)
	{
		this.originalPos = new Vector2(float.Parse(json["px1"].ToString()), float.Parse(json["py1"].ToString()));
		this.originalRotation = new Vector3(0f, 0f, float.Parse(json["r1"].ToString()));
		this.other.originalPos = new Vector2(float.Parse(json["px2"].ToString()), float.Parse(json["py2"].ToString()));
		this.other.originalRotation = new Vector3(0f, 0f, float.Parse(json["r2"].ToString()));
		this.SetTransform();
		this.other.SetTransform();
	}

	public void DeleOther()
	{
		if (this.other != null)
		{
			LevelStage.CurEditStageInst.RemoveObjWithEditor(this.other);
			UnityEngine.Object.Destroy(this.other.gameObject);
		}
	}
}
