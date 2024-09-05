using LitJson;
using System;
using System.Text;
using UnityEngine;

public class Conveyor : Primitives
{
	private Transform leftCircle;

	private Transform rightCircle;

	private SpriteRenderer arrow;

	private BoxCollider2D topCol;

	private BoxCollider2D bottomCol;

	private float _power = 1f;

	private bool _isright = true;

	private Vector3 rotateVec = default(Vector3);

	private float circleSpeed
	{
		get
		{
			return this._power * (float)((!this._isright) ? 1 : (-1)) * 16f;
		}
	}

	public float power
	{
		get
		{
			return this._power;
		}
		set
		{
			this._power = value;
		}
	}

	public bool IsRight
	{
		get
		{
			return this._isright;
		}
		set
		{
			this._isright = value;
			this.arrow.flipX = !this.IsRight;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.leftCircle = this.mTrans.Find("circleleft");
		this.rightCircle = this.mTrans.Find("circleright");
		this.arrow = Utils.Find<SpriteRenderer>(this.mTrans, "arrow");
		this.topCol = Utils.Find<BoxCollider2D>(this.mTrans, "TopCollider");
		this.bottomCol = Utils.Find<BoxCollider2D>(this.mTrans, "BottomCollider");
	}

	public override string Serialize()
	{
		StringBuilder stringBuilder = new StringBuilder(base.Serialize());
		stringBuilder.Append(",");
		stringBuilder.Append("\"power\":" + this.power);
		stringBuilder.Append(",");
		stringBuilder.Append("\"isright\":" + ((!this.IsRight) ? "1" : "0"));
		return stringBuilder.ToString();
	}

	public override void Deserialization(JsonData json)
	{
		float power = 0f;
		float.TryParse(json["power"].ToString(), out power);
		this.power = power;
		this.IsRight = (json["isright"].ToString() == "0");
		base.Deserialization(json);
		ConveyorCollider conveyorCollider = this.topCol.gameObject.AddComponent<ConveyorCollider>();
		conveyorCollider.conveyor = this;
		conveyorCollider.Top = true;
		ConveyorCollider conveyorCollider2 = this.bottomCol.gameObject.AddComponent<ConveyorCollider>();
		conveyorCollider2.conveyor = this;
		conveyorCollider2.Top = false;
	}

	public override void SetTransform()
	{
		base.SetTransform();
		this.leftCircle.localPosition = new Vector3(-this.originalScale.x / 2f, 0f, 0f);
		this.rightCircle.localPosition = new Vector3(this.originalScale.x / 2f, 0f, 0f);
		Vector2 size = this.arrow.size;
		size.x = this.originalScale.x - 1f;
		this.arrow.size = size;
		size = this.topCol.size;
		size.x = this.originalScale.x;
		this.topCol.size = size;
		this.bottomCol.size = size;
	}

	private void Update()
	{
		this.rotateVec.z = this.rotateVec.z + this.circleSpeed;
		this.leftCircle.rotation = Quaternion.Euler(this.rotateVec);
		this.rightCircle.rotation = Quaternion.Euler(this.rotateVec);
	}
}
