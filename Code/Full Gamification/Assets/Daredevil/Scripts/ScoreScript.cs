using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {

	public Text score;
	private float startTime;
	public ddPlayer player;
	private string secs;
	private bool runTimer = true; // run timer initialized to true;
	string minutes;
	float t;
	private double Scoretime = 0.0;

	// Use this for initialization


	void Start () {
		score = GetComponent<Text>();
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (!player.Stopped) 
		{
			startTime = Time.time; 

		}
		if (player.Stopped && runTimer) 
		{
			
			t = Time.time - startTime;
			secs = t.ToString("f2");
			score.text = secs;
			Scoretime = Convert.ToDouble(score.text);
			
		}

		if (player.StopTimer && player.DeadStatus == true) // stops timer when player runs out of lives
		{

			runTimer = false;  // Run timer gets stopped
			Scoretime = Convert.ToDouble(score.text); // record the time for score
			
		}

		
	}



	public double ScoreTime
	{
		get { return Scoretime; }
		
	}

	
}
