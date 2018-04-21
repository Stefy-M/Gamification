using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shroom_fly : MonoBehaviour {

	Rigidbody2D rb2d;
	public float vertspeed;
	public float destroyTimer;
	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
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
			Destroy(gameObject);

		}
	}
}
