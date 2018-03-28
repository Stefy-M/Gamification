using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Conqueror {
    public class Boss : MonoBehaviour
    {
        public float hp = 10f;
        public float maxhp = 10f;
        public float rof = 8f;
        public float maxrof = 8f;
        public int index = 1;
        public float movespeed = 1f;

        public GameObject bossDrop;

        private GameObject rocket;
        private float counter = 0f;
        private int level = 0;
        public Text bossName;
        public Slider bossHealthBar;

        private float moveTimer = 2;
        private float moveCooldown = 2;
        private bool movingRight = false;
        private Vector3 targetPosition;

        //Allows access to the setCountDown class inorder to access the countDownDone variable.
		private CountdownScript cs;

        void Start()
        {
			cs = GameObject.Find("GameManager").GetComponent<CountdownScript>();
            level = GameManager.instance.level;

            maxhp = 20 + 10 * (Mathf.Pow(1.3f, level));
            hp = maxhp;
            maxrof = (30f / (level * 0.5f));
            rof = maxrof;
            movespeed = Mathf.Max(5f, Mathf.Log(level, 2f) * 2);

            bossName = GameObject.Find("EnemyName").GetComponent<Text>();
            bossName.text = "Boss Lv" + (level+1);
            bossHealthBar = GameObject.Find("EnemyHealth").GetComponent<Slider>();
            bossHealthBar.minValue = 0;
            bossHealthBar.maxValue = maxhp;
            bossHealthBar.value = hp;
        }

        void FixedUpdate()
        {
            counter += Time.deltaTime;

            if (counter >= 3f) {
                if (index == 0)
                    index = 1;
                else 
                    index = 0;

                counter = 0f;
            }

			
            //Add logic here to get the boss to not shoot while the countDownDone is still false. The boss will only shoot when the countDownDone is true.
            if (cs.countDownDone == true)
            {
                Move();
                //added shoot here too because boss shouldn't be able to shoot if player can't
                Shoot();
            }
			            
        }

        void Update()
        {
            bossHealthBar.value = hp;
            if (hp <= 0) {
                GameObject.Instantiate(bossDrop, gameObject.transform.position, gameObject.transform.rotation);
                GameManager.instance.bossesLeft--;
                Destroy(gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Arena"))
                return;
            if (other.CompareTag("Bullet"))
                return;
            if (other.CompareTag("BossBullet"))
                return;
            if (other.CompareTag("Player"))
                return;
            if (other.CompareTag("Boss"))
                return;

            //Will destroy boss drops if it comes in contact (this might be an alright feature to leave in for now because that is the only way to get rid of the boss drop.
            //Later we could add an option to accept the boss drop and if user selects no it will be destroyed.
            Destroy(other.gameObject);
        }

        public void Move()
        {
            // These should really be going into state machines
            var player = GameObject.FindGameObjectWithTag("Player");
            switch (level % 4)
            {
                case 0:
                    moveTimer = 0;
                    // Move towards player type
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Random.Range(0, Mathf.Min(movespeed / 2f, 8f)) * Time.fixedDeltaTime);
                    break;
                case 1:
                    // Evading type
                    moveTimer -= Time.fixedDeltaTime;
                    if (moveTimer <= 0) {
                        if (transform.position.x > player.transform.position.x)
                            movingRight = false;
                        else
                            movingRight = true;
                        
                        moveTimer = Random.Range(moveCooldown * .2f, moveCooldown * .5f);
                    }

                    if (movingRight)
                        transform.Translate(new Vector3(movespeed * Time.fixedDeltaTime, 0f));
                    else
                        transform.Translate(new Vector3(-movespeed * Time.fixedDeltaTime, 0f));
                    break;
                case 2:
                    moveTimer = 0;
                    // Revolve around player type
                    break;
                case 3:
                    // Randumb type
                    moveTimer -= Time.fixedDeltaTime;
                    if (moveTimer <= 0)
                    {
                        targetPosition = new Vector3(Random.Range(-6f, 6f), Random.Range(-3f, 3f), 0);
                        moveTimer = moveCooldown;
                    }
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, movespeed * Time.fixedDeltaTime);
                    break;
            }
        }

        public void Shoot()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            switch (index)
            {
                case 0:
                    rof--;
                    if (rof <= 0)
                    {
                        var rocket = (GameObject)GameObject.Instantiate(GameManager.instance.bossBulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
                        rocket.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-4, 4) * level, Random.Range(-4, 4) * level);
                        rof = maxrof;
                    }
                    break;
                case 1:
                    rof--;
                    if (rof <= 0)
                    {
                        var rocket2 = (GameObject)GameObject.Instantiate(GameManager.instance.bossBulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
                        rocket2.GetComponent<Rigidbody2D>().velocity = directionToPlayer() * level;
                        rof = maxrof;
                    }
                    break;
            }
        }

        public Vector2 directionToPlayer()
        {
            var playerDir = GameObject.FindGameObjectWithTag("Player").transform.position;
            playerDir.x -= gameObject.transform.position.x;
            playerDir.y -= gameObject.transform.position.y;
            playerDir.Normalize();

            return playerDir;
        }
    }
}