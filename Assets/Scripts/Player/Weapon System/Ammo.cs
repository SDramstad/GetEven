using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour {

    [SerializeField]
    UIManager ui;

    [SerializeField]
    private int knifeAmmo = 9999999;
    [SerializeField]
    private int handgunAmmo = 9;
    [SerializeField]
    private int maxHandgunAmmo = 99;
    [SerializeField]
    private int shotgunAmmo = 3;
    [SerializeField]
    private int maxShotgunAmmo = 99;
    [SerializeField]
    private int rifleAmmo = 25;
    [SerializeField]
    private int maxRifleAmmo = 250;

    public Dictionary<string, int> tagToAmmo;

    void Awake()
    {
        tagToAmmo = new Dictionary<string, int> {
            {Constants.Knife, knifeAmmo},
            {Constants.Handgun, handgunAmmo},
            {Constants.Shotgun, shotgunAmmo},
            {Constants.Rifle, rifleAmmo},
            {Constants.Unarmed, 0},
        };
    }

    internal bool IsFull(string ammoType)
    {
        bool results = false;

        switch(ammoType)
        {
            case "Handgun":
                if (handgunAmmo > maxHandgunAmmo)
                {
                    results = true;
                }
                break;
            case "Shotgun":
                if (shotgunAmmo > maxShotgunAmmo)
                {
                    results = true;
                }
                break;
            case "Rifle":
                if (rifleAmmo > maxRifleAmmo)
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
        //ui.SetAmmoText(tagToAmmo[tag]);
    }
}
