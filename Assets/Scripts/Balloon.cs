using LitJson;
using System;
using System.Text;
using UnityEngine;

public class Balloon : InteractObj
{
	private Rigidbody2D rig;

	protected override void Awake()
	{
		base.Awake();
		this.rig = base.GetComponent<Rigidbody2D>();
	}

	public override string Serialize()
	{
		StringBuilder stringBuilder = new StringBuilder(base.Serialize());
		stringBuilder.Append(",");
		stringBuilder.Append("\"mass\":" + this.mass);
		return stringBuilder.ToString();
	}

	public override void Deserialization(JsonData json)
	{
		base.Deserialization(json);
		this.mass = float.Parse(json["mass"].ToString());
		this.rig.mass = this.mass;
	}
}
