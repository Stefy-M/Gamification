using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class incremental_item : MonoBehaviour {

	Rigidbody2D rb2d;
	public float vertspeed;
	public float destroyTimer;
	private int points;
	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		points = 0;
	}
	
	// Update is called once per frame
	void Update () {
		rb2d.velocity = new Vector2(0, 1 * vertspeed);
		Destroy(gameObject, destroyTimer);
		
		
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag.Equals("Player"))
		{
			//add in things for collecting 
			Destroy(gameObject);
			

		}
	}

	public int Points
	{
		get { return points; }
		set { this.points = value; }
	}

}
