using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyPad : MonoBehaviour
{
    public int speed;

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.GetComponent<Rigidbody>())
    //    {
    //        other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * speed);
    //    }
    //}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            Debug.Log("Player entered collider.");           
            other.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(500 * transform.forward, transform.position);            
        }
    }

}
