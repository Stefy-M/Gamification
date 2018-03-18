using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setCountDown : MonoBehaviour {
    private CountdownScript cs;

    public void setCountdown()
    {
        cs = GameObject.Find("GameManager").GetComponent<CountdownScript>();
        cs.counterDownDone = true;
    }
}
