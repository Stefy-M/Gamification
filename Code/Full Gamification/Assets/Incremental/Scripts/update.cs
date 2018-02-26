using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;
using UnityEngine.SceneManagement;
using System;

public delegate void strDele(string a, string b);

public enum dialogMode
{
	exitgame,
	changeGame,
	exitapplication,
	resetGame
}

public class update : MonoBehaviour {
    public GameObject upgrade;
    public GameObject bonus;
    public GameObject dialog;
    public GameObject dialog_yes;
    public GameObject dialog_no;
    public GameObject dialog_okay;
    public GameObject mainScreen;
    public Slider bar_progress;
    public Slider bar_exp;

    public Text txt_progress;
    public Text txt_exp;
    public Text txt_welcomeMsg;
    public Text txt_timeLeft;
    public Text txt_stamina;

    public Text txt_redeem_pc;
    public Text txt_redeem_ac;
    public Text txt_redeem_exp;

    public Text txt_mode;
    public Text txt_level;
    public Text txt_passiveCoin;
    public Text txt_activeCoin;
    public Text txt_message;
    public Text txt_messageTitle;
    public Text bonusCodeInput;
    public Text txt_exit;

    public Text playerInfoDisplay;

    public Button passiveActiveButton;
    public Image progressBarImage;
    public Text msglog;

	public Tutorial tut;

    string nextScene;
    bool upgradeWindow;
    bool bonusWindow;
    dialogMode _dialogMode;
    string localSaveData;

    int saveCount = 0;

    void Start()
    {
        //test purpose
//        player.Incre.stamina.cur = 10;
//        player.Incre.needTutorial = false;
//        player.Incre.timeleft.cur = 60 * 60;

		sendMsg (randomWelcomeMessage ());

        if (!player.Incre.startMessageDisplayed)
        {
            showMessage("While you were not playing the game, "
				+ " (" + TimeSpan.FromSeconds(player.Incre.LogOutpassiveProgressGained) + ")"
				+ (bal.getPassiveProgressBarRate() * player.Incre.LogOutpassiveProgressGained * 60).ToString("##.##") + "%"
				+ " Passive Progress were collected.", "Passive Progress Gain");
            player.Incre.startMessageDisplayed = true;
        }

        updatePlayerInfo();

		//set upgrade window
        upgradeWindow = false;
        bonusWindow = false;
        upgrade.SetActive(upgradeWindow);
        bonus.SetActive(bonusWindow);

		player.Incre.passive = !player.Incre.gameON;

//		if (player.Incre.timeleft.cur <= 0)
//            ("Active time finished.", "Active Time");
//        Vector3 newpos = new Vector3(0, 0, 0);
//        upgrade.transform.position = newpos;
    }

	// Update is called once per frame
	void Update()
    {
        updateModeButton();
        updateStamina();
        updateProgressBar();
        updateLv();
        updateCoin();
        updateRedeemText();
        updateExpBar();
        mainScreen.SetActive(!player.Incre.gameON);

        if (bar_progress.value < 0.021f)
            bar_progress.value = 0.021f;

        if (bar_exp.value < 0.021f)
            bar_exp.value = 0.021f;
		
		txt_exit.text = player.Incre.gameON ? "Close Game" : "Exit Game";

        if (saveCount >= 60 * 3)
        {
            Debug.Log("Saving...");
            save();
            saveCount = 0;
        }
        else
			saveCount++;
    }

    public void sendMsg(string msg)
    {
        msglog.text = ">> " + msg;
    }

    void showMessage(string msg, string title)
    {
        dialog.SetActive(true);
        dialog_yes.SetActive(false);
        dialog_no.SetActive(false);
        dialog_okay.SetActive(true);
        txt_message.text = msg;
        txt_messageTitle.text = title;
    }

    void showMessage2(string msg, string title, dialogMode mode)
    {
        dialog.SetActive(true);
        dialog_yes.SetActive(true);
        dialog_no.SetActive(true);
        dialog_okay.SetActive(false);
        txt_message.text = msg;
        _dialogMode = mode;
        txt_messageTitle.text = title;
    }

    public void exitPressed()
    {
        if (player.Incre.gameON)
        {
            nextScene = "ui";
            player.Incre.nextGame = minigame.none;
            showMessage2("Do you want to close the minigame?", "You sure?", dialogMode.exitgame);
        }
        else
            showMessage2("Do you want to close the whole game?",
				"Are you really sure?", dialogMode.exitapplication);
    }

    public void resetPressed()
    {
        showMessage2("Do you want to reset the entire game? All progress will be lost.",
			"Reset Game", dialogMode.resetGame);
    }

	// Unused
    public void buttonYesOrNoClicked(bool yes)
    {
        if (yes)
			switch (_dialogMode) {
			case dialogMode.changeGame:
				player.Incre.currentGame = player.Incre.nextGame;
				SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
				break;
			case dialogMode.exitgame:
				player.Incre.currentGame = minigame.none;
				player.Incre.gameON = false;
				player.Incre.passive = true;
				SceneManager.LoadScene("ui", LoadSceneMode.Single);
				break;
			case dialogMode.exitapplication:
				Application.Quit();
				break;
			case dialogMode.resetGame:
				GlobalControl.Instance.ResetGame();
				showMessage("The game has been reset.", "Game Reset");
				break;
			}

        dialog.SetActive(false);
    }

    void updatePlayerInfo()
    {
        if (playerInfoDisplay != null)
            playerInfoDisplay.text = string.Format("-player info-\r\nid: {0}\r\nreset: {1}",
				player.Incre.username, player.Incre.permanentPerksLV);
    }

    void updateRedeemText()
    {
		if (player.Incre.passive) //passive mode
        {
            txt_redeem_pc.text = string.Format("pc: +{0}",
				bal.getPassiveCoinBonus()* player.Incre.coin.boosterRate);
            txt_redeem_ac.text = string.Format("ac: +{0}", 0);
            txt_redeem_exp.text = string.Format("exp: +{0}",
				bal.getPassiveEXPRate() * player.Incre.exp.boosterRate);
        }
        else
        {
            txt_redeem_pc.text = string.Format("pc: +{0}",
				bal.getPassiveCoinBonus() * player.Incre.coin.boosterRate);
            txt_redeem_ac.text = string.Format("ac: +{0}",
				bal.getActiveCoinBonus() * player.Incre.coin.boosterRate);
            txt_redeem_exp.text = string.Format("exp: +{0}",
				bal.getActiveEXPRate() * player.Incre.exp.boosterRate);
        }
    }

    void updateModeButton()
    {
        ColorBlock cb = passiveActiveButton.colors;
        Color c;

        if (player.Incre.passive)
        {
            c = new Color32(16, 203, 255, 255);
            cb.normalColor = c;
            cb.highlightedColor = c;
            passiveActiveButton.colors = cb;
        }
        else
        {
            c = new Color32(255, 216, 0, 255);
            cb.normalColor = c;
            cb.highlightedColor = c;
            passiveActiveButton.colors = cb;
        }
    }

    //returns true if it is full (100%)
    void updateProgressBar()
    {
        //check over Progress
        double gainedExp = 0;

        if (player.Incre.debugging)
        {
            while (player.Incre.progress.cur > bal.getMaxProgress())
            {
                if (player.Incre.passive)
                {
                    earnPassiveCoin(bal.getPassiveCoinBonus());
                    earnExp(bal.getPassiveEXPRate());
                    player.Incre.progress.cur -= bal.getMaxProgress();
                    gainedExp += bal.getPassiveEXPRate(); //debug
                }
                else  //debugging ONLY
                {
                    earnActiveCoin(bal.getActiveCoinBonus());
                    earnPassiveCoin(bal.getPassiveCoinBonus());
                    earnExp(bal.getActiveEXPRate());
                    player.Incre.progress.cur -= bal.getMaxProgress();
                    gainedExp += bal.getActiveEXPRate(); //debug
                }
            }

            player.Incre.debugging = false;
        }

        //change color depends on mode
        if (player.Incre.passive)
			progressBarImage.color = new Color32(16, 203, 255, 255);
        else
			progressBarImage.color = new Color32(255, 216, 0, 255);

        //gets 100% ---> redeem coins and exp
        if (player.Incre.progress.cur >= bal.getMaxProgress())
        {
            if (!player.Incre.passive) //active gets both coins
            {
                if (isPlayerActive())
                {
                    //redeem both
                    earnPassiveCoin(bal.getPassiveCoinBonus()*(int)player.Incre.coin.boosterRate);
                    earnActiveCoin(bal.getActiveCoinBonus() * (int)player.Incre.coin.boosterRate);
                    earnExp(bal.getActiveEXPRate()*player.Incre.exp.boosterRate);
                    player.Incre.progress.cur = 0;
                }
            }
			else //Passive MODE
            {
                earnPassiveCoin(bal.getPassiveCoinBonus()* (int)player.Incre.coin.boosterRate);
                earnExp(bal.getPassiveEXPRate() * player.Incre.exp.boosterRate);
                //set to zero
                player.Incre.progress.cur = 0;
            }
        }
		else //not 100%
        {
			double boosterRate = player.Incre.progress.boosterRate;

            if (player.Incre.passive == true)
            {
                txt_mode.text = "Passive MODE";
                player.Incre.progress.cur += bal.getPassiveProgressBarRate() * boosterRate;
            }
            else
            {
                txt_mode.text = "Active MODE";
                player.Incre.progress.cur += bal.getActiveProgressBarRate() * boosterRate;
            }

            bar_progress.value = (float)player.Incre.progress.cur / bal.getMaxProgress();

            double progressRate;

            if (player.Incre.passive)
                progressRate = bal.getPassiveProgressBarRate() * boosterRate;
            else
                progressRate = bal.getActiveProgressBarRate() * boosterRate;

            txt_progress.text = string.Format("Prog {0}/100(+{1})",
				((player.Incre.progress.cur / bal.getMaxProgress() * 100)).ToString("N2"),
				(progressRate / bal.getMaxProgress() * 10000).ToString("N3"));
        }
    }

	string changeToTime(float sec)
    {
		DateTime t = new DateTime() + TimeSpan.FromSeconds(sec);
		return t.ToString("HH:mm:ss");
    }

    void updateStamina()
    {
        //stamina decreases when player enters some area in minigames
        txt_stamina.text = String.Format("STAMINA: {0}/{1}",
			player.Incre.stamina.cur, player.Incre.stamina.max);
    }

    void updateCoin()
    {
        txt_passiveCoin.text = player.Incre.coin.passive.ToString();
        txt_activeCoin.text = player.Incre.coin.active.ToString();
    }

    void updateLv()
    {
        txt_level.text = player.Incre.lv.ToString();
    }

    void updateExpBar()
    {
        bar_exp.value = (float)(player.Incre.exp.cur / bal.getMaxEXP());
        txt_exp.text = string.Format("EXP {0}/{1}", (int)player.Incre.exp.cur, (int)bal.getMaxEXP());

        if (player.Incre.exp.cur >= bal.getMaxEXP())
        {
            double remain = player.Incre.exp.cur - bal.getMaxEXP();
            levelUP();
            updateLv();
            player.Incre.exp.cur = (float)remain;
        }
    }

    public void earnPassiveCoin(int amount)
    {
        sendMsg("You earned " + amount + " PC!");
        player.Incre.coin.passive += amount;
    }

    public void earnActiveCoin(int amount)
    {
        sendMsg("You earned " + amount + " AC!");
        player.Incre.coin.active += amount;
    }

    public void earnExp(double amount)
    {
        sendMsg("You earned " + amount + " EXP!");
        player.Incre.exp.cur += (float)amount;
    }

    public void levelUP()
    {
        //visual effect will call in this function
        sendMsg("Level up!!!");
        player.Incre.lv++;

        //calculate new incremental values here
        //calculateIncremental();
    }

    public void upgradeButtonPressed()
    {
        upgradeWindow = !upgradeWindow;
        upgrade.SetActive(upgradeWindow);
        //upgrade.SendMessage("updateTitle");
        //upgrade.SendMessage("updatePrice");
    }

    public void bonusButtonPressed()
    {
        bonusWindow = !bonusWindow;
        bonus.SetActive(bonusWindow);
    }

	public void helpButtonPressed()
	{
		tut.Reset ();
	}

    public void changeMODE()
    {
        if (!player.Incre.gameON)
        {
            showMessage("Minigame is not running. You can't change it to active mode!", "Minigame Not Running");
            player.Incre.passive = true;
            return;
        }

        //have enough time and stamina
        if (player.Incre.timeleft.cur > 0 && player.Incre.stamina.cur > 0)
        {
            player.Incre.progress.cur = 0; //set to zero when switching between active and passive mode
            player.Incre.passive = !player.Incre.passive;
        }
        else if (player.Incre.timeleft.cur == 0)
            showMessage("You don't have enough 'Time Left'!", "Error!");

        updateModeButton();
    }

    public void minigameStart(int whichGame)
    {
        Debug.Log(player.Incre.currentGame.ToString());
        string sceneName = "";
        minigame selectedGame = minigame.none;

        switch (whichGame)
        {
            case 1:
                sceneName = "Mode_Choose";
                selectedGame = minigame.seeker;
                break;
            case 2:
                sceneName = "Mastermind_Menu";
                selectedGame = minigame.mastermind;
                break;
            case 3:
                sceneName = "Menu";
                selectedGame = minigame.conquer;
                break;
        }

        //if game is not on
        if (!player.Incre.gameON)
        {
            player.Incre.gameON = true;
            player.Incre.currentGame = selectedGame;
            Debug.Log(player.Incre.currentGame.ToString());
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        else if (player.Incre.gameON == true && player.Incre.currentGame != selectedGame) //game is on
        {
            nextScene = sceneName;
            player.Incre.nextGame = selectedGame;
            showMessage2("If you change the minigame, you will lose data. Continue to change the game?",
				"Warning!", dialogMode.changeGame);
        }
        else
        {
            nextScene = "ui";
            player.Incre.nextGame = minigame.none;
            showMessage2("Do you want to close the minigame?", "Are you sure?", dialogMode.exitgame);
        }
    }

    public void changeScenes(string newScene)
    {
        SceneManager.LoadScene(newScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public void closeMessageBox()
    {
        dialog.SetActive(false);
    }

    private bool isPlayerActive()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Clicked");
            return true;
        }

		if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            return true;

        if (Input.GetKeyDown("up") || Input.GetKeyDown("down") || Input.GetKeyDown("right") || Input.GetKeyDown("left") ||
            Input.GetKeyDown("a") || Input.GetKeyDown("s") || Input.GetKeyDown("d") || Input.GetKeyDown("w"))
            return true;

        if (player.Incre.debugging)
            return true;

        return false;
    }

    public void onClickGetBonus()
    {
        strDele fun = new strDele(showMessage);
        bonusCode.analyzeCode(bonusCodeInput.text, fun);
    }

	private string randomWelcomeMessage ()
	{
		string[] wels = {
			"Welcome!!!!!!!!!!!!!!!!!!!!!!",
			"Hello!!!!!!",
			"Lootboxes will be coming soon!",
			"Some numbers got bigger!",
			"No anime girls!",
			"Hostile takeover!",
			"Glorious!",
			"Fearless!",
			"Spoilers!",
			"Activated with moisture!",
			"We'd probably be better off with a Minecraft clone!",
			"Games as a service!",
			"Gold team rules!",
			"Ellipsis warning!",
			"Better than nothing!"
		};

		int i = UnityEngine.Random.Range (0, wels.Length);

		return wels [i];
	}

    public void debug_seekerReward(int usedStamina)
    {
        player.getReward(minigame.seeker, usedStamina);
    }

    public void debug_MastermindReward(int usedStamina)
    {
        player.getReward(minigame.mastermind, usedStamina);
    }

    public void debug_ConquerorReward(int usedStamina)
    {
        player.getReward(minigame.conquer, usedStamina);
    }

    public void resetTime()
    {
        player.Incre.stamina.cur = 10;
        player.Incre.stamina.cur = 10;
    }

    public void debug_lvUp()
    {
        player.Incre.lv++;
    }

    public void debug_lvDown()
    {
        player.Incre.lv--;
    }

    public void debug_noTime()
    {
        player.Incre.timeleft.cur -= 60*5;
    }

    public void debug_yesTime()
    {
        player.Incre.timeleft.cur += 60 * 5;
    }

    public void debug_init()
    {
        player.Incre.needTutorial = false;
    }

    public void debug_addDay(int i)
    {
        loginScript.updatePassiveProgress(DateTime.Now.AddDays(-i));
        player.Incre.debugging = true;
        player.Incre.passivePlayingTimeLeft -= 7;
        Debug.Log("Remaining passive days = " + player.Incre.passivePlayingTimeLeft);
    }

    public void debug_playHour(int i)
    {
        loginScript.updateActiveProgress(DateTime.Now.AddHours(-i));
        player.Incre.debugging = true;
		Debug.Log("Remaining active playing time = " + player.Incre.activePlayingTimeLeft--);
    }

    public void debug_gameON()
    {
        player.Incre.gameON = true;
    }

    public void debug_save()
    {
        save();
    }

    void save()
    {
        player.Incre.lastLogOut = DateTime.Now;
        Debug.Log("Last logout saved: " + player.Incre.lastLogOut.ToString());

        if (player.isLocal)
        {
            Debug.Log("Local saving");
            player.localIncreData = player.getJsonStr(game.incremental);
            player.localMasterData = player.getJsonStr(game.mastermind);
            player.localSeekerData = player.getJsonStr(game.seeker);
            player.localConquerData = player.getJsonStr(game.conquer);
        }
        else
        {
            Debug.Log("Online saving");
            NetworkManager.Instance.QueueMessage(new List<string>() { "INCREMENTAL", player.getJsonStr(game.incremental) });
            NetworkManager.Instance.QueueMessage(new List<string>() { "MASTERMIND", player.getJsonStr(game.mastermind) });
            NetworkManager.Instance.QueueMessage(new List<string>() { "SEEKER", player.getJsonStr(game.seeker) });
            NetworkManager.Instance.QueueMessage(new List<string>() { "CONQUEROR", player.getJsonStr(game.conquer) });
        }
    }

    public void debug_load()
    {
    }
}