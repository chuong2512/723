using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HinttipsView : BaseView
{
	private Text text;

	private Transform content;

	private Transform contentChapter;

	private void Awake()
	{
		this.content = base.transform.Find("Content");
		this.contentChapter = base.transform.Find("ChapterContent");
		this.text = base.Find<Text>(this.content, "Text");
		base.Find<Button>(base.transform, "Btn_ok").onClick.AddListener(new UnityAction(this.Close));
	}

	public override void Init(params object[] args)
	{
		if (args.Length >= 1)
		{
			if (args[0] is int)
			{
				this.content.gameObject.SetActive(false);
				this.contentChapter.gameObject.SetActive(true);
				base.Find<Text>(this.contentChapter, "Star/StarNUm").text = args[0].ToString();
			}
			else
			{
				this.text.text = (string)args[0];
			}
		}
		else
		{
			this.text.gameObject.SetActive(false);
		}
	}
}
