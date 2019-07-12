using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1M0_SlidingDoor : MonoBehaviour
{

    //private int state;
    //open/closed
    private bool open;

    private bool buttonActivated;
    private bool playerInBounds;
    private bool soundPlayed;

    [SerializeField]
    private AudioClip doorOpen;
    [SerializeField]
    private AudioClip doorClose;

    //private Quaternion EndRot1, EndRot2;
    private Vector3 _closedLocation;
    private Vector3 _openLocation;

    private void Start()
    {
        //EndRot1 = Quaternion.Euler(0, 90, 0); // Could be also set from the Inspector
        //EndRot2 = Quaternion.identity;
        //EndSlide1 = new Vector3(ident)
        _closedLocation = transform.position;
        _openLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z + 3);

        soundPlayed = false;
        buttonActivated = false;
        open = false;
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            playerInBounds = true;
        }
        else if (other.gameObject.GetComponent<Soldier>())
        {
            if(!open)
            {
                openDoor();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            playerInBounds = false;
        }
    }
    private void Update()
    {
        //check which state we're in

        //if player is in trigger, proceed, else dont
        if (playerInBounds)
        {            
            //otherwise we can take input
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Door press accepted.");
                buttonActivated = true;
            }
        }

        if (buttonActivated)
        {
            if (open)
            {
                Debug.Log("Closing door.");
                closeDoor();
            }
            else
            {
                Debug.Log("Opening door.");
                openDoor();
            }
        }




    }

    private void openDoor()
    {
        Debug.Log("openDoor()");
        //open it
        if (!soundPlayed)
        {
            GetComponent<AudioSource>().PlayOneShot(doorOpen);
            soundPlayed = true;
        }
        transform.position = Vector3.MoveTowards(transform.position, _openLocation, 3f * Time.deltaTime);
        
        //Debug.Log("We moving.");
        //if opened, end movement
        if (Vector3.Distance(transform.position, _openLocation) < 0.1f)
        {
            Debug.Log("Button activated is " + buttonActivated);
            open = true;
            buttonActivated = false;
            soundPlayed = false;
        }
    }

    private void closeDoor()
    {
        Debug.Log("closeDoor()");
        //close it
        if (!soundPlayed)
        {
            GetComponent<AudioSource>().PlayOneShot(doorClose);
            soundPlayed = true;
        }
        transform.position = Vector3.MoveTowards(transform.position, _closedLocation, 3f * Time.deltaTime);
        //Debug.Log("We moving.");
        //if closed, end movement
        if (Vector3.Distance(transform.position, _closedLocation) < 0.1f)
        {
            Debug.Log("Button activated is" + buttonActivated);
            open = false;
            buttonActivated = false;
            soundPlayed = false;
        }
    }
}