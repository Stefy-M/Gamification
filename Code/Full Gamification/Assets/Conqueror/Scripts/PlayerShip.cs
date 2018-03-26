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
        private bool invincible = false;
        private float flashTimer = 0.0f;
        private bool inBoss = false;

        private Gun workingGun;
        private Vector3 defaultPos = new Vector3(0f, -2.8f, 0f);
        private SpriteRenderer sprite;
        private Color originalColor;
        private CountdownScript countdown;

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
            originalColor = sprite.color;
            countdown = GameObject.Find("Countdown").GetComponent<CountdownScript>();
            Init();
        }

        public void Init()
        {
            playerName.text = player.Incre.username;
            sprite = GetComponent<SpriteRenderer>();
            
            transform.position = defaultPos;
            health = maxHealth;
            moveSpeed = defaultMoveSpeed;
            inBoss = false;
            flashTimer = 0.0f;
            invincible = false;

            //can be used to balance skills in the future
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

            //only prevent shooting on inital countdown, shooting at level transitions is OK
            if (countdown.initialCountDownDone)
            {
                if (Input.GetMouseButton(0))
                    workingGun.Fire();
            }

            if(invincible == true)
            {
                invincibleFlashing(.05f);
            }
            //if player stays inside boss, damage them if not invincible
            if (inBoss && !invincible)
                inBossDamage();

        }

        void LateUpdate()
        {
            skillCooldownBar.value = (skillCooldownTime - skillTimer);
        }

        
        void OnTriggerEnter2D(Collider2D other)
        {

            if (!invincible)
            {
                if (other.CompareTag("BossBullet"))
                {

                    SoundManager.instance.PlaySingle(playerHit);

                    health--;

                    if (healthBar)
                        healthBar.value = health;

                    Destroy(other.gameObject);
                }
                else if (other.CompareTag("Boss"))
                {
                    SoundManager.instance.PlaySingle(playerHit);
                    //Sets flag that player is in boss hitbox
                    inBoss = true;
                    //Reduces HP by third of max hp if hit by boss
                    health -= (maxHealth / 3);
                    if (healthBar)
                        healthBar.value = health;
                    //Adds iFrames
                    invincible = true;
                    //Become vulnurable again in 2 seconds
                    Invoke("resetInvulnerability", 2f);
                }
            }
            //If invincible only set flag if colliding with boss
            else
            {
                if (other.CompareTag("Boss"))
                {
                    //Sets flag that player is in boss hitbox
                    inBoss = true;
                }
            }
            if (health <= 0)
            {
                resetInvulnerability();
                ResetSpeed();
                CancelInvoke();
                GameManager.instance.GoToMenu();
            }
        }

        //If player stays inside boss
        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Boss"))
            {
                inBoss = false;   
            }       
        }

        //so sprite flashes when invincible (called from update function)
        void invincibleFlashing(float duration)
        {
            Color temp = sprite.color;
            flashTimer += Time.deltaTime;

            if (flashTimer >= duration)
            {
                if (temp.a == 255f)
                {
                    temp.a = 0f;
                    sprite.color = temp;
                }
                else
                {
                    temp.a = 255f;
                    sprite.color = temp;
                }
                flashTimer = 0f;
            }
        }

        public void resetInvulnerability()
        {
            //Resetting back to regular color (full opacity)
            Color temp;
            temp = sprite.color;
            temp.a = 255f;
            sprite.color = temp;
            //not invincible anymore
            invincible = false;
        }

        void inBossDamage()
        {
            SoundManager.instance.PlaySingle(playerHit);
            //Reduces HP by third of max hp if hit by boss
            health -= (maxHealth / 3);
            if (healthBar)
                healthBar.value = health;
            //Adds iFrames
            invincible = true;
            //Become vulnurable again in 2 seconds
            Invoke("resetInvulnerability", 2f);

            if (health <= 0)
            {
                resetInvulnerability();
                ResetSpeed();
                CancelInvoke();
                GameManager.instance.GoToMenu();
            }
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

        public void ResetSpeed()
        {
            sprite.color = originalColor;
            moveSpeed = defaultMoveSpeed;
        }
    }
}