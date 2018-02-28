using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.EventSystems;

public class loginScript : MonoBehaviour {
    public Text id;
    public GameObject pass;
    public Text status;
    public NetworkManager network;
	public InputField loginField;

	EventSystem system;
	bool tryLogin = false;

    void Start()
    {
        status.enabled = false;
        system = EventSystem.current;
		loginField.OnPointerClick(new PointerEventData(system));
		system.SetSelectedGameObject(loginField.gameObject, new BaseEventData(system));
    }

	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
		{
			Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

			if (next == null)
				next = loginField.gameObject.GetComponent<Selectable>();

            if (next != null)
            {
                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null)
                    inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
            //else Debug.Log("next nagivation element not found");
        }

        if (Input.GetKeyDown(KeyCode.Return))
            login();

        if (tryLogin)
        {
            status.enabled = true;
            status.text = network.loginFlag.ToString();

			switch (network.loginFlag) {
			// Unsuccessful login flags
			case -1:
				status.text = "Username not found.";
				break;
			case -2:
				status.text = "Password doesn't match.";
				break;
			case -3:
				status.text = "Update game client.";
				break;
			// Successful login flags
			case 1:
				status.text = "Login successful.";
				break;
			case 2:
				status.text = "Loading player's data...";
				break;
			case 3:
				player.load();
				status.text = "Loading Done.";
				player.isLocal = false;
				gameStart();
				break;
			// No server flag
			case 0:
				status.text = "Server is not connected.";
				break;
			}
        }
    }

    public void login()
    {
        network.StartConnection(id.text, pass.GetComponent<InputField>().text);
        tryLogin = true;
        Debug.Log("Start connection...");
    }
    
    public void localLogin()
    {
        player.localLoad();
        player.isLocal = true;
        gameStart();
    }

    public void gameStart()
    {
        if (player.Incre.startNew)
        {
            player.Incre.startNew = false;
        }
        else //continue user
        {
            Debug.Log("last logout: " + player.Incre.lastLogOut.ToString());
            Debug.Log("Now: " + DateTime.Now.ToString());
            updatePassiveProgress(player.Incre.lastLogOut);
            player.Incre.currentGame = minigame.none;
            player.Incre.gameON = false;
            player.Incre.passive = true;
            player.Incre.startMessageDisplayed = false;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("ui", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public static int updatePassiveProgress(DateTime lastLogOut)
    {
        TimeSpan span = DateTime.Now.Subtract(lastLogOut);
        Debug.Log(span.TotalSeconds);
        player.Incre.LogOutpassiveProgressGained = Convert.ToInt32(span.TotalSeconds);
        player.Incre.progress.cur += bal.getPassiveProgressBarRate() * span.TotalSeconds * 60;
        player.Incre.debugging = true;
        return 0;
    }

    //this function is ONLY for debugging purpose
    public static int updateActiveProgress(DateTime time)
    {
        TimeSpan span = DateTime.Now.Subtract(time);
        Debug.Log(span.TotalSeconds);
        player.Incre.progress.cur += bal.getActiveProgressBarRate() * span.TotalSeconds * 60;
        player.Incre.debugging = true;
        return 0;
    }
}

public class JsonStrings
{
    public string Seeker = "";
    public string Mastermind = "";
    public string Incremental = "";
    public string Conqueror = "";
}