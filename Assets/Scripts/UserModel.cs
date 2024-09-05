using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;

public class UserModel
{
	private static UserModel _user;

	public const string newPlayerKey = "newPlayerKey";

	private Dictionary<string, LevelData> levelDataList = new Dictionary<string, LevelData>();

	private Dictionary<string, LevelData> CoinLevelList = new Dictionary<string, LevelData>();

	public const string LevelJsonPath = "LevelConfig/OfficialJson";

	public const string LevelTexturePath = "LevelConfig/LevelTexture";

	public const string PassName = "PassLevelName";

	private const string MaxPassName = "MaxPassName";

	private const string PassTotalName = "PassTotalName";

	private const string LatelyLevelName = "LatelyLevelName";

	public const string MaxStarNum = "MaxStarNum";

	public static int MTPlayCount;

	public const string LatelyOpenCharpterStr = "LatelyOpenCharpter";



	private int money;

	private int hintCount;



	private const string GoodsName = "GoodsName_";

	public const string voice = "LoveShots_GameSetting_voice";

	public const string music = "LoveShots_GameSetting_music";

	public const string vibration = "LoveShots_GameSetting_bration";

	public const string IsNoADS = "LoveShots_IsNoADS";

	public const string playHintStr = "PlayHint";

	public const string newChartBoost = "NewPlayCharBoost";

	public event Action<int> OnMoneyChange;

	public event Action OnHintChange;

	public static UserModel Inst
	{
		get
		{
			if (UserModel._user == null)
			{
				UserModel._user = new UserModel();
			}
			return UserModel._user;
		}
	}

	public bool isNewPlayer
	{
		get
		{
			return PlayerPrefs.GetInt("newPlayerKey", 0) == 0;
		}
		set
		{
			PlayerPrefs.SetInt("newPlayerKey", (!value) ? 1 : 0);
		}
	}

	public string latelyLevel
	{
		get
		{
			return PlayerPrefs.GetString("LatelyLevelName", SceneSetting.inst.FristLevelId);
		}
		set
		{
			PlayerPrefs.SetString("LatelyLevelName", value);
		}
	}

	public string LatelyOpenCharpter
	{
		get
		{
			return PlayerPrefs.GetString("LatelyOpenCharpter", ChapterTemplate.Dic().Keys.First<string>());
		}
		set
		{
			PlayerPrefs.SetString("LatelyOpenCharpter", value);
		}
	}

	public int Money
	{
		get
		{
			return this.money = PlayerPrefs.GetInt("Money", 0);
		}
		set
		{
			int num = this.money;
			this.money = value;
			PlayerPrefs.SetInt("Money", this.money);
			if (this.OnMoneyChange != null)
			{
				this.OnMoneyChange(value - num);
			}
		}
	}

	public int HintCount
	{
		get
		{
			return this.hintCount = PlayerPrefs.GetInt("hintCount", 0);
		}
		set
		{
			this.hintCount = value;
			PlayerPrefs.SetInt("hintCount", this.hintCount);
			if (this.OnHintChange != null)
			{
				this.OnHintChange();
			}
		}
	}

	public static string UsedSkinKey
	{
		get
		{
			return PlayerPrefs.GetString("UsedSkinKey", "101");
		}
		set
		{
			PlayerPrefs.SetString("UsedSkinKey", value);
		}
	}

	public bool FristUseHint
	{
		get
		{
			return PlayerPrefs.GetInt("PlayHint", 0) == 0;
		}
		set
		{
			PlayerPrefs.SetInt("PlayHint", (!value) ? 1 : 0);
		}
	}

	public static int WatchVideoCount
	{
		get
		{
			return PlayerPrefs.GetInt("WatchVideoCount", 0);
		}
		set
		{
			PlayerPrefs.SetInt("WatchVideoCount", value);
		}
	}

	public int NewPlayerChartBoost
	{
		get
		{
			return PlayerPrefs.GetInt("NewPlayCharBoost", 0);
		}
		set
		{
			PlayerPrefs.SetInt("NewPlayCharBoost", value);
		}
	}

	public bool LoadLevelData()
	{
		if (this.levelDataList.Count == 0)
		{
			string path = "LevelConfig/OfficialJson";
			TextAsset textAsset = Resources.Load<TextAsset>(path);
			if (textAsset.bytes == null || textAsset.bytes.Length == 0)
			{
				return false;
			}
			MemoryStream memoryStream = new MemoryStream(textAsset.bytes);
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			int num = binaryReader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				try
				{
					string text = binaryReader.ReadString();
					if (string.IsNullOrEmpty(text))
					{
						UnityEngine.Debug.Log(string.Format("<color=red>内置关卡数据读取失败:{0}</color>", text));
					}
					else
					{
						string levelJson = binaryReader.ReadString();
						if (this.levelDataList.ContainsKey(text))
						{
							UnityEngine.Debug.Log(string.Format("<color=red>key 重复:{0}</color>", text));
						}
						else
						{
							LevelData levelData = new LevelData(text, i);
							levelData.LevelJson = levelJson;
							this.levelDataList.Add(text, levelData);
						}
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError(string.Concat(new object[]
					{
						"Length:",
						num,
						":实际长度：",
						i,
						" ",
						ex
					}));
				}
			}
			binaryReader.Close();
			memoryStream.Close();
			memoryStream.Dispose();
			this.InitPassLevel();
		}
		return true;
	}

	public void ReLoadLevelData()
	{
		this.levelDataList.Clear();
		this.LoadLevelData();
	}

	public LevelData GetLevelData(string key)
	{
		if (this.LoadLevelData() && this.levelDataList.ContainsKey(key))
		{
			return this.levelDataList[key];
		}
		return null;
	}

	public LevelData GetLevelData(int index)
	{
		LevelData result = null;
		if (this.LoadLevelData() && this.levelDataList.Count > index)
		{
			result = this.levelDataList.Values.ToList<LevelData>()[index];
		}
		return result;
	}

	public bool IsLastLevel(string key)
	{
		if (this.LoadLevelData() && this.levelDataList.Count > 0)
		{
			string a = this.levelDataList.Keys.Last<string>();
			if (a != key)
			{
				return false;
			}
		}
		return true;
	}

	public int GetNormalLevelCount()
	{
		if (this.LoadLevelData() && this.levelDataList.Count > 0)
		{
			return this.levelDataList.Count;
		}
		return 0;
	}

	public void InitPassLevel()
	{
		string @string = PlayerPrefs.GetString("PassLevelName");
		if (!string.IsNullOrEmpty(@string))
		{
			string[] array = @string.Split(new char[]
			{
				'|'
			});
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[]
				{
					','
				});
				LevelData levelData;
				if (this.levelDataList.TryGetValue(array2[0], out levelData))
				{
					levelData.passGrade = int.Parse(array2[1]);
				}
			}
		}
	}

	public void SavePassLevel(string key, int grade)
	{
		LevelData levelData;
		if (this.levelDataList.TryGetValue(key, out levelData))
		{
			PlayerPrefs.SetInt("MaxStarNum", this.GetStarCount() + Mathf.Max(0, grade - levelData.passGrade));
			levelData.passGrade = grade;
			if (grade > 0)
			{
				UserModel.SetFirstPassPlayCount(key);
			}
			if (grade >= 3)
			{
				UserModel.SetFirstThreeStarCount(key);
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, LevelData> current in this.levelDataList)
			{
				stringBuilder.Append(current.Key + "," + current.Value.passGrade);
				stringBuilder.Append("|");
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			if (grade != 0)
			{
				string text = (int.Parse(levelData.baseData.key) + 1).ToString();
				if (string.Compare(UserModel.GetMaxPassLevel(true), text, StringComparison.Ordinal) < 0)
				{
					PlayerPrefs.SetString("MaxPassName", text);
				}
			}
			PlayerPrefs.SetString("PassLevelName", stringBuilder.ToString());
		}
	}

	public int GetStarCount()
	{
		return PlayerPrefs.GetInt("MaxStarNum");
	}

	public static string GetMaxPassLevel(bool isGetMax = false)
	{
		if (!isGetMax && LevelStage.CurStageInst != null && LevelStage.CurStageInst.gameObject != null && LevelStage.CurStageInst.curlevelData != null)
		{
			return LevelStage.CurStageInst.curlevelData.baseData.key;
		}
		return PlayerPrefs.GetString("MaxPassName", SceneSetting.inst.FristLevelId);
	}

	public static int MTPlayLevelCount(string key)
	{
		return PlayerPrefs.GetInt("MTPlayLevelCount_" + key);
	}

	public static void AddMTPlayLevelCount(string key)
	{
		int value = UserModel.MTPlayLevelCount(key) + 1;
		PlayerPrefs.SetInt("MTPlayLevelCount_" + key, value);
	}

	public void AllPassLevelCount(out int max)
	{
		max = 0;
		foreach (KeyValuePair<string, LevelData> current in this.levelDataList)
		{
			if (current.Value.passGrade > 0)
			{
				max += current.Value.passGrade;
			}
		}
	}

	public static int GetFirstThreeStarCount(string key)
	{
		return PlayerPrefs.GetInt("FirstThreeStarCount_" + key);
	}

	public static void SetFirstThreeStarCount(string key)
	{
		if (!PlayerPrefs.HasKey("FirstThreeStarCount_" + key))
		{
			PlayerPrefs.SetInt("FirstThreeStarCount_" + key, UserModel.MTPlayLevelCount(key));
		}
	}

	public static int GetFirstPassPlayCount(string key)
	{
		return PlayerPrefs.GetInt("FirstPassPlayCount_" + key);
	}

	public static void SetFirstPassPlayCount(string key)
	{
		if (!PlayerPrefs.HasKey("FirstPassPlayCount_" + key))
		{
			PlayerPrefs.SetInt("FirstPassPlayCount_" + key, UserModel.MTPlayLevelCount(key));
		}
	}

	public static bool IsOwnGoods(string key)
	{
		return PlayerPrefs.GetInt("GoodsName_" + key) == 1 || SkinTemplate.Tem(new object[]
		{
			key
		}).obtainWay == 0;
	}

	public static void BuyGoods(string key, bool sendcloud = true)
	{
		PlayerPrefs.SetInt("GoodsName_" + key, 1);
		SkinTemplate skinTemplate = SkinTemplate.Tem(new object[]
		{
			key
		});
		int num = 0;
		foreach (KeyValuePair<string, SkinTemplate> current in SkinTemplate.Dic())
		{
			if (UserModel.IsOwnGoods(current.Key))
			{
				num++;
			}
		}
		MagicTavernHelper.Track("skins", new object[]
		{
			skinTemplate.obtainWay - 1,
			key,
			num
		});
		PlayerPrefs.Save();
	}

	public List<SkinTemplate> IsLockVideoSkin()
	{
		List<SkinTemplate> list = new List<SkinTemplate>();
		foreach (KeyValuePair<string, SkinTemplate> current in SkinTemplate.Dic())
		{
			if (current.Value.obtainWay == 2 && !UserModel.IsOwnGoods(current.Key))
			{
				int num = int.Parse(current.Value.price);
				if (num <= UserModel.WatchVideoCount)
				{
					list.Add(current.Value);
				}
			}
		}
		return list;
	}

	public static bool GetBration()
	{
		return UserModel.GetConfig("LoveShots_GameSetting_bration", "1") == "1";
	}

	public static void SetConfig(string name, string value)
	{
		PlayerPrefs.SetString(name, value);
		PlayerPrefs.Save();
	}

	public static string GetConfig(string name, string defaultValue = "0")
	{
		return PlayerPrefs.GetString(name, defaultValue);
	}

	public static bool IsPurchasePackage()
	{
		return UserModel.GetConfig("Store_Package", "0") == "1";
	}

	public static void SetPurchasePackage()
	{
		UserModel.SetConfig("Store_Package", "1");
	}

	public static bool TurntableIsFree()
	{
		return PlayerPrefs.GetInt("TurntableFristFree") == 0;
	}

	public static void SetTurntableNotFree()
	{
		PlayerPrefs.SetInt("TurntableFristFree", 1);
	}

	public static void SetNoAds()
	{
		PlayerPrefs.SetInt("IsNoads", 1);
	}

	public static bool IsNoAds()
	{
		return PlayerPrefs.GetInt("IsNoads") == 1;
	}
}
