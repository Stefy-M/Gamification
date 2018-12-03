using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conqueror {
    public class Bullet : MonoBehaviour
    {
        public AudioClip gunSound1;
        public AudioClip gunSound2;
        public AudioClip damagedBoss;
        public float damage = 1;

        void Start()
        {
            SoundManager.instance.RandomizeSfx(gunSound1, gunSound2);

            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Quaternion rot = Quaternion.LookRotation(gameObject.transform.position - mousePosition, Vector3.forward);
            gameObject.transform.rotation = rot;
            gameObject.transform.eulerAngles = new Vector3(0, 0, gameObject.transform.eulerAngles.z);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Bullet"))
                return;
            
            if (other.CompareTag("Boss")) {
                var boss = other.gameObject.GetComponent<Boss>();
                //This increases damage based on conqueror perk level
                boss.hp -= damage;
                SoundManager.instance.PlaySingle(damagedBoss);
                Destroy(gameObject);
            }

            if (other.CompareTag("Enemy")) {
                var mouse = gameObject.transform.position;
                mouse.x -= other.gameObject.transform.position.x;
                mouse.y -= other.gameObject.transform.position.y;
                mouse = mouse.normalized;

                other.gameObject.GetComponent<Rigidbody2D>().AddForce(mouse * 20);

                Destroy(other.gameObject);
            }
        }
    }
}