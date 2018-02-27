using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class flagBehavior : MonoBehaviour {

    public FlagManager button;
    public Text textToChange;

    public void onFlag()
    {
        button = GameObject.Find("FlagButton").GetComponent<FlagManager>();
        textToChange = GameObject.Find("FlagButtonText").GetComponent<Text>();

        textToChange.text = "Remove Flag";
        button.flagToRemove = gameObject;
    }
    public void offFlag()
    {
        textToChange = GameObject.Find("FlagButtonText").GetComponent<Text>();
        textToChange.text = "Place Flag";
    }

}
