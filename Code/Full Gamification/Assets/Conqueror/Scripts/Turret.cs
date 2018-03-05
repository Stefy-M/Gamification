using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conqueror {
    public class Turret : MonoBehaviour
    {
        public int index = 0;
        public int rof = 5;
        public int maxrof = 5;

        void Start()
        {
        }

        void FixedUpdate()
        {
            //turret.Shoot ();
        }

        public void Shoot()
        {
            Vector3 mouse = GameObject.Find("player").transform.position;

            mouse.x -= gameObject.transform.position.x;
            mouse.y -= gameObject.transform.position.y;
            mouse.Normalize();
            gameObject.GetComponent<Rigidbody2D>().AddForce(mouse * 5);
            rof--;

            if (rof <= 0) {
                Vector3 p = GameObject.Find ("player").transform.position;
                p.x -= gameObject.transform.position.x;
                p.y -= gameObject.transform.position.y;
                p.Normalize();
                GameObject projectile = (GameObject)GameObject.Instantiate(Resources.Load("BossBulletPrefab"), gameObject.transform.position, gameObject.transform.rotation);
                projectile.GetComponent<Rigidbody2D>().AddForce(p * 1000);
                rof = maxrof;
            }
        }
    }
}