using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	//This will be a gameManager class

	public static GameController instance;
	public GameObject [] Clouds = new GameObject[2];
	public Camera camera;
	public ScoreScript Score;
	public ddPlayer daredevilPlayer;
	private float speedMultiplier =1;
	private int difficulty = 1;
	public Text HSText;
	



	public bool isPlayerStopped = false; // used to signal when the player has reached lowest Y position
	private float inGameScore;
	private int inGameCoins;
	private float highestScore;
    private GameObject coinSpawner;
    private GameObject powerUpSpawner;


	
	// Use this for initialization
	void Start () {
		
		Clouds[0].SetActive(true);
		Clouds[1].SetActive(false);
        //HSText.text = PlayerPrefs.GetFloat("HighScore", 0).ToString();
        //highestScore = PlayerPrefs.GetFloat("HighScore", 0);
        //Online saving/loading for high score added here
        HSText.text = player.daredevilSave.score.ToString();
        highestScore = player.daredevilSave.score;
        player.daredevilSave.timesPlayed++;
        //get spawners to reduce coin spawn time and power up spawn rate for perk level tie in
        coinSpawner = GameObject.Find("Spawns/Coin_spawner");
        powerUpSpawner = GameObject.Find("Spawns/SlowItem_spawner");
        coinSpawner.GetComponent<BirdSpawning>().spawnRate -= player.Incre.daredevilPerkLevel;
        powerUpSpawner.GetComponent<BirdSpawning>().spawnRate -= player.Incre.daredevilPerkLevel;


    }
	
	// Update is called once per frame
	void Update ()
	{
		
		inGameScore =(float) Score.ScoreTime;
		inGameCoins = daredevilPlayer.CoinsCollected;

		if (inGameScore > highestScore)
		{
			//PlayerPrefs.SetFloat("HighScore", inGameScore);
            player.daredevilSave.score = inGameScore;
            HSText.text = player.daredevilSave.score.ToString();
            highestScore = player.daredevilSave.score;
            //HSText.text = PlayerPrefs.GetFloat("HighScore", 0).ToString();
			//highestScore = PlayerPrefs.GetFloat("HighScore", 0);

		}

	}

	private void FixedUpdate()
	{
		if (daredevilPlayer.ExecuteSlow )
		{
			//Debug.Log("SLOOOOOOW");
			speedMultiplier = 0.8f;
			Clouds[0].SetActive(true);
			Clouds[1].SetActive(false);
			camera.backgroundColor = new Color32(0, 168, 247, 255);
			//player.ExecuteSlow = false;
		}

		if ((inGameScore >= 10 && inGameScore < 20) && !daredevilPlayer.ExecuteSlow)
		{
			speedMultiplier = 1.2f;
			difficulty = 2;
		}

		if ((inGameScore >= 20 && inGameScore < 30) && !daredevilPlayer.ExecuteSlow)
		{
			speedMultiplier = 1.3f;
			difficulty = 3;
			
		}
		if ((inGameScore >= 30 && inGameScore < 40 )&& !daredevilPlayer.ExecuteSlow)
		{
			speedMultiplier = 1.5f;
			difficulty = 4;
			Clouds[0].SetActive(false);
			Clouds[1].SetActive(true);
			camera.backgroundColor = new Color32(44, 53, 165, 255);
		}

		if ((inGameScore >= 40 && inGameScore < 50) && !daredevilPlayer.ExecuteSlow)
		{
			speedMultiplier = 1.6f;
			
			difficulty = 5;
		}

		if (inGameScore >= 50 && !daredevilPlayer.ExecuteSlow)
		{
			speedMultiplier = 1.8f;
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

	public float HighestScore
	{
		get { return highestScore; }
	}
}
