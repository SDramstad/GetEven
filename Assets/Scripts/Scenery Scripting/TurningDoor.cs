using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningDoor : MonoBehaviour
{

    private int state;
    //open/closed

    private bool pressed;

    private Quaternion EndRot1, EndRot2;

    private void Start()
    {
        EndRot1 = Quaternion.Euler(0, 90, 0); // Could be also set from the Inspector
        EndRot2 = Quaternion.identity;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            pressed = true;

        if (pressed) // Rotation
        {
            if (transform.rotation == (state == 0 ? EndRot1 : EndRot2)) // Did the rotation end?
            {
                state ^= 1; // Inverts "State" from 0 to 1 and reverse
                pressed = false;
            } // Slerp Method Here
            else this.transform.rotation = Quaternion.Lerp(transform.rotation, state == 0 ? EndRot1 : EndRot2, Time.deltaTime * 5);
        }
    }
}