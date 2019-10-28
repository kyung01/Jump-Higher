using UnityEngine;
using System.Collections;

public class Coin : Entity
{
	public delegate void DelCoin(Coin self);
	public DelCoin evntTriggered;
	public Animator animator;
	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{

	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log(collision.gameObject.tag);
		if (evntTriggered != null) evntTriggered(this);
	}
}
