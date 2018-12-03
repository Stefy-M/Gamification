using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SokobanMenu : MonoBehaviour {

    public void PlayGame()
    {
        if(player.Incre.stamina.cur > 0)
        {
            player.Incre.stamina.cur--;
            SceneManager.LoadScene("MainScene");
        }
    }
}
