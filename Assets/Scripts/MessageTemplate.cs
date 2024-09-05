using LIBII;
using System;
using System.Collections.Generic;
using System.Text;

public class MessageTemplate
{
	protected static Dictionary<string, MessageTemplate> msData = new Dictionary<string, MessageTemplate>();

	protected static bool msIsInit = false;

	public string key;

	public string Text;

	public static List<MessageTemplate> Lis(params object[] keys)
	{
		MessageTemplate.Dic();
		string text = string.Empty;
		for (int i = 0; i < keys.Length; i++)
		{
			object obj = keys[i];
			text = text + obj.ToString() + ":";
		}
		List<MessageTemplate> list = new List<MessageTemplate>();
		foreach (KeyValuePair<string, MessageTemplate> current in MessageTemplate.msData)
		{
			if ((current.Key.ToString() + ":").StartsWith(text))
			{
				list.Add(current.Value);
			}
		}
		return list;
	}

	public static Dictionary<string, MessageTemplate> Dic()
	{
		if (!MessageTemplate.msIsInit)
		{
			MessageTemplate.msIsInit = true;
			string a = IScriptableObjectReader<SettingReader, SettingAsset>.ScriptableObject.Language.ToString().ToLower();
			if (a == "chinese")
			{
				MessageTemplate messageTemplate = new MessageTemplate();
				messageTemplate.key = "notify:nextday";
				messageTemplate.Text = "\ud83d\udc9e你的朋友想念你啦!";
				MessageTemplate.msData.Add(messageTemplate.key, messageTemplate);
				messageTemplate = new MessageTemplate();
				messageTemplate.key = "notify:thirdday";
				messageTemplate.Text = "\ud83d\udc96今天有特殊的礼物哟\ud83c\udf81, 快来领取吧！";
				MessageTemplate.msData.Add(messageTemplate.key, messageTemplate);
				messageTemplate = new MessageTemplate();
				messageTemplate.key = "notify:fourthday";
				messageTemplate.Text = "\ud83d\udc96嗨，朋友! 快来玩儿两把啦!\ud83d\udc96";
				MessageTemplate.msData.Add(messageTemplate.key, messageTemplate);
			}
			else
			{
				MessageTemplate messageTemplate = new MessageTemplate();
				messageTemplate.key = "notify:nextday";
				messageTemplate.Text = "\ud83d\udc9eTime to challenge! There're still many interesting levels you didn't try, let's start now!\ud83d\ude01|||A brand new day with brand new challenges! What are you waiting for?";
				MessageTemplate.msData.Add(messageTemplate.key, messageTemplate);
				messageTemplate = new MessageTemplate();
				messageTemplate.key = "notify:thirdday";
				messageTemplate.Text = "\ud83d\udc96Today’s special gift\ud83c\udf81, come to claim it.|||\ud83d\ude01 Splash The Duck Time! Come and play!\ud83d\ude01";
				MessageTemplate.msData.Add(messageTemplate.key, messageTemplate);
				messageTemplate = new MessageTemplate();
				messageTemplate.key = "notify:fourthday";
				messageTemplate.Text = "\ud83d\ude01Hey mate! Come have some fun now!\ud83d\udc96|||\ud83d\ude01What a busy day! Come on play and relax now!";
				MessageTemplate.msData.Add(messageTemplate.key, messageTemplate);
			}
		}
		return MessageTemplate.msData;
	}

	public static MessageTemplate Tem(params object[] keys)
	{
		MessageTemplate.Dic();
		StringBuilder stringBuilder = new StringBuilder(keys[0].ToString());
		if (keys.Length > 1)
		{
			for (int i = 1; i < keys.Length; i++)
			{
				stringBuilder.Append(":").Append(keys[i].ToString());
			}
		}
		MessageTemplate result;
		if (MessageTemplate.msData.TryGetValue(stringBuilder.ToString(), out result))
		{
			return result;
		}
		return null;
	}
}
