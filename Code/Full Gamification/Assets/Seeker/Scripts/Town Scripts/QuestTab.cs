using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestTab : MonoBehaviour {

    public Text quest_box;

	// Use this for initialization
	void Start () {
        string temp = GlobalControl.Instance.current_quest.Replace('_', ' ');     
        quest_box.text = temp;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetQuest()
    {
        string temp = GlobalControl.Instance.current_quest.Replace('_', ' ');
        quest_box.text = temp;
    }
}
