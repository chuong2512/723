using LitJson;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractObj : Primitives
{
	public float mass;

	private bool canInteract;

	public bool hintClick;



	public Rope rope;

	public event Action<Vector2> ObjClickEvent;

	public bool CanInteract
	{
		get
		{
			return this.canInteract;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		if (this.rigidbody2D != null)
		{
			this.mass = this.rigidbody2D.mass;
		}
	}

	public override string Serialize()
	{
		string arg = base.Serialize() + ",";
		return arg + "\"mass\":" + this.mass;
	}

	public override void Deserialization(JsonData json)
	{
		base.Deserialization(json);
		if (json.Keys.Contains("mass"))
		{
			this.mass = float.Parse(json["mass"].ToString());
			if (this.rigidbody2D != null)
			{
				this.rigidbody2D.mass = this.mass;
			}
		}
		if (!this.isSleep)
		{
			EditObjData component = base.GetComponent<EditObjData>();
			if (component.ObjData.IsNullOrEntry())
			{
				this.canInteract = true;
				UGUIEvent uGUIEvent = this.mTrans.gameObject.AddComponent<UGUIEvent>();
				uGUIEvent.onPointerClick += new Action<PointerEventData, UGUIEvent>(this.OnClick);
			}
		}
	}

	protected virtual void OnClick(PointerEventData d, UGUIEvent sender)
	{
		if (this.hintClick)
		{
			return;
		}
		if (this.ObjClickEvent != null)
		{
			this.ObjClickEvent(this.mTrans.position);
		}
		this.Break();
	}

	public virtual void Break()
	{
		if (this.rope != null)
		{
			UnityEngine.Object.Destroy(this.rope.gameObject);
			this.rope = null;
		}
		UnityEngine.Object.Destroy(this.mTrans.gameObject);
		if (LevelStage.CurStageInst != null && LevelStage.CurStageInst.gameObject != null)
		{
			LevelStage.CurStageInst.loadedObj.Remove(this);
			Common.PlaySoundEffect("SFX:pop");
			Transform transform = UnityEngine.Object.Instantiate<Transform>(Resources.Load<Transform>("Particles/dianji/dianji"));
			transform.parent = LevelStage.CurStageInst.GameObjParent;
			transform.localPosition = this.mTrans.localPosition;
			Common.PlayVibration(2);
		}
	}
}
