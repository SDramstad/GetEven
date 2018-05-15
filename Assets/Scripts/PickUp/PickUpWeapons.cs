using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapons : I_EnterTrigger {

    public string weapon;
    [SerializeField]
    private WeaponSwitcher _weaponManager;
    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _weaponPickUp;


    // Update is called once per frame
    void Update () {

        if (checkBounds())
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                switch (weapon)
                {
                    case "knife":
                        _weaponManager.hasKnife = true;
                        _uiManager.SetPDAText("Combat Knife", "It's a combat knife.");
                        break;
                    case "pistol":
                        _weaponManager.hasPistol = true;
                        _uiManager.SetPDAText("ARES P22-45 Handgun", "This handgun is increasingly common on the streets of Section-7 ever since ARES set up shop.");
                        break;
                    case "shotgun":
                        _weaponManager.hasShotgun = true;
                        _uiManager.SetPDAText("Direct Action Bulldog-10", "A powerful 12-Gauge semi-autoshotgun used professionally by MilSec.");
                        break;
                    case "rifle":
                        _weaponManager.hasRifle = true;
                        _uiManager.SetPDAText("Raptor MAC-23","A 10mm automatic rifle with good distance and solid accuracy.");
                        break;
                    default:
                        break;
                }

                //Debug.Log("Picked up " + weapon);
                GetComponent<AudioSource>().PlayOneShot(_weaponPickUp);
                Destroy(gameObject);
            }
        }

    }
}
