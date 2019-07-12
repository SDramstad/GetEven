using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAmmo : I_EnterTrigger {

    public string ammoType;
    public int ammoAmount;
    [SerializeField]
    private Ammo _ammo;
    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private AudioClip _ammoPickupSound;
    
	
	// Update is called once per frame

    void Start()
    {
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _ammo = GameObject.Find("Player").GetComponent<Ammo>();
    }

	void Update () {

        if (checkBounds())
        {
            if (!_ammo.IsFull(ammoType))
            {
                switch (ammoType)
                {

                    case "Handgun":
                        _ammo.AddAmmo("Handgun", ammoAmount);
                        break;
                    case "Shotgun":
                        _ammo.AddAmmo("Shotgun", ammoAmount);
                        break;
                    case "Rifle":
                        _ammo.AddAmmo("Rifle", ammoAmount);
                        break;
                    default:
                        break;
                }
                _uiManager.SetPickUpText("Picked up " + ammoAmount + " " + ammoType.ToLower() + " ammo.");
                GetComponent<AudioSource>().PlayOneShot(_ammoPickupSound);
                Destroy(gameObject);
            }
            else
            {
                _uiManager.SetPickUpText("Can't pick up " + ammoType + ", already full.");
            }
            //if (Input.GetButtonDown("Interact"))
            //{

            //}
        }

    }
}
