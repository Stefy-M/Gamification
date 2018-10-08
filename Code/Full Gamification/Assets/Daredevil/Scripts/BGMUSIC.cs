using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMUSIC : MonoBehaviour {

	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

    void Update()
    {
        // Something that kills this when switching to a different game.
        if (player.Incre.currentGame != minigame.daredevil)
            Destroy(gameObject);
    }

}
