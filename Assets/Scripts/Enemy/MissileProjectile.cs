using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileProjectile : MonoBehaviour
{
    public int damage;
    public int speed;
    [SerializeField]
    private AudioClip deathSound;
    [SerializeField]
    private ParticleSystem deathGFX;

    // Use this for initialization
    void Start()
    {
        StartCoroutine("DeathTimer");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    IEnumerator DeathTimer2()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        explode();
        StartCoroutine("DeathTimer2");

    }
    private void explode()
    {
        Collider[] inRadiusList = Physics.OverlapSphere(transform.position, 10f);

        int i = 0;
        while (i < inRadiusList.Length)
        {
            Rigidbody rb = inRadiusList[i].GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(500.0f, transform.position, 15f, 5f);
            }
            
            if (inRadiusList[i].GetComponent<AbstractTakesDamage>())
            {
                inRadiusList[i].GetComponent<AbstractTakesDamage>().TakeDamage(damage);
            }

            i++;
        }
        GetComponent<AudioSource>().PlayOneShot(deathSound);
        Instantiate(deathGFX, transform.position, Quaternion.identity);
    }

}
