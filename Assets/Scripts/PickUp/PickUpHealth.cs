using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpHealth : I_EnterTrigger {

    
    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _healthPickUp;
    
    void Start()
    {
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (checkBounds())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (playerObject.GetComponent<Player>().canPickUpHealth())
                {
                    playerObject.GetComponent<Player>().healthRegain(50);
                    _uiManager.SetPickUpText("Health +50 picked up");
                    GetComponent<AudioSource>().PlayOneShot(_healthPickUp);

                    Destroy(gameObject);
                }
                else
                {
                    _uiManager.SetPickUpText("Health is full, can't pick up.");
                }

            }
        }
        

    }
}
