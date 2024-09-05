using LitJson;
using System;
using System.Text;
using UnityEngine;

public class Teeterboard : Primitives
{
	public enum Achor
	{
		Left,
		Mid,
		Right
	}

	private Teeterboard.Achor _achor = Teeterboard.Achor.Mid;

	protected Transform achorTrans;

	public Teeterboard.Achor achor
	{
		get
		{
			return this._achor;
		}
		set
		{
			this._achor = value;
			this.SetAchor();
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.achorTrans = this.mTrans.Find("Achor");
	}

	public override void Deserialization(JsonData json)
	{
		this._achor = (Teeterboard.Achor)int.Parse(json["achor"].ToString());
		base.Deserialization(json);
	}

	public override string Serialize()
	{
		StringBuilder stringBuilder = new StringBuilder(base.Serialize());
		stringBuilder.Append(",");
		stringBuilder.Append("\"achor\":" + (int)this.achor);
		return stringBuilder.ToString();
	}

	public override void SetTransform()
	{
		base.SetTransform();
		this.SetAchor();
	}

	public void SetAchor()
	{
		Teeterboard.Achor achor = this.achor;
		if (achor != Teeterboard.Achor.Left)
		{
			if (achor != Teeterboard.Achor.Mid)
			{
				if (achor == Teeterboard.Achor.Right)
				{
					this.achorTrans.localPosition = new Vector3(this.originalScale.x / 2f, 0f, 0f);
				}
			}
			else
			{
				this.achorTrans.localPosition = new Vector3(0f, 0f, 0f);
			}
		}
		else
		{
			this.achorTrans.localPosition = new Vector3(-this.originalScale.x / 2f, 0f, 0f);
		}
	}
}
