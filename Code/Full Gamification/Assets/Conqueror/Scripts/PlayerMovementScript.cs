using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class conqueror
{
    public Gun g;
    public int hp;
    public float damage;
    public int skill;
    public Gun[] guns;
}
public class Player {
	public Gun g = new Gun(1,1,0);
	public int hp = 30;
	public float damage = 1;
	public int skill = 2;
	public Gun[] guns = new Gun[9];
	public int skillcd = 100;
	public GameObject player;
	public float movespeed = 5f;
	float movex;
	float movey;
	public int rof = 20;
	public Player() {
        //damage, speed, type
        int g1,g2,g3,ind;
        //guns [0] = new Gun (1,150,0);
		//guns [1] = new Gun (1,150,1);
		//guns [2] = new Gun (1,200,2);
		g = guns [0];
		//saving/loading
        if (!PlayerPrefs.HasKey("ConqSave"))
        {
            PlayerPrefsX.SetStringArray("ConqSave", Resources.Load<TextAsset>("save").text.Split('\n'));
        }
		string[] lines = PlayerPrefsX.GetStringArray("ConqSave");
		int.TryParse(lines[1].Split(' ')[1],out hp);
		float.TryParse(lines[2].Split(' ')[1],out damage);
		int.TryParse(lines[3].Split(' ')[1],out skill);

		int i = 0;
		while (i < lines.Length - 5) {
			if (lines [i + 5].Contains ("gun")) {
				if (i < guns.Length) {
					int.TryParse (lines [i + 5].Split (' ') [1], out g1);
					int.TryParse (lines [i + 5].Split (' ') [2], out g2);
					int.TryParse (lines [i + 5].Split (' ') [3], out g3);
					guns [i] = new Gun (g1 * damage, g2, g3);
				}
			}
			i++;
		}

        int.TryParse (lines [4].Split (' ') [1], out ind);
		g = guns[ind];
		/*
		guns [0].rof = 6;
		guns [0].maxrof = 6;
		guns [0].damage = .2f;
		guns [0].speed = 500;
		*/
	}

	public void useSkill() {
		skillcd--;
		if (skillcd > 0) {
			GameObject.Find ("Skillcd").GetComponent<Text> ().text = "Skill " + skillcd.ToString ();
		}
		else { GameObject.Find ("Skillcd").GetComponent<Text> ().text = "Skill 0"; }
		if (Input.GetKeyDown (KeyCode.Space)) {
			player = GameObject.Find ("player");

			if (skillcd <= 0) {
				//Debug.Log (skill.ToString ());
				switch (skill) {
				case 0:
					player.GetComponent<Rigidbody2D> ().velocity = new Vector2 (movex * movespeed, movey * movespeed);
					skillcd = 50;

					break;

				case 1:
					g.speed = g.speed * 2;
					g.shoot ();
					g.shoot ();
					g.shoot ();
					g.shoot ();
					g.speed = g.speed / 2;
					skillcd = 100;
			//double time
					break;

				case 2:
					for (int i = 0; i < 20; i++) {
					GameObject rocket = (GameObject)GameObject.Instantiate (Resources.Load ("BulletPrefab"), player.transform.position, player.transform.rotation);
						rocket.GetComponent<Rigidbody2D> ().velocity = new Vector2 (Random.Range (-4, 4), Random.Range (-4, 4));
					}
					skillcd = 100;
					break;
				}
			}
		}
	}

    // I fixed your movements so that it works better. Also locked the ship so it didn't rotate everywhere. -Ryan

	//public void Move() {
	//	movex = Input.GetAxis ("Horizontal");
	//	movey = Input.GetAxis ("Vertical");
	//	player.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (movex * movespeed, movey * movespeed));
	//	if (player.GetComponent<Rigidbody2D> ().velocity.magnitude > 5) {
	//		player.GetComponent<Rigidbody2D> ().velocity = player.GetComponent<Rigidbody2D> ().velocity.normalized * 5;
	//	}
	//}
}

public class PlayerMovementScript : MonoBehaviour {
	
	// Use this for initialization
	public Player player1;
    //public conqueror cplayer;
    private Rigidbody2D _player;
	Vector3 xyvec;

	GameObject rocket;
	void Start () {
		player1 = new Player ();
        //player1 = player.Conqueror;

		if (player.Conqueror.hp != 30) {
			//init values
			player.Conqueror.g = new Gun(1, 150, 0);
			player.Conqueror.hp = 30;
			player.Conqueror.damage = .3f;
			player.Conqueror.skill = 2;
			player.Conqueror.guns = new Gun[9];
			player.Conqueror.guns [0] = player.Conqueror.g;
		}
        if (player.Conqueror != null)
        {
			player1.hp = 30;
            player1.damage = player.Conqueror.damage;
            player1.g = player.Conqueror.g;
            player1.skill = player.Conqueror.skill;
            player1.guns = player.Conqueror.guns;
        }
        else
        {
            player1.g = new Gun(1, 150, 0);
            player1.hp = 30;
            player1.damage = .3f;
            player1.skill = 2;
            player1.guns = new Gun[9];
        }
        _player = GetComponent<Rigidbody2D>();
		GameObject healthslider = GameObject.Find("PlayerHP");
		healthslider.GetComponent<Slider> ().minValue = 0;
		healthslider.GetComponent<Slider> ().maxValue = player1.hp;
		healthslider.GetComponent<Slider>().value = player1.hp;
	}
		
	// Update is called once per frame

	void FixedUpdate () {
        //update conqueror fields for db
       // player.Conqueror.hp = player1.hp;
        player.Conqueror.damage = player1.damage;
        player.Conqueror.g = player1.g;
        player.Conqueror.skill = player1.skill;
        player.Conqueror.guns = player1.guns;

        //change rate of fire
        player1.g.rof--;
		if ((Input.GetMouseButton (1) && player1.g.rof <= 0) || (Input.GetMouseButton(0) && player1.g.rof <= 0)) {
			player1.g.shoot();
			player1.g.rof = player1.g.maxrof;
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadScene("Menu");
		}

        // New movement script
        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -.5f)
        {
            _player.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * player1.movespeed, _player.velocity.y);
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -.5f)
        {
            _player.velocity = new Vector2(_player.velocity.x, Input.GetAxisRaw("Vertical") * player1.movespeed);
        }
        if (Input.GetAxisRaw("Horizontal") < 0.5f && Input.GetAxisRaw("Horizontal") > -.5f)
        {
            _player.velocity = new Vector2(0f, _player.velocity.y);
        }
        if (Input.GetAxisRaw("Vertical") < 0.5f && Input.GetAxisRaw("Vertical") > -.5f)
        {
            _player.velocity = new Vector2(_player.velocity.x, 0f);
        }

        //player1.Move ();
        //if (GameObject.Find ("player").transform.position.y < -9) {
        //	GameObject.Find ("player").transform.position = new Vector2 (GameObject.Find ("player").transform.position.x, -8.5f);
        //} else if (GameObject.Find ("player").transform.position.y > 9) {
        //	GameObject.Find ("player").transform.position = new Vector2 (GameObject.Find ("player").transform.position.x, 8.5f);
        //}

        //if (GameObject.Find ("player").transform.position.x < -9) {
        //	GameObject.Find ("player").transform.position = new Vector2 (-8.5f,GameObject.Find ("player").transform.position.y);
        //} else if (GameObject.Find ("player").transform.position.y > 9) {
        //	GameObject.Find ("player").transform.position = new Vector2 (8.5f,GameObject.Find ("player").transform.position.y);
        //}


		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			player1.g = player1.guns [0];
			GameObject.Find ("Text").GetComponent<Text>().text = "Weapon 1";
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			player1.g = player1.guns [1];
			GameObject.Find ("Text").GetComponent<Text>().text = "Weapon 2";
		}
		 else if (Input.GetKeyDown (KeyCode.Alpha3)) {
		player1.g = player1.guns [2];
			GameObject.Find ("Text").GetComponent<Text>().text = "Weapon 3";
		}

		//skills
			player1.useSkill ();
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.name == "Arena")
			return;
		if (col.gameObject.name == "BulletPrefab")
			return;
		if (col.gameObject.name == "EnemyPrefab") {
			return;
		}
		player1.hp--;
		//update slider
		GameObject healthslider = GameObject.Find("PlayerHP");
		healthslider.GetComponent<Slider> ().value = player1.hp;

		Destroy (col.gameObject);
		if (player1.hp <= 0) {
			//Destroy(GameObject.Find("player"));
			SceneManager.LoadScene ("Menu");

			print ("COLLISION");
		}
	}


}
