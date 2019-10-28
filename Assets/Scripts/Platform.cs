using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Platform : MonoBehaviour
{
	public enum PlatformSize { SMALL, BIG };

	public List<Sprite> SPRITES_SMALL;
	public List<Sprite> SPRITES_SMALL_BROKEN;
	public List<Sprite> SPRITES_BIG;
	public List<Sprite> SPRITES_BIG_BROKEN;

	public SpriteRenderer spriteRenderer;
	public BoxCollider2D boxColliderSmall;
	public BoxCollider2D boxColliderBig;
	public bool isWeak;


	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

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
}
