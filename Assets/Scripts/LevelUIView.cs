using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelUIView : BaseView
{
	public enum StartType
	{
		Home = 1,
		LevelList,
		LevelAutoRestart,
		Restart,
		LevelNext,
		SuccessRestart,
		HintRestart
	}

	private LevelData mData;

	private Button restartBtn;

	private Button backBtn;

	private Text levelText;

	private RectTransform starBarTrans;

	private RectTransform[] stars;

	private Slider waterCountSlider;

	private Transform handleSlide;

	private int curStarLevel;

	private Transform countdownTrans;

	private Text countDownText;

	private Transform hintList;

	private Text hintText;

	private Button hintBtn;

	private Transform videoTrans;

	private Text hintNum;

	public CanvasGroup group;

	private bool isPlayVideo;

	private static UnityAction __f__am_cache0;

	private static Action __f__am_cache1;

	private void Awake()
	{
		this.restartBtn = base.Find<Button>(base.transform, "sp_top/an_restart/btn_restart");
		this.backBtn = base.Find<Button>(base.transform, "sp_top/btn_stop");
		this.levelText = base.Find<Text>(base.transform, "sp_top/tx_level");
		this.restartBtn.onClick.AddListener(new UnityAction(this.RestartBtnClick));
		this.backBtn.onClick.AddListener(new UnityAction(this.BackBtnClick));
		this.starBarTrans = base.Find<RectTransform>(base.transform, "sp_top/pl/StarControl");
		this.waterCountSlider = base.Find<Slider>(this.starBarTrans, "TimeSlider");
		this.handleSlide = this.waterCountSlider.transform.Find("HandleSlide");
		this.countdownTrans = base.transform.Find("countDown");
		this.countDownText = base.Find<Text>(this.countdownTrans, "countDown");
		this.hintList = base.transform.Find("HintList");
		this.hintText = base.Find<Text>(this.hintList, "Text");
		this.hintBtn = base.Find<Button>(base.transform, "sp_top/HintBtn");
		this.videoTrans = base.transform.Find("sp_top/HintBtn/video");
		this.hintNum = base.Find<Text>(base.transform, "sp_top/HintBtn/Num");
		this.stars = new RectTransform[3];
		for (int i = 0; i < 3; i++)
		{
			this.stars[i] = base.Find<RectTransform>(this.starBarTrans, "node_0" + i);
		}
		base.OpeniPhoneX();
		if (UserModel.Inst.HintCount > 0)
		{
			this.videoTrans.gameObject.SetActive(false);
			this.hintNum.gameObject.SetActive(true);
			this.hintNum.text = UserModel.Inst.HintCount.ToString();
		}
		this.group = base.Find<CanvasGroup>(base.transform, "sp_top");
	}

	public override void Init(params object[] args)
	{
		if (args.Length >= 1)
		{
			this.mData = (args[0] as LevelData);
		}
		if (args.Length >= 2)
		{
			LevelUIView.StartType startType = (LevelUIView.StartType)args[1];
			MagicTavernHelper.Track("levelStart", new object[]
			{
				(int)startType
			});
		}
		UserModel.AddMTPlayLevelCount(this.mData.key);
		UserModel.MTPlayCount++;
		UserModel.Inst.latelyLevel = this.mData.key;
		this.levelText.text = Utils.GetString("@level", new object[]
		{
			this.mData.baseData.index
		});
		GameScene.scene.curData = this.mData;
		this.mData.gameTime = 0f;
		this.curStarLevel = this.mData.StarWaterCount.Length;
		this.mData.starNum = this.curStarLevel;
		for (int i = 0; i < this.mData.StarWaterCount.Length; i++)
		{
			float x = Mathf.Lerp(0f, this.starBarTrans.sizeDelta.x, (this.mData.StarWaterCount[0] - this.mData.StarWaterCount[i]) / this.mData.StarWaterCount[0]);
			this.stars[i].anchoredPosition = new Vector2(x, -20f);
		}
		if (GameScene.scene.latelyHintLevel.Equals(this.mData.key))
		{
			this.hintBtn.gameObject.SetActive(false);
			Button button = base.Find<Button>(base.transform, "sp_top/SkipBtn");
			button.gameObject.SetActive(true);
			button.onClick.AddListener(delegate
			{
				UIManager.GetInst(false).CloseOpenedWindow<HintView>();
				LevelStage.CurStageInst.gameState = LevelStage.GameState.Success;
			});
		}
		else
		{
			this.hintBtn.onClick.AddListener(new UnityAction(this.HintBtnClick));
			GameScene.scene.latelyHintLevel = string.Empty;
		}
		ParticleGenerator.Inst.WaterCountChange += new Action(this.WaterCountChange);
	}

	private void WaterCountChange()
	{
		if (LevelStage.CurStageInst.gameState != LevelStage.GameState.Starting)
		{
			return;
		}
		this.waterCountSlider.value = Mathf.Max(new float[]
		{
			this.mData.StarWaterCount[0] - (float)ParticleGenerator.Inst.WaterCount
		}) / this.mData.StarWaterCount[0];
		if (this.curStarLevel > 1 && (float)ParticleGenerator.Inst.WaterCount > this.mData.StarWaterCount[this.curStarLevel - 1])
		{
			this.curStarLevel--;
			this.mData.starNum = this.curStarLevel;
			this.stars[this.curStarLevel].Find("star").gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		this.mData.gameTime += Time.deltaTime;
	}

	private void BackBtnClick()
	{
		MagicTavernHelper.Track("levelEnd", new object[]
		{
			this.mData.gameTime,
			2,
			UserModel.MTPlayLevelCount(this.mData.key),
			0,
			UserModel.GetFirstPassPlayCount(this.mData.key) + ";" + UserModel.GetFirstThreeStarCount(this.mData.key)
		});
		this.Close();
		LevelStage.DestroyWorld(false);
		UIManager.OpenWindow<LevelListView>(new object[]
		{
			ChapterTemplate.Tem(new object[]
			{
				this.mData.baseData.chapterId
			})
		});
		GameScene.scene.latelyHintLevel = string.Empty;
	}

	private void RestartBtnClick()
	{
		GameScene.scene.ShowCBRestart(this.mData, LevelUIView.StartType.Restart, ADSManager.CBLoaction.Restart);
		MagicTavernHelper.Track("levelEnd", new object[]
		{
			this.mData.gameTime,
			3,
			UserModel.MTPlayLevelCount(this.mData.key),
			0,
			UserModel.GetFirstPassPlayCount(this.mData.key) + ";" + UserModel.GetFirstThreeStarCount(this.mData.key)
		});
		this.ForbiddenRestartBtn();
	}

	private void HintBtnClick()
	{
		if (this.mData.baseData.index <= 5)
		{
			this.HintShow();
			return;
		}
		if (UserModel.Inst.HintCount > 0)
		{
			this.hintNum.text = UserModel.Inst.HintCount.ToString();
			UserModel.Inst.HintCount--;
			this.HintShow();
		}
		else
		{
			ADSManager.PlayVideo(ADSManager.VedioType.Hint, new Action(this.HintShow));
		}
	}

	private void HintShow()
	{
		MagicTavernHelper.Track("levelEnd", new object[]
		{
			this.mData.gameTime,
			4,
			UserModel.MTPlayLevelCount(this.mData.key),
			0,
			UserModel.GetFirstPassPlayCount(this.mData.key) + ";" + UserModel.GetFirstThreeStarCount(this.mData.key)
		});
		UIManager.GetInst(false).ShowBlack(delegate
		{
			LevelStage.DestroyWorld(false);
		}, delegate
		{
			LevelStage.Create(this.mData);
			this.Close();
			GameScene.scene.latelyHintLevel = this.mData.key;
			LevelUIView levelUIView = UIManager.OpenWindow<LevelUIView>(new object[]
			{
				this.mData,
				LevelUIView.StartType.HintRestart
			});
			UIManager.OpenWindow<HintView>(new object[0]);
		}, null);
	}

	public void ShowAddCoinText(Transform point, string key)
	{
		Text text = UnityEngine.Object.Instantiate<Text>(this.hintText, this.hintList);
		text.gameObject.SetActive(true);
		text.SetString(key, new object[0]);
		Vector3 point2 = UIManager.GetInst(false).m_UICamera.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(point.position));
		text.transform.localPosition = this.hintList.worldToLocalMatrix.MultiplyPoint(point2);
	}

	private void ForbiddenRestartBtn()
	{
		this.group.interactable = false;
		this.DelayToDo(3f, delegate
		{
			this.group.interactable = true;
		}, false);
	}

	public override void Close()
	{
		base.Close();
		if (ParticleGenerator.Inst != null)
		{
			ParticleGenerator.Inst.WaterCountChange -= new Action(this.WaterCountChange);
		}
	}
}
