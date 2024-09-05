using LitJson;
using System;
using System.Text;
using UnityEngine;

public class Block : Teeterboard
{
	public float speed;

	public override void Deserialization(JsonData json)
	{
		base.Deserialization(json);
		if (json.Keys.Contains("speed"))
		{
			this.speed = float.Parse(json["speed"].ToString());
		}
		if (!this.isSleep)
		{
			this.rigidbody2D.angularVelocity = this.speed;
		}
	}

	public override string Serialize()
	{
		StringBuilder stringBuilder = new StringBuilder();
		this.originalPos = this.mTrans.localPosition;
		this.originalRotation = this.mTrans.localEulerAngles;
		SpriteRenderer component = this.achorTrans.GetComponent<SpriteRenderer>();
		if (component != null && component.drawMode == SpriteDrawMode.Sliced)
		{
			this.originalScale = component.size;
		}
		else
		{
			this.originalScale = this.achorTrans.localScale;
		}
		stringBuilder.Append("\"sx\":" + this.originalScale.x + ",");
		stringBuilder.Append("\"sy\":" + this.originalScale.y.ToString() + ",");
		stringBuilder.Append("\"r\":" + this.OriginalRotation.z.ToString() + ",");
		stringBuilder.Append("\"px\":" + this.originalPos.x.ToString() + ",");
		stringBuilder.Append("\"py\":" + this.originalPos.y.ToString() + ",");
		stringBuilder.Append("\"prefabType\":\"" + this.prefabType + "\",");
		stringBuilder.Append("\"type\":\"" + base.GetType().Name + "\"");
		stringBuilder.Append(",");
		stringBuilder.Append("\"achor\":" + (int)base.achor);
		stringBuilder.Append(",");
		stringBuilder.Append("\"speed\":" + this.speed);
		return stringBuilder.ToString();
	}

	public override void SetTransform()
	{
		this.mTrans.localPosition = this.OriginalPos;
		this.mTrans.localRotation = Quaternion.Euler(this.OriginalRotation);
		SpriteRenderer component = this.achorTrans.GetComponent<SpriteRenderer>();
		if (component != null && component.drawMode == SpriteDrawMode.Sliced)
		{
			component.size = this.originalScale;
			Collider2D component2 = this.achorTrans.GetComponent<Collider2D>();
			if (component2 != null)
			{
				if (component2 is BoxCollider2D)
				{
					((BoxCollider2D)component2).size = this.originalScale;
				}
				else if (component2 is CircleCollider2D)
				{
					((CircleCollider2D)component2).radius = this.originalScale.x;
				}
			}
		}
		else
		{
			this.mTrans.localScale = this.originalScale;
		}
		base.SetAchor();
	}
}
