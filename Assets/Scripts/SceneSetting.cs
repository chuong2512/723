using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneSetting.asset")]
[Serializable]
public class SceneSetting : ScriptableObject
{
	private static SceneSetting _inst;

	public int Height = 2048;

	public int Width = 1536;

	[Tooltip("在16：9下摄像机的fieldOfView值")]
	public float CameraAdjust = 20f;

	[Tooltip("第一关ID")]
	public string FristLevelId = "1001";

	public Material RopeTexture;

	[Tooltip("生成水的最大数量")]
	public int WaterMaxCount = 500;

	[Tooltip("生成水的间隔时间")]
	public float WaterSpawnInterval = 0.025f;

	[Tooltip("水初始力")]
	public Vector3 waterForce;

	public List<ObjTypeEditData> ObjTypeDic;

	public static SceneSetting inst
	{
		get
		{
			if (SceneSetting._inst == null)
			{
				SceneSetting._inst = Resources.Load<SceneSetting>("SceneSetting");
				if (SceneSetting._inst == null)
				{
					SceneSetting._inst = ScriptableObject.CreateInstance<SceneSetting>();
				}
			}
			return SceneSetting._inst;
		}
	}

	public List<ObjTypeEditData> FindTypeDataListbyType(string type)
	{
		List<ObjTypeEditData> list = new List<ObjTypeEditData>();
		for (int i = 0; i < this.ObjTypeDic.Count; i++)
		{
			if (this.ObjTypeDic[i].key.ToString() == type)
			{
				list.Add(this.ObjTypeDic[i]);
			}
		}
		return list;
	}

	public string FindObjTypeListbyName(string type, string editorName)
	{
		for (int i = 0; i < this.ObjTypeDic.Count; i++)
		{
			if (this.ObjTypeDic[i].key.ToString() == type && this.ObjTypeDic[i].EditorShowName == editorName)
			{
				return this.ObjTypeDic[i].ObjType;
			}
		}
		return null;
	}
}
