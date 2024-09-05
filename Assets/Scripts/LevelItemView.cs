using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelItemView : MonoBehaviour
{
	private RawImage rawImage;

	private Text levelNum;

	private Button btn;

	private LevelData curData;

	private Transform lockTrans;

	private Transform starsTrans;

	private Transform[] stars;

	private Image bgSprite;

	private void Awake()
	{
		this.bgSprite = Utils.Find<Image>(base.transform, "sp");
		this.rawImage = Utils.Find<RawImage>(base.transform, "mask/Icon");
		this.levelNum = Utils.Find<Text>(base.transform, "tx_level");
		this.lockTrans = base.transform.Find("NoGarde");
		this.starsTrans = base.transform.Find("StarControl");
		this.btn = Utils.Find<Button>(base.transform, "Select");
		this.stars = new Transform[3];
		for (int i = 0; i < 3; i++)
		{
			this.stars[i] = this.starsTrans.Find("star" + i + "/star");
		}
	}

	public void Init(LevelData levelData)
	{
		this.curData = levelData;
		this.InitOther();
		this.btn.onClick.AddListener(delegate
		{
			if (this.curData.IsLock())
			{
				UIManager.OpenWindow<HinttipsView>(new object[]
				{
					Utils.GetString("@levelLock", new object[0])
				});
				return;
			}
			UIManager.GetInst(false).ShowBlack(delegate
			{
				LevelStage.Create(this.curData);
				UIManager.OpenWindow<LevelUIView>(new object[]
				{
					this.curData,
					LevelUIView.StartType.LevelList
				});
				UIManager.GetInst(false).GetOpenWindow<LevelListView>().Close();
			}, null, null);
		});
	}

	public void UpdateItem(LevelData levelData)
	{
		this.curData = levelData;
		this.InitOther();
	}

	private void InitOther()
	{
		this.rawImage.texture = Resources.Load<Texture>("LevelConfig/LevelTexture/" + this.curData.key);
		this.levelNum.SetString("@level", new object[]
		{
			this.curData.baseData.index.ToString()
		});
		if (this.curData.IsLock())
		{
			this.lockTrans.gameObject.SetActive(true);
			this.bgSprite.color = Utils.UIGrayColor;
			this.rawImage.color = Utils.UIGrayColor;
			for (int i = 0; i < 3; i++)
			{
				this.stars[i].gameObject.SetActive(false);
			}
		}
		else
		{
			this.lockTrans.gameObject.SetActive(false);
			this.bgSprite.color = Color.white;
			this.rawImage.color = Color.white;
			for (int j = 0; j < 3; j++)
			{
				if (this.curData.passGrade > j)
				{
					this.stars[j].gameObject.SetActive(true);
				}
				else
				{
					this.stars[j].gameObject.SetActive(false);
				}
			}
		}
	}
}
