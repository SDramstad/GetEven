using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1M1_PowerBox : MonoBehaviour {


    private bool isInBounds;
    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private bool _powerBoxDeactivated;
    [SerializeField]
    private GameObject _spotlight;
    [SerializeField]
    private GameObject _thisBoxLight;
    //[SerializeField]
    //private GameObject _thisBoxLightSource;


    void Start()
    {
        _powerBoxDeactivated = false;
        isInBounds = false;
        //_thisBoxLight = GetComponentInChildren<Material>();
    }
    
    void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.name == "Player")
        {
            if(!_powerBoxDeactivated)
            {
                _uiManager.SetPromptText("Press E to turn off the powerbox.");
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
            if (!_powerBoxDeactivated)
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
            if (Input.GetKeyDown(KeyCode.E) && !_powerBoxDeactivated)
            {
                _powerBoxDeactivated = true;
                _thisBoxLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);
                //_thisBoxLightSource.GetComponent<Light>().color
                GlobalGame.e1m1_powerboxes += 1;

                if (GlobalGame.e1m1_powerboxes == 2)
                {
                    _spotlight.gameObject.SetActive(false);
                }
            }
        }
    }
}
