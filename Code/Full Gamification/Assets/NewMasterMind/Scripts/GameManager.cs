using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

//here we can add in save data for sokoban later
[System.Serializable]
public class SokobanSave
{
    [JsonProperty("test")]
    public string test { get; set; }

    public SokobanSave()
    {
        test = "test sokoban save";
    }
}

public class GameManager : MonoBehaviour {
    public LevelBuilder m_LevelBuilder;
    public GameObject m_NextButton;
	private bool m_ReadyForInput;
	private Character m_Character;
	
    void Start()
    {
        m_NextButton.SetActive(false);
        ResetScene();
    }

	// Update is called once per frame
	void Update () {
		Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		moveInput.Normalize();
		if (moveInput.sqrMagnitude > 0.5)
        {
			if (m_ReadyForInput)
            {
				m_ReadyForInput = false;
				m_Character.Move(moveInput);
				m_NextButton.SetActive(IsLevelComplete());
			}
		}
		else
        {
			m_ReadyForInput = true;
		}
        if (Input.GetKeyUp(KeyCode.R))
        {
            ResetScene();
        }
    }

    public void NextLevel()
    {
        m_NextButton.SetActive(false);
        m_LevelBuilder.NextLevel();
        LevelTextScript.LevelValue += 1;
        StartCoroutine(ResetSceneASync());
    }

    public void ResetScene()
    {
        StartCoroutine(ResetSceneASync());
    }

    bool IsLevelComplete()
    {
        Box[] boxes = FindObjectsOfType<Box>();
        foreach (var box in boxes)
        {
            if (!box.m_OnCross)
                return false;
        }
        return true;
    }

    IEnumerator ResetSceneASync()
    {
        if (SceneManager.sceneCount > 1)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync("LevelScene");
            while (!asyncUnload.isDone)
            {
                yield return null;
                Debug.Log("Unloading...");
            }
            Debug.Log("Unload Done");
            Resources.UnloadUnusedAssets();
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelScene", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
            Debug.Log("Loading...");
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LevelScene"));
        m_LevelBuilder.Build();
        m_Character = FindObjectOfType<Character>();
        Debug.Log("Level loaded");
    }
}
