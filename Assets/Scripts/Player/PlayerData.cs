using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData {

    public int hp;
    public int maxhp;    
    public int pistolAmmo;
    public int shotgunAmmo;
    public int rifleAmmo;
    public int rocketAmmo;
    public bool hasKnife;
    public bool hasPistol;
    public bool hasShotgun;
    public bool hasRifle;
    public bool hasRocketgun;

    //difficulty, 1 easy, 2 normal, 3 hard, 4 nightmare
    public int difficulty;

    /*
     * THINGS THAT MAY NOT COME TO PASS
     * 
     * Upgrade system rules here. 
     * 
     */
    public int maxPistolAmmo;
    public int maxShotgunAmmo;
    public int maxRifleAmmo;
    public int maxRocketAmmo;

    //The amount of money currently held in the wallet.
    public int cash;

    //The amount of loot value you've got right now. Needs to be turned in to turn it into cash.
    public int lootValue;
    //Cash held in bank, for a system where cash in wallet is lost in a cash penalty on death.
    //public int bankCash;

    //Weapon upgrades
    /*
    public int pistolUpgrade;
    public int shotgunUpgrade;
    public int rifleUpgrade;
    */


    //SKILLS

    /// <summary>
    /// SKILLS
    /// 
    /// Each skill has a different value multiplier, meaning they cost more to level up. ( Rank x ValueMulti )
    /// 
    /// Ransacking, 500 : Increases quality of loot found, affects the random loot tables by knocking it up a notch.
    /// Ammunition, 200 : Increases amount of ammo found.
    /// Hacking, 250 : Lets you interact with hacking terminals to achieve different effects. Change allegiance of mechanical enemies, open doors, 
    /// Medical, 250 : Increase health gained from pickups.
    /// Speed, 500 : Increases speed
    /// //This is used for the character controller's base speed. It is doubled for sprint speed. Crouch speed should be halved.
    /// Jump, 250 : Increase jump height.
    /// 
    /// </summary>
    //public int sk_ransacking;
    //public int sk_ammo;
    //public int sk_hacking;
    // public int sk_medical;
    public float sk_speed;
    public float sk_jump;


    public PlayerData(bool fullyArmed = false)
    {
        if (fullyArmed)
        {
            hp = 125;
            maxhp = 125;
            pistolAmmo = 100;
            shotgunAmmo = 100;
            rifleAmmo = 255;
            rocketAmmo = 80;
            hasKnife = true;
            hasPistol = true;
            hasShotgun = true;
            hasRifle = true;
            hasRocketgun = true;
            cash = 500;
            lootValue = 500;
            difficulty = 2;
            sk_speed = 7;
            sk_jump = 10.5f;
        }
        else
        {

            hp = 100;
            maxhp = 100;
            pistolAmmo = Constants.STARTING_PISTOL_AMMO;
            shotgunAmmo = Constants.STARTING_SHOTGUN_AMMO;
            rifleAmmo = Constants.STARTING_SMG_AMMO;
            rocketAmmo = Constants.STARTING_ROCKET_AMMO;
            hasKnife = false;
            hasPistol = false;
            hasShotgun = false;
            hasRifle = false;
            hasRocketgun = false;
            cash = 0;
            lootValue = 0;
            difficulty = 2;
            sk_speed = 5;
            sk_jump = 10;
        }
    }

    //public PlayerData(bool fullyArmed)
    //{
    //    if (fullyArmed)
    //    {
    //        hp = 125;
    //        maxhp = 125;
    //        pistolAmmo = 45;
    //        shotgunAmmo = 22;
    //        rifleAmmo = 90;
    //        rocketAmmo = 35;
    //        hasKnife = true;
    //        hasPistol = true;
    //        hasShotgun = true;
    //        hasRifle = true;
    //        hasRocketgun = true;
    //        cash = 500;
    //        lootValue = 500;
    //        difficulty = 2;
    //        sk_speed = 7;
    //        sk_jump = 10.5f;
    //    }
    //    else
    //    {
    //        //exact copy of base player data
    //        hp = 100;
    //        maxhp = 100;
    //        pistolAmmo = Constants.STARTING_PISTOL_AMMO;
    //        shotgunAmmo = Constants.STARTING_SHOTGUN_AMMO;
    //        rifleAmmo = Constants.STARTING_SMG_AMMO;
    //        rocketAmmo = Constants.STARTING_ROCKET_AMMO;
    //        hasKnife = false;
    //        hasPistol = false;
    //        hasShotgun = false;
    //        hasRifle = false;
    //        hasRocketgun = false;
    //        cash = 0;
    //        lootValue = 0;
    //        difficulty = 2;
    //        sk_speed = 5;
    //        sk_jump = 10;
    //    }
    //}

    public PlayerData(PlayerData clone)
    {
        hp = clone.hp;
        maxhp = clone.maxhp;
        pistolAmmo = clone.pistolAmmo;
        shotgunAmmo = clone.shotgunAmmo;
        rifleAmmo = clone.rifleAmmo;
        rocketAmmo = clone.rocketAmmo;
        hasKnife = clone.hasKnife;
        hasPistol = clone.hasPistol;
        hasShotgun = clone.hasShotgun;
        hasRifle = clone.hasRifle;
        hasRocketgun = clone.hasRocketgun;
        cash = clone.cash;
        lootValue = clone.lootValue;
        sk_speed = clone.sk_speed;
        sk_jump = clone.sk_jump;
    }

    public float GetDifficulty_ProjectileSpeedMod()
    {
        float speedMod = 1f;
        switch (difficulty)
        {
            case 1:
                speedMod = 0.9f;
                break;
            case 2:
                speedMod = 1f;
                break;
            case 3:
                speedMod = 1.2f;
                break;
            case 4:
                speedMod = 1.5f;
                break;
            default:
                break;
        }
        return speedMod;
    }

    public float GetDifficulty_PlayerDamageMod()
    {
        float damageMod = 1f;
        switch (difficulty)
        {
            case 1:
                damageMod = 0.9f;
                break;
            case 2:
                damageMod = 1f;
                break;
            case 3:
                damageMod = 1.1f;
                break;
            case 4:
                damageMod = 1.5f;
                break;
            default:
                break;
        }
        return damageMod;
    }

}
