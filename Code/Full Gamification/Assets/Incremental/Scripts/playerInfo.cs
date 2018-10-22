using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Newtonsoft.Json;
using Conqueror;

#region structs
public struct coin
{
    public int A_upgradeLv;
    public int P_upgradeLv;
    public int active;
    public int passive;
    public int numBooster;
    public int boosterRemain;
    public double boosterRate;
}
public struct exp
{
    public int A_upgradeLv;
    public int P_upgradeLv;
    public double cur;
    public int numBooster;
    public int boosterRemain;
    public double boosterRate;
}

public struct prog
{
    public int A_upgradeLv;
    public int P_upgradeLv;
    public double cur;
    public double max;
    public int numBooster;
    public int boosterRemain;
    public double boosterRate;
}

public struct stam
{
    public static double _cur = 15;
    public static double _max = 15;
    public double cur
    {
        get
        {
            return _cur;
        }
        set
        {
            if (value >= _max)
            {
                _cur = _max;
            }
            else
                _cur = value;
        }
    }
    public double max
    {
        get
        {
            return _max;
        }
    }
}

public struct timeLeft
{
    public static double _cur = 60 * 60;
    public static double _max = 60 * 60;

    public double cur
    {
        get
        {
            if (_cur < 0)
            {
                return 0;
            }
            return _cur;
        }
        set
        {
            if (value >= _max)
            {
                _cur = _max;
            }
            else
                _cur = value;
        }
    }
    public double max
    {
        get
        {
            return _max;
        }
    }
}
#endregion

public class IncrementalData
{
    public bool startNew = true;
    public bool needTutorial = true;
    public coin coin = new coin { active = 0, A_upgradeLv = 1, boosterRate = 1, boosterRemain = 0, numBooster = 0, passive = 0, P_upgradeLv = 1 };
    public exp exp = new exp { A_upgradeLv = 1, boosterRate = 1, boosterRemain = 0, numBooster = 0, P_upgradeLv = 1 };
    public prog progress = new prog { A_upgradeLv = 1, boosterRate = 1, boosterRemain = 0, numBooster = 0, P_upgradeLv = 1 };
    public stam stamina;
    public timeLeft timeleft;
    public int lv = 1;
    public string username = "Username";
    public int permanentPerksLV = 1;
    public bool startMessageDisplayed = false;
    public DateTime lastLogOut = DateTime.Now;
    public bool passive = false;
    public bool gameON = false;
    public minigame currentGame;
    public minigame nextGame;
    public bool debugging = false;
    public int activePlayingTimeLeft = 16; //16 hours
    public int passivePlayingTimeLeft = 16 * 7; //16weeks
    public string titleString = "";
    public string titleCollectionStr = "";
    public bool[] titleCollection = new bool[50];
    public int LogOutpassiveProgressGained = 0;
    public List<string> usedCode = new List<string>();
    //New features for Ascension
    public int seekerPerkLevel;
    public int sudokuPerkLevel;
    public int conquerorPerkLevel;
    public int daredevilPerkLevel;
    public int sokobanPerkLevel;
}

public enum game
{
    incremental, seeker, mastermind, conquer, daredevil, sokoban
}

public static class player
{
    public static bool isLocal = true;

    public static string localIncreData
    {
        get
        {
            return PlayerPrefs.GetString("localIncreData", JsonConvert.SerializeObject(new IncrementalData()));
        }
        set
        {
            PlayerPrefs.SetString("localIncreData", value);
        }
    }

    public static string localMasterData
    {
        get
        {
            return PlayerPrefs.GetString("localMasterData", JsonConvert.SerializeObject(new sudokuData()));
        }
        set
        {
            PlayerPrefs.SetString("localMasterData", value);
        }
    }

    public static string localSeekerData
    {
        get
        {
            return PlayerPrefs.GetString("localSeekerData", JsonConvert.SerializeObject(new GlobalControl()));
        }
        set
        {
            PlayerPrefs.SetString("localSeekerData", value);
        }
    }

    public static string localConquerData
    {
        get
        {
            return PlayerPrefs.GetString("localConquerData", "");
        }
        set
        {
            PlayerPrefs.SetString("localConquerData", value);
        }
    }

	public static string localDaredevilData
	{
		get
		{
			return PlayerPrefs.GetString("localDaredevilData", "");
		}
		set
		{
			PlayerPrefs.SetString("localDaredevilData", value);
		}
	}



	public static string JsonStr = "";
    public static IncrementalData Incre = new IncrementalData();
    public static sudokuData Sudoku = new sudokuData();
    public static ConquerorSave conqueror = new ConquerorSave();
    public static seeker seekerData = new seeker();
    //TODO: add in saving with the server for new daredevil and sokoban games
    public static DaredevilSave daredevilSave = new DaredevilSave();
    public static SokobanSave sokobanSave = new SokobanSave();

    public static string getJsonStr(game g)
	{
		switch (g) {
		    case game.incremental:
			    return JsonConvert.SerializeObject(Incre);
		    case game.mastermind:
			    return JsonConvert.SerializeObject(Sudoku);
		    case game.seeker:
			    return JsonConvert.SerializeObject(seekerData);
		    case game.conquer:
                return JsonConvert.SerializeObject(conqueror);
            case game.daredevil:
                return JsonConvert.SerializeObject(daredevilSave);
            case game.sokoban:
                return JsonConvert.SerializeObject(sokobanSave);
            default:
			    return "";
		}
    }

    public static void localLoad()
    {
        if (!isLocal)
            return;

        try
        {
            Debug.Log(localIncreData);
            Incre = JsonConvert.DeserializeObject<IncrementalData>(localIncreData);
        }
        catch
        {
            Incre = new IncrementalData();
        }
        try
        {
            Sudoku = JsonConvert.DeserializeObject<sudokuData>(localMasterData);
        }
        catch
        {
            Sudoku = new sudokuData();
        }
        try
        {
            GlobalControl.Instance = JsonConvert.DeserializeObject<GlobalControl>(localSeekerData);
        }
        catch
        {
            GlobalControl.Instance = new GlobalControl();
        }
    }

    public static void load()
    {
        JsonStrings loadedjsonStrings;

        try
        {
             loadedjsonStrings = JsonConvert.DeserializeObject<JsonStrings>(NetworkManager.Instance.loaded_json);
        }
        catch
        {
            loadedjsonStrings = new JsonStrings();
        }

        Debug.Log(loadedjsonStrings.incremental);
        Debug.Log(loadedjsonStrings.mastermind);
        Debug.Log(loadedjsonStrings.conqueror);
        Debug.Log(loadedjsonStrings.seeker);
        Debug.Log(loadedjsonStrings.daredevil);
        Debug.Log(loadedjsonStrings.sokoban);

        if (loadedjsonStrings.incremental.Length > 0) //doesn't go in blocks if loaded string is empty string
        {
            try
            {
                Incre = JsonConvert.DeserializeObject<IncrementalData>(loadedjsonStrings.incremental);
            }
            catch
            {
                Incre = new IncrementalData();
            }
        }
        else
            Incre = new IncrementalData();
		
        if (loadedjsonStrings.mastermind.Length > 0)
        {
            try
            {
                Sudoku = JsonConvert.DeserializeObject<sudokuData>(loadedjsonStrings.mastermind);
            }
            catch
            {
                Sudoku = new sudokuData();
            }
        }
        else
            Sudoku = new sudokuData();
		
        if (loadedjsonStrings.conqueror.Length > 0)
        {
            try
            {
                conqueror = JsonConvert.DeserializeObject<ConquerorSave>(loadedjsonStrings.conqueror.ToString());
            }
            catch(Exception ex)
            {
                Debug.Log(ex.ToString());
                conqueror = new ConquerorSave();
            }
        }
        else
            conqueror = new ConquerorSave();
		
        if (loadedjsonStrings.seeker.Length > 0)
        {
            try
            {
                seekerData = JsonConvert.DeserializeObject<seeker>(loadedjsonStrings.seeker);
            }
            catch
            {
                seekerData = new seeker();
            }
        }
        else
            seekerData = new seeker();

        if (loadedjsonStrings.daredevil.Length > 0)
        {
            try
            {
                daredevilSave = JsonConvert.DeserializeObject<DaredevilSave>(loadedjsonStrings.daredevil);
            }
            catch
            {
                daredevilSave = new DaredevilSave();
            }
        }
        else
            daredevilSave = new DaredevilSave();

        if (loadedjsonStrings.daredevil.Length > 0)
        {
            try
            {
                daredevilSave = JsonConvert.DeserializeObject<DaredevilSave>(loadedjsonStrings.daredevil);
            }
            catch
            {
                daredevilSave = new DaredevilSave();
            }
        }
        else
            daredevilSave = new DaredevilSave();

        if (loadedjsonStrings.sokoban.Length > 0)
        {
            try
            {
                sokobanSave = JsonConvert.DeserializeObject<SokobanSave>(loadedjsonStrings.sokoban);
            }
            catch
            {
                sokobanSave = new SokobanSave();
            }
        }
        else
            sokobanSave = new SokobanSave();

        Incre.username = NetworkManager.Instance.GetUsername ();
    }

    /// <summary>
    /// balanced reward function.
    /// 
    /// </summary>
    /// <param name="type">game type</param>
    /// <param name="usedStamina">used stamina to play one minigame</param>
    public static void getReward(minigame type, int usedStamina)
    {
        //invalid input
        if (usedStamina <= 0)
            return;

        //This part will be adjusted based on necessary playing time for each minigame 
        double percentage = 0;  //total 100 %
        double rewardRate = 2.5;

		switch (type) {
		case minigame.conquer:
			percentage = 20;
			break;
		case minigame.mastermind:
			percentage = 40;
			break;
		case minigame.seeker:
			percentage = 30;
			break;
		}
		
        percentage = percentage * 0.01;

        //reward coin
        double activeCoinReward = bal.getActiveCoinBonus() * usedStamina * percentage * rewardRate;
        double passiveCoinReward = bal.getPassiveCoinBonus() * usedStamina * percentage * rewardRate;
        player.Incre.coin.active += (int)activeCoinReward;
        player.Incre.coin.passive += (int)passiveCoinReward;

        //reward EXP
        double expReward = bal.getActiveEXPRate() * usedStamina * percentage * rewardRate;
        player.Incre.exp.cur += expReward;
    }
}