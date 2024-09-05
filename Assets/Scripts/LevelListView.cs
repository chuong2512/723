using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelListView : BaseView
{
	private ListScrollRect scrollList;

	private Transform selectItem;

	private Transform mTransform;

	private Button homeBtn;

	private Transform soonTrans;

	private int openChatperCount;

	private string curLevelKey;

	private Text starNumText;

	private Transform chapterTrans;

	private Text allstarText;

	private List<LevelData> datas;

	private Text chapterName;

	private Text chapterStarNum;

	private List<LevelItemView> itemList = new List<LevelItemView>();

	private void Awake()
	{
		this.mTransform = base.transform;
		this.scrollList = this.mTransform.Find("Panel/Scroll View/Viewport/Content").GetComponent<ListScrollRect>();
		this.allstarText = base.Find<Text>(base.transform, "Top/StarNum/Text");
		this.selectItem = this.mTransform.Find("GardeModel");
		this.homeBtn = base.Find<Button>(base.transform, "Top/btn_back");
		this.chapterTrans = base.transform.Find("Chapter");
		this.chapterName = base.Find<Text>(this.chapterTrans, "tx_chapter");
		this.chapterStarNum = base.Find<Text>(this.chapterTrans, "tx_star");
		this.soonTrans = base.transform.Find("Soon");
		this.homeBtn.onClick.AddListener(delegate
		{
			UIManager.OpenWindow<ChapterView>(new object[0]);
			this.Close();
		});
		this.OpeniPhoneX();
	}

	public override void Init(params object[] args)
	{
		ChapterTemplate chapterTemplate = null;
		if (args.Length >= 1)
		{
			chapterTemplate = (args[0] as ChapterTemplate);
		}
		this.datas = ChapterModel.GetChapterAllLevel(chapterTemplate);
		ListScrollRect expr_26 = this.scrollList;
		expr_26.ED_UpdateItem = (ListScrollRect.EventUpdateItem)Delegate.Combine(expr_26.ED_UpdateItem, new ListScrollRect.EventUpdateItem(this.UpdateItem));
		this.scrollList.ListViewTranCount(this.datas.Count, delegate(int i, int dataIndex)
		{
			Transform transform = UnityEngine.Object.Instantiate<Transform>(this.selectItem, this.scrollList.transform);
			transform.gameObject.SetActive(true);
			LevelItemView levelItemView = transform.gameObject.AddComponent<LevelItemView>();
			levelItemView.Init(this.datas[dataIndex]);
			this.itemList.Add(levelItemView);
			return transform;
		}, 0, null);
		this.allstarText.text = UserModel.Inst.GetStarCount() + "/" + LevelTemplate.Dic().Count * 3;
		this.chapterName.text = chapterTemplate.name;
		this.chapterStarNum.text = ChapterModel.AllLevelStarNum(chapterTemplate.key) + "/" + chapterTemplate.num * 3;
	}

	private void UpdateItem(int i, int dataIndex)
	{
		if (i >= this.itemList.Count)
		{
			return;
		}
		LevelData levelData = this.datas[dataIndex];
		this.itemList[i].UpdateItem(levelData);
	}

	private new void OpeniPhoneX()
	{
		if ((double)((float)Screen.height * 1f / (float)Screen.width) * 1.0 > 2.0999999046325684)
		{
			base.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0f, -88f);
		}
	}

	private void OnDestroy()
	{
		Resources.UnloadUnusedAssets();
		GC.Collect();
	}
}
