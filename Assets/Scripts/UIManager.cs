using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	private sealed class _ShowBlack_c__AnonStorey0
	{
		internal Action call;

		internal Action startCall;

		internal Action transEndCall;

		internal UIManager _this;

		internal Color __m__0()
		{
			return this._this.transitionImage.color;
		}

		internal void __m__1(Color a)
		{
			this._this.transitionImage.color = a;
		}

		internal void __m__2()
		{
			Time.timeScale = 1f;
			if (this.call != null)
			{
				this.call();
			}
			DOTween.ToAlpha(() => this._this.transitionImage.color, delegate(Color a)
			{
				this._this.transitionImage.color = a;
			}, 0f, 0.25f).SetUpdate(true).SetDelay(0.2f).OnStart(delegate
			{
				if (this.startCall != null)
				{
					this.startCall();
				}
			}).OnComplete(delegate
			{
				if (this.transEndCall != null)
				{
					this.transEndCall();
				}
				this._this.transitionImage.gameObject.SetActive(false);
			});
		}

		internal Color __m__3()
		{
			return this._this.transitionImage.color;
		}

		internal void __m__4(Color a)
		{
			this._this.transitionImage.color = a;
		}

		internal void __m__5()
		{
			if (this.startCall != null)
			{
				this.startCall();
			}
		}

		internal void __m__6()
		{
			if (this.transEndCall != null)
			{
				this.transEndCall();
			}
			this._this.transitionImage.gameObject.SetActive(false);
		}
	}

	private static UIManager Inst;

	[HideInInspector]
	public Camera m_UICamera;

	[HideInInspector]
	public Transform windowRoot;

	private List<BaseView> viewList = new List<BaseView>();

	public UGUIEvent TouchEvent;

	public Canvas MainCnvas;

	public Image transitionImage;

	private static Material _UIGrayMat;

	public static Material UIGrayMat
	{
		get
		{
			if (UIManager._UIGrayMat == null)
			{
				UIManager._UIGrayMat = Resources.Load<Material>("Materials/UIGray");
			}
			return UIManager._UIGrayMat;
		}
	}

	public static UIManager GetInst(bool isOnDestroy = false)
	{
		if (isOnDestroy)
		{
			return UIManager.Inst;
		}
		if (UIManager.Inst == null)
		{
			UIManager.Inst = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("UI/Prefab/UIRoot")).GetComponent<UIManager>();
		}
		return UIManager.Inst;
	}

	private void Awake()
	{
		UIManager.Inst = this;
		this.windowRoot = base.transform.Find("#UpLayer/WindowRoot");
		this.m_UICamera = base.transform.Find("UICamera").GetComponent<Camera>();
		this.transitionImage = Utils.Find<Image>(base.transform, "#UpLayer/Transition");
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public static T OpenWindow<T>(params object[] args) where T : BaseView
	{
		if (UIManager.Inst == null)
		{
			UIManager.GetInst(false);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("UI/Prefab/" + typeof(T).Name), UIManager.Inst.windowRoot);
		Text[] componentsInChildren = gameObject.GetComponentsInChildren<Text>(true);
		Text[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Text text = array[i];
			if (text.name.StartsWith("@"))
			{
				text.SetString(text.name, new object[0]);
			}
		}
		BaseView baseView = gameObject.GetComponent<T>();
		UIManager.Inst.viewList.Add(baseView);
		baseView.Init(args);
		return (T)((object)baseView);
	}

	public void ShowBlack(Action call, Action startCall = null, Action transEndCall = null)
	{
		this.transitionImage.gameObject.SetActive(true);
		Time.timeScale = 0f;
		DOTween.ToAlpha(() => this.transitionImage.color, delegate(Color a)
		{
			this.transitionImage.color = a;
		}, 1f, 0.35f).SetUpdate(true).OnComplete(delegate
		{
			Time.timeScale = 1f;
			if (call != null)
			{
				call();
			}
			DOTween.ToAlpha(() => this.transitionImage.color, delegate(Color a)
			{
				this.transitionImage.color = a;
			}, 0f, 0.25f).SetUpdate(true).SetDelay(0.2f).OnStart(delegate
			{
				if (startCall != null)
				{
					startCall();
				}
			}).OnComplete(delegate
			{
				if (transEndCall != null)
				{
					transEndCall();
				}
				this.transitionImage.gameObject.SetActive(false);
			});
		});
	}

	public T GetOpenWindow<T>() where T : BaseView
	{
		return (T)((object)this.viewList.Find((BaseView o) => o.GetType() == typeof(T)));
	}

	public bool CloseOpenedWindow<T>() where T : BaseView
	{
		BaseView baseView = (T)((object)this.viewList.Find((BaseView o) => o.GetType() == typeof(T)));
		if (baseView)
		{
			baseView.Close();
			return true;
		}
		return false;
	}

	public void CloseAllWindow()
	{
		for (int i = this.viewList.Count - 1; i >= 0; i--)
		{
			this.viewList[i].Close();
		}
	}

	public void RemoveView(BaseView view)
	{
		this.viewList.Remove(view);
	}
}
