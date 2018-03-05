using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullScript : MonoBehaviour
{
    // Kill bullets that are offscreen
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Arena"))
            Destroy(gameObject);
    }
}
