using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ADSManager : MonoBehaviour
{
	public enum CBLoaction
	{
		LevelEnd = 1,
		BackGround,
		Restart
	}

	public enum VedioType
	{
		Hint = 1,
		Turntable,
		SkinVideo,
		ThreeStar
	}

	public static DateTime s_last_show_ads_time = DateTime.Now;

	public static ADSManager.CBLoaction slastShowChartBoostLocation = ADSManager.CBLoaction.LevelEnd;

	public static int cbCoolTime = 60;

	public static Action CharBoostStopCall;

	private static Action vedioPlayEnd;

	public static bool IsPlayingFreeRewardVideo = false;

	private static WJUtils.RewardVideoCallbackDelegate __f__mg_cache0;

	private static WJUtils.RewardVideoCallbackDelegate __f__mg_cache1;

	private void Awake()
	{
		//WJUtils.onInterstitialStart += new Action(this.OnInterstitialStart);
		//WJUtils.onInterstitialStop += new Action(this.OnInterstitialStop);
		//WJUtils.onInterstitialFail += new Action(this.OnInterstitialFail);
		//WJUtils.OnGetFreeRewardVideoStart += new WJUtils.RewardVideoCallbackDelegate(this.OnGetFreeRewardVideoStart);
		//WJUtils.onAdsClick += new Action<int>(this.OnAdsClick);
		base.InvokeRepeating("BannerShow", 0f, 2f);
	}

	public static bool ShowChartBoost(ADSManager.CBLoaction location = ADSManager.CBLoaction.LevelEnd)
	{
		ADSManager.slastShowChartBoostLocation = location;
		if (UserModel.Inst.GetLevelData(UserModel.Inst.latelyLevel).baseData.index < 10)
		{
			return false;
		}
		if (!WJUtils.IsInterstitialReady())
		{
			MagicTavernHelper.TrackADS("interstitial", 3, (int)location, MagicTavernHelper.ADS_FAILED.NotReady);
			return false;
		}
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			MagicTavernHelper.TrackADS("interstitial", 3, (int)location, MagicTavernHelper.ADS_FAILED.NetworkNotReachable);
			return false;
		}
		if ((DateTime.Now - ADSManager.s_last_show_ads_time).TotalSeconds > (double)ADSManager.cbCoolTime)
		{
			ADSManager.s_last_show_ads_time = DateTime.Now;
			Common.ShowChartBoost(true, true, location);
			return true;
		}
		MagicTavernHelper.TrackADS("interstitial", 3, (int)location, MagicTavernHelper.ADS_FAILED.Cooldown);
		return false;
	}

	private void OnInterstitialStart()
	{
		MagicTavernHelper.TrackADS("interstitial", 1, (int)ADSManager.slastShowChartBoostLocation, MagicTavernHelper.ADS_FAILED.None);
	}

	private void OnInterstitialStop()
	{
		ADSManager.ResetTime();
		MagicTavernHelper.TrackADS("interstitial", 2, (int)ADSManager.slastShowChartBoostLocation, MagicTavernHelper.ADS_FAILED.None);
		if (ADSManager.CharBoostStopCall != null)
		{
			ADSManager.CharBoostStopCall();
			ADSManager.CharBoostStopCall = null;
		}
	}

	private void OnInterstitialFail()
	{
		UnityEngine.Debug.Log("OnInterstitialFail:::cha ping  shi bai");
	}

	public static void ResetTime()
	{
		ADSManager.s_last_show_ads_time = DateTime.Now;
	}

	public static bool IsFree(ADSManager.VedioType key)
	{
		return PlayerPrefs.GetInt("Vedio_" + key.ToString().Split(new char[]
		{
			'_'
		})[0], 0) == 0;
	}

	public static void PlayVideo(ADSManager.VedioType key, Action playEndCall)
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			UIManager.OpenWindow<HinttipsView>(new object[]
			{
				Utils.GetString("@InternetLost", new object[0])
			});
			MagicTavernHelper.TrackADS("rewardedVideo", 3, (int)key, MagicTavernHelper.ADS_FAILED.NetworkNotReachable);
		}
		else if (WJUtils.IsGetFreeRewardVideoReady(false, key.ToString()))
		{
			ADSManager.vedioPlayEnd = playEndCall;
			if (ADSManager.__f__mg_cache0 == null)
			{
				ADSManager.__f__mg_cache0 = new WJUtils.RewardVideoCallbackDelegate(ADSManager.OnGetFreeRewardVideoClose);
			}
			//WJUtils.OnGetFreeRewardVideoClose += ADSManager.__f__mg_cache0;
			//WJUtils.PlayGetFreeRewardVideo(key.ToString(), true);
		}
		else
		{
			UIManager.OpenWindow<HinttipsView>(new object[]
			{
				Utils.GetString("@NotReady", new object[0])
			});
			MagicTavernHelper.TrackADS("rewardedVideo", 3, (int)key, MagicTavernHelper.ADS_FAILED.NotReady);
		}
	}

	private void OnGetFreeRewardVideoStart(string getFreeLockKey, int rewarded)
	{
		int originID = (int)Enum.Parse(typeof(ADSManager.VedioType), getFreeLockKey);
		MagicTavernHelper.TrackADS("rewardedVideo", 1, originID, MagicTavernHelper.ADS_FAILED.None);
		ADSManager.IsPlayingFreeRewardVideo = true;
	}

	public static void OnGetFreeRewardVideoClose(string getFreeLockKey, int rewarded)
	{
		int originID = (int)Enum.Parse(typeof(ADSManager.VedioType), getFreeLockKey);
		if (ADSManager.__f__mg_cache1 == null)
		{
			ADSManager.__f__mg_cache1 = new WJUtils.RewardVideoCallbackDelegate(ADSManager.OnGetFreeRewardVideoClose);
		}
		//WJUtils.OnGetFreeRewardVideoClose -= ADSManager.__f__mg_cache1;
		if (WJUtils.IsGetFreeUnlocked(getFreeLockKey))
		{
			UserModel.WatchVideoCount++;
			ADSManager.ResetTime();
			MagicTavernHelper.TrackADS("rewardedVideo", 2, originID, MagicTavernHelper.ADS_FAILED.None);
			if (ADSManager.vedioPlayEnd != null)
			{
				ADSManager.vedioPlayEnd();
				ADSManager.vedioPlayEnd = null;
			}
		}
		else
		{
			MagicTavernHelper.TrackADS("rewardedVideo", 3, originID, MagicTavernHelper.ADS_FAILED.UserCancel);
		}
		ADSManager.IsPlayingFreeRewardVideo = false;
	}

	private void OnAdsClick(int action)
	{
		if (action == 1)
		{
			MagicTavernHelper.TrackADS("banner", 4, 1, MagicTavernHelper.ADS_FAILED.None);
		}
		else if (action == 2)
		{
			MagicTavernHelper.TrackADS("interstitial", 4, (int)ADSManager.slastShowChartBoostLocation, MagicTavernHelper.ADS_FAILED.None);
		}
		else if (action == 3)
		{
			MagicTavernHelper.TrackADS("rewardedVideo", 4, 1, MagicTavernHelper.ADS_FAILED.None);
		}
	}

	public void BannerShow()
	{
		if (!WJUtils.IsAdsVisible())
		{
			Common.ShowAds(Common.AdsPosition.Bottom);
		}
	}

	public static void HideAds()
	{
		WJUtils.HideAds();
	}
}
