using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_AutomaticDoor : MonoBehaviour
{
    private bool isOpen;

    private bool doorActivated;
    
    private DoorState doorState;

    private enum DoorState {
        Closed,
        Open,
        Opening,
        Closing,
        WaitingForCooldown
    }


    private bool userInBounds;
    private bool soundPlayed;
    private int cooldownTime;

    [SerializeField]
    private AudioClip doorOpen;
    [SerializeField]
    private AudioClip doorClose;
    [SerializeField]
    private float distanceToLowerDoor;

    private Vector3 _closedLocation;
    private Vector3 _openLocation;

    private void Start()
    {
        _closedLocation = transform.position;
        _openLocation = new Vector3(transform.position.x, transform.position.y + distanceToLowerDoor, transform.position.z);

        soundPlayed = false;
        doorState = DoorState.Closed;
        isOpen = false;
        cooldownTime = 0;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<A_TakesDamage>())
        {
            Debug.Log("User in bounds");
            userInBounds = true;
        }
        
    }

    /// <summary>
    /// Should be cleaned up. Add a counter to number of people in trigger, when at 0 consider it time to close it.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<A_TakesDamage>())
        {
            userInBounds = false;
        }
    }
    private void Update()
    {
        //check which state we're in
        switch (doorState)
        {
            case DoorState.Closed:
                if (userInBounds)
                {
                    doorState = DoorState.Opening;
                }
                break;
            case DoorState.Open:
                break;
            case DoorState.Opening:
                openDoor();
                break;
            case DoorState.Closing:
                closeDoor();
                break;
            case DoorState.WaitingForCooldown:
                break;
            default:
                break;
        }     
    }

    private void CooldownOver()
    {
        Debug.Log("Cooldown over.");
        doorState = DoorState.Closing;
    } 

    private void openDoor()
    {
        PlaySound();

        transform.position = Vector3.MoveTowards(transform.position, _openLocation, 3f * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, _openLocation) < 0.1f)
        {
            Debug.Log("openDoor()");
            soundPlayed = false;
            //cooldown
            Invoke("CooldownOver", 3f);
            doorState = DoorState.WaitingForCooldown;
        }
    }

    private void closeDoor()
    {
        PlaySound();

        transform.position = Vector3.MoveTowards(transform.position, _closedLocation, 3f * Time.deltaTime);

        if (Vector3.Distance(transform.position, _closedLocation) < 0.1f)
        {
            doorState = DoorState.Closed;
            Debug.Log("closeDoor()");
            soundPlayed = false;
        }
    }

    private void PlaySound()
    {
        if (!soundPlayed && !GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().PlayOneShot(doorClose);
            soundPlayed = true;
        }
    }
}
