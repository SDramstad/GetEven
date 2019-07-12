using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HelicopterBoss : A_TakesDamage
{

    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    GameObject missilePrefab;
    [SerializeField]
    private AudioClip deathSound;
    [SerializeField]
    private AudioClip fireSound;
    [SerializeField]
    private AudioClip fireMissileSound;
    [SerializeField]
    private AudioClip painSound;
    [SerializeField]
    public AudioClip flightsound;

    public int health;

    //public int range;
    public int fireRange;

    public float fireRate;
    public ParticleSystem deathGFX;
    //public ParticleSystem painGFX;
    public ParticleSystem muzzleFlash;


    public int damage;

    public Transform weapon;
    NavMeshAgent agent;

    private Transform player;
    private GameObject target;
    private float timeLastFired;

    private bool fireMode;
    //fireMode false means machinegun fire.
    //firemode true means missiles.


    private Vector3 _leftSway;
    private Vector3 _rightSway;
    private bool hasMoved;

    private bool isDead;
    private bool playerIsInSight;

    /// <summary>
    /// GOAL:
    /// Set up two types of attack.
    /// Missiles and Bullets.
    /// Bullets use burst-fire implementation, and use standard soldier bullet mechanics.
    /// Missiles use explosive barrel rules.
    /// 
    /// </summary>

    // Use this for initialization
    void Start()
    {
        isDead = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _leftSway = transform.position;
        _rightSway = new Vector3(transform.position.x + 50, transform.position.y, transform.position.z);
        fireMode = true;
        hasMoved = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if dead, stop this
        if (isDead)
        {
            return;
        }

        //sway back and forth
        if (!hasMoved)
        {
            MoveLeft();
        }
        else
        {
            MoveRight();
        }

        if (Vector3.Distance(transform.position, player.position) < 100 && !playerIsInSight)
        {
            playerIsInSight = true;
        }

        if (playerIsInSight)
        {
            transform.LookAt(player);
        }
        //fire in bursts, every x seconds
        if (((Time.time - timeLastFired) > fireRate) && playerIsInSight)
        {
            StartCoroutine("BeginBurstFire");
        }
    }

    IEnumerator BeginBurstFire()
    {
        //declaring the timeLastFired twice here is done to keep the burst from repeatedly firing.
        //Hopefully this should work.
        timeLastFired = Time.time;

        if (fireMode)
        {
            //fire the missile
            fire();
            GetComponent<AudioSource>().PlayOneShot(fireMissileSound);
            timeLastFired = Time.time;
            fireMode = !fireMode;
        }
        else
        {
            //fire the burst
            int burstFireNumber = 12;
            GetComponent<AudioSource>().PlayOneShot(fireSound);
            while (burstFireNumber > 0)
            {
                fire();
                yield return new WaitForSeconds(0.1f);
                burstFireNumber--;
            }
            timeLastFired = Time.time;
            fireMode = !fireMode;
        }
    }

    private void MoveLeft()
    {
        transform.position = Vector3.MoveTowards(transform.position, _leftSway, 10f * Time.deltaTime);

        //Debug.Log("We moving.");
        //if opened, end movement
        if (Vector3.Distance(transform.position, _leftSway) < 0.1f)
        {
            hasMoved = true;
        }
    }

    private void MoveRight()
    {
        transform.position = Vector3.MoveTowards(transform.position, _rightSway, 10f * Time.deltaTime);

        //Debug.Log("We moving.");
        //if opened, end movement
        if (Vector3.Distance(transform.position, _rightSway) < 0.1f)
        {
            hasMoved = false;
        }
    }

    void fire()
    {
        target = GameObject.Find("Player");

        Vector3 direction = target.transform.position - transform.position;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        Debug.DrawRay(transform.position, direction, Color.red);

        if (Physics.Raycast(ray, out hit, fireRange))
        {
            weapon.LookAt(hit.transform);
            if (fireMode)
            { 
                Instantiate(missilePrefab, weapon.transform.position, weapon.transform.rotation);
            }
            else
            {
                Instantiate(bulletPrefab, weapon.transform.position, weapon.transform.rotation);
            }

            ParticleSystem _tempParticleSystem = Instantiate(muzzleFlash, weapon.transform.position, Quaternion.identity);
            Destroy(_tempParticleSystem, 5f);
        }

        //var range = player.transform.position - enemy.transform.position;
        //RaycastHit hit;
        //if (Physics.Raycast(enemy.transform.position, range, out hit))
        //{
        //    Debug.DrawRay(enemy.transform.position, player.transform.position, Color.black, 5f);


    }





    public override void TakeDamage(int amount)
    {
        if (isDead)
        {
            return;
        }

        health -= amount;

        if (health <= 0)
        {
            isDead = true;

            //Special handling for waves. Should be made more generalized.
            if (GameObject.Find("WaveManager"))
            {
                Debug.Log("Killed another enemy.");
                GameObject.Find("WaveManager").GetComponent<WaveManager>().enemyTakenDown();
            }

            GetComponent<AudioSource>().PlayOneShot(deathSound);
            Instantiate(deathGFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(painSound);
        }
    }

}
