using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{

    //Weapons
    public const string Knife = "Knife";
    public const string Handgun = "Handgun";
    public const string Shotgun = "Shotgun";
    public const string Unarmed = "Unarmed";
    public const string Rifle = "Rifle";
    public const string Rocketgun = "Rocketgun";
    public const string Frag = "Frag Grenade";

    //Starting ammo
    public const int STARTING_PISTOL_AMMO = 12;
    public const int STARTING_SHOTGUN_AMMO = 6;
    public const int STARTING_SMG_AMMO = 30;
    public const int STARTING_ROCKET_AMMO = 10;

    //Misc
    public const float CameraDefaultZoom = 66f;

    //Powerups
    public enum POWER_UP_NAMES {
        BERSERK,
        INVINCIBILITY,
        RAPIDFIRE
    };

    //Faction
    public enum Faction
    {
        State,
        Player,
        Crime,
        Mutant
    }

    public const float rapidFireTime = 35f;
    public const float invincibilityTime = 35f;

}
