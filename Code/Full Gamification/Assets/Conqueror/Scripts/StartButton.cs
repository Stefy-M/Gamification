using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class StartButton : MonoBehaviour {
    

	// Use this for initialization
	void Start () {
        //make text file if it does not exist
        //Debug.Log(System.IO.Directory.GetCurrentDirectory());
        
        if (!PlayerPrefs.HasKey("ConqSave"))
        {
            PlayerPrefsX.SetStringArray("ConqSave", Resources.Load<TextAsset>("save").text.Split('\n'));
        }

		if (SceneManager.GetSceneByName ("Inventory") == SceneManager.GetActiveScene ()) {
            //remove later, for testing only
            player.Incre.stamina.cur += 1;
			//
			updateGuns ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartGame() {
		//use up stamina
		if (player.Incre.stamina.cur > 0) {
            player.Incre.stamina.cur -= 1;
			SceneManager.LoadScene ("Scene1");
		}
	}
	public void StageSelect() {
		
		SceneManager.LoadScene ("StageSelect");
	}
	public void Stage1() {
		if (player.Incre.stamina.cur > 0) {
            player.Incre.stamina.cur -= 1;
			SceneManager.LoadScene ("Scene1");
		}
	}
	public void Stage2() {
		if (player.Incre.stamina.cur > 0) {
            player.Incre.stamina.cur -= 1;
			SceneManager.LoadScene ("Scene2");
		}
	}
	public void Stage3() {
		if (player.Incre.stamina.cur > 0) {
            player.Incre.stamina.cur -= 1;
			SceneManager.LoadScene ("Scene3");
		}
	}
	public void Stage4() {
		if (player.Incre.stamina.cur > 0) {
            player.Incre.stamina.cur -= 1;
			SceneManager.LoadScene ("Scene4");
		}
	}
	public void Stage5() {
		if (player.Incre.stamina.cur > 0) {
            player.Incre.stamina.cur -= 1;
			SceneManager.LoadScene ("Scene5");
		}
	}
	public void Stage6() {
		if (player.Incre.stamina.cur > 0) {
            player.Incre.stamina.cur -= 1;
			SceneManager.LoadScene ("Scene6");
		}
	}
	public void Stage7() {
		if (player.Incre.stamina.cur > 0) {
            player.Incre.stamina.cur -= 1;
			SceneManager.LoadScene ("Scene7");
		}
	}
	public void Stage8() {
		if (player.Incre.stamina.cur > 0) {
            player.Incre.stamina.cur -= 1;
			SceneManager.LoadScene ("Scene8");
		}
	}
	public void Stage9() {
		if (player.Incre.stamina.cur > 0) {
            player.Incre.stamina.cur -= 1;
			SceneManager.LoadScene ("Scene9");
		}
	}
	public void Stage10() {
		if (player.Incre.stamina.cur > 0) {
            player.Incre.stamina.cur -= 1;
			SceneManager.LoadScene ("Scene10");
		}
	}

	public void loadInventory() {
		SceneManager.LoadScene ("Inventory");
	}
	public void EndGame() {
		Application.Quit ();
	}

	public void Return() {
		SceneManager.LoadScene ("Menu");
	}

	public void changeWeapon() {
		Debug.Log ("HITTING CHANGE WEAPON");
		string name = EventSystem.current.currentSelectedGameObject.name;
		Debug.Log (name);
		string[] num = name.Split ('n');
		name = num [2];
		Debug.Log (name);

		int x = 0;
		int.TryParse(name,out x);
			player.Conqueror.g = player.Conqueror.guns [x];

    }

	public void loadFarm() {
		if (player.Incre.stamina.cur > 0) {
            player.Incre.stamina.cur -= 1;
			SceneManager.LoadScene ("Farm");
		}
	}

	public void updateGuns() {
		Debug.Log (player.Conqueror.guns [1]);
		Button[] buttons = GameObject.FindGameObjectWithTag("GunPanel").GetComponentsInChildren<Button> ();
		//print (buttons [0].name);
		string[] lines = PlayerPrefsX.GetStringArray("ConqSave");
       

		//ensures there is never an invalid save file
		if (lines.Length <= 5) {
			Debug.Log ("Lines Length <5");
			string[] lines2 = new string[6];
			lines2[0] = "Player";
			lines2[1] = "hp 60";
			lines2[2] = "damage .3";
			lines2[3] = "skill 2";
			lines2[4] = "equip 0";
			lines2 [5] = "gun1 1 150 0";
            PlayerPrefsX.SetStringArray("ConqSave", lines2);
			lines = PlayerPrefsX.GetStringArray("ConqSave");
        }




		Debug.Log (lines.Length + "Line Length");

		int i = 0;
		foreach (Button button in buttons) {

			try {
				if (player.Conqueror.guns[i].damage != 0) {
					button.GetComponentInChildren<Text> ().text = "Damage: " + (player.Conqueror.guns [i].damage * player.Conqueror.guns[i].speed) + "x" + (player.Conqueror.guns [i].type + 1);
				}
				else {
					button.GetComponentInChildren<Text> ().text = "No Gun";
				}
			}
			catch {
				Debug.Log ("Exception with updateGuns");
				button.GetComponentInChildren<Text> ().text = "No Gun";
			}
			i++;
		}
	}

	public void deleteGun() {
		GameObject text = GameObject.Find ("GunText");
		string[] lines = PlayerPrefsX.GetStringArray("ConqSave");
		int i = 0;
		List<string> list = new List<string>(lines);
		foreach (string line in lines) {
			//if (text.GetComponentInChildren<Text> ().text.Contains ("gun")) {
			Debug.Log(text.GetComponentsInChildren<Text>()[1].text);
			if (line.Contains(text.GetComponentsInChildren<Text>()[1].text)) {
				if (i != 5) {
					list.RemoveAt (i);
				}
				}
			i++;
		}
		lines = list.ToArray ();

        PlayerPrefsX.SetStringArray("ConqSave", lines);
        updateGuns ();
	}

	public void SetSkill() {
		GameObject s = GameObject.Find ("Change Skill");
		string[] lines = PlayerPrefsX.GetStringArray("ConqSave");
		lines [3] = "skill 0";
        PlayerPrefsX.SetStringArray("ConqSave", lines);
    }
	public void SetSkill2() {
		GameObject s = GameObject.Find ("Change Skill2");
        string[] lines = PlayerPrefsX.GetStringArray("ConqSave");
        lines [3] = "skill 1";
        PlayerPrefsX.SetStringArray("ConqSave", lines);
    }
	public void SetSkill3() {
		GameObject s = GameObject.Find ("Change Skill3");
        string[] lines = PlayerPrefsX.GetStringArray("ConqSave");
        lines [3] = "skill 2";
        PlayerPrefsX.SetStringArray("ConqSave", lines);
    }
}
