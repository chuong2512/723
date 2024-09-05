using LitJson;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MoveCube : Primitives
{
	public float Speed = 1f;

	public List<Vector3> movelist = new List<Vector3>();

	private List<LineRenderer> linelist = new List<LineRenderer>();

	private Material lineMat;

	private int index = -1;

	private Vector3 startposition = Vector3.zero;

	private Vector3 targetpostion = Vector3.zero;

	private bool Isforward;

	private float lastDis = 9999f;

	private float nowDis;

	private Vector2 velocity = Vector2.zero;

	protected override void Awake()
	{
		base.Awake();
		this.lineMat = Resources.Load<Material>("Material/Line/moveLine");
	}

	public override void Deserialization(JsonData json)
	{
		base.Deserialization(json);
		this.movelist.Clear();
		this.Speed = float.Parse(json["speed"].ToString());
		string[] array = json["vec"].ToString().Split(new char[]
		{
			'|'
		});
		int i = 0;
		while (i < array.Length - 1)
		{
			Vector3 item = default(Vector3);
			item.x = float.Parse(array[i]);
			i++;
			item.y = float.Parse(array[i]);
			i++;
			this.movelist.Add(item);
		}
		this.DrawLine();
		if (this.movelist.Count >= 2)
		{
			this.index = this.movelist.Count - 1;
			float num = Vector2.Distance(this.mTrans.localPosition, this.movelist[this.movelist.Count - 1]);
			for (int j = this.movelist.Count - 2; j >= 0; j--)
			{
				if (Vector2.Distance(this.mTrans.position, this.movelist[j]) < num)
				{
					this.index = j;
				}
			}
			if (num == 0f)
			{
				this.lastDis = -1f;
			}
		}
		if (this.index != -1)
		{
			this.startposition = this.mTrans.localPosition;
			this.targetpostion = this.movelist[this.index];
			this.velocity = (this.targetpostion - this.startposition).normalized * this.Speed;
		}
	}

	public override string Serialize()
	{
		StringBuilder stringBuilder = new StringBuilder(base.Serialize());
		stringBuilder.Append(",");
		stringBuilder.Append("\"speed\":" + this.Speed + ",");
		string text = string.Empty;
		using (List<Vector3>.Enumerator enumerator = this.movelist.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Vector2 vector = enumerator.Current;
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					vector.x,
					"|",
					vector.y,
					"|"
				});
			}
		}
		stringBuilder.Append("\"vec\":\"" + text + "\"");
		return stringBuilder.ToString();
	}

	public void DrawLine()
	{
		this.Clear();
		this.CreateLine();
	}

	private void CreateLine()
	{
		int num = 0;
		Vector3 vector = Vector3.zero;
		foreach (Vector3 current in this.movelist)
		{
			if (num == 0)
			{
				vector = current;
			}
			else
			{
				LineRenderer component = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Line_fan"), this.mTrans).GetComponent<LineRenderer>();
				component.SetPosition(0, vector + Vector3.forward);
				component.SetPosition(1, current + Vector3.forward);
				float num2 = Vector3.Distance(current, vector);
				this.linelist.Add(component);
				component.receiveShadows = false;
				component.allowOcclusionWhenDynamic = false;
				vector = current;
				component.material.mainTextureScale = new Vector2(num2 / component.widthMultiplier, 1f);
			}
			num++;
		}
	}

	private void Clear()
	{
		foreach (LineRenderer current in this.linelist)
		{
			UnityEngine.Object.Destroy(current.gameObject);
		}
		this.linelist.Clear();
	}

	protected virtual void Update()
	{
		if (this.isSleep)
		{
			return;
		}
		if (this.movelist.Count < 2)
		{
			return;
		}
		this.nowDis = Vector3.Distance(this.mTrans.localPosition, this.targetpostion);
		if (this.nowDis <= this.lastDis)
		{
			this.lastDis = this.nowDis;
		}
		else
		{
			this.mTrans.localPosition = this.targetpostion;
			if (this.Isforward)
			{
				this.index++;
			}
			else
			{
				this.index--;
			}
			if (this.index >= this.movelist.Count)
			{
				this.index = this.movelist.Count - 1;
				if (this.movelist[this.index] == this.movelist[0])
				{
					this.index = 1;
				}
				else
				{
					this.index = this.movelist.Count - 2;
					this.Isforward = false;
				}
			}
			else if (this.index < 0)
			{
				this.Isforward = true;
				this.index = 1;
			}
			this.startposition = this.mTrans.localPosition;
			this.targetpostion = this.movelist[this.index];
			this.velocity = (this.targetpostion - this.startposition).normalized * this.Speed;
			this.lastDis = 9999f;
		}
		this.rigidbody2D.velocity = this.velocity;
	}

	public void SetVec(Vector3 vec)
	{
		this.movelist.Add(vec);
		this.DrawLine();
	}

	private void OnDestroy()
	{
		this.Clear();
	}
}
