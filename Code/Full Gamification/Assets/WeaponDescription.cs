using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDescription : MonoBehaviour {

    // Use this for initialization
    public GameObject startMenu;
    public GameObject wepDescOverlay;
    
    public void displayWepDesc()
    {
        startMenu.SetActive(false);
        wepDescOverlay.SetActive(true);
    }

    public void displayStartMenu()
    {
        startMenu.SetActive(true);
        wepDescOverlay.SetActive(false);
    }
}
