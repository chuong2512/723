using LIBII;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class MagicTavernHelper : MonoBehaviour
{
	public enum ADS_FAILED
	{
		None,
		NetworkNotReachable,
		NotReady,
		UserCancel,
		Timeout,
		Cooldown
	}

	private sealed class _Post_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal string url;

		internal byte[] data;

		internal Dictionary<string, string> header;

		internal WWW _www___0;

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

		public _Post_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._www___0 = new WWW(this.url, this.data, this.header);
				this._current = this._www___0;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				if (this._www___0.error != null)
				{
					UnityEngine.Debug.Log("MT error is :" + this._www___0.error);
					MTBrokenNetworkLog.AddNewLog(this._www___0.error, this.data, this.header);
				}
				else
				{
					UnityEngine.Debug.Log("MT request successed");
				}
				this._PC = -1;
				break;
			}
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

	public const string confVersion = "1000";

	public const string MT_URL = "http://track.magictavern.com/track.do";

	public const string MT_EVENT_DAMN_IT = "damnit";

	public const string MT_EVENT_DEVICEINFO = "deviceinfo";

	public const string MT_EVENT_INSTALL = "install";

	public const string MT_EVENT_LOGIN = "login";

	public const string MT_EVENT_LEVELSTART = "levelStart";

	public const string MT_EVENT_LEVELEND = "levelEnd";

	public const string MT_EVENT_BACKGROUND = "background";

	public const string MT_ADS_INTERSTITIAL = "interstitial";

	public const string MT_ADS_REWARDEDVIDEO = "rewardedVideo";

	public const string MT_ADS_BANNER = "banner";

	public const string MT_SKINS = "skins";

	private static string AppID = string.Empty;

	private static string AppKey = string.Empty;

	private static MagicTavernHelper Instance;

	public static string Level;

	private DateTime sessionStartTime = DateTime.Now;

	private const string MT_EVENT_SESSION = "sessionRecord";

	private const string SESSION_START_KEY = "SessionStart";

	private const string SESSION_END_KEY = "SessionEnd";

	private const string SESSION_TIME_KEY = "SessionTime";

	private const string SESSION_INDEX_KEY = "SessionIndex";

	private const string SESSION_OVER_KEY = "SessionOver";

	private const long SESSION_INTERUPTION_THRESHOLD = 60L;

	public static bool IsMagicTavernEnabled()
	{
		return true;
	}

	public static void Init(string appId, string appKey)
	{
		MagicTavernHelper.AppID = appId;
		MagicTavernHelper.AppKey = appKey;
		if (!MagicTavernHelper.Instance)
		{
			MagicTavernHelper.Instance = new GameObject("__Magic__Tavern__Helper__").AddComponent<MagicTavernHelper>();
			UnityEngine.Object.DontDestroyOnLoad(MagicTavernHelper.Instance.gameObject);
		}
		MTBrokenNetworkLog.Init(MagicTavernHelper.Instance.transform, new Action<string>(MagicTavernHelper.Instance.ResendTrackMTEvent), new Action<byte[], Dictionary<string, string>>(MagicTavernHelper.Instance.ResendTrackMTEvent));
	}

	public static void Track(string mt_event_name, params object[] param)
	{
		if (!MagicTavernHelper.IsMagicTavernEnabled())
		{
			return;
		}
		if (MagicTavernHelper.Instance)
		{
			if (param.Length < 9)
			{
				MagicTavernHelper.Instance.TrackMTEvent(mt_event_name, param);
			}
			else
			{
				UnityEngine.Debug.LogWarning("MagicTavern 统计参数不能超过8个!此次操作将不做统计!");
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("未正确初始化[MagicTavernHelper],请在游戏启动调用MagicTavernHelper.Init!此次操作将不做统计!");
		}
	}

	public static void TrackADS(string adsName, int actionID, int originID, MagicTavernHelper.ADS_FAILED failed = MagicTavernHelper.ADS_FAILED.None)
	{
		string text = "NotReachable";
		if (WJUtils.IsWifiNetworkAvailable())
		{
			text = "ReachableViaLocalAreaNetwork";
		}
		else if (WJUtils.IsNetworkAvailable())
		{
			text = "ReachableViaCarrierDataNetwork";
		}
		MagicTavernHelper.Track(adsName, new object[]
		{
			actionID,
			originID,
			(failed != MagicTavernHelper.ADS_FAILED.None) ? failed.ToString() : string.Empty,
			text
		});
	}

	public static void TriggerAppStartEvent()
	{
	}

	private Dictionary<string, string> ToHeader(string json)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["Content-Type"] = "application/json";
		dictionary["sign"] = CryptUtils.Md5(json + MagicTavernHelper.AppKey, Encoding.UTF8).ToLower();
		return dictionary;
	}

	private string ToJson(string action, params object[] param)
	{
		string str = "{";
		str = str + "\"app\":\"" + MagicTavernHelper.AppID + "\",";
		str += "\"client\":\"Android\",";
		str = str + "\"uid\":\"" + WJUtils.GetDeviceUID() + "\",";
		str = str + "\"ver\":\"" + Application.version + "\",";
		str += "\"confVersion\":1000,";
		str += "\"fbid\":null,";
		string text = "{\"action\":\"" + action + "\",";
		text = text + "\"level\":" + MagicTavernHelper.Level.ToString() + ",";
		string str2 = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
		text = text + "\"time\":\"" + str2 + "\",";
		for (int i = 0; i < param.Length; i++)
		{
			text += string.Format("\"v{0}\":\"{1}\",", i + 1, param[i]);
		}
		text = text.Remove(text.Length - 1, 1) + "}";
		str += string.Format("\"events\":[{0}]", text);
		return str + "}";
	}

	private string getConfVersion()
	{
		string text = Application.version.Replace(".", string.Empty);
		int i = 0;
		int num = 4 - text.Length;
		while (i < num)
		{
			text += "0";
			i++;
		}
		return text;
	}

	private void TrackMTEvent(string mt_event_name, params object[] param)
	{
		if (mt_event_name == "background" || mt_event_name == "login" || mt_event_name == "sessionRecord" || mt_event_name == "interstitial")
		{
			MagicTavernHelper.Level = UserModel.GetMaxPassLevel(true);
		}
		else
		{
			MagicTavernHelper.Level = UserModel.GetMaxPassLevel(false);
		}
		string text = this.ToJson(mt_event_name, param);
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			MTBrokenNetworkLog.AddNewLog(text);
			return;
		}
		UnityEngine.Debug.Log("<color=#00FF00>TRACK-MT-EVENT:</color> <color=#F020F0>" + mt_event_name + "</color>\n" + text);
		base.StartCoroutine(this.Post("http://track.magictavern.com/track.do", Encoding.UTF8.GetBytes(text), this.ToHeader(text)));
	}

	private IEnumerator Post(string url, byte[] data, Dictionary<string, string> header)
	{
		MagicTavernHelper._Post_c__Iterator0 _Post_c__Iterator = new MagicTavernHelper._Post_c__Iterator0();
		_Post_c__Iterator.url = url;
		_Post_c__Iterator.data = data;
		_Post_c__Iterator.header = header;
		return _Post_c__Iterator;
	}

	private void Awake()
	{
		if (PlayerPrefs.GetInt("_MT_INSTALL", 0) == 0)
		{
			AbTest.InstallInit();
			PlayerPrefs.SetInt("_MT_INSTALL", 1);
			PlayerPrefs.Save();
			this.TrackMTEvent("install", new object[]
			{
				WJUtils.GetCompassBIStyleDeviceType(),
				WJUtils.GetDeviceOSFullname(),
				0,
				TimeZone.CurrentTimeZone.StandardName
			});
			this.TrackDeviceInfo();
		}
		AbTest.LoginInit();
		base.InvokeRepeating("OnTickBySecond", 1f, 1f);
	}

	public void OnApplicationPause(bool pauseStatus)
	{
		if (!pauseStatus)
		{
			this.TrackMTEvent("login", new object[]
			{
				UserModel.Inst.GetStarCount()
			});
			this.sessionStartTime = DateTime.Now;
			this.UpdateSessionIndex();
		}
		else
		{
			this.TrackMTEvent("background", new object[]
			{
				(DateTime.Now - this.sessionStartTime).TotalSeconds,
				UserModel.Inst.GetStarCount(),
				UserModel.MTPlayCount
			});
			UserModel.MTPlayCount = 0;
			this.TrackSessionRecord2();
		}
	}

	private void OnTickBySecond()
	{
		TimeSpan timeOfDay = DateTime.Now.TimeOfDay;
		if (timeOfDay.Hours == 23 && timeOfDay.Minutes == 59 && timeOfDay.Seconds == 59)
		{
			this.TrackMTEvent("background", new object[]
			{
				(DateTime.Now - this.sessionStartTime).TotalSeconds,
				UserModel.Inst.GetStarCount(),
				UserModel.MTPlayCount
			});
			UserModel.MTPlayCount = 0;
			long num = (long)Math.Floor((DateTime.Now - this.sessionStartTime).TotalSeconds);
			this.TrackMTEvent("sessionRecord", new object[]
			{
				num,
				PlayerPrefs.GetInt("SessionIndex", 0)
			});
		}
		else if (timeOfDay.Hours == 0 && timeOfDay.Minutes == 0 && timeOfDay.Seconds == 0)
		{
			this.TrackMTEvent("login", new object[]
			{
				UserModel.Inst.GetStarCount()
			});
			this.sessionStartTime = DateTime.Now;
			PlayerPrefs.SetInt("SessionIndex", PlayerPrefs.GetInt("SessionIndex", 0) + 1);
			PlayerPrefs.Save();
		}
	}

	public static DateTime ConvertFromUnixTimestamp(double timestamp)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		return dateTime.AddSeconds(timestamp);
	}

	public static long ConvertToUnixTimestamp(DateTime date)
	{
		DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		return (long)Math.Floor((date.ToUniversalTime() - d).TotalSeconds);
	}

	public static bool CheckDateChange(DateTime time1, DateTime time2)
	{
		return time1.Year != time2.Year || time1.Month != time2.Month || time1.Day != time2.Day;
	}

	public void UpdateSessionIndex()
	{
		long num = Convert.ToInt64(PlayerPrefs.GetString("SessionOver", "0"));
		long num2 = MagicTavernHelper.ConvertToUnixTimestamp(this.sessionStartTime);
		if (num > num2 || num2 - num > 60L)
		{
			PlayerPrefs.SetInt("SessionIndex", PlayerPrefs.GetInt("SessionIndex", 0) + 1);
			PlayerPrefs.Save();
		}
	}

	public void TrackSessionRecord2()
	{
		PlayerPrefs.SetString("SessionOver", MagicTavernHelper.ConvertToUnixTimestamp(DateTime.Now).ToString());
		long num = (long)Math.Floor((DateTime.Now - this.sessionStartTime).TotalSeconds);
		if (num > 0L)
		{
			this.TrackMTEvent("sessionRecord", new object[]
			{
				num,
				PlayerPrefs.GetInt("SessionIndex", 0)
			});
		}
	}

	private void ResendTrackMTEvent(string json)
	{
		UnityEngine.Debug.Log("<color=#00FF00>RESEND-TRACK-MT-EVENT:</color>\n" + json);
		base.StartCoroutine(this.Post("http://track.magictavern.com/track.do", Encoding.UTF8.GetBytes(json), this.ToHeader(json)));
	}

	private void ResendTrackMTEvent(byte[] data, Dictionary<string, string> header)
	{
		UnityEngine.Debug.Log("<color=#00FF00>RESENDFAILED-TRACK-MT-EVENT:</color>");
		base.StartCoroutine(this.Post("http://track.magictavern.com/track.do", data, header));
	}

	public void TrackDeviceInfo()
	{
		string deviceModel = SystemInfo.deviceModel;
		string operatingSystem = SystemInfo.operatingSystem;
		string text = SystemInfo.systemMemorySize.ToString();
		string graphicsDeviceName = SystemInfo.graphicsDeviceName;
		string text2 = SystemInfo.graphicsMemorySize.ToString();
		string text3 = SystemInfo.graphicsDeviceType.ToString();
		string text4 = string.Empty;
		IEnumerator enumerator = Enum.GetValues(typeof(TextureFormat)).GetEnumerator();
		int num = 0;
		int num2 = 0;
		while (enumerator.MoveNext())
		{
			TextureFormat textureFormat = (TextureFormat)enumerator.Current;
			bool flag = false;
			try
			{
				flag = SystemInfo.SupportsTextureFormat(textureFormat);
			}
			catch (Exception)
			{
				flag = false;
			}
			if (flag)
			{
				int num3 = (int)textureFormat;
				if (num2 == 0)
				{
					text4 += num3.ToString("X");
					num2 = 1;
				}
				else if (num3 == num + num2)
				{
					num2++;
				}
				else
				{
					if (num2 > 1)
					{
						text4 = text4 + "-" + num2.ToString("X");
					}
					text4 = text4 + "|" + num3.ToString("X");
					num = num3;
					num2 = 1;
				}
			}
		}
		if (num2 > 1)
		{
			text4 = text4 + "-" + num2.ToString("X");
		}
		string text5 = string.Format("{0}|{1}|{2}", SystemInfo.maxTextureSize, SystemInfo.graphicsShaderLevel, SystemInfo.processorType);
		this.TrackMTEvent("deviceinfo", new object[]
		{
			deviceModel,
			operatingSystem,
			text,
			graphicsDeviceName,
			text2,
			text3,
			text4,
			text5
		});
	}
}
