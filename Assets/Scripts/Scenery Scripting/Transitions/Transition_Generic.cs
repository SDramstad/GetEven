using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition_Generic : MonoBehaviour
{

    private bool isInBounds;
    public string descriptiveTargetSceneName;
    public string targetSceneName;
    public int transitionId;
    private Player _player;

    private Game _gameManager;
    private UIManager _uiManager;

    void Start()
    {
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<Game>();
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
                _player = GameObject.Find("Player").GetComponent<Player>();
                _player.SavePlayer();
                _gameManager.SaveArea();
                GlobalGame.entryId = transitionId;
                _uiManager.SetLoadingScreen();
                SceneManager.LoadScene(targetSceneName);
            }
        }
    }
}
