using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuMusic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        // Something that kills this when switching to a different game.
        if (player.Incre.currentGame != minigame.none)
            Destroy(gameObject);
    }
}
