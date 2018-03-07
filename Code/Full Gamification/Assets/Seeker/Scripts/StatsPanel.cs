using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour {

    public Text health;
    public Text stamina;
    public Text dexterity;
    public Text insight;
    public Text population;
    public Text rep;
    public Text totalUpgrades;


	// Use this for initialization
	void Start () {
        health.text = "Health: " + GlobalControl.Instance.hp.ToString();
        stamina.text = "Stamina: " + GlobalControl.Instance.stam.ToString();
        dexterity.text = "Dexterity: " + GlobalControl.Instance.dex.ToString();
        insight.text = "Insight: " + GlobalControl.Instance.ins.ToString();
        population.text = "Population: " + GlobalControl.Instance.current_population + "/" + GlobalControl.Instance.max_population;
        rep.text = "Reputation: " + GlobalControl.Instance.reputation;
        totalUpgrades.text = "Total Upgrades: " + (((GlobalControl.Instance.hp / 10) - 1) + ((GlobalControl.Instance.stam / 10) - 1) + (GlobalControl.Instance.dex - 1) + (GlobalControl.Instance.ins - 1)).ToString();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
