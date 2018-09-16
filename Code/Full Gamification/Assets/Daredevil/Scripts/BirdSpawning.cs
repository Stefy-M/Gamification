using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawning : MonoBehaviour {

	public GameObject bird;
	float randX;
	Vector2 whereToSpawn;
	public float spawnRate;
	float nextSpawn = 0.0f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > nextSpawn)
		{
			nextSpawn = Time.time + spawnRate;
			randX = Random.Range(10.3f, 94.0f);
			whereToSpawn = new Vector2(randX, transform.position.y);
			Instantiate(bird, whereToSpawn, Quaternion.identity);
		}
		
	}
}
