using LitJson;
using System;
using UnityEngine;

public class EntitiesFactory
{
	public static Primitives Create(ObjType type, Transform parent, string prefabType = "")
	{
		Primitives primitives;
		switch (type)
		{
		case ObjType.Duck:
			primitives = EntitiesFactory.CreatePrimitive<Duck>(parent, type.ToString(), prefabType);
			break;
		case ObjType.Bathtub:
			primitives = EntitiesFactory.CreatePrimitive<Bathtub>(parent, type.ToString(), prefabType);
			break;
		case ObjType.Rope:
			primitives = EntitiesFactory.CreatePrimitive<Rope>(parent, type.ToString(), prefabType);
			break;
		case ObjType.Water2D:
			primitives = EntitiesFactory.CreateWater(parent, prefabType);
			break;
		case ObjType.Container:
			primitives = EntitiesFactory.CreatePrimitive<Container>(parent, type.ToString(), prefabType);
			break;
		case ObjType.Teeterboard:
			primitives = EntitiesFactory.CreatePrimitive<Teeterboard>(parent, type.ToString(), prefabType);
			break;
		case ObjType.Portal:
			primitives = EntitiesFactory.CreatePortal(parent, prefabType);
			break;
		case ObjType.Block:
			primitives = EntitiesFactory.CreatePrimitive<Block>(parent, type.ToString(), prefabType);
			break;
		case ObjType.Fan:
			primitives = EntitiesFactory.CreatePrimitive<Fan>(parent, type.ToString(), prefabType);
			break;
		case ObjType.MoveCube:
			primitives = EntitiesFactory.CreatePrimitive<MoveCube>(parent, type.ToString(), prefabType);
			break;
		case ObjType.Conveyor:
			primitives = EntitiesFactory.CreatePrimitive<Conveyor>(parent, type.ToString(), prefabType);
			break;
		case ObjType.TriggerObj:
			primitives = EntitiesFactory.CreateTriggerObj(parent, prefabType);
			break;
		default:
			if (type != ObjType.HintObj)
			{
				primitives = EntitiesFactory.CreatePrimitive<Primitives>(parent, type.ToString(), prefabType);
			}
			else
			{
				primitives = EntitiesFactory.CreatePrimitive<HintObj>(parent, type.ToString(), prefabType);
			}
			break;
		}
		primitives.prefabType = prefabType;
		primitives.mTrans.name = type + prefabType;
		return primitives;
	}

	public static Primitives Create(string type, Transform parent, string prefabType = "")
	{
		ObjType type2 = (ObjType)Enum.Parse(typeof(ObjType), type);
		return EntitiesFactory.Create(type2, parent, prefabType);
	}

	public static T CreatePrimitive<T>(Transform parent, string path = "", string perfabType = "") where T : Primitives
	{
		if (path == string.Empty)
		{
			path = typeof(T).ToString();
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/GameObj/" + path + perfabType), parent);
		T t = gameObject.AddComponent<T>();
		t.minScale = 0.2f;
		return t;
	}

	public static Primitives CreateWater(Transform parent, string perfabType = "")
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/GameObj/Water" + perfabType), parent);
		return gameObject.GetComponent<Water2D>();
	}

	public static Portal CreatePortal(Transform parent, string perfabType)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/GameObj/Portal" + perfabType));
		gameObject.transform.SetParent(parent, false);
		Portal portal = gameObject.AddComponent<Portal>();
		portal.transferType = 1;
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/GameObj/Portal" + perfabType));
		gameObject2.transform.SetParent(parent, false);
		Portal portal2 = gameObject2.AddComponent<Portal>();
		portal2.transferType = 2;
		portal.other = portal2;
		portal2.other = portal;
		return portal;
	}

	public static TriggerObj CreateTriggerObj(Transform parent, string perfabType)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/GameObj/Trigger" + perfabType));
		gameObject.transform.SetParent(parent, false);
		TriggerObj triggerObj = gameObject.AddComponent<TriggerObj>();
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/GameObj/TriggerObj" + perfabType));
		gameObject2.transform.SetParent(parent, false);
		Primitives needTriggerObj = gameObject2.AddComponent<Primitives>();
		triggerObj.needTriggerObj = needTriggerObj;
		return triggerObj;
	}

	public static Primitives CopyObj(Primitives old, bool isCopy)
	{
		Primitives primitives = EntitiesFactory.Create(old.GetType().ToString(), old.transform.parent, old.prefabType);
		primitives.Json = JsonMapper.ToObject(old.GetJson());
		primitives.Sleep();
		Vector3 scale = old.OriginalScale;
		if (!isCopy)
		{
			EditObjData component = primitives.GetComponent<EditObjData>();
			if (component != null && !component.InheritScale)
			{
				scale = component.DefaultScale;
			}
		}
		primitives.CopyDeserialization(old.OriginalPos, scale, old.OriginalRotation);
		return primitives;
	}
}
