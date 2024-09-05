using System;
using UnityEngine;

internal class ConveyorCollider : MonoBehaviour
{
	private const float MAXSpeed = 20f;

	public bool Top = true;

	public Conveyor conveyor;

	private Vector2 force = Vector2.zero;

	private void OnCollisionStay2D(Collision2D other)
	{
		Rigidbody2D component = other.gameObject.GetComponent<Rigidbody2D>();
		if (component != null)
		{
			float num = 1f;
			if (component.sharedMaterial != null)
			{
				num = component.sharedMaterial.friction;
			}
			this.force.x = Mathf.Abs(component.mass * Physics2D.gravity.y) * num + Mathf.Abs(this.conveyor.power);
			this.force.x = this.force.x * base.transform.right.x;
			if (this.Top)
			{
				if (!this.conveyor.IsRight)
				{
					this.force *= -1f;
				}
			}
			else if (this.conveyor.IsRight)
			{
				this.force *= -1f;
			}
			if (Vector2.Dot(component.velocity, this.force.normalized) >= 20f * Mathf.Abs(this.conveyor.power))
			{
				return;
			}
			component.AddForce(this.force);
		}
	}
}
