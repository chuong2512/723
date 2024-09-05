using System;
using UnityEngine;
using UnityEngine.UI;

public class RenderWeaponView : MonoBehaviour
{
	private GameObject render;

	private int pos;

	public void InitRenderWeapon(GameObject render, RawImage image, float cameraSize, int pos)
	{
		this.render = render;
		RenderTexture temporary = RenderTexture.GetTemporary((int)image.rectTransform.sizeDelta.x * 2, (int)image.rectTransform.sizeDelta.y * 2, 16, RenderTextureFormat.ARGB32);
		Camera componentInChildren = render.GetComponentInChildren<Camera>();
		componentInChildren.targetTexture = temporary;
		componentInChildren.orthographicSize = cameraSize;
		image.texture = temporary;
		this.pos = pos;
	}

	public void InitRenderWeapon(GameObject render, RawImage image, Camera camera, int pos)
	{
		this.render = render;
		RenderTexture temporary = RenderTexture.GetTemporary((int)image.rectTransform.sizeDelta.x * 2, (int)image.rectTransform.sizeDelta.y * 2, 16, RenderTextureFormat.ARGB32);
		camera.targetTexture = temporary;
		image.texture = temporary;
		this.pos = pos;
	}

	private void OnDestroy()
	{
		RenderTexManager.AddNullPos(this.pos);
		UnityEngine.Object.Destroy(this.render);
	}
}
