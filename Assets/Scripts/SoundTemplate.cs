using System;
using System.Collections.Generic;
using System.Text;

public class SoundTemplate
{
	protected static Dictionary<string, SoundTemplate> msData = new Dictionary<string, SoundTemplate>();

	protected static bool msIsInit = false;

	public string key;

	public string MutexSoundID;

	public string MutexAndStopSoundID;

	public string Name;

	public float Delay;

	public bool IsLoop;

	public float Volume;

	public static List<SoundTemplate> Lis(params object[] keys)
	{
		SoundTemplate.Dic();
		string text = string.Empty;
		for (int i = 0; i < keys.Length; i++)
		{
			object obj = keys[i];
			text = text + obj.ToString() + ":";
		}
		List<SoundTemplate> list = new List<SoundTemplate>();
		foreach (KeyValuePair<string, SoundTemplate> current in SoundTemplate.msData)
		{
			if ((current.Key.ToString() + ":").StartsWith(text))
			{
				list.Add(current.Value);
			}
		}
		return list;
	}

	public static Dictionary<string, SoundTemplate> Dic()
	{
		if (!SoundTemplate.msIsInit)
		{
			SoundTemplate.msIsInit = true;
			SoundTemplate soundTemplate = new SoundTemplate();
			soundTemplate.key = "Common:button";
			soundTemplate.MutexSoundID = string.Empty;
			soundTemplate.MutexAndStopSoundID = string.Empty;
			soundTemplate.Name = "button";
			soundTemplate.Delay = 0f;
			soundTemplate.IsLoop = false;
			soundTemplate.Volume = 1f;
			SoundTemplate.msData.Add(soundTemplate.key, soundTemplate);
			soundTemplate = new SoundTemplate();
			soundTemplate.key = "BGM:bg";
			soundTemplate.MutexSoundID = string.Empty;
			soundTemplate.MutexAndStopSoundID = string.Empty;
			soundTemplate.Name = "bg";
			soundTemplate.Delay = 0f;
			soundTemplate.IsLoop = true;
			soundTemplate.Volume = 1f;
			SoundTemplate.msData.Add(soundTemplate.key, soundTemplate);
			soundTemplate = new SoundTemplate();
			soundTemplate.key = "SFX:portal";
			soundTemplate.MutexSoundID = string.Empty;
			soundTemplate.MutexAndStopSoundID = string.Empty;
			soundTemplate.Name = "portal";
			soundTemplate.Delay = 0f;
			soundTemplate.IsLoop = false;
			soundTemplate.Volume = 1f;
			SoundTemplate.msData.Add(soundTemplate.key, soundTemplate);
			soundTemplate = new SoundTemplate();
			soundTemplate.key = "SFX:star";
			soundTemplate.MutexSoundID = string.Empty;
			soundTemplate.MutexAndStopSoundID = string.Empty;
			soundTemplate.Name = "star";
			soundTemplate.Delay = 0f;
			soundTemplate.IsLoop = false;
			soundTemplate.Volume = 1f;
			SoundTemplate.msData.Add(soundTemplate.key, soundTemplate);
			soundTemplate = new SoundTemplate();
			soundTemplate.key = "SFX:intowater";
			soundTemplate.MutexSoundID = string.Empty;
			soundTemplate.MutexAndStopSoundID = string.Empty;
			soundTemplate.Name = "intowater";
			soundTemplate.Delay = 0f;
			soundTemplate.IsLoop = false;
			soundTemplate.Volume = 1f;
			SoundTemplate.msData.Add(soundTemplate.key, soundTemplate);
			soundTemplate = new SoundTemplate();
			soundTemplate.key = "SFX:draw";
			soundTemplate.MutexSoundID = string.Empty;
			soundTemplate.MutexAndStopSoundID = string.Empty;
			soundTemplate.Name = "draw";
			soundTemplate.Delay = 0f;
			soundTemplate.IsLoop = true;
			soundTemplate.Volume = 1f;
			SoundTemplate.msData.Add(soundTemplate.key, soundTemplate);
			soundTemplate = new SoundTemplate();
			soundTemplate.key = "SFX:win";
			soundTemplate.MutexSoundID = string.Empty;
			soundTemplate.MutexAndStopSoundID = string.Empty;
			soundTemplate.Name = "win";
			soundTemplate.Delay = 0f;
			soundTemplate.IsLoop = false;
			soundTemplate.Volume = 1f;
			SoundTemplate.msData.Add(soundTemplate.key, soundTemplate);
			soundTemplate = new SoundTemplate();
			soundTemplate.key = "SFX:ga";
			soundTemplate.MutexSoundID = string.Empty;
			soundTemplate.MutexAndStopSoundID = string.Empty;
			soundTemplate.Name = "ga";
			soundTemplate.Delay = 0f;
			soundTemplate.IsLoop = false;
			soundTemplate.Volume = 1f;
			SoundTemplate.msData.Add(soundTemplate.key, soundTemplate);
		}
		return SoundTemplate.msData;
	}

	public static SoundTemplate Tem(params object[] keys)
	{
		SoundTemplate.Dic();
		StringBuilder stringBuilder = new StringBuilder(keys[0].ToString());
		if (keys.Length > 1)
		{
			for (int i = 1; i < keys.Length; i++)
			{
				stringBuilder.Append(":").Append(keys[i].ToString());
			}
		}
		SoundTemplate result;
		if (SoundTemplate.msData.TryGetValue(stringBuilder.ToString(), out result))
		{
			return result;
		}
		return null;
	}
}
