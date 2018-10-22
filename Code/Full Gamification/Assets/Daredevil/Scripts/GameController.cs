using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	//This will be a gameManager class

	public static GameController instance;
	public ScoreScript Score;
	public ddPlayer player;
	private float speedMultiplier =1;
	private int difficulty = 1;
	



	public bool isPlayerStopped = false; // used to signal when the player has reached lowest Y position
	private float inGameScore;
	private int inGameCoins;


	
	// Use this for initialization
	void Start () {

		

		
	}
	
	// Update is called once per frame
	void Update ()
	{
		inGameScore =(float) Score.ScoreTime;
		inGameCoins = player.CoinsCollected;
		


	}

	private void FixedUpdate()
	{
		if (inGameScore >= 10 && inGameScore < 20)
		{
			speedMultiplier = 1.2f;
			difficulty = 2;
		}

		if (inGameScore >= 20 && inGameScore < 30)
		{
			speedMultiplier = 1.5f;
			difficulty = 3;
		}
		if (inGameScore >= 30 && inGameScore < 40)
		{
			speedMultiplier = 1.8f;
			difficulty = 4;
		}

		if (inGameScore >= 40 && inGameScore < 50)
		{
			speedMultiplier = 2.1f;
			difficulty = 5;
		}

		if (inGameScore >= 50)
		{
			speedMultiplier = 2.5f;
			difficulty = 6;
		}


	}

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}


	public float InGameScore
	{
		get { return inGameScore; }
	}

	public int InGameCoins
	{
		get { return InGameCoins; }
	}
	public bool isStopped()
	{
		return isPlayerStopped;

	}

	public int Difficulty
	{
		get { return difficulty; }
	}

	public float SpeedMultiplier
	{
		get { return speedMultiplier; }
	}
}
