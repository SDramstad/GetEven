using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : A_ThreatCharacter {
    
    public PlayerData localPlayerData = new PlayerData();
    //armor is a percentage decrease of all damage taken
    // 0 = unarmored, 1 = light (15%), 2 = medium(25%), 3 = heavy(35%)
    public int armor;
    public UIManager uiManager;
    public Game gameManager;

    //power ups
    private bool rapidFireActive = false;
    private float lastPickedUpRapidFire = 0.00f;
    private bool invincActive = false;
    private float lastPickedUpInvince = 0.00f;

    internal bool canPickUpHealth()
    {
        if (localPlayerData.hp < localPlayerData.maxhp)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    //public Game game;
    //public Weapon pistol;
    //public Weapon shotgun;
    // public Weapon assaultRifle;
    public bool isMale;
    public AudioClip playerDead;
    public AudioClip pickupSound;
    private GameObject flashlight;


    //sets the time regen can start kicking in
    private float canHealTime;


    // Use this for initialization
    void Start () {
        canHealTime = 0.0f;
        LoadPlayer();

        //debug tetsing
        //Debug.Log("Am i a super boy? Should be a 7: " + localPlayerData.sk_speed);;

        armor = 0;
        isMale = true;
        flashlight = GameObject.Find("F_Flashlight");

        if (flashlight == null)
        {
            Debug.Log("Flashlight object is null?");
        }

        flashlight.SetActive(false);

        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<Game>();
        InvokeRepeating("healthRegen", 1f, 0.2f);
	}
    
    private void LoadPlayer()
    {
        //load in data from globalcontrol
        GetComponent<Ammo>().tagToAmmo[Constants.Handgun] = GlobalControl.Instance.savedPlayerData.pistolAmmo;
        GetComponent<Ammo>().tagToAmmo[Constants.Shotgun] = GlobalControl.Instance.savedPlayerData.shotgunAmmo;
        GetComponent<Ammo>().tagToAmmo[Constants.Rifle] = GlobalControl.Instance.savedPlayerData.rifleAmmo;
        GetComponent<WeaponSwitcher>().hasKnife = GlobalControl.Instance.savedPlayerData.hasKnife;
        GetComponent<WeaponSwitcher>().hasPistol = GlobalControl.Instance.savedPlayerData.hasPistol;
        GetComponent<WeaponSwitcher>().hasShotgun = GlobalControl.Instance.savedPlayerData.hasShotgun;
        GetComponent<WeaponSwitcher>().hasRifle = GlobalControl.Instance.savedPlayerData.hasRifle;
        localPlayerData.hp = GlobalControl.Instance.savedPlayerData.hp;
        localPlayerData.maxhp = GlobalControl.Instance.savedPlayerData.maxhp;
        localPlayerData.difficulty = GlobalControl.Instance.savedPlayerData.difficulty;
        Debug.Log("Speed is currently: " + GetComponent<FirstPersonController>().m_WalkSpeed);
        Debug.Log("Speed in GlobalControl is " + GlobalControl.Instance.savedPlayerData.sk_speed);
        GetComponent<FirstPersonController>().m_JumpSpeed = GlobalControl.Instance.savedPlayerData.sk_jump;
        GetComponent<FirstPersonController>().m_WalkSpeed = GlobalControl.Instance.savedPlayerData.sk_speed;
        GetComponent<FirstPersonController>().m_RunSpeed = GlobalControl.Instance.savedPlayerData.sk_speed * 2;
    }

    public void SavePlayer()
    {
        Debug.Log("Saving player data.");
        GlobalControl.Instance.savedPlayerData.pistolAmmo = GetComponent<Ammo>().tagToAmmo[Constants.Handgun];
        GlobalControl.Instance.savedPlayerData.shotgunAmmo = GetComponent<Ammo>().tagToAmmo[Constants.Shotgun];
        GlobalControl.Instance.savedPlayerData.rifleAmmo = GetComponent<Ammo>().tagToAmmo[Constants.Rifle];
        GlobalControl.Instance.savedPlayerData.hasKnife = GetComponent<WeaponSwitcher>().hasKnife;
        GlobalControl.Instance.savedPlayerData.hasPistol = GetComponent<WeaponSwitcher>().hasPistol;
        GlobalControl.Instance.savedPlayerData.hasShotgun = GetComponent<WeaponSwitcher>().hasShotgun;
        GlobalControl.Instance.savedPlayerData.hasRifle = GetComponent<WeaponSwitcher>().hasRifle;        
        GlobalControl.Instance.savedPlayerData.hp = localPlayerData.hp;
        GlobalControl.Instance.savedPlayerData.maxhp = localPlayerData.maxhp;
        GlobalControl.Instance.savedPlayerData.difficulty = localPlayerData.difficulty;
        GlobalControl.Instance.savedPlayerData.sk_jump = GetComponent<FirstPersonController>().m_JumpSpeed;
        GlobalControl.Instance.savedPlayerData.sk_speed = GetComponent<FirstPersonController>().m_WalkSpeed;
    }

    public void OverChargePlayer()
    {
        Debug.Log("Maxxing out character.");
        //GetComponent<Ammo>().tagToAmmo[Constants.Handgun] = 255;
        //GetComponent<Ammo>().tagToAmmo[Constants.Shotgun] = 255;
        //GetComponent<Ammo>().tagToAmmo[Constants.Rifle] = 2550;
        GetComponent<WeaponSwitcher>().hasKnife = true;
        GetComponent<WeaponSwitcher>().hasPistol = true;
        GetComponent<WeaponSwitcher>().hasShotgun = true;
        GetComponent<WeaponSwitcher>().hasRifle = true;
        localPlayerData.hp = 200;
        localPlayerData.maxhp = 200;
        localPlayerData.difficulty = 1;
        Debug.Log("Speed is currently: " + GetComponent<FirstPersonController>().m_WalkSpeed);
        Debug.Log("Speed in GlobalControl is " + GlobalControl.Instance.savedPlayerData.sk_speed);
        GetComponent<FirstPersonController>().m_JumpSpeed = GlobalControl.Instance.savedPlayerData.sk_jump;
        GetComponent<FirstPersonController>().m_WalkSpeed = GlobalControl.Instance.savedPlayerData.sk_speed * 2;
        GetComponent<FirstPersonController>().m_RunSpeed = GlobalControl.Instance.savedPlayerData.sk_speed * 4;

    }

    public override void TakeDamage(int amount)
    {
        int healthDamage = amount;


        //regen starts up in 5 seconds of not taking damage
        canHealTime = Time.time + 5f;

        switch (armor) {
            case 1:
                healthDamage = (int)System.Math.Round(amount * 0.85m);
                break;
            case 2:
                healthDamage = (int)System.Math.Round(amount * 0.75m);
                break;
            case 3:
                healthDamage = (int)System.Math.Round(amount * 0.65m);
                break;
            default:
                break;
        }

        //difficulty damage adjustments
        //Debug.Log("Health DMG before diff mod: " + healthDamage);
        float tempHealthDamage = (float)healthDamage;
        tempHealthDamage *= localPlayerData.GetDifficulty_PlayerDamageMod();
        healthDamage = (int)System.Math.Round(tempHealthDamage);
        //Debug.Log("Health DMG after diff mod: " + healthDamage);

        if (invincActive)
        {
            //immune to damage
        }
        else
        {
            localPlayerData.hp -= healthDamage;
            uiManager.DamageFlash();

        }
        
        //Debug.Log("Health is " + localPlayerData.hp + " and global health is " + GlobalControl.Instance.savedPlayerData.hp);

        if (localPlayerData.hp <= 0)
        {
            GetComponent<AudioSource>().PlayOneShot(playerDead);
            gameManager.GameOver();
        }
    }

    public void healthRegain(int healthGain)
    {
        localPlayerData.hp += healthGain;
        normalizeHealth();
    }

    private void normalizeHealth()
    {
        if (localPlayerData.hp > localPlayerData.maxhp)
        {
            localPlayerData.hp = localPlayerData.maxhp;
        }   
    }
	// Update is called once per frame
	void Update () {
        uiManager.SetHealth(localPlayerData.hp);
        powerUpTimeCheck();
        if (Input.GetButtonDown("Flashlight"))
        {
            if (flashlight.activeSelf)
            {
                flashlight.SetActive(false);
            }
            else
            {
                flashlight.SetActive(true);
            }
        }

        
    }

    void healthRegen()
    {
        //Is invokved from the START on an INVOKEREPEATING.
        //Every second, this check runs. If Health is less than half the MAXHP, AND the time since last damage was taken is greater than 5 seconds
        if ( ((Time.time > canHealTime)) && (localPlayerData.hp < (localPlayerData.maxhp/ 2)) )
        {
            localPlayerData.hp += 1;
            //Debug.Log("Health is " + localPlayerData.hp);
        }
    }

    public void pickupInvincibility()
    {
        Debug.Log("Picked up Invincibility.");
        lastPickedUpInvince = Time.time;
        Invincibility();
    }

    private void Invincibility()
    {
        localPlayerData.hp = 666;
        invincActive = true;

    }

    private void DisableInvincibility()
    {
        localPlayerData.hp = localPlayerData.maxhp;
        invincActive = false;
    }

    public void pickupRapidFire()
    {
        Debug.Log("Picked up Rapid Fire. Time is " + Time.time + " and the time this power up should be over is " + (Time.time + Constants.rapidFireTime) + ".");
        lastPickedUpRapidFire = Time.time;
        rapidFireActive = true;
        GetComponent<WeaponSwitcher>().RapidFire();
    }

    private void powerUpTimeCheck()
    {
        //rapidfire
        if (Time.time >= lastPickedUpRapidFire + Constants.rapidFireTime && rapidFireActive)
        {
            //disable if wears off
            GetComponent<WeaponSwitcher>().EndRapidFire();
            rapidFireActive = false;
        }

        //invince
        if (Time.time >= lastPickedUpInvince + Constants.invincibilityTime && invincActive)
        {
            Debug.Log("Turning off Invince.");
            DisableInvincibility();
        }
        
    }
}
