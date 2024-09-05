using System;
using System.Collections.Generic;
using System.Text;

public class ChapterTemplate
{
	protected static Dictionary<string, ChapterTemplate> msData = new Dictionary<string, ChapterTemplate>();

	protected static bool msIsInit = false;

	public string key;

	public string startLevel;

	public int num;

	public string name;

	public int lockStarNum;

	public string preChapter;

	public static List<ChapterTemplate> Lis(params object[] keys)
	{
		ChapterTemplate.Dic();
		string text = string.Empty;
		for (int i = 0; i < keys.Length; i++)
		{
			object obj = keys[i];
			text = text + obj.ToString() + ":";
		}
		List<ChapterTemplate> list = new List<ChapterTemplate>();
		foreach (KeyValuePair<string, ChapterTemplate> current in ChapterTemplate.msData)
		{
			if ((current.Key.ToString() + ":").StartsWith(text))
			{
				list.Add(current.Value);
			}
		}
		return list;
	}

	public static Dictionary<string, ChapterTemplate> Dic()
	{
		if (!ChapterTemplate.msIsInit)
		{
			ChapterTemplate.msIsInit = true;
			ChapterTemplate chapterTemplate = new ChapterTemplate();
			chapterTemplate.key = "1001";
			chapterTemplate.startLevel = "1001";
			chapterTemplate.num = 10;
			chapterTemplate.name = "CHAPTER 1";
			chapterTemplate.lockStarNum = 0;
			chapterTemplate.preChapter = string.Empty;
			ChapterTemplate.msData.Add(chapterTemplate.key, chapterTemplate);
			chapterTemplate = new ChapterTemplate();
			chapterTemplate.key = "1002";
			chapterTemplate.startLevel = "1011";
			chapterTemplate.num = 10;
			chapterTemplate.name = "CHAPTER 2";
			chapterTemplate.lockStarNum = 20;
			chapterTemplate.preChapter = "1001";
			ChapterTemplate.msData.Add(chapterTemplate.key, chapterTemplate);
		}
		return ChapterTemplate.msData;
	}

	public static ChapterTemplate Tem(params object[] keys)
	{
		ChapterTemplate.Dic();
		StringBuilder stringBuilder = new StringBuilder(keys[0].ToString());
		if (keys.Length > 1)
		{
			for (int i = 1; i < keys.Length; i++)
			{
				stringBuilder.Append(":").Append(keys[i].ToString());
			}
		}
		ChapterTemplate result;
		if (ChapterTemplate.msData.TryGetValue(stringBuilder.ToString(), out result))
		{
			return result;
		}
		return null;
	}
}
