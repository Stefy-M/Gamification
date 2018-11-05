using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimerScript : MonoBehaviour
{
	public static bool levelComplete = false;
	public static bool resetTimer = false;
	public Text LevelTimerText;
	private float startTime;
	private float t = 0.0f;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (resetTimer) {
			startTime = Time.time;
			resetTimer = false;
			levelComplete = false;
		}

		if (!levelComplete) { // if level is not complete, update time
			t = Time.time - startTime;
		} else {
			// we should save the time of the current level here
		}

		string minutes = ((int)t / 60).ToString();
		string seconds = (t % 60).ToString("f0");

		LevelTimerText.text = "Time: " + minutes + "m " + seconds + "s";
	}
}
