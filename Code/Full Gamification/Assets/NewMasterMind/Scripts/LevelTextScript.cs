using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTextScript : MonoBehaviour
{
    public static int LevelValue = 1;
    Text Level;

	// Use this for initialization
	void Start ()
    {
        Level = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Level.text = "Level: " + LevelValue;
	}
}
