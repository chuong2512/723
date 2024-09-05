using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChapterView : BaseView
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

	private List<ChapterTemplate> chapterList;

	private List<ChapterItemView> itemList = new List<ChapterItemView>();

	private void Awake()
	{
		this.mTransform = base.transform;
		this.scrollList = this.mTransform.Find("Panel/Scroll View/Viewport/Content").GetComponent<ListScrollRect>();
		this.allstarText = base.Find<Text>(base.transform, "Top/StarNum/Text");
		this.selectItem = this.mTransform.Find("GardeModel");
		this.homeBtn = base.Find<Button>(base.transform, "Top/btn_back");
		this.chapterTrans = base.transform.Find("Chapter");
		this.soonTrans = base.transform.Find("Soon");
		this.homeBtn.onClick.AddListener(delegate
		{
			UIManager.OpenWindow<HomeView>(new object[0]);
			this.Close();
		});
		this.OpeniPhoneX();
	}

	public override void Init(params object[] args)
	{
		this.chapterList = ChapterTemplate.Dic().Values.ToList<ChapterTemplate>();
		ListScrollRect expr_1B = this.scrollList;
		expr_1B.ED_UpdateItem = (ListScrollRect.EventUpdateItem)Delegate.Combine(expr_1B.ED_UpdateItem, new ListScrollRect.EventUpdateItem(this.UpdateItem));
		this.scrollList.ListViewTranCount(this.chapterList.Count, delegate(int i, int dataIndex)
		{
			Transform transform = UnityEngine.Object.Instantiate<Transform>(this.selectItem, this.scrollList.transform);
			transform.gameObject.SetActive(true);
			ChapterItemView chapterItemView = transform.gameObject.AddComponent<ChapterItemView>();
			chapterItemView.Init(this.chapterList[dataIndex]);
			this.itemList.Add(chapterItemView);
			return transform;
		}, 0, null);
		this.allstarText.text = UserModel.Inst.GetStarCount() + "/" + LevelTemplate.Dic().Count * 3;
	}

	private void UpdateItem(int i, int dataIndex)
	{
		if (i >= this.itemList.Count)
		{
			return;
		}
		ChapterTemplate chapterTemplate = this.chapterList[dataIndex];
		this.itemList[i].UpdateItem(chapterTemplate);
	}

	private new void OpeniPhoneX()
	{
		if ((double)((float)Screen.height * 1f / (float)Screen.width) * 1.0 > 2.0999999046325684)
		{
			base.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0f, -88f);
		}
	}
}
