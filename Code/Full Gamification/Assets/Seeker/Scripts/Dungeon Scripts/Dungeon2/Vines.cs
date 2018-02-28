using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vines : MonoBehaviour {

    public GameObject player;
    public GameObject notification;
    public GameObject choice;
    private bool found;

    public int difficulty;
    public int damage;
    public bool has_fruit;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            choice.SetActive(true);
            if (!gameObject.GetComponent<SpriteRenderer>().enabled)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                notification.GetComponent<TextInfo>().AddText("The vines strike at you!");
                Dodge();
                choice.GetComponent<SecondDungeonPanel>().SetObject(gameObject);
                choice.GetComponent<SecondDungeonPanel>().SetText("Attempt to dig up vines?");
                found = true;
            }
            else
            {
                choice.GetComponent<SecondDungeonPanel>().SetObject(gameObject);
                choice.GetComponent<SecondDungeonPanel>().SetText("Attempt to dig up vines?");
            }
        }
    }

    private void Dodge()
    {
        //simple balance model - dodge scales based on dex
        float x = 0.0f;
        float finalRoll = 0.0f;
        float randDex = Random.Range(0.0f, player.GetComponent<PlayerStats>().GetTotalDexterity());
        float randInsight = Random.Range(0.0f, player.GetComponent<PlayerStats>().GetTotalInsight());
        float difficultyCheck = Random.Range(0.0f, difficulty);



        // Use player stamina. If player doesn't have 3 or more stamina, use all and apply it to dodge chance
        if (player.GetComponent<PlayerStats>().current_stamina <= 0)
        {
            notification.GetComponent<TextInfo>().AddText("You are tired, and can't dodge as well.");
            x = Random.Range(0.0f, player.GetComponent<PlayerStats>().GetTotalDexterity()) * 0.8f;
            player.GetComponent<PlayerStats>().current_stamina = 0;
        }
        else
        {
            notification.GetComponent<TextInfo>().AddText("You have enough energy to dodge. " + (difficulty / 2) + " energy spent.");
            player.GetComponent<PlayerStats>().Tired(difficulty / 2);
            x = Random.Range(0.0f, player.GetComponent<PlayerStats>().GetTotalDexterity());
        }

        // Rolls to see if player dodges trap

        // If fails roll

        finalRoll = (x + randDex) / 2;
        if (finalRoll <= difficultyCheck) //changed this line to not multiply insight by 1
        {
            // If the player's dexterity roll was fast enough, take reduced damage
            if (Mathf.CeilToInt(finalRoll) >= Mathf.CeilToInt(difficultyCheck * 0.8f))
            {
                player.GetComponent<PlayerStats>().Hurt(Mathf.FloorToInt(damage * 0.5f));
                notification.GetComponent<TextInfo>().AddText("You weren't fast enough to dodge the vines unscathed. Reduced damage taken.");

            }
            // Take full damage
            else
            {
                player.GetComponent<PlayerStats>().Hurt(Mathf.CeilToInt(damage));
                notification.GetComponent<TextInfo>().AddText("The vines lash and lands a direct hit. Full damage taken.");
            }
        }

        // Roll succeeds, take no damage
        else
        {
            notification.GetComponent<TextInfo>().AddText("You were able to dodge the vines with no damage taken!");
        }
    }

    private void DigUp()
    {
        //simple balance model - digging scales on insight
        float x = 0.0f;
        float finalRoll = 0.0f;
        float randDex = Random.Range(0.0f, player.GetComponent<PlayerStats>().GetTotalDexterity());
        float randInsight = Random.Range(0.0f, player.GetComponent<PlayerStats>().GetTotalInsight());
        float difficultyCheck = Random.Range(0.0f, difficulty);

        // If no stamina, reduce chance of success
        if (player.GetComponent<PlayerStats>().current_stamina <= 0)
        {
            notification.GetComponent<TextInfo>().AddText("You are tired, and can't dig as well.");
            x = Random.Range(0.0f, player.GetComponent<PlayerStats>().GetTotalInsight() * 0.8f);
        }
        else
        {
            notification.GetComponent<TextInfo>().AddText("You have enough energy to dig. " + (difficulty / 2) + " energy spent.");
            x = Random.Range(0.0f, player.GetComponent<PlayerStats>().GetTotalInsight());

            player.GetComponent<PlayerStats>().Tired(difficulty / 2);
        }
        finalRoll = (x + randInsight) / 2;
        if (finalRoll <= difficultyCheck)
        {
            notification.GetComponent<TextInfo>().AddText("The vines thrash at you.");
            Dodge();
            notification.GetComponent<TextInfo>().AddText("You failed to dig up the vines.");
        }
        else
        {
            gameObject.SetActive(false);
            notification.GetComponent<TextInfo>().AddText("You removed the vines!");
            if (has_fruit)
            {
                notification.GetComponent<TextInfo>().AddText("You found a vine fruit!");
            }
        }
    }

    public void AnswerYes()
    {
        DigUp();
        choice.SetActive(false);
    }

    public void AnswerNo()
    {
        if (!found)
        {
            return;
        }
        Dodge();
        choice.SetActive(false);
    }
}
