using DG.Tweening;
using LitJson;
using System;
using System.Text;
using UnityEngine;

public class TriggerObj : Primitives
{
	public Primitives needTriggerObj;

	public override string Serialize()
	{
		StringBuilder stringBuilder = new StringBuilder(base.Serialize());
		stringBuilder.Append(",");
		stringBuilder.Append("\"Other\":" + this.needTriggerObj.GetJson());
		return stringBuilder.ToString();
	}

	public override void Deserialization(JsonData json)
	{
		base.Deserialization(json);
		this.needTriggerObj.Deserialization(json["Other"]);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (this.needTriggerObj)
		{
			this.needTriggerObj.mTrans.DOScaleX(0f, 0.3f).OnComplete(delegate
			{
				UnityEngine.Object.Destroy(base.gameObject);
				UnityEngine.Object.Destroy(this.needTriggerObj.gameObject);
			});
			base.gameObject.SetActive(false);
		}
	}

	public void DeleOther()
	{
		if (this.needTriggerObj)
		{
			UnityEngine.Object.Destroy(this.needTriggerObj.gameObject);
		}
	}
}
