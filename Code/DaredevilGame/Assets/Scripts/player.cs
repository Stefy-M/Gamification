using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {

	//Jumping variables
	bool grounded = false;
	float groundCheckRadius = 0.2f;
	public LayerMask groundLayer;
	public Transform groundCheck;
	public float jumpHeight;
	public ScoreScript score;

	public int playerHP = 3;
	public GameObject heart1, heart2, heart3;
	int playerLayer, enemyLayer;
	bool coroutineAllowed = true;
	Color color;
	Renderer rend;
	private bool stopped = false;

	protected bool stoptimer = false;
	public float maxSpeed;
	private Rigidbody2D rb2d;
	Animator myAnim;
	private bool facingRight;


	float lowY; // point at which the player will stop falling

	public static bool isStopped; // being used as a signal to tell when player has reached stopping point

	// Use this for initialization
	void Start () {
		playerLayer = this.gameObject.layer;
		enemyLayer = LayerMask.NameToLayer("Enemy");
		Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
		heart1 = GameObject.Find("heart1");
		heart2 = GameObject.Find("heart2");
		heart3 = GameObject.Find("heart3");
		heart1.SetActive(true);
		heart2.SetActive(true);
		heart3.SetActive(true);
		rend = GetComponent<Renderer>();
		color = rend.material.color;


		rb2d = GetComponent<Rigidbody2D>();
		myAnim = GetComponent<Animator>();

		// track if player is facing right
		facingRight = true;


		lowY = -20; // player will stop at position in the Y coord of the game 
	}
	
	// Update is called once per frame
	void Update () {
		
		if (transform.position.y < lowY) // condition to check when player has reached lowY
		{
			stopped = true;
			maxSpeed = 20; // speed up 
			myAnim.SetBool("isGrounded", false); //start parachute animation
			transform.position = new Vector3(transform.position.x, lowY, transform.position.z); // player will only be able to move Horizontal

			GameController.instance.isPlayerStopped = true; // trigger game Instance that player has stopped in Global controller

			
		}

		
	}
	public bool StopTimer
	{
		get { return this.stoptimer; }
		

	}
	public bool Stopped
	{
		get { return stopped; }


	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag.Equals("Enemy_bird")) // checking to test each location a bird is hit
		{
			Debug.Log("X:"+collision.rigidbody.position.x+"\n" +"Y: "+ collision.rigidbody.position.y);
			playerHP -= 1;
			switch (playerHP) {
				case 2:
					heart3.gameObject.SetActive(false);
					if (coroutineAllowed)
					{
						StartCoroutine("Immortal");
					}
					break;
				case 1:
					heart2.gameObject.SetActive(false);
					if (coroutineAllowed)
					{
						StartCoroutine("Immortal");
					}
					break;
				case 0:
					heart1.gameObject.SetActive(false);
					if (coroutineAllowed)
					{
						StartCoroutine("Immortal");
					}
					break;
			}
			if (playerHP < 1) // do game over here
			{
				
				stoptimer = true;
			}
		}
	}

	IEnumerator Immortal()
	{
		coroutineAllowed = false;
		Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
		color.a = 0.5f;
		rend.material.color = color;
		yield return new WaitForSeconds(3f);
		Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
		color.a = 1f;
		rend.material.color = color;
		coroutineAllowed = true;
	}

	private void FixedUpdate()
	{
		//check if we are grounded, if no then we are falling
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
		myAnim.SetBool("isGrounded", grounded);

		myAnim.SetFloat("vertSpeed", rb2d.velocity.y); 


		float move = Input.GetAxis("Horizontal");
		myAnim.SetFloat("speed", Mathf.Abs(move));
		rb2d.velocity = new Vector2(maxSpeed * move, rb2d.velocity.y);

		if (move > 0 && !facingRight) // if player moving right and isnt facing right, then flip
		{
			flip();
		}
		else if (move < 0 && facingRight) //  if player moving left and isnt facing left, then flip
		{
			flip();
		}
	}

	public int HP
	{
		get { return this.playerHP; }

		set { this.playerHP = value; }

	}

	private void flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale; // taking values from transform
		theScale.x *= -1;

		transform.localScale = theScale;
	}
}
