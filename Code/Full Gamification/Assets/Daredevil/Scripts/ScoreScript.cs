using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

	public static int scoreVal = 0;
	public Text score;
	private float startTime;
	public ddPlayer p;
	private string secs;
	private bool runTimer = true;
	string minutes;
	float t;
	private string Scoretime;

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
		if (p.Stopped && runTimer)
		{
			
			t = Time.time - startTime;
			minutes = ((int)t / 60).ToString();
			secs = (t % 60).ToString("f2");

			score.text = secs;
		}

		if (p.StopTimer && p.DeadStatus == true) // stops timer when player runs out of lives
		{

			runTimer = false;
			Scoretime = score.text; // record the time for score
			//Debug.Log(Scoretime);
		}

		//Debug.Log("p.StopTimer: " + p.StopTimer + " p.DeadStatus: " + p.DeadStatus);
		
	}

	public string ScoreTime
	{
		get { return Scoretime; }
	}
}
