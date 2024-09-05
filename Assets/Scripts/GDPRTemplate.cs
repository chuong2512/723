using System;
using System.Collections.Generic;
using System.Text;

public class GDPRTemplate
{
	protected static Dictionary<string, GDPRTemplate> msData = new Dictionary<string, GDPRTemplate>();

	protected static bool msIsInit = false;

	public string key;

	public string text;

	public static List<GDPRTemplate> Lis(params object[] keys)
	{
		GDPRTemplate.Dic();
		string text = string.Empty;
		for (int i = 0; i < keys.Length; i++)
		{
			object obj = keys[i];
			text = text + obj.ToString() + ":";
		}
		List<GDPRTemplate> list = new List<GDPRTemplate>();
		foreach (KeyValuePair<string, GDPRTemplate> current in GDPRTemplate.msData)
		{
			if ((current.Key.ToString() + ":").StartsWith(text))
			{
				list.Add(current.Value);
			}
		}
		return list;
	}

	public static Dictionary<string, GDPRTemplate> Dic()
	{
		if (!GDPRTemplate.msIsInit)
		{
			GDPRTemplate.msIsInit = true;
			GDPRTemplate gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1001";
			gDPRTemplate.text = "Make <color=#ff5590>Love</color> <color=#01cedb>Shots</color> better and stay free!";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1002";
			gDPRTemplate.text = "We hope you're excited to try out Love Balls. Before you get started though, our team wanted to let you know that upon getting your consent we're going to continue improving our game with your device data. Specifically, we will be using your device data to optimize the gameplay mechanics, application stability, and to show relevant ads.\nWe thank you for playing our game and helping us in anyway possible. If you're ever not interested in sharing your data, you can always re-adjust your settings at a later time as well.";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1003";
			gDPRTemplate.text = "Awesome! I support that :)";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1004";
			gDPRTemplate.text = "Manage Data Settings";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1005";
			gDPRTemplate.text = "How your data makes <color=#ff5590>Love</color> <color=#01cedb>Balls</color> better!";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1006";
			gDPRTemplate.text = "We're here to give you a break down on what sharing your data allows us to do:";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1007";
			gDPRTemplate.text = "Analyze your Love Balls experience, so we can figure out what to improve and build next.";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1008";
			gDPRTemplate.text = "Immediately identify and fix any annoying bugs or issues that appear. We're here to help you, just as long as you allow us to.";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1009";
			gDPRTemplate.text = "Identify ads to show that will win your interest over.";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1010";
			gDPRTemplate.text = "Next";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1011";
			gDPRTemplate.text = "Here's the controls below to adjust which data we can utilize:";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1012";
			gDPRTemplate.text = "<       Analytics & Support";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1013";
			gDPRTemplate.text = "AppsFlyer and Compass";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1014";
			gDPRTemplate.text = "<       Advertising";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1015";
			gDPRTemplate.text = "AppLovin, AdMob, and MoPub";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1016";
			gDPRTemplate.text = "Here's a link to our partener's privacy policy:";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1017";
			gDPRTemplate.text = "<a href=https://www.applovin.com/privacy/>https://www.applovin.com/privacy/</a>\n<a href=https://policies.google.com/privacy/>https://policies.google.com/privacy/</a>\n<a href=https://www.mopub.com/legal/privacy/>https://www.mopub.com/legal/privacy/</a>\n<a href=https://www.mopub.com/legal/partners/>https://www.mopub.com/legal/partners/</a>\n<a href=https://policy.supertapx.com/>https://policy.supertapx.com/</a>\n<a href=https://www.appsflyer.com/privacy-policy/>https://www.appsflyer.com/privacy-policy/</a>";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1018";
			gDPRTemplate.text = "Accept";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1019";
			gDPRTemplate.text = "Back";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1020";
			gDPRTemplate.text = "If you don't give us consent to use your data, you will be making our ability to support the application harder. We may not be able to discover what's causing issues within the game, and will even need to show more ads since our ad partners are paying us less.";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1021";
			gDPRTemplate.text = "Let me fix my settings";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1022";
			gDPRTemplate.text = "I understand.";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
			gDPRTemplate = new GDPRTemplate();
			gDPRTemplate.key = "@1023";
			gDPRTemplate.text = "You are limiting our ability to maintain the game. If you want to fix this issue, please tap here.";
			GDPRTemplate.msData.Add(gDPRTemplate.key, gDPRTemplate);
		}
		return GDPRTemplate.msData;
	}

	public static GDPRTemplate Tem(params object[] keys)
	{
		GDPRTemplate.Dic();
		StringBuilder stringBuilder = new StringBuilder(keys[0].ToString());
		if (keys.Length > 1)
		{
			for (int i = 1; i < keys.Length; i++)
			{
				stringBuilder.Append(":").Append(keys[i].ToString());
			}
		}
		GDPRTemplate result;
		if (GDPRTemplate.msData.TryGetValue(stringBuilder.ToString(), out result))
		{
			return result;
		}
		return null;
	}
}
