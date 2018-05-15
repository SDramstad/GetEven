using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : AbstractTakesDamage {

    [SerializeField]
    private int health;
    [SerializeField]
    private int damage;
    [SerializeField]
    private AudioClip deathSound;
    [SerializeField]
    private AudioClip painSound;
    [SerializeField]
    private ParticleSystem deathGFX;

    

	// Use this for initialization
	void Start () {
        List<GameObject> inRadiusList = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void TakeDamage(int amount)
    {
        
        health -= amount;
        //Debug.Log("Health is " + health);

        if (health <= 0)
        {
            Debug.Log("This should be blowing up.");
            explode();

            //Game.RemoveEnemy();
        }
    }

    private void explode()
    {
        Collider[] inRadiusList = Physics.OverlapSphere(transform.position, 10f);

        int i = 0;
        while (i < inRadiusList.Length)
        {
            //Debug.Log("why");
            //GetComponent<Rigidbody>().AddExplosionForce(10f, transform.position, 5f);
            //Debug.Log("Barrel explosion hit: " + inRadiusList[i].name);
            if (!inRadiusList[i].GetComponent<ExplosiveBarrel>())
            {
                /*if(inRadiusList[i].GetComponent<Enemy>())
                {
                    inRadiusList[i].GetComponent<Enemy>().TakeDamage(damage);
                }

                if (inRadiusList[i].GetComponent<Player>())
                {
                    inRadiusList[i].GetComponent<Player>().TakeDamage(10);
                }*/

                //if (inRadiusList[i].GetComponent<Rigidbody>())
                //{
                //    inRadiusList[i].GetComponent<Rigidbody>().AddExplosionForce(10f, transform.position, 5f);
                //}

                Rigidbody rb = inRadiusList[i].GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(500.0f, transform.position, 15f, 5f);
                }
                inRadiusList[i].SendMessage("TakeDamage", damage);
            }
            i++;
        }
        //Debug.Log("workedd");

        GetComponent<AudioSource>().PlayOneShot(deathSound);
        Instantiate(deathGFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
