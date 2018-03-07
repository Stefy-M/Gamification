using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBack : MonoBehaviour {

    public float startX;
    public float startY;



    GameObject playerCharacter;

   public void teleport()
    {
        Vector3 temp = new Vector3(startX, startY, 0);
        playerCharacter = GameObject.Find("Player");
        playerCharacter.transform.position = temp;
    }

}
