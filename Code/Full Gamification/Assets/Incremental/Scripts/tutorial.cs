using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class tutorial : MonoBehaviour
{
    public List<GameObject> tutorials;
    private int index;
    private int prevIndex;

    void Start ()
    {
        index = 0;
        prevIndex = 0;

        foreach (var tempTutorial in tutorials)
            tempTutorial.SetActive(false);
		
        if (player.Incre.needTutorial)
        {
            tutorials[index].SetActive(true);
            prevIndex = index;
            player.Incre.needTutorial = false;
        }
	}

	public void Reset ()
	{
		player.Incre.needTutorial = true;
		this.Start();
	}

    public void nextTutorial ()
    {
        index++;

        if (index < tutorials.Count)
		{
            tutorials[prevIndex].SetActive(false);
            tutorials[index].SetActive(true);
            prevIndex = index;
        }
        else
            tutorials[prevIndex].SetActive(false); //end tutorial
    }
}
