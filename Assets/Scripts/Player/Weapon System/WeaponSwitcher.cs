using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitcher : MonoBehaviour
{

    public static string activeWeaponType;

    public GameObject knife;
    public GameObject pistol;
    public GameObject shotgun;
    public GameObject unarmed;
    public GameObject rifle;

    public bool hasKnife;
    public bool hasPistol;
    public bool hasShotgun;
    public bool hasRifle;

    GameObject activeGun;

    // Use this for initialization
    void Start()
    {
        activeWeaponType = Constants.Unarmed;
        activeGun = unarmed;
    }

    private void loadWeapon(GameObject weapon)
    {
        knife.SetActive(false);
        pistol.SetActive(false);
        shotgun.SetActive(false);
        rifle.SetActive(false);
        unarmed.SetActive(false);

        weapon.SetActive(true);
        activeGun = weapon;
        //gameUI.SetAmmoText(ammo.GetAmmo(activeGun.tag));
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown("`"))
        {
            loadWeapon(unarmed);
            activeWeaponType = Constants.Unarmed;
        }
        else if (Input.GetKeyDown("1") && hasKnife)
        {
            loadWeapon(knife);
            activeWeaponType = Constants.Knife;
            //gameUI.UpdateReticle();

        }
        else if (Input.GetKeyDown("2") && hasPistol)
        {
            loadWeapon(pistol);
            activeWeaponType = Constants.Handgun;
            //gameUI.UpdateReticle();
        }
        else if (Input.GetKeyDown("3") && hasShotgun)
        {
            loadWeapon(shotgun);
            activeWeaponType = Constants.Shotgun;
            //gameUI.UpdateReticle();
        }
        else if (Input.GetKeyDown("4") && hasRifle)
        {
            loadWeapon(rifle);
            activeWeaponType = Constants.Rifle;
            //gameUI.UpdateReticle();
        }
    }

    public GameObject GetActiveWeapon()
    {
        return activeGun;
    }
}

