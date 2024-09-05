using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HintView : BaseView
{
	private sealed class _HintShow_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _preTime___0;

		internal int _i___1;

		internal string[] _hintItem___2;

		internal float _time___2;

		internal Vector2 _pos___2;

		internal Vector3 _p___2;

		internal HintView _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _HintShow_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._preTime___0 = 0f;
				this._i___1 = 0;
				break;
			case 1u:
				this._preTime___0 = this._time___2;
				this._this.SetClick(this._pos___2);
				Time.timeScale = 0f;
				this._this.finger.gameObject.SetActive(true);
				this._p___2 = UIManager.GetInst(false).m_UICamera.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(this._pos___2));
				this._this.finger.transform.localPosition = this._this.transform.worldToLocalMatrix.MultiplyPoint(this._p___2);
				this._i___1++;
				break;
			default:
				return false;
			}
			if (this._i___1 < this._this.hintArr.Length)
			{
				this._hintItem___2 = this._this.hintArr[this._i___1].Split(new char[]
				{
					':'
				});
				this._time___2 = float.Parse(this._hintItem___2[0]);
				this._pos___2 = this._this.ToVector2(this._hintItem___2[1]);
				this._current = new WaitForSeconds(this._time___2 - this._preTime___0);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	private RectTransform finger;

	private RectTransform fingerImage;

	private string[] hintArr;

	private void Awake()
	{
		this.finger = base.Find<RectTransform>(base.transform, "finger");
		this.fingerImage = base.Find<RectTransform>(this.finger, "finger");
		DOTween.To(() => this.fingerImage.anchoredPosition, delegate(Vector2 v)
		{
			this.fingerImage.anchoredPosition = v;
		}, new Vector2(0f, 0f), 0.5f).SetEase(Ease.InOutBack).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
	}

	public override void Init(params object[] args)
	{
		if (LevelStage.CurStageInst != null)
		{
			if (LevelStage.CurStageInst.HintStr.IsNullOrEntry())
			{
				UnityEngine.Debug.LogError("该关没有提示！");
				return;
			}
			for (int i = 0; i < LevelStage.CurStageInst.InteractObjs.Count; i++)
			{
				LevelStage.CurStageInst.InteractObjs[i].hintClick = true;
			}
			this.hintArr = LevelStage.CurStageInst.HintStr.Split(new char[]
			{
				'+'
			});
			base.StartCoroutine(this.HintShow());
		}
	}

	private IEnumerator HintShow()
	{
		HintView._HintShow_c__Iterator0 _HintShow_c__Iterator = new HintView._HintShow_c__Iterator0();
		_HintShow_c__Iterator._this = this;
		return _HintShow_c__Iterator;
	}

	private void SetClick(Vector2 pos)
	{
		InteractObj interactObj = null;
		float num = 3.40282347E+38f;
		for (int i = 0; i < LevelStage.CurStageInst.InteractObjs.Count; i++)
		{
			InteractObj interactObj2 = LevelStage.CurStageInst.InteractObjs[i];
			if (interactObj2 != null && interactObj2.gameObject != null)
			{
				float num2 = Vector2.Distance(interactObj2.mTrans.position, pos);
				if (num > num2)
				{
					interactObj = interactObj2;
					num = num2;
				}
			}
		}
		if (interactObj != null && interactObj.gameObject != null)
		{
			interactObj.ObjClickEvent += delegate(Vector2 v)
			{
				Time.timeScale = 1f;
				this.finger.gameObject.SetActive(false);
			};
			interactObj.hintClick = false;
		}
	}

	public override void Close()
	{
		base.Close();
		Time.timeScale = 1f;
	}

	private Vector2 ToVector2(string str)
	{
		string text = str.Substring(1, str.Length - 2);
		string[] array = text.Split(new char[]
		{
			','
		});
		Vector2 result = default(Vector2);
		if (array.Length >= 2)
		{
			result.x = float.Parse(array[0]);
			result.y = float.Parse(array[1]);
		}
		return result;
	}
}
