using System;
using UnityEngine;

public class PortalTransfer : MonoBehaviour
{
	public Portal portal;

	private const float minSpeed = 8f;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.name.StartsWith("ing-"))
		{
			Rigidbody2D component = other.GetComponent<Rigidbody2D>();
			if (component != null)
			{
				if (other.tag == "Player")
				{
					Common.PlaySoundEffect("SFX:portal");
				}
				Vector2 a = Quaternion.Euler(-this.portal.mTrans.localEulerAngles) * component.velocity;
				other.name = "ing-" + this.portal.transferType + other.name;
				if (other.tag.Equals("DynamicParticle"))
				{
					other.transform.position = this.portal.other.point.position + new Vector3((float)UnityEngine.Random.Range(-20, 20) * 0.01f, 0f);
				}
				else
				{
					other.transform.position = this.portal.other.point.position;
				}
				if (a.magnitude < 8f)
				{
					component.velocity = this.portal.other.mTrans.up * 8f;
				}
				else
				{
					component.velocity = Quaternion.Euler(this.portal.other.mTrans.localEulerAngles) * -a;
				}
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.name.StartsWith("ing-") && !other.name.StartsWith("ing-" + this.portal.transferType))
		{
			other.name = other.name.Remove(0, 5);
		}
	}
}
