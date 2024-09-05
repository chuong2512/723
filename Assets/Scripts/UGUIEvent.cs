using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine.EventSystems;

public class UGUIEvent : EventTrigger
{
	public delegate void EventParam<T>(T t);



































	public event UGUIEvent.EventParam<PointerEventData> onBeginDrag;

	public event UGUIEvent.EventParam<BaseEventData> onCancel;

	public event UGUIEvent.EventParam<BaseEventData> onDeselect;

	public event Action<PointerEventData, UGUIEvent> onDrag;

	public event UGUIEvent.EventParam<PointerEventData> onDrop;

	public event UGUIEvent.EventParam<PointerEventData> onEndDrag;

	public event UGUIEvent.EventParam<PointerEventData> onInitializePotentialDrag;

	public event UGUIEvent.EventParam<AxisEventData> onMove;

	public event Action<PointerEventData, UGUIEvent> onPointerClick;

	public event Action<PointerEventData, UGUIEvent> onPointerDown;

	public event UGUIEvent.EventParam<PointerEventData> onPointerEnter;

	public event UGUIEvent.EventParam<PointerEventData> onPointerExit;

	public event Action<PointerEventData, UGUIEvent> onPointerUp;

	public event UGUIEvent.EventParam<PointerEventData> onScroll;

	public event UGUIEvent.EventParam<BaseEventData> onSelect;

	public event UGUIEvent.EventParam<BaseEventData> onSubmit;

	public event UGUIEvent.EventParam<BaseEventData> onUpdateSelected;

	public override void OnBeginDrag(PointerEventData eventData)
	{
		base.OnBeginDrag(eventData);
		if (this.onBeginDrag != null)
		{
			this.onBeginDrag(eventData);
		}
	}

	public override void OnCancel(BaseEventData eventData)
	{
		base.OnCancel(eventData);
		if (this.onCancel != null)
		{
			this.onCancel(eventData);
		}
	}

	public override void OnDeselect(BaseEventData eventData)
	{
		base.OnDeselect(eventData);
		if (this.onDeselect != null)
		{
			this.onDeselect(eventData);
		}
	}

	public override void OnDrag(PointerEventData eventData)
	{
		base.OnDrag(eventData);
		if (this.onDrag != null)
		{
			this.onDrag(eventData, this);
		}
	}

	public override void OnDrop(PointerEventData eventData)
	{
		base.OnDrop(eventData);
		if (this.onDrop != null)
		{
			this.onDrop(eventData);
		}
	}

	public override void OnEndDrag(PointerEventData eventData)
	{
		base.OnEndDrag(eventData);
		if (this.onEndDrag != null)
		{
			this.onEndDrag(eventData);
		}
	}

	public override void OnInitializePotentialDrag(PointerEventData eventData)
	{
		base.OnInitializePotentialDrag(eventData);
		if (this.onInitializePotentialDrag != null)
		{
			this.onInitializePotentialDrag(eventData);
		}
	}

	public override void OnMove(AxisEventData eventData)
	{
		base.OnMove(eventData);
		if (this.onMove != null)
		{
			this.onMove(eventData);
		}
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		base.OnPointerClick(eventData);
		if (this.onPointerClick != null)
		{
			this.onPointerClick(eventData, this);
		}
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);
		if (this.onPointerDown != null)
		{
			this.onPointerDown(eventData, this);
		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		if (this.onPointerEnter != null)
		{
			this.onPointerEnter(eventData);
		}
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		if (this.onPointerExit != null)
		{
			this.onPointerExit(eventData);
		}
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		if (this.onPointerUp != null)
		{
			this.onPointerUp(eventData, this);
		}
	}

	public override void OnScroll(PointerEventData eventData)
	{
		base.OnScroll(eventData);
		if (this.onScroll != null)
		{
			this.onScroll(eventData);
		}
	}

	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		if (this.onSelect != null)
		{
			this.onSelect(eventData);
		}
	}

	public override void OnSubmit(BaseEventData eventData)
	{
		base.OnSubmit(eventData);
		if (this.onSubmit != null)
		{
			this.onSubmit(eventData);
		}
	}

	public override void OnUpdateSelected(BaseEventData eventData)
	{
		base.OnUpdateSelected(eventData);
		if (this.onUpdateSelected != null)
		{
			this.onUpdateSelected(eventData);
		}
	}
}
