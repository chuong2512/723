using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class UGUIAtlas
{
	public enum AtlasType
	{
		Game
	}

	private static Dictionary<UGUIAtlas.AtlasType, SpriteAtlas> atlas = new Dictionary<UGUIAtlas.AtlasType, SpriteAtlas>();

	public static Sprite GetSprite(UGUIAtlas.AtlasType type, string name)
	{
		if (!UGUIAtlas.atlas.ContainsKey(type))
		{
			UGUIAtlas.atlas[type] = Resources.Load<SpriteAtlas>("UI/Atlas/" + type.ToString());
		}
		return UGUIAtlas.atlas[type].GetSprite(name);
	}
}
