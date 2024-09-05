using System;
using UnityEngine;

public class BaseView : MonoBehaviour
{
	public Action CloseCallBack;

	protected virtual void Start()
	{
		Canvas component = base.gameObject.GetComponent<Canvas>();
		Transform transform = UIManager.GetInst(false).windowRoot.Find("fang");
		if (transform != null)
		{
			if (component != null)
			{
				transform.gameObject.SetActive(true);
			}
			else
			{
				transform.gameObject.SetActive(false);
			}
		}
	}

	public virtual void Init(params object[] args)
	{
	}

	public T Find<T>(Transform parent, string namePath) where T : Component
	{
		Transform transform = parent.Find(namePath);
		if (transform != null)
		{
			return transform.GetComponent<T>();
		}
		return (T)((object)null);
	}

	public virtual void Close()
	{
		if (this.CloseCallBack != null)
		{
			this.CloseCallBack();
		}
		UIManager.GetInst(false).RemoveView(this);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public virtual void DelayClose(float delayTime = 1f)
	{
		if (this.CloseCallBack != null)
		{
			this.CloseCallBack();
		}
		UIManager.GetInst(false).RemoveView(this);
		UnityEngine.Object.Destroy(base.gameObject, delayTime);
	}

	public void OpeniPhoneX()
	{
	}
}
