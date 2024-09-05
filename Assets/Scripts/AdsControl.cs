using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SocialPlatforms;
#if UNITY_ADS
using GoogleMobileAds.Api;
#endif
using System;
using UnityEngine.Advertisements;
using UnityEngine.UI;
public class AdsControl : MonoBehaviour
{


    protected AdsControl()
    {
    }

    private static AdsControl _instance;
#if UNITY_ADS
    InterstitialAd interstitial;
    RewardBasedVideoAd rewardBasedVideo;
    BannerView bannerView;
    ShowOptions options;
#endif
    public string AdmobID_Android, AdmobID_IOS, BannerID_Android, BannerID_IOS;

    public string UnityID_Android, UnityID_IOS, UnityZoneID;

    public static AdsControl Instance { get { return _instance; } }

    void Awake()
    {
        if (FindObjectsOfType(typeof(AdsControl)).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        MakeNewInterstial();
        RequestBanner();
        if (PlayerPrefs.GetInt("RemoveAds") == 0)
            ShowBanner();
        else
            HideBanner();
#if UNITY_ADS
        if (Advertisement.isSupported)
        { // If the platform is supported,
#if UNITY_IOS
            Advertisement.Initialize (UnityID_IOS); // initialize Unity Ads.
#endif

#if UNITY_ANDROID
            Advertisement.Initialize(UnityID_Android); // initialize Unity Ads.
#endif
        }
        options = new ShowOptions();
        options.resultCallback = HandleShowResult;
#endif
        DontDestroyOnLoad(gameObject); //Already done by CBManager


    }


    public void HandleInterstialAdClosed(object sender, EventArgs args)
    {
#if UNITY_ADS
        if (interstitial != null)
            interstitial.Destroy();
        MakeNewInterstial();
#endif


    }

    void MakeNewInterstial()
    {

#if UNITY_ADS
#if UNITY_ANDROID
        interstitial = new InterstitialAd(AdmobID_Android);
#endif
#if UNITY_IPHONE
		interstitial = new InterstitialAd (AdmobID_IOS);
#endif
        interstitial.OnAdClosed += HandleInterstialAdClosed;
        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
#endif

    }


    public void showAds()
    {
#if UNITY_ADS
        int adsCounter = PlayerPrefs.GetInt("AdsCounter");

        if (adsCounter >= 2)
        {
            if (PlayerPrefs.GetInt("RemoveAds") == 0)
            {
                if (interstitial.IsLoaded())
                    interstitial.Show();
                /*
                else
                    if (Advertisement.IsReady())

                    Advertisement.Show();
                  */                
            }
            adsCounter = 0;
        }
        else
        {
            adsCounter++;
        }

        PlayerPrefs.SetInt("AdsCounter", adsCounter);
#endif
    }

    public void showAdsNormal()
    {

#if UNITY_ADS
            if (PlayerPrefs.GetInt("RemoveAds") == 0)
            {
                if (interstitial.IsLoaded())
                    interstitial.Show();
                 /*
            else
                    if (Advertisement.IsReady())

                Advertisement.Show();
                  */              
        }
        
#endif
    }





    private void RequestBanner()
    {

#if UNITY_ADS

#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
		string adUnitId = BannerID_Android;
#elif UNITY_IPHONE
		string adUnitId = BannerID_IOS;
#else
		string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        bannerView.LoadAd(request);

#endif

    }

    public void ShowBanner()
    {
#if UNITY_ADS

        if (PlayerPrefs.GetInt("RemoveAds") == 0)
            bannerView.Show();

#endif
    }

    public void HideBanner()
    {
#if UNITY_ADS

        bannerView.Hide();

#endif
    }





    public bool GetRewardAvailable()
    {


        bool avaiable = false;
#if UNITY_ADS
        avaiable = Advertisement.IsReady();

#endif
        return avaiable;
    }

    public void ShowRewardVideo()
    {
#if UNITY_ADS
        Advertisement.Show(UnityZoneID, options);

#endif
    }
#if UNITY_ADS
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
              
                break;
            case ShowResult.Skipped:
                break;
            case ShowResult.Failed:
                break;
        }
    }
#endif
}

