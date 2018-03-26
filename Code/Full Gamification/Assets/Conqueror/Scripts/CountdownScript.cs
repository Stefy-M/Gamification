using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conqueror {
	public class CountdownScript : MonoBehaviour {

	    //Count down variable that is set to false while the counter is greater than 1.
	    public bool countDownDone = false;
        public bool initialCountDownDone = false;
        public Sprite number3;
        public Sprite number2;
        public Sprite number1;
        private float timeLeft = 3.0f;
        

		// Use this for initialization
		void Start () {
            gameObject.GetComponent<SpriteRenderer>().sprite = number3;
		}
		
		// Update is called once per frame
		void Update () {
            if (!countDownDone)
            {
                timeLeft -= Time.deltaTime;
                if (timeLeft < 2.0f)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = number2;
                }
                if (timeLeft < 1.0f)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = number1;
                }
                if(timeLeft < 0.0f)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = null;
                    countDownDone = true;
                    initialCountDownDone = true;
                }
            }
        }

        public void resetCounter()
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = number3;
            timeLeft = 3.0f;
            countDownDone = false;
        }

        //Will need to reset the counter so that it goes back to initial behavior when player returns to main menu
        public void resetCounterMenu()
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = number3;
            timeLeft = 3.0f;
            countDownDone = false;
            initialCountDownDone = false;
        }


    }
}