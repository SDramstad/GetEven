using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{

    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButton(0) && (Time.time - lastFireTime) > fireRate && !GlobalGame.pauseMenuActive)
        {
            lastFireTime = Time.time;
            GetComponent<Animator>().Play("Rifle_Attack");
            Attack();
        }
    }
}
