using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationScene : WJApplication
{
	private DateTime mEnterBackgroundTime = DateTime.Now;

	private bool mIsPlayed;

	private bool isFrist = true;

	private static Action<bool> __f__am_cache0;

	protected override void SuperOnAwake()
	{
		UnityEngine.Debug.unityLogger.logEnabled = false;
		base.SuperOnAwake();
		this.Initialize();
		SceneManager.LoadScene("GameScene");
		base.gameObject.AddComponent<ADSManager>();
		Input.multiTouchEnabled = false;
	}

	private void OnApplicationFocus(bool isFocus)
	{
		if (isFocus)
		{
			WJUtils.ClearAllLocalNotifications();
			UnityEngine.Debug.Log(string.Format("ApplicationScene--------进入前台 {0}", DateTime.Now));
		}
		else
		{
			if (this.mIsPlayed)
			{
				this.mIsPlayed = false;
				this.mEnterBackgroundTime = DateTime.Now;
			}
			WJUtils.ClearAllLocalNotifications();
			WJUtils.AddCommonLocalNotifications(MessageTemplate.Tem(new object[]
			{
				"notify:nextday"
			}).Text, MessageTemplate.Tem(new object[]
			{
				"notify:thirdday"
			}).Text, MessageTemplate.Tem(new object[]
			{
				"notify:fourthday"
			}).Text);
			UnityEngine.Debug.Log(string.Format("ApplicationScene--------进入后台 {0}", DateTime.Now));
		}
		this.isFrist = false;
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (!pauseStatus)
		{
			//FacebookEventHelper.TrackAppActivateEvent();
		}
	}

	private void Initialize()
	{
		//WJUtils.Start("com.stx.splashtheduck", "54A6EF13ABFF04B4D893861024FB0E97");
		Social.localUser.Authenticate(delegate(bool success)
		{
			UnityEngine.Debug.Log("*** Authenticate: success = " + success);
		});
		MagicTavernHelper.Init("1077", "ec3a1a1a231588a0");
		MagicTavernHelper.TriggerAppStartEvent();
		AppsFlyerEventHelper.Init(string.Empty, "com.stx.splashtheduck");
	}
}
