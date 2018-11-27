using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

public class leaderboard : MonoBehaviour {
    public GameObject title;
    public GameObject table;
    public GameObject loading;
    public GameObject rowPrefab;
    //this will be used to store rows as a game object
    List<GameObject> rows;
    //This will be where we deserialize our leaderboards into
    List<IncrementalData> lbList = new List<IncrementalData>();

    string lbString = null;
    bool hasLB = false;

    // Use this for initialization
    void OnEnable () {
        //immediately put in request for leaderboard when this window is active
        rows = new List<GameObject>();
        GetLeaderboard();
    }

    // Update is called once per frame
    void Update () {
        //this happens if we need to get a new leaderboard from server
        if (NetworkManager.Instance.leaderboardInfo != null && lbString == null)
        {
            loading.SetActive(false);
            lbString = NetworkManager.Instance.leaderboardInfo;
            Debug.Log("From leaderboard: " + lbString);
            parseLeaderboard();
            buildLeaderboard();
            hasLB = true;
        }
        //if we havent gotten lb from server yet, display loading symbol
        else if (!hasLB)
        {
            //play loading animation
            loading.SetActive(true);
        }
	}

    //gets a new version of the leaderboard from the server
    public void GetLeaderboard()
    {
        //we no longer have lb (play loading anim)
        hasLB = false;
        //clear out string we are getting from server
        lbString = null;
        //clear out network string
        NetworkManager.Instance.leaderboardInfo = null;
        //clear out rows for new set of gameobjects.
        foreach(GameObject temp in rows)
        {
            Destroy(temp);
        }
        rows.Clear();
        rows = new List<GameObject>();
        //clear out lbList
        lbList.Clear();
        lbList = new List<IncrementalData>();
        //First part of function is getting the leaderboard info from the server.
        NetworkManager.Instance.requestLeaderboard();
        return;
    }


    //Parses leaderboard info from server and puts it into a list of 
    void parseLeaderboard()
    {
        string[] delim = new string[] { "|||" };
        string[] lbStrings = lbString.Split(delim, System.StringSplitOptions.None);

        IncrementalData tempJSON;
        int i = 1;
        //Turning each string into an Incremental Data in the list lbList
        foreach(string tempentry in lbStrings)
        {
            //There is always an empty string, fail on server side code.
            if(tempentry == "")
            {
                continue;
            }

            //remove objectid (should really do this on the server side)
            //wow c# is annoying, probably a better way to do this though
            string entry1 = tempentry.Remove(1, 64);
            string entry = entry1.Remove(entry1.Length - 1);
            Debug.Log(entry);

            try
            {
                tempJSON = JsonConvert.DeserializeObject<IncrementalData>(entry);
                Debug.Log("Successfully converted string " + i);
                lbList.Add(tempJSON);
            }
            catch
            {
                Debug.Log("Error converting string " + i);
            }
            i++;
        }

        return;
    }

    //this function builds the leaderboard ui to display to the user.
    void buildLeaderboard()
    {
        int i = 0;
        //puts relevant lb information in the text field of each entry in leaderboard.
        foreach (IncrementalData incre in lbList)
        {

            GameObject temp = Instantiate(rowPrefab, table.GetComponent<Transform>());
            Text tempNameText = temp.transform.GetChild(0).GetComponent<Text>();
            Text tempLevelText = temp.transform.GetChild(1).GetComponent<Text>();
            tempNameText.text = "" + (i+1) + ".\t" + incre.username;
            tempLevelText.text = "A: " + (incre.permanentPerksLV -1) + " LV: " + incre.lv;
            //now we move it down depending on what entry in the leaderboard it is.
            temp.transform.Translate(0, (float)(-0.66 * i), 0);
            //change color of row
            switch (i + 1)
            {
                //gold color for rank 1
                case 1:
                    temp.GetComponent<Image>().color = new Color(1.0f, 0.8431f, 0f);
                    break;
                //silver for 2
                case 2:
                    temp.GetComponent<Image>().color = new Color(0.7529f, 0.7529f, 0.7529f);
                    break;
                //bronze for 3
                case 3:
                    temp.GetComponent<Image>().color = new Color(0.8039f, 0.4980f, 0.19607f);
                    break;
                //light blue for the rest
                default:
                    temp.GetComponent<Image>().color = new Color((173f/255f), (216f/255f), (230f/255f));
                    break;

            }
            //add it here so we can destroy it later when updating lb.
            rows.Add(temp);
            i++;
        }
    }
    


    //functions we will need:
    //change leaderboard mode
    //get info from server
    //-deserialize json
    //-populate rows
    //-Populate leaderboard(leaderboard)
    //-Populate row (rank, username, Ascension/Level)
    //Refresh leaderboard
    //Make sure opt in/opt out work correctly

    //SERVERSIDE:
    //script that puts top 10 of whatever category (can be as many as makes sense) from mongoDB into a json formatted string and sends it back to game.
    //then script sends message back to game with correctly formatted string.

}
