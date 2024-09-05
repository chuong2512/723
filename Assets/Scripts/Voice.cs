using System;

public class Voice
{
	private const string voice = "GameSetting_voice";

	private const string music = "GameSetting_music";

	private static string BgMusicName = "BGM:bg";

	private static bool IsPlayingBackMusic;

	private static bool IsPauseBackMusic;

	public static bool GetVoice()
	{
		return GameSetting.GetConfig("GameSetting_voice", "1") == "1";
	}

	public static void SetVoice(bool able)
	{
		GameSetting.SetConfig("GameSetting_voice", (!able) ? "0" : "1");
	}

	public static bool GetMusic()
	{
		return GameSetting.GetConfig("GameSetting_music", "1") == "1";
	}

	public static void SetMusic(bool able, bool DoMusic = true)
	{
		GameSetting.SetConfig("GameSetting_music", (!able) ? "0" : "1");
		if (able)
		{
			if (DoMusic)
			{
				Voice.PlayBgMusic();
			}
		}
		else if (DoMusic)
		{
			Voice.PauseBackMusic();
		}
	}

	public static void PlayBgMusic()
	{
		if (!Voice.GetMusic())
		{
			return;
		}
		if (!Voice.IsPlayingBackMusic)
		{
			Common.PlayBackground(Voice.BgMusicName);
			Voice.IsPlayingBackMusic = true;
		}
		else if (Voice.IsPauseBackMusic)
		{
			WJSound2D.ResumeBackgroundMusic();
			Voice.IsPauseBackMusic = false;
		}
	}

	public static void PlayNewBgMusic(string musicName)
	{
		if (!Voice.GetVoice())
		{
			return;
		}
		Voice.BgMusicName = musicName;
		Voice.IsPlayingBackMusic = true;
		Voice.IsPauseBackMusic = false;
		Common.PlayBackground(musicName);
	}

	public static void PauseBackMusic()
	{
		Voice.IsPauseBackMusic = true;
		WJSound2D.PauseBackgroundMusic();
	}
}
