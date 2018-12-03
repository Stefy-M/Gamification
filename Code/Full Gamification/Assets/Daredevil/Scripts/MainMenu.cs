using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame()
	{
        if (player.Incre.stamina.cur > 0)
        {
            player.Incre.stamina.cur--;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
	}

	public void QuitGame()
	{
		Debug.Log("Quit");
		Application.Quit();

	}
}
