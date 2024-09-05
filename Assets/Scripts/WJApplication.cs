using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class WJApplication : MonoBehaviour
{
	public enum MoreGameEvent
	{
		Show,
		Close,
		Selected
	}

	private static WJApplication s_instance;





	public static event Action onApplicationDidEnterBackground;

	public static event Action onApplicationWillEnterForeground;

	public static WJApplication AppInstance
	{
		get
		{
			return WJApplication.s_instance;
		}
	}

	private void Awake()
	{
		base.name = "_LBLibraryUnity_";
		this.SuperOnAwake();
	}

	private void Start()
	{
		this.SuperOnStart();
	}

	protected virtual void SuperOnAwake()
	{
		if (WJApplication.s_instance != null)
		{
			throw new Exception("LBLibrary: There can be only one instance for WJApplication.");
		}
		Application.targetFrameRate = 60;
		WJApplication.s_instance = this;
		base.gameObject.AddComponent<WJSound2D>();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	protected virtual void SuperOnStart()
	{
		this.OnApplicationStart();
	}

	private void LateUpdate()
	{
		if (UnityEngine.Input.GetKeyUp(KeyCode.Menu))
		{
			WJUtils.CallAction_Void(16, string.Empty);
		}
		this.SuperLateUpdate();
		WJUtils.LateUpdate();
	}

	protected virtual void SuperLateUpdate()
	{
	}

	public void Native_Callback(string param)
	{
		WJUtils.Native_Callback(param);
	}

	public virtual void ApplicationDidEnterBackground()
	{
		if (WJApplication.onApplicationDidEnterBackground != null)
		{
			WJApplication.onApplicationDidEnterBackground();
		}
	}

	public virtual void ApplicationWillEnterForeground()
	{
		if (WJApplication.onApplicationWillEnterForeground != null)
		{
			WJApplication.onApplicationWillEnterForeground();
		}
	}

	protected virtual void OnApplicationStart()
	{
	}

	public virtual void OnMoreGameEvent(WJApplication.MoreGameEvent e)
	{
	}
}
