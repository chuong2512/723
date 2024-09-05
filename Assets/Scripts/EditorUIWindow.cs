using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorUIWindow : BaseView
{
	private sealed class _EventRegister_c__AnonStorey0
	{
		internal UGUIEvent openEvent;

		internal EditorUIWindow _this;

		internal void __m__0(PointerEventData pointData, UGUIEvent sender)
		{
			if (pointData.dragging)
			{
				return;
			}
			this._this.scrollTrans.gameObject.SetActive(true);
			this.openEvent.gameObject.SetActive(false);
			this._this.levelLoadUI.gameObject.SetActive(true);
			UIManager.GetInst(false).CloseOpenedWindow<EditorTransformBorder>();
		}
	}

	private sealed class _SaveBtnClick_c__AnonStorey1
	{
		internal string key;

		internal EditorUIWindow _this;

		internal void __m__0()
		{
			Texture2D tex = Common.Snapshoot(Camera.main, 512, 512);
			this._this.SaveTex(this.key, tex);
			this._this.DelayToDo(0.1f, delegate
			{
				this._this.PlayOrEdit(false);
				this._this.editorOrPlay.gameObject.SetActive(true);
			}, false);
		}

		internal void __m__1()
		{
			this._this.PlayOrEdit(false);
			this._this.editorOrPlay.gameObject.SetActive(true);
		}
	}

	private sealed class _LevelListBtnClick_c__AnonStorey2
	{
		internal int startIndex;

		internal List<string> list;

		internal EditorUIWindow _this;

		internal void __m__0(string indexStr)
		{
			this._this.levelNumIn.text = indexStr;
			int index = int.Parse(indexStr) - this.startIndex;
			this._this.LoadWorld(this.list[index]);
		}
	}

	private float mPixel2ngui = 1f;

	private Transform imageTrans;

	private Transform scrollContent;

	private Transform scrollTrans;

	private Toggle editorOrPlay;

	private Transform openMenu;

	private Transform levelLoadUI;

	private InputField levelNumIn;

	private Button LevelLoadBtn;

	private Button LevelSaveBtn;

	private Button LevelSaveAllBtn;

	private InputField levelListStartIn;

	private InputField levelListEndIn;

	private Button levelListBtn;

	private Text waterCountLabel;

	private Text hintText;

	private InputField saveAllIn;

	private Vector2 mTouchOffset = Vector2.zero;

	private void Awake()
	{
		this.scrollTrans = base.transform.Find("ScrollView");
		this.scrollContent = this.scrollTrans.Find("Viewport/Content");
		this.imageTrans = this.scrollContent.Find("item");
		this.openMenu = base.transform.Find("openMenu");
		this.editorOrPlay = base.transform.Find("editorOrPlay").GetComponent<Toggle>();
		this.levelLoadUI = base.transform.Find("levelLoadPanel");
		this.levelNumIn = this.levelLoadUI.Find("Input").GetComponent<InputField>();
		this.LevelLoadBtn = this.levelLoadUI.Find("loadBtn").GetComponent<Button>();
		this.LevelSaveBtn = this.levelLoadUI.Find("saveBtn").GetComponent<Button>();
		this.LevelSaveAllBtn = this.levelLoadUI.Find("saveAllBtn").GetComponent<Button>();
		this.levelListBtn = base.Find<Button>(this.levelLoadUI, "levelList/ShowBtn");
		this.levelListStartIn = base.Find<InputField>(this.levelLoadUI, "levelList/Input");
		this.levelListEndIn = base.Find<InputField>(this.levelLoadUI, "levelList/Input2");
		this.waterCountLabel = base.Find<Text>(base.transform, "Time");
		this.hintText = base.Find<Text>(base.transform, "tutorialHint/Text");
		this.saveAllIn = base.Find<InputField>(this.levelLoadUI, "saveAllIn");
		this.CreateAllEntities();
		this.EventRegister();
	}

	private void EventRegister()
	{
		this.editorOrPlay.onValueChanged.AddListener(new UnityAction<bool>(this.PlayOrEdit));
		UGUIEvent openEvent = this.openMenu.gameObject.AddComponent<UGUIEvent>();
		openEvent.onDrag += new Action<PointerEventData, UGUIEvent>(this.MenuDrag);
		openEvent.onPointerClick += delegate(PointerEventData pointData, UGUIEvent sender)
		{
			if (pointData.dragging)
			{
				return;
			}
			this.scrollTrans.gameObject.SetActive(true);
			openEvent.gameObject.SetActive(false);
			this.levelLoadUI.gameObject.SetActive(true);
			UIManager.GetInst(false).CloseOpenedWindow<EditorTransformBorder>();
		};
		this.LevelSaveBtn.onClick.AddListener(new UnityAction(this.SaveBtnClick));
		this.LevelLoadBtn.onClick.AddListener(new UnityAction(this.LoadBtnClick));
		this.LevelSaveAllBtn.onClick.AddListener(new UnityAction(this.SaveAllBtnClick));
		this.levelListBtn.onClick.AddListener(new UnityAction(this.LevelListBtnClick));
	}

	private void Update()
	{
		if (this.waterCountLabel.gameObject.activeSelf && ParticleGenerator.Inst != null)
		{
			this.waterCountLabel.text = ParticleGenerator.Inst.WaterCount.ToString();
		}
	}

	private void PlayOrEdit(bool toggle)
	{
		this.editorOrPlay.GetComponent<Image>().color = ((!toggle) ? Color.red : Color.green);
		if (toggle)
		{
			LevelStage.ToPreview();
			this.scrollTrans.gameObject.SetActive(false);
			this.openMenu.gameObject.SetActive(false);
			this.levelLoadUI.gameObject.SetActive(false);
			this.waterCountLabel.gameObject.SetActive(true);
		}
		else
		{
			this.scrollTrans.gameObject.SetActive(true);
			this.levelLoadUI.gameObject.SetActive(true);
			LevelStage.LoadAllObjToEditing(string.Empty);
			this.waterCountLabel.gameObject.SetActive(false);
		}
	}

	private void LoadBtnClick()
	{
		string text = this.levelNumIn.text;
		if (!string.IsNullOrEmpty(text))
		{
			string text2 = EditorData.ReadJsonData(text);
			if (!string.IsNullOrEmpty(text2))
			{
				this.LoadWorld(text2);
				UnityEngine.Debug.Log(string.Format("<color=green>{0}加载成功！</color>", text));
			}
		}
	}

	private void LoadWorld(string jsonStr)
	{
		LevelStage.DestroyWorld(true);
		LevelStage.LoadAllObjToEditing(jsonStr);
		for (int i = 0; i < LevelStage.CurEditStageInst.loadedObj.Count; i++)
		{
			UGUIEvent uGUIEvent = LevelStage.CurEditStageInst.loadedObj[i].gameObject.AddComponent<UGUIEvent>();
			uGUIEvent.onDrag += new Action<PointerEventData, UGUIEvent>(this.OnPuzzleDrag);
			uGUIEvent.onPointerDown += new Action<PointerEventData, UGUIEvent>(this.OnPointDown);
			uGUIEvent.onPointerClick += new Action<PointerEventData, UGUIEvent>(this.OnPointerClick);
			if (LevelStage.CurEditStageInst.loadedObj[i] is Portal)
			{
				Portal portal = LevelStage.CurEditStageInst.loadedObj[i] as Portal;
				UGUIEvent uGUIEvent2 = portal.other.gameObject.AddComponent<UGUIEvent>();
				uGUIEvent2.onDrag += new Action<PointerEventData, UGUIEvent>(this.OnPuzzleDrag);
				uGUIEvent2.onPointerDown += new Action<PointerEventData, UGUIEvent>(this.OnPointDown);
				uGUIEvent2.onPointerClick += new Action<PointerEventData, UGUIEvent>(this.OnPointerClick);
			}
		}
	}

	private void SaveBtnClick()
	{
		string key = this.levelNumIn.text;
		if (!string.IsNullOrEmpty(key))
		{
			EditorData.SaveJsonData(key, LevelStage.CurEditStageInst.GetJson());
			this.PlayOrEdit(true);
			this.editorOrPlay.gameObject.SetActive(false);
			this.waterCountLabel.gameObject.SetActive(false);
			if ((float)Screen.width * 1f / (float)Screen.height < 0.75f)
			{
				QuickHint.Inst.AddContent("截图需要在大于3:4下截图，截图才正确！", QuickHint.PosOffset.Middel);
			}
			for (int i = 0; i < LevelStage.CurStageInst.loadedObj.Count; i++)
			{
				if (LevelStage.CurStageInst.loadedObj[i] is HintObj)
				{
					UnityEngine.Object.Destroy(LevelStage.CurStageInst.loadedObj[i].gameObject);
				}
			}
			this.DelayToDo(0.1f, delegate
			{
				Texture2D tex = Common.Snapshoot(Camera.main, 512, 512);
				this.SaveTex(key, tex);
				this.DelayToDo(0.1f, delegate
				{
					this.PlayOrEdit(false);
					this.editorOrPlay.gameObject.SetActive(true);
				}, false);
			}, false);
		}
		else
		{
			UnityEngine.Debug.Log("<color=red>level key 不能为空！！</color>");
		}
	}

	private void SaveAllBtnClick()
	{
		string text = this.saveAllIn.text;
		if (!string.IsNullOrEmpty(text))
		{
			EditorData.SaveOfficialData(Convert.ToInt32(text));
		}
		else
		{
			QuickHint.Inst.AddContent("请输入要导出的终止关卡ID！", QuickHint.PosOffset.Middel);
			UnityEngine.Debug.Log("<color=red>请输入一个终止ID！！</color>");
		}
	}

	private void SaveTex(string key, Texture2D tex)
	{
		if (!Directory.Exists(EditorData.GetSaveTexturePath()))
		{
			Directory.CreateDirectory(EditorData.GetSaveTexturePath());
		}
		string path = EditorData.GetSaveTexturePath() + "/" + key + ".png";
		File.WriteAllBytes(path, tex.EncodeToPNG());
	}

	private void LevelListBtnClick()
	{
		int startIndex = PlayerPrefs.GetInt("LevelListStart", 1001);
		int num = PlayerPrefs.GetInt("LevelListEnd", 1100);
		if (!string.IsNullOrEmpty(this.levelListStartIn.text))
		{
			startIndex = int.Parse(this.levelListStartIn.text);
			PlayerPrefs.SetInt("LevelListStart", startIndex);
		}
		if (!string.IsNullOrEmpty(this.levelListEndIn.text))
		{
			num = int.Parse(this.levelListEndIn.text);
			PlayerPrefs.SetInt("LevelListEnd", num);
		}
		string text = this.levelNumIn.text;
		if (string.IsNullOrEmpty(text))
		{
			text = startIndex.ToString();
		}
		List<string> list = EditorData.LoadListData(startIndex, num);
		int num2 = int.Parse(text);
		if (num2 > num || num2 < startIndex)
		{
			num2 = startIndex;
		}
		UIManager.OpenWindow<EditorListWindow>(new object[]
		{
			startIndex,
			num,
			num2,
			new Action<string>(delegate(string indexStr)
			{
				this.levelNumIn.text = indexStr;
				int index = int.Parse(indexStr) - startIndex;
				this.LoadWorld(list[index]);
			})
		});
	}

	private void CreateAllEntities()
	{
		IEnumerator enumerator = Enum.GetValues(typeof(ObjType)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ObjType objType = (ObjType)enumerator.Current;
				Transform transform = UnityEngine.Object.Instantiate<Transform>(this.imageTrans, this.scrollContent);
				transform.gameObject.SetActive(true);
				GameObject gameObject = new GameObject();
				gameObject.name = "Point";
				RawImage componentInChildren = transform.GetComponentInChildren<RawImage>();
				if (objType != ObjType.Container && objType != ObjType.Bathtub)
				{
					RenderTexManager.GetInstance().InitForView(gameObject, componentInChildren, default(Vector3), 2f);
				}
				else
				{
					RenderTexManager.GetInstance().InitForView(gameObject, componentInChildren, Vector3.zero, 3.5f);
				}
				Primitives primitives = EntitiesFactory.Create(objType, gameObject.transform, string.Empty);
				transform.name = objType.ToString();
				primitives.Sleep();
				transform.gameObject.AddComponent<UGUIEvent>().onDrag += new Action<PointerEventData, UGUIEvent>(this.OnTemplateDrag);
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

	private void OnTemplateDrag(PointerEventData eventData, UGUIEvent sender)
	{
		if ((eventData.position - eventData.pressPosition).y * this.mPixel2ngui > 64f)
		{
			ObjType objType = (ObjType)Enum.Parse(typeof(ObjType), eventData.pointerPress.name);
			Primitives primitives = EntitiesFactory.Create(objType, LevelStage.CurEditStageInst.GameObjParent, string.Empty);
			LevelStage.CurEditStageInst.AddObjWithEditor(primitives);
			primitives.Sleep();
			eventData.pointerPress.GetComponentInParent<ScrollRect>().OnEndDrag(eventData);
			eventData.pointerPress = primitives.gameObject;
			eventData.pointerDrag = primitives.gameObject;
			UGUIEvent uGUIEvent = primitives.gameObject.AddComponent<UGUIEvent>();
			uGUIEvent.onDrag += new Action<PointerEventData, UGUIEvent>(this.OnPuzzleDrag);
			uGUIEvent.onPointerDown += new Action<PointerEventData, UGUIEvent>(this.OnPointDown);
			uGUIEvent.onPointerClick += new Action<PointerEventData, UGUIEvent>(this.OnPointerClick);
			if (objType == ObjType.Portal)
			{
				Portal portal = primitives as Portal;
				UGUIEvent uGUIEvent2 = portal.other.gameObject.AddComponent<UGUIEvent>();
				uGUIEvent2.onDrag += new Action<PointerEventData, UGUIEvent>(this.OnPuzzleDrag);
				uGUIEvent2.onPointerDown += new Action<PointerEventData, UGUIEvent>(this.OnPointDown);
				uGUIEvent2.onPointerClick += new Action<PointerEventData, UGUIEvent>(this.OnPointerClick);
			}
			else if (objType == ObjType.TriggerObj)
			{
				TriggerObj triggerObj = primitives as TriggerObj;
				UGUIEvent uGUIEvent3 = triggerObj.needTriggerObj.gameObject.AddComponent<UGUIEvent>();
				uGUIEvent3.onDrag += new Action<PointerEventData, UGUIEvent>(this.OnPuzzleDrag);
				uGUIEvent3.onPointerDown += new Action<PointerEventData, UGUIEvent>(this.OnPointDown);
				uGUIEvent3.onPointerClick += new Action<PointerEventData, UGUIEvent>(this.OnPointerClick);
			}
			this.mTouchOffset = Vector2.zero;
			this.OnPuzzleDrag(eventData, uGUIEvent);
		}
	}

	public void OnPointDown(PointerEventData eventData, UGUIEvent sender)
	{
		this.mTouchOffset = sender.transform.localPosition - Utils.ScreenToLocalPos(eventData.position, sender.transform.parent);
	}

	public void OnPuzzleDrag(PointerEventData eventData, UGUIEvent sender)
	{
		Vector2 a = sender.transform.parent.worldToLocalMatrix.MultiplyPoint(Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, -Camera.main.transform.position.z)));
		sender.GetComponent<Primitives>().SetOriginalPos(a + this.mTouchOffset);
	}

	public void OnPointerClick(PointerEventData eventData, UGUIEvent sender)
	{
		if (eventData.dragging)
		{
			return;
		}
		UIManager.OpenWindow<EditorTransformBorder>(new object[]
		{
			sender.GetComponent<Primitives>()
		});
		this.scrollTrans.gameObject.SetActive(false);
		this.openMenu.gameObject.SetActive(true);
		this.levelLoadUI.gameObject.SetActive(false);
	}

	private void MenuDrag(PointerEventData eventData, UGUIEvent sender)
	{
		Vector2 v = UIManager.GetInst(false).m_UICamera.ScreenToWorldPoint(eventData.position);
		this.openMenu.localPosition = this.openMenu.parent.worldToLocalMatrix.MultiplyPoint(v);
	}
}
