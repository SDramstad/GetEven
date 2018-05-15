using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : AbstractTakesDamage {

    public int health;
    public int maxHealth;
    //armor is a percentage decrease of all damage taken
    // 0 = unarmored, 1 = light (15%), 2 = medium(25%), 3 = heavy(35%)
    public int armor;
    public UIManager uiManager;
    public Game gameManager;
       // private WeaponManager weaponManager;
    private Ammo ammo;

    internal bool canPickUpHealth()
    {
        if (health < maxHealth)
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

    private float lastDamageTime;

    // Use this for initialization
    void Start () {
        //ammo = GetComponent<Ammo>();
        //weaponManager = GetComponent<WeaponManager>();
        lastDamageTime = Time.time - 10f;
        health = 100;
        maxHealth = 100;
        armor = 0;
        isMale = true;
        ammo = GameObject.Find("AmmoManager").GetComponent<Ammo>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<Game>();
        InvokeRepeating("healthRegen", 1f, 1f);
	}

    public override void TakeDamage(int amount)
    {
        int healthDamage = amount;
        float lastDamageTime = Time.time;


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

        /*
        if (armor > 0)
        {
            int effectiveArmor = armor * 2;
            
            effectiveArmor -= healthDamage;

            //if still armor, dont process health damage
            if (effectiveArmor > 0)
            {
                armor = effectiveArmor / 2;
                gameui.SetArmorText(armor);
                return;
            }

            armor = 0;
            gameui.SetArmorText(armor);
        }
    
        */
        
        health -= healthDamage;
        uiManager.DamageFlash();
        Debug.Log("Health is " + health);

        if (health <= 0)
        {
            //Debug.Log("ROBOTS WIN.");
            GetComponent<AudioSource>().PlayOneShot(playerDead);
            gameManager.GameOver();
            //game.GameOver();
        }
    }

    public void healthRegain(int healthGain)
    {
        health += healthGain;
        normalizeHealth();
    }

    private void normalizeHealth()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
	// Update is called once per frame
	void Update () {
        uiManager.SetHealth(health);	
    }

    void healthRegen()
    {
        //after x seconds of not taking damage, begin regaining health	
        if (((Time.time - lastDamageTime) > 10f) && (health < (maxHealth/2)))
        {
            health += 5;
            Debug.Log("Health is " + health);
        }
    }
    
}
