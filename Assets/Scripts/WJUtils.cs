using LIBII;
using PlistCS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;

public class WJUtils
{
	public delegate void OnActionCallbackDelegate(int callbackTag, string callbackValue);

	public delegate void EventAppendWatermarkOver(Texture2D resultTexture);

	private class AppendWatermarkInfo
	{
		public bool NeedAppendWatermark;

		public float WaitTime = 1f;

		public GameObject CurWatermarkTemp;

		public float UICameraDepth;

		public WJUtils.EventAppendWatermarkOver OnAppendWatermarkOver;
	}

	public delegate void ChartboostParentControlCallbackDelegate();

	public enum ChartboostLocation
	{
		Default,
		Startup,
		HomeScreen,
		LevelComplete
	}

	public delegate void RateClickedCallbackDelegate();

	public delegate void RewardVideoCallbackDelegate(string getFreeLockKey, int rewarded);

	protected class Vector2Node
	{
		public Vector2 point = Vector2.zero;

		public WJUtils.Vector2Node next;
	}

	[StructLayout(LayoutKind.Explicit)]
	private struct FloatIntUnion
	{
		[FieldOffset(0)]
		public float f;

		[FieldOffset(0)]
		public int i;
	}

	public delegate void HuaWeiGiftCodeDelegate();

	public delegate void OfferwallCallbackDelegate(int credits);

	public delegate void ShareResultCallbackDelegate(bool result, string msg);

	public const int ACTION_VOID_OPENURL = 1;

	public const int ACTION_VOID_SENDMAIL = 2;

	public const int ACTION_VOID_CALLBACK_DIALOG_YESNO = 3;

	public const int ACTION_VOID_SHOWINFO = 4;

	public const int ACTION_RETSTR_GET_ALBUM_PATH = 5;

	public const int ACTION_VOID_RESCAN_ALBUM = 6;

	public const int ACTION_VOID_CALLBACK_DIALOG_OK = 7;

	public const int ACTION_VOID_UPLOAD_PHOTO_FACEBOOK = 8;

	public const int ACTION_VOID_DIALOG_SHOW_WAITING = 9;

	public const int ACTION_VOID_DIALOG_HIDE_WAITING = 10;

	public const int _ACTION_VOID_CALLBACK_INAPP_PURCHASE = 11;

	private const int _ACTION_VOID_SHOW_ADS = 12;

	private const int _ACTION_VOID_HIDE_ADS = 13;

	public const int ACTION_VOID_GAME_INIT_COMPLETE = 14;

	private const int _ACTION_VOID_REMOVE_UNUSED_TEXTURES = 15;

	public const int ACTION_VOID_CONFIRM_QUIT = 16;

	public const int ACTION_VOID_RESCAN_MEDIA_FILE = 17;

	public const int ACTION_RETSTR_GET_THISAPP_VERSION_NAME = 18;

	public const int _ACTION_VOID_MOREGAME_SHOW_BUTTON = 19;

	public const int _ACTION_VOID_MOREGAME_HIDE_BUTTON = 20;

	private const int _ACTION_VOID_MOREGAME_UPDATE = 21;

	private const int _ACTION_VOID_RATE_CHECK_ONSTARTUP = 22;

	private const int _ACTION_VOID_RATE_PROMPT = 23;

	private const int _ACTION_VOID_MOREGAME_SET_FOLDER = 24;

	private const int _ACTION_VOID_MOREGAME_SHOW_DIALOG = 25;

	public const int ACTION_RETSTR_GET_DEVICE_MODEL = 26;

	public const int ACTION_VOID_CALLBACK_DIALOG_3BUTTONS = 27;

	private const int _ACTION_RETSTR_GET_ASSETS_FOLDER_FILES = 28;

	public const int ACTION_VOID_AWARD_SHOW_DIALOG = 29;

	private const int _ACTION_VOID_CALLBACK_ARCAMERA = 30;

	public const int ACTION_VOID_CHARTBOOST = 31;

	private const int _ACTION_RETSTR_BACK_PRESSED = 32;

	public const int ACTION_RETSTR_GET_DEVICE_TYPE = 33;

	public const int ACTION_IOS_SAVEPHOTO_TO_ALBUM = 34;

	private const int _ACTION_VOID_REMOVE_ADS = 35;

	private const int _ACTION_RETSTR_ADS_VISIBLE = 36;

	private const int _ACTION_VOID_CALLBACK_INAPP_RESTORE = 37;

	public const int ACTION_RETSTR_GET_DEVICE_ORIENTATION = 38;

	private const int _ACTION_RETSTR_ISADSREMOVED = 39;

	public const int ACTION_RETSTR_TEMPORARY_PATH = 40;

	private const int _ACTION_VOID_PREPARE_ADS_FULLSCREEN = 41;

	private const int _ACTION_VOID_SHOW_ADS_FULLSCREEN = 42;

	public const int ACTION_VOID_UMENG = 43;

	public const int ACTION_RETSTR_IOS_CACHE_PATH = 44;

	public const int ACTION_VOID_CALLBACK_TALKING_START_RECORD = 45;

	public const int ACTION_VOID_TALKING_STOP_RECORD = 46;

	public const int ACTION_VOID_CALLBACK_TALKING_START_PLAY = 47;

	public const int ACTION_VOID_TALKING_STOP_PLAYING = 48;

	private const int _ACTION_VOID_LOCAL_NOTIFI_CLEARALL = 49;

	private const int _ACTION_VOID_LOCAL_NOTIFI_ADD = 50;

	public const int ACTION_VOID_ADCOLONY_PLAY_VIDEO = 51;

	public const int ACTION_RETSTR_ADCOLONY_IS_READY = 52;

	private const int _ACTION_VOID_CALLBACK_ADCOLONY_REWARD = 53;

	private const int _ACTION_VOID_CALLBACK_ADCOLONY_VIDEO_START = 54;

	private const int _ACTION_VOID_CALLBACK_ADCOLONY_VIDEO_STOP = 55;

	private const int _ACTION_BACK_WITH_NOTIFICATION = 56;

	private const int _ACTION_RETSTR_ADS_REALLY_VISIBLE = 57;

	private const int _ACTION_VOID_GAMECENTER_LOGIN = 58;

	private const int _ACTION_VOID_GAMECENTER_SHOW_LEADERBOARD = 59;

	private const int _ACTION_VOID_GAMECENTER_SHOW_ACHIEVEMENT = 60;

	private const int _ACTION_VOID_GAMECENTER_SUBMIT_SCORE = 61;

	private const int _ACTION_VOID_GAMECENTER_SUBMIT_ACHIEVEMENT = 62;

	private const int _ACTION_VOID_FORPARENTS_SHOW_DIALOG = 63;

	private const int _ACTION_RETSTR_CHECK_NETWORK_AVAILABLE = 64;

	private const int ACTION_RETSTR_GET_IFA = 65;

	private const int ACTION_RETSTR_GET_MAC = 66;

	private const int ACTION_RETSTR_GET_UDID = 67;

	public const int ACTION_RETSTR_GET_IDFA_ORIGINAL = 105;

	public const int ACTION_RETSTR_GET_IDFV_ORIGINAL = 112;

	private const int _ACTION_VOID_CALLBACK_MOREGAME_SHOW = 68;

	private const int _ACTION_VOID_CALLBACK_MOREGAME_CLOSE = 69;

	private const int _ACTION_VOID_CALLBACK_MOREGAME_SELECTED = 70;

	private const int ACTION_VOID_IOS_SHARE_PHOTO = 71;

	private const int ACTION_RETSTR_GET_DEVICE_OS_VERSION = 72;

	private const int _ACTION_RETSTR_GET_ANDROID_PACKAGENAME = 73;

	private const int _ACTION_VOID_CALLBACK_MOREGAME_BUTTON_CLICKED = 74;

	private const int _ACTION_RETSTR_GET_APK_SIGNATURES = 77;

	private const int _ACTION_RETSTR_GET_PLATFORM_STORE_NAME = 78;

	private const int _ACTION_RETSTR_GET_MOREGAME_ENABLED = 79;

	private const int _ACTION_RETSTR_SYSTEM_UPTIME = 80;

	private const int _ACTION_VOID_CALLBACK_CB_SHOWPARENTCONTROL = 81;

	private const int _ACTION_VOID_CB_PARENTCONTROL_RESULT = 82;

	private const int _ACTION_VOID_MOREGAME_DIALOG_SET_CLICK_CALLBACK = 83;

	private const int _ACTION_VOID_CALLBACK_MOREGAME_DIALOG_CLICKED = 84;

	private const int _ACTION_VOID_MOREGAME_DIALOG_CLICK_CONTINUE = 85;

	private const int _ACTION_VOID_RATE_SET_CLICK_CALLBACK = 86;

	private const int _ACTION_VOID_CALLBACK_RATE_CLICKED = 87;

	private const int _ACTION_VOID_RATE_DO_RATING = 88;

	private const int _ACTION_VOID_CALLBACK_OFFERWALL = 91;

	private const int _ACTION_VOID_SHOW_OFFERWALL = 92;

	private const int _ACTION_RETSTR_IS_OFFERWALL_READY = 93;

	private const int _ACTION_VOID_CALLBACK_PAUSE_SOUND = 94;

	private const int _ACTION_VOID_CALLBACK_RESUME_SOUND = 95;

	private const int _ACTION_VOID_CHECK_OFFERWALL_REWARD = 96;

	private const int _ACTION_RETSTR_GET_ADS_REAL_SIZE = 102;

	private const int _ACTION_RETSTR_CHECK_WIFI_AVAILABLE = 104;

	private const int _ACTION_RETSTR_GET_DEVICE_OS_FULLNAME = 106;

	private const int _ACTION_RETSTR_GET_COMPASSBI_STYLE_DEVICE = 107;

	private const int _ACTION_VOID_GIFT_CODE_SHOW_DIALOG = 108;

	private const int _ACTION_VOID_CALLBACK_GIFT_CODE_SUCCESS = 109;

	private const int _ACTION_VOID_HUAWEI_SYNC_IAP = 110;

	private const int _ACTION_RETSTR_GET_TIME_ZONE = 111;

	private const int _ACTION_RETSTR_GET_SYSTEM_LANGUAGE = 113;

	private const int _ACTION_RETSTR_GET_LOCALIZED_STR = 114;

	private const int _ACTION_VOID_SHOW_HUAWEI_SIGNIN_BUTTON = 115;

	private const int _ACTION_VOID_HIDE_HUAWEI_SIGNIN_BUTTON = 116;

	private const int ACTION_VOID_VIBRATE = 117;

	private const int ACTION_RETSTR_INTERSTITIAL_READY = 118;

	private const int _ACTION_VOID_CALLBACK_INTERSTITIAL_START = 119;

	private const int _ACTION_VOID_CALLBACK_INTERSTITIAL_STOP = 120;

	private const int _ACTION_VOID_CALLBACK_INTERSTITIAL_FAIL = 121;

	private const int _ACTION_VOID_CALLBACK_ADS_CLICK = 122;

	private const int ACTION_VOID_REGISTER_NOFITIFCATION = 123;

	private const int _ACTION_VOID_FIRST_TIME_RATE = 124;

	private const int ACTION_VOID_IOS_RICH_SHARE_PHOTO = 125;

	private const int _ACTION_RETSTR_GET_LIBII_CHANNEL_NAME = 126;

	private const int _ACTION_VOID_CALLBACK_SUBSCRIPTION = 127;

	private const int _ACTION_VOID_CHECK_SUBSCRIPTION = 128;

	private const int _ACTION_VOID_CALLBACK_INAPP_PURCHASE_FAIL = 129;

	private const int _ACTION_VOID_SHOW_SUBSCRIPTION_INFO = 130;

	public const int _ACTION_VOID_CALLBACK_SHOW_GDPR_DIALOG = 131;

	public const int _ACTION_VOID_GDPR_AD_STATUS = 132;

	public const int _ACTION_VOID_UI_IMPACT_FEEDBACK = 133;

	public const int _ACTION_RETSTR_LOCALE_COUNTRY_CODE = 134;

	public const int _ACTION_VOID_LIBII_CROSS_INTERSTITIAL = 135;

	public const int _ACTION_RETSTR_LIBII_ANDROID_NATIVE_VERSION = 136;

	public const int _ACTION_VOID_CHECK_IAP_PURCHASE_FINISH = 137;

	public const int ACTION_VOID_CALLBACK_SHARE_RESULT = 999;

	public const string ACTION_RESULT_YES = "Y";

	public const string ACTION_RESULT_NO = "N";

	public const string ACTION_RESULT_OK = "O";

	public const string ACTION_RESULT_IAP_RESTORE_SUCCESS = "R";

	public const char ADSIZE_BANNER = '1';

	public const char ADSIZE_IAB_MRECT = '2';

	public const char ADSIZE_IAB_BANNER = '3';

	public const char ADSIZE_IAB_LEADERBOARD = '4';

	public const char ADSIZE_BANNER_AUTO = '5';

	public const int ADPOS_X_LEFT = -1;

	public const int ADPOS_X_CENTER = -2;

	public const int ADPOS_Y_TOP = -1;

	public const int ADPOS_Y_BOTTOM = -2;

	public const int ADWH_AUTO = 0;

	public const int ADWH_FULLSCREEN = -1;

	public const int MOREGAME_BUTTON_POS_X_RIGHT = -1;

	public const int MOREGAME_BUTTON_POS_Y_BOTTOM = -1;

	public const char MOREGAME_TEXT_HIDE = '0';

	public const char MOREGAME_TEXT_COLOR_WHITE = '1';

	public const char MOREGAME_TEXT_COLOR_BLACK = '2';

	public const string DEVICE_ORIENTATION_UNKNOWN = "0";

	public const string DEVICE_ORIENTATION_FACEUP = "1";

	public const string DEVICE_ORIENTATION_FACEDOWN = "2";

	public const string DEVICE_ORIENTATION_LandscapeLeft = "3";

	public const string DEVICE_ORIENTATION_LandscapeRight = "4";

	public const string DEVICE_ORIENTATION_Portrait = "5";

	public const string DEVICE_ORIENTATION_PortraitUpsideDown = "6";

	private static WJUtils s_instance;

	public static bool s_started = false;

	private static Dictionary<string, float> s_canClickLastTimeMap = new Dictionary<string, float>();

	private static WJUtils.OnActionCallbackDelegate last_callback_delegate = null;

	private static string[] ACTION_MESSAGE_SEPARATOR = new string[]
	{
		"#$#"
	};

	public static bool s_removeAdsByPurchaseAnything = true;

	private static Action<string> OnInAppPurchaseRetrySuccess = null;

	private static bool s_iapDisabled = false;

	public static Action<string> OnSubscription = null;

	public static Action<string> OnIapFail = null;

	private static List<object> sProductInfoListCache = null;

	private static RenderTexture s_lastCSRender = null;

	private static Texture2D s_lastCSTexture = null;

	private static string s_lastCSFromFileName = null;

	private static bool isCapFullScreen = true;

	private static Vector2 capSize = Vector2.zero;

	private static WJUtils.ChartboostParentControlCallbackDelegate OnChartboostParentControlCallback = null;

	private static Action onInterstitialStart = null;

	private static Action onInterstitialStop = null;

	private static Action onInterstitialFail = null;

	private static Action<int> onAdsClick = null;

	private static bool mIsShowChartboostOnPlayButtonPlayed = false;

	private static WJUtils.RateClickedCallbackDelegate OnRateClickedCallback = null;

	private static string s_deviceType = null;

	private static string s_storeName = null;

	private static string sChannelName = null;

	private static Dictionary<string, bool> s_getFreeUnlockedMap = new Dictionary<string, bool>();

	private static WJUtils.RewardVideoCallbackDelegate OnGetFreeRewardVideoCallback = null;

	private static WJUtils.RewardVideoCallbackDelegate OnGetFreeRewardVideoClose = null;

	private static WJUtils.RewardVideoCallbackDelegate OnGetFreeRewardVideoStart = null;

	private static string s_lastWatchGetFreeLockKey = string.Empty;

	private static List<string> s_filePaths = new List<string>();

	private static float[] s_sin_values = new float[]
	{
		0f,
		0.01745f,
		0.0349f,
		0.05234f,
		0.06976f,
		0.08716f,
		0.10453f,
		0.12187f,
		0.13917f,
		0.15643f,
		0.17365f,
		0.19081f,
		0.20791f,
		0.22495f,
		0.24192f,
		0.25882f,
		0.27564f,
		0.29237f,
		0.30902f,
		0.32557f,
		0.34202f,
		0.35837f,
		0.37461f,
		0.39073f,
		0.40674f,
		0.42262f,
		0.43837f,
		0.45399f,
		0.46947f,
		0.48481f,
		0.5f,
		0.51504f,
		0.52992f,
		0.54464f,
		0.55919f,
		0.57358f,
		0.58779f,
		0.60182f,
		0.61566f,
		0.62932f,
		0.64279f,
		0.65606f,
		0.66913f,
		0.682f,
		0.69466f,
		0.70711f,
		0.71934f,
		0.73135f,
		0.74314f,
		0.75471f,
		0.76604f,
		0.77715f,
		0.78801f,
		0.79864f,
		0.80902f,
		0.81915f,
		0.82904f,
		0.83867f,
		0.84805f,
		0.85717f,
		0.86603f,
		0.87462f,
		0.88295f,
		0.89101f,
		0.89879f,
		0.90631f,
		0.91355f,
		0.9205f,
		0.92718f,
		0.93358f,
		0.93969f,
		0.94552f,
		0.95106f,
		0.9563f,
		0.96126f,
		0.96593f,
		0.9703f,
		0.97437f,
		0.97815f,
		0.98163f,
		0.98481f,
		0.98769f,
		0.99027f,
		0.99255f,
		0.99452f,
		0.99619f,
		0.99756f,
		0.99863f,
		0.99939f,
		0.99985f,
		1f,
		0.99985f,
		0.99939f,
		0.99863f,
		0.99756f,
		0.99619f,
		0.99452f,
		0.99255f,
		0.99027f,
		0.98769f,
		0.98481f,
		0.98163f,
		0.97815f,
		0.97437f,
		0.9703f,
		0.96593f,
		0.96126f,
		0.9563f,
		0.95106f,
		0.94552f,
		0.93969f,
		0.93358f,
		0.92718f,
		0.9205f,
		0.91355f,
		0.90631f,
		0.89879f,
		0.89101f,
		0.88295f,
		0.87462f,
		0.86603f,
		0.85717f,
		0.84805f,
		0.83867f,
		0.82904f,
		0.81915f,
		0.80902f,
		0.79864f,
		0.78801f,
		0.77715f,
		0.76604f,
		0.75471f,
		0.74314f,
		0.73135f,
		0.71934f,
		0.70711f,
		0.69466f,
		0.682f,
		0.66913f,
		0.65606f,
		0.64279f,
		0.62932f,
		0.61566f,
		0.60182f,
		0.58779f,
		0.57358f,
		0.55919f,
		0.54464f,
		0.52992f,
		0.51504f,
		0.5f,
		0.48481f,
		0.46947f,
		0.45399f,
		0.43837f,
		0.42262f,
		0.40674f,
		0.39073f,
		0.37461f,
		0.35837f,
		0.34202f,
		0.32557f,
		0.30902f,
		0.29237f,
		0.27564f,
		0.25882f,
		0.24192f,
		0.22495f,
		0.20791f,
		0.19081f,
		0.17365f,
		0.15643f,
		0.13917f,
		0.12187f,
		0.10453f,
		0.08716f,
		0.06976f,
		0.05234f,
		0.0349f,
		0.01745f,
		0f,
		-0.01745f,
		-0.0349f,
		-0.05234f,
		-0.06976f,
		-0.08716f,
		-0.10453f,
		-0.12187f,
		-0.13917f,
		-0.15643f,
		-0.17365f,
		-0.19081f,
		-0.20791f,
		-0.22495f,
		-0.24192f,
		-0.25882f,
		-0.27564f,
		-0.29237f,
		-0.30902f,
		-0.32557f,
		-0.34202f,
		-0.35837f,
		-0.37461f,
		-0.39073f,
		-0.40674f,
		-0.42262f,
		-0.43837f,
		-0.45399f,
		-0.46947f,
		-0.48481f,
		-0.5f,
		-0.51504f,
		-0.52992f,
		-0.54464f,
		-0.55919f,
		-0.57358f,
		-0.58779f,
		-0.60182f,
		-0.61566f,
		-0.62932f,
		-0.64279f,
		-0.65606f,
		-0.66913f,
		-0.682f,
		-0.69466f,
		-0.70711f,
		-0.71934f,
		-0.73135f,
		-0.74314f,
		-0.75471f,
		-0.76604f,
		-0.77715f,
		-0.78801f,
		-0.79864f,
		-0.80902f,
		-0.81915f,
		-0.82904f,
		-0.83867f,
		-0.84805f,
		-0.85717f,
		-0.86603f,
		-0.87462f,
		-0.88295f,
		-0.89101f,
		-0.89879f,
		-0.90631f,
		-0.91355f,
		-0.9205f,
		-0.92718f,
		-0.93358f,
		-0.93969f,
		-0.94552f,
		-0.95106f,
		-0.9563f,
		-0.96126f,
		-0.96593f,
		-0.9703f,
		-0.97437f,
		-0.97815f,
		-0.98163f,
		-0.98481f,
		-0.98769f,
		-0.99027f,
		-0.99255f,
		-0.99452f,
		-0.99619f,
		-0.99756f,
		-0.99863f,
		-0.99939f,
		-0.99985f,
		-1f,
		-0.99985f,
		-0.99939f,
		-0.99863f,
		-0.99756f,
		-0.99619f,
		-0.99452f,
		-0.99255f,
		-0.99027f,
		-0.98769f,
		-0.98481f,
		-0.98163f,
		-0.97815f,
		-0.97437f,
		-0.9703f,
		-0.96593f,
		-0.96126f,
		-0.9563f,
		-0.95106f,
		-0.94552f,
		-0.93969f,
		-0.93358f,
		-0.92718f,
		-0.9205f,
		-0.91355f,
		-0.90631f,
		-0.89879f,
		-0.89101f,
		-0.88295f,
		-0.87462f,
		-0.86603f,
		-0.85717f,
		-0.84805f,
		-0.83867f,
		-0.82904f,
		-0.81915f,
		-0.80902f,
		-0.79864f,
		-0.78801f,
		-0.77715f,
		-0.76604f,
		-0.75471f,
		-0.74314f,
		-0.73135f,
		-0.71934f,
		-0.70711f,
		-0.69466f,
		-0.682f,
		-0.66913f,
		-0.65606f,
		-0.64279f,
		-0.62932f,
		-0.61566f,
		-0.60182f,
		-0.58779f,
		-0.57358f,
		-0.55919f,
		-0.54464f,
		-0.52992f,
		-0.51504f,
		-0.5f,
		-0.48481f,
		-0.46947f,
		-0.45399f,
		-0.43837f,
		-0.42262f,
		-0.40674f,
		-0.39073f,
		-0.37461f,
		-0.35837f,
		-0.34202f,
		-0.32557f,
		-0.30902f,
		-0.29237f,
		-0.27564f,
		-0.25882f,
		-0.24192f,
		-0.22495f,
		-0.20791f,
		-0.19081f,
		-0.17365f,
		-0.15643f,
		-0.13917f,
		-0.12187f,
		-0.10453f,
		-0.08716f,
		-0.06976f,
		-0.05234f,
		-0.0349f,
		-0.01745f
	};

	protected static int sNetworkAvailableCheckTimes = 0;

	public static WJUtils.HuaWeiGiftCodeDelegate OnHuaWeiGiftCodeSuccessed = null;

	private static WJUtils.OfferwallCallbackDelegate OnOfferwallCallback = null;

	private static Action OnShowGDPRDialog = null;


    /*
	public static event Action<string> OnInAppPurchaseRetrySuccess;

	public static event WJUtils.ChartboostParentControlCallbackDelegate OnChartboostParentControlCallback;

	public static event Action onInterstitialStart;

	public static event Action onInterstitialStop;

	public static event Action onInterstitialFail;

	public static event Action<int> onAdsClick;

	public static event WJUtils.RateClickedCallbackDelegate OnRateClickedCallback;

	public static event WJUtils.RewardVideoCallbackDelegate OnGetFreeRewardVideoCallback;

	public static event WJUtils.RewardVideoCallbackDelegate OnGetFreeRewardVideoClose;

	public static event WJUtils.RewardVideoCallbackDelegate OnGetFreeRewardVideoStart;

	public static event WJUtils.OfferwallCallbackDelegate OnOfferwallCallback;

	public static event Action OnShowGDPRDialog;

	public static event WJUtils.ShareResultCallbackDelegate OnShareResultCallback;
    */
	public static WJUtils SharedInstance
	{
		get
		{
			if (WJUtils.s_instance == null)
			{
				WJUtils.s_instance = new WJUtils();
			}
			return WJUtils.s_instance;
		}
	}

	public static void Start(string packageName, string signature)
	{
		WJUtils.s_started = true;
		if (PlayerPrefs.GetInt("isFirstRunAfterInstall2", 1) == 1)
		{
			PlayerPrefs.SetInt("isFirstRunAfterInstall2", 0);
			PlayerPrefs.Save();
		}
		else
		{
			PlayerPrefs.SetInt("isFirstRunAfterInstall", 0);
			PlayerPrefs.Save();
		}
		string text = WJUtils.CallAction_Retstr(73, string.Empty);
		if (!text.Equals(packageName))
		{
			UnityEngine.Debug.Log("Package Name Mismatch. Android Package Name : " + text + ", Unity Package Name : " + packageName);
			Application.Quit();
		}
		if (!WJUtils.CheckApkSignature(signature))
		{
			Application.Quit();
		}
		WJUtils.CheckIapPurchaseFinish();
	}

	private static bool CheckApkSignature(string signature)
	{
		byte[] array = new byte[10];
		for (int i = 0; i < 10; i++)
		{
			array[i] = (byte)(33 + 96217 * (i + 1) % 87);
		}
		string s = WJUtils.CallAction_Retstr(77, string.Empty);
		byte[] bytes = Encoding.ASCII.GetBytes(s);
		int num = bytes.Length;
		int num2 = 0;
		for (int j = 0; j < num; j += 2)
		{
			bytes[j] ^= array[num2];
			if (++num2 >= 10)
			{
				num2 = 0;
			}
		}
		string input = Convert.ToBase64String(bytes);
		string text = WJUtils.Md5(input);
		if (!signature.Equals(text))
		{
			if (WJUtils.IsFirstRunAfterInstall())
			{
				UnityEngine.Debug.LogError("APK Signature : " + text);
			}
			return false;
		}
		return true;
	}

	public static void SetLibiiNativeAndroidVersion(int version = 2)
	{
		if (WJUtils.CallAction_Retstr(136, version.ToString()) == "Y")
		{
			PlayerPrefs.SetInt("IsNewLibiiAndroidNativeVersion", 1);
		}
	}

	public static bool IsUseNewAndroidAdRule()
	{
		return PlayerPrefs.GetInt("IsNewLibiiAndroidNativeVersion", 0) == 1;
	}

	public static void CheckIapPurchaseFinish()
	{
		WJUtils.CallAction_Void(137, string.Empty);
	}

	public static bool IsFirstRunAfterInstall()
	{
		return PlayerPrefs.GetInt("isFirstRunAfterInstall", 1) == 1;
	}

	public static string GetSystemYMDHMS()
	{
		return DateTime.Now.ToString("yyyyMMddHHmmss");
	}

	public static string GetSystemYMD()
	{
		return DateTime.Now.ToString("yyyyMMdd");
	}

	public static int RandomInt(int count)
	{
		return UnityEngine.Random.Range(0, count);
	}

	public static bool CanClick(float t = 0.5f)
	{
		return WJUtils.CanClick("_default_", t);
	}

	public static bool CanClick(string key, float t = 0.5f)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float num;
		if (!WJUtils.s_canClickLastTimeMap.TryGetValue(key, out num))
		{
			WJUtils.s_canClickLastTimeMap.Add(key, realtimeSinceStartup);
			return true;
		}
		if (realtimeSinceStartup - num > t)
		{
			WJUtils.s_canClickLastTimeMap[key] = realtimeSinceStartup;
			return true;
		}
		return false;
	}

	public static void OpenURL(string url)
	{
		WJUtils.CallAction_Void(1, url);
	}

	public static void SendEMail(string toAddress, string subject, string content = "", string attachment = "")
	{
		StringBuilder stringBuilder = new StringBuilder(toAddress);
		stringBuilder.Append("|").Append(subject).Append("|").Append(content).Append("|").Append(attachment);
		WJUtils.CallAction_Void(2, stringBuilder.ToString());
	}

	public static void SendEMail(string allInOne)
	{
		WJUtils.CallAction_Void(2, allInOne);
	}

	public static void FacebookUploadPhoto(string photoFileName, string description = "", string facebookId = "")
	{
		StringBuilder stringBuilder = new StringBuilder(facebookId);
		stringBuilder.Append("|").Append(photoFileName).Append("|").Append(description);
		WJUtils.CallAction_Void(8, stringBuilder.ToString());
	}

	public static void SharePhoto(string photoFileName, string description, string facebookId)
	{
		WJUtils.FacebookUploadPhoto(photoFileName, description, facebookId);
	}

	public static void RichSharePhoto(string photoFileName, string description, string URL, string hashTag)
	{
		WJUtils.SharePhoto(photoFileName, description, string.Empty);
	}

	public static Vector2 ParsePositionString(string str)
	{
		int num = str.IndexOf(',');
		if (num <= 0)
		{
			return Vector2.zero;
		}
		float x = 0f;
		float y = 0f;
		float.TryParse(str.Substring(1, num - 1), out x);
		float.TryParse(str.Substring(num + 1, str.Length - num - 2), out y);
		return new Vector2(x, y);
	}

	public static Vector3 ParseVector3String(string str)
	{
		Vector3 result = default(Vector3);
		int num = str.IndexOf(',');
		float.TryParse(str.Substring(1, num - 1), out result.x);
		int num2 = str.IndexOf(',', num + 1);
		float.TryParse(str.Substring(num + 2, num2 - num - 2), out result.y);
		float.TryParse(str.Substring(num2 + 2, str.Length - num2 - 3), out result.z);
		return result;
	}

	public static Vector2 ParseVector2String(string str)
	{
		Vector2 result = default(Vector2);
		int num = str.IndexOf(',');
		float.TryParse(str.Substring(1, num - 1), out result.x);
		float.TryParse(str.Substring(num + 2, str.Length - num - 3), out result.y);
		return result;
	}

	public static string GetThisAppVerName()
	{
		return WJUtils.CallAction_Retstr(18, string.Empty);
	}

	public static float Dp2Pixel(float dp)
	{
		if (Screen.dpi == 0f)
		{
			return dp;
		}
		return dp * (Screen.dpi / 160f);
	}

	public static Vector2 Dp2Pixel(Vector2 dp)
	{
		if (Screen.dpi == 0f)
		{
			return dp;
		}
		return dp * (Screen.dpi / 160f);
	}

	public static Vector3 Dp2Pixel(Vector3 dp)
	{
		if (Screen.dpi == 0f)
		{
			return dp;
		}
		return dp * (Screen.dpi / 160f);
	}

	public static float Pixel2Dp(float pixel)
	{
		if (Screen.dpi == 0f)
		{
			return pixel;
		}
		return pixel * (160f / Screen.dpi);
	}

	public static Vector2 Pixel2Dp(Vector2 pixel)
	{
		if (Screen.dpi == 0f)
		{
			return pixel;
		}
		return pixel * (160f / Screen.dpi);
	}

	public static Vector3 Pixel2Dp(Vector3 pixel)
	{
		if (Screen.dpi == 0f)
		{
			return pixel;
		}
		return pixel * (160f / Screen.dpi);
	}

	public static bool ScreenPointRaycastClosestPoint(Vector3 screenPt, out RaycastHit hitInfo, int layerMask, float distance = 3.40282347E+38f, Camera camera = null)
	{
		hitInfo = default(RaycastHit);
		if (camera == null)
		{
			camera = Camera.main;
		}
		Ray ray = camera.ScreenPointToRay(screenPt);
		RaycastHit[] array = Physics.RaycastAll(ray, distance, layerMask);
		if (array != null && array.Length > 0)
		{
			float num = 3.40282347E+38f;
			RaycastHit[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				RaycastHit raycastHit = array2[i];
				if (raycastHit.distance < num)
				{
					num = raycastHit.distance;
					hitInfo = raycastHit;
				}
			}
			return true;
		}
		return false;
	}

	public static bool IsScreenPointHitGameObject(Vector3 screenPt, GameObject go, Camera camera, out RaycastHit hitInfo, bool rayCastAll = false)
	{
		bool result = false;
		hitInfo = default(RaycastHit);
		if (camera != null)
		{
			Ray ray = camera.ScreenPointToRay(screenPt);
			if (rayCastAll)
			{
				RaycastHit[] array = Physics.RaycastAll(ray);
				RaycastHit[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					RaycastHit raycastHit = array2[i];
					if (raycastHit.collider.gameObject == go)
					{
						hitInfo = raycastHit;
						result = true;
						UnityEngine.Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1f);
						break;
					}
				}
			}
			else if (Physics.Raycast(ray, out hitInfo))
			{
				UnityEngine.Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1f);
				result = (hitInfo.collider.gameObject == go);
			}
		}
		return result;
	}

	public static bool IsScreenPointHitGameObject(Vector3 screenPt, GameObject go, Camera camera)
	{
		RaycastHit raycastHit = default(RaycastHit);
		return WJUtils.IsScreenPointHitGameObject(screenPt, go, camera, out raycastHit, false);
	}

	public static bool IsScreenPointHitGameObject(Vector3 screenPt, GameObject go, Camera camera, bool rayCastAll)
	{
		RaycastHit raycastHit = default(RaycastHit);
		return WJUtils.IsScreenPointHitGameObject(screenPt, go, camera, out raycastHit, rayCastAll);
	}

	public static bool IsScreenPointHitGameObject(Vector3 screenPt, GameObject go, bool rayCastAll = false)
	{
		RaycastHit raycastHit = default(RaycastHit);
		return WJUtils.IsScreenPointHitGameObject(screenPt, go, Camera.main, out raycastHit, rayCastAll);
	}

	public static bool IsScreenPointHitGameObject(Vector3 screenPt, GameObject go, out RaycastHit hitInfo, bool rayCastAll = false)
	{
		return WJUtils.IsScreenPointHitGameObject(screenPt, go, Camera.main, out hitInfo, rayCastAll);
	}

	public static bool IsWorldPointHitGameObject(Vector3 worldPoint, GameObject go, Camera camera, out RaycastHit hitInfo, bool rayCastAll = false)
	{
		bool result = false;
		hitInfo = default(RaycastHit);
		if (camera != null)
		{
			Vector3 position = camera.gameObject.transform.position;
			Ray ray = new Ray(position, worldPoint - position);
			if (rayCastAll)
			{
				RaycastHit[] array = Physics.RaycastAll(ray);
				RaycastHit[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					RaycastHit raycastHit = array2[i];
					if (raycastHit.collider.gameObject == go)
					{
						hitInfo = raycastHit;
						result = true;
						UnityEngine.Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1f);
						break;
					}
				}
			}
			else if (Physics.Raycast(ray, out hitInfo))
			{
				UnityEngine.Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1f);
				result = (hitInfo.collider.gameObject == go);
			}
		}
		return result;
	}

	public static bool IsWorldPointHitGameObject(Vector3 worldPoint, GameObject go, Camera camera)
	{
		RaycastHit raycastHit = default(RaycastHit);
		return WJUtils.IsWorldPointHitGameObject(worldPoint, go, camera, out raycastHit, false);
	}

	public static bool IsWorldPointHitGameObject(Vector3 worldPoint, GameObject go, Camera camera, bool rayCastAll)
	{
		RaycastHit raycastHit = default(RaycastHit);
		return WJUtils.IsWorldPointHitGameObject(worldPoint, go, camera, out raycastHit, rayCastAll);
	}

	public static bool IsWorldPointHitGameObject(Vector3 worldPoint, GameObject go, bool rayCastAll = false)
	{
		RaycastHit raycastHit = default(RaycastHit);
		return WJUtils.IsWorldPointHitGameObject(worldPoint, go, Camera.main, out raycastHit, rayCastAll);
	}

	public static bool IsWorldPointHitGameObject(Vector3 worldPoint, GameObject go, out RaycastHit hitInfo, bool rayCastAll = false)
	{
		return WJUtils.IsWorldPointHitGameObject(worldPoint, go, Camera.main, out hitInfo, rayCastAll);
	}

	public static T FindComponent<T>(GameObject go, string name) where T : Component
	{
		go = WJUtils.FindGameObjectByNameInAllChildren(go, name);
		return (!(go == null)) ? go.GetComponent<T>() : ((T)((object)null));
	}

	public static GameObject FindGameObjectByName(GameObject parent, string name)
	{
		if (parent == null)
		{
			return null;
		}
		Transform transform = parent.transform;
		IEnumerator enumerator = transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform2 = (Transform)enumerator.Current;
				if (transform2.gameObject.name == name)
				{
					return transform2.gameObject;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return null;
	}

	public static GameObject FindGameObjectByNameInAllChildren(GameObject parent, string name)
	{
		if (parent == null)
		{
			return null;
		}
		GameObject gameObject = WJUtils.FindGameObjectByName(parent, name);
		if (gameObject != null)
		{
			return gameObject;
		}
		Transform transform = parent.transform;
		IEnumerator enumerator = transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform2 = (Transform)enumerator.Current;
				gameObject = WJUtils.FindGameObjectByNameInAllChildren(transform2.gameObject, name);
				if (gameObject != null)
				{
					return gameObject;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return null;
	}

	public static GameObject InstantiatePrefab(string prefabFullpath)
	{
		return UnityEngine.Object.Instantiate(Resources.Load(prefabFullpath)) as GameObject;
	}

	public static GameObject InstantiatePrefab(string prefabFullpath, GameObject parent)
	{
		return UnityEngine.Object.Instantiate(Resources.Load(prefabFullpath), parent.transform) as GameObject;
	}

	public static GameObject InstantiatePrefab(string prefabFullpath, Vector3 pos, Quaternion rotation, GameObject parent)
	{
		return UnityEngine.Object.Instantiate(Resources.Load(prefabFullpath), pos, rotation, parent.transform) as GameObject;
	}

	public static GameObject AddChild(GameObject parent, GameObject child, bool layerFollowParent = true)
	{
		if (layerFollowParent)
		{
			child.layer = parent.layer;
		}
		child.transform.parent = parent.transform;
		child.transform.localScale = Vector3.one;
		child.transform.localRotation = Quaternion.identity;
		child.transform.localPosition = Vector3.zero;
		return child;
	}

	public static IntPtr CreateAnsi(string str)
	{
		return Marshal.StringToHGlobalAnsi(str);
	}

	public static void ReleaseAnsi(IntPtr ptr)
	{
		Marshal.FreeHGlobal(ptr);
	}

	public static string AnsiToStr(IntPtr ptr)
	{
		return Marshal.PtrToStringAuto(ptr);
	}

	public static void CallAction_Void(int actiontype, string param = "")
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("wj.utils.LBLibraryAndroidUnity");
		androidJavaClass.CallStatic("android_callaction_void", new object[]
		{
			actiontype,
			param
		});
	}

	public static void CallAction_Void_Callback(int actiontype, int callbackTag, string param = "", WJUtils.OnActionCallbackDelegate callbackDelegate = null)
	{
		WJUtils.last_callback_delegate = callbackDelegate;
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("wj.utils.LBLibraryAndroidUnity");
		androidJavaClass.CallStatic("android_callaction_void_callback", new object[]
		{
			actiontype,
			callbackTag,
			param
		});
	}

	public static string CallAction_Retstr(int actiontype, string param = "")
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("wj.utils.LBLibraryAndroidUnity");
		return androidJavaClass.CallStatic<string>("android_callaction_retstring", new object[]
		{
			actiontype,
			param
		});
	}

	public static void Broadcast(string funcName)
	{
		GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			array[i].SendMessage(funcName, SendMessageOptions.DontRequireReceiver);
			i++;
		}
	}

	public static void Broadcast(string funcName, object param)
	{
		GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			array[i].SendMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
			i++;
		}
	}

	public static void Native_Callback(string param)
	{
		string[] array = param.Split(WJUtils.ACTION_MESSAGE_SEPARATOR, StringSplitOptions.None);
		if (array.Length != 3)
		{
			return;
		}
		int num = int.Parse(array[0]);
		string text = array[1];
		int num2 = int.Parse(array[2]);
		if (num2 == 56)
		{
			int num3 = text.IndexOf(',');
			if (num3 < 0)
			{
				return;
			}
			string funcName = text.Substring(0, num3);
			string param2 = text.Substring(num3 + 1, text.Length - num3 - 1);
			WJUtils.Broadcast(funcName, param2);
			return;
		}
		else
		{
            /*
			if (WJUtils.HandleActionCallback(num, text, num2))
			{
				return;
			}
			if (WJUtils.last_callback_delegate != null)
			{
				WJUtils.last_callback_delegate(num, text);
			}
			return;
			*/          
		}
	}
    /*
	private static bool HandleActionCallback(int tag, string value, int action)
	{
		switch (action)
		{
		case 119:
			AudioListener.volume = 0f;
			if (WJUtils.onInterstitialStart != null)
			{
				WJUtils.onInterstitialStart();
			}
			return true;
		case 120:
			if (!WJSound2D.Mute)
			{
				AudioListener.volume = 1f;
			}
			if (WJUtils.onInterstitialStop != null)
			{
				WJUtils.onInterstitialStop();
			}
			return true;
		case 121:
			if (WJUtils.onInterstitialFail != null)
			{
				WJUtils.onInterstitialFail();
			}
			return true;
		case 122:
			if (WJUtils.onAdsClick != null)
			{
				WJUtils.onAdsClick(int.Parse(value));
			}
			return true;
		case 123:
		case 124:
		case 125:
		case 126:
		case 128:
		case 130:
			IL_3D:
			switch (action)
			{
			case 68:
				if (WJApplication.AppInstance != null)
				{
					WJApplication.AppInstance.OnMoreGameEvent(WJApplication.MoreGameEvent.Show);
				}
				return true;
			case 69:
				if (WJApplication.AppInstance != null)
				{
					WJApplication.AppInstance.OnMoreGameEvent(WJApplication.MoreGameEvent.Close);
				}
				return true;
			case 70:
				if (WJApplication.AppInstance != null)
				{
					WJApplication.AppInstance.OnMoreGameEvent(WJApplication.MoreGameEvent.Selected);
				}
				return true;
			case 71:
			case 72:
			case 73:
				IL_62:
				switch (action)
				{
				case 53:
					WJUtils.SetGetFreeUnlocked(WJUtils.s_lastWatchGetFreeLockKey, true);
					if (WJUtils.OnGetFreeRewardVideoCallback != null)
					{
						WJUtils.OnGetFreeRewardVideoCallback(WJUtils.s_lastWatchGetFreeLockKey, tag);
					}
					return true;
				case 54:
					AudioListener.volume = 0f;
					if (WJUtils.OnGetFreeRewardVideoStart != null)
					{
						WJUtils.OnGetFreeRewardVideoStart(WJUtils.s_lastWatchGetFreeLockKey, tag);
					}
					return true;
				case 55:
					if (!WJSound2D.Mute)
					{
						AudioListener.volume = 1f;
					}
					if (WJUtils.OnGetFreeRewardVideoClose != null)
					{
						WJUtils.OnGetFreeRewardVideoClose(WJUtils.s_lastWatchGetFreeLockKey, tag);
					}
					return true;
				default:
					switch (action)
					{
					case 91:
						if (WJUtils.OnOfferwallCallback != null)
						{
							WJUtils.OnOfferwallCallback(tag);
						}
						return true;
					case 92:
					case 93:
						IL_94:
						switch (action)
						{
						case 81:
							if (WJUtils.OnChartboostParentControlCallback != null)
							{
								WJUtils.OnChartboostParentControlCallback();
							}
							return true;
						case 82:
						case 83:
							IL_AD:
							if (action != 11)
							{
								if (action == 16)
								{
									Application.Quit();
									return true;
								}
								if (action != 37)
								{
									if (action == 87)
									{
										if (WJUtils.OnRateClickedCallback != null)
										{
											WJUtils.OnRateClickedCallback();
										}
										return true;
									}
									if (action == 109)
									{
										if (WJUtils.OnHuaWeiGiftCodeSuccessed != null)
										{
											WJUtils.OnHuaWeiGiftCodeSuccessed();
										}
										return true;
									}
									if (action != 999)
									{
										return false;
									}
									if (WJUtils.OnShareResultCallback != null)
									{
										WJUtils.OnShareResultCallback(tag == 1, value);
									}
									return true;
								}
							}
							WJUtils.SetInAppPurchased(value, true);
							if (WJUtils.s_removeAdsByPurchaseAnything)
							{
								WJUtils.SetAdsRemoved(true);
								WJUtils.RemoveAds();
							}
							if (WJUtils.last_callback_delegate == null && action == 11 && WJUtils.OnInAppPurchaseRetrySuccess != null)
							{
								WJUtils.OnInAppPurchaseRetrySuccess(value);
								return true;
							}
							return false;
						case 84:
							return true;
						}
						goto IL_AD;
					case 94:
						AudioListener.volume = 0f;
						return true;
					case 95:
						if (!WJSound2D.Mute)
						{
							AudioListener.volume = 1f;
						}
						return true;
					}
					goto IL_94;
				}
				break;
			case 74:
				return true;
			}
			goto IL_62;
		case 127:
			if (WJUtils.OnSubscription != null)
			{
				WJUtils.OnSubscription(value);
			}
			return true;
		case 129:
			if (WJUtils.OnIapFail != null)
			{
				WJUtils.OnIapFail(value);
			}
			return true;
		case 131:
			if (WJUtils.OnShowGDPRDialog != null)
			{
				WJUtils.OnShowGDPRDialog();
			}
			return true;
		}
		goto IL_3D;
	}
    */
	public static void CheckSubscriptionActive()
	{
		WJUtils.CallAction_Void(128, string.Empty);
	}

	public static void InAppPurchase(int callbackTag, string iapId, WJUtils.OnActionCallbackDelegate callback)
	{
		if (WJUtils.s_iapDisabled)
		{
			return;
		}
		WJUtils.CallAction_Void_Callback(11, callbackTag, iapId, callback);
	}

	public static void InAppRestore(int callbackTag, WJUtils.OnActionCallbackDelegate callback)
	{
		if (WJUtils.s_iapDisabled)
		{
			return;
		}
		WJUtils.CallAction_Void_Callback(37, callbackTag, string.Empty, callback);
	}

	public static void SetInAppPurchased(string iapId, bool purchased)
	{
		PlayerPrefs.SetInt(iapId, (!purchased) ? 0 : 1);
		PlayerPrefs.Save();
	}

	public static bool IsInAppPurchased(string iapId, string iapAllId = null)
	{
		if (WJUtils.s_iapDisabled)
		{
			return true;
		}
		if (iapAllId != null)
		{
			bool flag = WJUtils.IsInAppPurchased(iapAllId, null);
			if (flag)
			{
				return true;
			}
		}
		return PlayerPrefs.GetInt(iapId, 0) == 1;
	}

	public static void SetIapDisabled(bool disabled)
	{
		WJUtils.s_iapDisabled = disabled;
	}

	public static bool IsIapDisabled()
	{
		return WJUtils.s_iapDisabled;
	}

	public static List<object> GetProductInfoList()
	{
		if (WJUtils.sProductInfoListCache != null)
		{
			return WJUtils.sProductInfoListCache;
		}
		string path = WJUtils.GetCachesFolderPath(string.Empty) + "Data/Raw/store/productInfo.plist";
		string path2 = "store/productInfo.plist";
		if (File.Exists(path))
		{
			WJUtils.sProductInfoListCache = (List<object>)Plist.readPlist(path);
		}
		else
		{
			string s = WJUtils.ReadStreamingAssetsFileAllText(path2);
			WJUtils.sProductInfoListCache = (List<object>)Plist.readPlist(Encoding.UTF8.GetBytes(s));
		}
		return WJUtils.sProductInfoListCache;
	}

	public static void SyncHuaWeiStorePlist()
	{
	}

	public static void ShowMessageBox(string titleAndMessage)
	{
		WJUtils.CallAction_Void_Callback(7, -1, titleAndMessage, null);
	}

	public static void ShowMessageBox(string title, string message)
	{
		WJUtils.ShowMessageBox(title + "|" + message);
	}

	public static void ShowMessageBoxYesNo(string titleAndMessage, int callbackTag, WJUtils.OnActionCallbackDelegate callback)
	{
		WJUtils.CallAction_Void_Callback(3, callbackTag, titleAndMessage, callback);
	}

	public static void ShowMessageBoxYesNo(string title, string message, int callbackTag, WJUtils.OnActionCallbackDelegate callback)
	{
		WJUtils.ShowMessageBoxYesNo(title + "|" + message, callbackTag, callback);
	}

	public static string CreateFileName(string path, int no, string suffix = "", int minDigits = 3)
	{
		string format = string.Concat(new object[]
		{
			path,
			"{0:D",
			minDigits,
			"}",
			suffix
		});
		return string.Format(format, no);
	}

	public static string GetWritableFolderPath(string folder = "")
	{
		string text = Application.persistentDataPath + "/" + folder;
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		return text;
	}

	public static string GetTemporaryFolderPath()
	{
		return WJUtils.CallAction_Retstr(40, string.Empty);
	}

	public static string GetCachesFolderPath(string folder = "")
	{
		return WJUtils.GetWritableFolderPath(folder);
	}

	public static string ReadFileAllText(string fullPath)
	{
		return WJUtils.ReadFileAllText(fullPath, Encoding.UTF8);
	}

	public static string ReadFileAllText(string fullPath, Encoding encoding)
	{
		if (fullPath.Contains("://"))
		{
			WWW wWW = new WWW(fullPath);
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			while (!wWW.isDone)
			{
				if (Time.realtimeSinceStartup - realtimeSinceStartup > 10f)
				{
					return string.Empty;
				}
			}
			string @string = encoding.GetString(wWW.bytes);
			wWW.Dispose();
			return @string;
		}
		return File.ReadAllText(fullPath, encoding);
	}

	public static string ReadStreamingAssetsFileAllText(string path)
	{
		return WJUtils.ReadFileAllText(Path.Combine(Application.streamingAssetsPath, path));
	}

	public static Dictionary<string, object> ReadStreamingAssetsPlist(string path)
	{
		string s = WJUtils.ReadStreamingAssetsFileAllText(path);
		return (Dictionary<string, object>)Plist.readPlist(Encoding.Default.GetBytes(s));
	}

	public static List<object> ReadStreamingAssetsPlistArray(string path)
	{
		string s = WJUtils.ReadStreamingAssetsFileAllText(path);
		return (List<object>)Plist.readPlist(Encoding.Default.GetBytes(s));
	}

	public static Texture2D CaptureScreen(bool isTransparent = false, int cullingMask = -1)
	{
		if (Camera.main == null)
		{
			UnityEngine.Debug.Log("WJUtils CaptureScreen: not found the mainCamera.");
			return null;
		}
		return WJUtils.CaptureScreen(Camera.main, isTransparent, cullingMask);
	}

	public static void SetCaptureScreenParm(bool isfullscreen, Vector2 _size)
	{
		WJUtils.isCapFullScreen = isfullscreen;
		WJUtils.capSize = _size;
	}

	public static Texture2D CaptureScreen(Camera camera, bool isTransparent = false, int cullingMask = -1)
	{
		WJUtils.s_lastCSFromFileName = null;
		int cullingMask2 = camera.cullingMask;
		if (cullingMask != -1)
		{
			camera.cullingMask = cullingMask;
		}
		int width = (!WJUtils.isCapFullScreen) ? ((int)WJUtils.capSize.x) : Screen.width;
		int height = (!WJUtils.isCapFullScreen) ? ((int)WJUtils.capSize.y) : Screen.height;
		if (WJUtils.s_lastCSRender == null)
		{
			WJUtils.s_lastCSRender = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
		}
		if (WJUtils.s_lastCSTexture != null && (WJUtils.s_lastCSTexture.width != Screen.width || WJUtils.s_lastCSTexture.height != Screen.height))
		{
			UnityEngine.Object.Destroy(WJUtils.s_lastCSTexture);
			WJUtils.s_lastCSTexture = null;
		}
		if (WJUtils.s_lastCSTexture == null)
		{
			WJUtils.s_lastCSTexture = new Texture2D(width, height, (!isTransparent) ? TextureFormat.RGB24 : TextureFormat.ARGB32, false);
			WJUtils.s_lastCSTexture.wrapMode = TextureWrapMode.Clamp;
		}
		camera.targetTexture = WJUtils.s_lastCSRender;
		camera.Render();
		camera.targetTexture = null;
		RenderTexture.active = WJUtils.s_lastCSRender;
		WJUtils.s_lastCSTexture.ReadPixels(new Rect(0f, 0f, (float)WJUtils.s_lastCSRender.width, (float)WJUtils.s_lastCSRender.height), 0, 0);
		WJUtils.s_lastCSTexture.Apply();
		RenderTexture.active = null;
		if (cullingMask != -1)
		{
			camera.cullingMask = cullingMask2;
		}
		return WJUtils.s_lastCSTexture;
	}

	public static Texture2D CaptureScreenFromFile(string fullPath)
	{
		WJUtils.ReleaseLastScreenshot();
		WJUtils.s_lastCSFromFileName = fullPath;
		if (!fullPath.Contains("://"))
		{
			fullPath = "file://" + fullPath;
		}
		WWW wWW = new WWW(fullPath);
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		while (!wWW.isDone)
		{
			if (Time.realtimeSinceStartup - realtimeSinceStartup > 3f)
			{
				return null;
			}
		}
		WJUtils.s_lastCSTexture = wWW.texture;
		wWW.Dispose();
		return WJUtils.s_lastCSTexture;
	}

	public static Texture2D ClipCaptureScreenTexture(int x, int y, int w, int h)
	{
		if (WJUtils.s_lastCSRender == null || WJUtils.s_lastCSTexture == null)
		{
			UnityEngine.Debug.LogError("Please use CaptureScreen before call me!");
			return null;
		}
		UnityEngine.Object.Destroy(WJUtils.s_lastCSTexture);
		WJUtils.s_lastCSTexture = new Texture2D(w, h, TextureFormat.RGB24, false);
		RenderTexture.active = WJUtils.s_lastCSRender;
		WJUtils.s_lastCSTexture.ReadPixels(new Rect((float)x, (float)y, (float)w, (float)h), 0, 0);
		WJUtils.s_lastCSTexture.Apply();
		RenderTexture.active = null;
		return WJUtils.s_lastCSTexture;
	}

	public static Texture2D GetLastScreenshotTexture2D()
	{
		return WJUtils.s_lastCSTexture;
	}

	public static bool SaveLastScreenshot(string toFileNameFullPath, bool isTransparent = false)
	{
		if (WJUtils.s_lastCSFromFileName != null)
		{
			File.Copy(WJUtils.s_lastCSFromFileName, toFileNameFullPath, true);
			return true;
		}
		if (WJUtils.s_lastCSTexture != null)
		{
			File.WriteAllBytes(toFileNameFullPath, WJUtils.s_lastCSTexture.EncodeToPNG());
			return true;
		}
		return false;
	}

	public static string SaveLastScreenshot(bool isTransparent = false)
	{
		string text = WJUtils.GetTemporaryFolderPath() + "screenshot.png";
		if (WJUtils.SaveLastScreenshot(text, isTransparent))
		{
			return text;
		}
		return null;
	}

	public static string SaveTextureToTemporaryFolderPath(Texture2D texture)
	{
		return WJUtils.SaveTextureToDisk(texture, WJUtils.GetTemporaryFolderPath() + "temptexture.png");
	}

	public static string SaveTextureToDisk(Texture2D texture, string fileNameFullPath)
	{
		File.WriteAllBytes(fileNameFullPath, texture.EncodeToPNG());
		return fileNameFullPath;
	}

	public static void SaveTextureToSystemAlbum(Texture2D texture, string folderName = "")
	{
		string path = WJUtils.CallAction_Retstr(5, folderName);
		string text = Path.Combine(path, WJUtils.GetSystemYMDHMS() + ".png");
		WJUtils.SaveTextureToDisk(texture, text);
		WJUtils.CallAction_Void(17, text);
		WJUtils.CallAction_Void(34, string.Empty);
	}

	public static void SaveLastScreenshotToSystemAlbum(string folderName = "")
	{
		string path = WJUtils.CallAction_Retstr(5, folderName);
		string text = Path.Combine(path, WJUtils.GetSystemYMDHMS() + ".png");
		if (!WJUtils.SaveLastScreenshot(text, false))
		{
			return;
		}
		WJUtils.CallAction_Void(17, text);
		WJUtils.CallAction_Void(34, string.Empty);
	}

	public static void ReleaseLastScreenshot()
	{
		if (WJUtils.s_lastCSRender != null)
		{
			WJUtils.s_lastCSRender.Release();
			UnityEngine.Object.Destroy(WJUtils.s_lastCSRender);
			WJUtils.s_lastCSRender = null;
		}
		if (WJUtils.s_lastCSTexture != null)
		{
			UnityEngine.Object.Destroy(WJUtils.s_lastCSTexture);
			WJUtils.s_lastCSTexture = null;
		}
	}

	public static void ShowAds(string adsId, char adSize, int x, int y)
	{
		if (!WJUtils.s_started)
		{
			Application.Quit();
			return;
		}
		string param = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", new object[]
		{
			adSize,
			x,
			y,
			0,
			0,
			adsId,
			0,
			0
		});
		WJUtils.CallAction_Void(12, param);
	}

	public static void HideAds()
	{
		WJUtils.CallAction_Void(13, string.Empty);
	}

	public static void RemoveAds()
	{
		WJUtils.CallAction_Void(35, string.Empty);
	}

	public static bool IsAdsVisible()
	{
		string text = WJUtils.CallAction_Retstr(36, string.Empty);
		return text.Equals("Y");
	}

	public static bool IsAdsReallyVisible()
	{
		string text = WJUtils.CallAction_Retstr(57, string.Empty);
		return text.Equals("Y");
	}

	public static bool IsAdsRemoved()
	{
		return PlayerPrefs.GetInt("isRemovedAds", 0) == 1;
	}

	public static void SetAdsRemoved(bool value)
	{
		PlayerPrefs.SetInt("isRemovedAds", (!value) ? 0 : 1);
		PlayerPrefs.Save();
	}

	public static Vector2 GetAdsRealSize(char adSize)
	{
		string param = string.Empty + adSize;
		string str = WJUtils.CallAction_Retstr(102, param);
		return WJUtils.ParsePositionString(str);
	}

	public static void PrepareAdsFullScreen(string interstitialId)
	{
		WJUtils.CallAction_Void(41, interstitialId);
	}

	public static void ShowAdsFullScreen()
	{
		WJUtils.CallAction_Void(42, string.Empty);
	}

	public static bool IsInterstitialReady()
	{
		return !WJUtils.IsAdsRemoved() && WJUtils.CallAction_Retstr(118, string.Empty) == "1";
	}

	public static void ShowChartboost(WJUtils.ChartboostLocation location)
	{
		int arg_11_0 = 31;
		int num = (int)location;
		WJUtils.CallAction_Void(arg_11_0, num.ToString());
	}

	public static void ShowChartboostOnPlayButton()
	{
		if (!WJUtils.CanClick("playClick", 3f))
		{
			return;
		}
		if (!WJUtils.mIsShowChartboostOnPlayButtonPlayed && PlayerPrefs.GetInt("isFirstPlay", 1) != 1)
		{
			WJUtils.mIsShowChartboostOnPlayButtonPlayed = true;
		}
		if (PlayerPrefs.GetInt("isFirstPlay", 1) == 1)
		{
			PlayerPrefs.SetInt("isFirstPlay", 0);
			PlayerPrefs.Save();
		}
	}

	public static void ChartboostParentControlPass(bool didPass)
	{
		WJUtils.CallAction_Void(82, (!didPass) ? "0" : "1");
	}

	public static void ShowLibiiCrossInterstitial()
	{
		WJUtils.CallAction_Void(135, string.Empty);
	}

	private static void CallRating(bool prompt, string title, string msg, string rateUrl, bool bnewVersionRateAgain)
	{
		int actiontype = (!prompt) ? 22 : 23;
		StringBuilder stringBuilder = new StringBuilder(title);
		stringBuilder.Append(",").Append(msg).Append(",").Append(rateUrl).Append(",").Append((!bnewVersionRateAgain) ? "0" : "1");
		WJUtils.CallAction_Void(actiontype, stringBuilder.ToString());
	}

	public static void CheckRatingOnStartup(string title, string msg, string rateUrl, bool bnewVersionRateAgain)
	{
		WJUtils.CallRating(false, title, msg, rateUrl, bnewVersionRateAgain);
	}

	public static void PromptForRating(string title, string msg, string rateUrl, bool bnewVersionRateAgain)
	{
		WJUtils.CallRating(true, title, msg, rateUrl, bnewVersionRateAgain);
	}

	public static void FirstTimeRating(string title, string msg, string rateUrl)
	{
		WJUtils.CallRating(false, title, msg, rateUrl, false);
	}

	public static void SetRateClickCallbackEnabled(bool enabled)
	{
		WJUtils.CallAction_Void(86, (!enabled) ? "0" : "1");
	}

	public static void RateDoRating(string rateUrl)
	{
		WJUtils.CallAction_Void(88, ",," + rateUrl);
	}

	public static void ShowARCamera(int callbackTag, string topFileName, bool topTouchAble = true, bool topFullscreen = false, bool bgTouchAble = false, bool bgFullscreen = false, WJUtils.OnActionCallbackDelegate callback = null, bool cameraFrame = false, string otherParams = "")
	{
		StringBuilder stringBuilder = new StringBuilder(topFileName);
		stringBuilder.Append("|").Append((!topTouchAble) ? "0" : "1").Append("|").Append((!topFullscreen) ? "0" : "1").Append("|").Append((!bgTouchAble) ? "0" : "1").Append("|").Append((!bgFullscreen) ? "0" : "1").Append("|").Append((!cameraFrame) ? "0" : "1").Append("|").Append(otherParams);
		WJUtils.CallAction_Void_Callback(30, callbackTag, stringBuilder.ToString(), callback);
	}

	public static string GetDeviceType()
	{
		if (WJUtils.s_deviceType == null)
		{
			WJUtils.s_deviceType = WJUtils.CallAction_Retstr(33, string.Empty);
		}
		return WJUtils.s_deviceType;
	}

	public static bool IsIPhone()
	{
		return false;
	}

	public static bool IsIPad()
	{
		return false;
	}

	public static bool IsIPhone5()
	{
		return false;
	}

	public static bool IsIosLowPerformanceDevice()
	{
		return false;
	}

	public static bool IsIosLowPerformanceDeviceIPad2()
	{
		return false;
	}

	public static bool IsIosLowPerformanceDeviceIPad2Mini1()
	{
		return false;
	}

	public static float getIOSDisplayScaleTimes()
	{
		return 1f;
	}

	public static bool IsIosRetina()
	{
		return false;
	}

	public static float GetDeviceOSVersion()
	{
		return 0f;
	}

	public static string GetPlatformStoreName()
	{
		if (WJUtils.s_storeName == null)
		{
			WJUtils.s_storeName = WJUtils.CallAction_Retstr(78, string.Empty);
		}
		return WJUtils.s_storeName;
	}

	public static string GetChannelName()
	{
		if (WJUtils.sChannelName == null)
		{
			WJUtils.sChannelName = WJUtils.CallAction_Retstr(126, string.Empty);
		}
		return WJUtils.sChannelName;
	}

	public static string GetCompassBIStyleDeviceType()
	{
		return WJUtils.CallAction_Retstr(107, string.Empty);
	}

	public static string GetDeviceOSFullname()
	{
		return WJUtils.CallAction_Retstr(106, string.Empty);
	}

	public static string GetDeviceUID()
	{
		string text = WJUtils.CallAction_Retstr(105, string.Empty);
		if (text == string.Empty)
		{
			text = WJUtils.CallAction_Retstr(112, string.Empty);
		}
		return text;
	}

	public static bool HasIDFA()
	{
		return false;
	}

	public static void RegisterNotification()
	{
		WJUtils.CallAction_Void(123, string.Empty);
	}

	public static void ClearAllLocalNotifications()
	{
		WJUtils.CallAction_Void(49, string.Empty);
	}

	private static string getRandomString(string str)
	{
		string[] array = str.Split(new string[]
		{
			"|||"
		}, StringSplitOptions.None);
		return array[UnityEngine.Random.Range(0, array.Length)];
	}

	public static void AddCommonLocalNotifications(string morrowMsg, string day357Msg, string day468Msg)
	{
		DateTime now = DateTime.Now;
		if (now.Hour >= 6 && now.Hour <= 22)
		{
			WJUtils.AddLocalNotification(WJUtils.getRandomString(morrowMsg), 86400f, false, "notifi.mp3");
		}
		else if (now.Hour > 22)
		{
			DateTime dateTime = new DateTime(now.Year, now.Month, now.Day, 22, 0, 0);
			int num = (int)(dateTime.AddDays(1.0) - now).TotalSeconds;
			WJUtils.AddLocalNotification(WJUtils.getRandomString(morrowMsg), (float)num, false, "notifi.mp3");
		}
		else
		{
			int num = (int)(new DateTime(now.Year, now.Month, now.Day, 22, 0, 0) - now).TotalSeconds;
			WJUtils.AddLocalNotification(WJUtils.getRandomString(morrowMsg), (float)num, false, "notifi.mp3");
		}
		for (int i = 2; i <= 6; i += 2)
		{
			if (now.Hour < 6)
			{
				DateTime dateTime2 = new DateTime(now.Year, now.Month, now.Day, 12, 0, 0);
				int num = (int)(dateTime2.AddDays((double)(i - 1)) - now).TotalSeconds;
				WJUtils.AddLocalNotification(WJUtils.getRandomString(day357Msg), (float)num, false, "notifi.mp3");
			}
			else
			{
				DateTime dateTime3 = new DateTime(now.Year, now.Month, now.Day, 12, 0, 0);
				int num = (int)(dateTime3.AddDays((double)i) - now).TotalSeconds;
				WJUtils.AddLocalNotification(WJUtils.getRandomString(day357Msg), (float)num, false, "notifi.mp3");
			}
		}
		for (int j = 3; j <= 7; j += 2)
		{
			if (now.Hour < 6)
			{
				DateTime dateTime4 = new DateTime(now.Year, now.Month, now.Day, 18, 0, 0);
				int num = (int)(dateTime4.AddDays((double)(j - 1)) - now).TotalSeconds;
				WJUtils.AddLocalNotification(WJUtils.getRandomString(day468Msg), (float)num, false, "notifi.mp3");
			}
			else
			{
				DateTime dateTime5 = new DateTime(now.Year, now.Month, now.Day, 18, 0, 0);
				int num = (int)(dateTime5.AddDays((double)j) - now).TotalSeconds;
				WJUtils.AddLocalNotification(WJUtils.getRandomString(day468Msg), (float)num, false, "notifi.mp3");
			}
		}
	}

	public static void AddLocalNotification(string notifiText, float waitSecond, bool clearAllNotifi = false, string soundFileName = "notifi.mp3")
	{
		string text = string.Format("{0}|{1}|{2}|{3}", new object[]
		{
			notifiText,
			waitSecond,
			(!clearAllNotifi) ? "0" : "1",
			soundFileName
		});
	}

	public static object ReadDictObjectByKeys(object o, params string[] keys)
	{
		for (int i = 0; i < keys.Length; i++)
		{
			string key = keys[i];
			Dictionary<string, object> dictionary = (Dictionary<string, object>)o;
			if (!dictionary.TryGetValue(key, out o))
			{
				return null;
			}
		}
		return o;
	}

	public static string ReadDictStringByKeys(object o, params string[] keys)
	{
		return (string)WJUtils.ReadDictObjectByKeys(o, keys);
	}

	public static float ReadDictFloatByKeys(object o, params string[] keys)
	{
		return (float)WJUtils.ReadDictObjectByKeys(o, keys);
	}

	public static int ReadDictIntByKeys(object o, params string[] keys)
	{
		return (int)WJUtils.ReadDictObjectByKeys(o, keys);
	}

	public static void GameCenterLogin()
	{
		WJUtils.CallAction_Void(58, string.Empty);
	}

	public static void GameCenterShowLeaderBoard()
	{
		WJUtils.CallAction_Void(59, string.Empty);
	}

	public static void GameCenterShowAchievement()
	{
		WJUtils.CallAction_Void(60, string.Empty);
	}

	public static void GameCenterSubmitScore(long score, string category)
	{
		string param = score + "," + category;
		WJUtils.CallAction_Void(61, param);
	}

	public static void GameCenterSubmitAchievement(float percent, string achievement)
	{
		string param = string.Format("{0:F2},{1}", percent, achievement);
		WJUtils.CallAction_Void(62, param);
	}

	public static bool IsGetFreeUnlocked(string getFreeLockKey)
	{
		bool flag;
		return WJUtils.s_getFreeUnlockedMap.TryGetValue(getFreeLockKey, out flag) && flag;
	}

	public static void SetGetFreeUnlocked(string getFreeLockKey, bool value)
	{
		WJUtils.s_getFreeUnlockedMap[getFreeLockKey] = value;
	}

	public static void ClearGetFreeUnlocked()
	{
		WJUtils.s_getFreeUnlockedMap.Clear();
	}

	public static bool IsGetFreeRewardVideoReady(bool removeGetFreeByAdsRemoved = true, string getFreeLockKey = "")
	{
		return (!removeGetFreeByAdsRemoved || !WJUtils.IsAdsRemoved()) && WJUtils.CallAction_Retstr(52, getFreeLockKey).Equals("1");
	}

	public static void PlayGetFreeRewardVideo(string getFreeLockKey, bool removeThisGetFreeUnlocked = true)
	{
		WJUtils.s_lastWatchGetFreeLockKey = getFreeLockKey;
		if (removeThisGetFreeUnlocked)
		{
			WJUtils.SetGetFreeUnlocked(getFreeLockKey, false);
		}
		WJUtils.CallAction_Void(51, getFreeLockKey);
	}

	public static string GetLastRewardVideoLockKey()
	{
		return WJUtils.s_lastWatchGetFreeLockKey;
	}

	protected static void RecursionDirToFilePaths(string rootDir, bool needMeta, params string[] excludeDir)
	{
		if (Directory.Exists(rootDir) && WJUtils.IsDirTestPassed(rootDir, excludeDir))
		{
			string[] array = Directory.GetFiles(rootDir);
			for (int i = 0; i < array.Length; i++)
			{
				if (needMeta)
				{
					if (!Directory.Exists(array[i].Replace(".meta", string.Empty)))
					{
						WJUtils.s_filePaths.Add(array[i]);
					}
				}
				else if (!array[i].EndsWith(".meta"))
				{
					WJUtils.s_filePaths.Add(array[i]);
				}
			}
			array = Directory.GetDirectories(rootDir);
			for (int j = 0; j < array.Length; j++)
			{
				WJUtils.RecursionDirToFilePaths(array[j], needMeta, excludeDir);
			}
		}
	}

	protected static bool IsDirTestPassed(string dir, params string[] excludeDir)
	{
		bool result = true;
		if (excludeDir != null)
		{
			for (int i = 0; i < excludeDir.Length; i++)
			{
				if (dir.EndsWith(excludeDir[i]))
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}

	public static List<string> QueryAllFilePaths(string rootDir, bool needMeta, params string[] excludeDir)
	{
		WJUtils.s_filePaths.Clear();
		WJUtils.RecursionDirToFilePaths(rootDir, needMeta, excludeDir);
		return WJUtils.s_filePaths;
	}

	public static bool ClosestPointOnLine(Vector2 linePt1, Vector2 linePt2, Vector2 point, out Vector2 retPoint, out float d)
	{
		Matrix4x4 matrix4x = Matrix4x4.TRS(linePt1, Quaternion.Euler(0f, 0f, WJUtils.CalcIncludedAngle(Vector2.right, linePt2 - linePt1)), Vector3.one);
		Matrix4x4 inverse = matrix4x.inverse;
		point = inverse.MultiplyPoint(point);
		Vector2 b = inverse.MultiplyPoint(linePt2);
		bool flag = point.x > 0f != point.x > b.x;
		if (flag)
		{
			d = Mathf.Abs(point.y);
			retPoint = matrix4x.MultiplyPoint(new Vector3(point.x, 0f, 0f));
		}
		else
		{
			float magnitude = point.magnitude;
			float magnitude2 = (point - b).magnitude;
			d = Mathf.Min(magnitude, magnitude2);
			retPoint = ((magnitude >= magnitude2) ? linePt2 : linePt1);
		}
		return flag;
	}

	public static bool IsPointInPoly(Vector2 testPoint, Vector2[] poly)
	{
		bool flag = false;
		int i = 0;
		int num = poly.Length - 1;
		while (i < poly.Length)
		{
			if (poly[i].y > testPoint.y != poly[num].y > testPoint.y && testPoint.x > poly[i].x + (poly[num].x - poly[i].x) * (poly[i].y - testPoint.y) / (poly[i].y - poly[num].y))
			{
				flag = !flag;
			}
			num = i++;
		}
		return flag;
	}

	public static float IsPointOnLine(Vector2 p, Vector2 start, Vector2 end)
	{
		return (start.x - p.x) * (end.y - p.y) - (end.x - p.x) * (start.y - p.y);
	}

	public static Vector2 CalcLineIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
	{
		Vector2 result = default(Vector2);
		float num = (p2.y - p1.y) * (p4.x - p3.x) - (p4.y - p3.y) * (p2.x - p1.x);
		float num2 = (p3.y - p1.y) * (p2.x - p1.x) * (p4.x - p3.x) + (p2.y - p1.y) * (p4.x - p3.x) * p1.x - (p4.y - p3.y) * (p2.x - p1.x) * p3.x;
		result.x = num2 / num;
		num = (p2.x - p1.x) * (p4.y - p3.y) - (p4.x - p3.x) * (p2.y - p1.y);
		num2 = (p3.x - p1.x) * (p2.y - p1.y) * (p4.y - p3.y) + p1.y * (p2.x - p1.x) * (p4.y - p3.y) - p3.y * (p4.x - p3.x) * (p2.y - p1.y);
		result.y = num2 / num;
		return result;
	}

	public static void OptimizePolygon(Vector2[] inpoints, out Vector2[] outpoints, float optimization = 1f)
	{
		WJUtils.Vector2Node vector2Node = new WJUtils.Vector2Node();
		vector2Node.point = inpoints[0];
		WJUtils.Vector2Node vector2Node2 = vector2Node;
		for (int i = 1; i < inpoints.Length; i++)
		{
			vector2Node.next = new WJUtils.Vector2Node();
			vector2Node = vector2Node.next;
			vector2Node.point = inpoints[i];
		}
		vector2Node.next = vector2Node2;
		vector2Node = vector2Node2;
		while (true)
		{
			if (Mathf.Abs(WJUtils.IsPointOnLine(vector2Node.next.point, vector2Node.point, vector2Node.next.next.point)) < optimization)
			{
				if (vector2Node.next == vector2Node2)
				{
					break;
				}
				vector2Node.next = vector2Node.next.next;
			}
			else
			{
				vector2Node = vector2Node.next;
			}
			if (vector2Node2 == vector2Node)
			{
				goto IL_D2;
			}
		}
		vector2Node.next = vector2Node.next.next;
		IL_D2:
		List<Vector2> list = new List<Vector2>(8);
		vector2Node2 = vector2Node;
		do
		{
			list.Add(vector2Node.point);
			vector2Node = vector2Node.next;
		}
		while (vector2Node2 != vector2Node);
		outpoints = ((list.Count <= 0) ? null : list.ToArray());
	}

	public static float CalcIncludedAngle(Vector3 from, Vector3 to)
	{
		from.y = from.z;
		to.y = to.z;
		Vector2 from2 = from;
		Vector2 to2 = to;
		return WJUtils.CalcIncludedAngle(from2, to2);
	}

	public static float CalcIncludedAngle(Vector2 from, Vector2 to)
	{
		return (Vector3.Cross(from, to).z <= 0f) ? (-Vector2.Angle(from, to)) : Vector2.Angle(from, to);
	}

	public static Bounds CalcGamaObjectBoundsInWorld(GameObject go)
	{
		Transform transform = go.transform;
		Vector3 max = Vector3.one * -3.40282347E+38f;
		Vector3 min = Vector3.one * 3.40282347E+38f;
		Bounds result = default(Bounds);
		Vector3[] array = new Vector3[8];
		if (!go.activeInHierarchy)
		{
			return result;
		}
		MeshFilter component = go.GetComponent<MeshFilter>();
		if (component && component.sharedMesh)
		{
			component.sharedMesh.RecalculateBounds();
			result = component.sharedMesh.bounds;
		}
		else
		{
			SkinnedMeshRenderer component2 = go.GetComponent<SkinnedMeshRenderer>();
			if (component2 && component2.rootBone)
			{
				transform = component2.rootBone;
				result = component2.localBounds;
			}
		}
		Vector3 extents = result.extents;
		array[0] = result.center + new Vector3(-extents.x, extents.y, extents.z);
		array[1] = result.center + new Vector3(-extents.x, extents.y, -extents.z);
		array[2] = result.center + new Vector3(extents.x, extents.y, extents.z);
		array[3] = result.center + new Vector3(extents.x, extents.y, -extents.z);
		array[4] = result.center + new Vector3(-extents.x, -extents.y, extents.z);
		array[5] = result.center + new Vector3(-extents.x, -extents.y, -extents.z);
		array[6] = result.center + new Vector3(extents.x, -extents.y, extents.z);
		array[7] = result.center + new Vector3(extents.x, -extents.y, -extents.z);
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = transform.localToWorldMatrix.MultiplyPoint(array[i]);
		}
		for (int j = 0; j < array.Length; j++)
		{
			min.x = Mathf.Min(array[j].x, min.x);
			min.y = Mathf.Min(array[j].y, min.y);
			min.z = Mathf.Min(array[j].z, min.z);
			max.x = Mathf.Max(array[j].x, max.x);
			max.y = Mathf.Max(array[j].y, max.y);
			max.z = Mathf.Max(array[j].z, max.z);
		}
		result.SetMinMax(min, max);
		for (int k = 0; k < go.transform.childCount; k++)
		{
			result.Encapsulate(WJUtils.CalcGamaObjectBoundsInWorld(go.transform.GetChild(k).gameObject));
		}
		return result;
	}

	public static float Clamp(float value, float min, float max, float bufferFactor, float damping = 2f)
	{
		float num = Mathf.Clamp(value, min, max);
		if (Mathf.Abs(value - num) > 0.001f)
		{
			float num2 = Mathf.Abs(value - num);
			float num3 = Mathf.Abs(max - min);
			float num4 = num2 / num3;
			num4 = Mathf.Min(1f, num4 / damping);
			num4 = Mathf.Sin(num4 * 3.14159274f / 2f);
			float num5 = Mathf.Abs(bufferFactor) * Mathf.Abs(max - min);
			num5 *= num4;
			num += ((value - num <= 0f) ? (-num5) : num5);
		}
		return num;
	}

	public static float HighPerformanceSin(float angle)
	{
		int num = (int)angle % 360;
		if (num < 0)
		{
			return -WJUtils.s_sin_values[-num];
		}
		return WJUtils.s_sin_values[num];
	}

	public static float HighPerformanceCos(float angle)
	{
		return WJUtils.HighPerformanceSin(angle + 90f);
	}

	public static float Sqrt(float number)
	{
		if (number == 0f)
		{
			return 0f;
		}
		float num = 0.5f * number;
		WJUtils.FloatIntUnion floatIntUnion;
		floatIntUnion.i = 0;
		floatIntUnion.f = number;
		floatIntUnion.i = 1597463174 - (floatIntUnion.i >> 1);
		floatIntUnion.f *= 1.5f - num * floatIntUnion.f * floatIntUnion.f;
		return floatIntUnion.f * number;
	}

	public static Color Hue2RGB(int hue, float alpha = 1f)
	{
		hue = Math.Abs(hue) % 360;
		float num = (float)(hue % 60) / 60f;
		int num2 = hue / 60;
		float num3 = num;
		float num4 = 1f - num;
		float r = 0f;
		float g = 0f;
		float b = 0f;
		switch (num2)
		{
		case 0:
			r = 1f;
			g = num3;
			b = 0f;
			break;
		case 1:
			r = num4;
			g = 1f;
			b = 0f;
			break;
		case 2:
			r = 0f;
			g = 1f;
			b = num3;
			break;
		case 3:
			r = 0f;
			g = num4;
			b = 1f;
			break;
		case 4:
			r = num3;
			g = 0f;
			b = 1f;
			break;
		case 5:
			r = 1f;
			g = 0f;
			b = num4;
			break;
		}
		return new Color(r, g, b, alpha);
	}

	public static float RGB2Hue(Color color)
	{
		float r = color.r;
		float g = color.g;
		float b = color.b;
		float num = Mathf.Max(new float[]
		{
			r,
			g,
			b
		});
		float num2 = Mathf.Min(new float[]
		{
			r,
			g,
			b
		});
		float result = 0f;
		if (num == r && g >= b)
		{
			if (num - num2 == 0f)
			{
				result = 0f;
			}
			else
			{
				result = 60f * (g - b) / (num - num2);
			}
		}
		else if (num == r && g < b)
		{
			result = 60f * (g - b) / (num - num2) + 360f;
		}
		else if (num == g)
		{
			result = 60f * (b - r) / (num - num2) + 120f;
		}
		else if (num == b)
		{
			result = 60f * (r - g) / (num - num2) + 240f;
		}
		return result;
	}

	public static int CalcLayer(params string[] layers)
	{
		int num = 0;
		for (int i = 0; i < layers.Length; i++)
		{
			num |= 1 << LayerMask.NameToLayer(layers[i]);
		}
		return num;
	}

	public static void SetAllRenderersLayer(GameObject go, string layerName)
	{
		Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = LayerMask.NameToLayer(layerName);
		}
	}

	public static void SetShader(GameObject go, Shader shader)
	{
		if (go.GetComponent<Renderer>() && go.GetComponent<Renderer>().material)
		{
			go.GetComponent<Renderer>().material.shader = shader;
		}
		for (int i = 0; i < go.transform.childCount; i++)
		{
			WJUtils.SetShader(go.transform.GetChild(i).gameObject, shader);
		}
	}

	public static MeshCollider CombineMeshCollidersToTarget(GameObject target, bool destroyRigidbody = true)
	{
		MeshCollider[] componentsInChildren = target.GetComponentsInChildren<MeshCollider>();
		CombineInstance[] array = new CombineInstance[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			array[i].mesh = componentsInChildren[i].sharedMesh;
			array[i].transform = target.transform.worldToLocalMatrix * componentsInChildren[i].transform.localToWorldMatrix;
			UnityEngine.Object.Destroy(componentsInChildren[i]);
			if (componentsInChildren[i].GetComponent<Rigidbody>() && destroyRigidbody)
			{
				UnityEngine.Object.Destroy(componentsInChildren[i].GetComponent<Rigidbody>());
			}
		}
		MeshCollider meshCollider = target.AddComponent<MeshCollider>();
		meshCollider.sharedMesh = new Mesh();
		meshCollider.sharedMesh.CombineMeshes(array);
		return meshCollider;
	}

	public static SkinnedMeshRenderer Skin2Skeleton(GameObject newSkinObject, Transform[] targetBones)
	{
		SkinnedMeshRenderer componentInChildren = newSkinObject.GetComponentInChildren<SkinnedMeshRenderer>();
		SpringSkeleton component = componentInChildren.GetComponent<SpringSkeleton>();
		componentInChildren.transform.parent = newSkinObject.transform.parent;
		List<Transform> list = new List<Transform>(componentInChildren.bones.Length);
		Transform[] bones = componentInChildren.bones;
		for (int i = 0; i < bones.Length; i++)
		{
			Transform transform = bones[i];
			for (int j = 0; j < targetBones.Length; j++)
			{
				Transform transform2 = targetBones[j];
				if (transform.name == transform2.name)
				{
					list.Add(transform2);
					break;
				}
			}
		}
		componentInChildren.bones = list.ToArray();
		for (int k = 0; k < targetBones.Length; k++)
		{
			Transform transform3 = targetBones[k];
			if (transform3.name == componentInChildren.rootBone.name)
			{
				componentInChildren.rootBone = transform3;
				break;
			}
		}
		if (component)
		{
			SpringCollider[] springColliders = component.SpringColliders;
			for (int l = 0; l < springColliders.Length; l++)
			{
				SpringCollider springCollider = springColliders[l];
				for (int m = 0; m < targetBones.Length; m++)
				{
					Transform transform4 = targetBones[m];
					if (springCollider.transform.name == transform4.name)
					{
						springCollider.transform = transform4;
						break;
					}
				}
			}
			SpringBone[] springBones = component.SpringBones;
			for (int n = 0; n < springBones.Length; n++)
			{
				SpringBone springBone = springBones[n];
				for (int num = 0; num < targetBones.Length; num++)
				{
					Transform transform5 = targetBones[num];
					if (springBone.transform.name == transform5.name)
					{
						springBone.transform = transform5;
						springBone.FixTransform = null;
						break;
					}
				}
			}
		}
		UnityEngine.Object.Destroy(newSkinObject);
		return componentInChildren;
	}

	public static void ShowAwardDialog(string thisAppId, string url)
	{
		WJUtils.CallAction_Void(29, thisAppId + "|" + url);
	}

	public static void ShowForParentDialog(string thisAppId, string url)
	{
		WJUtils.CallAction_Void(63, thisAppId + "|" + url);
	}

	public static bool IsNetworkAvailable()
	{
		return WJUtils.CallAction_Retstr(64, string.Empty) == "Y";
	}

	public static bool IsWifiNetworkAvailable()
	{
		return WJUtils.CallAction_Retstr(104, string.Empty) == "Y";
	}

	public static void UmengEvent(string eventId, params string[] values)
	{
		string text = eventId;
		for (int i = 0; i < values.Length; i++)
		{
			text = text + "," + values[i];
		}
		WJUtils.CallAction_Void(43, text);
	}

	public static void LateUpdate()
	{
	}

	public static string Md5(string input)
	{
		MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
		byte[] bytes = Encoding.ASCII.GetBytes(input);
		byte[] array = mD5CryptoServiceProvider.ComputeHash(bytes);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("X2"));
		}
		return stringBuilder.ToString();
	}

	public static void OpenHuaWeiGiftCode()
	{
		WJUtils.CallAction_Void(108, string.Empty);
	}

	public static void ShowHuaweiSignInButton()
	{
		WJUtils.CallAction_Void(115, string.Empty);
	}

	public static void HideHuaweiSignInButton()
	{
		WJUtils.CallAction_Void(116, string.Empty);
	}

	public static int GetOnOfferwallCallbackInvokeListCount()
	{
		return WJUtils.OnOfferwallCallback.GetInvocationList().Length;
	}

	public static bool IsOfferwallReady()
	{
		return WJUtils.CallAction_Retstr(93, string.Empty) == "1";
	}

	public static void ShowOfferwall()
	{
		WJUtils.CallAction_Void(92, string.Empty);
	}

	public static void CheckOfferwallRewardOnStartup()
	{
		WJUtils.CallAction_Void(96, string.Empty);
	}

	public static void setGDPRAdsConsent(bool consent)
	{
		WJUtils.CallAction_Void(132, (!consent) ? "0" : "1");
	}

	public static void PlayUIImpactFeedback(int type)
	{
	}

	public static string GetLocaleCountryCode()
	{
		return WJUtils.CallAction_Retstr(134, string.Empty);
	}
}
