﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class Projectile : MonoBehaviour {

    public float speed;
    public int damage;
    public GameObject hitFX;
    //everytime the bullet gets this far from start, damage is lowered.
    public float rangeIncrements;

    //who fired this projectile? should be used to make sure people can't hurt their own team
    public Faction factionOwner;

    //public GameObject target;

	// Use this for initialization
	void Start () {
        StartCoroutine("DeathTimer");
        //Debug.Log("Base speed is " + speed);
        speed *= GameObject.Find("Player").GetComponent<Player>().localPlayerData.GetDifficulty_ProjectileSpeedMod();
        //Debug.Log("Modified by difficulty speed is " + speed);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * Time.deltaTime * speed;
    }


    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        //dont stop if hitting a trigger
        if (collider.isTrigger == false)
        {
            if (collider.gameObject.GetComponent<Projectile>() != null)
            {
                Debug.Log("Projectile hit projectile. Time : " + Time.time);
            }
            if (collider.gameObject.GetComponent<A_TakesDamage>() != null)
            {

                if (collider.gameObject.GetComponent<A_ThreatCharacter>().GetFaction() == factionOwner)
                {
                    Debug.Log("A friendly fire 'accident' has occurred. No damage has been dealt.");
                }
                else
                {
                    collider.gameObject.GetComponent<A_TakesDamage>().TakeDamage(damage);
                }
            }

            if (hitFX != null)
            {
                GameObject _tempParticleSystem = Instantiate(hitFX, transform.position, transform.rotation);
                Destroy(_tempParticleSystem, 2f);
            }

            Destroy(gameObject);
        }
        

    }

    //void OnCollisionEnter(Collision collider)
    //{
    //    if (collider.gameObject.GetComponent<Player>() != null && collider.gameObject.tag == "Player")
    //    {
    //        Debug.Log("Hit player.");
    //        collider.gameObject.GetComponent<Player>().TakeDamage(damage);
    //    }
    //    if (collider.gameObject.GetComponent<Enemy>() != null)
    //    {
    //        //collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
    //        //Instantiate(collider.gameObject.GetComponent<Enemy>().painGFX, location.point, Quaternion.identity);
    //    }
    //    if (collider.gameObject.GetComponent<Civilian>() != null)
    //    {
    //        collider.gameObject.GetComponent<Civilian>().PlayOuch();
    //        //Instantiate(collider.gameObject.GetComponent<Enemy>().painGFX, location.point, Quaternion.identity);
    //    }

    //    Destroy(gameObject);
    //}
}
