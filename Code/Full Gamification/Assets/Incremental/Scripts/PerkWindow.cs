using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//This class will be used to create behavior of perk window screen after ascension - This will be a part of the incremental prefab 
public class PerkWindow : MonoBehaviour {

    //Mode: 0 for levelup screen, 1 for Ascension Screen, 2 for both
    int mode;
    public GameObject levelUpScreen;
    public GameObject ascendScreen;
    public GameObject switchModeButton;
    public Text titleText;
    public Text pointsText;
    bool switchAvailable = false;


    // Use this for initialization
    void OnEnable() {
        if (player.Incre.hasAscendPoint > 0)
        {
            mode = 1;
        }
        else if (player.Incre.hasLevelPoint > 0)
        {
            mode = 0;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(mode == 0)
        {
            levelUpScreen.SetActive(true);
            ascendScreen.SetActive(false);
            titleText.text = "Select a potion as a reward for leveling up!";
            pointsText.text = "Level Points: " + player.Incre.hasLevelPoint;

            //need to check if out of points and window is still up
            if (player.Incre.hasLevelPoint == 0)
            {
                if(player.Incre.hasAscendPoint > 0)
                {
                    switchModes();
                }
                else
                {
                    closeWindow();
                }
            }
        }
        if (mode == 1)
        {
            levelUpScreen.SetActive(false);
            ascendScreen.SetActive(true);
            titleText.text = "Select a perk as a reward for ascending!";
            pointsText.text = "Ascension Points: " + player.Incre.hasAscendPoint;

            //need to check if out of points and window is still up
            if (player.Incre.hasAscendPoint == 0)
            {
                if (player.Incre.hasLevelPoint > 0)
                {
                    switchModes();
                }
                else
                {
                    closeWindow();
                }
            }
        }
        if (player.Incre.hasAscendPoint > 0 && player.Incre.hasLevelPoint > 0)
            switchModeButton.SetActive(true);
        else
            switchModeButton.SetActive(false);

        if (player.Incre.hasAscendPoint == 0 && player.Incre.hasLevelPoint == 0)
            closeWindow();

        

    }

    public void switchModes()
    {
        if (mode == 0)
        {
            mode = 1;
        }
        else
        {
            mode = 0;
        }
    }

    public void closeWindow()
    {
        this.gameObject.SetActive(false);
    }

    //Actually rewarding player here
    
    //ascension rewards
    public void seekerReward()
    {
        player.Incre.seekerPerkLevel++;
        player.Incre.hasAscendPoint--;
    }

    public void sudokuReward()
    {
        player.Incre.sudokuPerkLevel++;
        player.Incre.hasAscendPoint--;
    }

    public void conqReward()
    {
        player.Incre.conquerorPerkLevel++;
        player.Incre.hasAscendPoint--;
    }

    public void ddReward()
    {
        player.Incre.daredevilPerkLevel++;
        player.Incre.hasAscendPoint--;
    }

    public void sokoReward()
    {
        player.Incre.sokobanPerkLevel++;
        player.Incre.hasAscendPoint--;
    }

    //potion rewards
    public void p1Reward()
    {
        player.Incre.hasLevelPoint--;
        player.Incre.progress.numBooster++;
    }

    public void p2Reward()
    {
        player.Incre.hasLevelPoint--;
        player.Incre.exp.numBooster++;
    }

    public void p3Reward()
    {
        player.Incre.hasLevelPoint--;
        player.Incre.coin.numBooster++;
    }


    /*function ideas
    -update titles (titles should display what perk level you are at)
    -update images (based on what the perk you are getting is)
    -upgrade perk (function to actually make the upgrade)
    -hover over or menu to show future unlocks (perk tree like thing)
    -Other ideas??
    */
}
