using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this can have different types depending on the leaderboard type
public class leaderBoardEntry
{
    //This is where we will deserialize JSON from server into these categories
}



public class leaderboard : MonoBehaviour {
    public GameObject title;
    public GameObject table;
    public GameObject rowPrefab;
    //this will be used to store 10 rows as a game object
    List<GameObject> rows;
    //This will be where we deserialize our leaderboards into
    List<leaderBoardEntry> lbType1 = new List<leaderBoardEntry>();
    List<leaderBoardEntry> lbType2 = new List<leaderBoardEntry>();

    // Use this for initialization
    void Start () {
        rows = new List<GameObject>();
    }
	

	// Update is called once per frame
	void Update () {
		
	}

    //functions we will need:
    //change leaderboard mode
    //get info from server
    //-deserialize json
    //-populate rows
    //  -Populate leaderboard(mode, leaderboard)
    //      -Populate row (rank, username, level)
    //Refresh leaderboard
    //Make sure opt in/opt out work correctly

    //SERVERSIDE:
    //script that puts top 10 of whatever category (can be as many as makes sense) from mongoDB into a json formatted string and sends it back to game.
    //then script sends message back to game with correctly formatted string.

}
