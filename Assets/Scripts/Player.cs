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

	public AudioSource adoSrcJump;
	public AudioSource adoSrcSuperJump;
	public AudioSource adoSrcLand;
	public AudioSource adoSrcDead;

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
	bool isAlive = true;

	private void Awake()
	{
		Player.self = this;
	}
	void updateCollision()
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

		bool isMovingUpward = rigidbody.velocity.y > 0;
		bool isInsideWorldObject = hitDistanceBottom < 1.0f || hitDistanceTop < 1.0f;
		animator.SetBool("isInside", isInsideWorldObject);
		bool oldIsPlayerOnGround = isPlayerOnGround;
		isPlayerOnGround = !isInsideWorldObject && (rigidbody.velocity.y==0) && (
			hitTestCenterBottom.transform != null ||
			hitTestLeft.transform != null ||
			hitTestRight.transform != null);
		if(isPlayerOnGround && !oldIsPlayerOnGround)
		{
			//player landed
			adoSrcLand.time = 0.122f;
			adoSrcLand.Play();
		}
		//Debug.Log(hitTestCenterBottom.transform + " "+ hitDistanceBottom);
		//	Debug.Log("IS stuck " +isInsideWorldObject + " " +hitDistanceBottom);

		if (isMovingUpward)
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
	}
	private void FixedUpdate()
	{

		if (!isAlive) return;
		fixedUpdateMove();
	}
	public void controllerMoveLeft()
	{
		movingDirection = Vector2.left;

	}
	public void controllerMoveRight()
	{
		movingDirection = Vector2.right;

	}
	public void controllerMoveStop()
	{
		movingDirection = Vector2.zero;

	}
	// Update is called once per frame
	void Update()
	{
		if (!isAlive) return;
		updateCollision();

		if (Input.GetKey(KeyCode.A))
		{
			controllerMoveLeft();
		}
		else if (Input.GetKey(KeyCode.D))
		{
			controllerMoveRight();
		}
		else
		{
			//controllerMoveStop();
		}
		if (Input.GetKey(KeyCode.W))
		{
			jump();

		}

		bool isLeft = rigidbody.velocity.x < 0;
		bool isPlayerMoving = rigidbody.velocity.x != 0;//|| rigidbody.velocity.y != 0;

		//turn around the sprite for left/right
		spriteRenderer.flipX = isLeft;

		animator.SetBool("isWalking", isPlayerMoving);
		animator.SetBool("isOnGround", isPlayerOnGround);

		if(this.transform.position.x < 0.5 || this.transform.position.x > 9.5)
		{
			float x = this.transform.position.x;
			x = Mathf.Max(0.5f, x);
			x = Mathf.Min(9.5f, x);
			this.transform.position = new Vector3(x, this.transform.position.y, this.transform.position.z);
		}
	}
	public override void reset()
	{
		base.reset();
		isAlive = true;
		isPlayerOnGround = true;
		rigidbody.transform.position = new Vector3(5, 1.5f, 0);
		rigidbody.isKinematic = false;
		animator.SetBool("isAlive", isAlive);
	}
	public void kill()
	{
		if (!isAlive) return;
		isAlive = false;
		rigidbody.isKinematic = true;
		rigidbody.velocity = Vector2.zero;
		animator.SetBool("isAlive", isAlive);
		adoSrcDead.Play();
	}
	public void superJump()
	{
		isSuperJumping = true;
		rigidbody.velocity = new Vector2(rigidbody.velocity.x, initialSuperJumperSpeed);
		collider.enabled = false;
		adoSrcSuperJump.Play();

	}
	public void jump()
	{
		if (!isPlayerOnGround) return;
		rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
		collider.enabled = false;
		adoSrcJump.Play();
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
