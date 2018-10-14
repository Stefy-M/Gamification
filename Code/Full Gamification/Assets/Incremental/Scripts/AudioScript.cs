using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour {

	public AudioClip audioClip;
	public AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource.clip = audioClip; 
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Space))
			 audioSource.Play ();
		if ((player.Incre.currentGame == minigame.seeker) || (player.Incre.currentGame == minigame.conquer) ||
			(player.Incre.currentGame == minigame.sokoban) || (player.Incre.currentGame == minigame.daredevil) ||
			(player.Incre.currentGame == minigame.mastermind)) 
		{
			audioSource.Stop ();
		}
	}
}
