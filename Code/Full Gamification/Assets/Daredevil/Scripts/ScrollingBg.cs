using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBg : MonoBehaviour {

	Rigidbody2D rb2d;
	BoxCollider2D box;
	public float scrollSpeed;
	private float bgVerticalLength;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		box = GetComponent<BoxCollider2D>();

		bgVerticalLength = 20;
		

		
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GameController.instance.isStopped()) // When player stops run scrolling background
		{
			rb2d.velocity = new Vector2(rb2d.velocity.x, GameController.instance.SpeedMultiplier * scrollSpeed);
			if (transform.position.y > 8)
			{
				repositionBg();
			}
		}
	}

	private void repositionBg()
	{
		Vector2 bgOffset = new Vector2(0, bgVerticalLength*2f);
		transform.position = (Vector2)transform.position - bgOffset;
	}
}
