using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

namespace Conqueror {
    [System.Serializable]
    public class ConquerorSave 
    {
        public List<Gun> guns = new List<Gun>();
        public int currentGun = 0;
        public int currentSkill = 0;
        public int highestLevelReached = 0; // Starts at 0. 0 = Level 1.

        public ConquerorSave()
        {
        }
    }

    public class PlayerShip : MonoBehaviour
    {
        public AudioClip playerHit;
        public Slider healthBar;
        public Slider skillCooldownBar;

        public int health;
        int maxHealth = 30;
        public float moveSpeed;
        float defaultMoveSpeed = 5;

        private Gun workingGun;
        private Vector3 defaultPos = new Vector3(0f, -2.8f, 0f);

        public Gun WorkingGun
        {
            get
            {
                return workingGun;
            }
            set
            {
                value.SetParent(gameObject);
                workingGun = value;
            }
        }

        float skillTimer = 0;
        float skillCooldownTime;

        public static string PlayerSkillToString(int i)
        {
            switch (i)
            {
                case 0:
                    return "Blue Blur";
                case 1:
                    return "Super Soaker";
                case 2:
                    return "Bulletsplosion";
                default:
                    return "None";
            }
        }

    	void Start()
        {
            Init();
        }

        public void Init()
        {
            transform.position = defaultPos;
            health = maxHealth;
            moveSpeed = defaultMoveSpeed;

            switch (GameManager.instance.workingSave.currentSkill)
            {
                case 0:
                    skillCooldownTime = 6f;
                    break;
                case 1:
                    skillCooldownTime = 6f;
                    break;
                case 2:
                    skillCooldownTime = 6f;
                    break;
            }
            workingGun = GameManager.instance.workingSave.guns[GameManager.instance.workingSave.currentGun];
            workingGun.SetParent(gameObject);

            if (healthBar)
            {
                healthBar.minValue = 0;
                healthBar.maxValue = health;
                healthBar.value = health;
            }

            if (skillCooldownBar)
            {
                skillCooldownBar.minValue = 0;
                skillCooldownBar.maxValue = skillCooldownTime;
            }
        }

    	void FixedUpdate()
        {
            // Analog controllers already have deadzones
            // This makes it so movement is 8-directional only
            var horMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized;
            var verMove = new Vector2(0, Input.GetAxisRaw("Vertical")).normalized;
            var moveDir = (horMove + verMove).normalized;
            transform.position += (Vector3)(moveDir * moveSpeed * Time.fixedDeltaTime);

            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Quaternion rot = Quaternion.LookRotation(transform.position - mousePosition, Vector3.forward);
            transform.rotation = rot;
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);

            var x = Mathf.Clamp(transform.position.x, -8f, 8f);
            var y = Mathf.Clamp(transform.position.y, -3.5f, 3.5f);
            transform.position = new Vector3(x, y, 0);
    	}

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                GameManager.instance.GoToMenu();

            skillTimer -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space))
                useSkill();

            if (Input.GetMouseButton(0))
                workingGun.Fire();
        }

        void LateUpdate()
        {
            skillCooldownBar.value = (skillCooldownTime - skillTimer);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("BossBullet"))
                return;
            
            SoundManager.instance.PlaySingle(playerHit);

            health--;

            if (healthBar)
                healthBar.value = health;

            Destroy(other.gameObject);

            if (health <= 0)
                GameManager.instance.GoToMenu();
        }

        public void useSkill()
        {
            if (skillTimer < 0) {
                switch (GameManager.instance.workingSave.currentSkill) {
                    case 0:
                        moveSpeed *= 2;
                        Invoke("ResetSpeed", 3);
                        break;
                    case 1:
                        {
                            float oldSpeed = workingGun.velocity;
                            workingGun.velocity = oldSpeed * 4;
                            workingGun.FireShot();
                            workingGun.FireShot();
                            workingGun.FireShot();
                            workingGun.FireShot();
                            workingGun.velocity = oldSpeed;
                            break;
                        }
                    case 2:
                        {
                            float oldSpeed = workingGun.velocity;
                            for (int i = 0; i < 20; i++)
                            {
                                workingGun.velocity = oldSpeed * 0.2f;
                                /*
                                GameObject rocket = (GameObject)GameObject.Instantiate(Resources.Load("BulletPrefab"), gameObject.transform.position, gameObject.transform.rotation);
                                rocket.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-4, 4), Random.Range(-4, 4));
                                */
                            }
                            workingGun.velocity = oldSpeed;
                            break;
                        }
                }

                skillTimer = Mathf.Max(skillCooldownTime, 0);
            }
        }

        void ResetSpeed()
        {
            moveSpeed = defaultMoveSpeed;
        }
    }
}