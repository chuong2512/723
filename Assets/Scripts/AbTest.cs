using System;
using UnityEngine;

public class AbTest
{
	private static bool IsOpenABTest;

	public const string ABTESTSIGN = "A_BTEST1.0.0";

	public static string confVersion = string.Empty;

	private static string ABTYPE = string.Empty;

	public static void InstallInit()
	{
		if (!AbTest.IsOpenABTest)
		{
			return;
		}
		PlayerPrefs.SetInt("A_BTEST1.0.0", 1);
	}

	public static void LoginInit()
	{
		if (!AbTest.IsOpenABTest)
		{
			return;
		}
		if (PlayerPrefs.GetInt("A_BTEST1.0.0", 0) == 0)
		{
			if (Common.IsAuditPackage())
			{
				AbTest.ABTYPE = "b";
				AbTest.confVersion = "1400";
			}
			else
			{
				AbTest.ABTYPE = "c";
				AbTest.confVersion = "1402";
			}
		}
		else if (Common.IsAuditPackage())
		{
			AbTest.ABTYPE = "b";
			AbTest.confVersion = "1400";
		}
		else
		{
			string text = WJUtils.GetDeviceUID();
			text = text.Substring(text.Length - 6, 6);
			int num = Convert.ToInt32(text, 16);
			num %= 2;
			if (num != 1)
			{
				AbTest.ABTYPE = "b";
				AbTest.confVersion = "1400";
			}
			else
			{
				AbTest.ABTYPE = "a";
				AbTest.confVersion = "1401";
			}
		}
	}
}
