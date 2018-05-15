using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed;
    public int damage;
    public Vector3 targetDirection;
    //public GameObject target;

	// Use this for initialization
	void Start () {
        //transform.LookAt(target.transform.position);
        StartCoroutine("DeathTimer");
        //transform.LookAt(targetDirection);
    }
	
	// Update is called once per frame
	void Update () {
        //Vector3.MoveTowards(transform.position, Vector3.forward, 3f * Time.deltaTime);
        transform.position += transform.forward * Time.deltaTime * speed;
        //transform.Translate(targetDirection * speed * Time.deltaTime);
        //transform.Translate(speed, speed, 0, Space.Self);
    }


    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<AbstractTakesDamage>() != null)
        {
            collider.gameObject.GetComponent<AbstractTakesDamage>().TakeDamage(damage);            
        }        
        Destroy(gameObject);

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
