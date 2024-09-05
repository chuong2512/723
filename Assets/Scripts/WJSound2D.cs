using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class WJSound2D : MonoBehaviour
{
	public delegate void OnMuteDelegate(bool isMute);

	public class PlayingEffectInfo
	{
		public AudioSource Source;

		public float StartTime;

		public bool IsInPool;

		public int KeyEffectId;

		public bool IsPause;

		public PlayingEffectInfo()
		{
			this.IsPause = false;
		}

		public PlayingEffectInfo(AudioSource source, float startTime, bool isInPool, int effectId)
		{
			this.Source = source;
			this.StartTime = startTime;
			this.IsInPool = isInPool;
			this.KeyEffectId = effectId;
		}
	}

	public static string audioClipPathPrefix = "Audio/";



	private const int EFFECT_SOURCE_POOL_SIZE = 2;

	private const float EFFECT_MIN_PLAY_TIME = 0.1f;

	private static WJSound2D s_instance = null;

	private AudioListener m_audioListener;

	private AudioSource m_bgAudioSource;

	private int m_lastEffectId = 1024;

	private Dictionary<int, WJSound2D.PlayingEffectInfo> m_allEffectDict = new Dictionary<int, WJSound2D.PlayingEffectInfo>();

	private static string s_lastPlayingBgMusicName = string.Empty;

	private static bool muteSoundEffectGetted = false;

	private static bool muteSoundEffect = false;

	private static bool muteBackgroundGetted = false;

	private static bool muteBackground = false;

	public static event WJSound2D.OnMuteDelegate OnMuteCallback;

	public static AudioSource BgAudioSource
	{
		get
		{
			return WJSound2D.s_instance.m_bgAudioSource;
		}
	}

	public static bool Mute
	{
		get
		{
			return PlayerPrefs.GetInt("isSound2DMute", 0) == 1;
		}
		set
		{
			PlayerPrefs.SetInt("isSound2DMute", (!value) ? 0 : 1);
			PlayerPrefs.Save();
			AudioListener.volume = (float)((!value) ? 1 : 0);
			if (WJSound2D.OnMuteCallback != null)
			{
				WJSound2D.OnMuteCallback(value);
			}
		}
	}

	public static bool MuteSoundEffect
	{
		get
		{
			if (!WJSound2D.muteSoundEffectGetted)
			{
				WJSound2D.muteSoundEffectGetted = true;
				WJSound2D.muteSoundEffect = (PlayerPrefs.GetInt("isSound2DMuteSoundEffect", 0) == 1);
			}
			return WJSound2D.muteSoundEffect;
		}
		set
		{
			WJSound2D.muteSoundEffect = value;
			PlayerPrefs.SetInt("isSound2DMuteSoundEffect", (!value) ? 0 : 1);
			PlayerPrefs.Save();
		}
	}

	public static bool MuteBackground
	{
		get
		{
			if (!WJSound2D.muteBackgroundGetted)
			{
				WJSound2D.muteBackgroundGetted = true;
				WJSound2D.muteBackground = (PlayerPrefs.GetInt("isSound2DMuteBackground", 0) == 1);
			}
			return WJSound2D.muteBackground;
		}
		set
		{
			WJSound2D.muteBackground = value;
			PlayerPrefs.SetInt("isSound2DMuteBackground", (!value) ? 0 : 1);
			PlayerPrefs.Save();
			if (WJSound2D.s_instance != null)
			{
				WJSound2D.s_instance.m_bgAudioSource.mute = WJSound2D.muteBackground;
			}
		}
	}

	private void Awake()
	{
		if (WJSound2D.s_instance != null)
		{
			return;
		}
		WJSound2D.s_instance = this;
		this.InitAudioListener();
		AudioListener.volume = (float)((!WJSound2D.Mute) ? 1 : 0);
		this.m_bgAudioSource = base.gameObject.AddComponent<AudioSource>();
		this.m_bgAudioSource.loop = true;
		this.m_bgAudioSource.playOnAwake = false;
		this.m_bgAudioSource.mute = WJSound2D.MuteBackground;
	}

	private void Start()
	{
		base.InvokeRepeating("CheckEffectAudioSource", 1f, 1f);
	}

	public static void PlayBackgroundMusic(string audioClip, float volume = 1f)
	{
		if (audioClip == WJSound2D.s_lastPlayingBgMusicName)
		{
			return;
		}
		AudioClip audioClip2 = Resources.Load<AudioClip>(WJSound2D.audioClipPathPrefix + audioClip);
		UnityEngine.Debug.Log("PlayBackgroundMusic " + WJSound2D.audioClipPathPrefix + audioClip);
		if (audioClip2 != null)
		{
			WJSound2D.s_lastPlayingBgMusicName = audioClip;
			WJSound2D.PlayBackgroundMusic(audioClip2, volume);
		}
		else
		{
			UnityEngine.Debug.Log("WJSound2D: not found audio clip " + audioClip);
		}
	}

	public static void PlayBackgroundMusic(AudioClip audioClip, float volume = 1f)
	{
		if (WJSound2D.s_instance == null)
		{
			return;
		}
		WJSound2D.s_instance.m_bgAudioSource.clip = audioClip;
		WJSound2D.s_instance.m_bgAudioSource.volume = volume;
		WJSound2D.s_instance.m_bgAudioSource.Play();
		if (WJSound2D.MuteBackground)
		{
			WJSound2D.s_instance.m_bgAudioSource.mute = true;
		}
	}

	public static void PauseBackgroundMusic()
	{
		if (WJSound2D.s_instance == null)
		{
			return;
		}
		WJSound2D.s_instance.m_bgAudioSource.Pause();
	}

	public static void ResumeBackgroundMusic()
	{
		if (WJSound2D.s_instance == null)
		{
			return;
		}
		WJSound2D.s_instance.m_bgAudioSource.Play();
	}

	public static int PlayEffect(string audioClip, bool loop = false, float volume = 1f, float delay = 0f)
	{
		AudioClip audioClip2 = Resources.Load<AudioClip>(WJSound2D.audioClipPathPrefix + audioClip);
		if (audioClip2 != null)
		{
			return WJSound2D.PlayEffect(audioClip2, loop, volume, delay);
		}
		UnityEngine.Debug.LogWarning("WJSound2D: not found audio clip " + audioClip);
		return -1;
	}

	public static int PlayEffect(AudioClip audioClip, bool loop = false, float volume = 1f, float delay = 0f)
	{
		if (WJSound2D.s_instance == null || WJSound2D.MuteSoundEffect)
		{
			return -1;
		}
		return WJSound2D.s_instance.PlayNewEffect(audioClip, loop, volume, delay);
	}

	public static void PlayEffectOneShot(string audioClip, float volume = 1f)
	{
		if (WJSound2D.s_instance == null || WJSound2D.MuteSoundEffect)
		{
			return;
		}
		AudioClip audioClip2 = Resources.Load<AudioClip>(WJSound2D.audioClipPathPrefix + audioClip);
		if (audioClip2 != null)
		{
			WJSound2D.s_instance.m_bgAudioSource.PlayOneShot(audioClip2, volume);
		}
		else
		{
			UnityEngine.Debug.LogWarning("WJSound2D: not found audio clip " + audioClip);
		}
	}

	public static void PlayEffectOneShot(AudioClip audioClip, float volume = 1f)
	{
		if (WJSound2D.s_instance == null || WJSound2D.MuteSoundEffect)
		{
			return;
		}
		WJSound2D.s_instance.m_bgAudioSource.PlayOneShot(audioClip, volume);
	}

	private int PlayNewEffect(AudioClip clip, bool loop, float volume, float delay)
	{
		WJSound2D.PlayingEffectInfo playingEffectInfo = null;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		Dictionary<int, WJSound2D.PlayingEffectInfo>.ValueCollection values = this.m_allEffectDict.Values;
		foreach (WJSound2D.PlayingEffectInfo current in values)
		{
			if (!current.Source.isPlaying && !current.IsPause && realtimeSinceStartup - current.StartTime > 0.1f)
			{
				playingEffectInfo = current;
				this.m_allEffectDict.Remove(current.KeyEffectId);
				break;
			}
		}
		if (playingEffectInfo == null)
		{
			playingEffectInfo = new WJSound2D.PlayingEffectInfo();
			playingEffectInfo.Source = base.gameObject.AddComponent<AudioSource>();
			playingEffectInfo.Source.playOnAwake = false;
			playingEffectInfo.IsInPool = (this.m_allEffectDict.Count < 2);
		}
		playingEffectInfo.StartTime = realtimeSinceStartup;
		playingEffectInfo.KeyEffectId = ++this.m_lastEffectId;
		playingEffectInfo.Source.loop = loop;
		playingEffectInfo.Source.clip = clip;
		playingEffectInfo.Source.volume = volume;
		playingEffectInfo.Source.PlayDelayed(delay);
		this.m_allEffectDict.Add(playingEffectInfo.KeyEffectId, playingEffectInfo);
		return playingEffectInfo.KeyEffectId;
	}

	public static void StopAllEffect()
	{
		if (WJSound2D.s_instance != null)
		{
			WJSound2D.s_instance.StopAllEffectImmediate();
		}
	}

	public static void StopAllEffectExcept(int effectId)
	{
		if (WJSound2D.s_instance != null)
		{
			WJSound2D.s_instance.StopAllEffectImmediateExcept(effectId);
		}
	}

	public static void StopEffect(int effectId)
	{
		if (effectId > -1 && WJSound2D.s_instance != null)
		{
			WJSound2D.s_instance.StopEffectImmediate(effectId);
		}
	}

	public static bool IsPlaying(int effectId)
	{
		bool result = false;
		if (WJSound2D.s_instance != null)
		{
			WJSound2D.PlayingEffectInfo playingEffectInfo = WJSound2D.s_instance.QueryEffect(effectId);
			if (playingEffectInfo != null)
			{
				result = playingEffectInfo.Source.isPlaying;
			}
		}
		return result;
	}

	public static void PauseEffect(int effectId)
	{
		if (WJSound2D.s_instance != null)
		{
			WJSound2D.PlayingEffectInfo playingEffectInfo = WJSound2D.s_instance.QueryEffect(effectId);
			if (playingEffectInfo != null)
			{
				playingEffectInfo.Source.Pause();
				playingEffectInfo.IsPause = true;
			}
		}
	}

	public static void ResumeEffect(int effectId)
	{
		if (WJSound2D.s_instance != null)
		{
			WJSound2D.PlayingEffectInfo playingEffectInfo = WJSound2D.s_instance.QueryEffect(effectId);
			if (playingEffectInfo != null)
			{
				playingEffectInfo.Source.Play();
				playingEffectInfo.IsPause = false;
			}
		}
	}

	public static WJSound2D.PlayingEffectInfo FindEffect(int effectId)
	{
		return WJSound2D.s_instance.QueryEffect(effectId);
	}

	private WJSound2D.PlayingEffectInfo QueryEffect(int effectId)
	{
		WJSound2D.PlayingEffectInfo result = null;
		if (this.m_allEffectDict.ContainsKey(effectId))
		{
			result = this.m_allEffectDict[effectId];
		}
		return result;
	}

	private void StopAllEffectImmediate()
	{
		LinkedList<WJSound2D.PlayingEffectInfo> linkedList = new LinkedList<WJSound2D.PlayingEffectInfo>();
		Dictionary<int, WJSound2D.PlayingEffectInfo>.ValueCollection values = this.m_allEffectDict.Values;
		foreach (WJSound2D.PlayingEffectInfo current in values)
		{
			current.IsPause = false;
			current.Source.Stop();
			current.Source.clip = null;
			if (!current.IsInPool)
			{
				linkedList.AddLast(current);
			}
		}
		foreach (WJSound2D.PlayingEffectInfo current2 in linkedList)
		{
			UnityEngine.Object.Destroy(current2.Source);
			this.m_allEffectDict.Remove(current2.KeyEffectId);
		}
	}

	private void StopAllEffectImmediateExcept(int effectId)
	{
		LinkedList<WJSound2D.PlayingEffectInfo> linkedList = new LinkedList<WJSound2D.PlayingEffectInfo>();
		Dictionary<int, WJSound2D.PlayingEffectInfo>.ValueCollection values = this.m_allEffectDict.Values;
		foreach (WJSound2D.PlayingEffectInfo current in values)
		{
			if (current.KeyEffectId != effectId)
			{
				current.IsPause = false;
				current.Source.Stop();
				current.Source.clip = null;
				if (!current.IsInPool)
				{
					linkedList.AddLast(current);
				}
			}
		}
		foreach (WJSound2D.PlayingEffectInfo current2 in linkedList)
		{
			UnityEngine.Object.Destroy(current2.Source);
			this.m_allEffectDict.Remove(current2.KeyEffectId);
		}
	}

	private void StopEffectImmediate(int effectId)
	{
		WJSound2D.PlayingEffectInfo playingEffectInfo;
		if (this.m_allEffectDict.TryGetValue(effectId, out playingEffectInfo))
		{
			playingEffectInfo.IsPause = false;
			playingEffectInfo.Source.Stop();
			playingEffectInfo.Source.clip = null;
			if (!playingEffectInfo.IsInPool)
			{
				UnityEngine.Object.Destroy(playingEffectInfo.Source);
				this.m_allEffectDict.Remove(effectId);
			}
		}
	}

	private void CheckEffectAudioSource()
	{
		if (this.m_allEffectDict.Count <= 2)
		{
			return;
		}
		LinkedList<int> linkedList = new LinkedList<int>();
		Dictionary<int, WJSound2D.PlayingEffectInfo>.ValueCollection values = this.m_allEffectDict.Values;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		foreach (WJSound2D.PlayingEffectInfo current in values)
		{
			if (!current.IsInPool && !current.Source.isPlaying && !current.IsPause && realtimeSinceStartup - current.StartTime > 0.1f)
			{
				UnityEngine.Object.Destroy(current.Source);
				linkedList.AddLast(current.KeyEffectId);
			}
		}
		foreach (int current2 in linkedList)
		{
			this.m_allEffectDict.Remove(current2);
		}
	}

	public static void RemoveAudioListener()
	{
		if (WJSound2D.s_instance != null)
		{
			WJSound2D.s_instance.DeleteAudioListener();
		}
	}

	private void DeleteAudioListener()
	{
		if (this.m_audioListener != null)
		{
			UnityEngine.Object.Destroy(this.m_audioListener);
			this.m_audioListener = null;
		}
	}

	public static void CreateAudioListener()
	{
		if (WJSound2D.s_instance != null)
		{
			WJSound2D.s_instance.InitAudioListener();
		}
	}

	private void InitAudioListener()
	{
		if (this.m_audioListener == null)
		{
			this.m_audioListener = base.gameObject.AddComponent<AudioListener>();
		}
	}
}
