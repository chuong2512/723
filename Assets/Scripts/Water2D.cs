using LitJson;
using System;
using UnityEngine;

public class Water2D : Primitives
{
	private struct WaterColumn
	{
		public float TargetHeight;

		public float Height;

		public float Speed;

		public float lastHeight;

		public bool splashNow;

		public Water2D water;

		public int index;

		public bool enableSplash;

		private bool hasToSplash;

		public void Update(float dampening, float tension)
		{
			float num = this.TargetHeight - this.Height;
			this.Speed += tension * num - this.Speed * dampening;
			this.Height += this.Speed;
			if (this.Height > this.TargetHeight + this.water.heightLimitToSplashParticles)
			{
				this.hasToSplash = true;
			}
			else
			{
				this.hasToSplash = false;
			}
			if (this.enableSplash && this.hasToSplash && this.Height < this.lastHeight)
			{
				this.enableSplash = false;
			}
			this.lastHeight = this.Height;
		}
	}

	public string aortingLayerName = "WaterUp";

	public int orderInLayer;

	private float width = 1f;

	private float height = 1f;

	[Tooltip("垂直细分的数量。")]
	public int waterSubdivisions = 1;

	[Tooltip("它是每一次srping的力。高值使填充更有弹性")]
	public float Tension = 0.025f;

	public float Dampening = 0.025f;

	[Tooltip("这个力会传播到邻近的弹簧多远")]
	public float Spread = 0.25f;

	public float objectSizeDampening = 1f;

	public int neighbours = 8;

	public float idleFactor;

	public float idleWavesSpeed;

	public float maxWaterForceApplied = 50f;

	public float heightLimitToSplashParticles = 10f;

	private const string _DISABLE_WAVES = "DISABLE_WAVES";

	private const string _DISABLE_REFRACTION = "DISABLE_REFRACTION";

	public bool DISABLE_REFRACTION;

	private Water2D.WaterColumn[] columns;

	private Mesh proceduralMesh;

	private Vector3[] meshVertices;

	private Mesh myMesh;

	private int firstUpVertex;

	private ParticleSystem instantiatedWaterSplash;

	private ParticleSystem instantiatedAfterPeakwaterSplash;

	private BoxCollider2D myCollider;

	private BuoyancyEffector2D buoyancy;

	private float idleWaveCounter;

	private int lastIdleWaveColumn = -1;

	private Material mat;

	private Renderer renderer;

	private Transform waterDiTrans;

	private Renderer renderer2;

	private Mesh meshDi;

	protected override void Awake()
	{
		base.Awake();
		this.mat = base.GetComponent<Renderer>().material;
		this.myCollider = base.GetComponent<BoxCollider2D>();
		this.renderer = base.GetComponent<MeshRenderer>();
		this.buoyancy = base.GetComponent<BuoyancyEffector2D>();
		this.UpdateShader();
		this.renderer.sortingLayerName = this.aortingLayerName;
		this.renderer.sortingOrder = this.orderInLayer;
		this.waterDiTrans = base.transform.Find("Shadow");
		this.renderer2 = this.waterDiTrans.GetComponent<MeshRenderer>();
		this.renderer2.sortingLayerName = "Water";
	}

	public override void Deserialization(JsonData json)
	{
		base.Deserialization(json);
		if (!this.isSleep)
		{
			this.width = this.originalScale.x;
			this.height = this.originalScale.y;
			this.CreateWaterPlane();
			this.InitializeWater();
		}
	}

	public override void SetTransform()
	{
		if (this.isSleep)
		{
			base.SetTransform();
		}
		else
		{
			this.mTrans.localPosition = this.OriginalPos;
			this.mTrans.localRotation = Quaternion.Euler(this.OriginalRotation);
			this.myCollider.size = this.originalScale;
		}
		this.mat.mainTextureScale = this.originalScale / 2f;
		this.buoyancy.surfaceLevel = Mathf.Max(0f, this.originalScale.y / 2f - 0.5f);
	}

	public void CreateWaterPlane()
	{
		int num = this.waterSubdivisions;
		int num2 = 1;
		MeshFilter component = base.GetComponent<MeshFilter>();
		MeshFilter component2 = this.waterDiTrans.GetComponent<MeshFilter>();
		this.proceduralMesh = new Mesh();
		this.proceduralMesh.name = "Water plane";
		int num3 = num + 1;
		int num4 = num2 + 1;
		int num5 = num * num2 * 6;
		int num6 = num3 * num4;
		Vector3[] array = new Vector3[num6];
		Vector2[] array2 = new Vector2[num6];
		int[] array3 = new int[num5];
		int num7 = 0;
		float num8 = 1f / (float)num;
		float num9 = 1f / (float)num2;
		float num10 = this.width / (float)num;
		float num11 = this.height / (float)num2;
		for (float num12 = 0f; num12 < (float)num4; num12 += 1f)
		{
			for (float num13 = 0f; num13 < (float)num3; num13 += 1f)
			{
				array[num7] = new Vector3(num13 * num10 - this.width / 2f, num12 * num11 - this.height / 2f, 0f);
				array2[num7++] = new Vector2(num13 * num8, num12 * num9);
			}
		}
		num7 = 0;
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				array3[num7] = i * num3 + j;
				array3[num7 + 1] = (i + 1) * num3 + j;
				array3[num7 + 2] = i * num3 + j + 1;
				array3[num7 + 3] = (i + 1) * num3 + j;
				array3[num7 + 4] = (i + 1) * num3 + j + 1;
				array3[num7 + 5] = i * num3 + j + 1;
				num7 += 6;
			}
		}
		this.proceduralMesh.vertices = array;
		this.proceduralMesh.triangles = array3;
		this.proceduralMesh.uv = array2;
		this.proceduralMesh.RecalculateBounds();
		this.proceduralMesh.RecalculateNormals();
		this.proceduralMesh.RecalculateTangents();
		component.mesh = this.proceduralMesh;
		component2.mesh = this.proceduralMesh;
	}

	private void InitializeWater()
	{
		this.myMesh = base.GetComponent<MeshFilter>().mesh;
		this.meshDi = this.waterDiTrans.GetComponent<MeshFilter>().mesh;
		this.myMesh.MarkDynamic();
		this.meshDi.MarkDynamic();
		this.meshVertices = this.myMesh.vertices;
		this.meshVertices = this.meshDi.vertices;
		this.firstUpVertex = (int)((float)this.meshVertices.Length * 0.5f);
		this.columns = new Water2D.WaterColumn[this.waterSubdivisions + 1];
		float y = this.meshVertices[this.meshVertices.Length - 1].y;
		for (int i = 0; i < this.columns.Length; i++)
		{
			this.columns[i].Height = y;
			this.columns[i].TargetHeight = y;
			this.columns[i].Speed = 0f;
			this.columns[i].lastHeight = y;
			this.columns[i].water = this;
			this.columns[i].index = i;
		}
	}

	private void Update()
	{
		if (this.isSleep)
		{
			return;
		}
		for (int i = 0; i < this.columns.Length; i++)
		{
			this.columns[i].Update(this.Dampening, this.Tension);
		}
		float[] array = new float[this.columns.Length];
		float[] array2 = new float[this.columns.Length];
		for (int j = 0; j < this.neighbours; j++)
		{
			for (int k = 0; k < this.columns.Length; k++)
			{
				if (k > 0)
				{
					array[k] = this.Spread * (this.columns[k].Height - this.columns[k - 1].Height);
					Water2D.WaterColumn[] expr_B6_cp_0 = this.columns;
					int expr_B6_cp_1 = k - 1;
					expr_B6_cp_0[expr_B6_cp_1].Speed = expr_B6_cp_0[expr_B6_cp_1].Speed + array[k];
				}
				if (k < this.columns.Length - 1)
				{
					array2[k] = this.Spread * (this.columns[k].Height - this.columns[k + 1].Height);
					Water2D.WaterColumn[] expr_118_cp_0 = this.columns;
					int expr_118_cp_1 = k + 1;
					expr_118_cp_0[expr_118_cp_1].Speed = expr_118_cp_0[expr_118_cp_1].Speed + array2[k];
				}
			}
			for (int l = 0; l < this.columns.Length; l++)
			{
				if (l > 0)
				{
					Water2D.WaterColumn[] expr_15C_cp_0 = this.columns;
					int expr_15C_cp_1 = l - 1;
					expr_15C_cp_0[expr_15C_cp_1].Height = expr_15C_cp_0[expr_15C_cp_1].Height + array[l];
				}
				if (l < this.columns.Length - 1)
				{
					Water2D.WaterColumn[] expr_18C_cp_0 = this.columns;
					int expr_18C_cp_1 = l + 1;
					expr_18C_cp_0[expr_18C_cp_1].Height = expr_18C_cp_0[expr_18C_cp_1].Height + array2[l];
				}
			}
		}
		int num = 0;
		for (int m = this.firstUpVertex; m < this.meshVertices.Length; m++)
		{
			this.meshVertices[m].y = this.columns[num].Height;
			num++;
		}
		this.myMesh.vertices = this.meshVertices;
		this.meshDi.vertices = this.meshVertices;
		if (this.idleFactor > 0f)
		{
			this.idleWaveCounter = Mathf.MoveTowards(this.idleWaveCounter, (float)(this.columns.Length - 1), Time.deltaTime * this.idleWavesSpeed);
			if ((int)this.idleWaveCounter != this.lastIdleWaveColumn)
			{
				this.MoveSpring((int)this.idleWaveCounter, this.idleFactor);
				this.lastIdleWaveColumn = (int)this.idleWaveCounter;
			}
			if (this.idleWaveCounter == (float)(this.columns.Length - 1))
			{
				this.idleWaveCounter = 0f;
			}
		}
	}

	public void ObjectEnteredWater(Vector3 _position, float _speed, float _size, bool _emitParticles)
	{
		float num = 3.40282347E+38f;
		int num2 = 0;
		int num3 = 0;
		for (int i = this.firstUpVertex; i < this.meshVertices.Length; i++)
		{
			float sqrMagnitude = (this.mTrans.TransformPoint(this.meshVertices[i]) - _position).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				num = sqrMagnitude;
				num2 = i;
				num3 = i - this.firstUpVertex;
			}
		}
		Water2D.WaterColumn[] expr_77_cp_0 = this.columns;
		int expr_77_cp_1 = num3;
		expr_77_cp_0[expr_77_cp_1].Speed = expr_77_cp_0[expr_77_cp_1].Speed + _speed;
		this.columns[num3].enableSplash = true;
		float num4 = this.width / (float)this.waterSubdivisions;
		int num5 = Mathf.Clamp(Mathf.FloorToInt(_size / num4), 0, this.columns.Length - 1);
		for (int j = 1; j < num5; j++)
		{
			if (num3 + j < this.columns.Length)
			{
				this.columns[num3 + j].Speed = _speed / (this.objectSizeDampening * (float)(j + 1));
			}
			if (num3 - j >= 0)
			{
				this.columns[num3 - j].Speed = _speed / (this.objectSizeDampening * (float)(j + 1));
			}
		}
		if (_emitParticles && this.instantiatedWaterSplash != null)
		{
			this.instantiatedWaterSplash.transform.position = this.mTrans.TransformPoint(this.meshVertices[num2]);
			this.instantiatedWaterSplash.Emit(UnityEngine.Random.Range(10, 20));
		}
	}

	public void ObjectEnteredWater(Vector3 _position, float _speed, Bounds _size, bool _emitParticles)
	{
		if (this.isSleep)
		{
			return;
		}
		this.ObjectEnteredWater(_position, _speed * 0.1f, _size.extents.x, _emitParticles);
	}

	public void SetHeight(float _newHeight)
	{
		for (int i = 0; i < this.columns.Length; i++)
		{
			this.columns[i].TargetHeight = _newHeight;
		}
		Vector3 v = this.myCollider.size;
		v.x = this.width;
		v.y = this.height * 0.5f + _newHeight;
		this.myCollider.size = v;
		this.myCollider.offset = new Vector3(0f, -this.height * 0.25f + _newHeight * 0.5f, 0f);
	}

	private void MoveSpring(int _springIndex, float _speed)
	{
		Water2D.WaterColumn[] expr_0C_cp_0 = this.columns;
		expr_0C_cp_0[_springIndex].Speed = expr_0C_cp_0[_springIndex].Speed + _speed;
	}

	private void SplashParticles(int _column)
	{
		this.instantiatedAfterPeakwaterSplash.transform.position = this.mTrans.TransformPoint(this.meshVertices[_column + this.firstUpVertex]);
		this.instantiatedAfterPeakwaterSplash.Emit(UnityEngine.Random.Range(5, 9));
	}

	private void OnTriggerEnter2D(Collider2D _collider)
	{
		Rigidbody2D component = _collider.GetComponent<Rigidbody2D>();
		if (component != null)
		{
			Vector3 position = new Vector3(_collider.transform.position.x, this.height / 2f + this.mTrans.position.y);
			this.ObjectEnteredWater(position, Mathf.Clamp(component.velocity.y * component.mass, -this.maxWaterForceApplied, this.maxWaterForceApplied), _collider.bounds, true);
		}
	}

	private void OnTriggerExit2D(Collider2D _collider)
	{
		Rigidbody2D component = _collider.GetComponent<Rigidbody2D>();
		if (component != null)
		{
			Vector2 v = new Vector3(_collider.transform.position.x, this.height / 2f + this.mTrans.position.y);
			this.ObjectEnteredWater(v, Mathf.Clamp(component.velocity.y * component.mass, -this.maxWaterForceApplied, this.maxWaterForceApplied), _collider.bounds, true);
		}
	}

	public void UpdateShader()
	{
		Material sharedMaterial = this.renderer.sharedMaterial;
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
