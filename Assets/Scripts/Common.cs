using LIBII;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Common
{
	public enum AdsPosition
	{
		Top,
		Bottom
	}

	private static Common.AdsPosition s_lastAdPos = Common.AdsPosition.Bottom;

	private static int s_chartBoostCount = 0;

	private static Dictionary<string, int> s_soundIds = new Dictionary<string, int>();

	private static string s_default_audio_path = "Audio/";

	private static string s_localization_audio_path = null;

	public const string URL_LIKE_US = "";

	public const string FACEBOOK_APPID = "";

	public const string FACEBOOK_SHARE = "";

	public const string SAVETO_ALBUM_FOLDER = "SplashTheDuck";

	public const string PACKAGE_NAME = "com.stx.splashtheduck";

	public const string DESCRIPTION_NAME = "Splash The Duck";

	public const string THIS_APP_ID = "";

	public const string THIS_APP_STORE_URL = "market://details?id=com.stx.splashtheduck";

	public const string THIS_APP_STORE_URL_LONG = "https://play.google.com/store/apps/details?id=com.stx.splashtheduck";

	public const string EMAIL_FEEDBACK = "contact@supertapx.com|Feedback about Splash The Duck!(Android) V";

	public const string EMAIL_SHARE = "|Splash The Duck!|Look at the best app for you!\n\nIt's the funnest game.\n\nHere is a link to Splash The Duck in the App Store: https://play.google.com/store/apps/details?id=com.stx.splashtheduck|";

	public const string RATE_TITLE = "";

	public const string RATE_MESSAGE = "If you like this game please rate it to keep free updates coming. Thanks!";

	public const string RATE_URL = "https://play.google.com/store/apps/details?id=com.stx.splashtheduck";

	public const bool RATE_NEW_VERSION_RATE_AGAIN = false;

	public const string URL_MOREGAME_DEF = "";

	public static bool IsInAppPurchased()
	{
		bool flag = true;
		Dictionary<string, StoreTemplate> dictionary = StoreTemplate.Dic();
		foreach (KeyValuePair<string, StoreTemplate> current in dictionary)
		{
			if (!(current.Key == "GetAll") && !(current.Key == "NoAds"))
			{
				flag &= WJUtils.IsInAppPurchased(current.Value.IapId, null);
			}
		}
		return WJUtils.IsInAppPurchased(StoreTemplate.Tem(new object[]
		{
			"GetAll"
		}).IapId, null) || flag;
	}

	public static bool IsGamePurchased()
	{
		return WJUtils.IsInAppPurchased(StoreTemplate.Tem(new object[]
		{
			"Game"
		}).IapId, StoreTemplate.Tem(new object[]
		{
			"GetAll"
		}).IapId);
	}

	public static bool IsItemPurchased()
	{
		return WJUtils.IsInAppPurchased(StoreTemplate.Tem(new object[]
		{
			"Item"
		}).IapId, StoreTemplate.Tem(new object[]
		{
			"GetAll"
		}).IapId);
	}

	public static bool IsDressPurchased()
	{
		return WJUtils.IsInAppPurchased(StoreTemplate.Tem(new object[]
		{
			"Dress"
		}).IapId, StoreTemplate.Tem(new object[]
		{
			"GetAll"
		}).IapId);
	}

	public static bool IsPonyPurchased()
	{
		return WJUtils.IsInAppPurchased(StoreTemplate.Tem(new object[]
		{
			"Pony"
		}).IapId, StoreTemplate.Tem(new object[]
		{
			"GetAll"
		}).IapId);
	}

	public static string GetIapGetAllPrice()
	{
		return Common.GetProductPriceByIapId(StoreTemplate.Tem(new object[]
		{
			"GetAll"
		}).IapId);
	}

	public static string GetIapGamePrice()
	{
		return Common.GetProductPriceByIapId(StoreTemplate.Tem(new object[]
		{
			"Game"
		}).IapId);
	}

	public static string GetIapItemPrice()
	{
		return Common.GetProductPriceByIapId(StoreTemplate.Tem(new object[]
		{
			"Item"
		}).IapId);
	}

	public static string GetIapNoAdsPrice()
	{
		return Common.GetProductPriceByIapId(StoreTemplate.Tem(new object[]
		{
			"NoAds"
		}).IapId);
	}

	public static string GetIapDressPrice()
	{
		return Common.GetProductPriceByIapId(StoreTemplate.Tem(new object[]
		{
			"Dress"
		}).IapId);
	}

	public static string GetIapPonyPrice()
	{
		return Common.GetProductPriceByIapId(StoreTemplate.Tem(new object[]
		{
			"Pony"
		}).IapId);
	}

	public static string GetIapIslandPrice(string key)
	{
		return Common.GetProductPriceByIapId(StoreTemplate.Tem(new object[]
		{
			key
		}).IapId);
	}

	public static void ShowAds(Common.AdsPosition adPos = Common.AdsPosition.Bottom)
	{
		if (Common.IsAdsRemoved())
		{
			return;
		}
		Common.s_lastAdPos = adPos;
		int y = (adPos != Common.AdsPosition.Top) ? (-2) : (-1);
		WJUtils.ShowAds(string.Empty, '5', -2, y);
	}

	public static Common.AdsPosition GetAdsLastPos()
	{
		return Common.s_lastAdPos;
	}

	public static void ShowAdsLastPos()
	{
		Common.ShowAds(Common.s_lastAdPos);
	}

	public static Vector2 GetAdsRealSize()
	{
		if (Common.IsAdsRemoved())
		{
			return Vector2.zero;
		}
		return WJUtils.GetAdsRealSize('5');
	}

	public static bool IsAdsRemoved()
	{
		return WJUtils.IsAdsRemoved();
	}

	public static bool IsAdsReallyVisible()
	{
		return WJUtils.IsAdsReallyVisible();
	}

	public static void ShowChartBoost(bool forceCount = false, bool forceShow = false, ADSManager.CBLoaction location = ADSManager.CBLoaction.LevelEnd)
	{
		Common.ChartBoostAddCounter();
		Common.ChartBoostCheckCounterAndShow(forceCount, forceShow, location);
	}

	public static void ChartBoostAddCounter()
	{
		Common.s_chartBoostCount++;
	}

	public static void ChartBoostCheckCounterAndShow(bool forceCount = false, bool forceShow = false, ADSManager.CBLoaction location = ADSManager.CBLoaction.LevelEnd)
	{
		if (WJUtils.IsAdsRemoved() && !forceShow)
		{
			return;
		}
		if (Common.s_chartBoostCount == 3 || forceCount || forceShow)
		{
			UnityEngine.Debug.Log("show chart boost integration.");
			WJUtils.CallAction_Void(31, string.Empty);
			Common.s_chartBoostCount = 0;
		}
	}

	public static string GetProductPriceByIapId(List<object> productInfoList, string iapId)
	{
		foreach (object current in productInfoList)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)current;
			if ((string)dictionary["productID"] == iapId)
			{
				return (string)dictionary["productPrice"];
			}
		}
		return "unknown";
	}

	public static string GetProductPriceByIapId(string iapId)
	{
		return Common.GetProductPriceByIapId(WJUtils.GetProductInfoList(), iapId);
	}

	public static string GetProductNumericPriceByIapId(List<object> productInfoList, string iapId, bool isMagicTavern)
	{
		foreach (object current in productInfoList)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)current;
			if ((string)dictionary["productID"] == iapId)
			{
				if (dictionary.ContainsKey("productPriceNumber"))
				{
					string result;
					if (isMagicTavern)
					{
						result = Convert.ToInt32(Convert.ToSingle(dictionary["productPriceNumber"]) * 100f).ToString();
						return result;
					}
					result = Convert.ToSingle(dictionary["productPriceNumber"]).ToString("F2", CultureInfo.InvariantCulture);
					return result;
				}
				else
				{
					string text = (string)dictionary["productPrice"];
					int num = 0;
					string text2 = text;
					for (int i = 0; i < text2.Length; i++)
					{
						char c = text2[i];
						if (c >= '0' && c <= '9')
						{
							break;
						}
						num++;
					}
					if (num < text.Length)
					{
						string result;
						if (isMagicTavern)
						{
							result = Convert.ToInt32(Convert.ToSingle(text.Substring(num)) * 100f).ToString();
							return result;
						}
						result = Convert.ToSingle(text.Substring(num)).ToString("F2", CultureInfo.InvariantCulture);
						return result;
					}
				}
			}
		}
		return "0";
	}

	public static string GetProductNumericPriceByIapId(string iapId, bool isMagicTavern)
	{
		return Common.GetProductNumericPriceByIapId(WJUtils.GetProductInfoList(), iapId, isMagicTavern);
	}

	public static string GetProductCurrencyCodeByIapId(List<object> productInfoList, string iapId)
	{
		foreach (object current in productInfoList)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)current;
			if ((string)dictionary["productID"] == iapId && dictionary.ContainsKey("productCurrencyCode"))
			{
				return (string)dictionary["productCurrencyCode"];
			}
		}
		return "USD";
	}

	public static string GetProductCurrencyCodeByIapId(string iapId)
	{
		return Common.GetProductCurrencyCodeByIapId(WJUtils.GetProductInfoList(), iapId);
	}

	public static void PlaySoundEffect3D(GameObject go, string soundKey, bool isloop, float minDis, float maxDis, float volume, float delay, int audioSourceIndex = 0)
	{
		if (!Voice.GetVoice())
		{
			return;
		}
		AudioSource[] components = go.GetComponents<AudioSource>();
		AudioSource audioSource;
		if (components.Length <= audioSourceIndex)
		{
			audioSource = go.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
		}
		else
		{
			audioSource = components[audioSourceIndex];
		}
		SoundTemplate soundTemplate;
		if (!SoundTemplate.Dic().TryGetValue(soundKey, out soundTemplate))
		{
			UnityEngine.Debug.Log("not found SoundEffect key:" + soundKey);
			return;
		}
		string[] array = soundTemplate.Name.Split(new char[]
		{
			':'
		});
		audioSource.clip = Resources.Load<AudioClip>(array[WJUtils.RandomInt(array.Length)]);
		audioSource.loop = isloop;
		audioSource.volume = volume;
		audioSource.rolloffMode = AudioRolloffMode.Linear;
		audioSource.minDistance = minDis;
		audioSource.maxDistance = maxDis;
		audioSource.PlayDelayed(delay);
	}

	public static void PlaySoundEffect3D(GameObject go, string soundKey, int audioSourceIndex = 0)
	{
		SoundTemplate soundTemplate;
		if (!SoundTemplate.Dic().TryGetValue(soundKey, out soundTemplate))
		{
			UnityEngine.Debug.Log("not found SoundEffect key:" + soundKey);
			return;
		}
		Common.PlaySoundEffect3D(go, soundKey, audioSourceIndex);
	}

	public static void StopSoundEffect3D(GameObject go, int audioSourceIndex = -1)
	{
		AudioSource[] components = go.GetComponents<AudioSource>();
		if (audioSourceIndex == -1)
		{
			AudioSource[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				AudioSource audioSource = array[i];
				audioSource.Stop();
			}
		}
		else if (audioSourceIndex < components.Length)
		{
			components[audioSourceIndex].Stop();
		}
	}

	public static void PlayBackground(string key)
	{
		if (!Voice.GetMusic())
		{
			return;
		}
		string text = SoundTemplate.Tem(new object[]
		{
			key
		}).Name;
		string[] array = text.Split(new char[]
		{
			':'
		});
		text = array[UnityEngine.Random.Range(0, array.Length)];
		WJSound2D.PlayBackgroundMusic(text, 1f);
	}

	public static int PlaySoundEffect(string soundKey)
	{
		if (!Voice.GetVoice())
		{
			return -1;
		}
		SoundTemplate soundTemplate;
		if (!SoundTemplate.Dic().TryGetValue(soundKey, out soundTemplate))
		{
			UnityEngine.Debug.LogWarning("not found SoundEffect key:" + soundKey);
			return -1;
		}
		return Common.PlaySoundEffect(soundKey, soundTemplate.Delay, soundTemplate.IsLoop, soundTemplate.Volume, false, soundTemplate);
	}

	public static int PlaySoundEffect(string soundKey, float delay, bool isLoop, float volume, bool bOneShot = false, SoundTemplate st = null)
	{
		if (!Voice.GetVoice())
		{
			return -1;
		}
		int num = -1;
		if (st == null && !SoundTemplate.Dic().TryGetValue(soundKey, out st))
		{
			UnityEngine.Debug.LogWarning("not found SoundEffect key:" + soundKey);
			return -1;
		}
		string[] array = st.Name.Split(new char[]
		{
			':'
		});
		if (array.Length == 0)
		{
			UnityEngine.Debug.LogWarning("没有指定的音效资源-播放失败:" + soundKey);
			return -1;
		}
		string text = array[UnityEngine.Random.Range(0, array.Length)];
		if (text == string.Empty)
		{
			UnityEngine.Debug.LogWarning("没有指定的音效资源-播放失败:" + soundKey);
			return -1;
		}
		if (st.MutexSoundID != string.Empty)
		{
			array = st.MutexSoundID.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				if (Common.IsPlaySoundEffect(array[i]))
				{
					UnityEngine.Debug.Log("播放声音失败[被互斥掉了!]: " + soundKey);
					return -1;
				}
			}
		}
		if (st.MutexAndStopSoundID != string.Empty)
		{
			array = st.MutexAndStopSoundID.Split(new char[]
			{
				','
			});
			for (int j = 0; j < array.Length; j++)
			{
				Common.StopSoundEffect(array[j]);
			}
		}
		if (Common.s_localization_audio_path == null)
		{
			Common.s_localization_audio_path = "Audio/" + IScriptableObjectReader<SettingReader, SettingAsset>.ScriptableObject.Language.ToString() + "/";
		}
		AudioClip audioClip = Resources.Load<AudioClip>(Common.s_localization_audio_path + text);
		if (audioClip == null)
		{
			audioClip = Resources.Load<AudioClip>(Common.s_default_audio_path + text);
		}
		if (audioClip == null)
		{
			UnityEngine.Debug.LogWarning(string.Format("未找到  Audio Clip :{0} 和 {1}", Common.s_localization_audio_path + text, Common.s_default_audio_path + text));
			return -1;
		}
		if (bOneShot)
		{
			WJSound2D.PlayEffectOneShot(audioClip, volume);
		}
		else
		{
			num = WJSound2D.PlayEffect(audioClip, isLoop, volume, delay);
			if (num != -1)
			{
				if (Common.s_soundIds.ContainsKey(soundKey))
				{
					Common.s_soundIds[soundKey] = num;
				}
				else
				{
					Common.s_soundIds.Add(soundKey, num);
				}
			}
		}
		return num;
	}

	public static void PlaySoundEffectOneShot(string soundKey)
	{
		if (!Voice.GetVoice())
		{
			return;
		}
		SoundTemplate soundTemplate;
		if (!SoundTemplate.Dic().TryGetValue(soundKey, out soundTemplate))
		{
			UnityEngine.Debug.LogWarning("not found SoundEffect key:" + soundKey);
			return;
		}
		Common.PlaySoundEffect(soundKey, soundTemplate.Delay, soundTemplate.IsLoop, soundTemplate.Volume, true, soundTemplate);
	}

	public static void StopSoundEffect(string soundKey)
	{
		if (Common.s_soundIds.ContainsKey(soundKey))
		{
			Common.StopSoundEffect(Common.s_soundIds[soundKey]);
		}
	}

	public static void StopSoundEffect(int soundId)
	{
		WJSound2D.StopEffect(soundId);
	}

	public static bool IsPlaySoundEffect(string soundKey)
	{
		bool result = false;
		if (Common.s_soundIds.ContainsKey(soundKey))
		{
			result = Common.IsPlaySoundEffect(Common.s_soundIds[soundKey]);
		}
		return result;
	}

	public static bool IsPlaySoundEffect(int soundId)
	{
		return WJSound2D.IsPlaying(soundId);
	}

	public static void PauseEffect(int soundId)
	{
		WJSound2D.PauseEffect(soundId);
	}

	public static void PauseEffect(string soundKey)
	{
		if (Common.s_soundIds.ContainsKey(soundKey))
		{
			Common.PauseEffect(Common.s_soundIds[soundKey]);
		}
	}

	public static void ResumeEffect(int soundId)
	{
		WJSound2D.ResumeEffect(soundId);
	}

	public static void ResumeEffect(string soundKey)
	{
		if (Common.s_soundIds.ContainsKey(soundKey))
		{
			Common.ResumeEffect(Common.s_soundIds[soundKey]);
		}
	}

	public static void ShowFirstTimeRate()
	{
		if (PlayerPrefs.GetInt("first_rate_show", 0) == 0)
		{
			WJUtils.FirstTimeRating(string.Empty, "If you like this game please rate it to keep free updates coming. Thanks!", "https://play.google.com/store/apps/details?id=com.stx.splashtheduck");
			PlayerPrefs.SetInt("first_rate_show", 1);
			PlayerPrefs.Save();
		}
	}

	public static string GetString(string key, params object[] args)
	{
		return string.Format(StringsTemplate.Tem(new object[]
		{
			key
		}).text, args);
	}

	public static string GetString(string key)
	{
		return StringsTemplate.Tem(new object[]
		{
			key
		}).text;
	}

	public static Texture2D Snapshoot(Camera camera, int w, int h)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(w, h, 24);
		Texture2D texture2D = new Texture2D(w, h, TextureFormat.ARGB32, false);
		RenderTexture renderTexture = camera.targetTexture;
		camera.targetTexture = temporary;
		camera.Render();
		camera.targetTexture = renderTexture;
		renderTexture = RenderTexture.active;
		RenderTexture.active = temporary;
		texture2D.ReadPixels(new Rect(0f, 0f, (float)w, (float)h), 0, 0);
		texture2D.Apply();
		RenderTexture.active = renderTexture;
		RenderTexture.ReleaseTemporary(temporary);
		return texture2D;
	}

	public static bool IsAuditPackage()
	{
		return false;
	}

	public static bool IsIapDisabled()
	{
		return false;
	}

	public static bool IsDomesticAdPackage()
	{
		return false;
	}

	public static void PlayVibration(int strength = 1)
	{
		if (!UserModel.GetBration())
		{
			return;
		}
		WJUtils.PlayUIImpactFeedback(strength);
	}
}
