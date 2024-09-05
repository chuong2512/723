using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Rope : Primitives
{
	private LineRenderer line;

	public Transform start;

	public Transform end;

	public bool isPole;

	private float PreDis = 0.3f;

	private PhysicMaterial material;

	private bool startCanBind = true;

	private bool endCanBind = true;

	private Transform editCollider;

	private List<Rigidbody2D> points = new List<Rigidbody2D>();

	protected override void Awake()
	{
		base.Awake();
		this.start = this.mTrans.Find("Start");
		this.end = this.mTrans.Find("End");
		this.line = Utils.Find<LineRenderer>(base.transform, "Line");
	}

	public Vector3 GetFristPostion()
	{
		return this.line.GetPosition(this.line.positionCount - 1);
	}

	public Vector3 GetLastPostion()
	{
		return this.line.GetPosition(0);
	}

	public void DrawRope()
	{
		float num = 1f;
		if (!this.isPole)
		{
			float num2 = Vector2.Distance(this.start.localPosition, this.end.localPosition);
			num = num2 / this.PreDis;
		}
		this.points.Add(this.start.GetComponent<Rigidbody2D>());
		int num3 = 1;
		while ((float)num3 < num)
		{
			GameObject gameObject = new GameObject("rope_" + num3);
			gameObject.layer = LayerMask.NameToLayer("Rope");
			Vector2 a = (this.end.localPosition - this.start.localPosition).normalized;
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = this.start.localPosition + (Vector3)a * this.PreDis * (float)num3;
			Rigidbody2D rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
			rigidbody2D.mass = 0.2f;
			rigidbody2D.gravityScale = 0f;
			HingeJoint2D hingeJoint2D = gameObject.AddComponent<HingeJoint2D>();
			this.points.Add(rigidbody2D);
			hingeJoint2D.connectedBody = this.points[num3 - 1];
			num3++;
		}
		this.points.Add(this.end.GetComponent<Rigidbody2D>());
		this.points[this.points.Count - 1].GetComponent<HingeJoint2D>().connectedBody = this.points[this.points.Count - 2];
		this.line.material.mainTextureScale = new Vector2(num, 1f);
		this.LineRenderDraw();
	}

	private void LineRenderDraw()
	{
		this.line.positionCount = this.points.Count;
		for (int i = 0; i < this.points.Count; i++)
		{
			this.line.SetPosition(i, this.points[i].transform.localPosition);
		}
	}

	private void Update()
	{
		if (this.isSleep)
		{
			this.line.positionCount = 2;
			this.line.SetPosition(0, this.start.localPosition);
			this.line.SetPosition(1, this.end.localPosition);
			this.SetEditCollider();
		}
		else
		{
			this.LineRenderDraw();
		}
	}

	private void SetEditCollider()
	{
		if (this.editCollider == null)
		{
			GameObject gameObject = new GameObject("editColider");
			gameObject.AddComponent<BoxCollider2D>();
			this.editCollider = gameObject.transform;
			this.editCollider.parent = this.mTrans;
		}
		this.editCollider.localPosition = (this.start.localPosition + this.end.localPosition) / 2f;
		this.editCollider.right = this.mTrans.localRotation * (this.end.localPosition - this.start.localPosition).normalized;
		this.editCollider.localScale = new Vector3(Vector2.Distance(this.start.localPosition, this.end.localPosition), 0.5f, 1f);
	}

	private void StartPointRopeBind(Collider2D col)
	{
		if (!this.startCanBind)
		{
			return;
		}
		if (this.RopeBind(this.start, col))
		{
			this.startCanBind = false;
			this.start.GetComponent<Collider2D>().enabled = false;
		}
	}

	private void EndPointRopeBind(Collider2D col)
	{
		if (!this.endCanBind)
		{
			return;
		}
		if (this.RopeBind(this.end, col))
		{
			this.endCanBind = false;
			this.end.GetComponent<Collider2D>().enabled = false;
		}
	}

	private bool RopeBind(Transform point, Collider2D col)
	{
		if (col.tag == "Rope")
		{
			return false;
		}
		Rigidbody2D attachedRigidbody = col.attachedRigidbody;
		if (attachedRigidbody)
		{
			HingeJoint2D hingeJoint2D = point.GetComponent<HingeJoint2D>();
			if (hingeJoint2D && hingeJoint2D.connectedBody != null)
			{
				hingeJoint2D = point.gameObject.AddComponent<HingeJoint2D>();
			}
			hingeJoint2D.connectedBody = attachedRigidbody;
			Primitives primitives = attachedRigidbody.GetComponent<Primitives>();
			if (primitives == null)
			{
				primitives = attachedRigidbody.transform.GetComponentInParent<Primitives>();
			}
			if (primitives != null && primitives is InteractObj)
			{
				((InteractObj)primitives).rope = this;
			}
			return true;
		}
		return false;
	}

	public override void Deserialization(JsonData json)
	{
		base.Deserialization(json);
		this.isPole = (int.Parse(json["isPole"].ToString()) == 1);
		string[] array = json["vec"].ToString().Split(new char[]
		{
			'|'
		});
		if (array.Length != 4)
		{
			return;
		}
		Vector2 v;
		v.x = float.Parse(array[0]);
		v.y = float.Parse(array[1]);
		Vector2 v2;
		v2.x = float.Parse(array[2]);
		v2.y = float.Parse(array[3]);
		this.start.localPosition = v;
		this.end.localPosition = v2;
		this.DrawRope();
		this.start.GetComponent<ColliderTrigger>().TriggerEnter += new Action<Collider2D>(this.StartPointRopeBind);
		this.end.GetComponent<ColliderTrigger>().TriggerEnter += new Action<Collider2D>(this.EndPointRopeBind);
		if (!this.isSleep)
		{
			this.DelayToDo(1f, delegate
			{
				this.end.GetComponent<Collider2D>().enabled = false;
				this.start.GetComponent<Collider2D>().enabled = false;
			}, false);
		}
	}

	public override string Serialize()
	{
		string text = base.Serialize();
		text += ",";
		text = text + "\"isPole\":" + ((!this.isPole) ? 0 : 1);
		text += ",";
		string text2 = string.Concat(new object[]
		{
			this.start.localPosition.x,
			"|",
			this.start.localPosition.y,
			"|"
		});
		string text3 = text2;
		text2 = string.Concat(new object[]
		{
			text3,
			this.end.localPosition.x,
			"|",
			this.end.localPosition.y
		});
		return text + "\"vec\":\"" + text2 + "\"";
	}

	private void OnDestroy()
	{
		this.line = null;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		for (int i = 0; i < this.points.Count; i++)
		{
			Gizmos.DrawRay(this.points[i].transform.position, Vector3.right);
		}
	}
}
