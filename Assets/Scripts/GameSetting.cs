using System;
using UnityEngine;

public class GameSetting
{
	public static void SetConfig(string name, string value)
	{
		PlayerPrefs.SetString(name, value);
		PlayerPrefs.Save();
	}

	public static void SetConfig(string name, int value)
	{
		PlayerPrefs.SetInt(name, value);
		PlayerPrefs.Save();
	}

	public static void SetConfig(string name, float value)
	{
		PlayerPrefs.SetFloat(name, value);
		PlayerPrefs.Save();
	}

	public static string GetConfig(string name, string defaultValue = "0")
	{
		return PlayerPrefs.GetString(name, defaultValue);
	}

	public static int GetConfig_Int(string name, int defaultValue = 0)
	{
		return PlayerPrefs.GetInt(name, defaultValue);
	}

	public static float GetConfig_Float(string name, float defaultValue = 0f)
	{
		return PlayerPrefs.GetFloat(name, defaultValue);
	}
}
