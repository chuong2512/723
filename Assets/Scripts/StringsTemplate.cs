using LIBII;
using System;
using System.Collections.Generic;
using System.Text;

public class StringsTemplate
{
	protected static Dictionary<string, StringsTemplate> msData = new Dictionary<string, StringsTemplate>();

	protected static bool msIsInit = false;

	public string key;

	public string text;

	public static List<StringsTemplate> Lis(params object[] keys)
	{
		StringsTemplate.Dic();
		string text = string.Empty;
		for (int i = 0; i < keys.Length; i++)
		{
			object obj = keys[i];
			text = text + obj.ToString() + ":";
		}
		List<StringsTemplate> list = new List<StringsTemplate>();
		foreach (KeyValuePair<string, StringsTemplate> current in StringsTemplate.msData)
		{
			if ((current.Key.ToString() + ":").StartsWith(text))
			{
				list.Add(current.Value);
			}
		}
		return list;
	}

	public static Dictionary<string, StringsTemplate> Dic()
	{
		if (!StringsTemplate.msIsInit)
		{
			StringsTemplate.msIsInit = true;
			string a = IScriptableObjectReader<SettingReader, SettingAsset>.ScriptableObject.Language.ToString().ToLower();
			if (a == "chinese")
			{
				StringsTemplate stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@level";
				stringsTemplate.text = "LEVEL {0}";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@NotEnoughMoney";
				stringsTemplate.text = "Not Enough Money";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@InternetLost";
				stringsTemplate.text = "Internet connection lost, please check it out and try again.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@NotReady";
				stringsTemplate.text = "Ads are not ready yet. Please try again in a few seconds.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@levelLock";
				stringsTemplate.text = "Pass The Previous Level To Unlock!";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@finish";
				stringsTemplate.text = "FINISH";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "watchvideo";
				stringsTemplate.text = "Watch Video{0}/{1}";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@TurnTable";
				stringsTemplate.text = "From TurnTable";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_1";
				stringsTemplate.text = "Tap the screen to release the water \nand splash the duck into the tub.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_2";
				stringsTemplate.text = "Try to splash the ball.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_3";
				stringsTemplate.text = "The portal can transfer the duck \nto another portal.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_4";
				stringsTemplate.text = "Try breaking the balance of the seesaw.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_5";
				stringsTemplate.text = "The duck and water can be blowed away.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_6";
				stringsTemplate.text = "Such blocks will drop.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_7";
				stringsTemplate.text = "Try pricking the balloons.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_8";
				stringsTemplate.text = "The rotating plate will spin around the axis.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_9";
				stringsTemplate.text = "The elastic board \ncan bounce objects.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_10";
				stringsTemplate.text = "Pay attention to the \ndirection of the conveyor.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_11";
				stringsTemplate.text = "Yes. It's a catapult.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@ChapterLock";
				stringsTemplate.text = "Not enough money";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@Collect3star";
				stringsTemplate.text = "Collect all 3 stars";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@NoThanks";
				stringsTemplate.text = "NO THANKS";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@TurntableStart";
				stringsTemplate.text = "SPIN";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@MoneyNotEnough";
				stringsTemplate.text = "Not enough money";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@InSpin";
				stringsTemplate.text = "IN SPIN";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@Unlocked";
				stringsTemplate.text = "UNLOCKED";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@InUse";
				stringsTemplate.text = "IN USE";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@GotIt";
				stringsTemplate.text = "GOT IT";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@UnlockChapter_1";
				stringsTemplate.text = "You should collect";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@UnlockChapter_2";
				stringsTemplate.text = "to unlock the chapter.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@GameQuit";
				stringsTemplate.text = "Are you sure to quit?";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@Cancel";
				stringsTemplate.text = "Cancel";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@Ok";
				stringsTemplate.text = "Ok";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
			}
			else
			{
				StringsTemplate stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@level";
				stringsTemplate.text = "LEVEL {0}";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@NotEnoughMoney";
				stringsTemplate.text = "Not Enough Money";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@InternetLost";
				stringsTemplate.text = "Internet connection lost, please check it out and try again.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@NotReady";
				stringsTemplate.text = "Ads are not ready yet. Please try again in a few seconds.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@levelLock";
				stringsTemplate.text = "Pass The Previous Level To Unlock!";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@finish";
				stringsTemplate.text = "FINISH";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "watchvideo";
				stringsTemplate.text = "Watch Video{0}/{1}";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@TurnTable";
				stringsTemplate.text = "From TurnTable";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_1";
				stringsTemplate.text = "Tap the screen to release the water \nand splash the duck into the tub.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_2";
				stringsTemplate.text = "Try to splash the ball.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_3";
				stringsTemplate.text = "The portal can transfer the duck \nto another portal.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_4";
				stringsTemplate.text = "Try breaking the balance of the seesaw.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_5";
				stringsTemplate.text = "The duck and water can be blowed away.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_6";
				stringsTemplate.text = "Such blocks will drop.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_7";
				stringsTemplate.text = "Try pricking the balloons.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_8";
				stringsTemplate.text = "The rotating plate will spin around the axis.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_9";
				stringsTemplate.text = "The elastic board \ncan bounce objects.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_10";
				stringsTemplate.text = "Pay attention to the \ndirection of the conveyor.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "tutorial_11";
				stringsTemplate.text = "Yes. It's a catapult.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@ChapterLock";
				stringsTemplate.text = "Not enough money";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@Collect3star";
				stringsTemplate.text = "Collect all 3 stars";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@NoThanks";
				stringsTemplate.text = "NO THANKS";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@TurntableStart";
				stringsTemplate.text = "SPIN";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@MoneyNotEnough";
				stringsTemplate.text = "Not enough money";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@InSpin";
				stringsTemplate.text = "IN SPIN";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@Unlocked";
				stringsTemplate.text = "UNLOCKED";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@InUse";
				stringsTemplate.text = "IN USE";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@GotIt";
				stringsTemplate.text = "GOT IT";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@UnlockChapter_1";
				stringsTemplate.text = "You should collect";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@UnlockChapter_2";
				stringsTemplate.text = "to unlock the chapter.";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@GameQuit";
				stringsTemplate.text = "Are you sure to quit?";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@Cancel";
				stringsTemplate.text = "Cancel";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
				stringsTemplate = new StringsTemplate();
				stringsTemplate.key = "@Ok";
				stringsTemplate.text = "Ok";
				StringsTemplate.msData.Add(stringsTemplate.key, stringsTemplate);
			}
		}
		return StringsTemplate.msData;
	}

	public static StringsTemplate Tem(params object[] keys)
	{
		StringsTemplate.Dic();
		StringBuilder stringBuilder = new StringBuilder(keys[0].ToString());
		if (keys.Length > 1)
		{
			for (int i = 1; i < keys.Length; i++)
			{
				stringBuilder.Append(":").Append(keys[i].ToString());
			}
		}
		StringsTemplate result;
		if (StringsTemplate.msData.TryGetValue(stringBuilder.ToString(), out result))
		{
			return result;
		}
		return null;
	}
}
