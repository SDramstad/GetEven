using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_InteractiveObj : MonoBehaviour {

    UIManager _uiManager;
    //bool interacted;
    A_GenericInteractEvent genericEvent;
    bool isInBounds;
    [SerializeField]
    string displayText = "Press Interact to Use Object";

    void Start()
    {
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        genericEvent = GetComponent<A_GenericInteractEvent>();
        isInBounds = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (isInBounds)
        {
            //display interact text
            if (Input.GetButtonDown("Interact"))
            {
                genericEvent.Run();
            }
        }
        
	}
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            isInBounds = true;
        }
    }
    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.name == "Player")
        {
            _uiManager.SetPromptText(displayText);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            isInBounds = false;
            _uiManager.HidePromptText();
        }
    }
}
