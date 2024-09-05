using System;
using UnityEngine;

public class RealTimeParticle : MonoBehaviour
{
	private ParticleSystem _particle;

	private float _deltaTime;

	private float _timeAtLastFrame;

	private void Awake()
	{
		this._particle = base.GetComponent<ParticleSystem>();
	}

	private void Update()
	{
		this._particle.Simulate(Time.unscaledDeltaTime, true, false);
	}
}
