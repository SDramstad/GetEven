using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocketgun : Weapon
{

    [SerializeField]
    private GameObject rocketProjectile;

    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButton(0) && (Time.time - lastFireTime) > fireRate && !GlobalGame.pauseMenuActive)
        {
            lastFireTime = Time.time;
            Fire();
        }
    }

    /*
     * Unique firing method for projectile launchers only.
     */
    private void Fire()
    {
        GetComponent<AudioSource>().PlayOneShot(attackSound);

        ammo.ConsumeAmmo(tag);

        GameObject tmp = Instantiate(muzzleFlashGFX, bulletExitPoint.transform.position, bulletExitPoint.transform.rotation);

        Destroy(tmp, 2f);

        GameObject tmp2 = Instantiate(rocketProjectile, bulletExitPoint.transform.position, bulletExitPoint.transform.rotation);

        //if (ammo.HasAmmo(tag))
        //{
        //    //play the weapon's attack sound
        //    GetComponent<AudioSource>().PlayOneShot(attackSound);

        //    ammo.ConsumeAmmo(tag);

        //    GameObject tmp = Instantiate(muzzleFlashGFX, bulletExitPoint.transform.position, bulletExitPoint.transform.rotation);

        //    Destroy(tmp, 2f);

        //    GameObject tmp2 = Instantiate(rocketProjectile, bulletExitPoint.transform.position, bulletExitPoint.transform.rotation);


        //    //Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        //    //RaycastHit hit;

        //    ////create a hit location
        //    //if (Physics.Raycast(ray, out hit))
        //    //{
        //    //    //bulletExitPoint.transform.LookAt(hit.transform);
        //    //    GameObject tmp2 = Instantiate(rocketProjectile, bulletExitPoint.transform.position, bulletExitPoint.transform.rotation);

        //    //    ////create rocket
        //    //    //GameObject rocket = Instantiate(rocketProjectile);
        //    //    //rocket.transform.position = bulletExitPoint.transform.position;

        //    //    ////lead the projectile to the target
        //    //    //Vector3 direction = rocket.transform.position - hit.transform.position;
        //    //    ////Ray ray2 = new Ray(transform.position, direction);
        //    //    //rocket.transform.LookAt(direction);
        //    //    ////RaycastHit hit2;

        //    //}

        //    //    //also perform recoil
        //    //    //Camera.main.transform.Rotate(recoil * Time.deltaTime, 0, 0);

        //    //    //GameObject projectile = Instantiate(rocketProjectile, bulletExitPoint.transform.position, bulletExitPoint.transform.rotation);
        //    //
        //}
        //else
        //{
        //    GetComponent<AudioSource>().PlayOneShot(dry_attackSound);
        //}
    }
}
