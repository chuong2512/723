using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public static class StaticUtils
{
	private sealed class _DelayToInvokeDo_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float delaySeconds;

		internal Action action;

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

		public _DelayToInvokeDo_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = new WaitForSeconds(this.delaySeconds);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				this.action();
				this._PC = -1;
				break;
			}
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

	private sealed class _DelayIgnoreTimeToDo_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _start___0;

		internal float delaySeconds;

		internal Action action;

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

		public _DelayIgnoreTimeToDo_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._start___0 = Time.realtimeSinceStartup;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (Time.realtimeSinceStartup < this._start___0 + this.delaySeconds)
			{
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this.action();
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

	public static Coroutine DelayToDo(this MonoBehaviour mono, float delayTime, Action action, bool ignoreTimeScale = false)
	{
		Coroutine result;
		if (ignoreTimeScale)
		{
			result = mono.StartCoroutine(StaticUtils.DelayIgnoreTimeToDo(delayTime, action));
		}
		else
		{
			result = mono.StartCoroutine(StaticUtils.DelayToInvokeDo(delayTime, action));
		}
		return result;
	}

	public static IEnumerator DelayToInvokeDo(float delaySeconds, Action action)
	{
		StaticUtils._DelayToInvokeDo_c__Iterator0 _DelayToInvokeDo_c__Iterator = new StaticUtils._DelayToInvokeDo_c__Iterator0();
		_DelayToInvokeDo_c__Iterator.delaySeconds = delaySeconds;
		_DelayToInvokeDo_c__Iterator.action = action;
		return _DelayToInvokeDo_c__Iterator;
	}

	public static IEnumerator DelayIgnoreTimeToDo(float delaySeconds, Action action)
	{
		StaticUtils._DelayIgnoreTimeToDo_c__Iterator1 _DelayIgnoreTimeToDo_c__Iterator = new StaticUtils._DelayIgnoreTimeToDo_c__Iterator1();
		_DelayIgnoreTimeToDo_c__Iterator.delaySeconds = delaySeconds;
		_DelayIgnoreTimeToDo_c__Iterator.action = action;
		return _DelayIgnoreTimeToDo_c__Iterator;
	}

	public static bool IsNullOrEntry(this string str)
	{
		return string.IsNullOrEmpty(str);
	}

	public static Vector3 ToVector3(this string str)
	{
		Vector3 zero = Vector3.zero;
		string[] array = str.Split(new char[]
		{
			','
		});
		if (array.Length >= 2)
		{
			zero.x = float.Parse(array[0]);
			zero.y = float.Parse(array[1]);
			if (array.Length >= 3)
			{
				zero.z = float.Parse(array[2]);
			}
		}
		return zero;
	}

	public static void SetLayerIncludeChild(this GameObject go, int layer)
	{
		go.layer = layer;
		go.transform.SetChildLayer(layer);
	}

	public static void SetChildLayer(this Transform t, int layer)
	{
		for (int i = 0; i < t.childCount; i++)
		{
			Transform child = t.GetChild(i);
			child.gameObject.layer = layer;
			child.SetChildLayer(layer);
		}
	}

	public static string SetString(this Text text, string key, params object[] args)
	{
		StringsTemplate stringsTemplate = StringsTemplate.Tem(new object[]
		{
			key
		});
		string text2;
		if (stringsTemplate == null)
		{
			UnityEngine.Debug.LogError("语言配置表不包含key:" + key);
			text2 = text.text;
		}
		else
		{
			text2 = stringsTemplate.text;
		}
		if (args != null && args.Length > 0)
		{
			text2 = string.Format(text2, args);
		}
		text.text = text2;
		return text2;
	}
}
