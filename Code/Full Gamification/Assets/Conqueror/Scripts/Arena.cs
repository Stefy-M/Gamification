using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Conqueror {
    public class Arena : MonoBehaviour {
        float time = 0f;
        float spawn = 0f;
        float stagetime = 300f;
        int rof = 20;
        int maxrof = 20;
        Text timeRemainingText;

        void Start()
        {
            timeRemainingText = GameObject.Find("Time").GetComponent<Text>();
        }

        void Update() {
            time += Time.deltaTime;
            spawn += Time.deltaTime;
            stagetime -= Time.deltaTime;

            if (SceneManager.GetActiveScene().name == "Farm") {
                timeRemainingText.text = "Time Remaining: " + stagetime.ToString() + "s";

                if (stagetime <= 0f)
                    SceneManager.LoadScene("Menu");

                if (spawn >= 3f) {
                    GameObject.Instantiate(Resources.Load("EnemyPrefab"), new Vector2(Random.Range(-8,8) , Random.Range(-8,8)), transform.rotation);
                    GameObject.Instantiate(Resources.Load("TurretPrefab"), new Vector2(Random.Range(-8,8) , Random.Range(-8,8)), transform.rotation);
                    spawn = 0f;
                }
            }

            switch (GameManager.instance.level) {
                case 4:
                    if (spawn >= 5f) {
                        GameObject.Instantiate(Resources.Load("EnemyPrefab"), new Vector2(Random.Range(-8,8) , Random.Range(-8,8)), transform.rotation);
                        spawn = 0f;
                    }
                    break;
                case 5:
                    if (spawn >= 2.5f) {
                        GameObject.Instantiate(Resources.Load("EnemyPrefab"), new Vector2(Random.Range(-8,8) , Random.Range(-8,8)), transform.rotation);
                        spawn = 0f;
                    }
                    break;
                case 6:
                    if (spawn >= 2f) {
                        GameObject.Instantiate(Resources.Load("EnemyPrefab"), new Vector2(Random.Range(-8,8) , Random.Range(-8,8)), transform.rotation);
                        spawn = 0f;
                    }
                    break;
                case 7:
                    if (spawn >= 1.5f) {
                        GameObject.Instantiate(Resources.Load("EnemyPrefab"), new Vector2(Random.Range(-8,8) , Random.Range(-8,8)), transform.rotation);
                        spawn = 0f;
                    }
                    break;
                case 8:
                    if (spawn >= 1f) {
                        GameObject.Instantiate(Resources.Load("EnemyPrefab"), new Vector2(Random.Range(-8,8) , Random.Range(-8,8)), transform.rotation);
                        GameObject.Instantiate(Resources.Load("TurretPrefab"), new Vector2(Random.Range(-8,8) , Random.Range(-8,8)), transform.rotation);
                        spawn = 0f;
                    }
                    break;
                case 9:
                    if (spawn >= 1f) {
                        GameObject.Instantiate(Resources.Load("EnemyPrefab"), new Vector2(Random.Range(-8,8) , Random.Range(-8,8)), transform.rotation);
                        GameObject.Instantiate(Resources.Load("TurretPrefab"), new Vector2(Random.Range(-8,8) , Random.Range(-8,8)), transform.rotation);
                        spawn = 0f;
                    }
                    break;
                case 10:
                    if (spawn >= .5f) {
                        GameObject.Instantiate(Resources.Load("EnemyPrefab"), new Vector2(Random.Range(-8,8) , Random.Range(-8,8)), transform.rotation);
                        GameObject.Instantiate(Resources.Load("TurretPrefab"), new Vector2(Random.Range(-8,8) , Random.Range(-8,8)), transform.rotation);
                        spawn = 0f;
                    }
                    break;
            }

            //make turrets shoot
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Turret");
            Vector3 playerPos = GameObject.Find("player").transform.position;

            for (int i = 0; i < enemies.Length; i++) {
                if (enemies[i]) {
                    playerPos.x -= enemies[i].transform.position.x;
                    playerPos.y -= enemies[i].transform.position.y;
                    playerPos.Normalize();
                    enemies[i].GetComponent<Rigidbody2D>().AddForce(playerPos * 5);
                    rof--;

                    if (rof <= 0) {
                        Vector3 p = GameObject.Find("player").transform.position;
                        p.x -= enemies[i].transform.position.x;
                        p.y -= enemies[i].transform.position.y;
                        p = p.normalized;
                        GameObject projectile = (GameObject)GameObject.Instantiate(Resources.Load("BossBulletPrefab"), enemies[i].transform.position, enemies[i].transform.rotation);
                        projectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(p.x * 1000, p.y * 1000));
                        rof = maxrof;
                    }
                }
            }
        }

        void OnCollisionEnter2D(Collision2D col) {
            if (SceneManager.GetActiveScene().name == "Scene3" && col.gameObject.tag == "BossBullet") {
                GameObject rocket = (GameObject)GameObject.Instantiate (Resources.Load("BossBulletPrefab"), col.gameObject.transform.position, col.gameObject.transform.rotation);
                rocket.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-4, 4) * 150, Random.Range (-4, 4) * 150));
            }

            if (col.gameObject.CompareTag("Bullet") || col.gameObject.CompareTag("BossBullet"))
                Destroy(col.gameObject);
        }
    }
}