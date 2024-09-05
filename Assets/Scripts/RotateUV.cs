using System;
using UnityEngine;

public class RotateUV : MonoBehaviour
{
	public MeshRenderer m_Renderer;

	public float Speed = 10f;

	private float m_Angle;

	private void Start()
	{
		if (this.m_Renderer == null)
		{
			this.m_Renderer = base.gameObject.GetComponent<MeshRenderer>();
		}
	}

	private void Update()
	{
		this.m_Angle += Time.deltaTime * this.Speed;
		if (this.m_Angle >= 360f)
		{
			this.m_Angle = 0f;
		}
		this.m_Renderer.material.SetFloat("_Angle", this.m_Angle * 0.0174532924f);
	}
}
