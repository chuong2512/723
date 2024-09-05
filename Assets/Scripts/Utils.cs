using System;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
	public static Color UIGrayColor = new Color(0.498039216f, 0.498039216f, 0.498039216f);

	public static string GetString(string key, params object[] args)
	{
		if (key.IsNullOrEntry())
		{
			return null;
		}
		StringsTemplate stringsTemplate = StringsTemplate.Tem(new object[]
		{
			key
		});
		if (stringsTemplate == null)
		{
			UnityEngine.Debug.LogError("语言配置表不包含key:" + key);
			return string.Empty;
		}
		if (args != null && args.Length > 0)
		{
			return string.Format(stringsTemplate.text, args);
		}
		return stringsTemplate.text;
	}

	public static Vector3 ScreenToLocalPos(Vector3 screenPos, Transform parent)
	{
		Vector3 position = new Vector3(screenPos.x, screenPos.y, -Camera.main.transform.position.z);
		Vector3 point = Camera.main.ScreenToWorldPoint(position);
		return parent.worldToLocalMatrix.MultiplyPoint(point);
	}

	public static T Find<T>(Transform parent, string namePath) where T : Component
	{
		Transform transform = parent.Find(namePath);
		if (transform != null)
		{
			return transform.GetComponent<T>();
		}
		return (T)((object)null);
	}

	public static int RandomIndex(List<int> prob)
	{
		int result = 0;
		int num = 0;
		for (int i = 0; i < prob.Count; i++)
		{
			num += prob[i];
		}
		int max = num * 1000;
		int num2 = UnityEngine.Random.Range(0, max);
		int num3 = 0;
		for (int j = 0; j < prob.Count; j++)
		{
			int num4 = num3 + prob[j];
			if (num2 >= num3 * 1000 && num2 < num4 * 1000)
			{
				result = j;
				break;
			}
			num3 = num4;
		}
		return result;
	}
}
