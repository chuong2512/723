using System;
using UnityEngine;

public class DynamicParticle : MonoBehaviour
{
	public enum STATES
	{
		WATER,
		GAS,
		LAVA,
		NONE
	}

	private DynamicParticle.STATES currentState = DynamicParticle.STATES.NONE;

	public SpriteRenderer particleImage;

	public Color waterColor;

	public Color gasColor;

	public Color lavaColor;

	private float GAS_FLOATABILITY = 7f;

	private float particleLifeTime = 3f;

	private float startTime;

	public float forceMagnitude = 1f;

	public float attractiveForceDis = 0.3f;

	private Transform mTrans;

	private Rigidbody2D rig;

	private void Awake()
	{
		this.mTrans = base.transform;
		this.rig = base.GetComponent<Rigidbody2D>();
		if (this.currentState == DynamicParticle.STATES.NONE)
		{
			this.SetState(DynamicParticle.STATES.WATER);
		}
	}

	public void SetState(DynamicParticle.STATES newState)
	{
		if (newState != this.currentState)
		{
			this.currentState = newState;
			this.startTime = Time.time;
			base.GetComponent<Rigidbody2D>().velocity = default(Vector2);
			switch (newState)
			{
			case DynamicParticle.STATES.WATER:
				this.particleImage.color = this.waterColor;
				base.GetComponent<Rigidbody2D>().gravityScale = 1f;
				break;
			case DynamicParticle.STATES.GAS:
				this.particleImage.color = this.gasColor;
				this.particleLifeTime /= 2f;
				base.GetComponent<Rigidbody2D>().gravityScale = 0f;
				base.gameObject.layer = LayerMask.NameToLayer("Gas");
				break;
			case DynamicParticle.STATES.LAVA:
				this.particleImage.color = this.lavaColor;
				base.GetComponent<Rigidbody2D>().gravityScale = 0.3f;
				break;
			case DynamicParticle.STATES.NONE:
				UnityEngine.Object.Destroy(base.gameObject);
				break;
			}
		}
	}

	private void Update()
	{
		if (!this.IsVisual())
		{
			ParticleGenerator.Inst.RemoveWaterPartical(base.gameObject);
		}
	}

	private bool IsVisual()
	{
		return this.mTrans.position.x < PBase.ScreenRightUp.x && this.mTrans.position.x > PBase.ScreenLeftDown.x && this.mTrans.position.y > PBase.ScreenLeftDown.y && this.mTrans.position.y < PBase.ScreenRightUp.y;
	}

	private void MovementAnimation()
	{
		Vector3 localScale = new Vector3(1f, 1f, 1f);
		localScale.x += Mathf.Abs(base.GetComponent<Rigidbody2D>().velocity.x) / 30f;
		localScale.z += Mathf.Abs(base.GetComponent<Rigidbody2D>().velocity.y) / 30f;
		localScale.y = 1f;
		this.particleImage.gameObject.transform.localScale = localScale;
	}

	private void ScaleDown()
	{
		float num = 1f - (Time.time - this.startTime) / this.particleLifeTime;
		Vector2 one = Vector2.one;
		if (num <= 0f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			one.x = num;
			one.y = num;
			base.transform.localScale = one;
		}
	}

	public void SetLifeTime(float time)
	{
		this.particleLifeTime = time;
	}
}
