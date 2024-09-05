using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class ParticleGenerator : MonoBehaviour
{
	private float lastSpawnTime = -3.40282347E+38f;

	private float lastPosChangeTime = -3.40282347E+38f;

	public int PARTICLE_LIFETIME = 3;

	public DynamicParticle.STATES particlesState;

	private bool isTouched;

	private Vector3 waterPos;

	private Transform mTran;

	private List<GameObject> particleList;

	private List<GameObject> DestroiedList;

	public static ParticleGenerator Inst;

	private GameObject particlePrefab;

	private float originalColRadius = 0.1f;

	private int waterCount;



	private Transform hydrantTrans;

	private Transform hyOnOffTrans;

	private float hydrantPosy = 9.5f;

	private const float hydrantMaxPos = 7.3f;

	private int i;

	private float soundtime;

	private Vector3 rangePos = new Vector3(0f, -2.5f);

	public event Action WaterCountChange;

	public int particleCount
	{
		get
		{
			return this.particleList.Count;
		}
	}

	public int WaterCount
	{
		get
		{
			return this.waterCount;
		}
	}

	private void Awake()
	{
		ParticleGenerator.Inst = this;
		this.particleList = new List<GameObject>();
		this.DestroiedList = new List<GameObject>();
		this.mTran = base.transform;
		this.RegisterTouchEvent();
		this.particlePrefab = Resources.Load<GameObject>("Prefabs/LiquidPhysics/DynamicParticle");
		this.hydrantTrans = UnityEngine.Object.Instantiate<Transform>(Resources.Load<Transform>("Prefabs/Huasha"));
		this.hyOnOffTrans = this.hydrantTrans.Find("famen");
		this.hydrantTrans.parent = this.mTran.parent;
		this.hydrantTrans.position = new Vector3(0f, this.hydrantPosy + 7.3f);
		this.hydrantTrans.gameObject.SetActive(false);
		this.originalColRadius = this.particlePrefab.GetComponent<CircleCollider2D>().radius;
		if (LevelStage.CurStageInst != null)
		{
			LevelStage.CurStageInst.SuccessEvent += delegate
			{
				this.hydrantTrans.Find("intowater").gameObject.SetActive(false);
			};
		}
	}

	private void RegisterTouchEvent()
	{
		UIManager.GetInst(false).TouchEvent.gameObject.SetActive(true);
		UIManager.GetInst(false).TouchEvent.onPointerDown += new Action<PointerEventData, UGUIEvent>(this.OnPointDown);
		UIManager.GetInst(false).TouchEvent.onPointerUp += new Action<PointerEventData, UGUIEvent>(this.OnPointUp);
		UIManager.GetInst(false).TouchEvent.onDrag += new Action<PointerEventData, UGUIEvent>(this.OnDrag);
	}

	public void OnPointDown(PointerEventData eventData, UGUIEvent sender)
	{
		this.SetPos(eventData);
		this.hydrantTrans.gameObject.SetActive(true);
		Vector3 position = this.hydrantTrans.position;
		position.x = this.waterPos.x;
		this.hydrantTrans.position = position;
		this.hydrantTrans.DOKill(false);
		this.hyOnOffTrans.DOKill(false);
		if (this.isTouched)
		{
			return;
		}
		this.hydrantTrans.DOMoveY(this.hydrantPosy, 0.2f, false).SetEase(Ease.OutCirc).OnComplete(delegate
		{
			this.isTouched = true;
			Common.PlaySoundEffect("SFX:draw");
		});
		this.hyOnOffTrans.DOLocalRotate(new Vector3(0f, 0f, -360f), 0.4f, RotateMode.LocalAxisAdd);
	}

	public void OnPointUp(PointerEventData eventData, UGUIEvent sender)
	{
		this.SetPos(eventData);
		this.hydrantTrans.DOKill(false);
		this.hyOnOffTrans.DOKill(false);
		this.isTouched = false;
		Vector3 position = this.hydrantTrans.position;
		position.x = this.waterPos.x;
		this.hydrantTrans.position = position;
		Common.StopSoundEffect("SFX:draw");
		this.hydrantTrans.DOMoveY(this.hydrantPosy + 7.3f, 0.2f, false).SetEase(Ease.InCubic).OnComplete(delegate
		{
			this.hydrantTrans.gameObject.SetActive(false);
		});
		this.hyOnOffTrans.DOLocalRotate(new Vector3(0f, 0f, 360f), 0.2f, RotateMode.LocalAxisAdd);
	}

	public void OnDrag(PointerEventData eventData, UGUIEvent sender)
	{
		this.SetPos(eventData);
	}

	private void SetPos(PointerEventData eventData)
	{
		this.waterPos = Utils.ScreenToLocalPos(eventData.position, LevelStage.CurStageInst.WaterParent);
		this.waterPos.y = this.hydrantPosy;
		this.waterPos.z = 0f;
	}

	private void Update()
	{
		if (LevelStage.CurStageInst.gameState == LevelStage.GameState.Starting && this.isTouched && this.lastSpawnTime + SceneSetting.inst.WaterSpawnInterval < Time.time)
		{
			this.soundtime -= Time.deltaTime;
			if (this.soundtime < 0f)
			{
				this.soundtime = 0.5f;
				Common.PlayVibration(1);
			}
			this.lastSpawnTime = Time.time;
			this.i = 0;
			while (this.i < UnityEngine.Random.Range(1, 3))
			{
				this.WaterGenerator();
				this.i++;
			}
		}
		else if (LevelStage.CurStageInst.gameState != LevelStage.GameState.Starting && this.isTouched)
		{
			this.isTouched = false;
			Common.StopSoundEffect("SFX:draw");
		}
	}

	private void WaterGenerator()
	{
		GameObject gameObject;
		if (this.DestroiedList.Count > 0)
		{
			gameObject = this.DestroiedList[this.DestroiedList.Count - 1];
			this.DestroiedList.RemoveAt(this.DestroiedList.Count - 1);
			gameObject.gameObject.SetActive(true);
			Rigidbody2D component = gameObject.GetComponent<Rigidbody2D>();
			component.velocity = Vector2.zero;
			component.angularVelocity = 0f;
		}
		else if (this.particleList.Count < SceneSetting.inst.WaterMaxCount)
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.particlePrefab);
		}
		else
		{
			gameObject = this.particleList[0];
			this.particleList.RemoveAt(0);
			Rigidbody2D component = gameObject.GetComponent<Rigidbody2D>();
			component.velocity = Vector2.zero;
			component.angularVelocity = 0f;
		}
		this.waterCount++;
		if (this.WaterCountChange != null)
		{
			this.WaterCountChange();
		}
		this.particleList.Add(gameObject);
		gameObject.name = "DP_" + this.particleList.Count;
		DynamicParticle component2 = gameObject.GetComponent<DynamicParticle>();
		CircleCollider2D component3 = component2.GetComponent<CircleCollider2D>();
		component3.radius = this.originalColRadius + (float)UnityEngine.Random.Range(-10, 20) * 0.001f;
		component2.SetLifeTime((float)this.PARTICLE_LIFETIME);
		component2.SetState(this.particlesState);
		gameObject.transform.parent = this.mTran;
		this.rangePos.x = UnityEngine.Random.Range(-20f, 20f) * 0.01f;
		this.hydrantTrans.position = this.waterPos;
		gameObject.transform.localPosition = this.waterPos + this.rangePos;
		gameObject.GetComponent<Rigidbody2D>().AddForce(SceneSetting.inst.waterForce, ForceMode2D.Impulse);
	}

	public void RemoveWaterPartical(GameObject go)
	{
		this.particleList.Remove(go);
		this.DestroiedList.Add(go);
		go.SetActive(false);
	}

	public void Reset()
	{
		this.waterCount = 0;
	}

	private void OnDestroy()
	{
		this.particleList.Clear();
		this.particleList = null;
		this.DestroiedList.Clear();
		this.DestroiedList = null;
		Common.StopSoundEffect("SFX:draw");
		ParticleGenerator.Inst = null;
		if (UIManager.GetInst(true) != null)
		{
			UIManager.GetInst(true).TouchEvent.gameObject.SetActive(false);
			UIManager.GetInst(true).TouchEvent.onPointerDown -= new Action<PointerEventData, UGUIEvent>(this.OnPointDown);
			UIManager.GetInst(true).TouchEvent.onPointerUp -= new Action<PointerEventData, UGUIEvent>(this.OnPointUp);
			UIManager.GetInst(true).TouchEvent.onDrag -= new Action<PointerEventData, UGUIEvent>(this.OnDrag);
		}
	}
}
