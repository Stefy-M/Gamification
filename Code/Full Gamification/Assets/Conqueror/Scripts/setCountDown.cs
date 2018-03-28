using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class sets the countdown to true when it hits 1.
namespace Conqueror {
public class setCountDown : MonoBehaviour {
	    private CountdownScript cs;
        public Animator cdAnim;

		public void setCountdown()
	    {
	        cs = GameObject.Find("GameManager").GetComponent<CountdownScript>();
	        //countDownDone gets set to true when it is done counting down.
	        cs.countDownDone = true;
	    }
        //this resets the countdown
        public void resetCountdown()
        {
            cs = GameObject.Find("GameManager").GetComponent<CountdownScript>();
            cs.countDownDone = false;          
        }
        //this replays the countdown from the start of the animation
        public void playCountdown()
        {
            cdAnim.Play("Countdown", -1, 0f);
        }
	}
}
