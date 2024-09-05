using LitJson;
using System;
using System.Text;
using UnityEngine;

public class Primitives : MonoBehaviour
{
	protected Vector3 originalPos = Vector3.zero;

	protected Vector3 originalRotation = Vector3.zero;

	protected Vector3 originalScale = Vector3.one;

	public string prefabType = string.Empty;

	[HideInInspector]
	public Transform mTrans;

	public float minScale = 1f;

	[HideInInspector]
	public JsonData Json;

	private Transform showTrans;

	[HideInInspector]
	public Rigidbody2D rigidbody2D;

	protected bool isSleep;

	protected Vector3 size = Vector3.zero;

	private Vector3 screenPostion = Vector3.zero;

	private Vector2 startVector = Vector3.zero;

	private float startAngle;

	public virtual Vector3 OriginalPos
	{
		get
		{
			return this.originalPos;
		}
	}

	public virtual Vector3 OriginalRotation
	{
		get
		{
			return this.originalRotation;
		}
		set
		{
			this.originalRotation = value;
		}
	}

	public virtual Vector3 OriginalScale
	{
		get
		{
			return this.originalScale;
		}
	}

	protected virtual void Awake()
	{
		this.mTrans = base.transform;
		this.showTrans = this.mTrans.Find("Shadow");
		EditObjData component = base.GetComponent<EditObjData>();
		if (component)
		{
			this.originalScale = component.DefaultScale;
		}
		this.rigidbody2D = base.GetComponent<Rigidbody2D>();
	}

	public virtual void Deserialization(JsonData json)
	{
		this.originalPos = new Vector2(float.Parse(json["px"].ToString()), float.Parse(json["py"].ToString()));
		this.originalScale = new Vector3(float.Parse(json["sx"].ToString()), float.Parse(json["sy"].ToString()), 1f);
		this.originalRotation = new Vector3(0f, 0f, float.Parse(json["r"].ToString()));
		this.SetTransform();
	}

	public virtual string Serialize()
	{
		StringBuilder stringBuilder = new StringBuilder();
		this.originalPos = this.mTrans.localPosition;
		this.originalRotation = this.mTrans.localEulerAngles;
		SpriteRenderer component = this.mTrans.GetComponent<SpriteRenderer>();
		if (component != null && component.drawMode != SpriteDrawMode.Simple)
		{
			this.originalScale = component.size;
		}
		else
		{
			this.originalScale = this.mTrans.localScale;
		}
		stringBuilder.Append("\"sx\":" + this.originalScale.x + ",");
		stringBuilder.Append("\"sy\":" + this.originalScale.y.ToString() + ",");
		stringBuilder.Append("\"r\":" + this.OriginalRotation.z.ToString() + ",");
		stringBuilder.Append("\"px\":" + this.originalPos.x.ToString() + ",");
		stringBuilder.Append("\"py\":" + this.originalPos.y.ToString() + ",");
		stringBuilder.Append("\"prefabType\":\"" + this.prefabType + "\",");
		stringBuilder.Append("\"type\":\"" + base.GetType().Name + "\"");
		return stringBuilder.ToString();
	}

	public virtual void SetTransform()
	{
		this.mTrans.localPosition = this.OriginalPos;
		this.mTrans.localRotation = Quaternion.Euler(this.OriginalRotation);
		SpriteRenderer component = this.mTrans.GetComponent<SpriteRenderer>();
		if (component != null && component.drawMode != SpriteDrawMode.Simple)
		{
			component.size = this.originalScale;
			if (this.showTrans != null)
			{
				this.showTrans.GetComponent<SpriteRenderer>().size = this.originalScale + new Vector3(0.16f, 0.16f);
			}
			Collider2D component2 = this.mTrans.GetComponent<Collider2D>();
			if (component2 != null)
			{
				if (component2 is BoxCollider2D)
				{
					((BoxCollider2D)component2).size = this.originalScale + ((!(this.showTrans == null)) ? new Vector3(0.16f, 0.16f) : Vector3.zero);
				}
				else if (component2 is CircleCollider2D)
				{
					((CircleCollider2D)component2).radius = this.originalScale.x;
				}
			}
		}
		else
		{
			this.mTrans.localScale = this.originalScale;
		}
	}

	public virtual void SetOriginalPos(Vector3 pos)
	{
		this.originalPos = pos;
		this.mTrans.localPosition = this.OriginalPos;
	}

	public virtual string GetJson()
	{
		return "{" + this.Serialize() + "}";
	}

	public virtual void Sleep()
	{
		this.isSleep = true;
		Rigidbody2D[] componentsInChildren = this.mTrans.GetComponentsInChildren<Rigidbody2D>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].bodyType = RigidbodyType2D.Static;
		}
	}

	public virtual void CopyDeserialization(Vector3 pos, Vector3 scale, Vector3 rotation)
	{
		this.Deserialization(this.Json);
		this.originalPos = pos;
		this.originalRotation = rotation;
		this.originalScale = scale;
		this.SetTransform();
	}

	protected float GetXGrowX()
	{
		if (base.transform.rotation == Quaternion.identity)
		{
			return 1f;
		}
		return Mathf.Cos(base.transform.rotation.eulerAngles.z * 0.0174532924f);
	}

	protected float GetXGrowY()
	{
		if (base.transform.rotation == Quaternion.identity)
		{
			return 0f;
		}
		return Mathf.Sin(base.transform.rotation.eulerAngles.z * 0.0174532924f);
	}

	protected float GetYGrowX()
	{
		if (base.transform.rotation == Quaternion.identity)
		{
			return 0f;
		}
		return Mathf.Sin(base.transform.rotation.eulerAngles.z * 0.0174532924f);
	}

	protected float GetYGrowY()
	{
		if (base.transform.rotation == Quaternion.identity)
		{
			return 1f;
		}
		return Mathf.Cos(base.transform.rotation.eulerAngles.z * 0.0174532924f);
	}

	public virtual void DragLeft(float x)
	{
		if (this.originalScale.x - this.minScale < 0f && x < 0f)
		{
			return;
		}
		float num = this.originalScale.x + x;
		if (num < this.minScale)
		{
			x = this.minScale - this.originalScale.x;
			num = this.minScale;
		}
		float num2 = x * -1f / 2f * this.GetXGrowX();
		this.size.y = this.originalPos.y - x / 2f * this.GetXGrowY();
		this.size.z = this.originalPos.z;
		this.size.x = this.originalPos.x + num2;
		this.originalPos = this.size;
		this.size.y = this.originalScale.y;
		this.size.z = this.originalScale.z;
		this.size.x = num;
		EditObjData component = this.mTrans.GetComponent<EditObjData>();
		if (component != null && component.sizeTransform == SizeTransform.Symmetrical)
		{
			this.size.y = this.size.x;
		}
		this.originalScale = this.size;
		this.SetTransform();
	}

	public virtual void DragRight(float x)
	{
		if (this.originalScale.x - this.minScale < 0f && x < 0f)
		{
			return;
		}
		float num = this.originalScale.x + x;
		if (num < this.minScale)
		{
			x = this.minScale - this.originalScale.x;
			num = this.minScale;
		}
		float num2 = x / 2f * this.GetXGrowX();
		this.size.y = base.transform.localPosition.y + x / 2f * this.GetXGrowY();
		this.size.z = base.transform.localPosition.z;
		this.size.x = this.originalPos.x + num2;
		this.originalPos = this.size;
		this.size.y = this.originalScale.y;
		this.size.z = this.originalScale.z;
		this.size.x = num;
		EditObjData component = this.mTrans.GetComponent<EditObjData>();
		if (component != null && component.sizeTransform == SizeTransform.Symmetrical)
		{
			this.size.y = this.size.x;
		}
		this.originalScale = this.size;
		this.originalScale = this.size;
		this.SetTransform();
	}

	public virtual void DragUp(float y)
	{
		if (this.originalScale.y - this.minScale < 0f && y < 0f)
		{
			return;
		}
		float num = this.originalScale.y + y;
		if (num < this.minScale)
		{
			y = this.minScale - this.originalScale.y;
			num = this.minScale;
		}
		float num2 = y / 2f * this.GetYGrowY();
		this.size.x = this.originalPos.x - y / 2f * this.GetYGrowX();
		this.size.z = this.originalPos.z;
		this.size.y = this.originalPos.y + num2;
		this.originalPos = this.size;
		this.size.x = this.originalScale.x;
		this.size.z = this.originalScale.z;
		this.size.y = num;
		this.originalScale = this.size;
		this.SetTransform();
	}

	public virtual void DragDown(float y)
	{
		if (this.originalScale.y - this.minScale < 0f && y < 0f)
		{
			return;
		}
		float num = this.originalScale.y + y;
		if (num < this.minScale)
		{
			y = this.minScale - this.originalScale.y;
			num = this.minScale;
		}
		float num2 = y * -1f / 2f * this.GetYGrowY();
		this.size.x = this.originalPos.x + y / 2f * this.GetYGrowX();
		this.size.z = this.originalPos.z;
		this.size.y = this.originalPos.y + num2;
		this.originalPos = this.size;
		this.size.x = this.originalScale.x;
		this.size.z = this.originalScale.z;
		this.size.y = num;
		this.originalScale = this.size;
		this.SetTransform();
	}

	public virtual void StartRotate(Vector3 start)
	{
		this.startAngle = base.transform.rotation.eulerAngles.z;
		this.screenPostion = Camera.main.WorldToScreenPoint(base.transform.position);
		this.startVector = this.screenPostion - start;
	}

	public virtual void OnRotate(Vector3 end)
	{
		Vector2 vector = this.screenPostion - end;
		float num = Vector2.Angle(this.startVector, vector);
		num *= Mathf.Sign(Vector3.Dot(Vector3.Cross(this.startVector, vector).normalized, base.transform.forward));
		num = ((num > 0f) ? num : (360f + num));
		Vector3 vector2 = new Vector3(0f, 0f, this.startAngle + num);
		this.originalRotation = vector2;
		this.SetTransform();
	}
}
