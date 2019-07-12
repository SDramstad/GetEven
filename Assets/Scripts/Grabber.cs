using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour {

    public bool isHoldingRigidBody = false;

    public Vector3 holdedRigidScale;

    public float holdedRigidMass = 0;

    public GameObject holdingRigidObject;

    public WeaponSwitcher PlayerWeaponSwitcher;
    public GameObject previouslyEquippedWeapon;

    void Start()
    {
        PlayerWeaponSwitcher = GameObject.Find("Player").GetComponent<WeaponSwitcher>();
    }

    void Update()
    {
        PhysicPickUp();
        FixHands();
    }

    /// <summary>
    /// Check if hands are empty, but the grabber system is still occupied. 
    /// This can happen from a grenade exploding in your hand, that kind of thing.
    /// </summary>
    private void FixHands()
    {
        if (isHoldingRigidBody && holdingRigidObject == null)
        {
            isHoldingRigidBody = false;
        }
    }

    public void PhysicPickUp()
    {

        if (Input.GetButtonDown("Interact") && !isHoldingRigidBody)
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {

                holdingRigidObject = hit.collider.gameObject;

                // Do stuff. Like instantiate an object at hit.point
                if (holdingRigidObject.GetComponent<Renderer>())
                {
                    if (Vector3.Distance(this.gameObject.transform.position, holdingRigidObject.transform.position) < 3f &&
                        holdingRigidObject.GetComponent<Renderer>().isVisible == true &&
                        holdingRigidObject.GetComponent<Rigidbody>() != null && 
                        holdingRigidObject.GetComponent<Rigidbody>().mass <= 3)
                    {
                        //needs to disarm the player when picking up an object
                        //remember the weapon we had equipped
                        previouslyEquippedWeapon = PlayerWeaponSwitcher.GetActiveWeapon();
                        PlayerWeaponSwitcher.loadWeapon(PlayerWeaponSwitcher.unarmed);

                        //handle grabbing 

                        holdedRigidScale = holdingRigidObject.transform.localScale;

                        holdingRigidObject.transform.parent = this.gameObject.transform;

                        holdedRigidMass = holdingRigidObject.GetComponent<Rigidbody>().mass;

                        Destroy(holdingRigidObject.GetComponent<Rigidbody>());

                        isHoldingRigidBody = true;

                        //if active grenade, put a pin in it
                        if (holdingRigidObject.GetComponent<Grenade_Frag>())
                        {
                            holdingRigidObject.GetComponent<Grenade_Frag>().StopCountdown();
                        }

                    }
                }
                else
                {
                }
            }

        }
        else if (Input.GetButtonDown("Interact") && isHoldingRigidBody)
        {
            PhysicThrow();
            StartCoroutine(RevertWeapon());
        }
        else if (Input.GetMouseButton(0) && isHoldingRigidBody)
        {
            PhysicThrow(15);
            StartCoroutine(RevertWeapon());
        }

    }

    //public void GrenadeGrab(GameObject grenade)
    //{
    //    holdingRigidObject = grenade;

    //    // Do stuff. Like instantiate an object at hit.point
    //    if (holdingRigidObject.GetComponent<Renderer>())
    //    {
    //        if (Vector3.Distance(this.gameObject.transform.position, holdingRigidObject.transform.position) < 3f &&
    //            holdingRigidObject.GetComponent<Renderer>().isVisible == true &&
    //            holdingRigidObject.GetComponent<Rigidbody>() != null &&
    //            holdingRigidObject.GetComponent<Rigidbody>().mass <= 3)
    //        {
    //            //needs to disarm the player when picking up an object
    //            //remember the weapon we had equipped
    //            previouslyEquippedWeapon = PlayerWeaponSwitcher.GetActiveWeapon();
    //            PlayerWeaponSwitcher.loadWeapon(PlayerWeaponSwitcher.unarmed);

    //            //handle grabbing 

    //            holdedRigidScale = holdingRigidObject.transform.localScale;

    //            holdingRigidObject.transform.parent = this.gameObject.transform;

    //            holdedRigidMass = holdingRigidObject.GetComponent<Rigidbody>().mass;

    //            Destroy(holdingRigidObject.GetComponent<Rigidbody>());

    //            isHoldingRigidBody = true;

    //        }
    //    }
    //}

    IEnumerator RevertWeapon()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerWeaponSwitcher.loadWeapon(previouslyEquippedWeapon);

    }

    private void PhysicThrow(float force = 0)
    {

        holdingRigidObject.transform.parent = null;

        holdingRigidObject.AddComponent<Rigidbody>();

        holdingRigidObject.GetComponent<Rigidbody>().mass = holdedRigidMass;

        holdingRigidObject.transform.localScale = holdedRigidScale;

        if (force != 0)
            holdingRigidObject.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * force, ForceMode.Impulse);

        isHoldingRigidBody = false;

    }
}
