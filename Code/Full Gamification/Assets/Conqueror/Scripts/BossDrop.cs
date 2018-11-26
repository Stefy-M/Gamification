using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Conqueror {
    public class BossDrop : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            int level = GameManager.instance.level;
            if (!other.CompareTag("Player"))
                return;

            // Add bonus to incremental
            player.Incre.coin.active += (int) 2 * level;
            player.Incre.coin.passive += (int) 1 * level;

            // Add stronger gun based on current level and perk level
            Gun g;
            float damage = (int)Random.Range(Mathf.Max(level - 2f, 1f), level + 2f) + (5 * player.Incre.conquerorPerkLevel);
            float rof = Random.Range((30f/111) / (1 + 0.1f * level), 30f/111);
            float speed = Random.Range(5, 20 + level * 2);
            int type = Random.Range(0, Mathf.Min(level, 3));

            var ship = other.GetComponent<PlayerShip>();
            g = new Gun(damage, rof, speed, type);
            g.SetParent(other.gameObject);

            // Debug gun
            Debug.Log("Generating gun: " + g.name);
            GameManager.instance.AddGun(g);

            int i = GameManager.instance.workingSave.guns.Count - 1;
            GameManager.instance.workingSave.currentGun = i;
            ship.WorkingGun = GameManager.instance.workingSave.guns[i];
            
            Destroy(gameObject);
        }
    }
}