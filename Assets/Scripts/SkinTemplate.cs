using System;
using System.Collections.Generic;
using System.Text;

public class SkinTemplate
{
	protected static Dictionary<string, SkinTemplate> msData = new Dictionary<string, SkinTemplate>();

	protected static bool msIsInit = false;

	public string key;

	public string aniIcon;

	public string icon;

	public string price;

	public int obtainWay;

	public string yanPartical;

	public string bodyPartical;

	public static List<SkinTemplate> Lis(params object[] keys)
	{
		SkinTemplate.Dic();
		string text = string.Empty;
		for (int i = 0; i < keys.Length; i++)
		{
			object obj = keys[i];
			text = text + obj.ToString() + ":";
		}
		List<SkinTemplate> list = new List<SkinTemplate>();
		foreach (KeyValuePair<string, SkinTemplate> current in SkinTemplate.msData)
		{
			if ((current.Key.ToString() + ":").StartsWith(text))
			{
				list.Add(current.Value);
			}
		}
		return list;
	}

	public static Dictionary<string, SkinTemplate> Dic()
	{
		if (!SkinTemplate.msIsInit)
		{
			SkinTemplate.msIsInit = true;
		}
		return SkinTemplate.msData;
	}

	public static SkinTemplate Tem(params object[] keys)
	{
		SkinTemplate.Dic();
		StringBuilder stringBuilder = new StringBuilder(keys[0].ToString());
		if (keys.Length > 1)
		{
			for (int i = 1; i < keys.Length; i++)
			{
				stringBuilder.Append(":").Append(keys[i].ToString());
			}
		}
		SkinTemplate result;
		if (SkinTemplate.msData.TryGetValue(stringBuilder.ToString(), out result))
		{
			return result;
		}
		return null;
	}
}
