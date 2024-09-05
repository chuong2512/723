using System;
using System.Collections.Generic;
using UnityEngine;

public class MTBrokenNetworkLog : MonoBehaviour
{
	private static bool IsInit = false;

	private static bool NeedToSave = false;

	private static float UpdateTime = 0f;

	private const float SaveTimeGap = 1f;

	private const string MTKeyName = "MTJsonName";

	private const int SaveMaxNum = 2000;

	private static Queue<string> jsonList = new Queue<string>();

	private const int SendNumPer = 5;

	private static Action<string> Resend = null;

	private static Action<byte[], Dictionary<string, string>> ResendFailed = null;

	public static void Init(Transform parent, Action<string> _Resend, Action<byte[], Dictionary<string, string>> _ResendFailed)
	{
		if (MTBrokenNetworkLog.IsInit)
		{
			return;
		}
		MTBrokenNetworkLog.IsInit = true;
		MTBrokenNetworkLog.Resend = _Resend;
		MTBrokenNetworkLog.ResendFailed = _ResendFailed;
		MTBrokenNetworkLog mTBrokenNetworkLog = new GameObject().AddComponent<MTBrokenNetworkLog>();
		mTBrokenNetworkLog.transform.SetParent(parent);
	}

	private void Start()
	{
		string @string = PlayerPrefs.GetString("MTJsonName");
		string[] array = @string.Split(new string[]
		{
			"+++"
		}, StringSplitOptions.RemoveEmptyEntries);
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string item = array2[i];
			MTBrokenNetworkLog.jsonList.Enqueue(item);
		}
	}

	public static void AddNewLog(string json)
	{
		if (MTBrokenNetworkLog.jsonList.Count >= 2000)
		{
			MTBrokenNetworkLog.jsonList.Dequeue();
		}
		MTBrokenNetworkLog.jsonList.Enqueue(json);
		MTBrokenNetworkLog.NeedToSave = true;
	}

	public static void AddNewLog(string error, byte[] data, Dictionary<string, string> header)
	{
		if (error == "400 Bad Request")
		{
			return;
		}
		if (MTBrokenNetworkLog.ResendFailed != null)
		{
			MTBrokenNetworkLog.ResendFailed(data, header);
		}
	}

	private void Update()
	{
		if (MTBrokenNetworkLog.jsonList.Count == 0)
		{
			return;
		}
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			if (MTBrokenNetworkLog.NeedToSave)
			{
				MTBrokenNetworkLog.UpdateTime += Time.deltaTime;
				if (MTBrokenNetworkLog.UpdateTime >= 1f)
				{
					MTBrokenNetworkLog.UpdateTime = 0f;
					this.Save();
				}
			}
		}
		else
		{
			if (MTBrokenNetworkLog.Resend == null)
			{
				return;
			}
			int num = (5 > MTBrokenNetworkLog.jsonList.Count) ? MTBrokenNetworkLog.jsonList.Count : 5;
			for (int i = 0; i < num; i++)
			{
				string text = MTBrokenNetworkLog.jsonList.Dequeue();
				if (!(text == string.Empty))
				{
					MTBrokenNetworkLog.Resend(text);
				}
			}
			this.Save();
		}
	}

	private void Save()
	{
		MTBrokenNetworkLog.NeedToSave = false;
		string text = string.Empty;
		foreach (string current in MTBrokenNetworkLog.jsonList)
		{
			text = text + current + "+++";
		}
		PlayerPrefs.SetString("MTJsonName", text);
		PlayerPrefs.Save();
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			this.Save();
		}
	}

	private void OnDestroy()
	{
		this.Save();
		MTBrokenNetworkLog.IsInit = false;
		MTBrokenNetworkLog.UpdateTime = 0f;
		MTBrokenNetworkLog.jsonList.Clear();
		MTBrokenNetworkLog.Resend = null;
		MTBrokenNetworkLog.ResendFailed = null;
	}
}
