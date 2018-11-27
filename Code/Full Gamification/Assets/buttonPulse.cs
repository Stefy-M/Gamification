using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonPulse : MonoBehaviour {

    bool pulseUp = true;
    public bool shouldPulse;
    public Image thisButton;
    float cur;

    private void OnEnable()
    {
        cur = 0.0f;
    }
    // Update is called once per frame
    void Update () {
        if (shouldPulse)
        {
            if (pulseUp)
            {
                cur += 0.01f;
                thisButton.color = new Color(cur, cur, cur);
                if (cur >= 0.9f)
                {
                    pulseUp = false;
                }
            }
            else
            {
                cur -= 0.01f;
                thisButton.color = new Color(cur, cur, cur);
                if (cur <= 0.2f)
                {
                    pulseUp = true;
                }
            }
        }
        else
        {
            thisButton.color = new Color(1f, 1f, 1f);
            cur = 0.0f;
        }

	}
}
