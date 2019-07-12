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

    private GameObject _player;
    private Ammo _ammo;

    private bool destroyAfterUse;

    void Start()
    {
        _player = GameObject.Find("Player");
        _weaponManager = _player.GetComponent<WeaponSwitcher>();
        _ammo = _player.GetComponent<Ammo>();
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        destroyAfterUse = true;
    }

    // Update is called once per frame
    void Update () {

        if (checkBounds())
        {

            if (Input.GetButtonDown("Interact"))
            {
                switch (weapon)
                {
                    case "knife":
                        //check if user has weapon already
                        if (_weaponManager.hasKnife)
                        {
                            _uiManager.SetPickUpText("Already have this weapon.");
                            destroyAfterUse = false;
                        } 
                        else
                        {
                            _weaponManager.hasKnife = true;
                            _uiManager.SetPDAText("Combat Knife", "It's a combat knife.");
                        }
                        break;
                    case "pistol":
                        if (_weaponManager.hasPistol)
                        {
                            //add a small amount of ammo
                            if (!_ammo.IsFull("Handgun"))
                            {
                                _ammo.AddAmmo(Constants.Handgun, 4);
                                _uiManager.SetPickUpText("Picked up 4 handgun bullets.");
                            }
                            else
                            {
                                _uiManager.SetPickUpText("Can't pick up, handgun ammo is full.");
                                destroyAfterUse = false;
                            }
                        }
                        else
                        {
                            _weaponManager.hasPistol = true;
                            _uiManager.SetPDAText("ARES P22-45 Handgun", "This handgun is increasingly common on the streets of Section-7 ever since ARES set up shop."); 
                        }
                        break;
                    case "shotgun":
                        if (_weaponManager.hasShotgun)
                        {
                            //add a small amount of ammo
                            if (!_ammo.IsFull("Shotgun"))
                            {
                                _ammo.AddAmmo("Shotgun", 2);
                                _uiManager.SetPickUpText("Picked up 2 shotgun shells.");

                            }
                            else
                            {
                                _uiManager.SetPickUpText("Can't pick up, shotgun ammo is full.");
                                destroyAfterUse = false;
                            }
                        }
                        else
                        {
                            _weaponManager.hasShotgun = true;
                            _uiManager.SetPDAText("Direct Action Bulldog-10", "A powerful 12-Gauge semi-autoshotgun used professionally by MilSec.");
                        }
                        break;
                    case "rifle":

                        if (_weaponManager.hasRifle)
                        {
                            if (!_ammo.IsFull("Rifle"))
                            {
                                _ammo.AddAmmo("Rifle", 8);
                                _uiManager.SetPickUpText("Picked up 8 rifle rounds.");

                            }
                            else
                            {
                                _uiManager.SetPickUpText("Can't pick up, rifle ammo is full.");
                                destroyAfterUse = false;
                            }
                        } 
                        else
                        {
                            _weaponManager.hasRifle = true;
                            _uiManager.SetPDAText("Raptor MAC-23", "A 10mm automatic rifle with good distance and solid accuracy.");
                        }
                        break;
                    default:
                        break;
                }
                
                if (destroyAfterUse)
                {
                    GetComponent<AudioSource>().PlayOneShot(_weaponPickUp);
                    Destroy(gameObject);
                }
            }
        }

    }
}
