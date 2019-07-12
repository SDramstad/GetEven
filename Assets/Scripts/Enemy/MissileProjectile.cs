using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileProjectile : MonoBehaviour
{
    public int damage;
    public int speed;
    public float range;
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
        yield return new WaitForSeconds(20);
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
        Collider[] inRadiusList = Physics.OverlapSphere(transform.position, range);

        int i = 0;

        while (i < inRadiusList.Length)
        {
            Rigidbody rb = inRadiusList[i].GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(500.0f, transform.position, 15f, 5f);
            }
            
            if (inRadiusList[i].GetComponent<A_TakesDamage>())
            {
                //raycast between the two points to check if the explosion can hit the target
                Vector3 explosionCenterLoc = transform.position;
                Vector3 targetLoc = inRadiusList[i].transform.position;
                //get the direction between the explosion's center and the current target's center
                Vector3 direction = inRadiusList[i].transform.position - transform.position;
                //create a ray going from explosion center to current target's center
                Ray ray = new Ray(transform.position, direction);
                Debug.DrawRay(transform.position, direction, Color.red);
                RaycastHit hit;

                //does the raycast
                if (Physics.Raycast(ray, out hit))
                {
                    //if the ray hits and the object hit has a component of class AbstractTakesDamage
                    if (hit.collider.gameObject.GetComponent<A_TakesDamage>() != null)
                    {
                        hit.collider.GetComponent<A_TakesDamage>().TakeDamage(damage);
                    }
                }
                


            }

            i++;
        }
        GetComponent<AudioSource>().PlayOneShot(deathSound);
        Instantiate(deathGFX, transform.position, Quaternion.identity);
    }

}
