using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class muzzleFlash : MonoBehaviour {

    public GameObject smokeFlash;
    public float flashTime;

    private void Start()
    {
        Deactivate();
    }

    public void Activate()
    {
        smokeFlash.SetActive(true);
        Invoke("Deactivate", flashTime);
    }
    public void Deactivate()
    {
        smokeFlash.SetActive(false);
    }

}
