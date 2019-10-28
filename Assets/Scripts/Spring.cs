using UnityEngine;
using System.Collections;

public class Spring : Entity
{

	public Animator animator;
	public delegate void DelSpring(Spring self);
	public DelSpring evntTriggered;

	public override void reset()
	{
		base.reset();
		animator.SetBool("isActivated", false);
	}
	private void OnTriggerStay2D(Collider2D collision)
	{

		if (Player.self.rigidbody.velocity.y <0&& evntTriggered != null && collision.tag == "Player")
		{

			evntTriggered(this);
			animator.SetBool("isActivated", true);
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
	}
}
