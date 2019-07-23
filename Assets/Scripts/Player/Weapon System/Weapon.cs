using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float fireRate;
    public float baseFireRate;
    public AudioClip attackSound;
    public AudioClip dry_attackSound;
    public GameObject hitFX;
    protected float lastFireTime;
    float zoomFactor = 2f;
    float zoomSpeed = 2f;
    float zoomFOV;
    public int range;
    public int damage;
    protected Ammo ammo;
    public TextMesh ammoDisplay;
    public UIManager uiManager;
    private GameObject crosshair_notarget;
    private GameObject crosshair_target;
    private GameObject crosshair_shotgun_notarget;
    private GameObject crosshair_shotgun_target;
    public GameObject muzzleFlashGFX;
    public GameObject bulletExitPoint;

    //the amount of push to the camera when firing
    public int recoil;
    //original camera position to always return to
    public Quaternion originalCameraPos;
    
    //force of weapon's push
    public int force;
    

    void Awake()
    {
        baseFireRate = fireRate;
    }
	// Use this for initialization
	protected virtual void Start () {
        zoomFOV = Constants.CameraDefaultZoom / zoomFactor;
        //Depreciated, ammo is stored within localPlayerData now
        //ammo = GameObject.Find("AmmoManager").GetComponent<Ammo>();
        ammo = GameObject.Find("Player").GetComponent<Ammo>();
        if (ammo == null)
        {
            Debug.Log("Ammo is null.");
        }
        lastFireTime = Time.time - 10;
        originalCameraPos = Camera.main.transform.rotation;
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }
	
	// Update is called once per frame
	protected virtual void Update () {

        //scan if target is in range and hurtable
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            if (hit.collider.gameObject.GetComponent<A_TakesDamage>())
            {
                uiManager.CrosshairTargetInSight(true);
            }
            else
            {
                uiManager.CrosshairTargetInSight(false);
            }
        }
        else
        {
            uiManager.CrosshairTargetInSight(false);
        }

        //for weapons with ammo, do this (may want to turn into an abstract class honestly)
        if (GetComponent<Pistol>() || GetComponent<Shotgun>() || GetComponent<Rifle>() || GetComponent<Rocketgun>())
        {
            GetComponentInChildren<TextMesh>().text = ammo.GetAmmo(tag).ToString();
        }
        


        if (Input.GetMouseButton(1))
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomFOV, zoomSpeed * Time.deltaTime);

        }
        else
        {
            Camera.main.fieldOfView = Constants.CameraDefaultZoom;
        }
    }

    protected virtual void Attack()
    {
        if (ammo.HasAmmo(tag))
        {
            //play the weapon's attack sound
            GetComponent<AudioSource>().PlayOneShot(attackSound);

            //if weapon is not knife, use ammo
            //also display muzzle flash
            if (!GetComponent<Knife>())
            {
                ammo.ConsumeAmmo(tag);
                GameObject tmp = Instantiate(muzzleFlashGFX, bulletExitPoint.transform.position, bulletExitPoint.transform.rotation);
                Destroy(tmp, 2f);
            }

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            //Debug.DrawRay(transform.position, Vector3.forward * 10, Color.red, 5f);
            if (Physics.Raycast(ray, out hit, range))
            {
                //If we create a particle object prefab, we can instantiate it here
                Debug.DrawLine(transform.position, hit.point, Color.red, 5f);
                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForceAtPosition(force * transform.forward, hit.point);
                }
                GameObject _tempParticleSystem = Instantiate(hitFX, hit.point, transform.rotation);
                Destroy(_tempParticleSystem, 2f);
                processHit(hit.collider.gameObject, hit);
            }

            //also perform recoil
            //Camera.main.transform.Rotate(recoil * Time.deltaTime, 0, 0);
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(dry_attackSound);
        }
    }


    protected void processHit(GameObject hitObject, RaycastHit location)
    {
        Debug.Log(hitObject.name + " was hit.");
        
        if (hitObject.GetComponent<A_TakesDamage>() != null)
        {
            hitObject.GetComponent<A_TakesDamage>().TakeDamage(damage);
            ParticleSystem _tempParticleSystem = Instantiate(hitObject.GetComponent<A_TakesDamage>().painGFX, location.point, Quaternion.identity);
            Destroy(_tempParticleSystem, 2f);
        }

    }
}
