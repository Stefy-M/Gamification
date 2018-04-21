using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	//This will be a gameManager class

	public static GameController instance;
	
	
	//List<GameObject> list = new List<GameObject>();
	



	public bool isPlayerStopped = false; // used to signal when the player has reached lowest Y position



	
	// Use this for initialization
	void Start () {

		

		
	}
	
	// Update is called once per frame
	void Update ()
	{
		//rb.velocity = new Vector2(0, 1 * 10);
		

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

	public bool isStopped()
	{
		return isPlayerStopped;

	}
}
