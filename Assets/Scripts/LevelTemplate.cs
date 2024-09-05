using System;
using System.Collections.Generic;
using System.Text;

public class LevelTemplate
{
	protected static Dictionary<string, LevelTemplate> msData = new Dictionary<string, LevelTemplate>();

	protected static bool msIsInit = false;

	public string key;

	public string name;

	public string chapterId;

	public int index;

	public float totalTime;

	public string starWaterCount;

	public static List<LevelTemplate> Lis(params object[] keys)
	{
		LevelTemplate.Dic();
		string text = string.Empty;
		for (int i = 0; i < keys.Length; i++)
		{
			object obj = keys[i];
			text = text + obj.ToString() + ":";
		}
		List<LevelTemplate> list = new List<LevelTemplate>();
		foreach (KeyValuePair<string, LevelTemplate> current in LevelTemplate.msData)
		{
			if ((current.Key.ToString() + ":").StartsWith(text))
			{
				list.Add(current.Value);
			}
		}
		return list;
	}

	public static Dictionary<string, LevelTemplate> Dic()
	{
		if (!LevelTemplate.msIsInit)
		{
			LevelTemplate.msIsInit = true;
			LevelTemplate levelTemplate = new LevelTemplate();
			levelTemplate.key = "1001";
			levelTemplate.name = "loveshot1";
			levelTemplate.chapterId = "1001";
			levelTemplate.index = 1;
			levelTemplate.totalTime = 100f;
			levelTemplate.starWaterCount = "600+500+400";
			LevelTemplate.msData.Add(levelTemplate.key, levelTemplate);
			levelTemplate = new LevelTemplate();
			levelTemplate.key = "1002";
			levelTemplate.name = "loveshot2";
			levelTemplate.chapterId = "1001";
			levelTemplate.index = 2;
			levelTemplate.totalTime = 100f;
			levelTemplate.starWaterCount = "600+500+400";
			LevelTemplate.msData.Add(levelTemplate.key, levelTemplate);
			levelTemplate = new LevelTemplate();
			levelTemplate.key = "1003";
			levelTemplate.name = "loveshot3";
			levelTemplate.chapterId = "1001";
			levelTemplate.index = 3;
			levelTemplate.totalTime = 100f;
			levelTemplate.starWaterCount = "600+500+400";
			LevelTemplate.msData.Add(levelTemplate.key, levelTemplate);
			levelTemplate = new LevelTemplate();
			levelTemplate.key = "1004";
			levelTemplate.name = "loveshot4";
			levelTemplate.chapterId = "1001";
			levelTemplate.index = 4;
			levelTemplate.totalTime = 100f;
			levelTemplate.starWaterCount = "500+400+300";
			LevelTemplate.msData.Add(levelTemplate.key, levelTemplate);
			levelTemplate = new LevelTemplate();
			levelTemplate.key = "1005";
			levelTemplate.name = "loveshot5";
			levelTemplate.chapterId = "1001";
			levelTemplate.index = 5;
			levelTemplate.totalTime = 100f;
			levelTemplate.starWaterCount = "400+300+200";
			LevelTemplate.msData.Add(levelTemplate.key, levelTemplate);
			levelTemplate = new LevelTemplate();
			levelTemplate.key = "1006";
			levelTemplate.name = "loveshot6";
			levelTemplate.chapterId = "1001";
			levelTemplate.index = 6;
			levelTemplate.totalTime = 100f;
			levelTemplate.starWaterCount = "400+300+200";
			LevelTemplate.msData.Add(levelTemplate.key, levelTemplate);
			levelTemplate = new LevelTemplate();
			levelTemplate.key = "1007";
			levelTemplate.name = "loveshot7";
			levelTemplate.chapterId = "1001";
			levelTemplate.index = 7;
			levelTemplate.totalTime = 100f;
			levelTemplate.starWaterCount = "400+300+200";
			LevelTemplate.msData.Add(levelTemplate.key, levelTemplate);
			levelTemplate = new LevelTemplate();
			levelTemplate.key = "1008";
			levelTemplate.name = "loveshot8";
			levelTemplate.chapterId = "1001";
			levelTemplate.index = 8;
			levelTemplate.totalTime = 100f;
			levelTemplate.starWaterCount = "400+300+200";
			LevelTemplate.msData.Add(levelTemplate.key, levelTemplate);
			levelTemplate = new LevelTemplate();
			levelTemplate.key = "1009";
			levelTemplate.name = "loveshot9";
			levelTemplate.chapterId = "1001";
			levelTemplate.index = 9;
			levelTemplate.totalTime = 100f;
			levelTemplate.starWaterCount = "400+300+200";
			LevelTemplate.msData.Add(levelTemplate.key, levelTemplate);
			levelTemplate = new LevelTemplate();
			levelTemplate.key = "1010";
			levelTemplate.name = "loveshot10";
			levelTemplate.chapterId = "1001";
			levelTemplate.index = 10;
			levelTemplate.totalTime = 100f;
			levelTemplate.starWaterCount = "250+200+150";
			LevelTemplate.msData.Add(levelTemplate.key, levelTemplate);
			levelTemplate = new LevelTemplate();
			levelTemplate.key = "1011";
			levelTemplate.name = "loveshot11";
			levelTemplate.chapterId = "1002";
			levelTemplate.index = 11;
			levelTemplate.totalTime = 100f;
			levelTemplate.starWaterCount = "250+200+150";
			LevelTemplate.msData.Add(levelTemplate.key, levelTemplate);
			levelTemplate = new LevelTemplate();
			levelTemplate.key = "1012";
			levelTemplate.name = "loveshot12";
			levelTemplate.chapterId = "1002";
			levelTemplate.index = 12;
			levelTemplate.totalTime = 100f;
			levelTemplate.starWaterCount = "200+150+100";
			LevelTemplate.msData.Add(levelTemplate.key, levelTemplate);
			levelTemplate = new LevelTemplate();
			levelTemplate.key = "1013";
			levelTemplate.name = "loveshot13";
			levelTemplate.chapterId = "1002";
			levelTemplate.index = 13;
			levelTemplate.totalTime = 100f;
			levelTemplate.starWaterCount = "200+150+100";
			LevelTemplate.msData.Add(levelTemplate.key, levelTemplate);
			levelTemplate = new LevelTemplate();
			levelTemplate.key = "1014";
			levelTemplate.name = "loveshot14";
			levelTemplate.chapterId = "1002";
			levelTemplate.index = 14;
			levelTemplate.totalTime = 100f;
			levelTemplate.starWaterCount = "200+150+100";
			LevelTemplate.msData.Add(levelTemplate.key, levelTemplate);
			levelTemplate = new LevelTemplate();
			levelTemplate.key = "1015";
			levelTemplate.name = "loveshot15";
			levelTemplate.chapterId = "1002";
			levelTemplate.index = 15;
			levelTemplate.totalTime = 100f;
			levelTemplate.starWaterCount = "200+150+100";
			LevelTemplate.msData.Add(levelTemplate.key, levelTemplate);
			levelTemplate = new LevelTemplate();
			levelTemplate.key = "1016";
			levelTemplate.name = "loveshot16";
			levelTemplate.chapterId = "1002";
			levelTemplate.index = 16;
			levelTemplate.totalTime = 100f;
			levelTemplate.starWaterCount = "200+150+100";
			LevelTemplate.msData.Add(levelTemplate.key, levelTemplate);
			levelTemplate = new LevelTemplate();
			levelTemplate.key = "1017";
			levelTemplate.name = "loveshot17";
			levelTemplate.chapterId = "1002";
			levelTemplate.index = 17;
			levelTemplate.totalTime = 100f;
			levelTemplate.starWaterCount = "200+150+100";
			LevelTemplate.msData.Add(levelTemplate.key, levelTemplate);
			levelTemplate = new LevelTemplate();
			levelTemplate.key = "1018";
			levelTemplate.name = "loveshot18";
			levelTemplate.chapterId = "1002";
			levelTemplate.index = 18;
			levelTemplate.totalTime = 100f;
			levelTemplate.starWaterCount = "200+150+100";
			LevelTemplate.msData.Add(levelTemplate.key, levelTemplate);
			levelTemplate = new LevelTemplate();
			levelTemplate.key = "1019";
			levelTemplate.name = "loveshot19";
			levelTemplate.chapterId = "1002";
			levelTemplate.index = 19;
			levelTemplate.totalTime = 100f;
			levelTemplate.starWaterCount = "200+150+100";
			LevelTemplate.msData.Add(levelTemplate.key, levelTemplate);
			levelTemplate = new LevelTemplate();
			levelTemplate.key = "1020";
			levelTemplate.name = "loveshot20";
			levelTemplate.chapterId = "1002";
			levelTemplate.index = 20;
			levelTemplate.totalTime = 100f;
			levelTemplate.starWaterCount = "200+150+100";
			LevelTemplate.msData.Add(levelTemplate.key, levelTemplate);
		}
		return LevelTemplate.msData;
	}

	public static LevelTemplate Tem(params object[] keys)
	{
		LevelTemplate.Dic();
		StringBuilder stringBuilder = new StringBuilder(keys[0].ToString());
		if (keys.Length > 1)
		{
			for (int i = 1; i < keys.Length; i++)
			{
				stringBuilder.Append(":").Append(keys[i].ToString());
			}
		}
		LevelTemplate result;
		if (LevelTemplate.msData.TryGetValue(stringBuilder.ToString(), out result))
		{
			return result;
		}
		return null;
	}
}
