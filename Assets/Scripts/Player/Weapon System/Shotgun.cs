using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{

    [SerializeField]
    private int pellets = 6;
    [SerializeField]
    private GameObject _projectile;

    //value used for shotgun pellet dispersion
    [SerializeField]
    private float dispersionValue = 0.06f;

    private int damageAdjusted;



    protected override void Start()
    {
        base.Start();
        damageAdjusted = damage / (pellets / 2);
        damage = damageAdjusted;
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButton(0) && (Time.time - lastFireTime) > fireRate && !GlobalGame.pauseMenuActive)
        {
            lastFireTime = Time.time;
            GetComponent<Animator>().Play("Shotgun_Attack");
            Attack();
        }
    }

    protected override void Attack()
    {
        if (ammo.HasAmmo(tag))
        {
            //play the weapon's attack sound
            GetComponent<AudioSource>().PlayOneShot(attackSound);

            // MULTI-PELLET SYSTEM FOR SHOTGUN

            //testing new pellet system

            //build an array of rays 
            Ray[] rays = new Ray[pellets];
            for (int i = 0; i < pellets; i++)
            {
                //generate a random spread for the hitscan shotgun blast
                float randx = UnityEngine.Random.Range(-dispersionValue, dispersionValue);
                float randy = UnityEngine.Random.Range(-dispersionValue, dispersionValue);
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f + randx, 0.5f + randy, 0));
                rays[i] = ray;
            }

            // for each ray, handle hits
            foreach (var ray in rays)
            {

                //test instantiate pellets
                //RESULTS: lookAt does not work well, player projectiles also may need to check to make sure they dont hurt the player
                //otherwise you can hit your bullets while falling and die
                //Ray centerRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                //RaycastHit hitDirection;

                //if (Physics.Raycast(ray, out hitDirection))
                //{
                //}

                //exit point needs to be calibrated to fit gear
                Vector3 currentBulletPoint = bulletExitPoint.transform.position + 
                    new Vector3(UnityEngine.Random.Range(-0.3f,0.3f), UnityEngine.Random.Range(-0.3f, 0.3f), UnityEngine.Random.Range(-0.3f, 0.3f)); 
                Quaternion currentBulletRotation = bulletExitPoint.transform.rotation;
                Instantiate(_projectile, currentBulletPoint,
                    currentBulletRotation);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, range))
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red, 5f);
                    
                    if (hit.rigidbody)
                    {
                        hit.rigidbody.AddForceAtPosition(force * transform.forward, hit.point);
                    }

                    GameObject _tempParticleSystem = Instantiate(hitFX, hit.point, transform.rotation);

                    Destroy(_tempParticleSystem, 2f);

                    processHit(hit.collider.gameObject, hit);
                }

            }

            //Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            //RaycastHit hit;

            ////Debug.DrawRay(transform.position, Vector3.forward * 10, Color.red, 5f);
            //if (Physics.Raycast(ray, out hit, range))
            //{
            //    //If we create a particle object prefab, we can instantiate it here
            //    Debug.DrawLine(transform.position, hit.point, Color.red, 5f);
            //    if (hit.rigidbody)
            //    {
            //        hit.rigidbody.AddForceAtPosition(force * transform.forward, hit.point);
            //    }
            //    GameObject _tempParticleSystem = Instantiate(hitFX, hit.point, transform.rotation);
            //    Destroy(_tempParticleSystem, 2f);
            //    processHit(hit.collider.gameObject, hit);
            //}

            //also perform recoil
            //Camera.main.transform.Rotate(recoil * Time.deltaTime, 0, 0);
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(dry_attackSound);
        }
    }
}
