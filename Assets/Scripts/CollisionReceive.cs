using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class CollisionReceive : MonoBehaviour
{


	public object[] objData;

	public event Action<Collision2D> TriggerEnter;

	public void Init(Action<Collision2D> triggerAction)
	{
		this.TriggerEnter = triggerAction;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (this.TriggerEnter != null)
		{
			this.TriggerEnter(other);
		}
	}
}
