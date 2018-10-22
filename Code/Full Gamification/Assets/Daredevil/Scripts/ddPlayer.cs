using Conqueror;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//here we can add save data to daredevil game
[System.Serializable]
public class DaredevilSave
{
    [JsonProperty("Score")]
    public float score { get; set; }
    [JsonProperty("Coins")]
    public int coins { get; set; }
    [JsonProperty("TimesPlayed")]
    public int timesPlayed { get; set; }
    

    public DaredevilSave()
    {
		score = 0;
		coins = 0;
		timesPlayed = 0;
    }
}

public class ddPlayer : MonoBehaviour {

	//Jumping variables *Not being used yet*
	bool grounded = false;
	float groundCheckRadius = 0.2f;
	public LayerMask groundLayer;
	public Transform groundCheck;


	//Player variables
	private float fallConstraintY; // point at which the player will stop falling
	public float maxSpeed;
	private Rigidbody2D rb2d;
	private int playerHP = 3;
	Animator myAnim;
	private static bool isStopped; // being used as a signal to tell when player has reached stopping point
	private bool facingRight;
	//public ScoreScript score;
	private bool isDead = false;
	public GameObject heart1, heart2, heart3;
	public ScoreScript playerScore;



	//In Game objects that effect player
	public incremental_item coin;
	private int coinsCollected;
	public GameObject restartButton,gameOverText;
	

	int playerLayer, enemyLayer, coinLayer; // used for collisions


	bool coroutineAllowed = true; // used to make player invisible after collision
	Color color;
	Renderer rend;
	
	private bool stopped = false;
	protected bool stoptimer = false;
	


	

	

	// Use this for initialization
	void Start () {

		
		
		playerLayer = this.gameObject.layer;
		coinLayer = LayerMask.NameToLayer("Coin");
		enemyLayer = LayerMask.NameToLayer("Enemy_bird");
		Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
		heart1 = GameObject.Find("heart1");
		heart2 = GameObject.Find("heart2");
		heart3 = GameObject.Find("heart3");
		heart1.SetActive(true);
		heart2.SetActive(true);
		heart3.SetActive(true);
		rend = GetComponent<Renderer>();
		color = rend.material.color;
		coin = new incremental_item();
		rb2d = GetComponent<Rigidbody2D>();
		myAnim = GetComponent<Animator>();
		
		

		// track if player is facing right
		facingRight = true;


		fallConstraintY = -20; // player will stop at position in the Y coord of the game 

		restartButton.SetActive(false);
		gameOverText.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

		
		if (transform.position.y < fallConstraintY) // condition to check when player has reached lowY
		{
			stopped = true;
			maxSpeed = 20; // speed up player movement
			myAnim.SetBool("isGrounded", false); //start parachute animation
			transform.position = new Vector3(transform.position.x, fallConstraintY, transform.position.z); // player will only be able to move Horizontal

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
			
			playerHP -= 1;
			switch (playerHP) {
				case 2:
					heart3.gameObject.SetActive(false);

					//courttine used to allow player to be invisible for short amount of time
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
				Debug.Log("Score: " + GameController.instance.InGameScore + " Coins Collected: " + coin.Points);
				isDead = true;
				stoptimer = true;
				gameOverText.SetActive(true);
				restartButton.SetActive(true);
				
				
			}
		}

		if (collision.gameObject.tag.Equals("Coin")) // when player collides with coin
		{
			coin.Points++;
			
		}
	}

	IEnumerator Immortal()
	{
		coroutineAllowed = false;
		Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true); // player cannot collide
		color.a = 0.5f;
		rend.material.color = color; // causes the color to fade
		yield return new WaitForSeconds(3f); // 3 sec timer of 'invisibility'
		Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
		color.a = 1f;
		rend.material.color = color;
		coroutineAllowed = true;
	}

	private void FixedUpdate()
	{
		//check if we are grounded, if no then we are falling
		if (isDead == false)
		{
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
		if (isDead == true)
		{

			myAnim.SetTrigger("Die");
			Physics2D.IgnoreLayerCollision(playerLayer, coinLayer, true); //player cant collect shrooms when dead
		}

	}

	public int HP
	{
		get { return this.playerHP; }

		set { this.playerHP = value; }

	}

	public int CoinsCollected
	{
		get { return coin.Points; }
	}

	
	public bool DeadStatus
	{
		get { return isDead; }
	}

	private void flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale; // taking values from transform
		theScale.x *= -1;

		transform.localScale = theScale;
	}
}
