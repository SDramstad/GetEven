using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrenadeSoldier : Soldier {

    private bool isSpooked;

    protected override void Update()
    {
        target = GameObject.Find("Player");

        if (isDead)
        {
            return;
        }

        //while spooked, he has to wait till he calms down to follow standard AI again
        if(isSpooked)
        {
            return;
        }

        //if too close to player to fire safely
        if (Vector3.Distance(target.transform.position, transform.position) < 10)
        {
            //start a cooroutine that has the grenade man flee for a few seconds
            if (!isSpooked)
            {
                StartCoroutine(Spooked());
            }
        }
        //if in range, alerted, and not cooling down, fire gun
        else if (Vector3.Distance(transform.position, player.position) < fireRange && !isInCooldown && alerted)
        {
            transform.LookAt(player);
            fire();
        }
        else if (isInCooldown)
        {
            //if in cooldown after firing, stop moving, and start the cooldown timer
            agent.isStopped = true;
            soldierAnimations.Play("Idle");
            if (!cooldownTimerRunning)
            {
                StartCoroutine(SoldierCooldown(fireRate));
            }
        }
        else if (Vector3.Distance(transform.position, player.position) <= 3)
        {
            //IF ON TOP OF PLAYER
            agent.isStopped = true;
            soldierAnimations.Play("Idle");
        }
        else if ((Vector3.Distance(transform.position, player.position) < range &&
            Vector3.Distance(transform.position, player.position) > 3) ||
            (alerted && Vector3.Distance(transform.position, player.position) > 3))
        {

            //if aware, start running towards them
            transform.LookAt(player);
            Alert();
            soldierAnimations.Play("Run");
            agent.SetDestination(player.position);
        }
    }

    protected override void fire()
    {

        StartCoroutine(BurstFire());
        isInCooldown = true;
    }

    IEnumerator Spooked()
    {
        isSpooked = true;

        //save normal speed
        float baseSpeed = agent.speed;
        float baseAccel = agent.acceleration;

        //triple speed to increase ability to get away
        agent.speed = baseSpeed * 3;
        agent.acceleration = baseAccel*3;

        //make sure we're able to run
        agent.isStopped = false;
        soldierAnimations.Play("Run");

        //distance to run
        int distance = 80; 

        //pick random direction
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
        randomDirection += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, -1);

        //run towards randomized position
        agent.SetDestination(navHit.position);

        //run until you arrive
        yield return new WaitUntil(() => agent.remainingDistance <= 2);

        //after we get there seconds, reset speed and stop being scared
        agent.speed = baseSpeed;
        agent.acceleration = baseAccel;
        isSpooked = false;
    }

    void fireSingleNade()
    {

        Vector3 direction = target.transform.position - transform.position;
        
        GameObject spawnedFrag = Instantiate(missilePrefab);
        spawnedFrag.transform.position = weapon.position;
        spawnedFrag.GetComponentInChildren<Rigidbody>().AddForce(direction, ForceMode.Impulse);

    }

    IEnumerator BurstFire()
    {
        int burstFire = 3;
        while (burstFire > 0)
        {
            fireSingleNade();
            yield return new WaitForSeconds(0.5f);
            burstFire--;
        }
    }
    
}
