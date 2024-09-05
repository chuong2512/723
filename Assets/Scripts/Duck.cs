using System;
using UnityEngine;

public class Duck : Primitives
{
	public float waterForce = 1f;

	private Vector2 centerOfMass = new Vector2(0f, -0.5f);

	private Collider2D selfCol;

	private Collider2D triggerColler;

	private ContactFilter2D filter2D;

	private Collider2D[] waterCols;

	private int colsCount;

	private Animator animator;

	private Vector2 forceVec = Vector2.up;

	protected override void Awake()
	{
		base.Awake();
		this.animator = base.GetComponent<Animator>();
		this.selfCol = base.GetComponent<Collider2D>();
		this.rigidbody2D.centerOfMass = this.centerOfMass;
		this.triggerColler = Utils.Find<Collider2D>(this.mTrans, "Trigger");
		this.filter2D = default(ContactFilter2D);
		this.filter2D.SetLayerMask(LayerMask.GetMask(new string[]
		{
			"Box"
		}));
		this.waterCols = new Collider2D[10];
		if (LevelStage.CurStageInst != null)
		{
			LevelStage.CurStageInst.SuccessEvent += new Action(this.SuccessEvent);
		}
	}

	private void FixedUpdate()
	{
		if (this.isSleep)
		{
			return;
		}
		this.colsCount = this.triggerColler.OverlapCollider(this.filter2D, this.waterCols);
		if (this.colsCount > 0)
		{
			this.forceVec.y = (float)this.colsCount * this.waterForce;
			if (this.colsCount >= 10)
			{
				this.rigidbody2D.gravityScale = 0f;
				if (this.rigidbody2D.velocity.magnitude < 0.1f && this.waterCols[0].GetComponent<Rigidbody2D>().velocity.magnitude < 1f)
				{
					if (this.selfCol.enabled)
					{
						this.selfCol.enabled = false;
					}
					this.mTrans.position += new Vector3(0f, 2f * Time.fixedDeltaTime);
					return;
				}
			}
			if (this.rigidbody2D.gravityScale != 1f)
			{
				if (!this.selfCol.enabled)
				{
					this.selfCol.enabled = true;
				}
				this.rigidbody2D.gravityScale = 1f;
			}
			this.rigidbody2D.AddForce(this.forceVec);
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (LevelStage.CurStageInst != null && LevelStage.CurStageInst.gameState == LevelStage.GameState.Starting && other.gameObject.tag != "DynamicParticle" && this.rigidbody2D.velocity.magnitude > 1f)
		{
			Common.PlaySoundEffect("SFX:ga");
			this.animator.Play("DuckCollosion");
		}
	}

	private void Update()
	{
		if (!this.isSleep && LevelStage.CurStageInst != null && LevelStage.CurStageInst.gameState == LevelStage.GameState.Starting && !this.IsVisual())
		{
			LevelStage.CurStageInst.gameState = LevelStage.GameState.Fail;
		}
	}

	private void SuccessEvent()
	{
		this.animator.Play("DuckSucess");
	}

	private bool IsVisual()
	{
		return this.mTrans.position.x < PBase.ScreenRightUp.x && this.mTrans.position.x > PBase.ScreenLeftDown.x && this.mTrans.position.y > PBase.ScreenLeftDown.y && this.mTrans.position.y < PBase.ScreenRightUp.y;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(base.transform.TransformPoint(this.centerOfMass), 0.1f);
	}
}
