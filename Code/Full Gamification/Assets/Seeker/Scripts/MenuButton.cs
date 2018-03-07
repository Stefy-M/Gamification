using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour {

    bool isClicked = false;
    string thisObject;
    GameObject temp;
    

    public void clickedBehavior()
    {
        thisObject = gameObject.name;
        switch (thisObject)
        {
            case "QuestButton":
                if(!isClicked)
                {
                    FindObject(GameObject.Find("QuestButton"), "QuestPanel").SetActive(true);
                    FindObject(GameObject.Find("InventoryButton"), "InventoryPanel").SetActive(false);
                    FindObject(GameObject.Find("StatsButton"), "StatsPanel").SetActive(false);
                    FindObject(GameObject.Find("MapButton"), "MapPanel").SetActive(false);
                    FindObject(GameObject.Find("UI"), "Overlay").SetActive(true);
                    isClicked = true;
                }
                else
                {
                    FindObject(GameObject.Find("QuestButton"), "QuestPanel").SetActive(false);
                    FindObject(GameObject.Find("UI"), "Overlay").SetActive(false);
                    isClicked = false;
                }
                break;
            case "StatsButton":
                if (!isClicked)
                {
                    FindObject(GameObject.Find("QuestButton"), "QuestPanel").SetActive(false);
                    FindObject(GameObject.Find("InventoryButton"), "InventoryPanel").SetActive(false);
                    FindObject(GameObject.Find("StatsButton"), "StatsPanel").SetActive(true);
                    FindObject(GameObject.Find("MapButton"), "MapPanel").SetActive(false);
                    FindObject(GameObject.Find("UI"), "Overlay").SetActive(true);
                    isClicked = true;
                }
                else
                {
                    FindObject(GameObject.Find("StatsButton"), "StatsPanel").SetActive(false);
                    FindObject(GameObject.Find("UI"), "Overlay").SetActive(false);
                    isClicked = false;
                }
                break;
            case "InventoryButton":
                if (!isClicked)
                {
                    FindObject(GameObject.Find("QuestButton"), "QuestPanel").SetActive(false);
                    FindObject(GameObject.Find("InventoryButton"), "InventoryPanel").SetActive(true);
                    FindObject(GameObject.Find("StatsButton"), "StatsPanel").SetActive(false);
                    FindObject(GameObject.Find("MapButton"), "MapPanel").SetActive(false);
                    FindObject(GameObject.Find("UI"), "Overlay").SetActive(true);
                    isClicked = true;
                }
                else
                {
                    FindObject(GameObject.Find("InventoryButton"), "InventoryPanel").SetActive(false);
                    FindObject(GameObject.Find("UI"), "Overlay").SetActive(false);
                    isClicked = false;
                }
                break;
            case "MapButton":
                if (!isClicked)
                {
                    FindObject(GameObject.Find("QuestButton"), "QuestPanel").SetActive(false);
                    FindObject(GameObject.Find("InventoryButton"), "InventoryPanel").SetActive(false);
                    FindObject(GameObject.Find("StatsButton"), "StatsPanel").SetActive(false);
                    FindObject(GameObject.Find("MapButton"), "MapPanel").SetActive(true);
                    FindObject(GameObject.Find("UI"), "Overlay").SetActive(true);
                    isClicked = true;
                }
                else
                {
                    FindObject(GameObject.Find("MapButton"), "MapPanel").SetActive(false);
                    FindObject(GameObject.Find("UI"), "Overlay").SetActive(false);
                    isClicked = false;
                }
                break;
            default:
                Debug.Log("Menu Button assigned incorrectly :(");
                break;
        }
    }

    public static GameObject FindObject(GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}
