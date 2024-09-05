using System;
using UnityEngine;

public class QuickHint : BaseView
{
	public enum PosOffset
	{
		Top,
		Middel
	}

	private static QuickHint slef;

	private QuickHintItem last;

	private GameObject ccBoard;

	private Transform cccontentTransform;

	public static QuickHint Inst
	{
		get
		{
			if (QuickHint.slef == null)
			{
				QuickHint.slef = UIManager.OpenWindow<QuickHint>(new object[0]);
			}
			return QuickHint.slef;
		}
	}

	private void Awake()
	{
		this.cccontentTransform = base.transform.Find("content");
		this.ccBoard = this.cccontentTransform.Find("Board").gameObject;
	}

	public QuickHintItem AddContent(string content, QuickHint.PosOffset offse = QuickHint.PosOffset.Middel)
	{
		RectTransform component = this.cccontentTransform.GetComponent<RectTransform>();
		if (offse != QuickHint.PosOffset.Middel)
		{
			if (offse == QuickHint.PosOffset.Top)
			{
				this.cccontentTransform.GetComponent<RectTransform>();
				component.anchorMin = new Vector2(0f, 1f);
				component.anchorMax = new Vector2(1f, 1f);
				component.anchoredPosition = new Vector2(0f, -120f);
			}
		}
		else
		{
			component.anchorMin = new Vector2(0f, 0.5f);
			component.anchorMax = new Vector2(1f, 0.5f);
			component.anchoredPosition = new Vector2(0f, 20f);
		}
		if (this.last != null && this.last.text.text == content && !this.last.IsDestroy)
		{
			this.last.Reset();
			return this.last;
		}
		this.last = QuickHintItem.Create(this.ccBoard, this.cccontentTransform, this.last, content);
		QuickHint.slef.transform.SetAsLastSibling();
		return this.last;
	}

	public static void SetVisible(bool able)
	{
		if (QuickHint.slef == null)
		{
			return;
		}
		QuickHint.slef.gameObject.SetActive(able);
	}

	private void OnDestroy()
	{
		this.last = null;
		QuickHint.slef = null;
	}
}
