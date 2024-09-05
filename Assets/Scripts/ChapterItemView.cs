using System;
using UnityEngine;
using UnityEngine.UI;

public class ChapterItemView : MonoBehaviour
{
	private RawImage rawImage;

	private Text chapterName;

	private Button btn;

	private ChapterTemplate curData;

	private Transform lockTrans;

	private Image bgSprite;

	private Image starSprite;

	private Text levelCountNum;

	private Text starNum;

	private void Awake()
	{
		this.bgSprite = Utils.Find<Image>(base.transform, "sp");
		this.starSprite = Utils.Find<Image>(base.transform, "StarNum");
		this.rawImage = Utils.Find<RawImage>(base.transform, "mask/Icon");
		this.chapterName = Utils.Find<Text>(base.transform, "NameDi/chapterName");
		this.levelCountNum = Utils.Find<Text>(base.transform, "NameDi/level");
		this.lockTrans = base.transform.Find("NoGarde");
		this.starNum = Utils.Find<Text>(base.transform, "StarNum/Text");
		this.btn = Utils.Find<Button>(base.transform, "Select");
	}

	public void Init(ChapterTemplate chapterTemplate)
	{
		this.curData = chapterTemplate;
		this.InitOther();
		this.btn.onClick.AddListener(delegate
		{
			int num = ChapterModel.ChapterIsOpen(this.curData.key);
			if (num > 0)
			{
				UIManager.OpenWindow<HinttipsView>(new object[]
				{
					this.curData.lockStarNum
				});
				return;
			}
			UIManager.GetInst(false).CloseOpenedWindow<ChapterView>();
			UIManager.OpenWindow<LevelListView>(new object[]
			{
				this.curData
			});
		});
	}

	public void UpdateItem(ChapterTemplate chapterTemplate)
	{
		this.curData = chapterTemplate;
		this.InitOther();
	}

	private void InitOther()
	{
		this.rawImage.texture = Resources.Load<Texture>("LevelConfig/LevelTexture/" + this.curData.startLevel);
		this.chapterName.text = this.curData.name;
		int num = int.Parse(this.curData.startLevel) - 1000;
		this.levelCountNum.text = num + " - " + (num + this.curData.num - 1);
		this.starNum.text = ChapterModel.AllLevelStarNum(this.curData.key) + "/" + this.curData.num * 3;
		if (ChapterModel.ChapterIsOpen(this.curData.key) > 0)
		{
			this.rawImage.color = Utils.UIGrayColor;
			this.bgSprite.color = Utils.UIGrayColor;
			this.starSprite.color = Utils.UIGrayColor;
			this.lockTrans.gameObject.SetActive(true);
		}
		else
		{
			this.rawImage.color = Color.white;
			this.bgSprite.color = Color.white;
			this.starSprite.color = Color.white;
			this.lockTrans.gameObject.SetActive(false);
		}
	}
}
