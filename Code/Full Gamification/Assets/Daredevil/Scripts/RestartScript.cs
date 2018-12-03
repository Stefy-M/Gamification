using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour {

	public ddPlayer Player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void restartScene()
	{
        if (player.Incre.stamina.cur > 0)
        {
            player.Incre.stamina.cur--;
            SceneManager.LoadScene("main");
        }
            
	}
}
