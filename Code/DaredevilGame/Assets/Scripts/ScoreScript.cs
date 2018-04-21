using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

	public static int scoreVal = 0;
	public Text score;
	private float startTime;
	public player p;
	public string secs;
	string minutes;
	float t;

	// Use this for initialization
	void Start () {
		score = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

		if (!p.Stopped)
		{
			startTime = Time.time;

		}
		if (p.Stopped)
		{
			
			t = Time.time - startTime;
			minutes = ((int)t / 60).ToString();
			secs = (t % 60).ToString("f2");

			score.text = secs;
		}

		else if (p.StopTimer && !p.Stopped)
		{
			

			score.text = secs;
		}
	}
}
