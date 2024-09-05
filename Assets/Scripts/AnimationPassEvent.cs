using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class AnimationPassEvent : MonoBehaviour
{


	public event Action<string> AniPassEvent;

	public void NameToEvent(string name)
	{
		if (this.AniPassEvent != null)
		{
			this.AniPassEvent(name);
		}
	}
}
