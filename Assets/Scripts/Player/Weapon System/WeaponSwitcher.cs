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
    //public GameObject rocketgun;
    public GameObject unarmed;
    public GameObject rifle;
    public GameObject frag;

    public bool hasKnife;
    public bool hasPistol;
    public bool hasShotgun;
    public bool hasRifle;
    public bool hasRocketgun;

    GameObject activeGun;
    Grabber grabManager;


    //private float knifeFireRate;
    //private float pistolFireRate;
    //private float shotgunFireRate;
    //private float rifleFireRate;

    // Use this for initialization
    void Start()
    {
        hasKnife = GlobalControl.Instance.savedPlayerData.hasKnife;
        hasPistol = GlobalControl.Instance.savedPlayerData.hasPistol;
        hasShotgun = GlobalControl.Instance.savedPlayerData.hasShotgun;
        hasRifle = GlobalControl.Instance.savedPlayerData.hasRifle;
        hasRocketgun = GlobalControl.Instance.savedPlayerData.hasRocketgun;
        activeWeaponType = Constants.Unarmed;
        activeGun = unarmed;

        //knifeFireRate = knife.GetComponent<Weapon>().fireRate;
        //pistolFireRate = pistol.GetComponent<Weapon>().fireRate;
        //shotgunFireRate = shotgun.GetComponent<Weapon>().fireRate;
        //rifleFireRate = rifle.GetComponent<Weapon>().fireRate;

        //TODO: Should be an inspector reference
        grabManager = GameObject.Find("FirstPersonCharacter").GetComponent<Grabber>();
    }

    public void loadWeapon(GameObject weapon)
    {
        knife.SetActive(false);
        pistol.SetActive(false);
        shotgun.SetActive(false);
        rifle.SetActive(false);
        //rocketgun.SetActive(false);
        unarmed.SetActive(false);

        weapon.SetActive(true);
        activeGun = weapon;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Unarmed"))
        {
            loadWeapon(unarmed);
            activeWeaponType = Constants.Unarmed;
        }
        else if (Input.GetButtonDown("Knife") && hasKnife)
        {
            loadWeapon(knife);
            activeWeaponType = Constants.Knife;
            //gameUI.UpdateReticle();

        }
        else if (Input.GetButtonDown("Pistol") && hasPistol)
        {
            loadWeapon(pistol);
            activeWeaponType = Constants.Handgun;
            //gameUI.UpdateReticle();
        }
        else if (Input.GetButtonDown("Shotgun") && hasShotgun)
        {
            loadWeapon(shotgun);
            activeWeaponType = Constants.Shotgun;
            //gameUI.UpdateReticle();
        }
        else if (Input.GetButtonDown("Rifle") && hasRifle)
        {
            loadWeapon(rifle);
            activeWeaponType = Constants.Rifle;
            //gameUI.UpdateReticle();
        }
        //else if (Input.GetButtonDown("Rocketgun") && hasRocketgun)
        //{
        //    Debug.Log("Rocket button pressed.");      
        //    loadWeapon(rocketgun);
        //    activeWeaponType = Constants.Rocketgun;
        //    //gameUI.UpdateReticle();
        //}
        else if (Input.GetButtonDown("Grenade"))
        {
            //spawn a frag grenade
            GameObject spawnedFrag = Instantiate(frag);
            //put it in front of you
            spawnedFrag.transform.position = grabManager.transform.position + grabManager.transform.forward;
            //firmly grasp it
            //TODO: FIGURE OUT NICE SOLUTION FOR THIS
            //grabManager.GrenadeGrab(spawnedFrag);
            spawnedFrag.GetComponentInChildren<Rigidbody>().AddForce(grabManager.transform.forward * 30, ForceMode.Impulse);

        }

    }

    public void RapidFire()
    {
        var weaponList = new List<Weapon>();
        weaponList.Add(knife.GetComponent<Weapon>());
        weaponList.Add(pistol.GetComponent<Weapon>());
        weaponList.Add(shotgun.GetComponent<Weapon>());
        weaponList.Add(rifle.GetComponent<Weapon>());

        foreach (var weapon in weaponList)
        {
            weapon.fireRate = weapon.baseFireRate * .5f;
        }
    }

    public void EndRapidFire()
    {
        var weaponList = new List<Weapon>();
        weaponList.Add(knife.GetComponent<Weapon>());
        weaponList.Add(pistol.GetComponent<Weapon>());
        weaponList.Add(shotgun.GetComponent<Weapon>());
        weaponList.Add(rifle.GetComponent<Weapon>());

        foreach (var weapon in weaponList)
        {
            weapon.fireRate = weapon.baseFireRate;
        }
    }

    public GameObject GetActiveWeapon()
    {
        return activeGun;
    }
}

