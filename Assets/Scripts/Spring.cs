using LitJson;
using System;
using System.Text;
using UnityEngine;

public class Spring : Primitives
{
	public const float originalDis = 3f;

	private Transform topTrans;

	private Rigidbody2D topRig;

	private Transform springRendererTrans;

	private Transform topTrans2;

	private Rigidbody2D topRig2;

	private Transform springRendererTrans2;

	private Transform doubleSprite;

	private float _mass = 100f;

	private float _compressDis;

	public bool isWake = true;

	public bool isTrrige;

	private bool isDouble;

	public float Mass
	{
		get
		{
			return this._mass;
		}
		set
		{
			this._mass = value;
			if (this.topRig)
			{
				this.topRig.mass = this._mass;
			}
			if (this.isDouble)
			{
				this.topRig2.mass = this._mass;
			}
		}
	}

	public float CompressDis
	{
		get
		{
			return this._compressDis;
		}
		set
		{
			this._compressDis = value;
			this.SetCompressDis(this._compressDis);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.topTrans = this.mTrans.Find("Top");
		this.springRendererTrans = this.mTrans.Find("Spring");
		this.topRig = this.topTrans.GetComponent<Rigidbody2D>();
		EditObjData component = base.GetComponent<EditObjData>();
		if (component)
		{
			this.isDouble = component.ObjData.Equals("Double");
		}
		if (this.isDouble)
		{
			this.topTrans2 = this.mTrans.Find("Top2");
			this.springRendererTrans2 = this.mTrans.Find("Spring2");
			this.doubleSprite = this.mTrans.Find("Sprite");
			this.topRig2 = this.topTrans2.GetComponent<Rigidbody2D>();
		}
		this.CompressDis = 1f;
	}

	public override void Deserialization(JsonData json)
	{
		base.Deserialization(json);
		this.isTrrige = (int.Parse(json["isTrrige"].ToString()) == 1);
		this.Mass = float.Parse(json["mass"].ToString());
		this.CompressDis = float.Parse(json["compressDis"].ToString());
		if (this.isTrrige && !this.isDouble)
		{
			this.topRig.bodyType = RigidbodyType2D.Static;
			CollisionReceive collisionReceive = this.topTrans.gameObject.AddComponent<CollisionReceive>();
			collisionReceive.TriggerEnter += new Action<Collision2D>(this.TriggerEnter);
		}
	}

	public override void SetTransform()
	{
		if (this.isDouble)
		{
			this.originalScale = Vector3.one;
			this.originalRotation = Vector3.zero;
		}
		else
		{
			float num = (this.originalRotation.z % 360f + 360f) % 360f;
			if (num > 315f || num < 45f)
			{
				this.originalRotation.z = 0f;
				this.topRig.constraints = (RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation);
			}
			else if (num > 45f && num < 135f)
			{
				this.originalRotation.z = 90f;
				this.topRig.constraints = (RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation);
			}
			else if (num > 135f && num < 225f)
			{
				this.originalRotation.z = 180f;
				this.topRig.constraints = (RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation);
			}
			else if (num > 225f && num < 315f)
			{
				this.originalRotation.z = -90f;
				this.topRig.constraints = (RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation);
			}
		}
		base.SetTransform();
	}

	public override string Serialize()
	{
		StringBuilder stringBuilder = new StringBuilder(base.Serialize());
		stringBuilder.Append(",");
		stringBuilder.Append("\"isTrrige\":" + ((!this.isTrrige) ? 0 : 1));
		stringBuilder.Append(",");
		stringBuilder.Append("\"mass\":" + this.Mass);
		stringBuilder.Append(",");
		stringBuilder.Append("\"compressDis\":" + this._compressDis);
		return stringBuilder.ToString();
	}

	private void Update()
	{
		if (this.isWake)
		{
			this.SetSpringRender();
		}
	}

	private void SetSpringRender()
	{
		float y = this.topTrans.localPosition.y / 3f;
		Vector2 v = this.springRendererTrans.localScale;
		v.y = y;
		this.springRendererTrans.localScale = v;
		if (this.isDouble)
		{
			y = this.topTrans2.localPosition.y / 3f;
			v = this.springRendererTrans2.localScale;
			v.y = y;
			this.springRendererTrans2.localScale = v;
			v.x = 0f;
		}
	}

	private void TriggerEnter(Collision2D col)
	{
		this.topRig.bodyType = RigidbodyType2D.Dynamic;
	}

	private void SetCompressDis(float compressDis)
	{
		Vector2 v = this.topTrans.localPosition;
		v.y = 3f - compressDis;
		this.topTrans.localPosition = v;
		if (this.isDouble)
		{
			v.x = this.topTrans2.localPosition.x;
			this.topTrans2.localPosition = v;
			v.x = 0f;
			this.doubleSprite.localPosition = v;
		}
		this.SetSpringRender();
	}

	public override void Sleep()
	{
		base.Sleep();
		this.isWake = false;
	}
}
