using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionShit : MonoBehaviour {
    
    private int count;
    private bool invokeIsRunning;
    private bool playerInTrigger;
    private GameObject target;

    void Start()
    {
        invokeIsRunning = false;
        playerInTrigger = false;
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(invokeIsRunning + " invokeIsRunning");
        if (other.gameObject.name == "Player" && invokeIsRunning == false)
        {
            //InvokeRepeating("countUp", 0f, 1f);
            Debug.Log("Player in trigger.");
            playerInTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            invokeIsRunning = false;
            playerInTrigger = false;
            CancelInvoke();
            count = 0;
            Debug.Log("Left trigger.");
        }
    }

    void Update()
    {
        target = GameObject.Find("Player");

        if (playerInTrigger)
        {
            Vector3 direction = target.transform.position - transform.position;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.collider.gameObject.name + " was hit.");

                if (hit.collider.gameObject.GetComponent<Player>() != null && !invokeIsRunning)
                {
                    InvokeRepeating("countUp", 0f, 1f);
                }
            }
        }
    }

    void countUp()
    {
        invokeIsRunning = true;
        count++;
		Debug.Log("Counting " + count);
    }
}

