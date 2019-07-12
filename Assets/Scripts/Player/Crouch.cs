using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour {

    public CharacterController characterController;
    private bool startSliding;
    private bool crouched;
	// Use this for initialization
	void Start () {
        characterController = gameObject.GetComponent<CharacterController>();

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("ToggleCrouch"))
        {
            if (crouched)
            {
                crouched = false;
                characterController.height = 1.8f;
            }
            else
            {
                crouched = true;
                characterController.height = 0.9f;
            }
        }
	}
}
