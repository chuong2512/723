using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameScene : PBase
{
	private sealed class _RestartGame_c__AnonStorey0
	{
		internal LevelData data;

		internal LevelUIView.StartType startType;

		internal void __m__0()
		{
			LevelStage.Create(this.data);
			UIManager.OpenWindow<LevelUIView>(new object[]
			{
				this.data,
				this.startType
			});
		}
	}

	private sealed class _GameSuccess_c__AnonStorey1
	{
		internal LevelData data;

		internal void __m__0()
		{
			UIManager.OpenWindow<ResultView>(new object[]
			{
				this.data
			});
			LevelStage.DestroyWorld(false);
			UIManager.GetInst(false).CloseOpenedWindow<LevelUIView>();
		}
	}

	public static GameScene scene;

	public LevelData curData;

	public string latelyHintLevel;

	private LevelUIView.StartType curStartType;

	private static Action __f__am_cache0;

	protected override void Awake()
	{
		base.Awake();
        Application.targetFrameRate = 60;
		GameScene.scene = this;
		this.OutScreenDestroy();
	}

	private void Start()
	{
		UIManager.OpenWindow<HomeView>(new object[0]);
		Voice.PlayNewBgMusic("BGM:bg");
	}

	public void ShowCBRestart(LevelData data, LevelUIView.StartType startType, ADSManager.CBLoaction loaction = ADSManager.CBLoaction.LevelEnd)
	{
		this.curData = data;
		this.RestartGame(data, startType);
	}

	private void PlayChartBoostEnd()
	{
		this.RestartGame(this.curData, this.curStartType);
	}

	private void RestartGame(LevelData data, LevelUIView.StartType startType)
	{
		if (LevelStage.CurStageInst == null)
		{
			return;
		}
		UIManager.GetInst(false).ShowBlack(delegate
		{
			UIManager.GetInst(false).CloseOpenedWindow<LevelUIView>();
			LevelStage.DestroyWorld(false);
		}, delegate
		{
			LevelStage.Create(data);
			UIManager.OpenWindow<LevelUIView>(new object[]
			{
				data,
				startType
			});
		}, null);
	}

	public void GameSuccess(LevelData data)
	{
		this.DelayToDo(2f, delegate
		{
			UIManager.OpenWindow<ResultView>(new object[]
			{
				data
			});
			LevelStage.DestroyWorld(false);
			UIManager.GetInst(false).CloseOpenedWindow<LevelUIView>();
		}, false);
	}

	public void OutScreenDestroy()
	{
		GameObject gameObject = new GameObject("DestroyOutScreen");
		Vector3 a = Camera.main.ViewportToWorldPoint(Vector3.zero);
		a.z = 0f;
		gameObject.transform.position = a - new Vector3(0f, 5f, 0f);
		BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
		boxCollider2D.size = new Vector2(1000f, 1f);
		boxCollider2D.isTrigger = true;
		gameObject.AddComponent<ColliderTrigger>().TriggerEnter += new Action<Collider2D>(this.TriggerEnter);
	}

	private void TriggerEnter(Collider2D col)
	{
		Primitives component = col.GetComponent<Primitives>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component.gameObject);
		}
	}

	public void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && !UIManager.GetInst(false).GetOpenWindow<QuitView>())
		{
			UIManager.OpenWindow<QuitView>(new object[0]);
		}
	}
}
