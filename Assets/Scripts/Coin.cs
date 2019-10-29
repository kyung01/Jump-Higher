using UnityEngine;
using System.Collections;

public class Coin : Entity
{
	public delegate void DelCoin(Coin self);
	public DelCoin evntTriggered;
	public Animator animator;
	public int value;
	bool isTriggered = false;
	public void setValue(int n)
	{
		value = n;
		animator.SetInteger("value", Random.Range(0, 3));
	}
	public override void reset()
	{
		base.reset();
		isTriggered = false;
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (isTriggered) return;
		//Debug.Log(collision.gameObject.tag);
		isTriggered = true;
		if (evntTriggered != null) evntTriggered(this);
	}
}
