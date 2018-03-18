using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

namespace Conqueror {
    public enum GunType {
        SingleSpray = 0,
        DoubleSpray = 1,
        TripleSpray = 2,
        Laser = 3
    }

    public class GunScript : MonoBehaviour
    {
        public void Awake()
        {
        }
    }

    /// <summary>
    /// Gun. Plan is to make certain archetypes of weapons and have those scale.
    /// </summary>
    public class Gun {
        public float damage;
        public float rof;
        public float velocity;
        public GunType type;

        GameObject bullet;
        GameObject shooter;

        float timeLastFired;

        private CountdownScript cs = GameObject.Find("GameManager").GetComponent<CountdownScript>();

        public string name {
            get
            {
                string ts = "";

                switch (type)
                {
                    case GunType.SingleSpray:
                        ts = "A";
                        break;
                    case GunType.DoubleSpray:
                        ts = "B";
                        break;
                    case GunType.TripleSpray:
                        ts = "C";
                        break;
                    case GunType.Laser:
                        ts = "D";
                        break;
                }

                return "D" + damage.ToString(".00") + "R" + rof.ToString(".00") + "V" + velocity.ToString(".00") + ts;
            }
        }

        public Gun()
            : this(1f, 30f/111, 20f, GunType.SingleSpray) {}

        public Gun(float d, float r, float v, int t)
            : this(d, r, v, (GunType)t) {}

        public Gun(float d, float r, float v, GunType t)
        {
            damage = d;
            rof = r; // in seconds
            velocity = v;
            type = t;
            bullet = GameManager.instance.bulletPrefab;
            ResetFireRate();
        }

        public void SetParent(GameObject p)
        {
            shooter = p;
        }

        public void Fire()
        {
            if (AttemptFire())
            {
                FireShot();
                DelayFireRate();
            }
        }

        public bool AttemptFire()
        {
            if (Time.time <= (timeLastFired + rof))
                return false;
            return true;
        }

        public void ResetFireRate() // So you can immediately fire again
        {
            timeLastFired = Time.time - rof;
        }

        public void DelayFireRate()
        {
            timeLastFired = Time.time;
        }

        public void FireShot()
        {
            // mouse's relative position to ship
            var mousePos = (Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - shooter.transform.position);
            var dir = mousePos.normalized;
            var bPos = shooter.transform.position + (Vector3)(dir * 0.2f);

            switch (type) {
                case GunType.SingleSpray:
                    {
                        var b = GameObject.Instantiate(bullet, bPos, Quaternion.identity);
                        b.GetComponent<Rigidbody2D>().velocity = dir * velocity;
                        b.GetComponent<Bullet>().damage = damage;
                        break;
                    }
                case GunType.DoubleSpray:
                    {
                        var b1 = GameObject.Instantiate(bullet, bPos, shooter.transform.rotation);
                        b1.GetComponent<Bullet>().damage = damage * .8f;
                        var b2 = GameObject.Instantiate(bullet, bPos, shooter.transform.rotation);
                        b2.GetComponent<Bullet>().damage = damage * .8f;

                        var angle = Vector2.SignedAngle(Vector2.right, dir);
                        var leftAngle = new Vector2();
                        var rightAngle = new Vector2();
                        leftAngle.x = Mathf.Cos(Mathf.Deg2Rad * (angle - 25));
                        leftAngle.y = Mathf.Sin(Mathf.Deg2Rad * (angle - 25));
                        rightAngle.x = Mathf.Cos(Mathf.Deg2Rad * (angle + 25));
                        rightAngle.y = Mathf.Sin(Mathf.Deg2Rad * (angle + 25));

                        b1.GetComponent<Rigidbody2D>().velocity = leftAngle * velocity;
                        b2.GetComponent<Rigidbody2D>().velocity = rightAngle * velocity;
                        break;
                    }
                case GunType.TripleSpray:
                    {
                        var b = GameObject.Instantiate(bullet, bPos, Quaternion.identity);
                        b.GetComponent<Bullet>().damage = damage * .8f;
                        var b1 = GameObject.Instantiate(bullet, bPos, shooter.transform.rotation);
                        b1.GetComponent<Bullet>().damage = damage * .6f;
                        var b2 = GameObject.Instantiate(bullet, bPos, shooter.transform.rotation);
                        b2.GetComponent<Bullet>().damage = damage * .6f;

                        var angle = Vector2.SignedAngle(Vector2.right, dir);
                        var leftAngle = new Vector2();
                        var rightAngle = new Vector2();
                        leftAngle.x = Mathf.Cos(Mathf.Deg2Rad * (angle - 50));
                        leftAngle.y = Mathf.Sin(Mathf.Deg2Rad * (angle - 50));
                        rightAngle.x = Mathf.Cos(Mathf.Deg2Rad * (angle + 50));
                        rightAngle.y = Mathf.Sin(Mathf.Deg2Rad * (angle + 50));

                        b.GetComponent<Rigidbody2D>().velocity = dir * velocity;
                        b1.GetComponent<Rigidbody2D>().velocity = leftAngle * velocity;
                        b2.GetComponent<Rigidbody2D>().velocity = rightAngle * velocity;
                        break;
                    }
                case GunType.Laser:
                    {
                        var b = GameObject.Instantiate(bullet, bPos, Quaternion.identity);
                        b.GetComponent<Bullet>().damage = damage * 2f;
                        break;
                    }
            }
        }
    }
}