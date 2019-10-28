using UnityEngine;
using System.Collections;

public class Player : Entity
{
	public static Player self;

	public float Y { 
		get { return this.transform.position.y; }
	}

	public Rigidbody2D rigidbody;
	public Animator animator;
	public SpriteRenderer spriteRenderer;
	public Collider2D collider;

	public float speed = 3;
	public float jumpSpeed = 10;
	public float initialSuperJumperSpeed;
	public float superJumpSpeed = 20;
	public float superJumpSpeedAceeleration;
	bool isSuperJumping = false;


	float speedTransitionRatio = 1.0f;
	Vector2 movingDirection = Vector2.zero;
	float speedTransitionTimeElapsed = 0;
	bool isPlayerOnGround = true;
	private void Awake()
	{
		Player.self = this;
	}
	
	// Use this for initialization
	void Start()
	{
	}
	void FixedUpdate()
	{

		var hitTestCenterTop = Physics2D.Raycast(this.transform.position,
			this.transform.up, 1.1f, LayerMask.GetMask("World"));
		var hitTestCenterBottom = Physics2D.Raycast(this.transform.position,
			-this.transform.up, 1.1f, LayerMask.GetMask("World"));
		var hitTestLeft = Physics2D.Raycast(this.transform.position - Vector3.right*0.5f,
			-this.transform.up, 1.1f, LayerMask.GetMask("World"));
		var hitTestRight = Physics2D.Raycast(this.transform.position + Vector3.right * 0.5f,
			-this.transform.up, 1.1f, LayerMask.GetMask("World"));
		float hitDistanceTop = (hitTestCenterTop.point - new Vector2(this.transform.position.x, this.transform.position.y)).magnitude;
		float hitDistanceBottom = (hitTestCenterBottom.point - new Vector2(this.transform.position.x, this.transform.position.y)).magnitude;

		bool isJumping = rigidbody.velocity.y > 0;
		bool isInsideWorldObject = hitDistanceBottom < 1.0f || hitDistanceTop < 1.0f;
		isPlayerOnGround = (hitTestCenterBottom.transform != null || 
			hitTestLeft.transform != null || hitTestRight.transform != null ) 
			&& !isInsideWorldObject && ! isJumping;
		//Debug.Log(hitTestCenterBottom.transform + " "+ hitDistanceBottom);
		//	Debug.Log("IS stuck " +isInsideWorldObject + " " +hitDistanceBottom);

		if (isJumping)
		{

			collider.enabled = false;
		}
		else
		{
			if (!isInsideWorldObject)
			{
				collider.enabled = true;
			}
		}
		fixedUpdateMove();
	}
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKey(KeyCode.A))
		{
			movingDirection = Vector2.left;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			movingDirection = Vector2.right;
		}
		else
		{
			movingDirection = Vector2.zero;
		}
		if (Input.GetKey(KeyCode.W))
		{
			jump();

		}

		bool isLeft = rigidbody.velocity.x < 0;
		bool isPlayerAlive = true;
		bool isPlayerMoving = rigidbody.velocity.x != 0;//|| rigidbody.velocity.y != 0;

		//turn around the sprite for left/right
		spriteRenderer.flipX = isLeft;

		animator.SetBool("isAlive", isPlayerAlive);
		animator.SetBool("isWalking", isPlayerMoving);
		animator.SetBool("isOnGround", isPlayerOnGround);

	}
	public void superJump()
	{
		isSuperJumping = true;
		rigidbody.velocity = new Vector2(rigidbody.velocity.x, initialSuperJumperSpeed);
		collider.enabled = false;

	}
	void jump()
	{
		if (!isPlayerOnGround) return;
		rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
		collider.enabled = false;
	}
	void fixedUpdateMove()
	{
		rigidbody.velocity = new Vector2( speed * movingDirection.x, rigidbody.velocity.y);
		if (isSuperJumping)
		{
			float newSuperJumpSpeed = rigidbody.velocity.y + superJumpSpeedAceeleration * Time.fixedDeltaTime;
			if(newSuperJumpSpeed> superJumpSpeed)
			{
				isSuperJumping = false;
				newSuperJumpSpeed = superJumpSpeed;
			}
			rigidbody.velocity = new Vector2(rigidbody.velocity.x,newSuperJumpSpeed - rigidbody.gravityScale * Physics2D.gravity.y *Time.fixedDeltaTime) ;
		}
	}
}
