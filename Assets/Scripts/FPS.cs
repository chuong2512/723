using System;
using UnityEngine;

public class FPS : MonoBehaviour
{
	private float m_LastUpdateShowTime;

	private float m_UpdateShowDeltaTime = 0.5f;

	private int m_FrameUpdate;

	private float m_FPS;

	private GUIStyle style;

	private void Awake()
	{
	}

	private void Start()
	{
		this.m_LastUpdateShowTime = Time.realtimeSinceStartup;
		this.style = new GUIStyle();
		this.style.normal.textColor = Color.red;
		this.style.fontSize = Screen.height / 50;
	}

	private void Update()
	{
		this.m_FrameUpdate++;
		if (Time.realtimeSinceStartup - this.m_LastUpdateShowTime >= this.m_UpdateShowDeltaTime)
		{
			this.m_FPS = (float)this.m_FrameUpdate / (Time.realtimeSinceStartup - this.m_LastUpdateShowTime);
			this.m_FrameUpdate = 0;
			this.m_LastUpdateShowTime = Time.realtimeSinceStartup;
		}
	}

	private void OnGUI()
	{
		GUILayout.Label(string.Concat(new object[]
		{
			"FPS: ",
			this.m_FPS.ToString("f2"),
			" P:",
			(!(ParticleGenerator.Inst == null)) ? ParticleGenerator.Inst.particleCount : 0
		}), this.style, new GUILayoutOption[0]);
	}
}
