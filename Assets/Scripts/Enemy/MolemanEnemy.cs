using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MolemanEnemy : A_TakesDamage {

    //[SerializeField]
    //Projectile missilePrefab;
    [SerializeField]
    private AudioClip deathSound;
    [SerializeField]
    private AudioClip fireSound;
    [SerializeField]
    private AudioClip painSound;
    
    public int health;
    
    public int range;
    public int fireRange;

    public Animator molemanAnimations;

    public float fireRate;
    public ParticleSystem deathGFX;
    //public new ParticleSystem painGFX;
    public ParticleSystem muzzleFlash;

    public int damage;

    public Transform weapon;
    NavMeshAgent agent;
    
    private Transform player;
    private GameObject target;
    private float timeLastFired;

    private bool isDead;
    //if has been attacked, should chase player forever
    private bool hasBeenAttacked;

    //if is alerted to you
    //todo: add implementation
    private bool alerted;

    // Use this for initialization
    void Start () {
        isDead = false;
        hasBeenAttacked = false;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (isDead)
        {
            return;
        }
               
        //if the player is close enough, hunt for them, OR if has been attacked and the two of you are not on top of each other
        if ((Vector3.Distance(transform.position, player.position) < range && Vector3.Distance(transform.position, player.position) > 3) || (alerted && Vector3.Distance(transform.position, player.position) > 3))
        {
            agent.isStopped = false;
            transform.LookAt(player);
            alerted = true;
            molemanAnimations.Play("Run");
            agent.SetDestination(player.position);
        }

        if (Vector3.Distance(transform.position, player.position) < fireRange && Time.time - timeLastFired > fireRate && alerted)
        {
            timeLastFired = Time.time;
            fire();
        }
    }

    void fire()
    {
        target = GameObject.Find("Player");
        agent.isStopped = true;

        Vector3 direction = target.transform.position - transform.position;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;


        Debug.DrawRay(transform.position, direction, Color.red);

        if (Physics.Raycast(ray, out hit, fireRange))
        {
            processHit(hit.collider.gameObject, hit);
        }

        molemanAnimations.Play("Attack");
        GetComponent<AudioSource>().PlayOneShot(fireSound);


    }

    private void processHit(GameObject hitObject, RaycastHit location)
    {
        Debug.Log(hitObject.name + " was hit.");

        //this section should be using a fucking interface but i need to look in that more

        if (hitObject.GetComponent<A_TakesDamage>() != null)
        {
            hitObject.GetComponent<A_TakesDamage>().TakeDamage(damage);
            ParticleSystem _tempParticleSystem = Instantiate(hitObject.GetComponent<A_TakesDamage>().painGFX, location.point, Quaternion.identity);
            Destroy(_tempParticleSystem, 2f);
        }

    }

    public override void TakeDamage(int amount)
    {
        alerted = true;

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

    //IEnumerator Death()
    //{
    //    ////roblox.enabled = false;
    //    //Instantiate(deathGFX, transform.position, Quaternion.identity);
    //    //yield return new WaitForSeconds(1.5f);
    //    //Destroy(gameObject);
    //}
}
