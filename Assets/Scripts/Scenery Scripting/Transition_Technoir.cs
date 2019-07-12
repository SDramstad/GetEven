using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition_Technoir : MonoBehaviour {

    private bool isInBounds;
    public string descriptiveTargetSceneName;
    public string targetSceneName;

    private UIManager _uiManager;

    void Start()
    {
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            _uiManager.SetPromptText("Press Interact to enter " + descriptiveTargetSceneName + ".");
            isInBounds = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            _uiManager.HidePromptText();
            isInBounds = false;
        }
    }

    void Update()
    {
        if (isInBounds)
        {
            if (Input.GetButtonDown("Interact"))
            {
                SceneManager.LoadScene(targetSceneName);
            }
        }
    }
}
