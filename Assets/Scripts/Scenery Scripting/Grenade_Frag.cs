using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Grenade_Frag : A_TakesDamage {

    [SerializeField]
    private int health;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float radius;
    [SerializeField]
    private AudioClip deathSound;
    [SerializeField]
    private AudioClip painSound;
    [SerializeField]
    private ParticleSystem deathGFX;
    [SerializeField]
    private TextMeshPro visibleTimer;
    [SerializeField]
    private GameObject parentObject;

    private GameObject player;
    [SerializeField]
    private int timer = 3;

    private bool isCountingDown;
    private bool isExploding;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        isExploding = false;
        isCountingDown = false;
	}
	
	// Update is called once per frame
	void Update () {
        //check if safe to blow up
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) > 10 && !isCountingDown)
        {
            isCountingDown = true;
            InvokeRepeating("Countdown", 1, 1f);
        }
    }

    private void Countdown()
    {
        timer--;
        visibleTimer.text = timer.ToString();
        if (timer <= 0)
        {
            CancelInvoke();
            explode();
        }
    }

    /// <summary>
    /// Hold the pin on an active grenade.
    /// Prevents grenade from blowing up in hands, which would break the grabber system, and is also neat.
    /// </summary>
    public void StopCountdown()
    {
        isCountingDown = false;
        CancelInvoke();
    }

    public override void TakeDamage(int amount)
    {
        
        health -= amount;
        //Debug.Log("Health is " + health);

        if (health <= 0)
        {

            if (!isExploding)
            {
                explode();
            }

            //Game.RemoveEnemy();
        }
    }

    private void explode()
    {
        CancelInvoke();
        isExploding = true;
        //gets a list of all gameobjects within an overlap sphere, originating at transform's position, going out 10 float units
        Collider[] inRadiusList = Physics.OverlapSphere(transform.position, 10f);

        int i = 0;
        while (i < inRadiusList.Length)
        {
            Rigidbody rb = inRadiusList[i].GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(200.0f, transform.position, radius, 5f);
            }
            if (inRadiusList[i].GetComponent<A_TakesDamage>() != null)
            {
                inRadiusList[i].SendMessage("TakeDamage", damage);
            }

            i++;
        }

        GetComponent<AudioSource>().PlayOneShot(deathSound);
        Instantiate(deathGFX, transform.position, Quaternion.identity);
        Renderer[] childObjects = GetComponentsInChildren<Renderer>();
        foreach (var item in childObjects)
        {
            item.enabled = false;
        }
        visibleTimer.renderer.enabled = false;
        StartCoroutine("DestroyGrenade");
    }

    IEnumerator DestroyGrenade()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
        Destroy(parentObject);
    }

}
