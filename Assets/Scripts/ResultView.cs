using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ResultView : BaseView
{
	private sealed class _ShowStar_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _i___1;

		internal ResultView _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _ShowStar_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._i___1 = 0;
				break;
			case 1u:
				this._this.starTrans.Find(string.Concat(new object[]
				{
					"node",
					this._i___1,
					"/star",
					this._i___1
				})).gameObject.SetActive(true);
				Common.PlaySoundEffect("SFX:star");
				Common.PlayVibration(3);
				this._i___1++;
				break;
			case 2u:
				this._this.StarEndShowBtns();
				goto IL_169;
			case 3u:
				this._this.StarEndShowBtns();
				goto IL_169;
			default:
				return false;
			}
			if (this._i___1 >= this._this.mData.starNum)
			{
				this._this.lahua.gameObject.SetActive(true);
				if (ADSManager.ShowChartBoost(ADSManager.CBLoaction.LevelEnd))
				{
					this._current = new WaitForSeconds(2f);
					if (!this._disposing)
					{
						this._PC = 2;
					}
				}
				else
				{
					this._current = new WaitForSeconds(0.5f);
					if (!this._disposing)
					{
						this._PC = 3;
					}
				}
			}
			else
			{
				this._current = new WaitForSeconds(0.5f);
				if (!this._disposing)
				{
					this._PC = 1;
				}
			}
			return true;
			IL_169:
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private sealed class _NextBtnClick_c__AnonStorey1
	{
		internal LevelData next;

		internal ResultView _this;

		internal void __m__0()
		{
			LevelStage.Create(this.next);
			UIManager.OpenWindow<LevelUIView>(new object[]
			{
				this.next,
				LevelUIView.StartType.LevelNext
			});
			this._this.Close();
		}
	}

	private sealed class _SkinBtnClick_c__AnonStorey2
	{
		internal string key;

		internal ResultView _this;

		internal void __m__0()
		{
			this._this.gameObject.SetActive(true);
			if (this.key != UserModel.UsedSkinKey)
			{
				this._this.mpb.SetTexture("_MainTex", Resources.Load<Texture>(SkinTemplate.Tem(new object[]
				{
					UserModel.UsedSkinKey
				}).aniIcon + "/mc"));
				this._this.skinRenderer.SetPropertyBlock(this._this.mpb);
			}
		}
	}

	private Button homeBtn;

	private Button levelBtn;

	private Text levelText;

	private Button nextBtn;

	private Button restartBtn;

	private LevelData mData;

	private Transform starTrans;

	private Text allstarText;

	private Transform lahua;

	private Button turntableBtn;

	private MeshRenderer skinRenderer;

	private Button skinBtn;

	private int aniIndex;

	private MaterialPropertyBlock mpb;

	private Transform videoIcon;

	private Transform BtnsTrans;

	private Coroutine coroutine;

	private static Action __f__mg_cache0;

	private void Awake()
	{
		this.homeBtn = base.Find<Button>(base.transform, "Top/btn_back");
		this.BtnsTrans = base.transform.Find("Panel/Button");
		this.levelBtn = base.Find<Button>(base.transform, "Panel/Button/btn_List");
		this.levelText = base.Find<Text>(base.transform, "Panel/tx_level");
		this.nextBtn = base.Find<Button>(base.transform, "Panel/Button/btn_next");
		this.restartBtn = base.Find<Button>(base.transform, "Panel/Button/btn_restart");
		this.turntableBtn = base.Find<Button>(base.transform, "Panel/turntableBtn");
		this.allstarText = base.Find<Text>(base.transform, "Top/StarNum/Text");
		this.lahua = base.transform.Find("lahua");
		this.starTrans = base.transform.Find("Panel/StarControl");
		this.videoIcon = base.transform.Find("Panel/turntableBtn/videoIcon");
		this.homeBtn.onClick.AddListener(new UnityAction(this.HomeBtnClick));
		this.levelBtn.onClick.AddListener(new UnityAction(this.BackBtnClick));
		this.restartBtn.onClick.AddListener(new UnityAction(this.RestartClick));
		this.nextBtn.onClick.AddListener(new UnityAction(this.NextBtnClick));
		this.turntableBtn.onClick.AddListener(new UnityAction(this.TurntableBtnClick));
		base.OpeniPhoneX();
		this.BtnsTrans.gameObject.SetActive(false);
		this.turntableBtn.gameObject.SetActive(false);
		this.homeBtn.gameObject.SetActive(false);
        AdsControl.Instance.showAds();
	}

	private void OnEnable()
	{
	}

	public override void Init(params object[] args)
	{
		if (args.Length >= 1)
		{
			this.mData = (args[0] as LevelData);
		}
		UserModel.Inst.SavePassLevel(this.mData.key, this.mData.starNum);
		MagicTavernHelper.Track("levelEnd", new object[]
		{
			this.mData.gameTime,
			1,
			UserModel.MTPlayLevelCount(this.mData.key),
			this.mData.starNum,
			UserModel.GetFirstPassPlayCount(this.mData.key) + ";" + UserModel.GetFirstThreeStarCount(this.mData.key)
		});
		if (UserModel.Inst.IsLastLevel(this.mData.key))
		{
			base.Find<Text>(this.nextBtn.transform, "@next").SetString("@finish", new object[0]);
		}
		this.levelText.text = Utils.GetString("@level", new object[]
		{
			this.mData.baseData.index
		});
		this.allstarText.text = UserModel.Inst.GetStarCount() + "/" + LevelTemplate.Dic().Count * 3;
		this.coroutine = UIManager.GetInst(false).StartCoroutine(this.ShowStar());
		Common.PlaySoundEffect("SFX:win");
		Voice.PauseBackMusic();
		float arg_1B9_1 = 4f;
		if (ResultView.__f__mg_cache0 == null)
		{
			ResultView.__f__mg_cache0 = new Action(Voice.PlayBgMusic);
		}
		this.DelayToDo(arg_1B9_1, ResultView.__f__mg_cache0, false);
	}

	private IEnumerator ShowStar()
	{
		ResultView._ShowStar_c__Iterator0 _ShowStar_c__Iterator = new ResultView._ShowStar_c__Iterator0();
		_ShowStar_c__Iterator._this = this;
		return _ShowStar_c__Iterator;
	}

	private void StarEndShowBtns()
	{
		this.BtnsTrans.gameObject.SetActive(true);
		this.homeBtn.gameObject.SetActive(true);
	}

	private void NextBtnClick()
	{
		if (UserModel.Inst.IsLastLevel(this.mData.key))
		{
			this.BackBtnClick();
		}
		else
		{
			LevelData next = UserModel.Inst.GetLevelData(this.mData.jsonIndex + 1);
			if (next.baseData.chapterId != this.mData.baseData.chapterId && ChapterModel.ChapterIsOpen(next.baseData.chapterId) > 0)
			{
				this.BackBtnClick();
				return;
			}
			UIManager.GetInst(false).ShowBlack(delegate
			{
				LevelStage.Create(next);
				UIManager.OpenWindow<LevelUIView>(new object[]
				{
					next,
					LevelUIView.StartType.LevelNext
				});
				this.Close();
			}, null, null);
		}
	}

	private void RestartClick()
	{
		UIManager.GetInst(false).ShowBlack(delegate
		{
			LevelStage.Create(this.mData);
			this.Close();
			UIManager.OpenWindow<LevelUIView>(new object[]
			{
				this.mData,
				LevelUIView.StartType.SuccessRestart
			});
		}, null, null);
	}

	private void BackBtnClick()
	{
		UIManager.OpenWindow<LevelListView>(new object[]
		{
			ChapterTemplate.Tem(new object[]
			{
				this.mData.baseData.chapterId
			})
		});
		this.Close();
	}

	private void HomeBtnClick()
	{
		UIManager.OpenWindow<HomeView>(new object[0]);
		this.Close();
	}

	public override void Close()
	{
		base.Close();
		Voice.PlayBgMusic();
		UIManager.GetInst(false).StopCoroutine(this.coroutine);
	}

	private void TurntableBtnClick()
	{
		if (UserModel.TurntableIsFree())
		{
			this.OpenTurnView();
		}
		else
		{
			ADSManager.PlayVideo(ADSManager.VedioType.Turntable, new Action(this.OpenTurnView));
		}
	}

	private void OpenTurnView()
	{
	}

	private void SkinBtnClick()
	{
		SkinStoreView skinStoreView = UIManager.OpenWindow<SkinStoreView>(new object[0]);
		string key = UserModel.UsedSkinKey;
		base.gameObject.SetActive(false);
		skinStoreView.CloseCallBack = delegate
		{
			this.gameObject.SetActive(true);
			if (key != UserModel.UsedSkinKey)
			{
				this.mpb.SetTexture("_MainTex", Resources.Load<Texture>(SkinTemplate.Tem(new object[]
				{
					UserModel.UsedSkinKey
				}).aniIcon + "/mc"));
				this.skinRenderer.SetPropertyBlock(this.mpb);
			}
		};
	}
}
