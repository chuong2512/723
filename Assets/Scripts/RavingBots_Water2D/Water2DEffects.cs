using System;
using UnityEngine;

namespace RavingBots.Water2D
{
	[RequireComponent(typeof(BuoyancyEffector2D))]
	public class Water2DEffects : MonoBehaviour
	{
		public Water2DSplashFX SplashFXPrefab;

		public int SplashFXPrecache = 30;

		public float SplashFXPowerScale = 0.1f;

		public float SplashFXPowerThreshold = 0.1f;

		public float SplashFXOffset = 0.2f;

		public AudioClip[] SplashFXSounds;

		public float SplashFXPowerToVolume = 1f;

		public float SplashFXPowerToPitch = 1f;

		public float FloatingSpeed = 1f;

		public float FloatingRange = 1f;

		private BuoyancyEffector2D _buoyancyEffector2D;

		private float _surfaceLevel;

		private Water2DSplashFX[] _splashCache;

		private int _splash;

		private void Awake()
		{
			this._buoyancyEffector2D = base.GetComponent<BuoyancyEffector2D>();
			this._surfaceLevel = this._buoyancyEffector2D.surfaceLevel;
			this._splashCache = new Water2DSplashFX[this.SplashFXPrecache];
			Transform transform = new GameObject("Splash Container").transform;
			for (int i = 0; i < this._splashCache.Length; i++)
			{
				Water2DSplashFX water2DSplashFX = UnityEngine.Object.Instantiate<Water2DSplashFX>(this.SplashFXPrefab);
				water2DSplashFX.transform.parent = transform;
				this._splashCache[i] = water2DSplashFX;
			}
		}

		private void FixedUpdate()
		{
			this._buoyancyEffector2D.surfaceLevel = this._surfaceLevel - this.FloatingRange * 0.5f * (Mathf.Sin(6.28318548f * this.FloatingSpeed * Time.fixedTime) + 1f);
		}

		public void OnTriggerEnter2D(Collider2D other)
		{
			Rigidbody2D component = other.transform.parent.GetComponent<Rigidbody2D>();
			float num = this.SplashFXPowerScale * Vector2.Dot(component.velocity, Vector2.down) * component.mass;
			if (num < this.SplashFXPowerThreshold)
			{
				return;
			}
			Water2DSplashFX water2DSplashFX = this._splashCache[this._splash];
			water2DSplashFX.transform.position = new Vector2(other.bounds.center.x, other.bounds.min.y - this.SplashFXOffset);
			water2DSplashFX.Play(num, this.SplashFXSounds[UnityEngine.Random.Range(0, this.SplashFXSounds.Length)], num * this.SplashFXPowerToVolume, this.SplashFXPowerToPitch / num);
			this._splash = (this._splash + 1) % this._splashCache.Length;
		}
	}
}
