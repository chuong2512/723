using System;
using UnityEngine;

public class EditObjData : MonoBehaviour
{
	[Tooltip("变换模型是否继承前任大小")]
	public bool InheritScale;

	public Vector3 DefaultScale = new Vector3(1f, 1f, 1f);

	public SizeTransform sizeTransform;

	[EnumFlags]
	public TransformConstraints transformConstraints;

	public string ObjData;

	private void OnValidate()
	{
		if (this.sizeTransform == SizeTransform.Symmetrical)
		{
			this.transformConstraints = TransformConstraints.ScaleY;
		}
	}
}
