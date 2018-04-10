using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Change_Scene : MonoBehaviour {

    public float delay;
    public string next_level;
    public bool can_change = true;
    public string[] lines;
    public GameObject dialogue;
    public GameObject fades;
    public int fade_dir;
    public bool quest_progress;

    void Start()
    {
        if(GameObject.Find("Fade") != null)
        {
            fades = GameObject.Find("Fade");
        }
    }

    public void CheckChange()
    {
        if (can_change)
        {
            if (fades != null)
            {
                fades.SetActive(true);
                fades.GetComponent<Fading>().enabled = true;
                fades.GetComponent<Fading>().BeginFade(fade_dir);
            }

            Invoke("ChangeScene", delay);
        }

        else
        {
            dialogue.GetComponent<DialogueInstances>().SetInstance(lines);
        }
    }

    public void ChangeScene()
    {
        if (quest_progress)
        {
            GlobalControl.Instance.QuestUpdate();
        }
        SceneManager.LoadScene(next_level);
        if (next_level == "Dungeons" || next_level == "First_Dungeon" || next_level == "Second_Dungeon" || next_level == "Third_Dungeon")
        {
            GlobalControl.Instance.inTown = false;
            GlobalControl.Instance.inDungeon = true;
            GlobalControl.Instance.playMusic(1);
        }
        else if((next_level == "Town_Market2" || next_level == "Town_Market3"))
        {
            GlobalControl.Instance.stopMusic();
            GlobalControl.Instance.inDungeon = false;
        }
        else if ((next_level == "Town_Market4" || next_level == "Town_Market") && !GlobalControl.Instance.inTown)
        {
            GlobalControl.Instance.inDungeon = false;
            GlobalControl.Instance.inTown = true;
            GlobalControl.Instance.playMusic(0);
        }
    }

    public void ChangeCanChange()
    {
        if (can_change == false)
        {
            can_change = true;
        }
        else
        {
            can_change = false;
        }
    }

    public void LoadStart()
    {
        if (fades != null)
        {
            fades.SetActive(true);
            fades.GetComponent<Fading>().enabled = true;
            fades.GetComponent<Fading>().BeginFade(fade_dir);
        }

        switch (GlobalControl.Instance.quest_progress)
        {
            case 0:
                next_level = "Intro1";
                break;
            case 1:
                next_level = "Town_Market1";
                //GlobalControl.Instance.inTown = true;
                //GlobalControl.Instance.playMusic(0);
                break;
            case 2:
                next_level = "Forest2";
                break;
            case 3:
                next_level = "Town_Market2";
                //GlobalControl.Instance.inTown = true;
                //GlobalControl.Instance.playMusic(0);
                break;
            case 4:
                next_level = "Town_Home3";
                //GlobalControl.Instance.inTown = true;
                //GlobalControl.Instance.playMusic(0);
                break;
            default:
                next_level = "Town_Market";
                GlobalControl.Instance.inTown = true;
                GlobalControl.Instance.playMusic(0);
                break;
        }

        Invoke("ChangeScene", delay);
    }
}
