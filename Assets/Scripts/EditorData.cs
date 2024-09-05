using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EditorData
{
	public const string JSON_FILE_NAME = "OfficialJson";

	public const string COIN_JSON_FILE_NAME = "CoinLevelJson";

	public const string EXTEND = ".bytes";

	private const string KEY_LIST = "EditorKeyList";

	private const string KEY_Name = "EditorKey_{0}";

	private static string EditorJsonPath = Application.persistentDataPath + "/Json";

	public static string GetPath(bool isSigle = true)
	{
		string str = (!isSigle) ? string.Empty : "/Sigle";
		if (EditorScene.Inst == null)
		{
			return EditorData.EditorJsonPath + str;
		}
		if (string.IsNullOrEmpty(EditorScene.Inst.levelFilePath))
		{
			return EditorData.EditorJsonPath + str;
		}
		return EditorScene.Inst.levelFilePath;
	}

	public static string GetSaveTexturePath()
	{
		string str = "/LevelTexture";
		if (EditorScene.Inst == null)
		{
			return EditorData.EditorJsonPath + str;
		}
		if (string.IsNullOrEmpty(EditorScene.Inst.levelFilePath))
		{
			return EditorData.EditorJsonPath + str;
		}
		return EditorScene.Inst.levelFilePath + str;
	}

	public static void SaveJsonData(string key, string json)
	{
		if (key == null || json == null)
		{
			return;
		}
		if (!Directory.Exists(EditorData.GetPath(true)))
		{
			Directory.CreateDirectory(EditorData.GetPath(true));
		}
		string text = EditorData.GetPath(true) + "/" + key + ".json";
		File.WriteAllText(text, json);
		UnityEngine.Debug.Log("<color=green>保存成功！</color>" + text);
	}

	public static string ReadJsonData(string key)
	{
		string text = EditorData.GetPath(true) + "/" + key + ".json";
		if (!File.Exists(text))
		{
			UnityEngine.Debug.Log("<color=red>备份文件不存在:</color>" + text);
			return null;
		}
		return File.ReadAllText(text);
	}

	public static List<string> LoadListData(int startlevel, int endLevel)
	{
		string path = EditorData.GetPath(false) + "/OfficialJson";
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		List<string> list = new List<string>();
		string empty = string.Empty;
		for (int i = startlevel; i <= endLevel; i++)
		{
			path = string.Concat(new object[]
			{
				EditorData.GetPath(true),
				"/",
				i,
				".json"
			});
			if (!File.Exists(path))
			{
				UnityEngine.Debug.Log("<color=red>备份文件不存在:</color>" + i);
				list.Add(string.Empty);
			}
			else
			{
				list.Add(File.ReadAllText(path));
			}
		}
		return list;
	}

	public static void SaveOfficialData(int endlevel)
	{
		string text = EditorData.GetPath(false) + "/OfficialJson";
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string text2 = text + "/OfficialJson" + DateTime.Now.ToString("yyyyMMddHHmm") + ".bytes";
		int num = int.Parse(SceneSetting.inst.FristLevelId);
		FileStream fileStream = new FileStream(text2, FileMode.Create);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		binaryWriter.Write(endlevel - num + 1);
		string text3 = string.Empty;
		for (int i = num; i <= endlevel; i++)
		{
			text = string.Concat(new object[]
			{
				EditorData.GetPath(true),
				"/",
				i,
				".json"
			});
			if (!File.Exists(text))
			{
				UnityEngine.Debug.Log("<color=red>备份文件不存在:</color>" + i);
			}
			else
			{
				text3 = text3 + i + ",";
				binaryWriter.Write(i.ToString());
				binaryWriter.Write(File.ReadAllText(text));
			}
		}
		binaryWriter.Close();
		fileStream.Close();
		if (!string.IsNullOrEmpty(text3))
		{
			UnityEngine.Debug.Log("<color=green>导出数据成功</color>" + text2 + " 导出关卡：" + text3);
		}
	}
}
