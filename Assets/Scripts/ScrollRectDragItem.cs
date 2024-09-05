using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectDragItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IEventSystemHandler
{
	private ScrollRect mRootScrollRect;

	private ScrollRect RootScrollRect
	{
		get
		{
			if (!this.mRootScrollRect)
			{
				this.mRootScrollRect = base.GetComponentInParent<ScrollRect>();
			}
			return this.mRootScrollRect;
		}
	}

	public void OnPointerDown(PointerEventData ped)
	{
		if (this.RootScrollRect)
		{
			this.RootScrollRect.OnBeginDrag(ped);
		}
	}

	public void OnBeginDrag(PointerEventData ped)
	{
		if (this.RootScrollRect)
		{
			this.RootScrollRect.OnBeginDrag(ped);
		}
	}

	public void OnDrag(PointerEventData ped)
	{
		if (this.RootScrollRect)
		{
			this.RootScrollRect.OnDrag(ped);
		}
	}

	public void OnEndDrag(PointerEventData ped)
	{
		if (this.RootScrollRect)
		{
			this.RootScrollRect.OnEndDrag(ped);
		}
	}
}
