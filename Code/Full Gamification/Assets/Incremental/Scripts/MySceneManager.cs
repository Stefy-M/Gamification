using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum minigame
{
    none,
    seeker,
    conquer,
    mastermind,
    daredevil,
    sokoban
}

public static class MySceneManager{

    public static string scene_seeker = "";
    public static string scene_mastermind = "";
    public static string scene_conquer = "";
    public static string scene_daredevil = "";
    public static string scene_sokoban = "";
    public static minigame activeGame;

    public static void sceneChange(minigame type, string newScene)
    {

        //unload
        if (activeGame == minigame.seeker)
        {
            SceneManager.UnloadSceneAsync(scene_seeker);
            scene_seeker = "";
        }
        else if (activeGame == minigame.mastermind)
        {
            SceneManager.UnloadSceneAsync(scene_mastermind);
            scene_mastermind = "";
        }
        else if (activeGame == minigame.conquer)
        {
            SceneManager.UnloadSceneAsync(scene_conquer);
            scene_conquer = "";
        }
        else if (activeGame == minigame.daredevil)
        {
            SceneManager.UnloadSceneAsync(scene_daredevil);
            scene_daredevil = "";
        }
        else if (activeGame == minigame.sokoban)
        {
            SceneManager.UnloadSceneAsync(scene_sokoban);
            scene_daredevil = "";
        }


        //load
        if (type == minigame.seeker)
        {
            SceneManager.LoadScene(newScene, LoadSceneMode.Additive);
            scene_seeker = newScene;
        }
        else if (type == minigame.mastermind)
        {
            SceneManager.LoadScene(newScene, LoadSceneMode.Additive);
            scene_mastermind = newScene;
        }
        else if (type == minigame.conquer)
        {
            SceneManager.LoadScene(newScene, LoadSceneMode.Additive);
            scene_conquer = newScene;
        }
        else if (type == minigame.daredevil)
        {
            SceneManager.LoadScene(newScene, LoadSceneMode.Additive);
            scene_daredevil = newScene;
        }
        else if (type == minigame.sokoban)
        {
            SceneManager.LoadScene(newScene, LoadSceneMode.Additive);
            scene_sokoban = newScene;
        }
        else
            Debug.Log("Please insert valid type");
        activeGame = type;
    }
}
