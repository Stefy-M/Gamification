using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour {

    private string no_new;
    public DialogueManager dialogue_set;

	// Use this for initialization
	void Start () {
        no_new = "Leader: \"We do not currently require your assistance.\"";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Quests()
    {
        switch(GlobalControl.Instance.quest_progress)
        {
            case 5:
                dialogue_set.Reset(GlobalControl.Instance.quest_dialogue[0]);
                dialogue_set.enabled = true;
                dialogue_set.EnableTextBox();
                break;
            case 6:
                dialogue_set.Reset(GlobalControl.Instance.quest_dialogue[1]);
                dialogue_set.enabled = true;
                dialogue_set.EnableTextBox();
                QuestChange("Go_to_the_dungeon_outside_of_the_village_with_the_animals_key_and_bring_back_the_work_animals._Recommended_Stats:_20_Upgrades.");
                Reward(500);
                GlobalControl.Instance.QuestUpdate();
                break;
            case 7:
                dialogue_set.Reset(GlobalControl.Instance.quest_dialogue[2]);
                dialogue_set.enabled = true;
                dialogue_set.EnableTextBox();
                break;
            case 8:
                dialogue_set.Reset(GlobalControl.Instance.quest_dialogue[3]);
                dialogue_set.enabled = true;
                dialogue_set.EnableTextBox();
                QuestChange("Please_wait_for_the_next_update_of_New_Reality!");
                Reward(2000);
                GlobalControl.Instance.QuestUpdate();
                break;
            default:
                break;
        }

    }

    private void Reward(int rep_amount)
    {
        GlobalControl.Instance.reputation += rep_amount;
    }

    public void QuestChange(string quest)
    {
        GlobalControl.Instance.QuestSet(quest);
    }
}
