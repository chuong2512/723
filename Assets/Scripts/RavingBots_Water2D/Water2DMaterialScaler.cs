using System;
using UnityEngine;

namespace RavingBots.Water2D
{
	[ExecuteInEditMode, RequireComponent(typeof(MeshRenderer))]
	public class Water2DMaterialScaler : MonoBehaviour
	{
		public string SortingLayerName = "Water";

		public bool DisableInGame = true;

		public bool UpdateEachFrame = true;

		[Range(0f, 1f)]
		public float Transparency = 0.5f;

		[Range(0f, 1f)]
		public float RefractionIntensity = 0.02f;

		public float BumpMapTilling = 0.1f;

		public float TextureTilling = 1f;

		public float WaveDensity = 1f;

		public float WaveAmplitude = 0.2f;

		public float WaveSpeed = 0.5f;

		[Range(0.001f, 1f)]
		public float WaveEdgeSoftness = 0.7f;

		[Range(0f, 1f)]
		public float WaveBlendLevel = 0.5f;

		public float CurrentSpeed = -0.15f;

		private const string _DISABLE_WAVES = "DISABLE_WAVES";

		private const string _DISABLE_REFRACTION = "DISABLE_REFRACTION";

		public bool DISABLE_WAVES;

		public bool DISABLE_REFRACTION;

		private MeshRenderer _renderer;

		private void Awake()
		{
			this._renderer = base.GetComponent<MeshRenderer>();
			this._renderer.sortingLayerName = this.SortingLayerName;
			if (Application.isPlaying)
			{
				if (this.DisableInGame)
				{
					base.enabled = false;
				}
				else
				{
					UnityEngine.Debug.LogWarning("Material is updated each frame (check DisableInGame to increase performance)");
				}
			}
		}

		private void Update()
		{
			if (this.UpdateEachFrame)
			{
				this.UpdateMaterial();
				this.UpdateShader();
			}
		}

		[ContextMenu("Update Material")]
		public void UpdateMaterial()
		{
			Material sharedMaterial = this._renderer.sharedMaterial;
			float x = base.transform.lossyScale.x;
			float y = base.transform.lossyScale.y;
			Color color = sharedMaterial.color;
			color.a = this.Transparency;
			sharedMaterial.color = color;
			sharedMaterial.SetFloat("_Intensity", this.RefractionIntensity);
			Vector4 vector = sharedMaterial.GetVector("_Wave");
			vector.x = this.WaveDensity * x;
			vector.y = this.WaveAmplitude / y;
			vector.z = this.WaveSpeed;
			vector.w = this.WaveEdgeSoftness;
			sharedMaterial.SetVector("_Wave", vector);
			sharedMaterial.SetFloat("_Level", this.WaveBlendLevel);
			sharedMaterial.SetFloat("_Current", this.CurrentSpeed / x);
			sharedMaterial.SetTextureScale("_MainTex", new Vector2(this.TextureTilling * y, x / y));
			sharedMaterial.SetTextureScale("_Refraction", new Vector2(this.BumpMapTilling * x, this.BumpMapTilling * y));
		}

		[ContextMenu("Update Shader")]
		public void UpdateShader()
		{
			Material sharedMaterial = this._renderer.sharedMaterial;
			this.SetKeyword(sharedMaterial, this.DISABLE_WAVES, "DISABLE_WAVES");
			this.SetKeyword(sharedMaterial, this.DISABLE_REFRACTION, "DISABLE_REFRACTION");
		}

		private void SetKeyword(Material m, bool state, string name)
		{
			if (state == m.IsKeywordEnabled(name))
			{
				return;
			}
			if (state)
			{
				m.EnableKeyword(name);
			}
			else
			{
				m.DisableKeyword(name);
			}
		}
	}
}
