using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSingleton : MonoBehaviour
{

	private static AudioSingleton instance = null;

	public static AudioSingleton Instance {
		get {
			return instance;
		}
	}

	void Awake ()
	{
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad (this.gameObject);
	}

    private void Update()
    {
        // Something that kills this when switching to a different game.
        if (player.Incre.currentGame != minigame.sokoban)
            Destroy(gameObject);
    }
}
