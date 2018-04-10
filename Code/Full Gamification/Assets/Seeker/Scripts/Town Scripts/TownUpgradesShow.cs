using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUpgradesShow : MonoBehaviour
{

    public Text maxpop;
    public Text training;
    public Text forge;
    public Text herb;
    public Text bakery;
    public Text reputation;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        maxpop.text = "Maximum Population: " + GlobalControl.Instance.max_population;
        training.text = "Training Multiplier: " + GlobalControl.Instance.training_level.ToString() + "x";
        forge.text = "Forge Shop: Level " + GlobalControl.Instance.forge_level.ToString();
        herb.text = "Herb Shop:  Level " + GlobalControl.Instance.herb_level.ToString();
        bakery.text = "Bakery Shop: Level " + GlobalControl.Instance.bakery_level.ToString();
        reputation.text = GlobalControl.Instance.reputation.ToString() + " Rep";
    }
}
