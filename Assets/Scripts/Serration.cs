using LitJson;
using System;
using UnityEngine;

public class Serration : Primitives
{
	private int _width = 1;

	public bool needBind;

	private float bindRadius = 1f;

	private Transform spikeTrans;

	private Transform bindTrans;

	private Rigidbody2D rig;

	public int width
	{
		get
		{
			return this._width;
		}
		set
		{
			this._width = Mathf.Max(1, value);
			this.SetWidth();
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.spikeTrans = this.mTrans.Find("spike_0");
		this.rig = base.GetComponent<Rigidbody2D>();
	}

	public override string Serialize()
	{
		string text = base.Serialize() + ",";
		text = text + "\"width\":" + this.width;
		text += ",";
		return text + "\"bind\":" + ((!this.needBind) ? 0 : 1).ToString();
	}

	public override void Deserialization(JsonData json)
	{
		base.Deserialization(json);
		this.width = int.Parse(json["width"].ToString());
		this.needBind = (int.Parse(json["bind"].ToString()) == 1);
	}

	public void SetWidth()
	{
		if (this.spikeTrans == null)
		{
			return;
		}
		int childCount = this.mTrans.childCount;
		int i = 1;
		int num = 1;
		while (i < Mathf.Max(this.width, childCount))
		{
			Transform transform = this.mTrans.Find("spike_" + num);
			if (transform != null)
			{
				num++;
				if (num > this.width)
				{
					UnityEngine.Object.Destroy(transform.gameObject);
				}
			}
			else if (num < this.width)
			{
				UnityEngine.Object.Instantiate<Transform>(this.spikeTrans, this.mTrans).name = "spike_" + num;
				num++;
			}
			i++;
		}
		for (int j = 0; j < this.width; j++)
		{
			this.mTrans.Find("spike_" + j).localPosition = new Vector3(0.5f + (float)(-(float)this.width) / 2f + 1f * (float)j, 0f, 0f);
		}
	}

	private void Start()
	{
		if (!this.isSleep && this.needBind)
		{
			RaycastHit2D raycastHit2D = Physics2D.Raycast(this.mTrans.position, -this.mTrans.up, 1f, ~LayerMask.GetMask(new string[]
			{
				"Serration"
			}));
			if (raycastHit2D.rigidbody != null)
			{
				FixedJoint2D fixedJoint2D = base.gameObject.AddComponent<FixedJoint2D>();
				fixedJoint2D.connectedBody = raycastHit2D.rigidbody;
				this.rig.bodyType = RigidbodyType2D.Dynamic;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Balloon")
		{
			Balloon component = collision.gameObject.GetComponent<Balloon>();
			component.Break();
		}
	}

	private void OnDrawGizmos()
	{
		if (this.needBind)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(this.mTrans.position, this.mTrans.position - this.mTrans.up);
		}
	}
}
