using System;
using UnityEngine;

namespace RavingBots.Water2D
{
	public class Water2DSplashFX : MonoBehaviour
	{
		public ParticleSystem DropParticles;

		public ParticleSystem BubbleParticles;

		public int DropCount = 30;

		[Range(0f, 1f)]
		public float RandDropLifetime = 1f;

		[Range(0f, 1f)]
		public float RandDropSpeed = 0.8f;

		public int BubbleCount = 30;

		[Range(0f, 1f)]
		public float RandBubbleLifetime = 1f;

		private ParticleSystem.Particle[] _drops;

		private ParticleSystem.Particle[] _bubbles;

		private AudioSource _audioSource;

		private float _gravityModifier;

		private void Awake()
		{
			this._audioSource = base.GetComponent<AudioSource>();
			this._gravityModifier = this.DropParticles.gravityModifier;
		}

		public void Play(float scale, AudioClip sound, float volume, float pitch)
		{
			this.PlayDrops(scale);
			this.PlayBubbles(scale);
			this._audioSource.Stop();
			this._audioSource.clip = sound;
			this._audioSource.volume = volume;
			this._audioSource.pitch = pitch;
			this._audioSource.Play();
		}

		private void PlayDrops(float scale)
		{
			this.DropParticles.gravityModifier = this._gravityModifier * scale;
			this.DropParticles.Emit(Mathf.RoundToInt(scale * (float)this.DropCount));
			this.PrepareTable(this.DropParticles, ref this._drops);
			this.DropParticles.GetParticles(this._drops);
			float num = Mathf.Sqrt(scale);
			for (int i = 0; i < this._drops.Length; i++)
			{
				ParticleSystem.Particle[] expr_6A_cp_0 = this._drops;
				int expr_6A_cp_1 = i;
				expr_6A_cp_0[expr_6A_cp_1].startLifetime = expr_6A_cp_0[expr_6A_cp_1].startLifetime * (1f - UnityEngine.Random.value * this.RandDropLifetime);
				ParticleSystem.Particle[] expr_94_cp_0 = this._drops;
				int expr_94_cp_1 = i;
				expr_94_cp_0[expr_94_cp_1].velocity = expr_94_cp_0[expr_94_cp_1].velocity * ((1f - UnityEngine.Random.value * this.RandDropSpeed) * scale);
				this._drops[i].rotation = this.GetAngle(this._drops[i].velocity);
				ParticleSystem.Particle[] expr_F1_cp_0 = this._drops;
				int expr_F1_cp_1 = i;
				expr_F1_cp_0[expr_F1_cp_1].startSize = expr_F1_cp_0[expr_F1_cp_1].startSize * num;
			}
			this.DropParticles.SetParticles(this._drops, this._drops.Length);
		}

		private void PlayBubbles(float scale)
		{
			this.BubbleParticles.Emit(Mathf.RoundToInt(scale * (float)this.BubbleCount));
			this.PrepareTable(this.BubbleParticles, ref this._bubbles);
			this.BubbleParticles.GetParticles(this._bubbles);
			float num = Mathf.Sqrt(scale);
			for (int i = 0; i < this._bubbles.Length; i++)
			{
				ParticleSystem.Particle[] expr_57_cp_0 = this._bubbles;
				int expr_57_cp_1 = i;
				expr_57_cp_0[expr_57_cp_1].startLifetime = expr_57_cp_0[expr_57_cp_1].startLifetime * (1f - UnityEngine.Random.value * this.RandBubbleLifetime);
				ParticleSystem.Particle[] expr_81_cp_0 = this._bubbles;
				int expr_81_cp_1 = i;
				expr_81_cp_0[expr_81_cp_1].startSize = expr_81_cp_0[expr_81_cp_1].startSize * num;
			}
			this.BubbleParticles.SetParticles(this._bubbles, this._bubbles.Length);
		}

		private void PrepareTable(ParticleSystem particleSystem, ref ParticleSystem.Particle[] particles)
		{
			if (particles == null || particles.Length != particleSystem.particleCount)
			{
				particles = new ParticleSystem.Particle[particleSystem.particleCount];
			}
		}

		private float GetAngle(Vector2 v)
		{
			return Vector2.Angle(Vector2.up, v) * Mathf.Sign(Vector2.Dot(v, Vector2.right));
		}
	}
}
