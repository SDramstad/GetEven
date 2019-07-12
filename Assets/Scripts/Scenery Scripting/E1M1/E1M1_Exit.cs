using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class E1M1_Exit : MonoBehaviour
{

    private bool isInBounds;
    [SerializeField]
    private UIManager _uiManager;

    void Start()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if (GlobalGame.e1m1_powerboxes == 2)
            {
                _uiManager.SetPromptText("Press E to leave E1M1.");
                isInBounds = true;
            }
            else
            {
                _uiManager.HidePromptText();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if (GlobalGame.e1m1_powerboxes == 2)
            {
                _uiManager.HidePromptText();
                isInBounds = false;
            }
        }
    }

    void Update()
    {
        if (isInBounds)
        {
            if (Input.GetKeyDown(KeyCode.E) && (GlobalGame.e1m1_powerboxes == 2))
            {
                _uiManager.SetLoadingScreen();
                SceneManager.LoadScene("E1M3");
            }
        }
    }
}