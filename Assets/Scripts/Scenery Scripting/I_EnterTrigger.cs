using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class I_EnterTrigger : MonoBehaviour {

    private bool isInBounds;
    protected Collider playerObject;
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            playerObject = other;
            isInBounds = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            playerObject = other;
            isInBounds = false;
        }
    }
    

    public bool checkBounds()
    {
        if (isInBounds)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
