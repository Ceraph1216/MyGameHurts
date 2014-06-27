using UnityEngine;
using System.Collections;

public class BasicPlayerMovementScript : MonoBehaviour 
{
	public tk2dSprite mySprite;
	public LayerMask groundCheckMask;
	
	public Transform leftSensorTransform;
	public Transform rightSensorTransform;
	public Transform groundSensorTransform;

	public Transform hitboxes;

	public GameObject attackHitbox;

	private bool isHitR;
	private bool isHitL;
	private bool isHitG;
	private bool isHitForward;
	
	private Vector2 hitPointR = Vector2.zero;
	private Vector2 hitPointL = Vector2.zero;
	private Vector2 hitPointG = Vector2.zero;
	private Vector2 hitPointForward = Vector2.zero;

	private Rigidbody2D myRigidbody;
	private Transform myTransform;

	// Use this for initialization
	void OnEnable () 
	{
		myRigidbody = rigidbody2D;
		myTransform = transform;

		SoftPauseScript.instance.SoftUpdate += SoftUpdate;
	}
	
	// Update is called once per frame
	void SoftUpdate (GameObject dispatcher) 
	{
		DrawRays();

		// Get current velocity
		Vector3 newVelocity = myRigidbody.velocity;

		// Listen for button presses
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (PlayerStateManager.instance.currentGroundState == Enums.PlayerGroundState.OnGround)
			{
				newVelocity = myRigidbody.velocity;
				newVelocity.y =  Constants.JUMP_FORCE;
				myRigidbody.velocity = newVelocity;
			}
		}

		if (Input.GetKeyDown(KeyCode.J))
		{
			Attack();
		}

		// Apply horizontal movement
		newVelocity = myRigidbody.velocity;
		newVelocity.x =  Input.GetAxis("Horizontal") * Constants.RUN_SPEED;
		myRigidbody.velocity = newVelocity;

		// Make sure the sprite is facing the right way
		if (mySprite.scale.x > 0) // facing right
		{
			if (Input.GetAxis("Horizontal") < 0) // Moving left
			{
				Vector3 newScale = mySprite.scale;
				newScale.x *= -1;
				mySprite.scale = newScale;
				hitboxes.localScale = newScale;
			}
		}

		if (mySprite.scale.x < 0) // facing left
		{
			if (Input.GetAxis("Horizontal") > 0) // Moving right
			{
				Vector3 newScale = mySprite.scale;
				newScale.x *= -1;
				mySprite.scale = newScale;
				hitboxes.localScale = newScale;
			}
		}

		//----------------------
		// Set states
		//----------------------

		// Set in air states

		if (!isHitG)
		{
			if (myRigidbody.velocity.y > 0.5f)
			{
				if (PlayerStateManager.instance.currentGroundState != Enums.PlayerGroundState.Rising)
				{
					PlayerStateManager.instance.currentGroundState = Enums.PlayerGroundState.Rising;
				}
			}
			else if (myRigidbody.velocity.y < -0.5f)
			{
				if (PlayerStateManager.instance.currentGroundState != Enums.PlayerGroundState.Falling)
				{
					PlayerStateManager.instance.currentGroundState = Enums.PlayerGroundState.Falling;
				}
			}
		}
		else
		{
			if (PlayerStateManager.instance.currentGroundState != Enums.PlayerGroundState.OnGround)
			{
				PlayerStateManager.instance.currentGroundState = Enums.PlayerGroundState.OnGround;
			}
		}

		// Set running state
		if (Mathf.Abs(myRigidbody.velocity.x) > 0.5f && !PlayerStateManager.instance.isMoving)
		{
			PlayerStateManager.instance.isMoving = true;
		}
		else if (Mathf.Abs(myRigidbody.velocity.x) <= 0.5f && PlayerStateManager.instance.isMoving)
		{
			PlayerStateManager.instance.isMoving = false;
		}
	}

	void DrawRays()
	{
		//---------------------------------------------------------------------
		// Use raycasting to find where the ground is in relation to the player
		// and what angle the ground is they are on.
		//---------------------------------------------------------------------
		
		// Set the direction and distance of the ground check
		Vector2 downDirection = new Vector2(0, -1);// -myTransform.up;
		Vector2 forwardDirection = new Vector2(myTransform.localScale.x, 0);
		float distanceSide = 1.5f;
		float distanceGround = 1.5f;
		float distanceFront = 1.5f;
		
		// Create debug visualizations of the rays being used
		Debug.DrawRay (rightSensorTransform.position, downDirection * distanceSide, Color.green);
		Debug.DrawRay (leftSensorTransform.position, downDirection * distanceSide, Color.green);
		Debug.DrawRay (groundSensorTransform.position, downDirection * distanceGround, Color.red);
		
		//		Debug.DrawRay (topSensorTransform.position, forwardDirection * distanceFront, Color.yellow);
		//		Debug.DrawRay (bottomSensorTransform.position, forwardDirection * distanceFront, Color.yellow);
		
		// Create a raycast for each of the 3 sensor points
		RaycastHit2D hitR = Physics2D.Raycast(rightSensorTransform.position, downDirection, distanceSide, groundCheckMask);
		RaycastHit2D hitL = Physics2D.Raycast(leftSensorTransform.position, downDirection, distanceSide, groundCheckMask);
		RaycastHit2D hitG = Physics2D.Raycast(groundSensorTransform.position, downDirection, distanceGround, groundCheckMask);
		//		RaycastHit2D hitT = Physics2D.Raycast(topSensorTransform.position, forwardDirection, distanceFront, groundCheckMask);
		//		RaycastHit2D hitB = Physics2D.Raycast(bottomSensorTransform.position, forwardDirection, distanceFront, groundCheckMask);
		
		isHitR = false;
		isHitL = false;
		isHitG = false;
		
		hitPointR = Vector2.zero;
		hitPointL = Vector2.zero;
		hitPointG = Vector2.zero;
		hitPointForward = Vector2.zero;
		
		// The right sensor hit something
		if (hitR != null && hitR.collider != null)
		{
			isHitR = true;
			hitPointR = hitR.point;
		}
		
		// The left sensor hit something
		if (hitL != null && hitL.collider != null)
		{
			isHitL = true;
			hitPointL = hitL.point;
		}
		
		// The middle sensor hit something
		if (hitG != null && hitG.collider != null)
		{
			isHitG = true;
			hitPointG = hitG.point;
		}
	}

	void Attack()
	{
		attackHitbox.SetActive(true);
	}
}
