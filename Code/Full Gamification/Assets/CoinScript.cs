using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour {

	public GameObject coin;
	float randX; // randomized position where the birds will spawn
	Vector2 whereToSpawn;
	public float spawnRate;
	float nextSpawn = 0.0f;
	private int coinsCollected;


	// Use this for initialization
	void Start()
	{
		coinsCollected = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (Time.time > nextSpawn)
		{
			nextSpawn = Time.time + spawnRate;
			randX = Random.Range(10.3f, 76.0f);
			whereToSpawn = new Vector2(randX, transform.position.y);
			Instantiate(coin, whereToSpawn, Quaternion.identity);
		}

	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag.Equals("Player")){
			coinsCollected++;
			Debug.Log("COOOOOOOOIIIIIIN: "+coinsCollected);
		}
	}

	public int CoinsCollected
	{
		get { return coinsCollected; }
	}
}
