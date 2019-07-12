using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CO_SpawnCloset : A_TakesDamage
{
    private int hp;
    [SerializeField]
    private int MAX_HP;
    [SerializeField]
    private int MAX_SPAWNED;
    [SerializeField]
    private GameObject lightDamageStuff;
    [SerializeField]
    private GameObject heavyDamageStuff;
    [SerializeField]
    private GameObject destroyedStuff;
    [SerializeField]
    private AudioClip deathSound;
    [SerializeField]
    private GameObject deathGFX;
    [SerializeField]
    private GameObject spawnBoyPrefab;
    [SerializeField]
    private GameObject spawnPosition;
    private bool activeCloset;

    private GameObject[] spawnedActorPointers = new GameObject[10];

    private enum damageStates
    {
        DESTROYED,
        HEAVYDAMAGE,
        LIGHTDAMAGE,
        UNDAMAGED
    }

    private damageStates currentState;

    public override void TakeDamage(int damage)
    {
        hp -= damage;
        UpdateDamageState();
    }

    public void StartCloset()
    {
        activeCloset = true;
    }

    private void UpdateDamageState()
    {
        if (hp == MAX_HP)
        {
            //this should never run
            Debug.Log("This should never have run unless you did 0 damage.");
            currentState = damageStates.UNDAMAGED;
        }
        else if (hp > (MAX_HP * .33))
        {
            lightDamageStuff.SetActive(true);
            currentState = damageStates.LIGHTDAMAGE;
        }
        else if (hp > 0)
        {
            lightDamageStuff.SetActive(false);
            heavyDamageStuff.SetActive(true);
            currentState = damageStates.HEAVYDAMAGE;
        }
        else if (hp <= 0)
        {
            heavyDamageStuff.SetActive(false);
            destroyedStuff.SetActive(true);
            currentState = damageStates.DESTROYED;
            StartCoroutine(Die());
        }
    }

    void Start()
    {
        hp = MAX_HP;
        activeCloset = false;
        InvokeRepeating("SpawnBoy", 0f, 15f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnBoy()
    {
        if (activeCloset && MAX_SPAWNED != 0)
        {
            Instantiate(spawnBoyPrefab, spawnPosition.transform.position, Quaternion.identity);
            MAX_SPAWNED--;
        }

    }

    IEnumerator Die()
    {
        GetComponent<AudioSource>().PlayOneShot(deathSound);
        Instantiate(deathGFX, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
