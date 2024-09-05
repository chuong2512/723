using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HomeView : BaseView
{
	private sealed class _Awake_c__AnonStorey0
	{
		internal Transform settingBts;

		internal HomeView _this;

		internal void __m__0()
		{
			this.settingBts.gameObject.SetActive(!this.settingBts.gameObject.activeSelf);
		}

		internal void __m__1()
		{
			UIManager.OpenWindow<ChapterView>(new object[0]);
			this._this.Close();
		}
	}

	private sealed class _SkinBtnClick_c__AnonStorey1
	{
		internal string key;

		internal HomeView _this;

		internal void __m__0()
		{
			this._this.gameObject.SetActive(true);
			if (this.key != UserModel.UsedSkinKey)
			{
				SkinTemplate skinTemplate = SkinTemplate.Tem(new object[]
				{
					UserModel.UsedSkinKey
				});
				this._this.mpb.SetTexture("_MainTex", Resources.Load<Texture>(skinTemplate.aniIcon + "/mc"));
				this._this.skinRenderer.SetPropertyBlock(this._this.mpb);
				if (this._this.yanName != skinTemplate.yanPartical + "home")
				{
					UnityEngine.Object.Destroy(this._this.yanTrans.gameObject);
					this._this.yanName = skinTemplate.yanPartical + "home";
					this._this.yanTrans = UnityEngine.Object.Instantiate<Transform>(Resources.Load<Transform>(this._this.yanName), this._this.transform.Find("Panel/mc/SkeletonUtility-Root/root/all/bone/bone2/bone7/bone8"));
				}
			}
		}
	}

	private sealed class _StartGame_c__AnonStorey2
	{
		internal LevelData levelData;

		internal HomeView _this;

		internal void __m__0()
		{
			LevelStage.Create(this.levelData);
			UIManager.OpenWindow<LevelUIView>(new object[]
			{
				this.levelData,
				LevelUIView.StartType.Home
			});
			this._this.Close();
		}
	}

	private Button startBtn;

	private Button levelBtn;

	private Button settingBtn;

	private Toggle musicBtn;

	private Toggle soundBtn;

	private Toggle vibBtn;

	private Text levelText;

	private Button skinBtn;

	private MeshRenderer skinRenderer;

	private string yanName;

	private Transform yanTrans;

	private Text moneyText;

	private MaterialPropertyBlock mpb;

	private static UnityAction __f__mg_cache0;

	private static UnityAction __f__am_cache0;

	private static UnityAction __f__am_cache1;

	private void Awake()
	{
		this.startBtn = base.Find<Button>(base.transform, "Panel/btn_start");
		this.levelBtn = base.Find<Button>(base.transform, "Panel/btn_level");
		this.skinRenderer = base.Find<MeshRenderer>(base.transform, "Panel/mc");
		this.startBtn.onClick.AddListener(new UnityAction(this.StartGame));
		this.settingBtn = base.Find<Button>(base.transform, "btn_setting");
		Transform settingBts = base.transform.Find("Settine");
		this.levelText = base.Find<Text>(this.startBtn.transform, "level");
		this.skinBtn = base.Find<Button>(base.transform, "SkinBtn");
		this.settingBtn.onClick.AddListener(delegate
		{
			settingBts.gameObject.SetActive(!settingBts.gameObject.activeSelf);
		});
		this.levelBtn.onClick.AddListener(delegate
		{
			UIManager.OpenWindow<ChapterView>(new object[0]);
			this.Close();
		});
		this.musicBtn = base.Find<Toggle>(settingBts, "btn_music");
		this.soundBtn = base.Find<Toggle>(settingBts, "btn_sound");
		this.vibBtn = base.Find<Toggle>(settingBts, "btn_shock");
		this.musicBtn.isOn = Voice.GetMusic();
		this.musicBtn.targetGraphic.gameObject.SetActive(!this.musicBtn.isOn);
		this.soundBtn.isOn = Voice.GetVoice();
		this.soundBtn.targetGraphic.gameObject.SetActive(!this.soundBtn.isOn);
		this.vibBtn.isOn = UserModel.GetBration();
		this.vibBtn.targetGraphic.gameObject.SetActive(!this.vibBtn.isOn);
		this.musicBtn.onValueChanged.AddListener(new UnityAction<bool>(this.BtnSetMusic));
		this.soundBtn.onValueChanged.AddListener(new UnityAction<bool>(this.BtnSetVoice));
		this.vibBtn.onValueChanged.AddListener(new UnityAction<bool>(this.SetBration));
		bool flag = false;
		this.vibBtn.gameObject.SetActive(!flag);
		base.OpeniPhoneX();
		this.levelText.SetString("@level", new object[]
		{
			LevelTemplate.Tem(new object[]
			{
				UserModel.Inst.latelyLevel
			}).index
		});
		this.skinBtn.onClick.AddListener(new UnityAction(this.SkinBtnClick));
		this.moneyText = base.Find<Text>(base.transform, "Coinsboard/Count");
		this.moneyText.text = UserModel.Inst.Money.ToString();
		this.skinBtn.transform.Find("skin_bg").DORotate(new Vector3(0f, 0f, 360f), 5f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1);
		UserModel.Inst.OnMoneyChange += new Action<int>(this.MoneyUpdate);
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
				SkinTemplate skinTemplate = SkinTemplate.Tem(new object[]
				{
					UserModel.UsedSkinKey
				});
				this.mpb.SetTexture("_MainTex", Resources.Load<Texture>(skinTemplate.aniIcon + "/mc"));
				this.skinRenderer.SetPropertyBlock(this.mpb);
				if (this.yanName != skinTemplate.yanPartical + "home")
				{
					UnityEngine.Object.Destroy(this.yanTrans.gameObject);
					this.yanName = skinTemplate.yanPartical + "home";
					this.yanTrans = UnityEngine.Object.Instantiate<Transform>(Resources.Load<Transform>(this.yanName), this.transform.Find("Panel/mc/SkeletonUtility-Root/root/all/bone/bone2/bone7/bone8"));
				}
			}
		};
	}

	public void BtnSetVoice(bool able)
	{
		Voice.SetVoice(able);
		this.soundBtn.targetGraphic.gameObject.SetActive(!able);
	}

	public void BtnSetMusic(bool able)
	{
		Voice.SetMusic(able, true);
		this.musicBtn.targetGraphic.gameObject.SetActive(!able);
	}

	private void SetBration(bool able)
	{
		this.vibBtn.targetGraphic.gameObject.SetActive(!able);
		UserModel.SetConfig("LoveShots_GameSetting_bration", (!able) ? "0" : "1");
		Common.PlayVibration(1);
	}

	private void StartGame()
	{
		LevelData levelData = UserModel.Inst.GetLevelData(UserModel.Inst.latelyLevel);
		UIManager.GetInst(false).ShowBlack(delegate
		{
			LevelStage.Create(levelData);
			UIManager.OpenWindow<LevelUIView>(new object[]
			{
				levelData,
				LevelUIView.StartType.Home
			});
			this.Close();
		}, null, null);
	}

	private void DebugBtns()
	{
		base.transform.Find("Debug").gameObject.SetActive(true);
		Button button = base.Find<Button>(base.transform, "Debug/DebugBtn");
		Button button2 = base.Find<Button>(base.transform, "Debug/ClearBtn");
		Button button3 = base.Find<Button>(base.transform, "Debug/MoneyBtn");
		Button button4 = base.Find<Button>(base.transform, "Debug/LockSkinsBtn");
		UnityEvent arg_86_0 = button2.onClick;
		if (HomeView.__f__mg_cache0 == null)
		{
			HomeView.__f__mg_cache0 = new UnityAction(PlayerPrefs.DeleteAll);
		}
		arg_86_0.AddListener(HomeView.__f__mg_cache0);
		button.onClick.AddListener(new UnityAction(this.UnLock));
		button3.onClick.AddListener(delegate
		{
			UserModel.Inst.Money += 100000;
			UserModel.Inst.HintCount += 99;
		});
		button4.onClick.AddListener(delegate
		{
			foreach (KeyValuePair<string, SkinTemplate> current in SkinTemplate.Dic())
			{
				UserModel.BuyGoods(current.Key, true);
			}
		});
	}

	public void UnLock()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 1001; i <= LevelTemplate.Dic().Count + 1000; i++)
		{
			stringBuilder.Append(i + "," + 3);
			stringBuilder.Append("|");
		}
		stringBuilder.Remove(stringBuilder.Length - 1, 1);
		PlayerPrefs.SetString("PassLevelName", stringBuilder.ToString());
		PlayerPrefs.SetInt("MaxStarNum", LevelTemplate.Dic().Count * 3);
		PlayerPrefs.Save();
	}

	private void MoneyUpdate(int v)
	{
		this.moneyText.text = UserModel.Inst.Money.ToString();
	}

	private void OnDestroy()
	{
		UserModel.Inst.OnMoneyChange -= new Action<int>(this.MoneyUpdate);
	}
}
