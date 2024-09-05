using System;
using UnityEngine;

public class PBase : MonoBehaviour
{
	private GameObject backgroundGO;

	[HideInInspector]
	public static Vector3 ScreenLeftDown;

	[HideInInspector]
	public static Vector3 ScreenRightUp;

	protected virtual void Awake()
	{
		this.backgroundGO = GameObject.FindGameObjectWithTag("Bg");
		GameObject gameObject = GameObject.FindGameObjectWithTag("MetaballPlane");
		if (gameObject != null)
		{
			gameObject.GetComponent<Renderer>().sortingLayerName = "Water";
		}
		this.MainCameraAdjust();
	}

	public void MainCameraAdjust()
	{
		float num = (float)SceneSetting.inst.Width * 1f / (float)SceneSetting.inst.Height / ((float)Screen.width * 1f / (float)Screen.height);
		if (Camera.main.orthographic)
		{
			Camera.main.orthographicSize = Mathf.Max(SceneSetting.inst.CameraAdjust, num * SceneSetting.inst.CameraAdjust);
		}
		else
		{
			Camera.main.fieldOfView = Mathf.Max(SceneSetting.inst.CameraAdjust, num * SceneSetting.inst.CameraAdjust);
		}
		PBase.ScreenLeftDown = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, -10f)) + new Vector3(-1f, -1f);
		PBase.ScreenRightUp = Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, -10f)) + new Vector3(1f, 1f);
	}

	public void SetBackground()
	{
		if (this.backgroundGO != null)
		{
			this.backgroundGO.transform.position = Camera.main.ScreenToWorldPoint(new Vector3((float)Screen.width / 2f, 0f, -Camera.main.transform.position.z + 10f)) - new Vector3(0f, 0f, 0f);
		}
	}
}
