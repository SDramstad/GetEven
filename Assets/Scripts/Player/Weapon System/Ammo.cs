using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour {
    
    //These are the default starting values for Ammo
    
    [SerializeField]
    public int maxHandgunAmmo = 120;
    [SerializeField]
    public int maxShotgunAmmo = 100;
    [SerializeField]
    public int maxRocketAmmo = 80;
    [SerializeField]
    public int maxRifleAmmo = 250;
    //[SerializeField]
    //public int knifeAmmo = 9999999;
    //[SerializeField]
    //public int handgunAmmo = 9;
    //[SerializeField]
    //public int shotgunAmmo = 20;
    //[SerializeField]
    //public int rocketAmmo = 20;
    //[SerializeField]
    //public int rifleAmmo = 25;

    public Dictionary<string, int> tagToAmmo;

    public void Start()
    {
        tagToAmmo = new Dictionary<string, int> {
            {Constants.Knife, 1},
            {Constants.Handgun, 0},
            {Constants.Shotgun, 0},
            {Constants.Rifle, 0},
            {Constants.Rocketgun, 0},
            {Constants.Unarmed, 0},
        };
    }
    
    public int[] gatherAmmoList()
    {
        int[] ammoList = { tagToAmmo[Constants.Handgun], tagToAmmo[Constants.Shotgun], tagToAmmo[Constants.Rifle], tagToAmmo[Constants.Rocketgun] };
        return ammoList;
    }

    internal bool IsFull(string ammoType)
    {
        bool results = false;

        switch(ammoType)
        {
            case "Handgun":
                if (tagToAmmo[Constants.Handgun] > maxHandgunAmmo)
                {
                    results = true;
                }
                break;
            case "Shotgun":
                if (tagToAmmo[Constants.Shotgun] > maxShotgunAmmo)
                {
                    results = true;
                }
                break;
            case "Rifle":
                if (tagToAmmo[Constants.Rifle] > maxRifleAmmo)
                {
                    results = true;
                }
                break;
            case "Rocketgun":
                if (tagToAmmo[Constants.Rocketgun] > maxRocketAmmo)
                {
                    results = true;
                }
                break;
            default:
                break;
        }

        return results;
    }

    public void AddAmmo(string tag, int ammo)
    {
        if (!tagToAmmo.ContainsKey(tag))
        {
            Debug.LogError("Unrecognized gun type passed: " + tag);
        }

        tagToAmmo[tag] += ammo;

        //normalize the ammo type if it goes over max
        if (IsFull(tag))
        {
            normalizeAmmo(tag);
        }
        
    }

    private void normalizeAmmo(string tag)
    {

        switch (tag)
        {
            case "Handgun":
                tagToAmmo[Constants.Handgun] = maxHandgunAmmo;
                break;
            case "Shotgun":
                tagToAmmo[Constants.Shotgun] = maxShotgunAmmo;
                break;
            case "Rifle":
                tagToAmmo[Constants.Rifle] = maxRifleAmmo;
                break;
            case "Rocketgun":
                tagToAmmo[Constants.Rocketgun] = maxRocketAmmo;
                break;
            default:
                break;
        }
    }

    //checks if gun has ammo
    public bool HasAmmo(string tag)
    {
        if (!tagToAmmo.ContainsKey(tag))
        {
            Debug.LogError("Unrecognized gun type passed: " + tag);
        }

        return tagToAmmo[tag] > 0;
    }

    public int GetAmmo(string tag)
    {
        if (!tagToAmmo.ContainsKey(tag))
        {
            Debug.LogError("Unrecognized gun type passed:" + tag);
        }

        return tagToAmmo[tag];
    }

    public void ConsumeAmmo(string tag)
    {
        if (!tagToAmmo.ContainsKey(tag))
        {
            Debug.LogError("Unrecognized gun type passed:" + tag);
        }

        tagToAmmo[tag]--;
    }
}
