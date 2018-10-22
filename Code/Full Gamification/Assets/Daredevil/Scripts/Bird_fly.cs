using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_fly : MonoBehaviour {

	// Use this for initialization
	private Rigidbody2D birdRB;
	public float flySpeed;
	//public GameObject blood;



	void Start () {
		float k;
		
		birdRB = GetComponent<Rigidbody2D>();
		k = birdRB.transform.position.y;
		//Debug.Log("Y start pos: " + k);
		
	}
	
	// Update is called once per frame
	void Update () {
		birdRB.velocity = new Vector2(0, GameController.instance.SpeedMultiplier * flySpeed);
		Destroy(gameObject, 8f);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		//Instantiate(blood,transform.position,Quaternion.identity);

		if (collision.gameObject.tag.Equals("Player")) //when bird collides with player
		{
			Destroy(gameObject);

		}

		

		if (collision.gameObject.tag.Equals("Bird_enemy"))
		{
			
			birdRB.isKinematic = false;
			
		}
	}
}
