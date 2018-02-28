using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagManager : MonoBehaviour
{

    public GameObject Flag;
    public List<GameObject> flags;
    public Text buttonText;
    private bool isOnFlag;
    public GameObject flagToRemove;
    public Text flagCountText;
    private Color red;
    private Color white;

    // Use this for initialization
    void Start()
    {
        red = new Color(1, 0, 0, 1);
        white = new Color(1, 1, 1, 1);
        buttonText.text = "Place Flag";
        flagCountText.text = "Flags: 0/10";
        flags = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonText.text == "Place Flag")
        {
            isOnFlag = false;
        }
        else if (buttonText.text == "Remove Flag")
        {
            isOnFlag = true;
        }

        if(flags.Count == 10)
        {
            flagCountText.color = red; 
        }
        else
        {
            flagCountText.color = white;
        }
        flagCountText.text = "Flags: " + flags.Count + "/10";
        

    }

    public void placeFlag()
    {
        GameObject tempFlag;
        if (!isOnFlag)
        {  
            if (flags.Count == 10)
            {
                tempFlag = Instantiate(Flag, new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y, 0f), Quaternion.identity);
                flags.Add(tempFlag);
                Destroy(flags[0]);
                flags.RemoveAt(0);
            }
            else
            {
                tempFlag = Instantiate(Flag, new Vector3(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y, 0f), Quaternion.identity);
                flags.Add(tempFlag);
            }

        }
        else
        {
            int i = 0;
            int location = 0;
            foreach (GameObject tFlag in flags)
            {
                if (tFlag == flagToRemove)
                {
                    location = i;
                }
                i++;
            }

            Destroy(flags[location]);
            flags.RemoveAt(location);
        }

    }
}
