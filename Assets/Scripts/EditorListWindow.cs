using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EditorListWindow : BaseView
{
	private sealed class _Init_c__AnonStorey0
	{
		internal Action<string> callback;

		internal EditorListWindow _this;

		internal Transform __m__0(int i, int dataIndex)
		{
			Transform transform = UnityEngine.Object.Instantiate<Transform>(this._this.selectItem, this._this.scrollList.transform);
			transform.gameObject.SetActive(true);
			EditLevelItemView editLevelItemView = transform.gameObject.AddComponent<EditLevelItemView>();
			editLevelItemView.Init(this._this.startId + dataIndex, this.callback);
			this._this.itemList.Add(editLevelItemView);
			return transform;
		}
	}

	private ListScrollRect scrollList;

	private Transform selectItem;

	private Transform mTransform;

	private Button close;

	private int openChatperCount;

	private string curLevelKey;

	private Transform chapterTrans;

	private Button sureBtn;

	private InputField pathIn;

	private int startId;

	private List<EditLevelItemView> itemList = new List<EditLevelItemView>();

	private void Awake()
	{
		this.mTransform = base.transform;
		this.scrollList = this.mTransform.Find("ScrollView/Viewport/Content").GetComponent<ListScrollRect>();
		this.sureBtn = base.Find<Button>(base.transform, "SureBtn");
		this.pathIn = base.Find<InputField>(base.transform, "InputField");
		this.selectItem = this.mTransform.Find("RawImage");
		this.close = base.Find<Button>(base.transform, "Close");
		this.close.onClick.AddListener(new UnityAction(this.Close));
		this.pathIn.text = EditorScene.Inst.levelFilePath;
		this.sureBtn.onClick.AddListener(new UnityAction(this.ChangePath));
	}

	private void ChangePath()
	{
		if (!this.pathIn.text.IsNullOrEntry() && this.pathIn.text != EditorScene.Inst.levelFilePath)
		{
			EditorScene.Inst.levelFilePath = this.pathIn.text;
			this.scrollList.UpdateShow();
		}
	}

	public override void Init(params object[] args)
	{
		this.startId = (int)args[0];
		int num = (int)args[1];
		int num2 = (int)args[2];
		Action<string> callback = args[3] as Action<string>;
		ListScrollRect expr_41 = this.scrollList;
		expr_41.ED_UpdateItem = (ListScrollRect.EventUpdateItem)Delegate.Combine(expr_41.ED_UpdateItem, new ListScrollRect.EventUpdateItem(this.UpdateItem));
		this.scrollList.ListViewTranCount(num - this.startId + 1, delegate(int i, int dataIndex)
		{
			Transform transform = UnityEngine.Object.Instantiate<Transform>(this.selectItem, this.scrollList.transform);
			transform.gameObject.SetActive(true);
			EditLevelItemView editLevelItemView = transform.gameObject.AddComponent<EditLevelItemView>();
			editLevelItemView.Init(this.startId + dataIndex, callback);
			this.itemList.Add(editLevelItemView);
			return transform;
		}, 0, null);
		RectTransform component = this.scrollList.GetComponent<RectTransform>();
		float y = this.scrollList.GetAnchoredPositionByDataIndex(num2 - this.startId).y;
		component.anchoredPosition = new Vector2(component.anchoredPosition.x, y);
		this.scrollList.UpdateShow();
	}

	private void UpdateItem(int i, int dataIndex)
	{
		if (i >= this.itemList.Count)
		{
			return;
		}
		this.itemList[i].UpdateItem(this.startId + dataIndex);
	}
}
