using LitJson;
using System;
using System.Text;
using UnityEngine;

public class HintObj : Primitives
{
	public string hintStrKey = "hint";

	public Transform textTrans;

	private TextMesh textMesh;

	protected override void Awake()
	{
		base.Awake();
		this.textTrans = this.mTrans.Find("Text");
		this.textMesh = this.textTrans.GetComponent<TextMesh>();
	}

	public override string Serialize()
	{
		StringBuilder stringBuilder = new StringBuilder(base.Serialize());
		stringBuilder.Append(",");
		stringBuilder.Append("\"hintStrKey\":\"" + this.hintStrKey + "\"");
		stringBuilder.Append(",");
		string str = this.textTrans.localPosition.x + "|" + this.textTrans.localPosition.y;
		stringBuilder.Append("\"point\":\"" + str + "\"");
		return stringBuilder.ToString();
	}

	public override void Deserialization(JsonData json)
	{
		this.hintStrKey = json["hintStrKey"].ToString();
		if (json.Keys.Contains("point"))
		{
			string[] array = json["point"].ToString().Split(new char[]
			{
				'|'
			});
			Vector2 v;
			v.x = float.Parse(array[0]);
			v.y = float.Parse(array[1]);
			this.textTrans.localPosition = v;
		}
		this.ShowString();
		base.Deserialization(json);
	}

	public void ShowString()
	{
		this.textMesh.text = Utils.GetString(this.hintStrKey, new object[0]);
		this.textMesh.transform.rotation = Quaternion.identity;
	}

	public override void SetTransform()
	{
		base.SetTransform();
		this.textMesh.transform.rotation = Quaternion.identity;
	}

	public override void Sleep()
	{
		base.Sleep();
		base.gameObject.AddComponent<BoxCollider2D>();
	}
}
