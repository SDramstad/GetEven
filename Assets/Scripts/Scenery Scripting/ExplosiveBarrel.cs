using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : A_TakesDamage {

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

    private bool isExploding;

	// Use this for initialization
	void Start () {
        List<GameObject> inRadiusList = new List<GameObject>();
        isExploding = false;
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
            if (!isExploding)
            {
                explode();
            }

            //Game.RemoveEnemy();
        }
    }

    private void explode()
    {
        isExploding = true;
        //gets a list of all gameobjects within an overlap sphere, originating at transform's position, going out 10 float units
        Collider[] inRadiusList = Physics.OverlapSphere(transform.position, 10f);

        int i = 0;
        while (i < inRadiusList.Length)
        {
            Rigidbody rb = inRadiusList[i].GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(500.0f, transform.position, 15f, 5f);
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
        GetComponent<MeshCollider>().enabled = false;
        StartCoroutine("DestroyBarrel");
    }

    IEnumerator DestroyBarrel()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }

}
