using LitJson;
using System;
using System.Text;
using UnityEngine;

public class Fan : Primitives
{
	public float height = 10f;

	public float width = 3f;

	public float MaxForce = 20f;

	public float MinForce = 20f;

	private Transform regionTrans;

	private bool isWake = true;

	private LineRenderer _line;

	private ParticleSystem particle;

	private Transform fengSprite;

	private RaycastHit2D[] hit;

	public LineRenderer Line
	{
		get
		{
			if (this._line == null)
			{
				this._line = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Line_fan"), this.mTrans).GetComponent<LineRenderer>();
			}
			return this._line;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.regionTrans = this.mTrans.Find("region");
		this.particle = base.GetComponentInChildren<ParticleSystem>();
		this.fengSprite = this.mTrans.Find("feng1");
	}

	public void SetParameter(float height, float width, float MaxForce, float MinForce)
	{
		this.height = height;
		this.width = width;
		this.MaxForce = MaxForce;
		this.MinForce = MinForce;
	}

	public override void Deserialization(JsonData json)
	{
		this.height = float.Parse(json["height"].ToString());
		this.width = float.Parse(json["width"].ToString());
		this.MaxForce = float.Parse(json["MaxForce"].ToString());
		this.MinForce = float.Parse(json["MinForce"].ToString());
		base.Deserialization(json);
		this.SetParticleRegion();
	}

	private void SetParticleRegion()
	{
		this.particle.startLifetime = this.height / 10f;
		this.fengSprite.localPosition = new Vector3(0f, this.height / 2f, 0f);
		this.fengSprite.localScale = new Vector3(this.height * 0.15f, 1f, 1f);
	}

	public override string Serialize()
	{
		StringBuilder stringBuilder = new StringBuilder(base.Serialize());
		stringBuilder.Append(",");
		stringBuilder.Append("\"height\":" + this.height);
		stringBuilder.Append(",");
		stringBuilder.Append("\"width\":" + this.width);
		stringBuilder.Append(",");
		stringBuilder.Append("\"MaxForce\":" + this.MaxForce);
		stringBuilder.Append(",");
		stringBuilder.Append("\"MinForce\":" + this.MinForce);
		return stringBuilder.ToString();
	}

	public override void SetTransform()
	{
		base.SetTransform();
		if (this.isSleep)
		{
			this.DrawRegion();
		}
	}

	private void FixedUpdate()
	{
		if (!this.isWake)
		{
			return;
		}
		this.hit = Physics2D.BoxCastAll(this.mTrans.position, new Vector2(this.width / 2f, 0.1f), this.mTrans.eulerAngles.z, this.mTrans.up, this.height, ~LayerMask.GetMask(new string[]
		{
			"Rope"
		}));
		if (this.hit.Length > 0)
		{
			for (int i = 0; i < this.hit.Length; i++)
			{
				if (this.hit[i].rigidbody)
				{
					Vector2 vector = this.mTrans.worldToLocalMatrix.MultiplyPoint(this.hit[i].transform.position);
					float d = Mathf.Lerp(this.MaxForce, this.MinForce, vector.y / this.height);
					this.hit[i].rigidbody.AddForce(this.mTrans.up * d * this.hit[i].rigidbody.mass);
				}
			}
		}
	}

	public override void Sleep()
	{
		base.Sleep();
		this.isWake = false;
	}

	public void DrawRegion()
	{
		this.SetParticleRegion();
		this.Line.positionCount = 5;
		Vector3 position = this.mTrans.localToWorldMatrix.MultiplyPoint(new Vector3(this.width / 2f, 0.1f));
		Vector3 position2 = this.mTrans.localToWorldMatrix.MultiplyPoint(new Vector3(-this.width / 2f, 0.1f));
		Vector3 position3 = this.mTrans.localToWorldMatrix.MultiplyPoint(new Vector3(-this.width / 2f, this.height));
		Vector3 position4 = this.mTrans.localToWorldMatrix.MultiplyPoint(new Vector3(this.width / 2f, this.height));
		this.Line.SetPosition(0, position);
		this.Line.SetPosition(1, position2);
		this.Line.SetPosition(2, position3);
		this.Line.SetPosition(3, position4);
		this.Line.SetPosition(4, position);
	}
}
