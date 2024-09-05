using System;
using System.Collections.Generic;
using System.Text;

public class StoreTemplate
{
	protected static Dictionary<string, StoreTemplate> msData = new Dictionary<string, StoreTemplate>();

	protected static bool msIsInit = false;

	public string key;

	public string IapId;

	public int GetCoin;

	public bool Noads;

	public string Skin;

	public string PriceStr;

	public static List<StoreTemplate> Lis(params object[] keys)
	{
		StoreTemplate.Dic();
		string text = string.Empty;
		for (int i = 0; i < keys.Length; i++)
		{
			object obj = keys[i];
			text = text + obj.ToString() + ":";
		}
		List<StoreTemplate> list = new List<StoreTemplate>();
		foreach (KeyValuePair<string, StoreTemplate> current in StoreTemplate.msData)
		{
			if ((current.Key.ToString() + ":").StartsWith(text))
			{
				list.Add(current.Value);
			}
		}
		return list;
	}

	public static Dictionary<string, StoreTemplate> Dic()
	{
		if (!StoreTemplate.msIsInit)
		{
			StoreTemplate.msIsInit = true;
			StoreTemplate storeTemplate = new StoreTemplate();
			storeTemplate.key = "1800c";
			storeTemplate.IapId = "st20180904ls1800c";
			storeTemplate.GetCoin = 1800;
			storeTemplate.Noads = false;
			storeTemplate.Skin = string.Empty;
			storeTemplate.PriceStr = "$0.99";
			StoreTemplate.msData.Add(storeTemplate.key, storeTemplate);
			storeTemplate = new StoreTemplate();
			storeTemplate.key = "10000c";
			storeTemplate.IapId = "st20180904ls10000c";
			storeTemplate.GetCoin = 10000;
			storeTemplate.Noads = false;
			storeTemplate.Skin = string.Empty;
			storeTemplate.PriceStr = "$4.99";
			StoreTemplate.msData.Add(storeTemplate.key, storeTemplate);
			storeTemplate = new StoreTemplate();
			storeTemplate.key = "22000c";
			storeTemplate.IapId = "st20180904ls22000c";
			storeTemplate.GetCoin = 22000;
			storeTemplate.Noads = false;
			storeTemplate.Skin = string.Empty;
			storeTemplate.PriceStr = "$9.99";
			StoreTemplate.msData.Add(storeTemplate.key, storeTemplate);
			storeTemplate = new StoreTemplate();
			storeTemplate.key = "48000c";
			storeTemplate.IapId = "st20180904ls48000c";
			storeTemplate.GetCoin = 48000;
			storeTemplate.Noads = false;
			storeTemplate.Skin = string.Empty;
			storeTemplate.PriceStr = "$19.99";
			StoreTemplate.msData.Add(storeTemplate.key, storeTemplate);
			storeTemplate = new StoreTemplate();
			storeTemplate.key = "package";
			storeTemplate.IapId = "st20180904lspackage";
			storeTemplate.GetCoin = 10000;
			storeTemplate.Noads = true;
			storeTemplate.Skin = "2018";
			storeTemplate.PriceStr = "$9.99+$4.99";
			StoreTemplate.msData.Add(storeTemplate.key, storeTemplate);
		}
		return StoreTemplate.msData;
	}

	public static StoreTemplate Tem(params object[] keys)
	{
		StoreTemplate.Dic();
		StringBuilder stringBuilder = new StringBuilder(keys[0].ToString());
		if (keys.Length > 1)
		{
			for (int i = 1; i < keys.Length; i++)
			{
				stringBuilder.Append(":").Append(keys[i].ToString());
			}
		}
		StoreTemplate result;
		if (StoreTemplate.msData.TryGetValue(stringBuilder.ToString(), out result))
		{
			return result;
		}
		return null;
	}
}
