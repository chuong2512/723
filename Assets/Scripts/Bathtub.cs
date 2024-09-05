using DG.Tweening;
using System;
using UnityEngine;

public class Bathtub : Primitives
{
	private Transform qipao;

	protected override void Awake()
	{
		base.Awake();
		this.qipao = this.mTrans.Find("qipao");
		if (LevelStage.CurStageInst != null)
		{
			LevelStage.CurStageInst.SuccessEvent += new Action(this.SuccessEvent);
		}
	}

	private void SuccessEvent()
	{
		this.qipao.gameObject.SetActive(true);
		this.mTrans.Find("bathtubWater").DOScaleY(1.8f, 1.5f);
		Common.PlayVibration(3);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals("DynamicParticle"))
		{
			ParticleGenerator.Inst.RemoveWaterPartical(other.gameObject);
		}
		else
		{
			Rigidbody2D component = other.GetComponent<Rigidbody2D>();
			if (component != null && component.velocity.magnitude > 2f)
			{
				Transform transform = UnityEngine.Object.Instantiate<Transform>(Resources.Load<Transform>("Particles/SFX_Dropwater/intowater"));
				transform.parent = LevelStage.CurStageInst.GameObjParent;
				transform.position = new Vector3(other.transform.position.x, this.mTrans.position.y + this.originalScale.y / 2f);
				Common.PlaySoundEffect("SFX:intowater");
			}
		}
		if (other.tag.Equals("Player") && LevelStage.CurStageInst.gameState == LevelStage.GameState.Starting)
		{
			LevelStage.CurStageInst.gameState = LevelStage.GameState.Success;
		}
	}
}
