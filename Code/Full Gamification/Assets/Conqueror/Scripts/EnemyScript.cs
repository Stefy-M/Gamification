using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conqueror {
    public class EnemyScript : MonoBehaviour
    {
        Enemy enemy;

        public class Enemy
        {
            public GameObject enemy;
            public int index = 0;
            public int rof = 10;
            public int maxrof = 10;

            public Enemy()
            {
                enemy = GameObject.FindGameObjectWithTag("Enemy");
            }

            public void Shoot()
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                Vector3 mouse = GameObject.Find("player").transform.position;
                for (int i = 0; i < enemies.Length; i++) {
                    if (enemies[i]) {
                        mouse.x -= enemies[i].transform.position.x;
                        mouse.y -= enemies[i].transform.position.y;
                        mouse = mouse.normalized;
                        enemies[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(mouse.x * 5, mouse.y * 5));
                        rof--;

                        if (rof <= 0) {
                            GameObject rocket = (GameObject)GameObject.Instantiate(Resources.Load("BossBulletPrefab"), enemies[i].transform.position, enemies[i].transform.rotation);
                            rocket.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range (-4, 4) * 150, Random.Range(-4, 4) * 150));
                            rof = maxrof;
                        }
                    }
                }
            }

            public void Move()
            {
                GameObject[] enemies;
                Vector3 mouse = GameObject.Find("player").transform.position;

                enemies = GameObject.FindGameObjectsWithTag("Enemy");
                for (int i = 0; i < enemies.Length; i++) {
                    if (enemies[i]) {
                        mouse.x -= enemies[i].transform.position.x;
                        mouse.y -= enemies[i].transform.position.y;
                        mouse = mouse.normalized;
                        enemies [i].GetComponent<Rigidbody2D> ().AddForce (new Vector2 (mouse.x * 5, mouse.y * 5));
                    }
                }
            }
        }

        void Start()
        {
            enemy = new Enemy();
        }

        void FixedUpdate()
        {
            enemy.Shoot();
            enemy.Move();
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.name == "player") {
                GameObject.Find("player").GetComponent<PlayerShip>().health--;
                GameObject.Instantiate(Resources.Load("BossDropPrefab"), enemy.enemy.transform.position, enemy.enemy.transform.rotation);
            }
            /*
            if (col.gameObject.name == "BulletPrefab")
                Destroy (GameObject.Find ("Enemy"));
            */
        }
    }

}