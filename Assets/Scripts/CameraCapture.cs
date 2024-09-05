using System;
using System.IO;
using UnityEngine;

public class CameraCapture : MonoBehaviour
{
	public enum CaptureSize
	{
		CameraSize,
		ScreenResolution,
		FixedSize
	}

	public Camera targetCamera;

	public CameraCapture.CaptureSize captureSize;

	public Vector2 pixelSize;

	public string savePath = "StreamingAssets/";

	public string fileName = "cameraCapture.png";

	public void saveCapture()
	{
		Vector2 vector = this.pixelSize;
		if (this.captureSize == CameraCapture.CaptureSize.CameraSize)
		{
			vector = new Vector2((float)this.targetCamera.pixelWidth, (float)this.targetCamera.pixelHeight);
		}
		else if (this.captureSize == CameraCapture.CaptureSize.ScreenResolution)
		{
			vector = new Vector2((float)Screen.currentResolution.width, (float)Screen.currentResolution.height);
		}
		string path = EditorData.GetSaveTexturePath() + "/" + this.savePath + this.fileName;
		CameraCapture.saveTexture(path, CameraCapture.capture(this.targetCamera, (int)vector.x, (int)vector.y));
	}

	public static Texture2D capture(Camera camera)
	{
		return CameraCapture.capture(camera, Screen.width, Screen.height);
	}

	public static Texture2D capture(Camera camera, int width, int height)
	{
		RenderTexture renderTexture = new RenderTexture(width, height, 0);
		renderTexture.depth = 24;
		renderTexture.antiAliasing = 8;
		camera.targetTexture = renderTexture;
		camera.RenderDontRestore();
		RenderTexture.active = renderTexture;
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false, true);
		Rect source = new Rect(0f, 0f, (float)width, (float)height);
		texture2D.ReadPixels(source, 0, 0);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		camera.targetTexture = null;
		RenderTexture.active = null;
		UnityEngine.Object.Destroy(renderTexture);
		return texture2D;
	}

	public static void saveTexture(string path, Texture2D texture)
	{
		File.WriteAllBytes(path, texture.EncodeToPNG());
	}
}
