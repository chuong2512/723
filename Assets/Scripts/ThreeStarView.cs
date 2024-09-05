using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class ThreeStarView : BaseView
{
	private Button collectBtn;

	private Button closeBtn;

	private LevelData data;

	private void Awake()
	{
		this.closeBtn = base.Find<Button>(base.transform, "NoBtn");
		this.collectBtn = base.Find<Button>(base.transform, "collectBtn");
	}

	public override void Init(params object[] args)
	{
		base.Init(args);
		if (args.Length >= 1)
		{
			this.data = (args[0] as LevelData);
		}
		for (int i = 0; i < this.data.starNum; i++)
		{
			base.transform.Find(string.Concat(new object[]
			{
				"StarControl/star",
				i,
				"/star",
				i
			})).gameObject.SetActive(true);
		}
		this.closeBtn.onClick.AddListener(new UnityAction(this.Close));
		this.collectBtn.onClick.AddListener(new UnityAction(this.CollectBtnClick));
	}

	private void CollectBtnClick()
	{
		ADSManager.PlayVideo(ADSManager.VedioType.ThreeStar, delegate
		{
			this.data.starNum = 3;
			this.Close();
		});
	}
}
