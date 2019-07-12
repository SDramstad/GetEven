using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : A_TakesDamage {

    [SerializeField]
    protected GameObject missilePrefab;
    [SerializeField]
    protected AudioClip deathSound;
    [SerializeField]
    protected AudioClip fireSound;
    [SerializeField]
    protected AudioClip painSound;
    
    public int health;    
    public int range;
    public int fireRange;

    public Animator soldierAnimations;

    public float fireRate;
    public ParticleSystem deathGFX;
    //public ParticleSystem painGFX;
    public ParticleSystem muzzleFlash;

    public int damage;

    public Transform weapon;
    protected NavMeshAgent agent;

    protected Transform player;
    protected GameObject target;
    protected float timeLastFired;

    protected bool isDead;
    
    /// <summary>
    /// Which AI Set they currently have activated.
    /// 0 = Cautious Ranged
    /// 1 = Rushdown Ranged
    /// </summary>
    protected int aiset;

    /// <summary>
    /// Cooldown occurs after shooting, soldiers wait around while this is on
    /// </summary>
    protected bool isInCooldown = false;
    protected bool cooldownTimerRunning = false;

    //if has been attacked, should chase player forever
    protected bool hasBeenAttacked;

    //TODO: Stances, could move these to something more publically usable to create a consistent enemy framework
    protected enum Stances
    {
        Unaware,
        Firing,
        Waiting,
        Running,
        Standing,
        Dead
    }

    //Current Stance
    protected Stances _stance;

    //if is alerted to you
    //todo: add implementation
    public bool alerted;

    // Use this for initialization
    void Start () {
        isDead = false;
        hasBeenAttacked = false;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        _stance = Stances.Unaware;
        ///Randomly set which AI Set this soldier was set up with
        aiset = UnityEngine.Random.Range(0, 0);

	}

    // Calls out to all nearby friends and gets them to attack the player.'
    // Additionally, sets the enemy as able to attack the player.
    public void Alert()
    {
        Collider[] inRadiusList = Physics.OverlapSphere(transform.position, 20f);
                
        foreach (var gameObject in inRadiusList)
        {
            if (gameObject.GetComponent<Soldier>() != null)
            {
                if (gameObject.GetComponent<Soldier>().alerted == false)
                {
                    gameObject.GetComponent<Soldier>().alerted = true;
                }
            }
        }
        
        alerted = true;
    }

    public bool isAlerted()
    {
        return alerted;
    }
	
	// Update is called once per frame
	protected virtual void Update ()
    {
        if (isDead)
        {
            return;
        }

        if (aiset == 0)
        {
            CautiousRangedAISet();
        }
        else if (aiset == 1)
        {
            //should never reach, rushdown does not work yet
            RushdownAISet();
        }

    }

    private void RushdownAISet()
    {
        //if player in range, fire
        if (Vector3.Distance(transform.position, player.position) < fireRange - 5 && Time.time - timeLastFired > fireRate && alerted)
        {
            //6
            _stance = Stances.Firing;
            agent.isStopped = true;
            timeLastFired = Time.time;
            fire();
        }
        //if the player is close enough, hunt for them, OR if has been attacked and the two of you are not on top of each other
        else if ((Vector3.Distance(transform.position, player.position) < range && Vector3.Distance(transform.position, player.position) > 3) || (alerted && Vector3.Distance(transform.position, player.position) > 3))
        {
            transform.LookAt(player);
            //alerted = true;
            Alert();
            soldierAnimations.Play("Run");
            _stance = Stances.Running;
            agent.SetDestination(player.position);
        }

        //
        else if (Vector3.Distance(transform.position, player.position) <= 3)
        {
            soldierAnimations.Play("Idle");
            agent.isStopped = true;
            StartCoroutine(RestartRunning());
        }
    }

    private void CautiousRangedAISet()
    {
        //if in range, alerted, and not cooling down, fire gun
        if (Vector3.Distance(transform.position, player.position) < fireRange - 10 && !isInCooldown && alerted)
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

    protected IEnumerator SoldierCooldown(float fireRate)
    {
        cooldownTimerRunning = true;
        yield return new WaitForSeconds(fireRate);
        isInCooldown = false;
        agent.isStopped = false;
        cooldownTimerRunning = false;
    }

    protected virtual void fire()
    {
        target = GameObject.Find("Player");

        Vector3 direction = target.transform.position - transform.position;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        Debug.DrawRay(transform.position, direction, Color.red);

        if (Physics.Raycast(ray, out hit, fireRange))
        {
            weapon.LookAt(hit.transform);
            Instantiate(missilePrefab, weapon.transform.position, weapon.transform.rotation);          

            ParticleSystem _tempParticleSystem = Instantiate(muzzleFlash, weapon.transform.position, Quaternion.identity);
            Destroy(_tempParticleSystem, 3f);
            soldierAnimations.Play("Fire");
            GetComponent<AudioSource>().PlayOneShot(fireSound);
        }

        isInCooldown = true;

    }

    protected IEnumerator RestartRunning(float waitTime = 1f)
    {
        yield return new WaitForSeconds(waitTime);
        agent.isStopped = false;
    }

    protected void processHit(GameObject hitObject, RaycastHit location)
    {
        Debug.Log(hitObject.name + " was hit.");

        //this section should be using a fucking interface but i need to look in that more

        if (hitObject.GetComponent<A_TakesDamage>() != null)
        {
            hitObject.GetComponent<A_TakesDamage>().TakeDamage(damage);
            ParticleSystem _tempParticleSystem = Instantiate(hitObject.GetComponent<A_TakesDamage>().painGFX, location.point, Quaternion.identity);
            Destroy(_tempParticleSystem, 2f);
        }
        /*
        if (hitObject.GetComponent<Enemy>() != null)
        {
            hitObject.GetComponent<Enemy>().TakeDamage(damage);
            Instantiate(hitObject.GetComponent<Enemy>().painGFX, location.point, Quaternion.identity);
        }

        if (hitObject.GetComponent<Player>() != null)
        {
            hitObject.GetComponent<Player>().TakeDamage(damage);
        }

        if (hitObject.GetComponent<Civilian>() != null)
        {
            hitObject.GetComponent<Civilian>().TakeDamage();
            //Instantiate(hitObject.GetComponent<Civilian>().painGFX, location.point, Quaternion.identity);
        }

        if (hitObject.GetComponent<ExplosiveBarrel>() != null)
        {
            Debug.Log("Explosive barrel got hit.");
            hitObject.GetComponent<ExplosiveBarrel>().TakeDamage(damage);
        }
        */
    }

    public override void TakeDamage(int amount)
    {
        Alert();

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
                //Debug.Log("Killed another enemy.");
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
