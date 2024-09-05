using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderTexManager : MonoBehaviour
{
	private static RenderTexManager _instance;

	private Vector3 detlaVec = new Vector3(50f, 0f, 0f);

	private Transform renderPrefab;

	private Queue<int> nullPos = new Queue<int>();

	private int posCount = 1;

	public static RenderTexManager GetInstance()
	{
		if (RenderTexManager._instance == null)
		{
			RenderTexManager._instance = new GameObject
			{
				transform = 
				{
					position = new Vector3(100f, 100f, 0f)
				},
				name = "RenderTexManager"
			}.AddComponent<RenderTexManager>();
		}
		return RenderTexManager._instance;
	}

	protected void Awake()
	{
		RenderTexManager._instance = this;
		this.renderPrefab = Resources.Load<Transform>("Prefabs/Render");
	}

	public void InitForView(GameObject renderObj, RawImage image, Vector3 localPos = default(Vector3), float cameraSize = 2f)
	{
		renderObj.transform.parent = this.InitForView(image, cameraSize);
		renderObj.transform.localPosition = localPos;
	}

	public Transform InitForView(RawImage image, float cameraSize = 2f)
	{
		int pos;
		Transform transform = this.CreateRender(out pos);
		RenderWeaponView renderWeaponView = image.gameObject.AddComponent<RenderWeaponView>();
		renderWeaponView.InitRenderWeapon(transform.gameObject, image, cameraSize, pos);
		return transform;
	}

	public Transform InitForViewForMainCamera(GameObject renderObj, RawImage image, Vector3 localPos = default(Vector3))
	{
		int pos;
		Transform transform = this.CreateRender(out pos);
		renderObj.transform.parent = transform;
		renderObj.transform.localPosition = localPos;
		RenderWeaponView renderWeaponView = image.gameObject.AddComponent<RenderWeaponView>();
		Camera componentInChildren = transform.GetComponentInChildren<Camera>();
		componentInChildren.transform.localPosition = new Vector3(0f, 8.45f, -20f);
		componentInChildren.orthographicSize = 9f;
		renderWeaponView.InitRenderWeapon(transform.gameObject, image, componentInChildren, pos);
		return transform;
	}

	public Transform InitForShowShop(GameObject renderObj, RawImage image, Vector3 localPos = default(Vector3), float cameraSize = 2f)
	{
		int pos;
		Transform transform = this.CreateRender(out pos);
		renderObj.transform.parent = transform;
		renderObj.transform.localPosition = localPos;
		RenderWeaponView renderWeaponView = image.gameObject.AddComponent<RenderWeaponView>();
		Camera componentInChildren = transform.GetComponentInChildren<Camera>();
		componentInChildren.transform.localPosition = new Vector3(0f, 0f, 10f);
		componentInChildren.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
		componentInChildren.orthographicSize = cameraSize;
		renderWeaponView.InitRenderWeapon(transform.gameObject, image, componentInChildren, pos);
		return transform;
	}

	private Transform CreateRender(out int n)
	{
		Transform transform = UnityEngine.Object.Instantiate<Transform>(this.renderPrefab);
		n = this.posCount;
		if (this.nullPos.Count > 0)
		{
			n = this.nullPos.Dequeue();
		}
		else
		{
			this.posCount++;
			n = this.posCount;
		}
		transform.parent = base.transform;
		transform.localPosition = this.detlaVec * (float)n;
		transform.localScale = Vector3.one;
		transform.name = "render" + base.transform.childCount;
		return transform;
	}

	public static void AddNullPos(int pos)
	{
		if (RenderTexManager._instance == null)
		{
			return;
		}
		RenderTexManager._instance.nullPos.Enqueue(pos);
	}
}
