using System;
using System.Collections.Generic;

public class AppsFlyerEventHelper
{
	public const string AF_DEV_KEY = "zcKrZYJWnrWWctCxcLNnyT";

	public static void Init(string iosAppId, string androidPackageName)
	{
		AppsFlyer.setAppsFlyerKey("zcKrZYJWnrWWctCxcLNnyT");
		AppsFlyer.setAppID(androidPackageName);
		AppsFlyer.init("zcKrZYJWnrWWctCxcLNnyT", "AppsFlyerTrackerCallbacks");
	}

	public static void TrackEvent(string eventName)
	{
		AppsFlyerEventHelper.TrackEvent(eventName, new Dictionary<string, string>());
	}

	public static void TrackEvent(string eventName, Dictionary<string, string> parameters)
	{
		AppsFlyer.trackRichEvent(eventName, parameters);
	}

	public static void TrackLevelAchievedEvent(int level)
	{
		AppsFlyerEventHelper.TrackEvent("af_level_achieved", new Dictionary<string, string>
		{
			{
				"af_level",
				level.ToString()
			}
		});
	}

	public static void TrackTutorialCompleteEvent(string tutorialId, string tutorialName)
	{
		AppsFlyerEventHelper.TrackEvent("af_tutorial_completion", new Dictionary<string, string>
		{
			{
				"af_success",
				"1"
			},
			{
				"af_content_id",
				tutorialId
			},
			{
				"af_content_type",
				tutorialName
			}
		});
	}

	public static void TrackPurchaseEvent(float revenue, string currency, string skuId, string skuName)
	{
		AppsFlyerEventHelper.TrackEvent("af_purchase", new Dictionary<string, string>
		{
			{
				"af_revenue",
				revenue.ToString()
			},
			{
				"af_currency",
				currency
			},
			{
				"af_content_id",
				skuId
			},
			{
				"af_content_type",
				skuName
			}
		});
	}

	public static void StopTracking()
	{
		AppsFlyer.stopTracking(true);
	}
}
