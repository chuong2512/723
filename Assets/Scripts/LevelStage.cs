using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class LevelStage : MonoBehaviour
{
	public enum GameState
	{
		Starting,
		Success,
		Fail
	}

	public static LevelStage CurStageInst;

	public Transform GameObjParent;

	public Transform WaterParent;

	private Transform mTrans;

	public string HintStr;





	private LevelStage.GameState _gameState;

	private LevelData levelData;

	private List<JsonData> JosnList = new List<JsonData>();

	public List<Primitives> loadedObj = new List<Primitives>();

	public List<InteractObj> InteractObjs = new List<InteractObj>();

	private bool preCountDown;

	public static LevelStage CurEditStageInst;

	public event Action GameFailEvent;

	public event Action SuccessEvent;

	public LevelData curlevelData
	{
		get
		{
			return this.levelData;
		}
	}

	public LevelStage.GameState gameState
	{
		get
		{
			return this._gameState;
		}
		set
		{
			if (this._gameState != LevelStage.GameState.Fail && value == LevelStage.GameState.Fail)
			{
				this._gameState = LevelStage.GameState.Fail;
				if (this.levelData != null)
				{
					MagicTavernHelper.Track("levelEnd", new object[]
					{
						this.levelData.gameTime,
						0,
						UserModel.MTPlayLevelCount(this.levelData.key),
						0,
						UserModel.GetFirstPassPlayCount(this.levelData.key) + ";" + UserModel.GetFirstThreeStarCount(this.levelData.key)
					});
					GameScene.scene.ShowCBRestart(this.levelData, LevelUIView.StartType.LevelAutoRestart, ADSManager.CBLoaction.LevelEnd);
					if (this.GameFailEvent != null)
					{
						this.GameFailEvent();
					}
				}
			}
			else if (this._gameState != LevelStage.GameState.Success && value == LevelStage.GameState.Success)
			{
				this._gameState = LevelStage.GameState.Success;
				if (this.levelData != null)
				{
					LevelUIView openWindow = UIManager.GetInst(false).GetOpenWindow<LevelUIView>();
					if (openWindow)
					{
						openWindow.group.interactable = false;
					}
					GameScene.scene.GameSuccess(this.levelData);
				}
				if (this.SuccessEvent != null)
				{
					this.SuccessEvent();
				}
			}
		}
	}

	public static LevelStage Create(LevelData levelData)
	{
		LevelStage levelStage = LevelStage.Create(levelData.LevelJson);
		levelStage.levelData = levelData;
		return levelStage;
	}

	public static LevelStage Create(string json = "")
	{
		GameObject gameObject = new GameObject("LevelStage");
		GameObject gameObject2 = new GameObject("GameObjParent");
		GameObject gameObject3 = new GameObject("WaterParent");
		gameObject3.transform.parent = gameObject.transform;
		gameObject3.transform.localPosition = Vector3.zero;
		gameObject2.transform.parent = gameObject.transform;
		gameObject2.transform.localPosition = Vector3.zero;
		LevelStage levelStage = gameObject.AddComponent<LevelStage>();
		LevelStage.CurStageInst = levelStage;
		LevelStage.CurStageInst.GameObjParent = gameObject2.transform;
		LevelStage.CurStageInst.WaterParent = gameObject3.transform;
		if (!json.IsNullOrEntry())
		{
			LevelStage.CurStageInst.LoadJson(json);
			LevelStage.CurStageInst.CreateAllObj(false);
		}
		return levelStage;
	}

	public void LoadJson(string json)
	{
		if (json != string.Empty)
		{
			JsonData jsonData = JsonMapper.ToObject(json);
			JsonData jsonData2 = jsonData["root"];
			if (jsonData.Keys.Contains("tutorialHint"))
			{
				this.HintStr = jsonData["tutorialHint"].ToString();
			}
			IEnumerator enumerator = ((IEnumerable)jsonData2).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					JsonData item = (JsonData)enumerator.Current;
					this.JosnList.Add(item);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}

	private void CreateAllObj(bool isEdit = false)
	{
		if (!isEdit && this.WaterParent != null)
		{
			this.WaterParent.gameObject.AddComponent<ParticleGenerator>();
		}
		for (int i = 0; i < this.JosnList.Count; i++)
		{
			this.CreateSingle(this.JosnList[i], isEdit);
		}
	}

	private void CreateSingle(JsonData node, bool isEdit = false)
	{
		string type = node["type"].ToString();
		string prefabType = string.Empty;
		if (node.Keys.Contains("prefabType"))
		{
			prefabType = node["prefabType"].ToString();
		}
		Primitives primitives = EntitiesFactory.Create(type, this.GameObjParent, prefabType);
		if (isEdit)
		{
			primitives.Sleep();
			primitives.Deserialization(node);
		}
		else if (primitives)
		{
			primitives.Deserialization(node);
		}
		this.loadedObj.Add(primitives);
		if (primitives is InteractObj)
		{
			this.InteractObjs.Add(primitives as InteractObj);
		}
	}

	public bool DetectionCountDown()
	{
		if (!this.preCountDown)
		{
			this.preCountDown = this.DetectionCD();
			return false;
		}
		return this.preCountDown;
	}

	private bool DetectionCD()
	{
		for (int i = 0; i < this.loadedObj.Count; i++)
		{
			if (this.loadedObj[i] is InteractObj && ((InteractObj)this.loadedObj[i]).CanInteract)
			{
				return false;
			}
		}
		for (int j = 0; j < this.loadedObj.Count; j++)
		{
			if (this.loadedObj[j] != null && this.loadedObj[j].rigidbody2D != null)
			{
				if (this.loadedObj[j].rigidbody2D.velocity.magnitude > 0.5f)
				{
					return false;
				}
				if (Mathf.Abs(this.loadedObj[j].rigidbody2D.angularVelocity) > 1f)
				{
					return false;
				}
			}
		}
		return true;
	}

	public static void LoadAllObjToEditing(string json = "")
	{
		LevelStage.DestroyWorld(false);
		if (LevelStage.CurEditStageInst == null)
		{
			GameObject gameObject = new GameObject("EditLevelWorld");
			GameObject gameObject2 = new GameObject("GameObjParent");
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.transform.localPosition = Vector3.zero;
			LevelStage curEditStageInst = gameObject.AddComponent<LevelStage>();
			LevelStage.CurEditStageInst = curEditStageInst;
			LevelStage.CurEditStageInst.GameObjParent = gameObject2.transform;
		}
		if (!json.IsNullOrEntry())
		{
			LevelStage.CurEditStageInst.LoadJson(json);
			LevelStage.CurEditStageInst.CreateAllObj(true);
		}
		else
		{
			LevelStage.CurEditStageInst.gameObject.SetActive(true);
		}
	}

	public static void ToPreview()
	{
		LevelStage.CurEditStageInst.gameObject.SetActive(false);
		string json = LevelStage.CurEditStageInst.GetJson();
		LevelStage.Create(json);
	}

	public void AddObjWithEditor(Primitives obj)
	{
		this.loadedObj.Add(obj);
	}

	public void RemoveObjWithEditor(Primitives obj)
	{
		this.loadedObj.Remove(obj);
	}

	public string GetJson()
	{
		if (this.loadedObj.Count > 0)
		{
			string text = string.Empty;
			for (int i = 0; i < this.loadedObj.Count; i++)
			{
				text = text + this.loadedObj[i].GetJson() + ",";
			}
			if (text[text.Length - 1] == ',')
			{
				text = text.Remove(text.Length - 1);
			}
			return "{\"tutorialHint\":\"%tutorialHint%\",\"root\":[%content%]}".Replace("%content%", text).Replace("%tutorialHint%", this.HintStr);
		}
		return string.Empty;
	}

	public static void DestroyWorld(bool IsEditWorld = false)
	{
		if (IsEditWorld)
		{
			if (LevelStage.CurEditStageInst != null)
			{
				UnityEngine.Object.Destroy(LevelStage.CurEditStageInst.gameObject);
			}
			LevelStage.CurEditStageInst = null;
		}
		else
		{
			if (LevelStage.CurStageInst != null)
			{
				UnityEngine.Object.Destroy(LevelStage.CurStageInst.gameObject);
			}
			LevelStage.CurStageInst = null;
			if (ParticleGenerator.Inst != null)
			{
				ParticleGenerator.Inst.Reset();
			}
			UIManager.GetInst(false).CloseOpenedWindow<HintView>();
		}
	}
}
