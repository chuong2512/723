using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{


	public object[] objData;

	public event Action<Collider2D> TriggerEnter;

	public void Init(Action<Collider2D> triggerAction)
	{
		this.TriggerEnter = triggerAction;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (this.TriggerEnter != null)
		{
			this.TriggerEnter(other);
		}
	}
}
