using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Platform : Entity
{
	static float TIMER_WEKA_PLATFORM_DISAPPEAR = 3;
	public enum PlatformSize { SMALL, BIG };

	public List<Sprite> SPRITES_SMALL;
	public List<Sprite> SPRITES_SMALL_BROKEN;
	public List<Sprite> SPRITES_BIG;
	public List<Sprite> SPRITES_BIG_BROKEN;

	public SpriteRenderer spriteRenderer;
	public BoxCollider2D boxColliderSmall;
	public BoxCollider2D boxColliderBig;
	public bool isWeak;
	bool isTouched = false;
	float timeRemainingToDisappear = 0;


	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if(isWeak && isTouched)
		{
			timeRemainingToDisappear -= Time.deltaTime;
			float ratio =Mathf.Max(0, timeRemainingToDisappear / TIMER_WEKA_PLATFORM_DISAPPEAR);
			spriteRenderer.color = new Color(1,1,1,ratio);
			if (timeRemainingToDisappear<0)
			{
				this.gameObject.SetActive(false);
			}
		}
	}
	public void init(PlatformSize size, bool isWeak)
	{
		this.isWeak = isWeak;
		switch (size)
		{
			case PlatformSize.SMALL:
				boxColliderSmall.enabled = true;
				boxColliderBig.enabled = false;
				break;
			case PlatformSize.BIG:
				boxColliderSmall.enabled = false;
				boxColliderBig.enabled = true;
				break;
		}
		if (isWeak)
		{
			switch (size)
			{
				case PlatformSize.SMALL:
					spriteRenderer.sprite = SPRITES_SMALL_BROKEN[Random.Range(0, SPRITES_SMALL_BROKEN.Count)];
					break;
				case PlatformSize.BIG:
					spriteRenderer.sprite = SPRITES_BIG_BROKEN[Random.Range(0, SPRITES_BIG_BROKEN.Count)];
					break;
			}
		}
		else
		{
			switch (size)
			{
				case PlatformSize.SMALL:
					spriteRenderer.sprite = SPRITES_SMALL[Random.Range(0, SPRITES_SMALL.Count)];
					break;
				case PlatformSize.BIG:
					spriteRenderer.sprite = SPRITES_BIG[Random.Range(0, SPRITES_BIG.Count)];
					break;
			}
		}
		this.gameObject.SetActive(true);

	}
	public override void reset()
	{
		base.reset();
		isWeak = false;
		isTouched = false;
		spriteRenderer.color = Color.white;
		timeRemainingToDisappear = TIMER_WEKA_PLATFORM_DISAPPEAR;
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.transform.tag == "Player")
			isTouched = true;
	}
}
