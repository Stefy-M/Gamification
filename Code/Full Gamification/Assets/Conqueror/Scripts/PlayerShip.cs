using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using Newtonsoft.Json;

namespace Conqueror {
    [System.Serializable]
    public class ConquerorSave 
    {
        [JsonProperty("guns")]
        public List<Gun> guns { get; set; }
        [JsonProperty("currentGun")]
        public int currentGun { get; set; }
        [JsonProperty("currentSkill")]
        public int currentSkill { get; set; }
        [JsonProperty("highestLevelReached")]
        public int highestLevelReached { get; set; } // Starts at 0. 0 = Level 1.

        public ConquerorSave()
        {
            guns = new List<Gun>();
            currentGun = 0;
            currentSkill = 0;
            highestLevelReached = 0;
        }
    }

    


    public class PlayerShip : MonoBehaviour
    {
        public AudioClip playerHit;
        public Slider healthBar;
        public Slider skillCooldownBar;
        public Text playerName;

        public int health;
        int maxHealth = 30;
        public float moveSpeed;
        float defaultMoveSpeed = 5;

        private Gun workingGun;
        private Vector3 defaultPos = new Vector3(0f, -2.8f, 0f);
        private SpriteRenderer sprite;
        private Color originalColor;

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

        public static string PlayerSkillDescriptionToString(int i)
        {
            switch (i)
            {
                case 0:
                    return "Doubles your speed for 3 seconds";
                case 1:
                    return "Fires four bullets at once";
                case 2:
                    return "Releases a barrage of bullets";
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
            playerName.text = player.Incre.username;
            sprite = GetComponent<SpriteRenderer>();
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
                        //Doubles movespeed for three seconds
                        originalColor = sprite.color;
                        sprite.color = new Color(0, 0, 255, 255);
                        moveSpeed *= 2;
                        Invoke("ResetSpeed", 3);
                        break;
                    case 1:
                        {
                            //Fires four bullets at once
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

                            //this makes bullets faster on average (don't know if this is really necessary since they are random anyways.
                            workingGun.velocity = oldSpeed * 0.2f;
                            for (int i = 0; i < 20; i++)
                            {
                                
                                GameObject rocket = GameObject.Instantiate(GameManager.instance.bulletPrefab, transform.position, Quaternion.identity);

                                Vector2 dir = new Vector2(Random.Range(-4, 4), Random.Range(-4, 4));
                                //to make sure the bullets don't get stuck, probably not the best solution but it works
                                while ((-1.0 < dir.x && dir.x <= 0) || (0 <= dir.x && dir.x < 1))
                                {
                                    dir.x = Random.Range(-4, 4);
                                }
                                while ((-1.0 < dir.y && dir.y <= 0) || (0 <= dir.y && dir.y < 1))
                                {
                                    dir.y = Random.Range(-4, 4);
                                }

                                rocket.GetComponent<Rigidbody2D>().velocity = workingGun.velocity * dir;

                                
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
            sprite.color = originalColor;
            moveSpeed = defaultMoveSpeed;
        }
    }
}