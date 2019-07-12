using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPowerup : I_EnterTrigger {

    
    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _healthPickUp;
    [SerializeField]
    private powerUpType powerUp;

    private enum powerUpType
    {
        RAPIDFIRE,
        BERSERKER,
        INVINCIBILITY,
        UNLIMITEDAMMO
    }
    
    void Start()
    {
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (checkBounds())
        {

            switch (powerUp)
            {
                case powerUpType.RAPIDFIRE:
                    playerObject.GetComponent<Player>().pickupRapidFire();
                    _uiManager.SetPickUpText("RAPIDFIRE ACTIVATED.\nREADY TO BLEED.\nLASTS FOR " + Constants.rapidFireTime + ".");
                    break;
                case powerUpType.BERSERKER:
                    _uiManager.SetPickUpText("INCOMPLETE.");
                    break;
                case powerUpType.INVINCIBILITY:
                    playerObject.GetComponent<Player>().pickupInvincibility();
                    _uiManager.SetPickUpText("INVINCIBILITY ACIVATED.\nLET THEM COME. LET THEM ALL COME.\nLASTS FOR " + Constants.invincibilityTime + ".");
                    break;
                case powerUpType.UNLIMITEDAMMO:
                    _uiManager.SetPickUpText("INCOMPLETE.");
                    break;
                default:
                    break;
            }

            Destroy(gameObject);
        }
        

    }
}
